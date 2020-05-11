using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace FaplesEditor
{
    public partial class fpxTileEditor : UserControl
    {
        #region Declarations
        XmlDocument gTiles = null;
        List<fpxTile> gTileList = null;
        Bitmap gCurrSheet = null;
        Bitmap gCurrTile = null;

        Dictionary<int, List<fpxHold>> gHolds = new Dictionary<int, List<fpxHold>>();
        int iHoldGroup = 0;
        int iPointX = 0;
        int iPointY = 0;
        #endregion

        #region Properties
        public bool Dirty { get; set; } = false;
        #endregion

        #region Constructor
        public fpxTileEditor()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Event Handlers
        private void btnAddSheet_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.TILES_PATH);
                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.TILES_PATH))
                    {
                        MessageBox.Show("Please select image within 'Tiles' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    string sName = opf.SafeFileName;
                    XmlNode oTiles = gTiles.SelectSingleNode("//fpxTiles");
                    if (Utility.XmlNodeExists(oTiles, sName))
                    {
                        try
                        {
                            XmlAttribute oName = gTiles.CreateAttribute("Name");
                            oName.Value = sName;

                            XmlNode oSheet = gTiles.CreateNode("element", "Sheet", "");
                            oSheet.Attributes.Append(oName);
                            oTiles.AppendChild(oSheet);
                            gTiles.ReplaceChild(oTiles, oTiles);
                            bValid = true;
                            cmbSheet.Items.Add(sName);

                            if (cmbSheet.SelectedIndex < 0)
                            {
                                int iIndex = Math.Max(cmbSheet.Items.Count - 1, 0);

                                cmbSheet.SelectedIndex = iIndex;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inserting Collection. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Collection already exists. Please use a different name.");
                    }
                }
                else
                    return;
            }

            ValidateSave();
            ManageControls();
        }

        private void btnAddTile_Click(object sender, EventArgs e)
        {
            bool bValid = false;
            var oAddDialog = new fpxAddDialog();

            while (!bValid)
            {
                oAddDialog.ShowDialog();

                if (oAddDialog.DialogResult == DialogResult.Cancel)
                    return;

                if (oAddDialog.DialogResult == DialogResult.OK)
                {
                    string sName = oAddDialog.FileName;
                    XmlNode oCollections = gTiles.SelectSingleNode("//fpxTiles");
                    XmlNode oSheet = null;

                    foreach (XmlNode oNode in oCollections.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                        {
                            oSheet = oNode;
                            break;
                        }
                    }

                    int iInc = 0;
                    string sIncCheck = sName + "/" + iInc;

                    while (!Utility.XmlNodeExists(oSheet, sIncCheck))
                    {
                        iInc++;
                        sIncCheck = sName + "/" + iInc;
                    }

                    sName = sName + "/" + iInc;

                    if (Utility.XmlNodeExists(oSheet, sName))
                    {
                        try
                        {

                            fpxTile oTile = new fpxTile();
                            oTile.Name = sName;
                            oTile.SpriteSheet = oSheet.Attributes["Name"].Value;

                            XmlAttribute oName = gTiles.CreateAttribute("Name");
                            oName.Value = oTile.Name;
                            XmlAttribute oX = gTiles.CreateAttribute("x");
                            oX.Value = oTile.x.ToString();
                            XmlAttribute oY = gTiles.CreateAttribute("y");
                            oY.Value = oTile.y.ToString();
                            XmlAttribute oWidth = gTiles.CreateAttribute("width");
                            oWidth.Value = oTile.width.ToString();
                            XmlAttribute oHeight = gTiles.CreateAttribute("height");
                            oHeight.Value = oTile.height.ToString();
                            XmlNode oTileN = gTiles.CreateNode("element", "Tile", "");
                            oTileN.Attributes.Append(oName);
                            oTileN.Attributes.Append(oX);
                            oTileN.Attributes.Append(oY);
                            oTileN.Attributes.Append(oWidth);
                            oTileN.Attributes.Append(oHeight);

                            oSheet.AppendChild(oTileN);
                            oCollections.ReplaceChild(oSheet, oSheet);
                            bValid = true;

                            gTileList.Add(oTile);

                            dgvTiles.Rows.Add(new DataGridViewRow());
                            DataGridViewRow oRow = dgvTiles.Rows[dgvTiles.Rows.Count - 1];
                            oRow.MinimumHeight = 96;
                            oRow.Cells[0].Value = new Bitmap(1, 1);
                            oRow.Cells[1].Value = sName;

                            if (dgvTiles.SelectedRows.Count < 1)
                            {
                                int iIndex = Math.Max(dgvTiles.Rows.Count - 1, 0);

                                dgvTiles.Rows[iIndex].Selected = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inserting Object. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Object already exists. Please use a different name.");
                    }
                }
            }

            oAddDialog.Close();
            ValidateSave();
            ManageControls();
        }

        private void btnDeleteSheet_Click(object sender, EventArgs e)
        {
            ClearTiles();

            string sName = cmbSheet.SelectedItem.ToString();
            XmlNode oCollections = gTiles.SelectSingleNode("//fpxTiles");

            if (!Utility.XmlNodeExists(oCollections, sName))
            {
                foreach (XmlNode oNode in oCollections)
                {
                    if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                    {
                        oCollections.RemoveChild(oNode);
                        gTiles.ReplaceChild(oCollections, oCollections);
                        cmbSheet.Items.Remove(cmbSheet.SelectedItem);
                        break;
                    }
                }
            }

            if (cmbSheet.Items.Count > 0)
                cmbSheet.SelectedIndex = 0;
            else
                cmbSheet.SelectedIndex = -1;

            ValidateSave();
            ManageControls();
        }

        private void btnDeleteTile_Click(object sender, EventArgs e)
        {
            ClearObjectProperties();

            string sName = dgvTiles.SelectedRows[0].Cells[1].Value.ToString();
            XmlNode oCollections = gTiles.SelectSingleNode("//fpxTiles");
            XmlNode oSheet = null;

            foreach (XmlNode oNode in oCollections.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                {
                    oSheet = oNode;
                    break;
                }
            }

            if (!Utility.XmlNodeExists(oSheet, sName))
            {
                foreach (XmlNode oNode in oSheet)
                {
                    if (oNode.Attributes["Name"].Value.Equals(dgvTiles.SelectedRows[0].Cells[1].Value))
                    {
                        oSheet.RemoveChild(oNode);
                        oCollections.ReplaceChild(oSheet, oSheet);
                        gTiles.ReplaceChild(oCollections, oCollections);
                        dgvTiles.Rows.Remove(dgvTiles.SelectedRows[0]);
                        break;
                    }
                }

                if (dgvTiles.Rows.Count > 0)
                {
                    dgvTiles.Rows[0].Selected = true;

                    fpxTile oObj = gTileList[0];

                    Point oSpriteLocation = new Point(oObj.x, oObj.y);
                    Size oSpriteSize = new Size(oObj.width, oObj.height);
                    Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);
                    Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                    if (gCurrTile != null)
                        gCurrTile.Dispose();

                    gCurrTile = spriteFull;

                    numX.Value = oObj.x;
                    numY.Value = oObj.y;
                    numWidth.Value = oObj.width;
                    numHeight.Value = oObj.height;
                }
                else
                {
                    if (gCurrTile != null)
                    {
                        gCurrTile.Dispose();
                        gCurrTile = null;
                    }
                }
            }

            ValidateSave();
            ManageControls();
        }

        private void cmbSheet_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearTiles();

            if (cmbSheet.SelectedIndex == -1)
                cmbSheet.Items.Clear();

            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            if (gCurrTile != null)
                gCurrTile.Dispose();

            gCurrSheet = new Bitmap(Path.Combine(Utility.TILES_PATH, cmbSheet.SelectedItem.ToString()));
            gTileList = new List<fpxTile>();
            gCurrTile = null;

            foreach (XmlNode oSheet in gTiles.SelectSingleNode("//fpxTiles").ChildNodes)
            {
                if (oSheet.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                {
                    dgvTiles.Rows.Clear();

                    foreach (XmlNode oTile in oSheet.ChildNodes)
                    {
                        fpxTile oT = new fpxTile();
                        oT.Name = oTile.Attributes["Name"].Value;
                        oT.SpriteSheet = oSheet.Attributes["Name"].Value;
                        oT.x = int.Parse(oTile.Attributes["x"].Value);
                        oT.y = int.Parse(oTile.Attributes["y"].Value);
                        oT.width = int.Parse(oTile.Attributes["width"].Value);
                        oT.height = int.Parse(oTile.Attributes["height"].Value);

                        XmlNode oHolds = oTile.LastChild;

                        Dictionary<int, List<fpxHold>> colGroups = new Dictionary<int, List<fpxHold>>();
                        foreach (XmlNode oGroupHold in oHolds)
                        {
                            List<fpxHold> colHolds = new List<fpxHold>();
                            foreach (XmlNode oHold in oGroupHold)
                            {
                                fpxHold hold = new fpxHold();
                                hold.id = int.Parse(oHold.Attributes["id"].Value);
                                hold.gid = int.Parse(oHold.Attributes["gid"].Value);
                                hold.lid = int.Parse(oHold.Attributes["lid"].Value);
                                hold.x1 = int.Parse(oHold.Attributes["x1"].Value);
                                hold.y1 = int.Parse(oHold.Attributes["y1"].Value);
                                hold.x2 = int.Parse(oHold.Attributes["x2"].Value);
                                hold.y2 = int.Parse(oHold.Attributes["y2"].Value);
                                hold.nextid = int.Parse(oHold.Attributes["nextid"].Value);
                                hold.previd = int.Parse(oHold.Attributes["previd"].Value);
                                hold.type = oHold.Attributes["type"].Value;
                                hold.force = int.Parse(oHold.Attributes["force"].Value);
                                hold.cantPass = bool.Parse(oHold.Attributes["cantPass"].Value);
                                hold.cantDrop = bool.Parse(oHold.Attributes["cantDrop"].Value);
                                hold.cantMove = bool.Parse(oHold.Attributes["cantMove"].Value);
                                colHolds.Add(hold);
                            }

                            for (int i = 0; i < colHolds.Count; i++)
                            {
                                fpxHold currH = colHolds.ElementAtOrDefault(i);
                                fpxHold prevH = colHolds.ElementAtOrDefault(i - 1);

                                if (prevH != null)
                                {
                                    prevH.next = currH;
                                    prevH.nextid = currH.id;
                                    currH.prev = prevH;
                                    currH.previd = prevH.id;

                                    colHolds[i] = currH;
                                    colHolds[i - 1] = prevH;
                                }
                            }

                            colGroups.Add(int.Parse(oGroupHold.Attributes["id"].Value), colHolds);
                        }
                        oT.HoldGroups = colGroups;

                        gTileList.Add(oT);

                        Point oSpriteLocation = new Point(oT.x, oT.y);
                        Size oSpriteSize = new Size(oT.width, oT.height);
                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                        Bitmap spriteSnap = Utility.GetSprite(gCurrSheet, oCrop);

                        int iSmallWidth = oSpriteSize.Width;
                        int iSmallHeight = oSpriteSize.Height;

                        decimal dWidth = iSmallWidth;
                        decimal dHeight = iSmallHeight;


                        decimal ratioX = Math.Max((dWidth / dHeight), 0.1m);
                        decimal ratioY = Math.Max((dHeight / dWidth), 0.1m);

                        if (ratioX > ratioY)
                        {
                            ratioY = 64;
                            ratioX = 64 * ratioX;

                        }
                        else if (ratioX < ratioY)
                        {
                            ratioY = 64 * ratioY;
                            ratioX = 64;
                        }
                        else
                        {
                            ratioY = 64;
                            ratioX = 64;
                        }

                        if (iSmallWidth > 64)
                        {
                            iSmallWidth = decimal.ToInt32(ratioX);

                            if (iSmallWidth > 128)
                                iSmallWidth = 128;
                        }


                        if (iSmallHeight > 64)
                        {
                            iSmallHeight = decimal.ToInt32(ratioY);
                            if (iSmallHeight > 128)
                                iSmallHeight = 128;
                        }

                        spriteSnap = Utility.ResizeImage(spriteSnap, iSmallWidth, iSmallHeight);

                        dgvTiles.Rows.Add(new DataGridViewRow());
                        DataGridViewRow oRow = dgvTiles.Rows[dgvTiles.Rows.Count - 1];
                        oRow.MinimumHeight = 96;
                        oRow.Cells[0].Value = spriteSnap;
                        oRow.Cells[1].Value = oT.Name;

                    }

                    if (dgvTiles.Rows.Count > 0)
                    {
                        dgvTiles.Rows[0].Selected = true;
                        fpxTile oT = gTileList[0];

                        Point oSpriteLocation = new Point(oT.x, oT.y);
                        Size oSpriteSize = new Size(oT.width, oT.height);
                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);
                        Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                        if (gCurrTile != null)
                            gCurrTile.Dispose();

                        gCurrTile = spriteFull;

                        numX.Value = oT.x;
                        numY.Value = oT.y;
                        numWidth.Value = oT.width;
                        numHeight.Value = oT.height;

                        gHolds = new Dictionary<int, List<fpxHold>>(oT.HoldGroups);
                        tsbHolds.Items.Clear();

                        for (int i = 0; i < gHolds.Count; i++)
                        {
                            List<fpxHold> holds = gHolds[i];
                            iHoldGroup++;

                            switch (holds[0].type)
                            {
                                case "foot":
                                    tsbHolds.Items.Add("foot" + i);
                                    break;
                                case "climb":
                                    tsbHolds.Items.Add("climb" + i);
                                    break;
                                case "seat":
                                    tsbHolds.Items.Add("seat" + i);
                                    break;
                                default:
                                    tsbHolds.Items.Add("foot" + i);
                                    break;
                            }
                        }

                        if (tsbHolds.Items.Count > 0)
                            tsbHolds.SelectedIndex = 0;
                    }
                    else
                    {
                        numX.Value = numX.Minimum;
                        numY.Value = numY.Minimum;
                        numWidth.Value = numWidth.Minimum;
                        numHeight.Value = numHeight.Minimum;
                    }
                }
            }

            ManageControls();
        }

        private void dgvTiles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTiles.SelectedRows.Count > 0)
            {
                tsbHolds.Enabled = true;
                tsbHolds.Items.Clear();
                tsbHolds.SelectedItem = null;
                tsbHolds.Enabled = false;
                iHoldGroup = 0;
                gHolds.Clear();


                foreach (fpxTile oTile in gTileList)
                {
                    if (oTile.Name.Equals(dgvTiles.SelectedRows[0].Cells[1].Value))
                    {
                        numX.Value = oTile.x;
                        numY.Value = oTile.y;
                        numWidth.Value = oTile.width;
                        numHeight.Value = oTile.height;

                        gHolds = new Dictionary<int, List<fpxHold>>(oTile.HoldGroups);
                        tsbHolds.Items.Clear();

                        for (int i = 0; i < gHolds.Count; i++)
                        {
                            List<fpxHold> holds = gHolds[i];
                            iHoldGroup++;

                            switch (holds[0].type)
                            {
                                case "foot":
                                    tsbHolds.Items.Add("foot" + i);
                                    break;
                                case "climb":
                                    tsbHolds.Items.Add("climb" + i);
                                    break;
                                case "seat":
                                    tsbHolds.Items.Add("seat" + i);
                                    break;
                                default:
                                    tsbHolds.Items.Add("foot" + i);
                                    break;
                            }
                        }


                        if (tsbHolds.Items.Count > 0)
                            tsbHolds.SelectedIndex = 0;
                    }
                }

                ediTile.Invalidate();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sXml = gTiles.OuterXml;

            XmlDocument xSave = new XmlDocument();
            xSave.LoadXml(sXml);
            EditorManager.SetTileCollection(xSave);

            ValidateSave();
            ManageControls();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel your changes?", "Faples Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result.Equals(DialogResult.Yes))
            {
                string sXml = EditorManager.GetTileCollection().OuterXml;

                XmlDocument xLoad = new XmlDocument();
                xLoad.LoadXml(sXml);
                gTiles = xLoad;

                ClearSheets();

                foreach (XmlNode oNode in gTiles.FirstChild.ChildNodes)
                {
                    cmbSheet.Items.Add(oNode.Attributes["Name"].Value);
                }

                if (cmbSheet.Items.Count > 0)
                    cmbSheet.SelectedIndex = 0;
                else
                    cmbSheet.SelectedIndex = -1;
            }

            ValidateSave();
            ManageControls();
        }
        private void numX_ValueChanged(object sender, EventArgs e)
        {
            if (dgvTiles.SelectedRows.Count > 0)
            {
                fpxTile oObj = gTileList[dgvTiles.SelectedRows[0].Index];

                if (numX.Value != oObj.x)
                    oObj.x = (int)numX.Value;

                ValidateTile();
            }
        }

        private void numY_ValueChanged(object sender, EventArgs e)
        {
            if (dgvTiles.SelectedRows.Count > 0)
            {
                fpxTile oObj = gTileList[dgvTiles.SelectedRows[0].Index];

                if (numY.Value != oObj.y)
                    oObj.y = (int)numY.Value;

                ValidateTile();
            }
        }

        private void numWidth_ValueChanged(object sender, EventArgs e)
        {
            if (dgvTiles.SelectedRows.Count > 0)
            {
                fpxTile oObj = gTileList[dgvTiles.SelectedRows[0].Index];

                if (numWidth.Value != oObj.width)
                    oObj.width = (int)numWidth.Value;

                ValidateTile();
            }
        }

        private void numHeight_ValueChanged(object sender, EventArgs e)
        {
            if (dgvTiles.SelectedRows.Count > 0)
            {
                fpxTile oObj = gTileList[dgvTiles.SelectedRows[0].Index];

                if (numHeight.Value != oObj.height)
                    oObj.height = (int)numHeight.Value;

                ValidateTile();
            }
        }
        private void tsbFoothold_Click(object sender, EventArgs e)
        {
            tsbFoothold.Checked = !tsbFoothold.Checked;

            ValidateSave();
            ManageControls();
        }

        private void tsbSeathold_Click(object sender, EventArgs e)
        {
            tsbSeathold.Checked = !tsbSeathold.Checked;

            ValidateSave();
            ManageControls();
        }

        private void tsbClimbhold_Click(object sender, EventArgs e)
        {
            tsbClimbhold.Checked = !tsbClimbhold.Checked;

            ValidateSave();
            ManageControls();
        }

        private void tsbAddPoint_Click(object sender, EventArgs e)
        {
            tsbAddPoint.Checked = !tsbAddPoint.Checked;

            ValidateSave();
            ManageControls();
        }

        private void tsbConfirm_Click(object sender, EventArgs e)
        {
            tsbAddPoint.Checked = !tsbAddPoint.Checked;

            if (gHolds[iHoldGroup].Count > 0)
            {
                if (tsbFoothold.Checked)
                    tsbHolds.Items.Add("foot" + iHoldGroup);
                else if (tsbClimbhold.Checked)
                    tsbHolds.Items.Add("climb" + iHoldGroup);
                else if (tsbSeathold.Checked)
                    tsbHolds.Items.Add("seat" + iHoldGroup);


                iHoldGroup = gHolds.Count;

                if (tsbHolds.SelectedIndex < 0)
                    tsbHolds.SelectedIndex = 0;
            }

            gTileList[dgvTiles.SelectedRows[0].Index].HoldGroups = new Dictionary<int, List<fpxHold>>(gHolds);

            ValidateTile();
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            gHolds = new Dictionary<int, List<fpxHold>>(gTileList[dgvTiles.SelectedRows[0].Index].HoldGroups);
            int iIndex = tsbHolds.SelectedIndex;

            if (gHolds[iIndex].Count > 0)
            {
                tsbHolds.Items.RemoveAt(iIndex);
                gHolds.Remove(iIndex);

                Dictionary<int, List<fpxHold>> holdReorder = new Dictionary<int, List<fpxHold>>();

                foreach (int key in gHolds.Keys)
                {
                    if (key > iIndex)
                    {
                        holdReorder.Add(key - 1, gHolds[key]);
                    }
                    else
                    {
                        holdReorder.Add(key, gHolds[key]);
                    }
                }

                gHolds = new Dictionary<int, List<fpxHold>>(holdReorder);

                iHoldGroup = gHolds.Count;

                if (tsbHolds.Items.Count > 0)
                {
                    if (tsbHolds.SelectedIndex < 0)
                        tsbHolds.SelectedIndex = 0;
                }
            }

            gTileList[dgvTiles.SelectedRows[0].Index].HoldGroups = new Dictionary<int, List<fpxHold>>(gHolds);

            ValidateTile();
        }
        private void tsbHoldProperties_Click(object sender, EventArgs e)
        {

            if (tsbHolds.SelectedIndex > -1)
            {
                var oHoldProperties = new fpxHoldProperties();
                oHoldProperties.LoadHolds(gHolds[tsbHolds.SelectedIndex]);

                oHoldProperties.ShowDialog();

                if (oHoldProperties.DialogResult == DialogResult.Cancel)
                    return;


                if (oHoldProperties.DialogResult == DialogResult.OK)
                {
                    gHolds[tsbHolds.SelectedIndex] = oHoldProperties.SaveHolds();
                    gTileList[dgvTiles.SelectedRows[0].Index].HoldGroups = new Dictionary<int, List<fpxHold>>(gHolds);
                    ValidateTile();
                }
            }
        }
        private void tsbHolds_SelectedIndexChanged(object sender, EventArgs e)
        {
            ediTile.Invalidate();
        }

        private void ediTile_MouseMove(object sender, MouseEventArgs e)
        {
            iPointX = e.X;
            iPointY = e.Y;

            ediTile.Invalidate();
        }

        private void ediTile_MouseClick(object sender, MouseEventArgs e)
        {
            if (tsbAddPoint.Checked)
            {
                if (!gHolds.ContainsKey(iHoldGroup))
                    gHolds.Add(iHoldGroup, new List<fpxHold>());

                List<fpxHold> holdList = gHolds[iHoldGroup];

                fpxHold prevH = holdList.ElementAtOrDefault(holdList.Count - 1);

                fpxHold hold = new fpxHold();
                hold.id = holdList.Count;
                hold.gid = iHoldGroup;
                hold.x1 = Utility.GetGameResolutionX(e.X);
                hold.y1 = Utility.GetGameResolutionY(e.Y);

                if (tsbFoothold.Checked)
                    hold.type = "foot";
                else if (tsbClimbhold.Checked)
                    hold.type = "climb";
                else if (tsbSeathold.Checked)
                    hold.type = "seat";

                if (prevH != null)
                {
                    hold.prev = prevH;
                    hold.previd = prevH.id;
                    prevH.x2 = hold.x1;
                    prevH.y2 = hold.y1;
                    prevH.next = hold;
                    prevH.nextid = hold.id;
                    holdList[holdList.Count - 1] = prevH;
                }

                holdList.Add(hold);

                gHolds[iHoldGroup] = holdList;
            
               ediTile.Invalidate();
            }
        }
        private void ediTile_Paint(object sender, PaintEventArgs e)
        {
            if (gCurrTile != null)
            {
                Graphics g = e.Graphics;

                int iWidth = Utility.GetGameResolutionX(gCurrTile.Width);
                int iHeight = Utility.GetGameResolutionY(gCurrTile.Height);

                Rectangle rectObj = new Rectangle(0, 0, iWidth, iHeight);

                if (rectObj.Width < 5)
                    rectObj.Width = 5;

                if (rectObj.Height < 5)
                    rectObj.Height = 5;

                ediTile.Size = rectObj.Size;

                g.DrawImage(gCurrTile, rectObj);

                for (int i = 0; i < gHolds.Count; i++)
                {
                    List<fpxHold> holdGroup = gHolds[i];
                    foreach (fpxHold hold in holdGroup)
                    {
                        Pen pen = new Pen(Color.Black, Utility.GRAPHICS_POINT_LINE);
                        // Create pen.
                        if (hold.type.Equals("foot"))
                            pen.Color = Color.Red;
                        else if (hold.type.Equals("climb"))
                            pen.Color = Color.Green;
                        else if (hold.type.Equals("seat"))
                            pen.Color = Color.Orange;

                        if (tsbHolds.SelectedIndex == i)
                            pen.Color = Color.Blue;
                        // Create location and size of rectangle.
                        int x = Utility.GetGameResolutionX(hold.x1);
                        int y = Utility.GetGameResolutionY(hold.y1);
                        int width = Utility.GetGameResolutionX(Utility.GRAPHICS_POINT_RECT);
                        int height = Utility.GetGameResolutionY(Utility.GRAPHICS_POINT_RECT);

                        e.Graphics.DrawRectangle(pen, x, y, width, height);

                        if (hold.next != null)
                        {
                            int x2 = Utility.GetGameResolutionX(hold.x2);
                            int y2 = Utility.GetGameResolutionY(hold.y2);
                            e.Graphics.DrawLine(pen, new Point(x, y), new Point(x2, y2));
                        }
                    }
                }
            }
            else
            {
                Graphics g = e.Graphics;

               ediTile.Size = new Size(5, 5);
                g.Clear(Color.FromKnownColor(KnownColor.Control));
            }

            if (tsbAddPoint.Checked)
            {
                Graphics g = e.Graphics;

                Pen pen = new Pen(Color.Black, Utility.GRAPHICS_POINT_LINE);
                // Create pen.
                if (tsbFoothold.Checked)
                    pen.Color = Color.Red;
                else if (tsbClimbhold.Checked)
                    pen.Color = Color.Green;
                else if (tsbSeathold.Checked)
                    pen.Color = Color.Orange;

                //Point local = new Point(Cursor.Position.X, Cursor.Position.Y);
                // Create location and size of rectangle.
                int x = Utility.GetGameResolutionX(iPointX);
                int y = Utility.GetGameResolutionY(iPointY);
                int width = Utility.GetGameResolutionX(Utility.GRAPHICS_POINT_RECT);
                int height = Utility.GetGameResolutionY(Utility.GRAPHICS_POINT_RECT);

                g.DrawRectangle(pen, x - width / 2, y - height / 2, width, height);
            }
        }
        #endregion

        #region Main Methods
        public void Prepare()
        {
            string sXml = EditorManager.GetTileCollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gTiles = xLoad;

            PrepareControls();

            ManageControls();
        }
        public void PrepareControls()
        {
            DataGridViewImageColumn colImg = new DataGridViewImageColumn();
            colImg.Width = (int)(dgvTiles.Width * 0.75);
            DataGridViewTextBoxColumn colText = new DataGridViewTextBoxColumn();
            colText.Width = (int)(dgvTiles.Width * 0.25);

            dgvTiles.Columns.Add(colImg);
            dgvTiles.Columns.Add(colText);

            dgvTiles.ColumnHeadersVisible = false;
            dgvTiles.RowHeadersVisible = false;
            dgvTiles.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvTiles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvTiles.AllowUserToAddRows = false;
        }
        public void ValidateSave()
        {
            bool bDirtyCheck = false;

            if (!gTiles.OuterXml.Equals(EditorManager.GetTileCollection().OuterXml))
                bDirtyCheck = true;

            Dirty = bDirtyCheck;
        }
        public void ValidateTile()
        {
            string sName = dgvTiles.SelectedRows[0].Cells[1].Value.ToString();
            XmlNode oCollections = gTiles.SelectSingleNode("//fpxTiles");
            XmlNode oSheet = null;

            foreach (XmlNode oNode in oCollections.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                {
                    oSheet = oNode;
                    break;
                }
            }

            if (!Utility.XmlNodeExists(oSheet, sName))
            {
                fpxTile oT = gTileList[dgvTiles.SelectedRows[0].Index];

                foreach (XmlNode oNode in oSheet)
                {
                    if (oNode.Attributes["Name"].Value.Equals(oT.Name))
                    {
                        Point oSpriteLocation = new Point(oT.x, oT.y);
                        Size oSpriteSize = new Size(oT.width, oT.height);
                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);
                        Bitmap spriteSnap = Utility.GetSprite(gCurrSheet, oCrop);

                        int iSmallWidth = oSpriteSize.Width;
                        int iSmallHeight = oSpriteSize.Height;

                        decimal dWidth = iSmallWidth;
                        decimal dHeight = iSmallHeight;


                        decimal ratioX = Math.Max((dWidth / dHeight), 0.1m);
                        decimal ratioY = Math.Max((dHeight / dWidth), 0.1m);

                        if (ratioX > ratioY)
                        {
                            ratioY = 64;
                            ratioX = 64 * ratioX;

                        }
                        else if (ratioX < ratioY)
                        {
                            ratioY = 64 * ratioY;
                            ratioX = 64;
                        }
                        else
                        {
                            ratioY = 64;
                            ratioX = 64;
                        }

                        if (iSmallWidth > 64)
                        {
                            iSmallWidth = decimal.ToInt32(ratioX);

                            if (iSmallWidth > 128)
                                iSmallWidth = 128;
                        }


                        if (iSmallHeight > 64)
                        {
                            iSmallHeight = decimal.ToInt32(ratioY);
                            if (iSmallHeight > 128)
                                iSmallHeight = 128;
                        }

                        spriteSnap = Utility.ResizeImage(spriteSnap, iSmallWidth, iSmallHeight);

                        dgvTiles.SelectedRows[0].Cells[0].Dispose();
                        dgvTiles.SelectedRows[0].Cells[0].Value = spriteSnap;

                        XmlAttribute oName = gTiles.CreateAttribute("Name");
                        oName.Value = oT.Name;
                        XmlAttribute oX = gTiles.CreateAttribute("x");
                        oX.Value = oT.x.ToString();
                        XmlAttribute oY = gTiles.CreateAttribute("y");
                        oY.Value = oT.y.ToString();
                        XmlAttribute oWidth = gTiles.CreateAttribute("width");
                        oWidth.Value = oT.width.ToString();
                        XmlAttribute oHeight = gTiles.CreateAttribute("height");
                        oHeight.Value = oT.height.ToString();

                        XmlNode oHolds = gTiles.CreateNode("element", "Holds", "");

                        for (int i = 0; i < oT.HoldGroups.Count; i++)
                        {
                            XmlNode oHoldGroup = gTiles.CreateNode("element", "HoldGroup", "");
                            XmlAttribute oGroupId = gTiles.CreateAttribute("id");
                            oGroupId.Value = i.ToString();
                            oHoldGroup.Attributes.Append(oGroupId);

                            foreach (fpxHold hold in oT.HoldGroups[i])
                            {
                                XmlAttribute oId = gTiles.CreateAttribute("id");
                                oId.Value = hold.id.ToString();
                                XmlAttribute oGid = gTiles.CreateAttribute("gid");
                                oGid.Value = hold.gid.ToString();
                                XmlAttribute oLid = gTiles.CreateAttribute("lid");
                                oLid.Value = hold.lid.ToString();
                                XmlAttribute oX1 = gTiles.CreateAttribute("x1");
                                oX1.Value = hold.x1.ToString();
                                XmlAttribute oX2 = gTiles.CreateAttribute("x2");
                                oX2.Value = hold.x2.ToString();
                                XmlAttribute oY1 = gTiles.CreateAttribute("y1");
                                oY1.Value = hold.y1.ToString();
                                XmlAttribute oY2 = gTiles.CreateAttribute("y2");
                                oY2.Value = hold.y2.ToString();
                                XmlAttribute oNextId = gTiles.CreateAttribute("nextid");
                                oNextId.Value = hold.nextid.ToString();
                                XmlAttribute oPrevId = gTiles.CreateAttribute("previd");
                                oPrevId.Value = hold.previd.ToString();
                                XmlAttribute oType = gTiles.CreateAttribute("type");
                                oType.Value = hold.type;
                                XmlAttribute oForce = gTiles.CreateAttribute("force");
                                oForce.Value = hold.force.ToString();
                                XmlAttribute oCantPass = gTiles.CreateAttribute("cantPass");
                                oCantPass.Value = hold.cantPass.ToString();
                                XmlAttribute oCantDrop = gTiles.CreateAttribute("cantDrop");
                                oCantDrop.Value = hold.cantDrop.ToString();
                                XmlAttribute oCantMove = gTiles.CreateAttribute("cantMove");
                                oCantMove.Value = hold.cantMove.ToString();

                                XmlNode holdPoint = gTiles.CreateNode("element", "Hold", "");
                                holdPoint.Attributes.Append(oId);
                                holdPoint.Attributes.Append(oGid);
                                holdPoint.Attributes.Append(oLid);
                                holdPoint.Attributes.Append(oX1);
                                holdPoint.Attributes.Append(oX2);
                                holdPoint.Attributes.Append(oY1);
                                holdPoint.Attributes.Append(oY2);
                                holdPoint.Attributes.Append(oNextId);
                                holdPoint.Attributes.Append(oPrevId);
                                holdPoint.Attributes.Append(oType);
                                holdPoint.Attributes.Append(oForce);
                                holdPoint.Attributes.Append(oCantPass);
                                holdPoint.Attributes.Append(oCantDrop);
                                holdPoint.Attributes.Append(oCantMove);

                                oHoldGroup.AppendChild(holdPoint);
                            }

                            oHolds.AppendChild(oHoldGroup);
                        }

                        Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                        if (gCurrTile != null)
                            gCurrTile.Dispose();

                        gCurrTile = spriteFull;


                        XmlNode oObject = gTiles.CreateNode("element", "Tile", "");
                        oObject.Attributes.Append(oName);
                        oObject.Attributes.Append(oX);
                        oObject.Attributes.Append(oY);
                        oObject.Attributes.Append(oWidth);
                        oObject.Attributes.Append(oHeight);
                        oObject.AppendChild(oHolds);


                        oSheet.ReplaceChild(oObject, oNode);
                        oCollections.ReplaceChild(oSheet, oSheet);
                        gTiles.ReplaceChild(oCollections, oCollections);
                        break;
                    }
                }
            }

            ValidateSave();
            ManageControls();
        }
        public void ManageControls()
        {
            // Display lists

            bool bSheet = false;
            bool bSheetAdd = false;
            bool bSheetDel = false;

            bool bTile = false;
            bool bTileAdd = false;
            bool bTileDel = false;

            // Display object properties
            bool bTileSelected = false;

            bool bPointMode = false;
            bool bAddingPoint = false;



            // Enable Buttons that are always displayed
            bSheetAdd = true;

            if (cmbSheet.Items.Count > 0)
            {
                bSheet = true;
                bTileAdd = true;

                if (cmbSheet.SelectedItem != null)
                {
                    bSheetDel = true;
                }

                if (dgvTiles.Rows.Count > 0)
                {
                    bTile = true;

                    if (dgvTiles.SelectedRows.Count > 0)
                    {
                        bTileDel = true;
                        bTileSelected = true;
                    }
                }
            }

            // Check Toggle visibility
            if (tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked)
            {
                bPointMode = true;

                if (tsbAddPoint.Checked)
                {
                    bAddingPoint = true; ;
                }
            }

            pnlToolbox.Enabled = !bPointMode && !bAddingPoint;
            pnlProperties.Enabled = !bPointMode && !bAddingPoint;

            tsbAddPoint.Enabled = bPointMode;
            tsbHolds.Enabled = bPointMode && !bAddingPoint && tsbHolds.Items.Count > 0;
            tsbDelete.Enabled = bPointMode && !bAddingPoint && tsbHolds.Items.Count > 0;
            tsbHoldProperties.Enabled = bPointMode && !bAddingPoint && tsbHolds.Items.Count > 0;
            tsbConfirm.Enabled = bAddingPoint;

            // Enable/Disable All controls

            cmbSheet.Enabled = bSheet;
            btnAddSheet.Enabled = bSheetAdd;
            btnDeleteSheet.Enabled = bSheetDel;

            dgvTiles.Enabled = bTile;
            btnAddTile.Enabled = bTileAdd;
            btnDeleteTile.Enabled = bTileDel;

            tsbFoothold.Enabled = bTileSelected && !tsbClimbhold.Checked && !tsbSeathold.Checked && !bAddingPoint;
            tsbClimbhold.Enabled = bTileSelected && !tsbFoothold.Checked && !tsbSeathold.Checked && !bAddingPoint;
            tsbSeathold.Enabled = bTileSelected && !tsbFoothold.Checked && !tsbClimbhold.Checked && !bAddingPoint;

            numX.Enabled = bTileSelected;
            numY.Enabled = bTileSelected;
            numWidth.Enabled = bTileSelected;
            numHeight.Enabled = bTileSelected;

            btnSave.Enabled = Dirty;
            btnCancel.Enabled = Dirty;

           ediTile.Invalidate();
        }

        public void ClearSheets()
        {
            cmbSheet.Items.Clear();
            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            gCurrSheet = null;

            ClearTiles();
        }
        public void ClearTiles()
        {
            dgvTiles.Rows.Clear();
            gTileList = new List<fpxTile>();
            if (gCurrTile != null)
                gCurrTile.Dispose();

            gCurrTile = null;

            ClearObjectProperties();
        }
        public void ClearObjectProperties()
        {
            numX.Value = numX.Minimum;
            numY.Value = numY.Minimum;
            numWidth.Value = numWidth.Minimum;
            numHeight.Value = numHeight.Minimum;
            ClearHoldProperties();
        }

        public void ClearHoldProperties()
        {
            gHolds = new Dictionary<int, List<fpxHold>>();
            tsbHolds.Items.Clear();
            iHoldGroup = 0;
        }
        public void Reload()
        {
            ClearSheets();

            string sXml = EditorManager.GetTileCollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gTiles = xLoad;

            XmlNode oCollections = gTiles.SelectSingleNode("//fpxTiles");

            foreach (XmlNode oCollection in oCollections.ChildNodes)
            {
                cmbSheet.Items.Add(oCollection.Attributes["Name"].Value);
            }

            if (cmbSheet.Items.Count > 0)
                cmbSheet.SelectedIndex = 0;
        }
        #endregion
    }
}
