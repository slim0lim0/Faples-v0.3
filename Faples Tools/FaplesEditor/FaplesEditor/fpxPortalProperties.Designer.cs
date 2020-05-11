namespace FaplesEditor
{
    partial class fpxPortalProperties
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
            this.lblType = new System.Windows.Forms.Label();
            this.lblMap = new System.Windows.Forms.Label();
            this.cmbPortalType = new System.Windows.Forms.ComboBox();
            this.cmbMapName = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.cmbTargetPortal = new System.Windows.Forms.ComboBox();
            this.lblTargetPortal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(12, 13);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(64, 13);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Portal Type:";
            // 
            // lblMap
            // 
            this.lblMap.AutoSize = true;
            this.lblMap.Location = new System.Drawing.Point(14, 70);
            this.lblMap.Name = "lblMap";
            this.lblMap.Size = new System.Drawing.Size(62, 13);
            this.lblMap.TabIndex = 1;
            this.lblMap.Text = "Map Name:";
            // 
            // cmbPortalType
            // 
            this.cmbPortalType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPortalType.FormattingEnabled = true;
            this.cmbPortalType.Location = new System.Drawing.Point(93, 13);
            this.cmbPortalType.Name = "cmbPortalType";
            this.cmbPortalType.Size = new System.Drawing.Size(234, 21);
            this.cmbPortalType.TabIndex = 2;
            this.cmbPortalType.SelectedValueChanged += new System.EventHandler(this.cmbPortalType_SelectedValueChanged);
            // 
            // cmbMapName
            // 
            this.cmbMapName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapName.FormattingEnabled = true;
            this.cmbMapName.Location = new System.Drawing.Point(93, 67);
            this.cmbMapName.Name = "cmbMapName";
            this.cmbMapName.Size = new System.Drawing.Size(234, 21);
            this.cmbMapName.TabIndex = 3;
            this.cmbMapName.SelectedValueChanged += new System.EventHandler(this.cmbMapName_SelectedValueChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(197, 134);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(61, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(264, 134);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbRegion
            // 
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.Location = new System.Drawing.Point(93, 40);
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(234, 21);
            this.cmbRegion.TabIndex = 6;
            this.cmbRegion.SelectedValueChanged += new System.EventHandler(this.cmbRegion_SelectedValueChanged);
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(12, 43);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(75, 13);
            this.lblRegion.TabIndex = 7;
            this.lblRegion.Text = "Region Name:";
            // 
            // cmbTargetPortal
            // 
            this.cmbTargetPortal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetPortal.FormattingEnabled = true;
            this.cmbTargetPortal.Location = new System.Drawing.Point(93, 94);
            this.cmbTargetPortal.Name = "cmbTargetPortal";
            this.cmbTargetPortal.Size = new System.Drawing.Size(234, 21);
            this.cmbTargetPortal.TabIndex = 9;
            this.cmbTargetPortal.SelectedValueChanged += new System.EventHandler(this.cmbTargetPortal_SelectedValueChanged);
            // 
            // lblTargetPortal
            // 
            this.lblTargetPortal.AutoSize = true;
            this.lblTargetPortal.Location = new System.Drawing.Point(14, 97);
            this.lblTargetPortal.Name = "lblTargetPortal";
            this.lblTargetPortal.Size = new System.Drawing.Size(71, 13);
            this.lblTargetPortal.TabIndex = 8;
            this.lblTargetPortal.Text = "Target Portal:";
            // 
            // fpxPortalProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 169);
            this.Controls.Add(this.cmbTargetPortal);
            this.Controls.Add(this.lblTargetPortal);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.cmbRegion);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbMapName);
            this.Controls.Add(this.cmbPortalType);
            this.Controls.Add(this.lblMap);
            this.Controls.Add(this.lblType);
            this.Name = "fpxPortalProperties";
            this.Text = "Portal Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblMap;
        private System.Windows.Forms.ComboBox cmbPortalType;
        private System.Windows.Forms.ComboBox cmbMapName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbRegion;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.ComboBox cmbTargetPortal;
        private System.Windows.Forms.Label lblTargetPortal;
    }
}