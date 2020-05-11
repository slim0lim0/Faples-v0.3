using System;

namespace FaplesEditor
{
    partial class fpxObjectEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fpxObjectEditor));
            this.pnlToolbox = new System.Windows.Forms.Panel();
            this.dgvObjects = new System.Windows.Forms.DataGridView();
            this.btnDeleteSheet = new System.Windows.Forms.Button();
            this.lblSheet = new System.Windows.Forms.Label();
            this.cmbSheet = new System.Windows.Forms.ComboBox();
            this.btnDeleteCategory = new System.Windows.Forms.Button();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.btnDeleteCollection = new System.Windows.Forms.Button();
            this.lblDiv1 = new System.Windows.Forms.Label();
            this.lblCollection = new System.Windows.Forms.Label();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.btnAddSheet = new System.Windows.Forms.Button();
            this.cmbCollection = new System.Windows.Forms.ComboBox();
            this.btnAddCollection = new System.Windows.Forms.Button();
            this.pnlEditor = new System.Windows.Forms.Panel();
            this.ediObject = new FaplesEditor.fpxPreviewPanel();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.btnDeleteObject = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAddObject = new System.Windows.Forms.Button();
            this.lblX = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblHeight = new System.Windows.Forms.Label();
            this.numY = new System.Windows.Forms.NumericUpDown();
            this.numX = new System.Windows.Forms.NumericUpDown();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.pnlProperties = new System.Windows.Forms.Panel();
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
            this.pnlAnimations = new System.Windows.Forms.Panel();
            this.pnlLookUp = new System.Windows.Forms.Panel();
            this.lblLookUp = new System.Windows.Forms.Label();
            this.lvwAnimations = new System.Windows.Forms.ListView();
            this.colAnim = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlAnimOptions = new System.Windows.Forms.Panel();
            this.numReelIndex = new System.Windows.Forms.NumericUpDown();
            this.lblReelIndex = new System.Windows.Forms.Label();
            this.numTotalFrames = new System.Windows.Forms.NumericUpDown();
            this.lblTotalFrames = new System.Windows.Forms.Label();
            this.numFrameWidth = new System.Windows.Forms.NumericUpDown();
            this.numReelHeight = new System.Windows.Forms.NumericUpDown();
            this.lblReelHeight = new System.Windows.Forms.Label();
            this.lblFrameWidth = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.numPlaySpeed = new System.Windows.Forms.NumericUpDown();
            this.btnRemoveAnim = new System.Windows.Forms.Button();
            this.btnAddAnim = new System.Windows.Forms.Button();
            this.chkAnimated = new System.Windows.Forms.CheckBox();
            this.pnlToolbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObjects)).BeginInit();
            this.pnlEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            this.pnlProperties.SuspendLayout();
            this.tsToolbar.SuspendLayout();
            this.pnlAnimations.SuspendLayout();
            this.pnlLookUp.SuspendLayout();
            this.pnlAnimOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReelIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalFrames)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReelHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPlaySpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlToolbox
            // 
            this.pnlToolbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlToolbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlToolbox.Controls.Add(this.dgvObjects);
            this.pnlToolbox.Controls.Add(this.btnDeleteSheet);
            this.pnlToolbox.Controls.Add(this.lblSheet);
            this.pnlToolbox.Controls.Add(this.cmbSheet);
            this.pnlToolbox.Controls.Add(this.btnDeleteCategory);
            this.pnlToolbox.Controls.Add(this.cmbCategory);
            this.pnlToolbox.Controls.Add(this.lblCategory);
            this.pnlToolbox.Controls.Add(this.btnDeleteCollection);
            this.pnlToolbox.Controls.Add(this.lblDiv1);
            this.pnlToolbox.Controls.Add(this.lblCollection);
            this.pnlToolbox.Controls.Add(this.btnAddCategory);
            this.pnlToolbox.Controls.Add(this.btnAddSheet);
            this.pnlToolbox.Controls.Add(this.cmbCollection);
            this.pnlToolbox.Controls.Add(this.btnAddCollection);
            this.pnlToolbox.Location = new System.Drawing.Point(909, 74);
            this.pnlToolbox.Name = "pnlToolbox";
            this.pnlToolbox.Size = new System.Drawing.Size(368, 492);
            this.pnlToolbox.TabIndex = 15;
            // 
            // dgvObjects
            // 
            this.dgvObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvObjects.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvObjects.Location = new System.Drawing.Point(3, 112);
            this.dgvObjects.MultiSelect = false;
            this.dgvObjects.Name = "dgvObjects";
            this.dgvObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvObjects.Size = new System.Drawing.Size(360, 373);
            this.dgvObjects.TabIndex = 19;
            this.dgvObjects.SelectionChanged += new System.EventHandler(this.dgvObjects_SelectionChanged);
            // 
            // btnDeleteSheet
            // 
            this.btnDeleteSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteSheet.Location = new System.Drawing.Point(306, 85);
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
            this.lblSheet.Location = new System.Drawing.Point(17, 88);
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
            this.cmbSheet.Location = new System.Drawing.Point(77, 85);
            this.cmbSheet.Name = "cmbSheet";
            this.cmbSheet.Size = new System.Drawing.Size(160, 21);
            this.cmbSheet.TabIndex = 16;
            this.cmbSheet.SelectedValueChanged += new System.EventHandler(this.cmbSheet_SelectedValueChanged);
            // 
            // btnDeleteCategory
            // 
            this.btnDeleteCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteCategory.Location = new System.Drawing.Point(306, 28);
            this.btnDeleteCategory.Name = "btnDeleteCategory";
            this.btnDeleteCategory.Size = new System.Drawing.Size(57, 21);
            this.btnDeleteCategory.TabIndex = 15;
            this.btnDeleteCategory.Text = "Delete";
            this.btnDeleteCategory.UseVisualStyleBackColor = true;
            this.btnDeleteCategory.Click += new System.EventHandler(this.btnDeleteCategory_Click);
            // 
            // cmbCategory
            // 
            this.cmbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(77, 27);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(160, 21);
            this.cmbCategory.TabIndex = 13;
            this.cmbCategory.SelectedValueChanged += new System.EventHandler(this.cmbCategory_SelectedValueChanged);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(15, 30);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Category:";
            // 
            // btnDeleteCollection
            // 
            this.btnDeleteCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteCollection.Location = new System.Drawing.Point(306, 2);
            this.btnDeleteCollection.Name = "btnDeleteCollection";
            this.btnDeleteCollection.Size = new System.Drawing.Size(57, 21);
            this.btnDeleteCollection.TabIndex = 12;
            this.btnDeleteCollection.Text = "Delete";
            this.btnDeleteCollection.UseVisualStyleBackColor = true;
            this.btnDeleteCollection.Click += new System.EventHandler(this.btnDeleteCollection_Click);
            // 
            // lblDiv1
            // 
            this.lblDiv1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDiv1.Location = new System.Drawing.Point(14, 56);
            this.lblDiv1.Name = "lblDiv1";
            this.lblDiv1.Size = new System.Drawing.Size(340, 2);
            this.lblDiv1.TabIndex = 6;
            // 
            // lblCollection
            // 
            this.lblCollection.AutoSize = true;
            this.lblCollection.Location = new System.Drawing.Point(15, 5);
            this.lblCollection.Name = "lblCollection";
            this.lblCollection.Size = new System.Drawing.Size(56, 13);
            this.lblCollection.TabIndex = 1;
            this.lblCollection.Text = "Collection:";
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCategory.Location = new System.Drawing.Point(243, 28);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(57, 21);
            this.btnAddCategory.TabIndex = 14;
            this.btnAddCategory.Text = "New...";
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // btnAddSheet
            // 
            this.btnAddSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSheet.Location = new System.Drawing.Point(243, 85);
            this.btnAddSheet.Name = "btnAddSheet";
            this.btnAddSheet.Size = new System.Drawing.Size(57, 21);
            this.btnAddSheet.TabIndex = 17;
            this.btnAddSheet.Text = "New...";
            this.btnAddSheet.UseVisualStyleBackColor = true;
            this.btnAddSheet.Click += new System.EventHandler(this.btnAddSheet_Click);
            // 
            // cmbCollection
            // 
            this.cmbCollection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCollection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCollection.FormattingEnabled = true;
            this.cmbCollection.Location = new System.Drawing.Point(77, 1);
            this.cmbCollection.Name = "cmbCollection";
            this.cmbCollection.Size = new System.Drawing.Size(160, 21);
            this.cmbCollection.TabIndex = 10;
            this.cmbCollection.SelectedValueChanged += new System.EventHandler(this.cmbCollection_SelectedValueChanged);
            // 
            // btnAddCollection
            // 
            this.btnAddCollection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCollection.Location = new System.Drawing.Point(243, 2);
            this.btnAddCollection.Name = "btnAddCollection";
            this.btnAddCollection.Size = new System.Drawing.Size(57, 21);
            this.btnAddCollection.TabIndex = 11;
            this.btnAddCollection.Text = "New...";
            this.btnAddCollection.UseVisualStyleBackColor = true;
            this.btnAddCollection.Click += new System.EventHandler(this.btnAddCollection_Click);
            // 
            // pnlEditor
            // 
            this.pnlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlEditor.AutoScroll = true;
            this.pnlEditor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEditor.Controls.Add(this.ediObject);
            this.pnlEditor.Location = new System.Drawing.Point(205, 74);
            this.pnlEditor.Name = "pnlEditor";
            this.pnlEditor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pnlEditor.Size = new System.Drawing.Size(702, 651);
            this.pnlEditor.TabIndex = 14;
            // 
            // ediObject
            // 
            this.ediObject.AutoScroll = true;
            this.ediObject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ediObject.Location = new System.Drawing.Point(3, 4);
            this.ediObject.Name = "ediObject";
            this.ediObject.Size = new System.Drawing.Size(694, 642);
            this.ediObject.TabIndex = 0;
            this.ediObject.Paint += new System.Windows.Forms.PaintEventHandler(this.ediObject_Paint);
            this.ediObject.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ediObject_MouseClick);
            this.ediObject.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ediObject_MouseMove);
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(77, 83);
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
            // btnDeleteObject
            // 
            this.btnDeleteObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteObject.Location = new System.Drawing.Point(77, 125);
            this.btnDeleteObject.Name = "btnDeleteObject";
            this.btnDeleteObject.Size = new System.Drawing.Size(64, 23);
            this.btnDeleteObject.TabIndex = 26;
            this.btnDeleteObject.Text = "Delete";
            this.btnDeleteObject.UseVisualStyleBackColor = true;
            this.btnDeleteObject.Click += new System.EventHandler(this.btnDeleteObject_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(221, 125);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(67, 23);
            this.btnSave.TabIndex = 27;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAddObject
            // 
            this.btnAddObject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddObject.Location = new System.Drawing.Point(4, 125);
            this.btnAddObject.Name = "btnAddObject";
            this.btnAddObject.Size = new System.Drawing.Size(67, 23);
            this.btnAddObject.TabIndex = 25;
            this.btnAddObject.Text = "New...";
            this.btnAddObject.UseVisualStyleBackColor = true;
            this.btnAddObject.Click += new System.EventHandler(this.btnAddObject_Click);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(11, 7);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(17, 13);
            this.lblX.TabIndex = 3;
            this.lblX.Text = "X:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(294, 125);
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
            this.lblHeight.Location = new System.Drawing.Point(11, 85);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(41, 13);
            this.lblHeight.TabIndex = 11;
            this.lblHeight.Text = "Height:";
            // 
            // numY
            // 
            this.numY.Location = new System.Drawing.Point(77, 31);
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
            this.numX.Location = new System.Drawing.Point(77, 5);
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
            this.numWidth.Location = new System.Drawing.Point(77, 57);
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
            this.lblWidth.Location = new System.Drawing.Point(11, 59);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(38, 13);
            this.lblWidth.TabIndex = 10;
            this.lblWidth.Text = "Width:";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(11, 33);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(17, 13);
            this.lblY.TabIndex = 8;
            this.lblY.Text = "Y:";
            // 
            // pnlProperties
            // 
            this.pnlProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProperties.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlProperties.Controls.Add(this.numHeight);
            this.pnlProperties.Controls.Add(this.btnDeleteObject);
            this.pnlProperties.Controls.Add(this.btnSave);
            this.pnlProperties.Controls.Add(this.btnAddObject);
            this.pnlProperties.Controls.Add(this.lblX);
            this.pnlProperties.Controls.Add(this.btnCancel);
            this.pnlProperties.Controls.Add(this.lblHeight);
            this.pnlProperties.Controls.Add(this.numY);
            this.pnlProperties.Controls.Add(this.numX);
            this.pnlProperties.Controls.Add(this.numWidth);
            this.pnlProperties.Controls.Add(this.lblWidth);
            this.pnlProperties.Controls.Add(this.lblY);
            this.pnlProperties.Location = new System.Drawing.Point(909, 572);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(368, 153);
            this.pnlProperties.TabIndex = 16;
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
            this.tsToolbar.TabIndex = 1;
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
            this.tsbAddPoint.ToolTipText = resources.GetString("tsbAddPoint.ToolTipText");
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
            this.tsbConfirm.ToolTipText = "Confirm\r\n----------\r\nSaves the current group of added points.";
            this.tsbConfirm.Click += new System.EventHandler(this.tsbConfirm_Click);
            // 
            // tsbHolds
            // 
            this.tsbHolds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tsbHolds.Name = "tsbHolds";
            this.tsbHolds.Size = new System.Drawing.Size(121, 70);
            this.tsbHolds.ToolTipText = resources.GetString("tsbHolds.ToolTipText");
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
            this.tsbHoldProperties.ToolTipText = resources.GetString("tsbHoldProperties.ToolTipText");
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
            this.tsbDelete.ToolTipText = "Remove\r\n----------\r\nRemoves the currently selected Hold group.";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // pnlAnimations
            // 
            this.pnlAnimations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlAnimations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAnimations.Controls.Add(this.pnlLookUp);
            this.pnlAnimations.Controls.Add(this.lvwAnimations);
            this.pnlAnimations.Controls.Add(this.pnlAnimOptions);
            this.pnlAnimations.Location = new System.Drawing.Point(3, 74);
            this.pnlAnimations.Name = "pnlAnimations";
            this.pnlAnimations.Size = new System.Drawing.Size(200, 651);
            this.pnlAnimations.TabIndex = 13;
            // 
            // pnlLookUp
            // 
            this.pnlLookUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlLookUp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLookUp.Controls.Add(this.lblLookUp);
            this.pnlLookUp.Location = new System.Drawing.Point(3, 529);
            this.pnlLookUp.Name = "pnlLookUp";
            this.pnlLookUp.Size = new System.Drawing.Size(192, 117);
            this.pnlLookUp.TabIndex = 2;
            // 
            // lblLookUp
            // 
            this.lblLookUp.AutoSize = true;
            this.lblLookUp.Location = new System.Drawing.Point(14, 9);
            this.lblLookUp.Name = "lblLookUp";
            this.lblLookUp.Size = new System.Drawing.Size(48, 13);
            this.lblLookUp.TabIndex = 0;
            this.lblLookUp.Text = "Look Up";
            // 
            // lvwAnimations
            // 
            this.lvwAnimations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lvwAnimations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvwAnimations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAnim});
            this.lvwAnimations.Enabled = false;
            this.lvwAnimations.Location = new System.Drawing.Point(3, 244);
            this.lvwAnimations.MultiSelect = false;
            this.lvwAnimations.Name = "lvwAnimations";
            this.lvwAnimations.Size = new System.Drawing.Size(192, 279);
            this.lvwAnimations.TabIndex = 9;
            this.lvwAnimations.UseCompatibleStateImageBehavior = false;
            this.lvwAnimations.SelectedIndexChanged += new System.EventHandler(this.lvwAnimations_SelectedIndexChanged);
            // 
            // colAnim
            // 
            this.colAnim.Width = 279;
            // 
            // pnlAnimOptions
            // 
            this.pnlAnimOptions.Controls.Add(this.numReelIndex);
            this.pnlAnimOptions.Controls.Add(this.lblReelIndex);
            this.pnlAnimOptions.Controls.Add(this.numTotalFrames);
            this.pnlAnimOptions.Controls.Add(this.lblTotalFrames);
            this.pnlAnimOptions.Controls.Add(this.numFrameWidth);
            this.pnlAnimOptions.Controls.Add(this.numReelHeight);
            this.pnlAnimOptions.Controls.Add(this.lblReelHeight);
            this.pnlAnimOptions.Controls.Add(this.lblFrameWidth);
            this.pnlAnimOptions.Controls.Add(this.lblSpeed);
            this.pnlAnimOptions.Controls.Add(this.numPlaySpeed);
            this.pnlAnimOptions.Controls.Add(this.btnRemoveAnim);
            this.pnlAnimOptions.Controls.Add(this.btnAddAnim);
            this.pnlAnimOptions.Controls.Add(this.chkAnimated);
            this.pnlAnimOptions.Location = new System.Drawing.Point(3, 3);
            this.pnlAnimOptions.Name = "pnlAnimOptions";
            this.pnlAnimOptions.Size = new System.Drawing.Size(192, 235);
            this.pnlAnimOptions.TabIndex = 0;
            // 
            // numReelIndex
            // 
            this.numReelIndex.Enabled = false;
            this.numReelIndex.Location = new System.Drawing.Point(76, 95);
            this.numReelIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReelIndex.Name = "numReelIndex";
            this.numReelIndex.Size = new System.Drawing.Size(56, 20);
            this.numReelIndex.TabIndex = 3;
            this.numReelIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReelIndex.ValueChanged += new System.EventHandler(this.numReelIndex_ValueChanged);
            // 
            // lblReelIndex
            // 
            this.lblReelIndex.AutoSize = true;
            this.lblReelIndex.Location = new System.Drawing.Point(5, 101);
            this.lblReelIndex.Name = "lblReelIndex";
            this.lblReelIndex.Size = new System.Drawing.Size(61, 13);
            this.lblReelIndex.TabIndex = 23;
            this.lblReelIndex.Text = "Reel Index:";
            // 
            // numTotalFrames
            // 
            this.numTotalFrames.Enabled = false;
            this.numTotalFrames.Location = new System.Drawing.Point(76, 147);
            this.numTotalFrames.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTotalFrames.Name = "numTotalFrames";
            this.numTotalFrames.Size = new System.Drawing.Size(56, 20);
            this.numTotalFrames.TabIndex = 5;
            this.numTotalFrames.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTotalFrames.ValueChanged += new System.EventHandler(this.numTotalFrames_ValueChanged);
            // 
            // lblTotalFrames
            // 
            this.lblTotalFrames.AutoSize = true;
            this.lblTotalFrames.Location = new System.Drawing.Point(5, 149);
            this.lblTotalFrames.Name = "lblTotalFrames";
            this.lblTotalFrames.Size = new System.Drawing.Size(71, 13);
            this.lblTotalFrames.TabIndex = 21;
            this.lblTotalFrames.Text = "Total Frames:";
            // 
            // numFrameWidth
            // 
            this.numFrameWidth.Enabled = false;
            this.numFrameWidth.Location = new System.Drawing.Point(76, 121);
            this.numFrameWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numFrameWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrameWidth.Name = "numFrameWidth";
            this.numFrameWidth.Size = new System.Drawing.Size(56, 20);
            this.numFrameWidth.TabIndex = 4;
            this.numFrameWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrameWidth.ValueChanged += new System.EventHandler(this.numFrameWidth_ValueChanged);
            // 
            // numReelHeight
            // 
            this.numReelHeight.Enabled = false;
            this.numReelHeight.Location = new System.Drawing.Point(77, 32);
            this.numReelHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numReelHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReelHeight.Name = "numReelHeight";
            this.numReelHeight.Size = new System.Drawing.Size(56, 20);
            this.numReelHeight.TabIndex = 2;
            this.numReelHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numReelHeight.ValueChanged += new System.EventHandler(this.numReelHeight_ValueChanged);
            // 
            // lblReelHeight
            // 
            this.lblReelHeight.AutoSize = true;
            this.lblReelHeight.Location = new System.Drawing.Point(5, 34);
            this.lblReelHeight.Name = "lblReelHeight";
            this.lblReelHeight.Size = new System.Drawing.Size(66, 13);
            this.lblReelHeight.TabIndex = 18;
            this.lblReelHeight.Text = "Reel Height:";
            // 
            // lblFrameWidth
            // 
            this.lblFrameWidth.AutoSize = true;
            this.lblFrameWidth.Location = new System.Drawing.Point(5, 123);
            this.lblFrameWidth.Name = "lblFrameWidth";
            this.lblFrameWidth.Size = new System.Drawing.Size(70, 13);
            this.lblFrameWidth.TabIndex = 17;
            this.lblFrameWidth.Text = "Frame Width:";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(15, 182);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 13);
            this.lblSpeed.TabIndex = 16;
            this.lblSpeed.Text = "Speed:";
            // 
            // numPlaySpeed
            // 
            this.numPlaySpeed.DecimalPlaces = 2;
            this.numPlaySpeed.Enabled = false;
            this.numPlaySpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPlaySpeed.Location = new System.Drawing.Point(76, 180);
            this.numPlaySpeed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPlaySpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPlaySpeed.Name = "numPlaySpeed";
            this.numPlaySpeed.Size = new System.Drawing.Size(56, 20);
            this.numPlaySpeed.TabIndex = 6;
            this.numPlaySpeed.UseWaitCursor = true;
            this.numPlaySpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPlaySpeed.ValueChanged += new System.EventHandler(this.numPlaySpeed_ValueChanged);
            // 
            // btnRemoveAnim
            // 
            this.btnRemoveAnim.Enabled = false;
            this.btnRemoveAnim.Location = new System.Drawing.Point(76, 209);
            this.btnRemoveAnim.Name = "btnRemoveAnim";
            this.btnRemoveAnim.Size = new System.Drawing.Size(67, 23);
            this.btnRemoveAnim.TabIndex = 8;
            this.btnRemoveAnim.Text = "Remove";
            this.btnRemoveAnim.UseVisualStyleBackColor = true;
            this.btnRemoveAnim.Click += new System.EventHandler(this.btnRemoveAnim_Click);
            // 
            // btnAddAnim
            // 
            this.btnAddAnim.Enabled = false;
            this.btnAddAnim.Location = new System.Drawing.Point(3, 209);
            this.btnAddAnim.Name = "btnAddAnim";
            this.btnAddAnim.Size = new System.Drawing.Size(67, 23);
            this.btnAddAnim.TabIndex = 7;
            this.btnAddAnim.Text = "Add";
            this.btnAddAnim.UseVisualStyleBackColor = true;
            this.btnAddAnim.Click += new System.EventHandler(this.btnAddAnim_Click);
            // 
            // chkAnimated
            // 
            this.chkAnimated.AutoSize = true;
            this.chkAnimated.Location = new System.Drawing.Point(4, 4);
            this.chkAnimated.Name = "chkAnimated";
            this.chkAnimated.Size = new System.Drawing.Size(76, 17);
            this.chkAnimated.TabIndex = 1;
            this.chkAnimated.Text = "Animated?";
            this.chkAnimated.UseVisualStyleBackColor = true;
            this.chkAnimated.CheckedChanged += new System.EventHandler(this.chkAnimated_CheckedChanged);
            // 
            // fpxObjectEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlToolbox);
            this.Controls.Add(this.pnlEditor);
            this.Controls.Add(this.pnlProperties);
            this.Controls.Add(this.tsToolbar);
            this.Controls.Add(this.pnlAnimations);
            this.Name = "fpxObjectEditor";
            this.Size = new System.Drawing.Size(1280, 727);
            this.pnlToolbox.ResumeLayout(false);
            this.pnlToolbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvObjects)).EndInit();
            this.pnlEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            this.pnlProperties.ResumeLayout(false);
            this.pnlProperties.PerformLayout();
            this.tsToolbar.ResumeLayout(false);
            this.tsToolbar.PerformLayout();
            this.pnlAnimations.ResumeLayout(false);
            this.pnlLookUp.ResumeLayout(false);
            this.pnlLookUp.PerformLayout();
            this.pnlAnimOptions.ResumeLayout(false);
            this.pnlAnimOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReelIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTotalFrames)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrameWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReelHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPlaySpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlToolbox;
        private System.Windows.Forms.DataGridView dgvObjects;
        private System.Windows.Forms.Button btnDeleteSheet;
        private System.Windows.Forms.Label lblSheet;
        private System.Windows.Forms.ComboBox cmbSheet;
        private System.Windows.Forms.Button btnDeleteCategory;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Button btnDeleteCollection;
        private System.Windows.Forms.Label lblDiv1;
        private System.Windows.Forms.Label lblCollection;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Button btnAddSheet;
        private System.Windows.Forms.ComboBox cmbCollection;
        private System.Windows.Forms.Button btnAddCollection;
        private System.Windows.Forms.Panel pnlEditor;
        private fpxPreviewPanel ediObject;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Button btnDeleteObject;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnAddObject;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.NumericUpDown numY;
        private System.Windows.Forms.NumericUpDown numX;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Panel pnlProperties;
        private System.Windows.Forms.ToolStrip tsToolbar;
        private System.Windows.Forms.ToolStripButton tsbFoothold;
        private System.Windows.Forms.ToolStripButton tsbSeathold;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        private System.Windows.Forms.ToolStripSeparator tssMap1;
        private System.Windows.Forms.Panel pnlAnimations;
        private System.Windows.Forms.Panel pnlLookUp;
        private System.Windows.Forms.Label lblLookUp;
        private System.Windows.Forms.ListView lvwAnimations;
        private System.Windows.Forms.ColumnHeader colAnim;
        private System.Windows.Forms.Panel pnlAnimOptions;
        private System.Windows.Forms.NumericUpDown numReelIndex;
        private System.Windows.Forms.Label lblReelIndex;
        private System.Windows.Forms.NumericUpDown numTotalFrames;
        private System.Windows.Forms.Label lblTotalFrames;
        private System.Windows.Forms.NumericUpDown numFrameWidth;
        private System.Windows.Forms.NumericUpDown numReelHeight;
        private System.Windows.Forms.Label lblReelHeight;
        private System.Windows.Forms.Label lblFrameWidth;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.NumericUpDown numPlaySpeed;
        private System.Windows.Forms.Button btnRemoveAnim;
        private System.Windows.Forms.Button btnAddAnim;
        private System.Windows.Forms.CheckBox chkAnimated;
        private System.Windows.Forms.ToolStripButton tsbClimbhold;
        private System.Windows.Forms.ToolStripButton tsbAddPoint;
        private System.Windows.Forms.ToolStripComboBox tsbHolds;
        private System.Windows.Forms.ToolStripButton tsbConfirm;
        private System.Windows.Forms.ToolStripButton tsbHoldProperties;
    }
}
