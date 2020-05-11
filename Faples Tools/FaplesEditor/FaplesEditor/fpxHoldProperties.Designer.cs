namespace FaplesEditor
{
    partial class fpxHoldProperties
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
            this.dgvHolds = new System.Windows.Forms.DataGridView();
            this.lblId = new System.Windows.Forms.Label();
            this.lblGroupId = new System.Windows.Forms.Label();
            this.lblNextId = new System.Windows.Forms.Label();
            this.lblX2 = new System.Windows.Forms.Label();
            this.lblY2 = new System.Windows.Forms.Label();
            this.lblPrevId = new System.Windows.Forms.Label();
            this.lblX1 = new System.Windows.Forms.Label();
            this.lblY1 = new System.Windows.Forms.Label();
            this.lblForce = new System.Windows.Forms.Label();
            this.lblCantPass = new System.Windows.Forms.Label();
            this.lblCantDrop = new System.Windows.Forms.Label();
            this.lblCantMove = new System.Windows.Forms.Label();
            this.numId = new System.Windows.Forms.NumericUpDown();
            this.numY2 = new System.Windows.Forms.NumericUpDown();
            this.numX2 = new System.Windows.Forms.NumericUpDown();
            this.numNextId = new System.Windows.Forms.NumericUpDown();
            this.numX1 = new System.Windows.Forms.NumericUpDown();
            this.numPrevId = new System.Windows.Forms.NumericUpDown();
            this.numGid = new System.Windows.Forms.NumericUpDown();
            this.numY1 = new System.Windows.Forms.NumericUpDown();
            this.numForce = new System.Windows.Forms.NumericUpDown();
            this.cmbCantPass = new System.Windows.Forms.ComboBox();
            this.cmbCantDrop = new System.Windows.Forms.ComboBox();
            this.cmbCantMove = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtType = new System.Windows.Forms.TextBox();
            this.lblType = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHolds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNextId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrevId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numForce)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvHolds
            // 
            this.dgvHolds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHolds.Location = new System.Drawing.Point(12, 12);
            this.dgvHolds.MultiSelect = false;
            this.dgvHolds.Name = "dgvHolds";
            this.dgvHolds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHolds.Size = new System.Drawing.Size(240, 426);
            this.dgvHolds.TabIndex = 0;
            this.dgvHolds.SelectionChanged += new System.EventHandler(this.dgvHolds_SelectionChanged);
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(3, 5);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(21, 13);
            this.lblId.TabIndex = 1;
            this.lblId.Text = "ID:";
            // 
            // lblGroupId
            // 
            this.lblGroupId.AutoSize = true;
            this.lblGroupId.Location = new System.Drawing.Point(3, 31);
            this.lblGroupId.Name = "lblGroupId";
            this.lblGroupId.Size = new System.Drawing.Size(53, 13);
            this.lblGroupId.TabIndex = 2;
            this.lblGroupId.Text = "Group ID:";
            // 
            // lblNextId
            // 
            this.lblNextId.AutoSize = true;
            this.lblNextId.Location = new System.Drawing.Point(3, 57);
            this.lblNextId.Name = "lblNextId";
            this.lblNextId.Size = new System.Drawing.Size(46, 13);
            this.lblNextId.TabIndex = 3;
            this.lblNextId.Text = "Next ID:";
            // 
            // lblX2
            // 
            this.lblX2.AutoSize = true;
            this.lblX2.Location = new System.Drawing.Point(3, 83);
            this.lblX2.Name = "lblX2";
            this.lblX2.Size = new System.Drawing.Size(23, 13);
            this.lblX2.TabIndex = 4;
            this.lblX2.Text = "X2:";
            // 
            // lblY2
            // 
            this.lblY2.AutoSize = true;
            this.lblY2.Location = new System.Drawing.Point(3, 109);
            this.lblY2.Name = "lblY2";
            this.lblY2.Size = new System.Drawing.Size(23, 13);
            this.lblY2.TabIndex = 5;
            this.lblY2.Text = "Y2:";
            // 
            // lblPrevId
            // 
            this.lblPrevId.AutoSize = true;
            this.lblPrevId.Location = new System.Drawing.Point(3, 135);
            this.lblPrevId.Name = "lblPrevId";
            this.lblPrevId.Size = new System.Drawing.Size(46, 13);
            this.lblPrevId.TabIndex = 6;
            this.lblPrevId.Text = "Prev ID:";
            // 
            // lblX1
            // 
            this.lblX1.AutoSize = true;
            this.lblX1.Location = new System.Drawing.Point(6, 194);
            this.lblX1.Name = "lblX1";
            this.lblX1.Size = new System.Drawing.Size(23, 13);
            this.lblX1.TabIndex = 7;
            this.lblX1.Text = "X1:";
            // 
            // lblY1
            // 
            this.lblY1.AutoSize = true;
            this.lblY1.Location = new System.Drawing.Point(6, 222);
            this.lblY1.Name = "lblY1";
            this.lblY1.Size = new System.Drawing.Size(23, 13);
            this.lblY1.TabIndex = 8;
            this.lblY1.Text = "Y1:";
            // 
            // lblForce
            // 
            this.lblForce.AutoSize = true;
            this.lblForce.Location = new System.Drawing.Point(6, 248);
            this.lblForce.Name = "lblForce";
            this.lblForce.Size = new System.Drawing.Size(37, 13);
            this.lblForce.TabIndex = 9;
            this.lblForce.Text = "Force:";
            // 
            // lblCantPass
            // 
            this.lblCantPass.AutoSize = true;
            this.lblCantPass.Location = new System.Drawing.Point(6, 275);
            this.lblCantPass.Name = "lblCantPass";
            this.lblCantPass.Size = new System.Drawing.Size(60, 13);
            this.lblCantPass.TabIndex = 10;
            this.lblCantPass.Text = "Can\'t Pass:";
            // 
            // lblCantDrop
            // 
            this.lblCantDrop.AutoSize = true;
            this.lblCantDrop.Location = new System.Drawing.Point(6, 302);
            this.lblCantDrop.Name = "lblCantDrop";
            this.lblCantDrop.Size = new System.Drawing.Size(60, 13);
            this.lblCantDrop.TabIndex = 11;
            this.lblCantDrop.Text = "Can\'t Drop:";
            // 
            // lblCantMove
            // 
            this.lblCantMove.AutoSize = true;
            this.lblCantMove.Location = new System.Drawing.Point(6, 329);
            this.lblCantMove.Name = "lblCantMove";
            this.lblCantMove.Size = new System.Drawing.Size(64, 13);
            this.lblCantMove.TabIndex = 12;
            this.lblCantMove.Text = "Can\'t Move:";
            // 
            // numId
            // 
            this.numId.Enabled = false;
            this.numId.Location = new System.Drawing.Point(75, 3);
            this.numId.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numId.Name = "numId";
            this.numId.Size = new System.Drawing.Size(53, 20);
            this.numId.TabIndex = 13;
            // 
            // numY2
            // 
            this.numY2.Enabled = false;
            this.numY2.Location = new System.Drawing.Point(75, 107);
            this.numY2.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numY2.Name = "numY2";
            this.numY2.Size = new System.Drawing.Size(53, 20);
            this.numY2.TabIndex = 14;
            // 
            // numX2
            // 
            this.numX2.Enabled = false;
            this.numX2.Location = new System.Drawing.Point(75, 81);
            this.numX2.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numX2.Name = "numX2";
            this.numX2.Size = new System.Drawing.Size(53, 20);
            this.numX2.TabIndex = 15;
            // 
            // numNextId
            // 
            this.numNextId.Enabled = false;
            this.numNextId.Location = new System.Drawing.Point(75, 55);
            this.numNextId.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numNextId.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.numNextId.Name = "numNextId";
            this.numNextId.Size = new System.Drawing.Size(53, 20);
            this.numNextId.TabIndex = 16;
            // 
            // numX1
            // 
            this.numX1.Location = new System.Drawing.Point(75, 194);
            this.numX1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numX1.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.numX1.Name = "numX1";
            this.numX1.Size = new System.Drawing.Size(53, 20);
            this.numX1.TabIndex = 17;
            this.numX1.ValueChanged += new System.EventHandler(this.numX1_ValueChanged);
            // 
            // numPrevId
            // 
            this.numPrevId.Enabled = false;
            this.numPrevId.Location = new System.Drawing.Point(75, 133);
            this.numPrevId.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numPrevId.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.numPrevId.Name = "numPrevId";
            this.numPrevId.Size = new System.Drawing.Size(53, 20);
            this.numPrevId.TabIndex = 18;
            // 
            // numGid
            // 
            this.numGid.Enabled = false;
            this.numGid.Location = new System.Drawing.Point(75, 29);
            this.numGid.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numGid.Name = "numGid";
            this.numGid.Size = new System.Drawing.Size(53, 20);
            this.numGid.TabIndex = 19;
            // 
            // numY1
            // 
            this.numY1.Location = new System.Drawing.Point(75, 220);
            this.numY1.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numY1.Minimum = new decimal(new int[] {
            99,
            0,
            0,
            -2147483648});
            this.numY1.Name = "numY1";
            this.numY1.Size = new System.Drawing.Size(53, 20);
            this.numY1.TabIndex = 20;
            this.numY1.ValueChanged += new System.EventHandler(this.numY1_ValueChanged);
            // 
            // numForce
            // 
            this.numForce.Location = new System.Drawing.Point(75, 246);
            this.numForce.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numForce.Name = "numForce";
            this.numForce.Size = new System.Drawing.Size(53, 20);
            this.numForce.TabIndex = 21;
            this.numForce.ValueChanged += new System.EventHandler(this.numForce_ValueChanged);
            // 
            // cmbCantPass
            // 
            this.cmbCantPass.FormattingEnabled = true;
            this.cmbCantPass.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cmbCantPass.Location = new System.Drawing.Point(75, 272);
            this.cmbCantPass.Name = "cmbCantPass";
            this.cmbCantPass.Size = new System.Drawing.Size(53, 21);
            this.cmbCantPass.TabIndex = 22;
            this.cmbCantPass.Text = "True";
            this.cmbCantPass.SelectedIndexChanged += new System.EventHandler(this.cmbCantPass_SelectedIndexChanged);
            // 
            // cmbCantDrop
            // 
            this.cmbCantDrop.FormattingEnabled = true;
            this.cmbCantDrop.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cmbCantDrop.Location = new System.Drawing.Point(75, 299);
            this.cmbCantDrop.Name = "cmbCantDrop";
            this.cmbCantDrop.Size = new System.Drawing.Size(53, 21);
            this.cmbCantDrop.TabIndex = 23;
            this.cmbCantDrop.Text = "True";
            this.cmbCantDrop.SelectedIndexChanged += new System.EventHandler(this.cmbCantDrop_SelectedIndexChanged);
            // 
            // cmbCantMove
            // 
            this.cmbCantMove.FormattingEnabled = true;
            this.cmbCantMove.Items.AddRange(new object[] {
            "True",
            "False"});
            this.cmbCantMove.Location = new System.Drawing.Point(75, 326);
            this.cmbCantMove.Name = "cmbCantMove";
            this.cmbCantMove.Size = new System.Drawing.Size(53, 21);
            this.cmbCantMove.TabIndex = 24;
            this.cmbCantMove.Text = "True";
            this.cmbCantMove.SelectedIndexChanged += new System.EventHandler(this.cmbCantMove_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(383, 415);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(302, 415);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 26;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtType);
            this.panel1.Controls.Add(this.lblType);
            this.panel1.Controls.Add(this.lblPrevId);
            this.panel1.Controls.Add(this.lblId);
            this.panel1.Controls.Add(this.lblGroupId);
            this.panel1.Controls.Add(this.cmbCantMove);
            this.panel1.Controls.Add(this.lblNextId);
            this.panel1.Controls.Add(this.cmbCantDrop);
            this.panel1.Controls.Add(this.lblX2);
            this.panel1.Controls.Add(this.cmbCantPass);
            this.panel1.Controls.Add(this.lblY2);
            this.panel1.Controls.Add(this.numForce);
            this.panel1.Controls.Add(this.lblX1);
            this.panel1.Controls.Add(this.numY1);
            this.panel1.Controls.Add(this.lblY1);
            this.panel1.Controls.Add(this.numGid);
            this.panel1.Controls.Add(this.lblForce);
            this.panel1.Controls.Add(this.numPrevId);
            this.panel1.Controls.Add(this.lblCantPass);
            this.panel1.Controls.Add(this.numX1);
            this.panel1.Controls.Add(this.lblCantDrop);
            this.panel1.Controls.Add(this.numNextId);
            this.panel1.Controls.Add(this.lblCantMove);
            this.panel1.Controls.Add(this.numX2);
            this.panel1.Controls.Add(this.numId);
            this.panel1.Controls.Add(this.numY2);
            this.panel1.Location = new System.Drawing.Point(258, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 397);
            this.panel1.TabIndex = 27;
            // 
            // txtType
            // 
            this.txtType.Enabled = false;
            this.txtType.Location = new System.Drawing.Point(75, 160);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(81, 20);
            this.txtType.TabIndex = 26;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(6, 164);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 25;
            this.lblType.Text = "Type:";
            // 
            // fpxHoldProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 448);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.dgvHolds);
            this.Name = "fpxHoldProperties";
            this.Text = "Hold Properties";
            ((System.ComponentModel.ISupportInitialize)(this.dgvHolds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNextId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrevId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numForce)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHolds;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblGroupId;
        private System.Windows.Forms.Label lblNextId;
        private System.Windows.Forms.Label lblX2;
        private System.Windows.Forms.Label lblY2;
        private System.Windows.Forms.Label lblPrevId;
        private System.Windows.Forms.Label lblX1;
        private System.Windows.Forms.Label lblY1;
        private System.Windows.Forms.Label lblForce;
        private System.Windows.Forms.Label lblCantPass;
        private System.Windows.Forms.Label lblCantDrop;
        private System.Windows.Forms.Label lblCantMove;
        private System.Windows.Forms.NumericUpDown numId;
        private System.Windows.Forms.NumericUpDown numY2;
        private System.Windows.Forms.NumericUpDown numX2;
        private System.Windows.Forms.NumericUpDown numNextId;
        private System.Windows.Forms.NumericUpDown numX1;
        private System.Windows.Forms.NumericUpDown numPrevId;
        private System.Windows.Forms.NumericUpDown numGid;
        private System.Windows.Forms.NumericUpDown numY1;
        private System.Windows.Forms.NumericUpDown numForce;
        private System.Windows.Forms.ComboBox cmbCantPass;
        private System.Windows.Forms.ComboBox cmbCantDrop;
        private System.Windows.Forms.ComboBox cmbCantMove;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtType;
        private System.Windows.Forms.Label lblType;
    }
}