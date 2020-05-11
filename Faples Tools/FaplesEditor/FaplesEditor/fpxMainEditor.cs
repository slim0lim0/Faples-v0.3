using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace FaplesEditor
{
    enum eEditorMode
    {
        eMapEditor,
        eTileEditor,
        eObjectEditor,
        eCharacterEditor,
        eUIEditor
    }

    public partial class fpxMainEditor : Form
    {
        #region Declarations

        private eEditorMode gEditorMode = eEditorMode.eMapEditor;


        private fpxMapEditor gMapEditor;
        private fpxTileEditor gTileEditor;
        private fpxObjectEditor gObjectEditor;
        private fpxUIEditor gUIEditor;
        private fpxCharacterEditor gCharacterEditor;

        #endregion

        #region Constructor
        public fpxMainEditor()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Main Methods
        public void Prepare()
        {
            // Load Editors
            LoadEditors();

            // Prepare Resources
            PrepareResources();

            PrepareSettings();

            // Set default start view
            gEditorMode = eEditorMode.eMapEditor;
            ManageControls();
            mapEditorToolStripMenuItem.Checked = true;
        }

        public void PrepareSettings()
        {
            var oToolStrip480x272 = new ToolStripMenuItem();
            oToolStrip480x272.Name = "tsRes480x272";
            oToolStrip480x272.Text = "480x272";

            var oToolStrip640x360 = new ToolStripMenuItem();
            oToolStrip640x360.Name = "tsRes640x360";
            oToolStrip640x360.Text = "640x360";

            var oToolStrip848x480 = new ToolStripMenuItem();
            oToolStrip848x480.Name = "tsRes848x480";
            oToolStrip848x480.Text = "848x480";

            var oToolStrip854x480 = new ToolStripMenuItem();
            oToolStrip854x480.Name = "tsRes854x480";
            oToolStrip854x480.Text = "854x480";

            var oToolStrip960x540 = new ToolStripMenuItem();
            oToolStrip960x540.Name = "tsRes960x540";
            oToolStrip960x540.Text = "960x540";

            var oToolStrip960x544 = new ToolStripMenuItem();
            oToolStrip960x544.Name = "tsRes960x544";
            oToolStrip960x544.Text = "960x544";

            var oToolStrip1024x576 = new ToolStripMenuItem();
            oToolStrip1024x576.Name = "tsRes1024x576";
            oToolStrip1024x576.Text = "1024x576";

            var oToolStrip1024x600 = new ToolStripMenuItem();
            oToolStrip1024x600.Name = "tsRes1024x600";
            oToolStrip1024x600.Text = "1024x600";

            var oToolStrip1136x600 = new ToolStripMenuItem();
            oToolStrip1136x600.Name = "tsRes1136x600";
            oToolStrip1136x600.Text = "1136x600";

            var oToolStrip1280x720 = new ToolStripMenuItem();
            oToolStrip1280x720.Name = "tsRes1280x720";
            oToolStrip1280x720.Text = "1280x720";

            var oToolStrip1334x750 = new ToolStripMenuItem();
            oToolStrip1334x750.Name = "tsRes1334x750";
            oToolStrip1334x750.Text = "1334x750";

            var oToolStrip1366x768 = new ToolStripMenuItem();
            oToolStrip1366x768.Name = "tsRes1366x768";
            oToolStrip1366x768.Text = "1366x768";

            var oToolStrip1600x900 = new ToolStripMenuItem();
            oToolStrip1600x900.Name = "tsRes1600x900";
            oToolStrip1600x900.Text = "1600x900";

            var oToolStrip1776x1000 = new ToolStripMenuItem();
            oToolStrip1776x1000.Name = "tsRes1776x1000";
            oToolStrip1776x1000.Text = "1776x1000";

            var oToolStrip1920x1080 = new ToolStripMenuItem();
            oToolStrip1920x1080.Name = "tsRes1920x1080";
            oToolStrip1920x1080.Text = "1920x1080";
            oToolStrip1920x1080.Checked = true;

            var oToolStrip2048x1152 = new ToolStripMenuItem();
            oToolStrip2048x1152.Name = "tsRes2048x1152";
            oToolStrip2048x1152.Text = "2048x1152";

            var oToolStrip2560x1440 = new ToolStripMenuItem();
            oToolStrip2560x1440.Name = "tsRes2560x1440";
            oToolStrip2560x1440.Text = "2560x1440";

            var oToolStrip3200x1800 = new ToolStripMenuItem();
            oToolStrip3200x1800.Name = "tsRes3200x1800";
            oToolStrip3200x1800.Text = "3200x1800";

            var oToolStrip3840x2160 = new ToolStripMenuItem();
            oToolStrip3840x2160.Name = "tsRes3840x2160";
            oToolStrip3840x2160.Text = "3840x2160";

            tsGameResolution.DropDownItems.Add(oToolStrip480x272);
            tsGameResolution.DropDownItems.Add(oToolStrip640x360);
            tsGameResolution.DropDownItems.Add(oToolStrip848x480);
            tsGameResolution.DropDownItems.Add(oToolStrip854x480);
            tsGameResolution.DropDownItems.Add(oToolStrip960x540);
            tsGameResolution.DropDownItems.Add(oToolStrip960x544);
            tsGameResolution.DropDownItems.Add(oToolStrip1024x576);
            tsGameResolution.DropDownItems.Add(oToolStrip1024x600);
            tsGameResolution.DropDownItems.Add(oToolStrip1136x600);
            tsGameResolution.DropDownItems.Add(oToolStrip1280x720);
            tsGameResolution.DropDownItems.Add(oToolStrip1334x750);
            tsGameResolution.DropDownItems.Add(oToolStrip1366x768);
            tsGameResolution.DropDownItems.Add(oToolStrip1600x900);
            tsGameResolution.DropDownItems.Add(oToolStrip1776x1000);
            tsGameResolution.DropDownItems.Add(oToolStrip1920x1080);
            tsGameResolution.DropDownItems.Add(oToolStrip2048x1152);
            tsGameResolution.DropDownItems.Add(oToolStrip2560x1440);
            tsGameResolution.DropDownItems.Add(oToolStrip3200x1800);
            tsGameResolution.DropDownItems.Add(oToolStrip3840x2160);
        }

        public void PrepareResources()
        {
            // Load Fonts
            Utility.LoadAllFonts();
        }

        public void LoadEditors()
        {
            // Load Map Editor
            gMapEditor = new fpxMapEditor();
            this.Controls.Add(gMapEditor);
            gMapEditor.BringToFront();
            gMapEditor.Location = new Point(50, 50);
            gMapEditor.Enabled = true;
            gMapEditor.Dock = DockStyle.Fill;
            gMapEditor.Visible = false;


            // Load Object Editor
            gObjectEditor = new fpxObjectEditor();
            this.Controls.Add(gObjectEditor);
            gObjectEditor.BringToFront();
            gObjectEditor.Location = new Point(50, 50);
            gObjectEditor.Enabled = true;
            gObjectEditor.Dock = DockStyle.Fill;
            gObjectEditor.Visible = false;



            // Load Tile Editor
            gTileEditor = new fpxTileEditor();
            this.Controls.Add(gTileEditor);
            gTileEditor.BringToFront();
            gTileEditor.Location = new Point(50, 50);
            gTileEditor.Enabled = true;
            gTileEditor.Dock = DockStyle.Fill;
            gTileEditor.Visible = false;


            // Load UI Editor
            gUIEditor = new fpxUIEditor();
            this.Controls.Add(gUIEditor);
            gUIEditor.BringToFront();
            gUIEditor.Location = new Point(50, 50);
            gUIEditor.Enabled = true;
            gUIEditor.Dock = DockStyle.Fill;
            gUIEditor.Visible = false;

            gCharacterEditor = new fpxCharacterEditor();
            this.Controls.Add(gCharacterEditor);
            gCharacterEditor.BringToFront();
            gCharacterEditor.Location = new Point(50, 50);
            gCharacterEditor.Enabled = true;
            gCharacterEditor.Dock = DockStyle.Fill;
            gCharacterEditor.Visible = false;
        }

        public void ManageControls()
        {
            switch (gEditorMode)
            {
                case eEditorMode.eMapEditor:
                    SetMode(true, false, false, false, false);
                    break;
                case eEditorMode.eTileEditor:
                    SetMode(false, true, false, false, false);
                    break;
                case eEditorMode.eObjectEditor:
                    SetMode(false, false, true, false, false);
                    break;
                case eEditorMode.eUIEditor:
                    SetMode(false, false, false, true, false);
                    break;
                case eEditorMode.eCharacterEditor:
                    SetMode(false, false, false, false, true);
                    break;
                default:
                    break;
            }
        }

        public void SetMode(bool bMap, bool bTile, bool bObject, bool bUI, bool bPlayer)
        {
            gMapEditor.Visible = bMap;
            gTileEditor.Visible = bTile;
            gObjectEditor.Visible = bObject;
            gUIEditor.Visible = bUI;
            gCharacterEditor.Visible = bPlayer;
        }

        public void ResetMode()
        {
            mapEditorToolStripMenuItem.Checked = false;
            tileEditorToolStripMenuItem.Checked = false;
            objectEditorToolStripMenuItem.Checked = false;
            uIEditorToolStripMenuItem.Checked = false;
            characterEditorToolStripMenuItem.Checked = false;
        }

        #endregion

        #region Event Handlers

        private void objectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objectToolStripMenuItem.Checked = !objectToolStripMenuItem.Checked;

            gMapEditor.ObjectView = objectToolStripMenuItem.Checked;
            gUIEditor.ObjectView = objectToolStripMenuItem.Checked;
        }

        private void portalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            portalToolStripMenuItem.Checked = !portalToolStripMenuItem.Checked;

            gMapEditor.PortalView = portalToolStripMenuItem.Checked;
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tileToolStripMenuItem.Checked = !tileToolStripMenuItem.Checked;

            gMapEditor.TileView = tileToolStripMenuItem.Checked;
            gUIEditor.TileView = tileToolStripMenuItem.Checked;
        }

        private void sceneryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sceneryToolStripMenuItem.Checked = !sceneryToolStripMenuItem.Checked;

            gMapEditor.SceneryView = sceneryToolStripMenuItem.Checked;
            gUIEditor.SceneryView = sceneryToolStripMenuItem.Checked;
        }

        private void climbholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            climbholdToolStripMenuItem.Checked = !climbholdToolStripMenuItem.Checked;

            gMapEditor.HoldView = climbholdToolStripMenuItem.Checked;
        }

        private void seatholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            seatholdToolStripMenuItem.Checked = !seatholdToolStripMenuItem.Checked;

            gMapEditor.HoldView = seatholdToolStripMenuItem.Checked;
        }

        private void footholdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            footholdToolStripMenuItem.Checked = !footholdToolStripMenuItem.Checked;

            gMapEditor.HoldView = footholdToolStripMenuItem.Checked;
        }
        private void controlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlToolStripMenuItem.Checked = !controlToolStripMenuItem.Checked;

            gUIEditor.ControlView = controlToolStripMenuItem.Checked;
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(opf.FileName);

                    if (xDoc.DocumentElement.Name == "fpxMaps")
                    {
                        EditorManager.LoadMaps(xDoc);

                        gMapEditor.Reload();

                        bValid = true;

                        ManageControls();
                    }
                    else
                    {
                        MessageBox.Show("Not a valid Object file. Please validate your Object file.", "Load Object", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    return;
            }
        }
        private void loadUIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(opf.FileName);

                    if (xDoc.DocumentElement.Name == "fpxUI")
                    {
                        EditorManager.LoadUI(xDoc);

                        gUIEditor.Reload();

                        bValid = true;

                        ManageControls();
                    }
                    else
                    {
                        MessageBox.Show("Not a valid Object file. Please validate your Object file.", "Load Object", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    return;
            }
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EXPORT_FILES_PATH);

            FPXFormat.Export(sDirectory);
        }

        private void mapEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetMode();
            mapEditorToolStripMenuItem.Checked = true;
            gEditorMode = eEditorMode.eMapEditor;
            ManageControls();
        }

        private void tileEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetMode();
            tileEditorToolStripMenuItem.Checked = true;
            gEditorMode = eEditorMode.eTileEditor;
            ManageControls();
        }

        private void objectEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetMode();
            objectEditorToolStripMenuItem.Checked = true;
            gEditorMode = eEditorMode.eObjectEditor;
            ManageControls();
        }
        private void uIEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetMode();
            uIEditorToolStripMenuItem.Checked = true;
            gEditorMode = eEditorMode.eUIEditor;
            ManageControls();
        }
        private void characterEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetMode();
            characterEditorToolStripMenuItem.Checked = true;
            gEditorMode = eEditorMode.eCharacterEditor;
            ManageControls();
        }
        private void loadObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(opf.FileName);

                    if (xDoc.DocumentElement.Name == "fpxObjects")
                    {
                        EditorManager.LoadObjects(xDoc);

                        gObjectEditor.Reload();

                        bValid = true;

                        gMapEditor.ReloadObjects();
                        gUIEditor.ReloadObjects();
                        ManageControls();
                    }
                    else
                    {
                        MessageBox.Show("Not a valid Object file. Please validate your Object file.", "Load Object", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    return;
            }
        }
        private void loadTilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while(!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(opf.FileName);

                    if(xDoc.DocumentElement.Name == "fpxTiles")
                    {
                        EditorManager.LoadTiles(xDoc);

                        gTileEditor.Reload();

                        bValid = true;

                        gMapEditor.ReloadTiles();
                        gUIEditor.ReloadTiles();
                        ManageControls();
                    }
                    else
                    {
                        MessageBox.Show("Not a valid Object file. Please validate your Object file.", "Load Object", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    return;
            }
        }
        private void loadCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(opf.FileName);

                    if (xDoc.DocumentElement.Name == "fpxCharacters")
                    {
                        EditorManager.LoadCharacters(xDoc);

                        gCharacterEditor.Reload();

                        bValid = true;
                        ManageControls();
                    }
                    else
                    {
                        MessageBox.Show("Not a valid Characters file. Please validate your Characters file.", "Load Characters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                    return;
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gEditorMode == eEditorMode.eObjectEditor)
            {
                SaveFileDialog opf = new SaveFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() == DialogResult.OK)
                {
                    EditorManager.GetObjectCollection().Save(opf.FileName);
                }
            }
            else if (gEditorMode == eEditorMode.eTileEditor)
            {
                SaveFileDialog opf = new SaveFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() == DialogResult.OK)
                {
                    EditorManager.GetTileCollection().Save(opf.FileName);
                }

            }
            else if(gEditorMode == eEditorMode.eMapEditor)
            {
                SaveFileDialog opf = new SaveFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() == DialogResult.OK)
                {
                    EditorManager.GetMapCollection().Save(opf.FileName);
                }
            }
            else if(gEditorMode == eEditorMode.eUIEditor)
            {
                SaveFileDialog opf = new SaveFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() == DialogResult.OK)
                {
                    EditorManager.GetUICollection().Save(opf.FileName);
                }
            }
            else if(gEditorMode == eEditorMode.eCharacterEditor)
            {
                SaveFileDialog opf = new SaveFileDialog();
                opf.Filter = "XML files(*.xml)| *.xml";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.EDITOR_FILES_PATH);

                if (opf.ShowDialog() == DialogResult.OK)
                {
                    EditorManager.GetCharacterCollection().Save(opf.FileName);
                }
            }
        }
        #endregion

        #region Test Methods
        //private void ExportToFpx()
        //{
        //    string[] strings = File.ReadAllLines("Resources\\John.txt");

        //    Structures.pers oPers = StructureTools.stringArrayToPersFile(strings);

        //    byte[] testfile = StructureTools.persToBytes(oPers);
        //    File.WriteAllBytes("test.pers", testfile);

        //    MessageBox.Show("Person has CONVERTED successfully!");
        //}

        //private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Structures.pers newP = StructureTools.bytesToPers(File.ReadAllBytes("test2.pers"));

        //    File.WriteAllBytes("test2.pers", StructureTools.persToBytes(newP));

        //    MessageBox.Show("Person has LOADED successfully!");
        //}

        //private void ediMap_Paint(object sender, PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    Image image = Image.FromFile("Resources\\map01.png");
        //    Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);
        //    ediMap.Size = new Size(image.Width, image.Height);
        //    g.DrawImage(image, rect);
        //    image.Dispose();
        //}

        #endregion

        private void tsGameResolution_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem oItem in tsGameResolution.DropDownItems)
            {
                oItem.Checked = false;

                if (oItem.Selected)
                {
                    Utility.SetGameResolution(oItem.Text);
                    oItem.Checked = true;
                }
            } 
        }
    }
}
