namespace FaplesEditor
{
    partial class fpxMainEditor
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadObjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadTilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadUIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.mapEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uIEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.characterEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.footholdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seatholdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.climbholdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sceneryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.objectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.portalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsGameResolution = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editorToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1132, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripSeparator3,
            this.exportToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.newToolStripMenuItem.Text = "New...";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadObjectsToolStripMenuItem,
            this.loadTilesToolStripMenuItem,
            this.loadMapToolStripMenuItem,
            this.loadUIToolStripMenuItem,
            this.loadCharactersToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.loadToolStripMenuItem.Text = "Load...";
            // 
            // loadObjectsToolStripMenuItem
            // 
            this.loadObjectsToolStripMenuItem.Name = "loadObjectsToolStripMenuItem";
            this.loadObjectsToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.loadObjectsToolStripMenuItem.Text = "Load Objects...";
            this.loadObjectsToolStripMenuItem.Click += new System.EventHandler(this.loadObjectsToolStripMenuItem_Click);
            // 
            // loadTilesToolStripMenuItem
            // 
            this.loadTilesToolStripMenuItem.Name = "loadTilesToolStripMenuItem";
            this.loadTilesToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.loadTilesToolStripMenuItem.Text = "Load Tiles...";
            this.loadTilesToolStripMenuItem.Click += new System.EventHandler(this.loadTilesToolStripMenuItem_Click);
            // 
            // loadMapToolStripMenuItem
            // 
            this.loadMapToolStripMenuItem.Name = "loadMapToolStripMenuItem";
            this.loadMapToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.loadMapToolStripMenuItem.Text = "Load Map...";
            this.loadMapToolStripMenuItem.Click += new System.EventHandler(this.loadMapToolStripMenuItem_Click);
            // 
            // loadUIToolStripMenuItem
            // 
            this.loadUIToolStripMenuItem.Name = "loadUIToolStripMenuItem";
            this.loadUIToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.loadUIToolStripMenuItem.Text = "Load UI...";
            this.loadUIToolStripMenuItem.Click += new System.EventHandler(this.loadUIToolStripMenuItem_Click);
            // 
            // loadCharactersToolStripMenuItem
            // 
            this.loadCharactersToolStripMenuItem.Name = "loadCharactersToolStripMenuItem";
            this.loadCharactersToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.loadCharactersToolStripMenuItem.Text = "Load Characters...";
            this.loadCharactersToolStripMenuItem.Click += new System.EventHandler(this.loadCharactersToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.saveToolStripMenuItem.Text = "Save...";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(114, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(114, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.exportToolStripMenuItem.Text = "Export...";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(114, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editorToolStripMenuItem
            // 
            this.editorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.objectEditorToolStripMenuItem,
            this.tileEditorToolStripMenuItem,
            this.toolStripSeparator4,
            this.mapEditorToolStripMenuItem,
            this.uIEditorToolStripMenuItem,
            this.characterEditorToolStripMenuItem});
            this.editorToolStripMenuItem.Name = "editorToolStripMenuItem";
            this.editorToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.editorToolStripMenuItem.Text = "Editor";
            // 
            // objectEditorToolStripMenuItem
            // 
            this.objectEditorToolStripMenuItem.Name = "objectEditorToolStripMenuItem";
            this.objectEditorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.objectEditorToolStripMenuItem.Text = "Object Editor";
            this.objectEditorToolStripMenuItem.Click += new System.EventHandler(this.objectEditorToolStripMenuItem_Click);
            // 
            // tileEditorToolStripMenuItem
            // 
            this.tileEditorToolStripMenuItem.Name = "tileEditorToolStripMenuItem";
            this.tileEditorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.tileEditorToolStripMenuItem.Text = "Tile Editor";
            this.tileEditorToolStripMenuItem.Click += new System.EventHandler(this.tileEditorToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(156, 6);
            // 
            // mapEditorToolStripMenuItem
            // 
            this.mapEditorToolStripMenuItem.Name = "mapEditorToolStripMenuItem";
            this.mapEditorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.mapEditorToolStripMenuItem.Text = "Map Editor";
            this.mapEditorToolStripMenuItem.Click += new System.EventHandler(this.mapEditorToolStripMenuItem_Click);
            // 
            // uIEditorToolStripMenuItem
            // 
            this.uIEditorToolStripMenuItem.Name = "uIEditorToolStripMenuItem";
            this.uIEditorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.uIEditorToolStripMenuItem.Text = "UI Editor";
            this.uIEditorToolStripMenuItem.Click += new System.EventHandler(this.uIEditorToolStripMenuItem_Click);
            // 
            // characterEditorToolStripMenuItem
            // 
            this.characterEditorToolStripMenuItem.Name = "characterEditorToolStripMenuItem";
            this.characterEditorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.characterEditorToolStripMenuItem.Text = "Character Editor";
            this.characterEditorToolStripMenuItem.Click += new System.EventHandler(this.characterEditorToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.footholdToolStripMenuItem,
            this.seatholdToolStripMenuItem,
            this.climbholdToolStripMenuItem,
            this.sceneryToolStripMenuItem,
            this.tileToolStripMenuItem,
            this.objectToolStripMenuItem,
            this.portalToolStripMenuItem,
            this.controlToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // footholdToolStripMenuItem
            // 
            this.footholdToolStripMenuItem.Checked = true;
            this.footholdToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.footholdToolStripMenuItem.Name = "footholdToolStripMenuItem";
            this.footholdToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.footholdToolStripMenuItem.Text = "Foothold";
            this.footholdToolStripMenuItem.Click += new System.EventHandler(this.footholdToolStripMenuItem_Click);
            // 
            // seatholdToolStripMenuItem
            // 
            this.seatholdToolStripMenuItem.Checked = true;
            this.seatholdToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.seatholdToolStripMenuItem.Name = "seatholdToolStripMenuItem";
            this.seatholdToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.seatholdToolStripMenuItem.Text = "Seathold";
            this.seatholdToolStripMenuItem.Click += new System.EventHandler(this.seatholdToolStripMenuItem_Click);
            // 
            // climbholdToolStripMenuItem
            // 
            this.climbholdToolStripMenuItem.Checked = true;
            this.climbholdToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.climbholdToolStripMenuItem.Name = "climbholdToolStripMenuItem";
            this.climbholdToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.climbholdToolStripMenuItem.Text = "Climbhold";
            this.climbholdToolStripMenuItem.Click += new System.EventHandler(this.climbholdToolStripMenuItem_Click);
            // 
            // sceneryToolStripMenuItem
            // 
            this.sceneryToolStripMenuItem.Checked = true;
            this.sceneryToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sceneryToolStripMenuItem.Name = "sceneryToolStripMenuItem";
            this.sceneryToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.sceneryToolStripMenuItem.Text = "Scenery";
            this.sceneryToolStripMenuItem.Click += new System.EventHandler(this.sceneryToolStripMenuItem_Click);
            // 
            // tileToolStripMenuItem
            // 
            this.tileToolStripMenuItem.Checked = true;
            this.tileToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
            this.tileToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.tileToolStripMenuItem.Text = "Tile";
            this.tileToolStripMenuItem.Click += new System.EventHandler(this.tileToolStripMenuItem_Click);
            // 
            // objectToolStripMenuItem
            // 
            this.objectToolStripMenuItem.Checked = true;
            this.objectToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.objectToolStripMenuItem.Name = "objectToolStripMenuItem";
            this.objectToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.objectToolStripMenuItem.Text = "Object";
            this.objectToolStripMenuItem.Click += new System.EventHandler(this.objectToolStripMenuItem_Click);
            // 
            // portalToolStripMenuItem
            // 
            this.portalToolStripMenuItem.Checked = true;
            this.portalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.portalToolStripMenuItem.Name = "portalToolStripMenuItem";
            this.portalToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.portalToolStripMenuItem.Text = "Portal";
            this.portalToolStripMenuItem.Click += new System.EventHandler(this.portalToolStripMenuItem_Click);
            // 
            // controlToolStripMenuItem
            // 
            this.controlToolStripMenuItem.Checked = true;
            this.controlToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.controlToolStripMenuItem.Name = "controlToolStripMenuItem";
            this.controlToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.controlToolStripMenuItem.Text = "Control";
            this.controlToolStripMenuItem.Click += new System.EventHandler(this.controlToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsGameResolution});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // tsGameResolution
            // 
            this.tsGameResolution.Name = "tsGameResolution";
            this.tsGameResolution.Size = new System.Drawing.Size(180, 22);
            this.tsGameResolution.Text = "Game Resolution";
            this.tsGameResolution.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsGameResolution_DropDownItemClicked);
            // 
            // fpxMainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 748);
            this.Controls.Add(this.menuStrip);
            this.MinimumSize = new System.Drawing.Size(960, 640);
            this.Name = "fpxMainEditor";
            this.Text = "fpxMainEditor";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem footholdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seatholdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem climbholdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sceneryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem objectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem portalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadObjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadTilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem uIEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem controlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadUIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem characterEditorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadCharactersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsGameResolution;
    }
}