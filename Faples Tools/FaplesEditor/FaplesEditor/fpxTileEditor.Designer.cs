namespace FaplesEditor
{
    partial class fpxTileEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fpxTileEditor));
            this.tsToolbar = new System.Windows.Forms.ToolStrip();
            this.tsbFoothold = new System.Windows.Forms.ToolStripButton();
            this.tsbSeathold = new System.Windows.Forms.ToolStripButton();
            this.tsbClimbhold = new System.Windows.Forms.ToolStripButton();
            this.tssMap1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddPoint = new System.Windows.Forms.ToolStripButton();
            this.tsbConfirm = new System.Windows.Forms.ToolStripButton();
            this.tsbHolds = new System.Windows.Forms.ToolStripComboBox();
            this.tsbHoldProperties = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.ediTile = new FaplesEditor.fpxPreviewPanel();
            this.pnlToolbox = new System.Windows.Forms.Panel();
            this.dgvTiles = new System.Windows.Forms.DataGridView();
            this.btnDeleteSheet = new System.Windows.Forms.Button();
            this.lblSheet = new System.Windows.Forms.Label();
            this.cmbSheet = new System.Windows.Forms.ComboBox();
            this.btnAddSheet = new System.Windows.Forms.Button();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.btnDeleteTile = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddTile = new System.Windows.Forms.Button();
            this.lblX = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblHeight = new System.Windows.Forms.Label();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.tsToolbar.SuspendLayout();
            this.pnlEditor.SuspendLayout();
            this.pnlToolbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTiles)).BeginInit();
            this.pnlProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // tsToolbar
            // 
            this.tsToolbar.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.tsToolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFoothold,
            this.tsbSeathold,
            this.tsbClimbhold,
            this.tssMap1,
            this.tsbAddPoint,
            this.tsbConfirm,
            this.tsbHolds,
            this.tsbHoldProperties,
            this.tsbDelete});
            this.tsToolbar.Location = new System.Drawing.Point(0, 0);
            this.tsToolbar.Name = "tsToolbar";
            this.tsToolbar.Size = new System.Drawing.Size(1280, 70);
            this.tsToolbar.TabIndex = 2;
            this.tsToolbar.TabStop = true;
            this.tsToolbar.Text = "toolStrip1";
            // 
            // tsbFoothold
            // 
            this.tsbFoothold.Image = ((System.Drawing.Image)(resources.GetObject("tsbFoothold.Image")));
            this.tsbFoothold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFoothold.Name = "tsbFoothold";
            this.tsbFoothold.Size = new System.Drawing.Size(59, 67);
            this.tsbFoothold.Text = "Foothold";
            this.tsbFoothold.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbFoothold.ToolTipText = resources.GetString("tsbFoothold.ToolTipText");
            this.tsbFoothold.Click += new System.EventHandler(this.tsbFoothold_Click);
            // 
            // tsbSeathold
            // 
            this.tsbSeathold.Image = ((System.Drawing.Image)(resources.GetObject("tsbSeathold.Image")));
            this.tsbSeathold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSeathold.Name = "tsbSeathold";
            this.tsbSeathold.Size = new System.Drawing.Size(57, 67);
            this.tsbSeathold.Text = "Seathold";
            this.tsbSeathold.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSeathold.ToolTipText = "Seathold\r\n----------\r\nAllows you to create anchor points that map out where a pla" +
    "yer can sit.\r\n\r\nThis is only available when interacting with foreground layers (" +
    "0 - 7).";
            this.tsbSeathold.Click += new System.EventHandler(this.tsbSeathold_Click);
            // 
            // tsbClimbhold
            // 
            this.tsbClimbhold.Image = ((System.Drawing.Image)(resources.GetObject("tsbClimbhold.Image")));
            this.tsbClimbhold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClimbhold.Name = "tsbClimbhold";
            this.tsbClimbhold.Size = new System.Drawing.Size(67, 67);
            this.tsbClimbhold.Text = "Climbhold";
            this.tsbClimbhold.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbClimbhold.ToolTipText = "Climbhold\r\n----------\r\nAllows you to create anchor points that map out where a pl" +
    "ayer can climb up or down.\r\n\r\nThis is only available when interacting with foreg" +
    "round layers (0 - 7).";
            this.tsbClimbhold.Click += new System.EventHandler(this.tsbClimbhold_Click);
            // 
            // tssMap1
            // 
            this.tssMap1.Name = "tssMap1";
            this.tssMap1.Size = new System.Drawing.Size(6, 70);
            // 
            // tsbAddPoint
            // 
            this.tsbAddPoint.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddPoint.Image")));
            this.tsbAddPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddPoint.Name = "tsbAddPoint";
            this.tsbAddPoint.Size = new System.Drawing.Size(52, 67);
            this.tsbAddPoint.Text = "Add";
            this.tsbAddPoint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAddPoint.ToolTipText = "Climbhold\r\n----------\r\nAllows you to create anchor points that map out where a pl" +
    "ayer can climb up or down.\r\n\r\nThis is only available when interacting with foreg" +
    "round layers (0 - 7).";
            this.tsbAddPoint.Click += new System.EventHandler(this.tsbAddPoint_Click);
            // 
            // tsbConfirm
            // 
            this.tsbConfirm.Image = ((System.Drawing.Image)(resources.GetObject("tsbConfirm.Image")));
            this.tsbConfirm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConfirm.Name = "tsbConfirm";
            this.tsbConfirm.Size = new System.Drawing.Size(55, 67);
            this.tsbConfirm.Text = "Confirm";
            this.tsbConfirm.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbConfirm.ToolTipText = "Climbhold\r\n----------\r\nAllows you to create anchor points that map out where a pl" +
    "ayer can climb up or down.\r\n\r\nThis is only available when interacting with foreg" +
    "round layers (0 - 7).";
            this.tsbConfirm.Click += new System.EventHandler(this.tsbConfirm_Click);
            // 
            // tsbHolds
            // 
            this.tsbHolds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsbHolds.Name = "tsbHolds";
            this.tsbHolds.Size = new System.Drawing.Size(121, 70);
            this.tsbHolds.SelectedIndexChanged += new System.EventHandler(this.tsbHolds_SelectedIndexChanged);
            // 
            // tsbHoldProperties
            // 
            this.tsbHoldProperties.Image = ((System.Drawing.Image)(resources.GetObject("tsbHoldProperties.Image")));
            this.tsbHoldProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHoldProperties.Name = "tsbHoldProperties";
            this.tsbHoldProperties.Size = new System.Drawing.Size(64, 67);
            this.tsbHoldProperties.Text = "Properties";
            this.tsbHoldProperties.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbHoldProperties.Click += new System.EventHandler(this.tsbHoldProperties_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(54, 67);
            this.tsbDelete.Text = "Remove";
            this.tsbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDelete.ToolTipText = "Climbhold\r\n----------\r\nAllows you to create anchor points that map out where a pl" +
    "ayer can climb up or down.\r\n\r\nThis is only available when interacting with foreg" +
    "round layers (0 - 7).";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // pnlEditor
            // 
            this.pnlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEditor.AutoScroll = true;
            this.pnlEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEditor.Controls.Add(this.ediTile);
            this.pnlEditor.Location = new System.Drawing.Point(3, 73);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlEditor.Size = new System.Drawing.Size(904, 651);
            this.pnlEditor.TabIndex = 15;
            // 
            // ediTile
            // 
            this.ediTile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ediTile.AutoScroll = true;
            this.ediTile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ediTile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ediTile.Location = new System.Drawing.Point(3, 3);
            this.ediTile.Name = "ediTile";
            this.ediTile.Size = new System.Drawing.Size(896, 643);
            this.ediTile.TabIndex = 0;
            this.ediTile.Paint += new System.Windows.Forms.PaintEventHandler(this.ediTile_Paint);
            this.ediTile.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ediTile_MouseClick);
            this.ediTile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ediTile_MouseMove);
            // 
            // pnlToolbox
            // 
            this.pnlToolbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlToolbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlToolbox.Controls.Add(this.dgvTiles);
            this.pnlToolbox.Controls.Add(this.btnDeleteSheet);
            this.pnlToolbox.Controls.Add(this.lblSheet);
            this.pnlToolbox.Controls.Add(this.cmbSheet);
            this.pnlToolbox.Controls.Add(this.btnAddSheet);
            this.pnlToolbox.Location = new System.Drawing.Point(909, 73);
            this.pnlToolbox.Name = "pnlToolbox";
            this.pnlToolbox.Size = new System.Drawing.Size(368, 492);
            this.pnlToolbox.TabIndex = 16;
            // 
            // dgvTiles
            // 
            this.dgvTiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTiles.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvTiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTiles.Location = new System.Drawing.Point(3, 35);
            this.dgvTiles.MultiSelect = false;
            this.dgvTiles.Name = "dgvTiles";
            this.dgvTiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTiles.Size = new System.Drawing.Size(360, 450);
            this.dgvTiles.TabIndex = 19;
            this.dgvTiles.SelectionChanged += new System.EventHandler(this.dgvTiles_SelectionChanged);
            // 
            // btnDeleteSheet
            // 
            this.btnDeleteSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSheet.Location = new System.Drawing.Point(306, 7);
            this.btnDeleteSheet.Name = "btnDeleteSheet";
            this.btnDeleteSheet.Size = new System.Drawing.Size(57, 21);
            this.btnDeleteSheet.TabIndex = 18;
            this.btnDeleteSheet.Text = "Delete";
            this.btnDeleteSheet.UseVisualStyleBackColor = true;
            this.btnDeleteSheet.Click += new System.EventHandler(this.btnDeleteSheet_Click);
            // 
            // lblSheet
            // 
            this.lblSheet.AutoSize = true;
            this.lblSheet.Location = new System.Drawing.Point(17, 10);
            this.lblSheet.Name = "lblSheet";
            this.lblSheet.Size = new System.Drawing.Size(38, 13);
            this.lblSheet.TabIndex = 4;
            this.lblSheet.Text = "Sheet:";
            // 
            // cmbSheet
            // 
            this.cmbSheet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSheet.FormattingEnabled = true;
            this.cmbSheet.Location = new System.Drawing.Point(77, 7);
            this.cmbSheet.Name = "cmbSheet";
            this.cmbSheet.Size = new System.Drawing.Size(160, 21);
            this.cmbSheet.TabIndex = 16;
            this.cmbSheet.SelectedValueChanged += new System.EventHandler(this.cmbSheet_SelectedValueChanged);
            // 
            // btnAddSheet
            // 
            this.btnAddSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSheet.Location = new System.Drawing.Point(243, 7);
            this.btnAddSheet.Name = "btnAddSheet";
            this.btnAddSheet.Size = new System.Drawing.Size(57, 21);
            this.btnAddSheet.TabIndex = 17;
            this.btnAddSheet.Text = "New...";
            this.btnAddSheet.UseVisualStyleBackColor = true;
            this.btnAddSheet.Click += new System.EventHandler(this.btnAddSheet_Click);
            // 
            // pnlProperties
            // 
            this.pnlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProperties.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProperties.Controls.Add(this.numHeight);
            this.pnlProperties.Controls.Add(this.btnDeleteTile);
            this.pnlProperties.Controls.Add(this.btnSave);
            this.pnlProperties.Controls.Add(this.btnAddTile);
            this.pnlProperties.Controls.Add(this.lblX);
            this.pnlProperties.Controls.Add(this.btnCancel);
            this.pnlProperties.Controls.Add(this.lblHeight);
            this.pnlProperties.Controls.Add(this.numY);
            this.pnlProperties.Controls.Add(this.numX);
            this.pnlProperties.Controls.Add(this.numWidth);
            this.pnlProperties.Controls.Add(this.lblWidth);
            this.pnlProperties.Controls.Add(this.lblY);
            this.pnlProperties.Location = new System.Drawing.Point(909, 571);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(368, 149);
            this.pnlProperties.TabIndex = 17;
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(77, 84);
            this.numHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(120, 20);
            this.numHeight.TabIndex = 24;
            this.numHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeight.ValueChanged += new System.EventHandler(this.numHeight_ValueChanged);
            // 
            // btnDeleteTile
            // 
            this.btnDeleteTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTile.Location = new System.Drawing.Point(77, 121);
            this.btnDeleteTile.Name = "btnDeleteTile";
            this.btnDeleteTile.Size = new System.Drawing.Size(64, 23);
            this.btnDeleteTile.TabIndex = 26;
            this.btnDeleteTile.Text = "Delete";
            this.btnDeleteTile.UseVisualStyleBackColor = true;
            this.btnDeleteTile.Click += new System.EventHandler(this.btnDeleteTile_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(221, 121);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(67, 23);
            this.btnSave.TabIndex = 27;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddTile
            // 
            this.btnAddTile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTile.Location = new System.Drawing.Point(4, 121);
            this.btnAddTile.Name = "btnAddTile";
            this.btnAddTile.Size = new System.Drawing.Size(67, 23);
            this.btnAddTile.TabIndex = 25;
            this.btnAddTile.Text = "New...";
            this.btnAddTile.UseVisualStyleBackColor = true;
            this.btnAddTile.Click += new System.EventHandler(this.btnAddTile_Click);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(11, 8);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(17, 13);
            this.lblX.TabIndex = 3;
            this.lblX.Text = "X:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(294, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(11, 86);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 11;
            this.lblHeight.Text = "Height:";
            // 
            // numY
            // 
            this.numY.Location = new System.Drawing.Point(77, 32);
            this.numY.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numY.Name = "numY";
            this.numY.Size = new System.Drawing.Size(120, 20);
            this.numY.TabIndex = 22;
            this.numY.ValueChanged += new System.EventHandler(this.numY_ValueChanged);
            // 
            // numX
            // 
            this.numX.Location = new System.Drawing.Point(77, 6);
            this.numX.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numX.Name = "numX";
            this.numX.Size = new System.Drawing.Size(120, 20);
            this.numX.TabIndex = 21;
            this.numX.ValueChanged += new System.EventHandler(this.numX_ValueChanged);
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(77, 58);
            this.numWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(120, 20);
            this.numWidth.TabIndex = 23;
            this.numWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWidth.ValueChanged += new System.EventHandler(this.numWidth_ValueChanged);
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(11, 60);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 10;
            this.lblWidth.Text = "Width:";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(11, 34);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(17, 13);
            this.lblY.TabIndex = 8;
            this.lblY.Text = "Y:";
            // 
            // fpxTileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlProperties);
            this.Controls.Add(this.pnlToolbox);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.tsToolbar);
            this.Name = "fpxTileEditor";
            this.Size = new System.Drawing.Size(1280, 727);
            this.tsToolbar.ResumeLayout(false);
            this.tsToolbar.PerformLayout();
            this.pnlEditor.ResumeLayout(false);
            this.pnlToolbox.ResumeLayout(false);
            this.pnlToolbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTiles)).EndInit();
            this.pnlProperties.ResumeLayout(false);
            this.pnlProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsToolbar;
        private System.Windows.Forms.ToolStripButton tsbFoothold;
        private System.Windows.Forms.ToolStripButton tsbSeathold;
        private System.Windows.Forms.ToolStripButton tsbClimbhold;
        private System.Windows.Forms.ToolStripSeparator tssMap1;
        private System.Windows.Forms.ToolStripButton tsbAddPoint;
        private System.Windows.Forms.ToolStripButton tsbConfirm;
        private System.Windows.Forms.ToolStripComboBox tsbHolds;
        private System.Windows.Forms.ToolStripButton tsbHoldProperties;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.Panel pnlEditor;
        private System.Windows.Forms.Panel pnlToolbox;
        private System.Windows.Forms.DataGridView dgvTiles;
        private System.Windows.Forms.Button btnDeleteSheet;
        private System.Windows.Forms.Label lblSheet;
        private System.Windows.Forms.ComboBox cmbSheet;
        private System.Windows.Forms.Button btnAddSheet;
        private System.Windows.Forms.Panel pnlProperties;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Button btnDeleteTile;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddTile;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numX;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblY;
        private fpxPreviewPanel ediTile;
    }
}
