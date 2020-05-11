using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace FaplesEditor
{
    public partial class fpxUIEditor : UserControl
    {
        #region Declarations
        XmlDocument gUIDoc = new XmlDocument();
        XmlDocument gObjDoc = new XmlDocument();
        XmlDocument gTileDoc = new XmlDocument();

        List<fpxObject> gObjects = new List<fpxObject>();
        List<fpxTile> gTiles = new List<fpxTile>();
        List<fpxUI> gUI = new List<fpxUI>();
        Dictionary<int, fpxUILayer> gUILayers = new Dictionary<int, fpxUILayer>();
        List<fpxControl> gControls = new List<fpxControl>();

        Bitmap gCurrSheet = null;
        Bitmap gCurrSprite = null;

        bool bControlSizing = false;
        int iControlStartPointX = 0;
        int iControlStartPointY = 0;

        int iPointX = 0;
        int iPointY = 0;
        #endregion

        #region Properties
        public bool ObjectView { get; set; } = true;
        public bool TileView { get; set; } = true;
        public bool SceneryView { get; set; } = true;
        public bool ControlView { get; set; } = true;
        public bool Flipped { get; set; } = false;
        public bool Dirty { get; set; } = false;
        public bool Clicked { get; set; } = false;
        #endregion

        #region Constructor
        public fpxUIEditor()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Event Handlers
        private void btnSave_Click(object sender, EventArgs e)
        {
            ValidateUI();
            string sXml = gUIDoc.OuterXml;

            XmlDocument xSave = new XmlDocument();
            xSave.LoadXml(sXml);
            EditorManager.SetUICollection(xSave);

            ValidateSave();
            ManageControls();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel your changes?", "Faples Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result.Equals(DialogResult.Yes))
            {
                string sXml = EditorManager.GetUICollection().OuterXml;

                XmlDocument xLoad = new XmlDocument();
                xLoad.LoadXml(sXml);
                gUIDoc = xLoad;

                ClearUI();

                foreach (XmlNode oNode in gUIDoc.FirstChild.ChildNodes)
                {
                    cmbUI.Items.Add(oNode.Attributes["Name"].Value);
                }

                if (cmbUI.Items.Count > 0)
                    cmbUI.SelectedIndex = 0;
                else
                    cmbUI.SelectedIndex = -1;
            }

            ValidateSave();
            ManageControls();
        }
        private void tsbUI_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbUI.Checked;

            ResetMode();
            tsbUI.Checked = bSwapState;

            ManageControls();
        }

        private void tsbScenery_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbScenery.Checked;

            ResetMode();
            tsbScenery.Checked = bSwapState;

            ManageControls();
        }
        private void tsbTile_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbTile.Checked;

            ResetMode();
            tsbTile.Checked = bSwapState;

            ManageControls();
        }

        private void tsbObject_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbObject.Checked;

            ResetMode();
            tsbObject.Checked = bSwapState;

            ManageControls();
        }
        private void tsbControl_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbControl.Checked;

            ResetMode();
            tsbControl.Checked = bSwapState;

            tsbList.Items.Clear();

            if(cmbUI.Items.Count > 0)
            {
                foreach (fpxControl oControl in gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls)
                {
                    tsbList.Items.Add(oControl.Name);
                }

                if(tsbList.Items.Count > 0)
                {
                    tsbList.SelectedIndex = 0;
                }
            }
         

            ManageControls();
        }
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            tsbAdd.Checked = !tsbAdd.Checked;

            ValidateUI();
            ManageControls();
        }

        private void tsbConfirm_Click(object sender, EventArgs e)
        {
            tsbAdd.Checked = !tsbAdd.Checked;

            tsbList.Items.Clear();

            if (tsbObject.Checked)
            {
                foreach (fpxMapObject obj in gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UIObjects)
                {
                    tsbList.Items.Add(obj.Object.Name);
                }      
            }
            else if (tsbTile.Checked)
            {
                foreach (fpxMapTile tile in gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UITiles)
                {
                    tsbList.Items.Add(tile.Tile.Name);
                }
            }
            else if(tsbControl.Checked)
            {
                foreach (fpxControl oControl in gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UIControls)
                {
                    tsbList.Items.Add(oControl.Name);
                }
            }

            gUI[cmbUI.SelectedIndex].UILayers = new List<fpxUILayer>();

            foreach (fpxUILayer oLayer in gUILayers.Values)
            {
                gUI[cmbUI.SelectedIndex].UILayers.Add(oLayer);
            }

            ValidateUI();

            if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                tsbList.SelectedIndex = 0;
        }

        private void tsbProperties_Click(object sender, EventArgs e)
        {

        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (tsbObject.Checked)
            {
                gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UIObjects.RemoveAt(tsbList.SelectedIndex);
                tsbList.Items.Remove(tsbList.SelectedItem);
            }
            else if (tsbTile.Checked)
            {
                gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UITiles.RemoveAt(tsbList.SelectedIndex);
                tsbList.Items.Remove(tsbList.SelectedItem);
            }
            else if(tsbControl.Checked)
            {
                gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UIControls.RemoveAt(tsbList.SelectedIndex);
                tsbList.Items.Remove(tsbList.SelectedItem);
            }

            gUI[cmbUI.SelectedIndex].UILayers = new List<fpxUILayer>();

            foreach (fpxUILayer oLayer in gUILayers.Values)
            {
                gUI[cmbUI.SelectedIndex].UILayers.Add(oLayer);
            }

            ValidateUI();

            if (tsbList.Items.Count > 0)
                tsbList.SelectedIndex = 0;
        }

        private void tsbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tsbControl.Checked && gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls.Count > 0)
            {
                fpxControl oControl = gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex];

                txtControlName.Text = oControl.Name;
                numControlWidth.Value = oControl.Width;
                numControlHeight.Value = oControl.Height;
                numControlX.Value = oControl.LocX;
                numControlY.Value = oControl.LocY;
                cmbControlType.SelectedItem = oControl.Type;
            }

            ediUI.Invalidate();

            ManageControls();
        }
        private void cmbObjCollection_SelectedValueChanged(object sender, EventArgs e)
        {

            if (cmbObjCollection.SelectedIndex == -1)
                cmbObjCollection.Items.Clear();

            ClearObjectCategories();

            foreach (XmlNode oCollection in gObjDoc.SelectSingleNode("//fpxObjects").ChildNodes)
            {
                if (oCollection.Attributes["Name"].Value.Equals(cmbObjCollection.SelectedItem.ToString()))
                {
                    cmbObjCategory.Items.Clear();

                    foreach (XmlNode oCategory in oCollection.ChildNodes)
                    {
                        cmbObjCategory.Items.Add(oCategory.Attributes["Name"].Value);
                    }

                    if (cmbObjCategory.Items.Count > 0)
                    {
                        cmbObjCategory.SelectedIndex = 0;
                    }
                }
            }

            ManageControls();
        }

        private void cmbObjCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbObjCategory.SelectedIndex == -1)
                cmbObjCategory.Items.Clear();

            ClearObjectSheets();

            foreach (XmlNode oCollection in gObjDoc.SelectSingleNode("//fpxObjects").ChildNodes)
            {
                if (oCollection.Attributes["Name"].Value.Equals(cmbObjCollection.SelectedItem.ToString()))
                {
                    foreach (XmlNode oCategory in oCollection.ChildNodes)
                    {
                        if (oCategory.Attributes["Name"].Value.Equals(cmbObjCategory.SelectedItem.ToString()))
                        {
                            cmbObjSheet.Items.Clear();

                            foreach (XmlNode oSheet in oCategory.ChildNodes)
                            {
                                cmbObjSheet.Items.Add(oSheet.Attributes["Name"].Value);
                            }

                            if (cmbObjSheet.Items.Count > 0)
                            {
                                cmbObjSheet.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }

            ManageControls();
        }

        private void cmbObjSheet_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearObjects();

            if (cmbObjSheet.SelectedIndex == -1)
                cmbObjSheet.Items.Clear();

            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            if (gCurrSprite != null)
                gCurrSprite.Dispose();

            gCurrSheet = new Bitmap(Path.Combine(Utility.OBJECTS_PATH, cmbObjSheet.SelectedItem.ToString()));
            gObjects = new List<fpxObject>();
            gCurrSprite = null;

            foreach (XmlNode oCollection in gObjDoc.SelectSingleNode("//fpxObjects").ChildNodes)
            {
                if (oCollection.Attributes["Name"].Value.Equals(cmbObjCollection.SelectedItem.ToString()))
                {
                    foreach (XmlNode oCategory in oCollection.ChildNodes)
                    {
                        if (oCategory.Attributes["Name"].Value.Equals(cmbObjCategory.SelectedItem.ToString()))
                        {
                            foreach (XmlNode oSheet in oCategory.ChildNodes)
                            {
                                if (oSheet.Attributes["Name"].Value.Equals(cmbObjSheet.SelectedItem.ToString()))
                                {
                                    dgvObjects.Rows.Clear();

                                    foreach (XmlNode oObject in oSheet.ChildNodes)
                                    {
                                        fpxObject oObj = new fpxObject();
                                        oObj.Name = oObject.Attributes["Name"].Value;
                                        oObj.SpriteSheet = oSheet.Attributes["Name"].Value;
                                        oObj.x = int.Parse(oObject.Attributes["x"].Value);
                                        oObj.y = int.Parse(oObject.Attributes["y"].Value);
                                        oObj.width = int.Parse(oObject.Attributes["width"].Value);
                                        oObj.height = int.Parse(oObject.Attributes["height"].Value);
                                        oObj.Animated = bool.Parse(oObject.Attributes["Animated"].Value);

                                        XmlNode oAnimations = oObject.FirstChild;
                                        oObj.AnimationIndex = int.Parse(oAnimations.Attributes["Index"].Value);

                                        foreach (XmlNode oAnimation in oAnimations.ChildNodes)
                                        {
                                            fpxAnimation anim = new fpxAnimation();
                                            anim.Name = oAnimation.Attributes["Name"].Value;
                                            anim.ReelHeight = int.Parse(oAnimation.Attributes["ReelHeight"].Value);
                                            anim.ReelIndex = int.Parse(oAnimation.Attributes["ReelIndex"].Value);
                                            anim.FrameWidth = int.Parse(oAnimation.Attributes["FrameWidth"].Value);
                                            anim.TotalFrames = int.Parse(oAnimation.Attributes["TotalFrames"].Value);
                                            anim.FrameSpeed = decimal.Parse(oAnimation.Attributes["FrameSpeed"].Value);
                                            oObj.Animations.Add(anim);
                                        }

                                        gObjects.Add(oObj);

                                        Point oSpriteLocation = new Point(oObj.x, oObj.y);
                                        Size oSpriteSize = new Size(oObj.width, oObj.height);
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

                                        dgvObjects.Rows.Add(new DataGridViewRow());
                                        DataGridViewRow oRow = dgvObjects.Rows[dgvObjects.Rows.Count - 1];
                                        oRow.MinimumHeight = 96;
                                        oRow.Cells[0].Value = spriteSnap;
                                        oRow.Cells[1].Value = oObj.Name;
                                    }

                                    if (dgvObjects.Rows.Count > 0)
                                    {
                                        dgvObjects.Rows[0].Selected = true;
                                        fpxObject oObj = gObjects[0];

                                        Point oSpriteLocation = new Point(oObj.x, oObj.y);
                                        Size oSpriteSize = new Size(oObj.width, oObj.height);
                                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);
                                        Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                                        if (gCurrSprite != null)
                                            gCurrSprite.Dispose();

                                        gCurrSprite = spriteFull;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ManageControls();
        }
        private void dgvObjects_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                if (gCurrSprite != null)
                    gCurrSprite.Dispose();

                foreach (fpxObject oObject in gObjects)
                {
                    if (oObject.Name.Equals(dgvObjects.SelectedRows[0].Cells[1].Value))
                    {
                        Point oSpriteLocation = new Point(oObject.x, oObject.y);
                        Size oSpriteSize = new Size(oObject.width, oObject.height);
                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                        Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                        gCurrSprite = spriteFull;

                        if (chkFlip.Checked)
                        {
                            gCurrSprite.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                    }
                }

                ediUI.Invalidate();
            }
        }
        private void cmbTileSheets_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearTiles();

            if (cmbTileSheets.SelectedIndex == -1)
                cmbTileSheets.Items.Clear();

            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            if (gCurrSprite != null)
                gCurrSprite.Dispose();

            gCurrSheet = new Bitmap(Path.Combine(Utility.TILES_PATH, cmbTileSheets.SelectedItem.ToString()));
            gTiles = new List<fpxTile>();
            gCurrSprite = null;

            foreach (XmlNode oSheet in gTileDoc.SelectSingleNode("//fpxTiles").ChildNodes)
            {
                if (oSheet.Attributes["Name"].Value.Equals(cmbTileSheets.SelectedItem.ToString()))
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

                        gTiles.Add(oT);

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
                        fpxTile oT = gTiles[0];

                        Point oSpriteLocation = new Point(oT.x, oT.y);
                        Size oSpriteSize = new Size(oT.width, oT.height);
                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);
                        Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                        if (gCurrSprite != null)
                            gCurrSprite.Dispose();

                        gCurrSprite = spriteFull;
                    }
                }
            }

            ManageControls();
        }

        private void dgvTiles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTiles.SelectedRows.Count > 0)
            {
                Flipped = false;

                if (gCurrSprite != null)
                    gCurrSprite.Dispose();

                foreach (fpxTile oTile in gTiles)
                {
                    if (oTile.Name.Equals(dgvTiles.SelectedRows[0].Cells[1].Value))
                    {
                        Point oSpriteLocation = new Point(oTile.x, oTile.y);
                        Size oSpriteSize = new Size(oTile.width, oTile.height);
                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                        Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                        gCurrSprite = spriteFull;
                    }
                }

                ediUI.Invalidate();
            }
        }
        private void dgvScenery_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int oLayer = int.Parse(cmbLayer.SelectedItem.ToString());

            string sFlip = "";

            if (chkFlip.Checked)
                sFlip += "Flipped";

            if (gCurrSprite != null)
                gCurrSprite.Dispose();

            if (gUILayers[oLayer].UISprites.Count > 0)
            {
                foreach (fpxSprite oSprMapping in gUILayers[oLayer].UISprites)
                {
                    if (oSprMapping.SpriteSheet.Equals(txtScenery.Text) && oSprMapping.SpriteName.Equals(txtScenery.Text))
                    {
                        oSprMapping.Sprite.Dispose();
                        gUILayers[oLayer].UISprites.Remove(oSprMapping);
                        break;
                    }
                    else if (oSprMapping.SpriteSheet.Equals(txtScenery.Text) && oSprMapping.SpriteName.Equals(txtScenery.Text + "Flipped"))
                    {
                        oSprMapping.Sprite.Dispose();
                        gUILayers[oLayer].UISprites.Remove(oSprMapping);
                        break;
                    }
                }
            }

            txtScenery.Text = dgvScenery.Rows[e.RowIndex].Cells[1].Value.ToString();

            gCurrSprite = new Bitmap(Path.Combine(Utility.SCENERY_PATH, txtScenery.Text));

            fpxMapScenery oScenMapping = new fpxMapScenery();
            oScenMapping.SpriteName = txtScenery.Text;

            foreach (fpxSprite oSprMapping in gUILayers[oLayer].UISprites)
            {
                if (oSprMapping.SpriteSheet.Equals(txtScenery.Text) && oSprMapping.SpriteName.Equals(txtScenery.Text + sFlip))
                {
                    oScenMapping.Sprite = (Bitmap)oSprMapping.Sprite.Clone();
                    oSprMapping.UsageCount++;
                }
            }

            if (oScenMapping.Sprite == null)
            {
                fpxSprite oSprMapping = new fpxSprite();
                oSprMapping.SpriteSheet = txtScenery.Text;
                oSprMapping.SpriteName = txtScenery.Text + sFlip;
                oSprMapping.Sprite = (Bitmap)gCurrSprite.Clone();
                if (chkFlip.Checked)
                    oSprMapping.Sprite.RotateFlip(RotateFlipType.RotateNoneFlipX);
                oSprMapping.UsageCount++;

                oScenMapping.Sprite = (Bitmap)oSprMapping.Sprite.Clone();
                gUILayers[oLayer].UISprites.Add(oSprMapping);
            }

            if (gUILayers[oLayer].UIScenery.Sprite != null)
                gUILayers[oLayer].UIScenery.Sprite.Dispose();

            gUILayers[oLayer].UIScenery = oScenMapping;

            if (cmbUI.Items.Count > 0 && cmbUI.SelectedItem != null)
                ValidateUI();
        }
        private void cmbLayer_SelectedValueChanged(object sender, EventArgs e)
        {
            if (tsbObject.Checked)
            {
                tsbList.Items.Clear();

                foreach (fpxMapObject obj in gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UIObjects)
                {
                    tsbList.Items.Add(obj.Object.Name);
                }

                if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                    tsbList.SelectedIndex = 0;
            }
            else if (tsbTile.Checked)
            {
                tsbList.Items.Clear();

                foreach (fpxMapTile tile in gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UITiles)
                {
                    tsbList.Items.Add(tile.Tile.Name);
                }

                if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                    tsbList.SelectedIndex = 0;
            }
            else if(tsbControl.Checked)
            {
                tsbList.Items.Clear();

                foreach (fpxControl oControl in gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls)
                {
                    tsbList.Items.Add(oControl.Name);
                }

                if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                    tsbList.SelectedIndex = 0;
            }

            if (cmbLayer.SelectedItem != null)
            {
                numScrollX.Value = gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].ScrollX;
                numScrollY.Value = gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].ScrollY;
                txtScenery.Text = gUILayers[int.Parse(cmbLayer.SelectedItem.ToString())].UIScenery.SpriteName;
            }


            ManageControls();
        }
        private void numScrollX_ValueChanged(object sender, EventArgs e)
        {
            gUILayers[cmbLayer.SelectedIndex].ScrollX = numScrollX.Value;

            if (cmbUI.Items.Count > 0 && cmbUI.SelectedItem != null)
                ValidateUI();
        }

        private void numScrollY_ValueChanged(object sender, EventArgs e)
        {
            gUILayers[cmbLayer.SelectedIndex].ScrollY = numScrollY.Value;

            if (cmbUI.Items.Count > 0 && cmbUI.SelectedItem != null)
                ValidateUI();
        }
        private void btnSceneryClear_Click(object sender, EventArgs e)
        {
            int oLayer = int.Parse(cmbLayer.SelectedItem.ToString());

            txtScenery.Text = "";
            if (gUILayers[oLayer].UIScenery.Sprite != null)
                gUILayers[oLayer].UIScenery.Sprite.Dispose();

            gUILayers[oLayer].UIScenery = new fpxMapScenery();
            ValidateUI();
        }
        private void chkFlip_CheckedChanged(object sender, EventArgs e)
        {
            if (gCurrSprite != null)
            {
                gCurrSprite.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
        }
        private void btnUINew_Click(object sender, EventArgs e)
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

                    if (!cmbUI.Items.Contains(sName))
                    {
                        try
                        {
                            fpxUI ui = new fpxUI();
                            ui.ID = cmbUI.Items.Count;
                            ui.Name = sName;
                            ui.Static = true;
                        
                            for (int i = 0; i < cmbLayer.Items.Count; i++)
                            {
                                fpxUILayer oMapLayer = new fpxUILayer();
                                ui.UILayers.Add(oMapLayer);
                            }

                            gUI.Add(ui);

                            bValid = true;
                            cmbUI.Items.Add(sName);

                            if (cmbUI.SelectedIndex < 0)
                            {
                                int iIndex = Math.Max(cmbUI.Items.Count - 1, 0);

                                cmbUI.SelectedIndex = iIndex;

                                ediUI.Size = new Size(5, 5);
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Error inserting UI. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }                      
                    }
                    else
                    {
                        MessageBox.Show("UI already exists. Please use a different name.");
                    }
                }
            }

            oAddDialog.Close();
            ValidateUI();
        }
        private void cmbUI_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbUI.SelectedIndex > -1)
            {
                fpxUI oUI = gUI[cmbUI.SelectedIndex];

                gUILayers = new Dictionary<int, fpxUILayer>();

                for (int i = 0; i < oUI.UILayers.Count; i++)
                {
                    gUILayers.Add(i, oUI.UILayers[i]);
                }

                numUIWidth.Value = oUI.Width;
                numUIHeight.Value = oUI.Height;
                numUIX.Value = oUI.X;
                numUIY.Value = oUI.Y;
                chkStatic.Checked = oUI.Static;       
            }

            ediUI.Invalidate();
        }
        private void numUIWidth_ValueChanged(object sender, EventArgs e)
        {
            if (cmbUI.SelectedIndex > -1)
            {
                fpxUI oUI = gUI[cmbUI.SelectedIndex];

                if (numUIWidth.Value != oUI.Width)
                    oUI.Width = (int)numUIWidth.Value;

                int iWidth = Math.Max(oUI.Width, 5);
                int iHeight = Math.Max(oUI.Height, 5);
                ediUI.Size = new Size(iWidth, iHeight);

                gUI[cmbUI.SelectedIndex] = oUI;

                ValidateUI();
            }
        }

        private void numUIHeight_ValueChanged(object sender, EventArgs e)
        {
            if (cmbUI.SelectedIndex > -1)
            {
                fpxUI oUI = gUI[cmbUI.SelectedIndex];

                if (numUIHeight.Value != oUI.Height)
                    oUI.Height = (int)numUIHeight.Value;

                int iWidth = Math.Max(oUI.Width, 5);
                int iHeight = Math.Max(oUI.Height, 5);
                ediUI.Size = new Size(iWidth, iHeight);

                ValidateUI();
            }
        }

        private void numUIX_ValueChanged(object sender, EventArgs e)
        {
            if (cmbUI.SelectedIndex > -1)
            {
                fpxUI oUI = gUI[cmbUI.SelectedIndex];

                if (numUIX.Value != oUI.X)
                    oUI.X = (int)numUIX.Value;

                ValidateUI();
            }
        }

        private void numUIY_ValueChanged(object sender, EventArgs e)
        {
            if (cmbUI.SelectedIndex > -1)
            {
                fpxUI oUI = gUI[cmbUI.SelectedIndex];

                if (numUIY.Value != oUI.Y)
                    oUI.Y = (int)numUIY.Value;

                ValidateUI();
            }
        }
        private void txtControlName_TextChanged(object sender, EventArgs e)
        {
            if (tsbList.SelectedIndex > -1 && gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls.Count > 0)
            {              
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex].Name = txtControlName.Text;
                tsbList.Items[tsbList.SelectedIndex] = gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex].Name;
                ValidateUI();
            }
        }
        private void btnControlProperties_Click(object sender, EventArgs e)
        {
            var oControlDialog = new fpxControlDialog();

            oControlDialog.ControlType = cmbControlType.SelectedItem.ToString();

            fpxControl Control = gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex];
            oControlDialog.CurrentControl = Control;


            oControlDialog.ShowDialog();

            if (oControlDialog.DialogResult == DialogResult.Cancel)
            {
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex] = oControlDialog.BackupControl;
                ValidateUI();
            }


            if (oControlDialog.DialogResult == DialogResult.OK)
            {
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex] = oControlDialog.CurrentControl;
                ValidateUI();
            }
        }

        private void cmbControlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tsbList.SelectedIndex > -1 && gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls.Count > 0)
            {
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex].Type = cmbControlType.Text;
                ValidateUI();
            }
        }

        private void numControlWidth_ValueChanged(object sender, EventArgs e)
        {
            if (tsbList.SelectedIndex > -1 && gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls.Count > 0)
            {
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex].Width = (int)numControlWidth.Value;
                ValidateUI();
            }
        }

        private void numControlHeight_ValueChanged(object sender, EventArgs e)
        {
            if (tsbList.SelectedIndex > -1 && gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls.Count > 0)
            {
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex].Height = (int)numControlHeight.Value;
                ValidateUI();
            }
        }

        private void numControlX_ValueChanged(object sender, EventArgs e)
        {
            if (tsbList.SelectedIndex > -1 && gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls.Count > 0)
            {
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex].LocX = (int)numControlX.Value;
                ValidateUI();
            }
        }

        private void numControlY_ValueChanged(object sender, EventArgs e)
        {
            if (tsbList.SelectedIndex > -1 && gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls.Count > 0)
            {
                gUI[cmbUI.SelectedIndex].UILayers[cmbLayer.SelectedIndex].UIControls[tsbList.SelectedIndex].LocY = (int)numControlY.Value;
                ValidateUI();
            }
        }
        private void chkStatic_CheckedChanged(object sender, EventArgs e)
        {
            if (cmbUI.SelectedIndex > -1)
            {
                fpxUI oUI = gUI[cmbUI.SelectedIndex];

                oUI.Static = !oUI.Static;

                ValidateUI();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            ediUI.Invalidate();
        }
        private void ediUI_MouseMove(object sender, MouseEventArgs e)
        {
            iPointX = e.X;
            iPointY = e.Y;

            ediUI.Invalidate();
        }
        private void ediUI_MouseDown(object sender, MouseEventArgs e)
        {
            Clicked = true;
        }
        private void ediUI_MouseUp(object sender, MouseEventArgs e)
        {
            Clicked = false;
        }
        private void ediUI_MouseClick(object sender, MouseEventArgs e)
        {
            if (tsbAdd.Checked)
            {
                if (tsbObject.Checked && dgvObjects.SelectedRows.Count > 0)
                {
                    int oLayer = int.Parse(cmbLayer.SelectedItem.ToString());

                    fpxMapObject oObjMapping = new fpxMapObject();
                    oObjMapping.MapCoor = new Point(iPointX - gCurrSprite.Width / 2, iPointY - gCurrSprite.Height / 2);
                    fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];
                    oObjMapping.Object = oObj;
                    oObjMapping.Flipped = chkFlip.Checked;

                    string sFlip = "";

                    if (chkFlip.Checked)
                    {
                        sFlip += "Flipped";
                    }

                    foreach (fpxSprite oSprMapping in gUILayers[oLayer].UISprites)
                    {
                        if (oSprMapping.SpriteSheet.Equals(cmbObjSheet.SelectedItem) && oSprMapping.SpriteName.Equals(dgvObjects.SelectedRows[0].Cells[1].Value + sFlip))
                        {
                            oObjMapping.Sprite = (Bitmap)oSprMapping.Sprite.Clone();
                            oSprMapping.UsageCount++;
                        }
                    }

                    if (oObjMapping.Sprite == null)
                    {
                        fpxSprite oSprMapping = new fpxSprite();
                        oSprMapping.SpriteSheet = cmbObjSheet.SelectedItem.ToString();
                        oSprMapping.SpriteName = dgvObjects.SelectedRows[0].Cells[1].Value.ToString() + sFlip;
                        oSprMapping.Sprite = (Bitmap)gCurrSprite.Clone();
                        oSprMapping.UsageCount++;

                        oObjMapping.Sprite = (Bitmap)oSprMapping.Sprite.Clone();
                        gUILayers[oLayer].UISprites.Add(oSprMapping);
                    }

                    gUILayers[oLayer].UIObjects.Add(oObjMapping);
                }
                else if (tsbTile.Checked && dgvTiles.SelectedRows.Count > 0)
                {
                    int oLayer = int.Parse(cmbLayer.SelectedItem.ToString());

                    fpxMapTile oTileMapping = new fpxMapTile();
                    oTileMapping.MapCoor = new Point(iPointX - gCurrSprite.Width / 2, iPointY - gCurrSprite.Height / 2);
                    fpxTile oTile = gTiles[dgvTiles.SelectedRows[0].Index];
                    oTileMapping.Tile = oTile;
                    oTileMapping.Flipped = chkFlip.Checked;

                    string sFlip = "";

                    if (chkFlip.Checked)
                        sFlip += "Flipped";

                    foreach (fpxSprite oSprMapping in gUILayers[oLayer].UISprites)
                    {
                        if (oSprMapping.SpriteSheet.Equals(cmbTileSheets.SelectedItem) && oSprMapping.SpriteName.Equals(dgvTiles.SelectedRows[0].Cells[1].Value + sFlip))
                        {
                            oTileMapping.Sprite = (Bitmap)oSprMapping.Sprite.Clone();
                            oSprMapping.UsageCount++;
                        }
                    }

                    if (oTileMapping.Sprite == null)
                    {
                        fpxSprite oSprMapping = new fpxSprite();
                        oSprMapping.SpriteSheet = cmbTileSheets.SelectedItem.ToString();
                        oSprMapping.SpriteName = dgvTiles.SelectedRows[0].Cells[1].Value.ToString() + sFlip;
                        oSprMapping.Sprite = (Bitmap)gCurrSprite.Clone();
                        oSprMapping.UsageCount++;

                        oTileMapping.Sprite = (Bitmap)oSprMapping.Sprite.Clone();
                        gUILayers[oLayer].UISprites.Add(oSprMapping);
                    }

                    gUILayers[oLayer].UITiles.Add(oTileMapping);
                }
                else if (tsbControl.Checked)
                {
                    bControlSizing = !bControlSizing;

                    if(bControlSizing)
                    {
                        iControlStartPointX = e.X;
                        iControlStartPointY = e.Y;
                    }
                    else
                    {
                        int iX = iControlStartPointX;
                        int iY = iControlStartPointY;
                        int iWidth = iPointX - iControlStartPointX;
                        int iHeight = iPointY - iControlStartPointY;


                        if (iWidth < 0)
                        {
                            iX = iPointX;
                            iWidth = iControlStartPointX - iPointX;
                        }


                        if (iHeight < 0)
                        {
                            iY = iPointY;
                            iHeight = iControlStartPointY - iPointY;
                        }

                        int oLayer = int.Parse(cmbLayer.SelectedItem.ToString());

                        fpxControl oControl = new fpxControl();
                        oControl.Name = "control" + gUILayers[oLayer].UIControls.Count;
                        oControl.LocX = iX;
                        oControl.LocY = iY;
                        oControl.Width = iWidth;
                        oControl.Height = iHeight;
                        oControl.Type = "Textbox";

                        gUILayers[oLayer].UIControls.Add(oControl);
                    }
                }
            }
        }
        private void ediUI_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(ediUI.BackColor);

            foreach (fpxUILayer oLayer in gUILayers.Values)
            {
                if (!chkHideForward.Checked || oLayer.LayerID <= int.Parse(cmbLayer.SelectedItem.ToString()))
                {
                    if (SceneryView)
                    {
                        if (oLayer.UIScenery.Sprite != null)
                        {
                            fpxMapScenery os = oLayer.UIScenery;

                            Rectangle oMapRect = new Rectangle(0, 0, os.Sprite.Width, os.Sprite.Height);
                            g.DrawImage(os.Sprite, oMapRect);
                        }
                    }

                    if (TileView)
                    {
                        for (int i = 0; i < oLayer.UITiles.Count; i++)
                        {
                            fpxMapTile ot = oLayer.UITiles[i];

                            Rectangle oMapRect = new Rectangle(ot.MapCoor.X, ot.MapCoor.Y, ot.Tile.width, ot.Tile.height);
                            g.DrawImage(ot.Sprite, oMapRect);


                            if (tsbTile.Checked && tsbList.SelectedIndex == i && (oLayer.LayerID == int.Parse(cmbLayer.SelectedItem.ToString())))
                            {
                                Pen pen = new Pen(Color.Black, 3);

                                Rectangle oSelectRect = new Rectangle(ot.MapCoor.X, ot.MapCoor.Y, ot.Tile.width, ot.Tile.height);
                                g.DrawRectangle(pen, oSelectRect);
                            }
                        }
                    }
                    if (ObjectView)
                    {
                        for (int i = 0; i < oLayer.UIObjects.Count; i++)
                        {
                            fpxMapObject o = oLayer.UIObjects[i];

                            Rectangle oMapRect = new Rectangle(o.MapCoor.X, o.MapCoor.Y, o.Object.width, o.Object.height);
                            g.DrawImage(o.Sprite, oMapRect);

                            if (tsbObject.Checked && tsbList.SelectedIndex == i && (oLayer.LayerID == int.Parse(cmbLayer.SelectedItem.ToString())))
                            {
                                Pen pen = new Pen(Color.Black, 3);

                                Rectangle oSelectRect = new Rectangle(o.MapCoor.X, o.MapCoor.Y, o.Object.width, o.Object.height);
                                g.DrawRectangle(pen, oSelectRect);
                            }
                        }
                    }
                    if(ControlView)
                    {
                        for (int i = 0; i < oLayer.UIControls.Count; i++)
                        {
                            Pen pen = new Pen(Color.Black, 1);

                            fpxControl con = oLayer.UIControls[i];

                            if (tsbControl.Checked && tsbList.SelectedIndex == i && (oLayer.LayerID == int.Parse(cmbLayer.SelectedItem.ToString())))
                            {
                                pen = new Pen(Color.Blue, 1);
                            }

                            Rectangle oConRect = new Rectangle(con.LocX, con.LocY, con.Width, con.Height);
                            g.DrawRectangle(pen, oConRect);
                            
                            if(con.Type == "Textbox")
                            {
                                SolidBrush drawBrush = new SolidBrush(con.Color);

                                if(con.Label)
                                    g.DrawString(con.LabelValue, new Font(con.Font, con.Size), drawBrush, con.LocX + 5, con.LocY + 5);
                                else
                                    g.DrawString(con.Name, new Font(con.Font, con.Size), drawBrush, con.LocX + 5, con.LocY + 5);
                            }
                            else if(con.Type == "Button")
                            {
                                if(con.SpriteBaseName != "" && con.SpriteBase == null)
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, con.SpriteBaseName));
                                    con.SpriteBase = new Bitmap(oBmp, new Size(con.Width, con.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }
                                if (con.SpriteBase != null)
                                {
                                    g.DrawImage(con.SpriteBase, con.LocX, con.LocY);
                                }

                                if (con.SpriteFocusName != "" && con.SpriteFocus == null)
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, con.SpriteFocusName));
                                    con.SpriteFocus = new Bitmap(oBmp, new Size(con.Width, con.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }
                                if (con.SpriteFocus != null)
                                {
                                    if (tsbList.SelectedIndex == i)
                                    {
                                         g.DrawImage(con.SpriteFocus, con.LocX, con.LocY);
                                    }
                                }

                                if (con.SpriteHoverName != "" && con.SpriteHover == null)
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, con.SpriteHoverName));
                                    con.SpriteHover = new Bitmap(oBmp, new Size(con.Width, con.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }
                                if (con.SpriteHover != null)
                                {
                                    if (iPointX > con.LocX && iPointX < con.LocX + con.Width && iPointY > con.LocY && iPointY < con.LocY + con.Height)
                                    {
                                        g.DrawImage(con.SpriteHover, con.LocX, con.LocY);
                                    }
                                }

                                if (con.SpriteClickName != "" && con.SpriteClick == null)
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, con.SpriteClickName));
                                    con.SpriteClick = new Bitmap(oBmp, new Size(con.Width, con.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }
                                if (con.SpriteClick != null)
                                {
                                    if (Clicked)
                                    {
                                        if (iPointX > con.LocX && iPointX < con.LocX + con.Width && iPointY > con.LocY && iPointY < con.LocY + con.Height)
                                        {
                                            g.DrawImage(con.SpriteClick, con.LocX, con.LocY);
                                        }
                                    }
                                }
                            }
                            else if(con.Type == "Percentage Bar")
                            {
                                if(con.PercentBar != null)
                                {
                                    Rectangle clip = new Rectangle(0, 0, (int)(con.PercentBar.Width * (con.PercentValue / 100)), con.PercentBar.Height);
                                    g.DrawImage(con.PercentBar, con.LocX, con.LocY, clip, GraphicsUnit.Pixel);
                                }
                            }
                            else if(con.Type == "Options")
                            {

                            }
                        }
                    }
                }
            }

            if (tsbAdd.Checked)
            {
                if (tsbObject.Checked || tsbTile.Checked)
                {
                    if (gCurrSprite != null)
                    {
                        Rectangle rectObj = new Rectangle(iPointX - gCurrSprite.Width / 2, iPointY - gCurrSprite.Height / 2, gCurrSprite.Width, gCurrSprite.Height);

                        if (rectObj.Width < 5)
                            rectObj.Width = 5;

                        if (rectObj.Height < 5)
                            rectObj.Height = 5;

                        g.DrawImage(gCurrSprite, rectObj);
                    }
                }
                else if(tsbControl.Checked)
                {
                    if(bControlSizing)
                    {
                        Pen pen = new Pen(Color.Blue, 3);
                        pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                        bool bReverseX = false;
                        bool bReverseY = false;

                        if(iPointX < iControlStartPointX)
                        {
                            bReverseX = true;
                        }

                        if(iPointY < iControlStartPointY)
                        {
                            bReverseY = true;
                        }

                        int x = iControlStartPointX;
                        int y = iControlStartPointY;

                        int width = iPointX - iControlStartPointX;
                        int height = iPointY + -iControlStartPointY;

                        if(bReverseX)
                        {
                            x = iPointX;
                            width = iControlStartPointX - iPointX;
                        }

                        if (bReverseY)
                        {
                            y = iPointY;
                            height = iControlStartPointY - iPointY;
                        }

                        
                        g.DrawRectangle(pen, new Rectangle(x, y, width, height));
                    }
                }
            }
        }
        #endregion

        #region Main Methods
        private void Prepare()
        {
            ResetMode();
            tsbUI.Checked = true;

            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;

            string sXml = EditorManager.GetUICollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gUIDoc = xLoad;

            PrepareLayers();

            PrepareControls();
            PrepareScenery();

            ManageControls();
        }
        public void PrepareLayers()
        {
            foreach (var oItem in cmbLayer.Items)
            {
                fpxUILayer oLayer = new fpxUILayer();
                int iLayer = int.Parse(oItem.ToString());
                oLayer.LayerID = iLayer;
                gUILayers.Add(iLayer, oLayer);
            }
        }
        public void PrepareScenery()
        {
            string[] images = Directory.GetFiles(Utility.SCENERY_PATH, "*.png");
            foreach (string image in images)
            {
                Bitmap spriteSnap = new Bitmap(image);

                Point oSpriteLocation = new Point(0, 0);
                Size oSpriteSize = new Size(spriteSnap.Width, spriteSnap.Height);
                Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                spriteSnap = Utility.GetSprite(spriteSnap, oCrop);

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

                dgvScenery.Rows.Add(new DataGridViewRow());
                DataGridViewRow oRow = dgvScenery.Rows[dgvScenery.Rows.Count - 1];
                oRow.MinimumHeight = 96;
                oRow.Cells[0].Value = spriteSnap;
                oRow.Cells[1].Value = Path.GetFileName(image);
            }
        }

        public void PrepareControls()
        {
            // Global Controls
            cmbLayer.SelectedIndex = cmbLayer.Items.Count - 1;

            // Scenery Controls
            DataGridViewImageColumn colSceneryImg = new DataGridViewImageColumn();
            colSceneryImg.Width = (int)(dgvScenery.Width * 0.75);
            DataGridViewTextBoxColumn colSceneryText = new DataGridViewTextBoxColumn();
            colSceneryText.Width = (int)(dgvScenery.Width * 0.25);

            dgvScenery.Columns.Add(colSceneryImg);
            dgvScenery.Columns.Add(colSceneryText);

            dgvScenery.ColumnHeadersVisible = false;
            dgvScenery.RowHeadersVisible = false;
            dgvScenery.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvScenery.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvScenery.AllowUserToAddRows = false;

            // Object Controls
            DataGridViewImageColumn colObjectImg = new DataGridViewImageColumn();
            colObjectImg.Width = (int)(dgvObjects.Width * 0.75);
            DataGridViewTextBoxColumn colObjectText = new DataGridViewTextBoxColumn();
            colObjectText.Width = (int)(dgvObjects.Width * 0.25);

            dgvObjects.Columns.Add(colObjectImg);
            dgvObjects.Columns.Add(colObjectText);

            dgvObjects.ColumnHeadersVisible = false;
            dgvObjects.RowHeadersVisible = false;
            dgvObjects.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvObjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvObjects.AllowUserToAddRows = false;

            // Tile Controls
            DataGridViewImageColumn colTileImg = new DataGridViewImageColumn();
            colTileImg.Width = (int)(dgvTiles.Width * 0.75);
            DataGridViewTextBoxColumn colTileText = new DataGridViewTextBoxColumn();
            colTileText.Width = (int)(dgvTiles.Width * 0.25);

            dgvTiles.Columns.Add(colTileImg);
            dgvTiles.Columns.Add(colTileText);

            dgvTiles.ColumnHeadersVisible = false;
            dgvTiles.RowHeadersVisible = false;
            dgvTiles.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvTiles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvTiles.AllowUserToAddRows = false;

            // Control Controls
            cmbControlType.Items.Add("Textbox");
            cmbControlType.Items.Add("Button");
            cmbControlType.Items.Add("Percentage Bar");
            cmbControlType.Items.Add("Options");
            cmbControlType.Items.Add("Combobox");
            cmbControlType.SelectedIndex = 0;
        }
        public void Reload()
        {
            ClearUI();

            string sXml = EditorManager.GetUICollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gUIDoc = xLoad;

            XmlNode oUIs = gUIDoc.SelectSingleNode("//fpxUI");
            foreach (XmlNode oUI in oUIs.ChildNodes)
            {
                fpxUI oU = new fpxUI();
                oU.ID = int.Parse(oUI.Attributes["ID"].Value);
                oU.Name = oUI.Attributes["Name"].Value;
                oU.Width = int.Parse(oUI.Attributes["width"].Value);
                oU.Height = int.Parse(oUI.Attributes["height"].Value);
                oU.Static = bool.Parse(oUI.Attributes["static"].Value);
                oU.X = int.Parse(oUI.Attributes["x"].Value);
                oU.Y = int.Parse(oUI.Attributes["y"].Value);

                foreach (XmlNode oLayer in oUI.FirstChild.ChildNodes)
                {
                    fpxUILayer oL = new fpxUILayer();
                    oL.LayerID = int.Parse(oLayer.Attributes["ID"].Value);
                    oL.ScrollX = decimal.Parse(oLayer.Attributes["ScrollX"].Value);
                    oL.ScrollY = decimal.Parse(oLayer.Attributes["ScrollY"].Value);

                    foreach (XmlNode child in oLayer.ChildNodes)
                    {
                        if (child.Name == "Sprites")
                        {
                            foreach (XmlNode oSprite in child.ChildNodes)
                            {

                                fpxSprite oS = new fpxSprite();
                                oS.SpriteName = oSprite.Attributes["Name"].Value;
                                oS.SpriteSheet = oSprite.Attributes["SpriteSheet"].Value;

                                oL.UISprites.Add(oS);
                            }
                        }
                        else if (child.Name == "Scenery")
                        {
                            XmlNode oScenery = child;

                            if (!string.IsNullOrWhiteSpace(oScenery.Attributes["Sprite"].Value))
                            {
                                fpxMapScenery oUIScenery = new fpxMapScenery();
                                oUIScenery.SpriteName = oScenery.Attributes["Sprite"].Value;
                                oUIScenery.Flipped = bool.Parse(oScenery.Attributes["Flipped"].Value);

                                Bitmap sheet = new Bitmap(Path.Combine(Utility.SCENERY_PATH, oUIScenery.SpriteName));
                                Point oSpriteLocation = new Point(0, 0);
                                Size oSpriteSize = new Size(sheet.Width, sheet.Height);
                                Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                                string sFlip = "";
                                if (bool.Parse(oScenery.Attributes["Flipped"].Value))
                                    sFlip += "Flipped";

                                Bitmap sprite = (Bitmap)Utility.GetSprite(sheet, oCrop).Clone();

                                if (oUIScenery.Flipped)
                                    sprite.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                foreach (fpxSprite oSprMapping in oL.UISprites)
                                {
                                    if (oSprMapping.SpriteSheet.Equals(oUIScenery.SpriteName) && oSprMapping.SpriteName.Equals(oUIScenery.SpriteName))
                                    {
                                        if (oSprMapping.Sprite == null)
                                            oSprMapping.Sprite = sprite;

                                        oSprMapping.UsageCount++;
                                    }
                                }

                                oUIScenery.Sprite = sprite;
                                oL.UIScenery = oUIScenery;
                            }
                        }
                        else if (child.Name == "Objects")
                        {
                            foreach (XmlNode oObject in child.ChildNodes)
                            {
                                fpxObject obj = new fpxObject();
                                obj.Name = oObject.Attributes["Name"].Value;
                                obj.SpriteSheet = oObject.Attributes["SpriteSheet"].Value;
                                obj.x = int.Parse(oObject.Attributes["x"].Value);
                                obj.y = int.Parse(oObject.Attributes["y"].Value);
                                obj.width = int.Parse(oObject.Attributes["width"].Value);
                                obj.height = int.Parse(oObject.Attributes["height"].Value);
                                obj.Animated = bool.Parse(oObject.Attributes["Animated"].Value);
                                obj.AnimationIndex = int.Parse(oObject.FirstChild.Attributes["Index"].Value);

                                foreach (XmlNode oAnimation in oObject.FirstChild.ChildNodes)
                                {
                                    fpxAnimation anim = new fpxAnimation();
                                    anim.Name = oAnimation.Attributes["Name"].Value;
                                    anim.ReelHeight = int.Parse(oAnimation.Attributes["ReelHeight"].Value);
                                    anim.ReelIndex = int.Parse(oAnimation.Attributes["ReelIndex"].Value);
                                    anim.FrameWidth = int.Parse(oAnimation.Attributes["FrameWidth"].Value);
                                    anim.TotalFrames = int.Parse(oAnimation.Attributes["TotalFrames"].Value);
                                    anim.FrameSpeed = decimal.Parse(oAnimation.Attributes["FrameSpeed"].Value);

                                    obj.Animations.Add(anim);
                                }

                                foreach (XmlNode oHoldGroup in oObject.LastChild.ChildNodes)
                                {
                                    List<fpxHold> oHG = new List<fpxHold>();

                                    foreach (XmlNode oHold in oHoldGroup)
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
                                        oHG.Add(hold);
                                    }

                                    for (int i = 0; i < oHG.Count; i++)
                                    {
                                        fpxHold currH = oHG.ElementAtOrDefault(i);
                                        fpxHold prevH = oHG.ElementAtOrDefault(i - 1);

                                        if (prevH != null)
                                        {
                                            prevH.next = currH;
                                            prevH.nextid = currH.id;
                                            currH.prev = prevH;
                                            currH.previd = prevH.id;

                                            oHG[i] = currH;
                                            oHG[i - 1] = prevH;
                                        }
                                    }

                                    obj.HoldGroups.Add(int.Parse(oHoldGroup.Attributes["id"].Value), oHG);
                                }

                                Bitmap sheet = new Bitmap(Path.Combine(Utility.OBJECTS_PATH, obj.SpriteSheet));
                                Point oSpriteLocation = new Point(obj.x, obj.y);
                                Size oSpriteSize = new Size(obj.width, obj.height);
                                Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                                //oS.Sprite = Utility.GetSprite(gCurrSheet, oCrop);

                                string sFlip = "";
                                if (bool.Parse(oObject.Attributes["Flipped"].Value))
                                    sFlip += "Flipped";

                                Bitmap sprite = (Bitmap)Utility.GetSprite(sheet, oCrop).Clone();

                                if (bool.Parse(oObject.Attributes["Flipped"].Value))
                                    sprite.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                foreach (fpxSprite oSprMapping in oL.UISprites)
                                {
                                    if (oSprMapping.SpriteSheet.Equals(obj.SpriteSheet) && oSprMapping.SpriteName.Equals(obj.Name + sFlip))
                                    {
                                        if (oSprMapping.Sprite == null)
                                            oSprMapping.Sprite = sprite;

                                        oSprMapping.UsageCount++;
                                    }
                                }


                                fpxMapObject o = new fpxMapObject();
                                o.Flipped = bool.Parse(oObject.Attributes["Flipped"].Value);
                                o.MapCoor = new Point(int.Parse(oObject.Attributes["MapCoorX"].Value), int.Parse(oObject.Attributes["MapCoorY"].Value));
                                o.Object = obj;
                                o.Sprite = sprite;

                                oL.UIObjects.Add(o);
                            }
                        }
                        else if (child.Name == "Tiles")
                        {
                            foreach (XmlNode oTile in child.ChildNodes)
                            {
                                fpxTile tile = new fpxTile();
                                tile.Name = oTile.Attributes["Name"].Value;
                                tile.SpriteSheet = oTile.Attributes["SpriteSheet"].Value;
                                tile.x = int.Parse(oTile.Attributes["x"].Value);
                                tile.y = int.Parse(oTile.Attributes["y"].Value);
                                tile.width = int.Parse(oTile.Attributes["width"].Value);
                                tile.height = int.Parse(oTile.Attributes["height"].Value);

                                foreach (XmlNode oHoldGroup in oTile.SelectSingleNode("//Holds").ChildNodes)
                                {
                                    List<fpxHold> oHG = new List<fpxHold>();

                                    foreach (XmlNode oHold in oHoldGroup)
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
                                        oHG.Add(hold);
                                    }

                                    for (int i = 0; i < oHG.Count; i++)
                                    {
                                        fpxHold currH = oHG.ElementAtOrDefault(i);
                                        fpxHold prevH = oHG.ElementAtOrDefault(i - 1);

                                        if (prevH != null)
                                        {
                                            prevH.next = currH;
                                            prevH.nextid = currH.id;
                                            currH.prev = prevH;
                                            currH.previd = prevH.id;

                                            oHG[i] = currH;
                                            oHG[i - 1] = prevH;
                                        }
                                    }

                                    tile.HoldGroups.Add(int.Parse(oHoldGroup.Attributes["id"].Value), oHG);
                                }

                                Bitmap sheet = new Bitmap(Path.Combine(Utility.TILES_PATH, tile.SpriteSheet));
                                Point oSpriteLocation = new Point(tile.x, tile.y);
                                Size oSpriteSize = new Size(tile.width, tile.height);
                                Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                                //oS.Sprite = Utility.GetSprite(gCurrSheet, oCrop);

                                string sFlip = "";

                                if (bool.Parse(oTile.Attributes["Flipped"].Value))
                                    sFlip += "Flipped";

                                Bitmap sprite = (Bitmap)Utility.GetSprite(sheet, oCrop).Clone();

                                if (bool.Parse(oTile.Attributes["Flipped"].Value))
                                    sprite.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                foreach (fpxSprite oSprMapping in oL.UISprites)
                                {
                                    if (oSprMapping.SpriteSheet.Equals(tile.SpriteSheet) && oSprMapping.SpriteName.Equals(tile.Name + sFlip))
                                    {
                                        if (oSprMapping.Sprite == null)
                                            oSprMapping.Sprite = sprite;

                                        oSprMapping.UsageCount++;
                                    }
                                }

                                fpxMapTile oT = new fpxMapTile();
                                oT.Flipped = bool.Parse(oTile.Attributes["Flipped"].Value);
                                oT.MapCoor = new Point(int.Parse(oTile.Attributes["MapCoorX"].Value), int.Parse(oTile.Attributes["MapCoorY"].Value));
                                oT.Tile = tile;
                                oT.Sprite = sprite;

                                oL.UITiles.Add(oT);
                            }
                        }
                        else if (child.Name == "Controls")
                        {
                            foreach (XmlNode oControl in child.ChildNodes)
                            {
                                fpxControl control = new fpxControl();
                                control.Name = oControl.Attributes["Name"].Value;
                                control.Type = oControl.Attributes["Type"].Value;
                                control.LocX = int.Parse(oControl.Attributes["LocX"].Value);
                                control.LocY = int.Parse(oControl.Attributes["LocY"].Value);
                                control.Width = int.Parse(oControl.Attributes["Width"].Value);
                                control.Height = int.Parse(oControl.Attributes["Height"].Value);

                                control.Font = Utility.GetFontFamilyByName(oControl.Attributes["Font"].Value);
                                control.Size = int.Parse(oControl.Attributes["Size"].Value);
                                int iR = int.Parse(oControl.Attributes["ColorR"].Value);
                                int iG = int.Parse(oControl.Attributes["ColorG"].Value);
                                int iB = int.Parse(oControl.Attributes["ColorB"].Value);
                                control.Color = Color.FromArgb(iR, iG, iB);
                                control.Label = bool.Parse(oControl.Attributes["Label"].Value);
                                control.LabelValue = oControl.Attributes["LabelValue"].Value;
                                control.Multiline = bool.Parse(oControl.Attributes["Multiline"].Value);

                                control.SpriteBaseName = oControl.Attributes["ButtonBase"].Value;
                                control.SpriteHoverName = oControl.Attributes["ButtonHover"].Value;
                                control.SpriteClickName = oControl.Attributes["ButtonClick"].Value;
                                control.SpriteFocusName = oControl.Attributes["ButtonFocus"].Value;

                                if (control.SpriteBaseName != "")
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, control.SpriteBaseName));
                                    control.SpriteBase = new Bitmap(oBmp, new Size(control.Width, control.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }
                                   
                                if (control.SpriteHoverName != "")
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, control.SpriteHoverName));
                                    control.SpriteHover = new Bitmap(oBmp, new Size(control.Width, control.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }

                                if (control.SpriteClickName != "")
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, control.SpriteClickName));
                                    control.SpriteClick = new Bitmap(oBmp, new Size(control.Width, control.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }

                                if (control.SpriteFocusName != "")
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, control.SpriteFocusName));
                                    control.SpriteFocus = new Bitmap(oBmp, new Size(control.Width, control.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }

                                control.PercentBarName = oControl.Attributes["PercentBar"].Value;

                                if (control.PercentBarName != "")
                                {
                                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, control.PercentBarName));
                                    control.PercentBar = new Bitmap(oBmp, new Size(control.Width, control.Height));
                                    oBmp.Dispose();
                                    oBmp = null;
                                }

                                oL.UIControls.Add(control);
                            }
                        }
                    }

                    gUILayers[oL.LayerID] = oL;
                }

                foreach (fpxUILayer oUILayer in gUILayers.Values)
                {
                    oU.UILayers.Add(oUILayer);
                }

                gUI.Add(oU);
            }

            foreach (fpxUI oUI in gUI)
            {
                cmbUI.Items.Add(oUI.Name);
            }

            if (cmbUI.Items.Count > 0)
            {
                cmbUI.SelectedIndex = 0;
                ValidateUI();
            }
        }
        public void ReloadObjects()
        {
            ClearObjectCollections();
            gObjDoc.LoadXml(EditorManager.GetObjectCollection().OuterXml);

            XmlNode oCollections = gObjDoc.SelectSingleNode("//fpxObjects");

            foreach (XmlNode oCollection in oCollections.ChildNodes)
            {
                cmbObjCollection.Items.Add(oCollection.Attributes["Name"].Value);
            }

            if (cmbObjCollection.Items.Count > 0)
                cmbObjCollection.SelectedIndex = 0;
        }
        public void ReloadTiles()
        {
            ClearTileSheets();
            gTileDoc.LoadXml(EditorManager.GetTileCollection().OuterXml);

            XmlNode oSheets = gTileDoc.SelectSingleNode("//fpxTiles");

            foreach (XmlNode oSheet in oSheets.ChildNodes)
            {
                cmbTileSheets.Items.Add(oSheet.Attributes["Name"].Value);
            }

            if (cmbTileSheets.Items.Count > 0)
                cmbTileSheets.SelectedIndex = 0;
        }

        private void ResetMode()
        {
            tsbUI.Checked = false;
            tsbScenery.Checked = false;
            tsbTile.Checked = false;
            tsbObject.Checked = false;
            tsbControl.Checked = false;
            tsbAdd.Checked = false;
        }

        private void ManageControls()
        {
            bool bUIMode = false;
            bool bSceneryMode = false;
            bool bTileMode = false;
            bool bObjectMode = false;
            bool bControlMode = false;
            bool bElementListPopulated = tsbList.Items.Count > 0;
            bool bElementItemSelected = tsbList.SelectedItem != null;
            bool bBackgroundSelected = false;
            bool bUICollection = false;
            bool bAdding = false;


            if (cmbUI.SelectedIndex > -1)
            {
                bUICollection = true;

                if (cmbLayer.Items.Count > 0)
                {
                    if (cmbLayer.SelectedIndex == 0)
                        bBackgroundSelected = true;
                }
            }

            bUIMode = tsbUI.Checked;
            bSceneryMode = tsbScenery.Checked;
            bTileMode = tsbTile.Checked;
            bObjectMode = tsbObject.Checked;
            bControlMode = tsbControl.Checked;
            bAdding = tsbAdd.Checked;


            tsbUI.Enabled = !bAdding;
            tsbScenery.Enabled = bUICollection && !bAdding && bBackgroundSelected;
            tsbTile.Enabled = bUICollection && !bAdding;
            tsbObject.Enabled = bUICollection && !bAdding;
            tsbControl.Enabled = bUICollection && !bAdding && !bBackgroundSelected;

            cmbLayer.Enabled = bUICollection;
            chkHideForward.Enabled = bUICollection;
            numScrollX.Enabled = bBackgroundSelected;
            numScrollY.Enabled = bBackgroundSelected;
            btnSceneryClear.Enabled = !string.IsNullOrWhiteSpace(txtScenery.Text);

            cmbUI.Enabled = bUICollection;
            btnUIRemove.Enabled = bUICollection;
            numUIHeight.Enabled = bUICollection;
            numUIWidth.Enabled = bUICollection;
            numUIX.Enabled = bUICollection;
            numUIY.Enabled = bUICollection;
            chkStatic.Enabled = bUICollection;

            cmbControlType.Enabled = bControlMode && bElementListPopulated;
            btnControlProperties.Enabled = bControlMode && bElementListPopulated;
            numControlHeight.Enabled = bControlMode && bElementListPopulated;
            numControlWidth.Enabled = bControlMode && bElementListPopulated;
            numControlX.Enabled = bControlMode && bElementListPopulated;
            numControlY.Enabled = bControlMode && bElementListPopulated;
            pbControlPreview.Enabled = bControlMode && bElementListPopulated;

            tsbAdd.Enabled = (bObjectMode || bTileMode || bControlMode) && !bAdding;
            tsbConfirm.Enabled = (bObjectMode || bTileMode || bControlMode) && tsbAdd.Checked;
            tsbList.Enabled = (bObjectMode || bTileMode || bControlMode) && bElementListPopulated;
            tsbProperties.Enabled = (bObjectMode || bTileMode || bControlMode) && bElementItemSelected;
            tsbDelete.Enabled = (bObjectMode || bTileMode || bControlMode) && bElementItemSelected;

            tsbAdd.Visible = bObjectMode || bTileMode || bControlMode;
            tsbConfirm.Visible = bObjectMode || bTileMode || bControlMode;
            tsbList.Visible = bObjectMode || bTileMode || bControlMode;
            tsbProperties.Visible = bObjectMode || bTileMode || bControlMode;
            tsbDelete.Visible = bObjectMode || bTileMode || bControlMode;

            pnlGlobalProperties.Enabled = !bSceneryMode && !tsbAdd.Checked;
            pnlUI.Visible = bUIMode;
            pnlScenery.Visible = bSceneryMode;
            pnlObjects.Visible = bObjectMode;
            pnlTiles.Visible = bTileMode;
            pnlControls.Visible = bControlMode;

            btnSave.Enabled = Dirty;
            btnCancel.Enabled = Dirty;
        }
        public void ValidateUI()
        {

            // Load all current UI data into an XML document to be saved out.

            XmlNode oUIRoot = gUIDoc.CreateElement("fpxUI");

            foreach(fpxUI oUI in gUI)
            {
                XmlAttribute oID = gUIDoc.CreateAttribute("ID");
                oID.Value = oUI.ID.ToString();
                XmlAttribute oName = gUIDoc.CreateAttribute("Name");
                oName.Value = oUI.Name;
                XmlAttribute oWidth = gUIDoc.CreateAttribute("width");
                oWidth.Value = oUI.Width.ToString();
                XmlAttribute oHeight = gUIDoc.CreateAttribute("height");
                oHeight.Value = oUI.Height.ToString();
                XmlAttribute oX = gUIDoc.CreateAttribute("x");
                oX.Value = oUI.X.ToString();
                XmlAttribute oY = gUIDoc.CreateAttribute("y");
                oY.Value = oUI.Y.ToString();
                XmlAttribute oStatic = gUIDoc.CreateAttribute("static");
                oStatic.Value = oUI.Static.ToString();

                XmlNode oLayers = gUIDoc.CreateNode("element", "Layers", "");

                for (int i = 0; i < cmbLayer.Items.Count; i++)
                {
                    fpxUILayer oL = oUI.UILayers[i];

                    XmlNode oScenery = gUIDoc.CreateNode("element", "Scenery", "");
                    XmlNode oSprites = gUIDoc.CreateNode("element", "Sprites", "");
                    XmlNode oObjects = gUIDoc.CreateNode("element", "Objects", "");
                    XmlNode oTiles = gUIDoc.CreateNode("element", "Tiles", "");
                    XmlNode oControls = gUIDoc.CreateNode("element", "Controls", "");

                    XmlNode oLayer = gUIDoc.CreateNode("element", "Layer", "");
                    XmlAttribute oLayerID = gUIDoc.CreateAttribute("ID");
                    oLayerID.Value = (i).ToString();
                    XmlAttribute oLayerScrollX = gUIDoc.CreateAttribute("ScrollX");
                    oLayerScrollX.Value =  oL.ScrollX.ToString();
                    XmlAttribute oLayerScrollY = gUIDoc.CreateAttribute("ScrollY");
                    oLayerScrollY.Value = oL.ScrollY.ToString();
                    oLayer.Attributes.Append(oLayerID);
                    oLayer.Attributes.Append(oLayerScrollX);
                    oLayer.Attributes.Append(oLayerScrollY);       

                    XmlAttribute oScenerySprite = gUIDoc.CreateAttribute("Sprite");
                    oScenerySprite.Value = oL.UIScenery.SpriteName;
                    XmlAttribute oSceneryFlipped = gUIDoc.CreateAttribute("Flipped");
                    oSceneryFlipped.Value = oL.UIScenery.Flipped.ToString();
                    oScenery.Attributes.Append(oScenerySprite);
                    oScenery.Attributes.Append(oSceneryFlipped);

                    foreach (fpxSprite oSpr in oL.UISprites)
                    {
                        XmlAttribute oSprName = gUIDoc.CreateAttribute("Name");
                        oSprName.Value = oSpr.SpriteName;
                        XmlAttribute oSprSheet = gUIDoc.CreateAttribute("SpriteSheet");
                        oSprSheet.Value = oSpr.SpriteSheet;

                        XmlNode oSprite = gUIDoc.CreateNode("element", "Sprite", "");
                        oSprite.Attributes.Append(oSprName);
                        oSprite.Attributes.Append(oSprSheet);

                        oSprites.AppendChild(oSprite);
                    }

                    foreach (fpxMapObject oObj in oL.UIObjects)
                    {
                        XmlAttribute oObjName = gUIDoc.CreateAttribute("Name");
                        oObjName.Value = oObj.Object.Name;
                        XmlAttribute oObjMapCoorX = gUIDoc.CreateAttribute("MapCoorX");
                        oObjMapCoorX.Value = oObj.MapCoor.X.ToString();
                        XmlAttribute oObjMapCoorY = gUIDoc.CreateAttribute("MapCoorY");
                        oObjMapCoorY.Value = oObj.MapCoor.Y.ToString();
                        XmlAttribute oObjFlipped = gUIDoc.CreateAttribute("Flipped");
                        oObjFlipped.Value = oObj.Flipped.ToString();
                        XmlAttribute oObjFlip = gUIDoc.CreateAttribute("Name");
                        oObjFlip.Value = oObj.Flipped.ToString();
                        XmlAttribute oObjSpriteName = gUIDoc.CreateAttribute("SpriteSheet");
                        oObjSpriteName.Value = oObj.Object.SpriteSheet;
                        XmlAttribute oObjX = gUIDoc.CreateAttribute("x");
                        oObjX.Value = oObj.Object.x.ToString();
                        XmlAttribute oObjY = gUIDoc.CreateAttribute("y");
                        oObjY.Value = oObj.Object.y.ToString();
                        XmlAttribute oObjWidth = gUIDoc.CreateAttribute("width");
                        oObjWidth.Value = oObj.Object.width.ToString();
                        XmlAttribute oObjHeight = gUIDoc.CreateAttribute("height");
                        oObjHeight.Value = oObj.Object.height.ToString();
                        XmlAttribute oAnimated = gUIDoc.CreateAttribute("Animated");
                        oAnimated.Value = oObj.Object.Animated.ToString();
                        XmlNode oAnimations = gUIDoc.CreateNode("element", "Animations", "");
                        XmlAttribute oAnimationIndex = gUIDoc.CreateAttribute("Index");
                        oAnimationIndex.Value = oObj.Object.AnimationIndex.ToString();

                        if (oObj.Object.Animations.Count > 0)
                        {
                            for (int iA = 0; iA < oObj.Object.Animations.Count; iA++)
                            {
                                if (iA == oObj.Object.AnimationIndex - 1)
                                {
                                    fpxAnimation anim = oObj.Object.Animations[iA];

                                    XmlAttribute oAnimName = gUIDoc.CreateAttribute("Name");
                                    oAnimName.Value = anim.Name.ToString();
                                    XmlAttribute oReelHeight = gUIDoc.CreateAttribute("ReelHeight");
                                    oReelHeight.Value = anim.ReelHeight.ToString();
                                    XmlAttribute oReelIndex = gUIDoc.CreateAttribute("ReelIndex");
                                    oReelIndex.Value = anim.ReelIndex.ToString();
                                    XmlAttribute oFrameWidth = gUIDoc.CreateAttribute("FrameWidth");
                                    oFrameWidth.Value = anim.FrameWidth.ToString();
                                    XmlAttribute oTotalFrames = gUIDoc.CreateAttribute("TotalFrames");
                                    oTotalFrames.Value = anim.TotalFrames.ToString();
                                    XmlAttribute oFrameSpeed = gUIDoc.CreateAttribute("FrameSpeed");
                                    oFrameSpeed.Value = anim.FrameSpeed.ToString();

                                    XmlNode oAnim = gUIDoc.CreateNode("element", "Animation", "");
                                    oAnim.Attributes.Append(oAnimName);
                                    oAnim.Attributes.Append(oReelHeight);
                                    oAnim.Attributes.Append(oReelIndex);
                                    oAnim.Attributes.Append(oFrameWidth);
                                    oAnim.Attributes.Append(oTotalFrames);
                                    oAnim.Attributes.Append(oFrameSpeed);
                                    oAnimations.AppendChild(oAnim);
                                }
                            }
                        }

                        oAnimations.Attributes.Append(oAnimationIndex);

                        XmlNode oObject = gUIDoc.CreateNode("element", "Object", "");
                        oObject.Attributes.Append(oObjName);
                        oObject.Attributes.Append(oObjSpriteName);
                        oObject.Attributes.Append(oObjMapCoorX);
                        oObject.Attributes.Append(oObjMapCoorY);
                        oObject.Attributes.Append(oObjFlipped);
                        oObject.Attributes.Append(oObjX);
                        oObject.Attributes.Append(oObjY);
                        oObject.Attributes.Append(oObjWidth);
                        oObject.Attributes.Append(oObjHeight);
                        oObject.Attributes.Append(oAnimated);
                        oObject.AppendChild(oAnimations);

                        oObjects.AppendChild(oObject);
                    }

                    foreach (fpxMapTile oTile in oL.UITiles)
                    {
                        XmlAttribute oTileName = gUIDoc.CreateAttribute("Name");
                        oTileName.Value = oTile.Tile.Name;
                        XmlAttribute oTileMapCoorX = gUIDoc.CreateAttribute("MapCoorX");
                        oTileMapCoorX.Value = oTile.MapCoor.X.ToString();
                        XmlAttribute oTileMapCoorY = gUIDoc.CreateAttribute("MapCoorY");
                        oTileMapCoorY.Value = oTile.MapCoor.Y.ToString();
                        XmlAttribute oTileFlipped = gUIDoc.CreateAttribute("Flipped");
                        oTileFlipped.Value = oTile.Flipped.ToString();
                        XmlAttribute oTileFlip = gUIDoc.CreateAttribute("Name");
                        oTileFlip.Value = oTile.Flipped.ToString();
                        XmlAttribute oTileSpriteName = gUIDoc.CreateAttribute("SpriteSheet");
                        oTileSpriteName.Value = oTile.Tile.SpriteSheet;
                        XmlAttribute oTileX = gUIDoc.CreateAttribute("x");
                        oX.Value = oTile.Tile.x.ToString();
                        XmlAttribute oTileY = gUIDoc.CreateAttribute("y");
                        oY.Value = oTile.Tile.y.ToString();
                        XmlAttribute oTileWidth = gUIDoc.CreateAttribute("width");
                        oTileWidth.Value = oTile.Tile.width.ToString();
                        XmlAttribute oTileHeight = gUIDoc.CreateAttribute("height");
                        oTileHeight.Value = oTile.Tile.height.ToString();

                        XmlNode oTileN = gUIDoc.CreateNode("element", "Tile", "");
                        oTileN.Attributes.Append(oTileName);
                        oTileN.Attributes.Append(oTileSpriteName);
                        oTileN.Attributes.Append(oTileMapCoorX);
                        oTileN.Attributes.Append(oTileMapCoorY);
                        oTileN.Attributes.Append(oTileX);
                        oTileN.Attributes.Append(oTileY);
                        oTileN.Attributes.Append(oTileWidth);
                        oTileN.Attributes.Append(oTileHeight);
                        oTileN.Attributes.Append(oTileFlipped);

                        oTiles.AppendChild(oTileN);
                    }

                    foreach (fpxControl oControl in oL.UIControls)
                    {
                        XmlAttribute oControlName = gUIDoc.CreateAttribute("Name");
                        oControlName.Value = oControl.Name;
                        XmlAttribute oControlType = gUIDoc.CreateAttribute("Type");
                        oControlType.Value = oControl.Type;
                        XmlAttribute oControlWidth = gUIDoc.CreateAttribute("Width");
                        oControlWidth.Value = oControl.Width.ToString();
                        XmlAttribute oControlHeight = gUIDoc.CreateAttribute("Height");
                        oControlHeight.Value = oControl.Height.ToString();
                        XmlAttribute oControlLocX = gUIDoc.CreateAttribute("LocX");
                        oControlLocX.Value = oControl.LocX.ToString();
                        XmlAttribute oControlLocY = gUIDoc.CreateAttribute("LocY");
                        oControlLocY.Value = oControl.LocY.ToString();

                        XmlAttribute oControlMultiline = gUIDoc.CreateAttribute("Multiline");
                        oControlMultiline.Value = oControl.Multiline.ToString();
                        XmlAttribute oControlFont = gUIDoc.CreateAttribute("Font");
                        oControlFont.Value = oControl.Font.Name;
                        XmlAttribute oControlSize = gUIDoc.CreateAttribute("Size");
                        oControlSize.Value = oControl.Size.ToString();
                        XmlAttribute oControlColorR = gUIDoc.CreateAttribute("ColorR");
                        oControlColorR.Value = oControl.Color.R.ToString();
                        XmlAttribute oControlColorG = gUIDoc.CreateAttribute("ColorG");
                        oControlColorG.Value = oControl.Color.G.ToString();
                        XmlAttribute oControlColorB = gUIDoc.CreateAttribute("ColorB");
                        oControlColorB.Value = oControl.Color.B.ToString();
                        XmlAttribute oControlLabel = gUIDoc.CreateAttribute("Label");
                        oControlLabel.Value = oControl.Label.ToString();
                        XmlAttribute oControlLabelValue = gUIDoc.CreateAttribute("LabelValue");
                        oControlLabelValue.Value = oControl.LabelValue;

                        XmlAttribute oControlBtnBase = gUIDoc.CreateAttribute("ButtonBase");
                        oControlBtnBase.Value = oControl.SpriteBaseName;
                        XmlAttribute oControlBtnHover = gUIDoc.CreateAttribute("ButtonHover");
                        oControlBtnHover.Value = oControl.SpriteHoverName;
                        XmlAttribute oControlBtnClick = gUIDoc.CreateAttribute("ButtonClick");
                        oControlBtnClick.Value = oControl.SpriteClickName;
                        XmlAttribute oControlBtnFocus = gUIDoc.CreateAttribute("ButtonFocus");
                        oControlBtnFocus.Value = oControl.SpriteFocusName;

                        XmlAttribute oControlPercentBar = gUIDoc.CreateAttribute("PercentBar");
                        oControlPercentBar.Value = oControl.PercentBarName.ToString();

                        XmlAttribute oControlFlow = gUIDoc.CreateAttribute("FlowDirection");
                        oControlFlow.Value = oControl.FlowDirection;
                        XmlAttribute oControlRadio = gUIDoc.CreateAttribute("Radio");
                        oControlRadio.Value = oControl.Radio.ToString();

                        // Add Chedkbox Options

                        // XmlAttribute oControlColorB = gUIDoc.CreateAttribute("ColorB");
                        //oControlColorB.Value = oControl.Color.B.ToString();

                        // Add Combo Options

                        //XmlAttribute oControlLabel = gUIDoc.CreateAttribute("Label");
                        //oControlLabel.Value = oControl.Label.ToString();
                        //XmlAttribute oControlLabelValue = gUIDoc.CreateAttribute("LabelValue");
                        //oControlLabelValue.Value = oControl.LabelValue;

                        XmlNode oControlN = gUIDoc.CreateNode("element", "Control", "");
                        oControlN.Attributes.Append(oControlName);
                        oControlN.Attributes.Append(oControlType);
                        oControlN.Attributes.Append(oControlWidth);
                        oControlN.Attributes.Append(oControlHeight);
                        oControlN.Attributes.Append(oControlLocX);
                        oControlN.Attributes.Append(oControlLocY);
                        oControlN.Attributes.Append(oControlMultiline);
                        oControlN.Attributes.Append(oControlFont);
                        oControlN.Attributes.Append(oControlSize);
                        oControlN.Attributes.Append(oControlColorR);
                        oControlN.Attributes.Append(oControlColorG);
                        oControlN.Attributes.Append(oControlColorB);
                        oControlN.Attributes.Append(oControlLabel);
                        oControlN.Attributes.Append(oControlLabelValue);
                        oControlN.Attributes.Append(oControlBtnBase);
                        oControlN.Attributes.Append(oControlBtnHover);
                        oControlN.Attributes.Append(oControlBtnClick);
                        oControlN.Attributes.Append(oControlBtnFocus);
                        oControlN.Attributes.Append(oControlPercentBar);
                        oControlN.Attributes.Append(oControlFlow);
                        oControlN.Attributes.Append(oControlRadio);

                        oControls.AppendChild(oControlN);
                    }

                    oLayer.AppendChild(oSprites);
                    oLayer.AppendChild(oScenery);
                    oLayer.AppendChild(oObjects);
                    oLayer.AppendChild(oTiles);
                    oLayer.AppendChild(oControls);

                    oLayers.AppendChild(oLayer);
                }

                XmlNode oUIn = gUIDoc.CreateNode("element", "UI", "");
                oUIn.Attributes.Append(oID);
                oUIn.Attributes.Append(oName);
                oUIn.Attributes.Append(oWidth);
                oUIn.Attributes.Append(oHeight);
                oUIn.Attributes.Append(oX);
                oUIn.Attributes.Append(oY);
                oUIn.Attributes.Append(oStatic);
                oUIn.AppendChild(oLayers);

                oUIRoot.AppendChild(oUIn);
            }

            gUIDoc.RemoveAll();
            gUIDoc.AppendChild(oUIRoot);


            ValidateSave();
            ManageControls();
        }
        public void ValidateSave()
        {
            bool bDirtyCheck = false;

            if (!gUIDoc.OuterXml.Equals(EditorManager.GetUICollection().OuterXml))
                bDirtyCheck = true;

            Dirty = bDirtyCheck;
        }
        #endregion

        #region Helper Methods
        public void ClearUI()
        {
            cmbUI.Items.Clear();
            gUI = new List<fpxUI>();
            gUILayers = new Dictionary<int, fpxUILayer>();
            ClearUiProperties();
        }
        public void ClearUiProperties()
        {
            numUIWidth.Value = 5;
            numUIHeight.Value = 5;
            numUIX.Value = 0;
            numUIY.Value = 0;
            chkStatic.Checked = true;
        }
        public void ClearObjectCollections()
        {
            cmbObjCollection.Items.Clear();
            ClearObjectCategories();
        }
        public void ClearObjectCategories()
        {
            cmbObjCategory.Items.Clear();
            ClearObjectSheets();
        }
        public void ClearObjectSheets()
        {
            cmbObjSheet.Items.Clear();
            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            gCurrSheet = null;

            ClearObjects();
        }
        public void ClearObjects()
        {
            dgvObjects.Rows.Clear();
            gObjects = new List<fpxObject>();
            if (gCurrSprite != null)
                gCurrSprite.Dispose();

            gCurrSprite = null;
        }

        public void ClearTileSheets()
        {
            cmbTileSheets.Items.Clear();
            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            gCurrSheet = null;

            ClearTiles();
        }
        public void ClearTiles()
        {
            dgvTiles.Rows.Clear();
            gTiles = new List<fpxTile>();
            if (gCurrSprite != null)
                gCurrSprite.Dispose();

            gCurrSprite = null;
        }
        #endregion
    }
}
