namespace fan
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grpRemote = new System.Windows.Forms.GroupBox();
            this.picFanAnimation = new System.Windows.Forms.PictureBox();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnWind = new System.Windows.Forms.Button();
            this.btnTimer = new System.Windows.Forms.Button();
            this.btnRhythm = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.Button();
            this.lblLampWindWeak = new System.Windows.Forms.Label();
            this.lblLampWindMid = new System.Windows.Forms.Label();
            this.lblLampWindStrong = new System.Windows.Forms.Label();
            this.lblLampTimer1h = new System.Windows.Forms.Label();
            this.lblLampTimer2h = new System.Windows.Forms.Label();
            this.lblLampTimer4h = new System.Windows.Forms.Label();
            this.lblLampRhythm = new System.Windows.Forms.Label();
            this.lblLampAuto = new System.Windows.Forms.Label();
            this.grpGraph = new System.Windows.Forms.GroupBox();
            this.picGraphArea = new System.Windows.Forms.PictureBox();
            this.grpLogTable = new System.Windows.Forms.GroupBox();
            this.dgvLogHistory = new System.Windows.Forms.DataGridView();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpPort = new System.Windows.Forms.GroupBox();
            this.cmbPort = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.chkLoggerEnable = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.grpRemote.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFanAnimation)).BeginInit();
            this.grpGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picGraphArea)).BeginInit();
            this.grpLogTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogHistory)).BeginInit();
            this.grpPort.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpPort
            // 
            this.grpPort.Controls.Add(this.cmbPort);
            this.grpPort.Controls.Add(this.btnConnect);
            this.grpPort.Controls.Add(this.btnDisconnect);
            this.grpPort.Controls.Add(this.chkLoggerEnable);
            this.grpPort.Location = new System.Drawing.Point(12, 5);
            this.grpPort.Name = "grpPort";
            this.grpPort.Size = new System.Drawing.Size(560, 45);
            this.grpPort.TabIndex = 0;
            this.grpPort.TabStop = false;
            this.grpPort.Text = "通信設定・データロガーSW";
            // 
            // cmbPort
            // 
            this.cmbPort.FormattingEnabled = true;
            this.cmbPort.Location = new System.Drawing.Point(10, 16);
            this.cmbPort.Name = "cmbPort";
            this.cmbPort.Size = new System.Drawing.Size(80, 20);
            this.cmbPort.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(95, 15);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(55, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "接続";
            this.btnConnect.UseVisualStyleBackColor = true;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(155, 15);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(55, 23);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "切断";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            // 
            // chkLoggerEnable
            // 
            this.chkLoggerEnable.AutoSize = true;
            this.chkLoggerEnable.Checked = true;
            this.chkLoggerEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLoggerEnable.Location = new System.Drawing.Point(230, 19);
            this.chkLoggerEnable.Name = "chkLoggerEnable";
            this.chkLoggerEnable.Size = new System.Drawing.Size(188, 16);
            this.chkLoggerEnable.TabIndex = 3;
            this.chkLoggerEnable.Text = "データロガー機能有効 (1分/イベント)";
            this.chkLoggerEnable.UseVisualStyleBackColor = true;
            // 
            // grpRemote
            // 
            this.grpRemote.Controls.Add(this.picFanAnimation);
            this.grpRemote.Controls.Add(this.btnPower);
            this.grpRemote.Controls.Add(this.btnWind);
            this.grpRemote.Controls.Add(this.btnTimer);
            this.grpRemote.Controls.Add(this.btnRhythm);
            this.grpRemote.Controls.Add(this.btnAuto);
            this.grpRemote.Controls.Add(this.lblLampWindWeak);
            this.grpRemote.Controls.Add(this.lblLampWindMid);
            this.grpRemote.Controls.Add(this.lblLampWindStrong);
            this.grpRemote.Controls.Add(this.lblLampTimer1h);
            this.grpRemote.Controls.Add(this.lblLampTimer2h);
            this.grpRemote.Controls.Add(this.lblLampTimer4h);
            this.grpRemote.Controls.Add(this.lblLampRhythm);
            this.grpRemote.Controls.Add(this.lblLampAuto);
            this.grpRemote.Location = new System.Drawing.Point(12, 55);
            this.grpRemote.Name = "grpRemote";
            this.grpRemote.Size = new System.Drawing.Size(560, 130);
            this.grpRemote.TabIndex = 1;
            this.grpRemote.TabStop = false;
            this.grpRemote.Text = "リモート操作 ＆ ステータス表示パネル";
            // 
            // picFanAnimation
            // 
            this.picFanAnimation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picFanAnimation.Location = new System.Drawing.Point(15, 20);
            this.picFanAnimation.Name = "picFanAnimation";
            this.picFanAnimation.Size = new System.Drawing.Size(90, 95);
            this.picFanAnimation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFanAnimation.TabIndex = 0;
            this.picFanAnimation.TabStop = false;
            // 
            // btnPower
            // 
            this.btnPower.Location = new System.Drawing.Point(120, 20);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(75, 30);
            this.btnPower.TabIndex = 1;
            this.btnPower.Text = "電源 入/切";
            this.btnPower.UseVisualStyleBackColor = true;
            // 
            // btnWind
            // 
            this.btnWind.Location = new System.Drawing.Point(205, 20);
            this.btnWind.Name = "btnWind";
            this.btnWind.Size = new System.Drawing.Size(75, 30);
            this.btnWind.TabIndex = 2;
            this.btnWind.Text = "風量切替";
            this.btnWind.UseVisualStyleBackColor = true;
            // 
            // btnTimer
            // 
            this.btnTimer.Location = new System.Drawing.Point(290, 20);
            this.btnTimer.Name = "btnTimer";
            this.btnTimer.Size = new System.Drawing.Size(75, 30);
            this.btnTimer.TabIndex = 3;
            this.btnTimer.Text = "タイマー";
            this.btnTimer.UseVisualStyleBackColor = true;
            // 
            // btnRhythm
            // 
            this.btnRhythm.Location = new System.Drawing.Point(375, 20);
            this.btnRhythm.Name = "btnRhythm";
            this.btnRhythm.Size = new System.Drawing.Size(75, 30);
            this.btnRhythm.TabIndex = 4;
            this.btnRhythm.Text = "リズム風";
            this.btnRhythm.UseVisualStyleBackColor = true;
            // 
            // btnAuto
            // 
            this.btnAuto.Location = new System.Drawing.Point(460, 20);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(75, 30);
            this.btnAuto.TabIndex = 5;
            this.btnAuto.Text = "自動運転";
            this.btnAuto.UseVisualStyleBackColor = true;
            // 
            // lblLampWindWeak
            // 
            this.lblLampWindWeak.BackColor = System.Drawing.Color.LightGray;
            this.lblLampWindWeak.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampWindWeak.Location = new System.Drawing.Point(205, 60);
            this.lblLampWindWeak.Name = "lblLampWindWeak";
            this.lblLampWindWeak.Size = new System.Drawing.Size(22, 20);
            this.lblLampWindWeak.TabIndex = 6;
            this.lblLampWindWeak.Text = "弱";
            this.lblLampWindWeak.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLampWindMid
            // 
            this.lblLampWindMid.BackColor = System.Drawing.Color.LightGray;
            this.lblLampWindMid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampWindMid.Location = new System.Drawing.Point(231, 60);
            this.lblLampWindMid.Name = "lblLampWindMid";
            this.lblLampWindMid.Size = new System.Drawing.Size(22, 20);
            this.lblLampWindMid.TabIndex = 7;
            this.lblLampWindMid.Text = "中";
            this.lblLampWindMid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLampWindStrong
            // 
            this.lblLampWindStrong.BackColor = System.Drawing.Color.LightGray;
            this.lblLampWindStrong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampWindStrong.Location = new System.Drawing.Point(257, 60);
            this.lblLampWindStrong.Name = "lblLampWindStrong";
            this.lblLampWindStrong.Size = new System.Drawing.Size(22, 20);
            this.lblLampWindStrong.TabIndex = 8;
            this.lblLampWindStrong.Text = "強";
            this.lblLampWindStrong.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLampTimer1h
            // 
            this.lblLampTimer1h.BackColor = System.Drawing.Color.LightGray;
            this.lblLampTimer1h.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampTimer1h.Location = new System.Drawing.Point(290, 60);
            this.lblLampTimer1h.Name = "lblLampTimer1h";
            this.lblLampTimer1h.Size = new System.Drawing.Size(22, 20);
            this.lblLampTimer1h.TabIndex = 9;
            this.lblLampTimer1h.Text = "1h";
            this.lblLampTimer1h.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLampTimer2h
            // 
            this.lblLampTimer2h.BackColor = System.Drawing.Color.LightGray;
            this.lblLampTimer2h.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampTimer2h.Location = new System.Drawing.Point(316, 60);
            this.lblLampTimer2h.Name = "lblLampTimer2h";
            this.lblLampTimer2h.Size = new System.Drawing.Size(22, 20);
            this.lblLampTimer2h.TabIndex = 10;
            this.lblLampTimer2h.Text = "2h";
            this.lblLampTimer2h.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLampTimer4h
            // 
            this.lblLampTimer4h.BackColor = System.Drawing.Color.LightGray;
            this.lblLampTimer4h.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampTimer4h.Location = new System.Drawing.Point(342, 60);
            this.lblLampTimer4h.Name = "lblLampTimer4h";
            this.lblLampTimer4h.Size = new System.Drawing.Size(22, 20);
            this.lblLampTimer4h.TabIndex = 11;
            this.lblLampTimer4h.Text = "4h";
            this.lblLampTimer4h.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLampRhythm
            // 
            this.lblLampRhythm.BackColor = System.Drawing.Color.LightGray;
            this.lblLampRhythm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampRhythm.Location = new System.Drawing.Point(375, 60);
            this.lblLampRhythm.Name = "lblLampRhythm";
            this.lblLampRhythm.Size = new System.Drawing.Size(75, 20);
            this.lblLampRhythm.TabIndex = 12;
            this.lblLampRhythm.Text = "リズム";
            this.lblLampRhythm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLampAuto
            // 
            this.lblLampAuto.BackColor = System.Drawing.Color.LightGray;
            this.lblLampAuto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLampAuto.Location = new System.Drawing.Point(460, 60);
            this.lblLampAuto.Name = "lblLampAuto";
            this.lblLampAuto.Size = new System.Drawing.Size(75, 20);
            this.lblLampAuto.TabIndex = 13;
            this.lblLampAuto.Text = "自動";
            this.lblLampAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpGraph
            // 
            this.grpGraph.Controls.Add(this.picGraphArea);
            this.grpGraph.Location = new System.Drawing.Point(12, 190);
            this.grpGraph.Name = "grpGraph";
            this.grpGraph.Size = new System.Drawing.Size(560, 160);
            this.grpGraph.TabIndex = 2;
            this.grpGraph.TabStop = false;
            this.grpGraph.Text = "トレンドグラフ表示 (室温・リズム・風量 / 1時間)";
            // 
            // picGraphArea
            // 
            this.picGraphArea.BackColor = System.Drawing.Color.White;
            this.picGraphArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGraphArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picGraphArea.Location = new System.Drawing.Point(3, 15);
            this.picGraphArea.Name = "picGraphArea";
            this.picGraphArea.Size = new System.Drawing.Size(554, 142);
            this.picGraphArea.TabIndex = 0;
            this.picGraphArea.TabStop = false;
            // 
            // grpLogTable
            // 
            this.grpLogTable.Controls.Add(this.dgvLogHistory);
            this.grpLogTable.Location = new System.Drawing.Point(12, 355);
            this.grpLogTable.Name = "grpLogTable";
            this.grpLogTable.Size = new System.Drawing.Size(560, 190);
            this.grpLogTable.TabIndex = 3;
            this.grpLogTable.TabStop = false;
            this.grpLogTable.Text = "データログ履歴表 (リアルタイム更新)";
            // 
            // dgvLogHistory
            // 
            this.dgvLogHistory.AllowUserToAddRows = false;
            this.dgvLogHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTime,
            this.colOp,
            this.colStatus,
            this.colTemp});
            this.dgvLogHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLogHistory.Location = new System.Drawing.Point(3, 15);
            this.dgvLogHistory.Name = "dgvLogHistory";
            this.dgvLogHistory.ReadOnly = true;
            this.dgvLogHistory.RowTemplate.Height = 21;
            this.dgvLogHistory.Size = new System.Drawing.Size(554, 172);
            this.dgvLogHistory.TabIndex = 0;
            // 
            // colTime
            // 
            this.colTime.HeaderText = "時分秒";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            // 
            // colOp
            // 
            this.colOp.HeaderText = "操作";
            this.colOp.Name = "colOp";
            this.colOp.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "制御状態";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colTemp
            // 
            this.colTemp.HeaderText = "気温 (℃)";
            this.colTemp.Name = "colTemp";
            this.colTemp.ReadOnly = true;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 555);
            this.Controls.Add(this.grpLogTable);
            this.Controls.Add(this.grpGraph);
            this.Controls.Add(this.grpRemote);
            this.Controls.Add(this.grpPort);
            this.Name = "Form1";
            this.Text = "マイコン扇風機 リモート操作・データロガーシステム";
            this.grpPort.ResumeLayout(false);
            this.grpPort.PerformLayout();
            this.grpRemote.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFanAnimation)).EndInit();
            this.grpGraph.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picGraphArea)).EndInit();
            this.grpLogTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpPort;
        private System.Windows.Forms.ComboBox cmbPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.CheckBox chkLoggerEnable;
        private System.Windows.Forms.GroupBox grpRemote;
        private System.Windows.Forms.PictureBox picFanAnimation;
        private System.Windows.Forms.Button btnPower;
        private System.Windows.Forms.Button btnWind;
        private System.Windows.Forms.Button btnTimer;
        private System.Windows.Forms.Button btnRhythm;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Label lblLampWindWeak;
        private System.Windows.Forms.Label lblLampWindMid;
        private System.Windows.Forms.Label lblLampWindStrong;
        private System.Windows.Forms.Label lblLampTimer1h;
        private System.Windows.Forms.Label lblLampTimer2h;
        private System.Windows.Forms.Label lblLampTimer4h;
        private System.Windows.Forms.Label lblLampRhythm;
        private System.Windows.Forms.Label lblLampAuto;
        private System.Windows.Forms.GroupBox grpGraph;
        private System.Windows.Forms.PictureBox picGraphArea;
        private System.Windows.Forms.GroupBox grpLogTable;
        private System.Windows.Forms.DataGridView dgvLogHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTemp;
        private System.Windows.Forms.Timer timer1;
        private System.IO.Ports.SerialPort serialPort1;
    }
}