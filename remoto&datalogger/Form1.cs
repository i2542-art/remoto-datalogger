using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace fan
{
    public partial class Form1 : Form
    {
        // --- 排他制御（ロック）用オブジェクト ---
        private readonly object serialLock = new object();

        // --- 内部変数 ---
        private Timer fanAnimationTimer;
        private Timer rhythmTimer;
        private int currentImageIndex = 0;
        private Image[] fanImages;

        private DateTime lastLogTime = DateTime.Now;
        private int logCount = 0;

        private int rhythmStep = 0;
        private int currentRhythmOutputLevel = 0; // リズム風実行中の実際の出力レベル(0:停止, 1:弱, 2:中, 3:強)
        private int timerRemainingSeconds = 0;

        private List<GraphDataPoint> graphHistory = new List<GraphDataPoint>();
        private ComboBox cmbGraphRange;

        private bool isPowerOn = false;
        private string windSpeed = "弱";
        private string timerSetting = "なし";
        private bool isRhythm = false;
        private bool isAuto = false;
        private double currentTemp = 25.0;

        public Form1()
        {
            InitializeComponent();
            InitializeFanSystem();
        }

        private void InitializeFanSystem()
        {
            fanAnimationTimer = new Timer();
            fanAnimationTimer.Interval = 300;
            fanAnimationTimer.Tick += FanAnimationTimer_Tick;

            rhythmTimer = new Timer();
            rhythmTimer.Interval = 2000;
            rhythmTimer.Tick += RhythmTimer_Tick;
            rhythmTimer.Start();

            // --- グラフ表示範囲切り替え ComboBox ---
            cmbGraphRange = new ComboBox();
            cmbGraphRange.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGraphRange.Size = new Size(100, 23);
            cmbGraphRange.Location = new Point(picGraphArea.Right - 110, picGraphArea.Top + 5);
            cmbGraphRange.Items.AddRange(new string[] { "直近10件", "直近30件", "直近60件", "全履歴" });
            cmbGraphRange.SelectedIndex = 2;
            cmbGraphRange.SelectedIndexChanged += (s, e) => DrawTrendGraph();
            this.Controls.Add(cmbGraphRange);
            cmbGraphRange.BringToFront();

            // --- 扇風機アニメーション画像の動的生成（GDI+ 3コマ） ---
            fanImages = new Image[3];
            for (int i = 0; i < 3; i++)
            {
                Bitmap bmp = new Bitmap(90, 95);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(Color.White);

                    int cx = 45, cy = 40;
                    int radius = 35;

                    using (SolidBrush baseBrush = new SolidBrush(Color.FromArgb(220, 225, 230)))
                    {
                        g.FillRectangle(baseBrush, cx - 3, cy + 10, 6, 35);
                        g.FillEllipse(baseBrush, cx - 18, cy + 38, 36, 10);
                    }
                    g.DrawEllipse(Pens.Silver, cx - 18, cy + 38, 36, 10);

                    using (Pen outerPen = new Pen(Color.FromArgb(180, 190, 200), 2))
                    {
                        g.DrawEllipse(outerPen, cx - radius, cy - radius, radius * 2, radius * 2);
                    }
                    using (Pen gridPen = new Pen(Color.FromArgb(220, 225, 230), 1))
                    {
                        for (int a = 0; a < 360; a += 45)
                        {
                            float rad = a * (float)Math.PI / 180f;
                            g.DrawLine(gridPen, cx, cy, cx + (float)(radius * Math.Cos(rad)), cy + (float)(radius * Math.Sin(rad)));
                        }
                    }

                    double angleOffset = i * (Math.PI / 4.0);
                    using (SolidBrush bladeBrush = new SolidBrush(Color.FromArgb(170, 100, 180, 245)))
                    {
                        for (int b = 0; b < 3; b++)
                        {
                            double bladeAngle = angleOffset + (b * 2.0 * Math.PI / 3.0);
                            int bx = cx + (int)(20 * Math.Cos(bladeAngle));
                            int by = cy + (int)(20 * Math.Sin(bladeAngle));

                            g.FillEllipse(bladeBrush, bx - 11, by - 11, 22, 22);
                        }
                    }

                    using (SolidBrush hubBrush = new SolidBrush(Color.FromArgb(240, 245, 250)))
                    {
                        g.FillEllipse(hubBrush, cx - 7, cy - 7, 14, 14);
                    }
                    g.DrawEllipse(Pens.LightGray, cx - 7, cy - 7, 14, 14);
                }
                fanImages[i] = bmp;
            }
            picFanAnimation.Image = fanImages[0];

            this.serialPort1.DataReceived += SerialPort1_DataReceived;
            this.timer1.Tick += Timer1_Tick;
            this.timer1.Start();

            this.btnPower.Click += (s, e) => SendRemoteCommand("POWER");
            this.btnWind.Click += (s, e) => SendRemoteCommand("WIND");
            this.btnTimer.Click += (s, e) => SendRemoteCommand("TIMER");
            this.btnRhythm.Click += (s, e) => SendRemoteCommand("RHYTHM");
            this.btnAuto.Click += (s, e) => SendRemoteCommand("AUTO");

            this.btnConnect.Click += BtnConnect_Click;
            this.btnDisconnect.Click += BtnDisconnect_Click;

            cmbPort.Items.AddRange(SerialPort.GetPortNames());
            if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;

            DrawTrendGraph();
            UpdateUIStatus();
        }

        // --------------------------------------------------
        // コマンド送信 ＆ 電源ON/OFFの即時ログ記録
        // --------------------------------------------------
        private void SendRemoteCommand(string commandType)
        {
            string cmd = "";
            switch (commandType)
            {
                case "POWER":
                    cmd = "CMD_POWER";
                    isPowerOn = !isPowerOn;

                    if (!isPowerOn)
                    {
                        RecordPowerEventLog("電源OFF");
                        isRhythm = false;
                        isAuto = false;
                        timerSetting = "なし";
                        timerRemainingSeconds = 0;
                    }
                    else
                    {
                        RecordPowerEventLog("電源ON");
                        lastLogTime = DateTime.Now;
                    }
                    break;

                case "WIND":
                    cmd = "CMD_WIND";
                    if (isPowerOn)
                    {
                        if (windSpeed == "弱") windSpeed = "中";
                        else if (windSpeed == "中") windSpeed = "強";
                        else windSpeed = "弱";

                        isAuto = false; // 自動運転は解除（リズムは維持）
                        rhythmStep = 0; // リズムのステップをリセット
                    }
                    break;

                case "TIMER":
                    cmd = "CMD_TIMER";
                    if (isPowerOn)
                    {
                        if (timerSetting == "なし")
                        {
                            timerSetting = "1h";
                            timerRemainingSeconds = 3600;
                        }
                        else if (timerSetting == "1h")
                        {
                            timerSetting = "2h";
                            timerRemainingSeconds = 7200;
                        }
                        else if (timerSetting == "2h")
                        {
                            timerSetting = "4h";
                            timerRemainingSeconds = 14400;
                        }
                        else
                        {
                            timerSetting = "なし";
                            timerRemainingSeconds = 0;
                        }
                    }
                    break;

                case "RHYTHM":
                    cmd = "CMD_RHYTHM";
                    if (isPowerOn)
                    {
                        isRhythm = !isRhythm;
                        if (isRhythm)
                        {
                            isAuto = false;
                            rhythmStep = 0;
                        }
                    }
                    break;

                case "AUTO":
                    cmd = "CMD_AUTO";
                    if (isPowerOn)
                    {
                        isAuto = !isAuto;
                        if (isAuto) isRhythm = false;
                    }
                    break;
            }

            if (serialPort1.IsOpen && !string.IsNullOrEmpty(cmd))
            {
                try { serialPort1.WriteLine(cmd); } catch { }
            }

            UpdateUIStatus();
        }

        // --------------------------------------------------
        // 電源ON/OFF 専用イベントログ記録
        // --------------------------------------------------
        private void RecordPowerEventLog(string opName)
        {
            if (!chkLoggerEnable.Checked) return;

            string timeStr = DateTime.Now.ToString("HH:mm:ss");
            string statusStr = isPowerOn ? (isAuto ? $"自動運転（{windSpeed}）" : (isRhythm ? $"{windSpeed}リズム" : windSpeed)) : "停止";

            if (logCount >= 1000) dgvLogHistory.Rows.Clear();
            dgvLogHistory.Rows.Insert(0, timeStr, opName, statusStr, $"{currentTemp:F1}");
            logCount++;

            SaveToCsvFile(timeStr, opName, statusStr, currentTemp);

            int currentWindLevel = 0;
            if (isPowerOn)
            {
                if (isRhythm) currentWindLevel = currentRhythmOutputLevel;
                else currentWindLevel = (windSpeed == "弱" ? 1 : (windSpeed == "中" ? 2 : 3));
            }

            graphHistory.Add(new GraphDataPoint
            {
                Time = DateTime.Now,
                Temperature = currentTemp,
                IsRhythm = isRhythm,
                WindLevel = currentWindLevel
            });
            DrawTrendGraph();
        }

        // --------------------------------------------------
        // リズム風制御タイマー（現在の設定風量を上限とし、0まで下降させる動作）
        // --------------------------------------------------
        private void RhythmTimer_Tick(object sender, EventArgs e)
        {
            if (isPowerOn && isRhythm)
            {
                rhythmStep = (rhythmStep + 1) % 4;

                if (windSpeed == "弱")
                {
                    // 「弱」リズム: 弱 → 停止 → 弱 → 停止
                    currentRhythmOutputLevel = (rhythmStep % 2 == 0) ? 1 : 0;
                }
                else if (windSpeed == "中")
                {
                    // 「中」リズム: 中 → 弱 → 停止 → 弱
                    switch (rhythmStep)
                    {
                        case 0: currentRhythmOutputLevel = 2; break; // 中
                        case 1: currentRhythmOutputLevel = 1; break; // 弱
                        case 2: currentRhythmOutputLevel = 0; break; // 停止
                        case 3: currentRhythmOutputLevel = 1; break; // 弱
                    }
                }
                else if (windSpeed == "強")
                {
                    // 「強」リズム: 強 → 中 → 弱 → 停止
                    switch (rhythmStep)
                    {
                        case 0: currentRhythmOutputLevel = 3; break; // 強
                        case 1: currentRhythmOutputLevel = 2; break; // 中
                        case 2: currentRhythmOutputLevel = 1; break; // 弱
                        case 3: currentRhythmOutputLevel = 0; break; // 停止
                    }
                }

                // 実際の出力レベルに合わせてアニメーション速度とアニメーションタイマーのON/OFFを切替
                switch (currentRhythmOutputLevel)
                {
                    case 3: // 強
                        fanAnimationTimer.Interval = 90;
                        if (!fanAnimationTimer.Enabled) fanAnimationTimer.Start();
                        break;
                    case 2: // 中
                        fanAnimationTimer.Interval = 180;
                        if (!fanAnimationTimer.Enabled) fanAnimationTimer.Start();
                        break;
                    case 1: // 弱
                        fanAnimationTimer.Interval = 300;
                        if (!fanAnimationTimer.Enabled) fanAnimationTimer.Start();
                        break;
                    case 0: // 停止（羽が一時停止）
                        fanAnimationTimer.Stop();
                        break;
                }

                UpdateUIStatus();
            }
        }

        // --------------------------------------------------
        // マイコン受信処理（排他制御 lock）
        // --------------------------------------------------
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string rawData = "";

                lock (serialLock)
                {
                    if (serialPort1.IsOpen && serialPort1.BytesToRead > 0)
                    {
                        rawData = serialPort1.ReadLine();
                    }
                }

                if (!string.IsNullOrEmpty(rawData))
                {
                    this.Invoke(new Action(() =>
                    {
                        UpdateUIStatus();
                    }));
                }
            }
            catch { }
        }

        // --------------------------------------------------
        // ランプ点灯 ＆ アニメーション状態更新
        // --------------------------------------------------
        private void UpdateUIStatus()
        {
            lblLampWindWeak.BackColor = Color.LightGray;
            lblLampWindMid.BackColor = Color.LightGray;
            lblLampWindStrong.BackColor = Color.LightGray;
            lblLampTimer1h.BackColor = Color.LightGray;
            lblLampTimer2h.BackColor = Color.LightGray;
            lblLampTimer4h.BackColor = Color.LightGray;
            lblLampRhythm.BackColor = Color.LightGray;
            lblLampAuto.BackColor = Color.LightGray;

            if (isPowerOn)
            {
                if (isAuto)
                {
                    if (currentTemp < 24.0) windSpeed = "弱";
                    else if (currentTemp < 28.0) windSpeed = "中";
                    else windSpeed = "強";
                }

                // 基本風量ランプ点灯（リズム中もベースの風量レベルを表示）
                if (windSpeed == "弱") lblLampWindWeak.BackColor = Color.Red;
                else if (windSpeed == "中") lblLampWindMid.BackColor = Color.Red;
                else if (windSpeed == "強") lblLampWindStrong.BackColor = Color.Red;

                if (timerSetting == "1h") lblLampTimer1h.BackColor = Color.Red;
                else if (timerSetting == "2h") lblLampTimer2h.BackColor = Color.Red;
                else if (timerSetting == "4h") lblLampTimer4h.BackColor = Color.Red;

                if (isRhythm) lblLampRhythm.BackColor = Color.Red;
                if (isAuto) lblLampAuto.BackColor = Color.Red;

                // リズムOFFの時のみ、通常のアニメーション制御を行う
                if (!isRhythm)
                {
                    if (windSpeed == "弱") fanAnimationTimer.Interval = 300;
                    else if (windSpeed == "中") fanAnimationTimer.Interval = 180;
                    else if (windSpeed == "強") fanAnimationTimer.Interval = 90;

                    if (!fanAnimationTimer.Enabled) fanAnimationTimer.Start();
                }
            }
            else
            {
                fanAnimationTimer.Stop();
            }
        }

        private void FanAnimationTimer_Tick(object sender, EventArgs e)
        {
            currentImageIndex = (currentImageIndex + 1) % 3;
            picFanAnimation.Image = fanImages[currentImageIndex];
        }

        // --------------------------------------------------
        // 1秒周期タイマー処理
        // --------------------------------------------------
        private void Timer1_Tick(object sender, EventArgs e)
        {
            // 1. タイマー自動停止カウントダウン
            if (isPowerOn && timerRemainingSeconds > 0)
            {
                timerRemainingSeconds--;

                if (timerRemainingSeconds > 7200) timerSetting = "4h";
                else if (timerRemainingSeconds > 3600) timerSetting = "2h";
                else if (timerRemainingSeconds > 0) timerSetting = "1h";

                if (timerRemainingSeconds <= 0)
                {
                    isPowerOn = false;
                    RecordPowerEventLog("タイマー満了停止");

                    timerSetting = "なし";
                    isRhythm = false;
                    isAuto = false;

                    if (serialPort1.IsOpen)
                    {
                        try { serialPort1.WriteLine("CMD_POWER"); } catch { }
                    }

                    UpdateUIStatus();
                }
            }

            // 2. 外気温シミュレーション
            if (isPowerOn && isAuto)
            {
                currentTemp += 0.5;
                if (currentTemp > 30.0) currentTemp = 22.0;
                UpdateUIStatus();
            }

            // 3. 1分周期ログ記録・グラフ更新
            CheckAndRunDataLogger();
        }

        // --------------------------------------------------
        // 定期ログ書き込み＆グラフ更新（電源ON時1分周期）
        // --------------------------------------------------
        private void CheckAndRunDataLogger()
        {
            if (!chkLoggerEnable.Checked || !isPowerOn) return;

            bool isOneMinutePassed = (DateTime.Now - lastLogTime).TotalSeconds >= 60;

            if (isOneMinutePassed)
            {
                string timeStr = DateTime.Now.ToString("HH:mm:ss");

                string statusStr = "";
                if (isAuto)
                {
                    statusStr = $"自動運転（{windSpeed}）";
                }
                else if (isRhythm)
                {
                    statusStr = $"{windSpeed}リズム";
                }
                else
                {
                    statusStr = windSpeed;
                }

                if (logCount >= 1000) dgvLogHistory.Rows.Clear();
                dgvLogHistory.Rows.Insert(0, timeStr, "定期サンプリング", statusStr, $"{currentTemp:F1}");
                logCount++;

                SaveToCsvFile(timeStr, "定期サンプリング", statusStr, currentTemp);

                int currentWindLevel = 0;
                if (isPowerOn)
                {
                    if (isRhythm) currentWindLevel = currentRhythmOutputLevel;
                    else currentWindLevel = (windSpeed == "弱" ? 1 : (windSpeed == "中" ? 2 : 3));
                }

                graphHistory.Add(new GraphDataPoint
                {
                    Time = DateTime.Now,
                    Temperature = currentTemp,
                    IsRhythm = isRhythm,
                    WindLevel = currentWindLevel
                });

                DrawTrendGraph();

                lastLogTime = DateTime.Now;
            }
        }

        // --------------------------------------------------
        // CSV保存
        // --------------------------------------------------
        private void SaveToCsvFile(string timeStr, string op, string status, double temp)
        {
            try
            {
                string dirPath = Path.Combine(Application.StartupPath, "Logs");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                string fileName = DateTime.Now.ToString("yyyyMMdd") + ".csv";
                string filePath = Path.Combine(dirPath, fileName);

                bool isNewFile = !File.Exists(filePath);

                using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
                {
                    if (isNewFile)
                    {
                        sw.WriteLine("時分秒,操作,制御状態,気温(℃)");
                    }
                    sw.WriteLine($"{timeStr},{op},{status},{temp:F1}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CSV保存エラー: " + ex.Message);
            }
        }

        // --------------------------------------------------
        // CSV手動エクスポート
        // --------------------------------------------------
        public void ExportDgvToCsv()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSVファイル(*.csv)|*.csv";
                sfd.FileName = $"FanLog_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            sw.WriteLine("時分秒,操作,制御状態,気温(℃)");
                            foreach (DataGridViewRow row in dgvLogHistory.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    sw.WriteLine($"{row.Cells[0].Value},{row.Cells[1].Value},{row.Cells[2].Value},{row.Cells[3].Value}");
                                }
                            }
                        }
                        MessageBox.Show("CSVファイルを書き出しました。", "エクスポート完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("書き出し失敗: " + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // --------------------------------------------------
        // トレンドグラフ描画（風量目盛り間隔均等化版）
        // --------------------------------------------------
        private void DrawTrendGraph()
        {
            int w = picGraphArea.Width;
            int h = picGraphArea.Height;
            if (w <= 0 || h <= 0) return;

            Bitmap bmp = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.White);

                Pen gridPen = new Pen(Color.FromArgb(235, 235, 235), 1);
                Pen dashPen = new Pen(Color.FromArgb(220, 220, 220), 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                Font font = new Font("MS UI Gothic", 8.0f);
                Font labelFont = new Font("MS UI Gothic", 8.5f, FontStyle.Bold);

                int sectionH = h / 3;
                int leftMargin = 85;

                // セパレート線描画
                g.DrawLine(Pens.LightGray, leftMargin, 0, leftMargin, h);
                g.DrawLine(gridPen, 0, sectionH, w, sectionH);
                g.DrawLine(gridPen, 0, sectionH * 2, w, sectionH * 2);

                using (Brush labelBrush = new SolidBrush(Color.DarkSlateGray))
                using (Brush subLabelBrush = new SolidBrush(Color.Gray))
                {
                    // 1. 室温エリアの目盛り
                    g.DrawString("室温[℃]", labelFont, labelBrush, 2, 5);
                    g.DrawString("30℃ -", font, subLabelBrush, 52, 6);
                    g.DrawString("25℃ -", font, subLabelBrush, 52, sectionH / 2 - 5);
                    g.DrawString("20℃ -", font, subLabelBrush, 52, sectionH - 16);
                    g.DrawLine(dashPen, leftMargin, sectionH / 2, w, sectionH / 2);

                    // 2. リズム風エリアの目盛り
                    g.DrawString("リズム", labelFont, labelBrush, 2, sectionH + 5);
                    g.DrawString("ON -", font, subLabelBrush, 58, sectionH + 8);
                    g.DrawString("OFF -", font, subLabelBrush, 52, sectionH * 2 - 16);

                    // 3. 風量エリアの目盛り（完全均等配置計算）
                    g.DrawString("風量", labelFont, labelBrush, 2, sectionH * 2 + 5);

                    float yWindTop = sectionH * 2 + 10;
                    float yWindBottom = sectionH * 3 - 10;
                    float yWindHeight = yWindBottom - yWindTop;

                    float yStrong = yWindTop;                      // 強
                    float yMid = yWindTop + yWindHeight * 0.33f;  // 中
                    float yWeak = yWindTop + yWindHeight * 0.66f; // 弱
                    float yStop = yWindBottom;                    // 停

                    g.DrawString("強 -", font, subLabelBrush, 62, yStrong - 5);
                    g.DrawString("中 -", font, subLabelBrush, 62, yMid - 5);
                    g.DrawString("弱 -", font, subLabelBrush, 62, yWeak - 5);
                    g.DrawString("停 -", font, subLabelBrush, 62, yStop - 5);

                    g.DrawLine(dashPen, leftMargin, yStrong, w, yStrong);
                    g.DrawLine(dashPen, leftMargin, yMid, w, yMid);
                    g.DrawLine(dashPen, leftMargin, yWeak, w, yWeak);
                }

                List<GraphDataPoint> displayData = new List<GraphDataPoint>();
                int rangeCount = 60;

                if (cmbGraphRange != null && cmbGraphRange.SelectedItem != null)
                {
                    string selected = cmbGraphRange.SelectedItem.ToString();
                    if (selected == "直近10件") rangeCount = 10;
                    else if (selected == "直近30件") rangeCount = 30;
                    else if (selected == "直近60件") rangeCount = 60;
                    else if (selected == "全履歴") rangeCount = graphHistory.Count;
                }

                if (graphHistory.Count > 0)
                {
                    int startIndex = Math.Max(0, graphHistory.Count - rangeCount);
                    displayData = graphHistory.GetRange(startIndex, graphHistory.Count - startIndex);
                }

                if (displayData.Count > 1)
                {
                    int maxSlots = Math.Max(2, rangeCount);
                    float xStep = (float)(w - leftMargin - 10) / (float)(maxSlots - 1);

                    // 風量Y位置取得ヘルパー関数
                    float GetWindYPosition(int level)
                    {
                        float yWindTop = sectionH * 2 + 10;
                        float yWindBottom = sectionH * 3 - 10;
                        float yWindHeight = yWindBottom - yWindTop;

                        if (level == 3) return yWindTop;                      // 強
                        if (level == 2) return yWindTop + yWindHeight * 0.33f; // 中
                        if (level == 1) return yWindTop + yWindHeight * 0.66f; // 弱
                        return yWindBottom;                                   // 停
                    }

                    for (int i = 0; i < displayData.Count - 1; i++)
                    {
                        float x1 = leftMargin + i * xStep;
                        float x2 = leftMargin + (i + 1) * xStep;

                        // 1. 室温ライン
                        float yTemp1 = sectionH - 6 - (float)((displayData[i].Temperature - 15.0) / 15.0 * (sectionH - 18));
                        float yTemp2 = sectionH - 6 - (float)((displayData[i + 1].Temperature - 15.0) / 15.0 * (sectionH - 18));
                        using (Pen tempPen = new Pen(Color.IndianRed, 2))
                        {
                            g.DrawLine(tempPen, x1, Math.Max(5, yTemp1), x2, Math.Max(5, yTemp2));
                        }

                        // 2. リズム風ライン
                        float yRhythm1 = displayData[i].IsRhythm ? (sectionH + 12) : (sectionH * 2 - 12);
                        float yRhythm2 = displayData[i + 1].IsRhythm ? (sectionH + 12) : (sectionH * 2 - 12);
                        using (Pen rhythmPen = new Pen(Color.RoyalBlue, 1.5f))
                        {
                            g.DrawLine(rhythmPen, x1, yRhythm1, x2, yRhythm1);
                            g.DrawLine(rhythmPen, x2, yRhythm1, x2, yRhythm2);
                        }

                        // 3. 風量ライン
                        float yWind1 = GetWindYPosition(displayData[i].WindLevel);
                        float yWind2 = GetWindYPosition(displayData[i + 1].WindLevel);
                        using (Pen windPen = new Pen(Color.MediumSeaGreen, 1.5f))
                        {
                            g.DrawLine(windPen, x1, yWind1, x2, yWind1);
                            g.DrawLine(windPen, x2, yWind1, x2, yWind2);
                        }
                    }
                }
            }

            picGraphArea.Image?.Dispose();
            picGraphArea.Image = bmp;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPort.SelectedItem != null)
                {
                    serialPort1.PortName = cmbPort.SelectedItem.ToString();
                    serialPort1.Open();
                    MessageBox.Show("マイコンと接続しました。");
                }
            }
            catch (Exception ex) { MessageBox.Show("接続失敗: " + ex.Message); }
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            lock (serialLock)
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    MessageBox.Show("切断しました。");
                }
            }
        }
    }

    public class GraphDataPoint
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
        public bool IsRhythm { get; set; }
        public int WindLevel { get; set; }
    }
}