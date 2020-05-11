namespace FaplesEditor
{
    partial class fpxItemEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fpxItemEditor));
            this.tsToolbar = new System.Windows.Forms.ToolStrip();
            this.tsbEquipment = new System.Windows.Forms.ToolStripButton();
            this.tsbUsables = new System.Windows.Forms.ToolStripButton();
            this.tsbEtc = new System.Windows.Forms.ToolStripButton();
            this.tsbMoney = new System.Windows.Forms.ToolStripButton();
            this.flpToolbox = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlCharacterProperties = new System.Windows.Forms.Panel();
            this.numLevel = new System.Windows.Forms.NumericUpDown();
            this.lblLevel = new System.Windows.Forms.Label();
            this.txtEquipmentName = new System.Windows.Forms.TextBox();
            this.lblEquipmentName = new System.Windows.Forms.Label();
            this.dgvSprites = new System.Windows.Forms.DataGridView();
            this.btnDeleteSprite = new System.Windows.Forms.Button();
            this.chkAnimated = new System.Windows.Forms.CheckBox();
            this.btnAddSprite = new System.Windows.Forms.Button();
            this.chkSkeleton = new System.Windows.Forms.CheckBox();
            this.lblSprite = new System.Windows.Forms.Label();
            this.btnDeleteCharacter = new System.Windows.Forms.Button();
            this.cmbCharacter = new System.Windows.Forms.ComboBox();
            this.btnAddCharacter = new System.Windows.Forms.Button();
            this.lblEquipment = new System.Windows.Forms.Label();
            this.pnlSave = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.ediItem = new FaplesEditor.fpxPreviewPanel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblBuyPrice = new System.Windows.Forms.Label();
            this.lblSellPrice = new System.Windows.Forms.Label();
            this.numBuyPrice = new System.Windows.Forms.NumericUpDown();
            this.numSellPrice = new System.Windows.Forms.NumericUpDown();
            this.tsToolbar.SuspendLayout();
            this.flpToolbox.SuspendLayout();
            this.pnlCharacterProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSprites)).BeginInit();
            this.pnlSave.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBuyPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // tsToolbar
            // 
            this.tsToolbar.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.tsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbEquipment,
            this.tsbUsables,
            this.tsbEtc,
            this.tsbMoney});
            this.tsToolbar.Location = new System.Drawing.Point(0, 0);
            this.tsToolbar.Name = "tsToolbar";
            this.tsToolbar.Size = new System.Drawing.Size(1280, 70);
            this.tsToolbar.TabIndex = 18;
            this.tsToolbar.TabStop = true;
            this.tsToolbar.Text = "toolStrip1";
            // 
            // tsbEquipment
            // 
            this.tsbEquipment.Image = ((System.Drawing.Image)(resources.GetObject("tsbEquipment.Image")));
            this.tsbEquipment.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEquipment.Name = "tsbEquipment";
            this.tsbEquipment.Size = new System.Drawing.Size(69, 67);
            this.tsbEquipment.Text = "Equipment";
            this.tsbEquipment.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEquipment.ToolTipText = resources.GetString("tsbEquipment.ToolTipText");
            // 
            // tsbUsables
            // 
            this.tsbUsables.Image = ((System.Drawing.Image)(resources.GetObject("tsbUsables.Image")));
            this.tsbUsables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUsables.Name = "tsbUsables";
            this.tsbUsables.Size = new System.Drawing.Size(52, 67);
            this.tsbUsables.Text = "Usables";
            this.tsbUsables.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbUsables.ToolTipText = resources.GetString("tsbUsables.ToolTipText");
            // 
            // tsbEtc
            // 
            this.tsbEtc.Image = ((System.Drawing.Image)(resources.GetObject("tsbEtc.Image")));
            this.tsbEtc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEtc.Name = "tsbEtc";
            this.tsbEtc.Size = new System.Drawing.Size(52, 67);
            this.tsbEtc.Text = "Etc.";
            this.tsbEtc.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbEtc.ToolTipText = resources.GetString("tsbEtc.ToolTipText");
            // 
            // tsbMoney
            // 
            this.tsbMoney.Image = ((System.Drawing.Image)(resources.GetObject("tsbMoney.Image")));
            this.tsbMoney.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoney.Name = "tsbMoney";
            this.tsbMoney.Size = new System.Drawing.Size(52, 67);
            this.tsbMoney.Text = "Money";
            this.tsbMoney.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbMoney.ToolTipText = resources.GetString("tsbMoney.ToolTipText");
            // 
            // flpToolbox
            // 
            this.flpToolbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpToolbox.AutoScroll = true;
            this.flpToolbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpToolbox.Controls.Add(this.pnlCharacterProperties);
            this.flpToolbox.Location = new System.Drawing.Point(863, 73);
            this.flpToolbox.Name = "flpToolbox";
            this.flpToolbox.Size = new System.Drawing.Size(414, 611);
            this.flpToolbox.TabIndex = 21;
            // 
            // pnlCharacterProperties
            // 
            this.pnlCharacterProperties.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCharacterProperties.Controls.Add(this.numSellPrice);
            this.pnlCharacterProperties.Controls.Add(this.numBuyPrice);
            this.pnlCharacterProperties.Controls.Add(this.lblSellPrice);
            this.pnlCharacterProperties.Controls.Add(this.lblBuyPrice);
            this.pnlCharacterProperties.Controls.Add(this.txtDescription);
            this.pnlCharacterProperties.Controls.Add(this.lblDescription);
            this.pnlCharacterProperties.Controls.Add(this.numLevel);
            this.pnlCharacterProperties.Controls.Add(this.lblLevel);
            this.pnlCharacterProperties.Controls.Add(this.txtEquipmentName);
            this.pnlCharacterProperties.Controls.Add(this.lblEquipmentName);
            this.pnlCharacterProperties.Controls.Add(this.dgvSprites);
            this.pnlCharacterProperties.Controls.Add(this.btnDeleteSprite);
            this.pnlCharacterProperties.Controls.Add(this.chkAnimated);
            this.pnlCharacterProperties.Controls.Add(this.btnAddSprite);
            this.pnlCharacterProperties.Controls.Add(this.chkSkeleton);
            this.pnlCharacterProperties.Controls.Add(this.lblSprite);
            this.pnlCharacterProperties.Controls.Add(this.btnDeleteCharacter);
            this.pnlCharacterProperties.Controls.Add(this.cmbCharacter);
            this.pnlCharacterProperties.Controls.Add(this.btnAddCharacter);
            this.pnlCharacterProperties.Controls.Add(this.lblEquipment);
            this.pnlCharacterProperties.Location = new System.Drawing.Point(3, 3);
            this.pnlCharacterProperties.Name = "pnlCharacterProperties";
            this.pnlCharacterProperties.Size = new System.Drawing.Size(398, 599);
            this.pnlCharacterProperties.TabIndex = 12;
            // 
            // numLevel
            // 
            this.numLevel.Location = new System.Drawing.Point(72, 186);
            this.numLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLevel.Name = "numLevel";
            this.numLevel.Size = new System.Drawing.Size(52, 20);
            this.numLevel.TabIndex = 29;
            this.numLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(28, 188);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(36, 13);
            this.lblLevel.TabIndex = 28;
            this.lblLevel.Text = "Level:";
            // 
            // txtEquipmentName
            // 
            this.txtEquipmentName.Location = new System.Drawing.Point(72, 37);
            this.txtEquipmentName.Name = "txtEquipmentName";
            this.txtEquipmentName.Size = new System.Drawing.Size(157, 20);
            this.txtEquipmentName.TabIndex = 27;
            // 
            // lblEquipmentName
            // 
            this.lblEquipmentName.AutoSize = true;
            this.lblEquipmentName.Location = new System.Drawing.Point(28, 40);
            this.lblEquipmentName.Name = "lblEquipmentName";
            this.lblEquipmentName.Size = new System.Drawing.Size(38, 13);
            this.lblEquipmentName.TabIndex = 26;
            this.lblEquipmentName.Text = "Name:";
            // 
            // dgvSprites
            // 
            this.dgvSprites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSprites.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvSprites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSprites.Location = new System.Drawing.Point(3, 466);
            this.dgvSprites.MultiSelect = false;
            this.dgvSprites.Name = "dgvSprites";
            this.dgvSprites.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSprites.Size = new System.Drawing.Size(387, 128);
            this.dgvSprites.TabIndex = 25;
            // 
            // btnDeleteSprite
            // 
            this.btnDeleteSprite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSprite.Location = new System.Drawing.Point(340, 439);
            this.btnDeleteSprite.Name = "btnDeleteSprite";
            this.btnDeleteSprite.Size = new System.Drawing.Size(57, 21);
            this.btnDeleteSprite.TabIndex = 24;
            this.btnDeleteSprite.Text = "Delete";
            this.btnDeleteSprite.UseVisualStyleBackColor = true;
            // 
            // chkAnimated
            // 
            this.chkAnimated.AutoSize = true;
            this.chkAnimated.Location = new System.Drawing.Point(14, 346);
            this.chkAnimated.Name = "chkAnimated";
            this.chkAnimated.Size = new System.Drawing.Size(76, 17);
            this.chkAnimated.TabIndex = 17;
            this.chkAnimated.Text = "Animated?";
            this.chkAnimated.UseVisualStyleBackColor = true;
            // 
            // btnAddSprite
            // 
            this.btnAddSprite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSprite.Location = new System.Drawing.Point(277, 439);
            this.btnAddSprite.Name = "btnAddSprite";
            this.btnAddSprite.Size = new System.Drawing.Size(57, 21);
            this.btnAddSprite.TabIndex = 23;
            this.btnAddSprite.Text = "New...";
            this.btnAddSprite.UseVisualStyleBackColor = true;
            // 
            // chkSkeleton
            // 
            this.chkSkeleton.AutoSize = true;
            this.chkSkeleton.Location = new System.Drawing.Point(14, 323);
            this.chkSkeleton.Name = "chkSkeleton";
            this.chkSkeleton.Size = new System.Drawing.Size(74, 17);
            this.chkSkeleton.TabIndex = 16;
            this.chkSkeleton.Text = "Skeleton?";
            this.chkSkeleton.UseVisualStyleBackColor = true;
            // 
            // lblSprite
            // 
            this.lblSprite.AutoSize = true;
            this.lblSprite.Location = new System.Drawing.Point(6, 439);
            this.lblSprite.Name = "lblSprite";
            this.lblSprite.Size = new System.Drawing.Size(39, 13);
            this.lblSprite.TabIndex = 21;
            this.lblSprite.Text = "Sprites";
            // 
            // btnDeleteCharacter
            // 
            this.btnDeleteCharacter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteCharacter.Location = new System.Drawing.Point(298, 6);
            this.btnDeleteCharacter.Name = "btnDeleteCharacter";
            this.btnDeleteCharacter.Size = new System.Drawing.Size(57, 21);
            this.btnDeleteCharacter.TabIndex = 15;
            this.btnDeleteCharacter.Text = "Delete";
            this.btnDeleteCharacter.UseVisualStyleBackColor = true;
            // 
            // cmbCharacter
            // 
            this.cmbCharacter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCharacter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCharacter.FormattingEnabled = true;
            this.cmbCharacter.Location = new System.Drawing.Point(72, 6);
            this.cmbCharacter.Name = "cmbCharacter";
            this.cmbCharacter.Size = new System.Drawing.Size(157, 21);
            this.cmbCharacter.TabIndex = 13;
            // 
            // btnAddCharacter
            // 
            this.btnAddCharacter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCharacter.Location = new System.Drawing.Point(235, 6);
            this.btnAddCharacter.Name = "btnAddCharacter";
            this.btnAddCharacter.Size = new System.Drawing.Size(57, 21);
            this.btnAddCharacter.TabIndex = 14;
            this.btnAddCharacter.Text = "New...";
            this.btnAddCharacter.UseVisualStyleBackColor = true;
            // 
            // lblEquipment
            // 
            this.lblEquipment.AutoSize = true;
            this.lblEquipment.Location = new System.Drawing.Point(6, 9);
            this.lblEquipment.Name = "lblEquipment";
            this.lblEquipment.Size = new System.Drawing.Size(60, 13);
            this.lblEquipment.TabIndex = 0;
            this.lblEquipment.Text = "Equipment:";
            // 
            // pnlSave
            // 
            this.pnlSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSave.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSave.Controls.Add(this.btnSave);
            this.pnlSave.Controls.Add(this.btnCancel);
            this.pnlSave.Location = new System.Drawing.Point(863, 690);
            this.pnlSave.Name = "pnlSave";
            this.pnlSave.Size = new System.Drawing.Size(414, 31);
            this.pnlSave.TabIndex = 37;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(138, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(219, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlEditor
            // 
            this.pnlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEditor.AutoScroll = true;
            this.pnlEditor.Controls.Add(this.ediItem);
            this.pnlEditor.Location = new System.Drawing.Point(7, 73);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlEditor.Size = new System.Drawing.Size(850, 648);
            this.pnlEditor.TabIndex = 38;
            // 
            // ediItem
            // 
            this.ediItem.AutoScroll = true;
            this.ediItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ediItem.Location = new System.Drawing.Point(3, 3);
            this.ediItem.Name = "ediItem";
            this.ediItem.Size = new System.Drawing.Size(394, 279);
            this.ediItem.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(6, 68);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 30;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(72, 65);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(318, 58);
            this.txtDescription.TabIndex = 31;
            // 
            // lblBuyPrice
            // 
            this.lblBuyPrice.AutoSize = true;
            this.lblBuyPrice.Location = new System.Drawing.Point(11, 140);
            this.lblBuyPrice.Name = "lblBuyPrice";
            this.lblBuyPrice.Size = new System.Drawing.Size(55, 13);
            this.lblBuyPrice.TabIndex = 32;
            this.lblBuyPrice.Text = "Buy Price:";
            // 
            // lblSellPrice
            // 
            this.lblSellPrice.AutoSize = true;
            this.lblSellPrice.Location = new System.Drawing.Point(131, 140);
            this.lblSellPrice.Name = "lblSellPrice";
            this.lblSellPrice.Size = new System.Drawing.Size(54, 13);
            this.lblSellPrice.TabIndex = 33;
            this.lblSellPrice.Text = "Sell Price:";
            // 
            // numBuyPrice
            // 
            this.numBuyPrice.Location = new System.Drawing.Point(72, 138);
            this.numBuyPrice.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.numBuyPrice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBuyPrice.Name = "numBuyPrice";
            this.numBuyPrice.Size = new System.Drawing.Size(52, 20);
            this.numBuyPrice.TabIndex = 34;
            this.numBuyPrice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numSellPrice
            // 
            this.numSellPrice.Location = new System.Drawing.Point(191, 138);
            this.numSellPrice.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.numSellPrice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSellPrice.Name = "numSellPrice";
            this.numSellPrice.Size = new System.Drawing.Size(52, 20);
            this.numSellPrice.TabIndex = 35;
            this.numSellPrice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // fpxItemEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlSave);
            this.Controls.Add(this.flpToolbox);
            this.Controls.Add(this.tsToolbar);
            this.Name = "fpxItemEditor";
            this.Size = new System.Drawing.Size(1280, 727);
            this.tsToolbar.ResumeLayout(false);
            this.tsToolbar.PerformLayout();
            this.flpToolbox.ResumeLayout(false);
            this.pnlCharacterProperties.ResumeLayout(false);
            this.pnlCharacterProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSprites)).EndInit();
            this.pnlSave.ResumeLayout(false);
            this.pnlEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numBuyPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSellPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsToolbar;
        private System.Windows.Forms.ToolStripButton tsbEquipment;
        private System.Windows.Forms.FlowLayoutPanel flpToolbox;
        private System.Windows.Forms.Panel pnlCharacterProperties;
        private System.Windows.Forms.DataGridView dgvSprites;
        private System.Windows.Forms.Button btnDeleteSprite;
        private System.Windows.Forms.CheckBox chkAnimated;
        private System.Windows.Forms.Button btnAddSprite;
        private System.Windows.Forms.CheckBox chkSkeleton;
        private System.Windows.Forms.Label lblSprite;
        private System.Windows.Forms.Button btnDeleteCharacter;
        private System.Windows.Forms.ComboBox cmbCharacter;
        private System.Windows.Forms.Button btnAddCharacter;
        private System.Windows.Forms.Label lblEquipment;
        private System.Windows.Forms.Panel pnlSave;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlEditor;
        private fpxPreviewPanel ediItem;
        private System.Windows.Forms.ToolStripButton tsbUsables;
        private System.Windows.Forms.ToolStripButton tsbEtc;
        private System.Windows.Forms.ToolStripButton tsbMoney;
        private System.Windows.Forms.Label lblEquipmentName;
        private System.Windows.Forms.TextBox txtEquipmentName;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.NumericUpDown numLevel;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblBuyPrice;
        private System.Windows.Forms.Label lblSellPrice;
        private System.Windows.Forms.NumericUpDown numSellPrice;
        private System.Windows.Forms.NumericUpDown numBuyPrice;
    }
}
