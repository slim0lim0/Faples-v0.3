namespace FaplesClientHarness
{
    partial class FplClientThreaded
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
            this.lblTest = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblDiv1 = new System.Windows.Forms.Label();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtServerChat = new System.Windows.Forms.TextBox();
            this.txtServerLog = new System.Windows.Forms.TextBox();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblServerPort = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.lblServerIP = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.btnWorld1 = new System.Windows.Forms.Button();
            this.btnWorld3 = new System.Windows.Forms.Button();
            this.btnWorld2 = new System.Windows.Forms.Button();
            this.lblWorld = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = true;
            this.lblTest.Location = new System.Drawing.Point(443, 13);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(98, 13);
            this.lblTest.TabIndex = 38;
            this.lblTest.Text = "Byte Package Test";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(446, 83);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 37;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblDiv1
            // 
            this.lblDiv1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblDiv1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDiv1.Location = new System.Drawing.Point(415, 12);
            this.lblDiv1.Name = "lblDiv1";
            this.lblDiv1.Size = new System.Drawing.Size(10, 544);
            this.lblDiv1.TabIndex = 36;
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(319, 531);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(75, 23);
            this.btnSendMsg.TabIndex = 35;
            this.btnSendMsg.Text = "Send";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.BtnSendMsg_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(12, 533);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(301, 20);
            this.txtMessage.TabIndex = 34;
            // 
            // txtServerChat
            // 
            this.txtServerChat.Location = new System.Drawing.Point(12, 342);
            this.txtServerChat.Multiline = true;
            this.txtServerChat.Name = "txtServerChat";
            this.txtServerChat.ReadOnly = true;
            this.txtServerChat.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtServerChat.Size = new System.Drawing.Size(382, 185);
            this.txtServerChat.TabIndex = 33;
            // 
            // txtServerLog
            // 
            this.txtServerLog.Location = new System.Drawing.Point(12, 70);
            this.txtServerLog.Multiline = true;
            this.txtServerLog.Name = "txtServerLog";
            this.txtServerLog.ReadOnly = true;
            this.txtServerLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtServerLog.Size = new System.Drawing.Size(382, 266);
            this.txtServerLog.TabIndex = 32;
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(325, 33);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(69, 22);
            this.btnDisconnect.TabIndex = 31;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(259, 33);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(60, 22);
            this.btnConnect.TabIndex = 30;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblServerPort
            // 
            this.lblServerPort.AutoSize = true;
            this.lblServerPort.Location = new System.Drawing.Point(163, 36);
            this.lblServerPort.Name = "lblServerPort";
            this.lblServerPort.Size = new System.Drawing.Size(29, 13);
            this.lblServerPort.TabIndex = 29;
            this.lblServerPort.Text = "Port:";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(198, 33);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(55, 20);
            this.txtServerPort.TabIndex = 28;
            this.txtServerPort.Text = "29278";
            // 
            // lblServerIP
            // 
            this.lblServerIP.AutoSize = true;
            this.lblServerIP.Location = new System.Drawing.Point(12, 36);
            this.lblServerIP.Name = "lblServerIP";
            this.lblServerIP.Size = new System.Drawing.Size(20, 13);
            this.lblServerIP.TabIndex = 27;
            this.lblServerIP.Text = "IP:";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(9, 9);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 26;
            this.lblServer.Text = "Server";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(38, 33);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(119, 20);
            this.txtServerIP.TabIndex = 25;
            this.txtServerIP.Text = "127.0.0.1";
            // 
            // btnWorld1
            // 
            this.btnWorld1.Location = new System.Drawing.Point(446, 154);
            this.btnWorld1.Name = "btnWorld1";
            this.btnWorld1.Size = new System.Drawing.Size(75, 23);
            this.btnWorld1.TabIndex = 39;
            this.btnWorld1.Text = "World 1";
            this.btnWorld1.UseVisualStyleBackColor = true;
            this.btnWorld1.Click += new System.EventHandler(this.BtnWorld1_Click);
            // 
            // btnWorld3
            // 
            this.btnWorld3.Location = new System.Drawing.Point(446, 212);
            this.btnWorld3.Name = "btnWorld3";
            this.btnWorld3.Size = new System.Drawing.Size(75, 23);
            this.btnWorld3.TabIndex = 40;
            this.btnWorld3.Text = "World 3";
            this.btnWorld3.UseVisualStyleBackColor = true;
            this.btnWorld3.Click += new System.EventHandler(this.BtnWorld3_Click);
            // 
            // btnWorld2
            // 
            this.btnWorld2.Location = new System.Drawing.Point(446, 183);
            this.btnWorld2.Name = "btnWorld2";
            this.btnWorld2.Size = new System.Drawing.Size(75, 23);
            this.btnWorld2.TabIndex = 41;
            this.btnWorld2.Text = "World 2";
            this.btnWorld2.UseVisualStyleBackColor = true;
            this.btnWorld2.Click += new System.EventHandler(this.BtnWorld2_Click);
            // 
            // lblWorld
            // 
            this.lblWorld.AutoSize = true;
            this.lblWorld.Location = new System.Drawing.Point(446, 120);
            this.lblWorld.Name = "lblWorld";
            this.lblWorld.Size = new System.Drawing.Size(35, 13);
            this.lblWorld.TabIndex = 42;
            this.lblWorld.Text = "World";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(446, 53);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(33, 13);
            this.lblLogin.TabIndex = 43;
            this.lblLogin.Text = "Login";
            // 
            // FplClientThreaded
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 568);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.lblWorld);
            this.Controls.Add(this.btnWorld2);
            this.Controls.Add(this.btnWorld3);
            this.Controls.Add(this.btnWorld1);
            this.Controls.Add(this.lblTest);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblDiv1);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtServerChat);
            this.Controls.Add(this.txtServerLog);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lblServerPort);
            this.Controls.Add(this.txtServerPort);
            this.Controls.Add(this.lblServerIP);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.txtServerIP);
            this.Name = "FplClientThreaded";
            this.Text = "FplClientThreaded";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblDiv1;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtServerChat;
        private System.Windows.Forms.TextBox txtServerLog;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblServerPort;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label lblServerIP;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Button btnWorld1;
        private System.Windows.Forms.Button btnWorld3;
        private System.Windows.Forms.Button btnWorld2;
        private System.Windows.Forms.Label lblWorld;
        private System.Windows.Forms.Label lblLogin;
    }
}