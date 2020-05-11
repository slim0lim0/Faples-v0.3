namespace FaplesServer
{
    partial class FplServerThreaded
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bgwManager = new System.ComponentModel.BackgroundWorker();
            this.bgwListener = new System.ComponentModel.BackgroundWorker();
            this.bgwChat = new System.ComponentModel.BackgroundWorker();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtServerChat = new System.Windows.Forms.TextBox();
            this.txtServerLog = new System.Windows.Forms.TextBox();
            this.btnServerStop = new System.Windows.Forms.Button();
            this.btnServerStart = new System.Windows.Forms.Button();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.bgwSender = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(319, 530);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 21;
            this.btnSendMsg.Text = "Send";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 532);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(301, 20);
            this.txtMessage.TabIndex = 20;
            // 
            // txtServerChat
            // 
            this.txtServerChat.Location = new System.Drawing.Point(12, 341);
            this.txtServerChat.Multiline = true;
            this.txtServerChat.Name = "txtServerChat";
            this.txtServerChat.ReadOnly = true;
            this.txtServerChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtServerChat.Size = new System.Drawing.Size(382, 185);
            this.txtServerChat.TabIndex = 19;
            // 
            // txtServerLog
            // 
            this.txtServerLog.Location = new System.Drawing.Point(12, 69);
            this.txtServerLog.Multiline = true;
            this.txtServerLog.Name = "txtServerLog";
            this.txtServerLog.ReadOnly = true;
            this.txtServerLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtServerLog.Size = new System.Drawing.Size(382, 266);
            this.txtServerLog.TabIndex = 18;
            // 
            // btnServerStop
            // 
            this.btnServerStop.Location = new System.Drawing.Point(334, 32);
            this.btnServerStop.Name = "btnServerStop";
            this.btnServerStop.Size = new System.Drawing.Size(60, 22);
            this.btnServerStop.TabIndex = 17;
            this.btnServerStop.Text = "Stop";
            this.btnServerStop.UseVisualStyleBackColor = true;
            // 
            // btnServerStart
            // 
            this.btnServerStart.Location = new System.Drawing.Point(268, 32);
            this.btnServerStart.Name = "btnServerStart";
            this.btnServerStart.Size = new System.Drawing.Size(60, 22);
            this.btnServerStart.TabIndex = 16;
            this.btnServerStart.Text = "Start";
            this.btnServerStart.UseVisualStyleBackColor = true;
            this.btnServerStart.Click += new System.EventHandler(this.btnServerStart_Click);
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(163, 35);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(29, 13);
            this.lblServerPort.TabIndex = 15;
            this.lblServerPort.Text = "Port:";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(198, 32);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(55, 20);
            this.txtServerPort.TabIndex = 14;
            this.txtServerPort.Text = "29278";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(12, 35);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(20, 13);
            this.lblServerIP.TabIndex = 13;
            this.lblServerIP.Text = "IP:";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(9, 8);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 12;
            this.lblServer.Text = "Server";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(38, 32);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(119, 20);
            this.txtServerIP.TabIndex = 11;
            this.txtServerIP.Text = "127.0.0.1";
            // 
            // FplServerThreaded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 567);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtServerChat);
            this.Controls.Add(this.txtServerLog);
            this.Controls.Add(this.btnServerStop);
            this.Controls.Add(this.btnServerStart);
            this.Controls.Add(this.lblServerPort);
            this.Controls.Add(this.txtServerPort);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtServerIP);
            this.Name = "FplServerThreaded";
            this.Text = "FplServerThreaded";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgwManager;
        private System.ComponentModel.BackgroundWorker bgwListener;
        private System.ComponentModel.BackgroundWorker bgwChat;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtServerChat;
        private System.Windows.Forms.TextBox txtServerLog;
        private System.Windows.Forms.Button btnServerStop;
        private System.Windows.Forms.Button btnServerStart;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.ComponentModel.BackgroundWorker bgwSender;
    }
}