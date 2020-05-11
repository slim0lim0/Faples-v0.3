namespace FaplesEditor
{
    partial class fpxSkeletonDialog
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
            this.lblConnectingPart = new System.Windows.Forms.Label();
            this.cmbConnectPoint = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblPartType = new System.Windows.Forms.Label();
            this.cmbPartType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblConnectingPart
            // 
            this.lblConnectingPart.AutoSize = true;
            this.lblConnectingPart.Location = new System.Drawing.Point(12, 22);
            this.lblConnectingPart.Name = "lblConnectingPart";
            this.lblConnectingPart.Size = new System.Drawing.Size(73, 13);
            this.lblConnectingPart.TabIndex = 0;
            this.lblConnectingPart.Text = "Connect from:";
            // 
            // cmbConnectPoint
            // 
            this.cmbConnectPoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnectPoint.FormattingEnabled = true;
            this.cmbConnectPoint.Location = new System.Drawing.Point(91, 19);
            this.cmbConnectPoint.Name = "cmbConnectPoint";
            this.cmbConnectPoint.Size = new System.Drawing.Size(198, 21);
            this.cmbConnectPoint.TabIndex = 1;
            this.cmbConnectPoint.SelectedIndexChanged += new System.EventHandler(this.cmbConnectPoint_SelectedIndexChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(133, 98);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(214, 98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPartType
            // 
            this.lblPartType.AutoSize = true;
            this.lblPartType.Location = new System.Drawing.Point(12, 57);
            this.lblPartType.Name = "lblPartType";
            this.lblPartType.Size = new System.Drawing.Size(56, 13);
            this.lblPartType.TabIndex = 6;
            this.lblPartType.Text = "Part Type:";
            // 
            // cmbPartType
            // 
            this.cmbPartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPartType.FormattingEnabled = true;
            this.cmbPartType.Location = new System.Drawing.Point(91, 54);
            this.cmbPartType.Name = "cmbPartType";
            this.cmbPartType.Size = new System.Drawing.Size(198, 21);
            this.cmbPartType.TabIndex = 7;
            this.cmbPartType.SelectedIndexChanged += new System.EventHandler(this.cmbPartType_SelectedIndexChanged);
            // 
            // fpxSkeletonDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 133);
            this.Controls.Add(this.cmbPartType);
            this.Controls.Add(this.lblPartType);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.cmbConnectPoint);
            this.Controls.Add(this.lblConnectingPart);
            this.Name = "fpxSkeletonDialog";
            this.Text = "Skeleton Part";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblConnectingPart;
        private System.Windows.Forms.ComboBox cmbConnectPoint;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblPartType;
        private System.Windows.Forms.ComboBox cmbPartType;
    }
}