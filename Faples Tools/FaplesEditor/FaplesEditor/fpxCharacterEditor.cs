using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Linq;

namespace FaplesEditor
{
    public partial class fpxCharacterEditor : UserControl
    {
        #region Declarations
        int iMagnitude = 1;
        XmlDocument gCharacterDoc = null;
        List<fpxCharacter> gCharacters = new List<fpxCharacter>();
        Bitmap gCurrSprite = null;
        int iFrame = 0;

        KeyValuePair<int, fpxSkeletonPart> gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(-1, null);
        int iTargetXAdjustment = 0;
        int iTargetYAdjustment = 0;

        Dictionary<string, Bitmap> gCachedImages = new Dictionary<string, Bitmap>();

        bool bPlayAnimation = false;
        #endregion

        #region Properties
        bool Dirty { get; set; } = false;
        Timer AnimationPlayer { get; } = new Timer();
        #endregion

        #region Constructor
        public fpxCharacterEditor()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Event Handlers
        private void btnSave_Click(object sender, EventArgs e)
        {
            ValidateCharacters();

            string sXml = gCharacterDoc.OuterXml;

            XmlDocument xSave = new XmlDocument();
            xSave.LoadXml(sXml);
            EditorManager.SetCharacterCollection(xSave);

            ValidateSave();
            ManageControls();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel your changes?", "Faples Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result.Equals(DialogResult.Yes))
            {
                string sXml = EditorManager.GetCharacterCollection().OuterXml;

                XmlDocument xLoad = new XmlDocument();
                xLoad.LoadXml(sXml);
                gCharacterDoc = xLoad;

                ClearCharacters();

                foreach (XmlNode oNode in gCharacterDoc.ChildNodes)
                {
                    cmbCharacter.Items.Add(oNode.Attributes["Name"].Value);
                }

                if (cmbCharacter.Items.Count > 0)
                    cmbCharacter.SelectedIndex = 0;
                else
                    cmbCharacter.SelectedIndex = -1;
            }

            ValidateSave();
            ManageControls();
        }
        private void tsbCharacter_Click(object sender, EventArgs e)
        {
            bool bState = tsbCharacter.Checked;

            ResetMode();
            tsbCharacter.Checked = !bState;

            ValidateSave();
            ManageControls();
        }

        private void tsbSkeleton_Click(object sender, EventArgs e)
        {
            bool bState = tsbSkeleton.Checked;

            ResetMode();
            tsbSkeleton.Checked = !bState;

            ValidateSave();
            ManageControls();
        }

        private void tsbAnimation_Click(object sender, EventArgs e)
        {
            bool bState = tsbAnimation.Checked;

            ResetMode();
            tsbAnimation.Checked = !bState;

            ValidateSave();
            ManageControls();
        }

        private void tsbFeatures_Click(object sender, EventArgs e)
        {
            bool bState = tsbFeatures.Checked;

            ResetMode();
            tsbFeatures.Checked = !bState;
            tsbSkin.Checked = tsbFeatures.Checked;
            tsbSkull.Checked = !tsbFeatures.Checked;

            ValidateSave();
            ManageControls();
        }


        private void tsbSkull_Click(object sender, EventArgs e)
        {
            tsbSkull.Checked = !tsbSkull.Checked;

            ManageControls();
            ediCharacter.Invalidate();
        }

        private void tsbSkin_Click(object sender, EventArgs e)
        {
            tsbSkin.Checked = !tsbSkin.Checked;

            ManageControls();
            ediCharacter.Invalidate();
        }
        private void tsbFace_Click(object sender, EventArgs e)
        {
            tsbFace.Checked = !tsbFace.Checked;

            ManageControls();
            ediCharacter.Invalidate();
        }
        private void btnAddCharacter_Click(object sender, EventArgs e)
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

                    if (!cmbCharacter.Items.Contains(sName))
                    {
                        try
                        {
                            fpxCharacter oChar = new fpxCharacter();
                            oChar.Name = sName;

                            gCharacters.Add(oChar);

                            cmbCharacter.Items.Add(sName);

                            if (cmbCharacter.SelectedIndex < 0)
                            {
                                int iIndex = Math.Max(cmbCharacter.Items.Count - 1, 0);

                                cmbCharacter.SelectedIndex = iIndex;
                            }

                            bValid = true;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inserting Character. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Character already exists. Please use a different name.");
                    }
                }
            }

            ValidateCharacters();
        }
        private void btnDeleteCharacter_Click(object sender, EventArgs e)
        {
            ClearCharacterProperties();

            gCharacters.RemoveAt(cmbCharacter.SelectedIndex);
            cmbCharacter.Items.Remove(cmbCharacter.SelectedItem);

            if (cmbCharacter.Items.Count > 0)
                cmbCharacter.SelectedIndex = 0;
            else
                cmbCharacter.SelectedIndex = -1;

            ValidateCharacters();
        }
        private void cmbCharacter_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearCharacterProperties();

            if (cmbCharacter.SelectedIndex > -1)
            {
                foreach (fpxCharacter oCharacter in gCharacters)
                {
                    if (oCharacter.Name.Equals(cmbCharacter.SelectedItem))
                    {
                        chkSkeleton.Checked = oCharacter.UseSkeleton;
                        chkAnimated.Checked = oCharacter.Animated;

                        numSkeletonWidth.Value = oCharacter.FrameWidth;
                        numSkeletonHeight.Value = oCharacter.FrameHeight;

                        foreach (fpxSkeletonPart oPart in oCharacter.Skeleton)
                        {
                            dgvParts.Rows.Add(new DataGridViewRow());
                            DataGridViewRow oRow = dgvParts.Rows[dgvParts.Rows.Count - 1];
                            oRow.MinimumHeight = 96;
                            if (oPart.SpriteBitmap != null)
                            {
                                var oSpriteCopy = new Bitmap(oPart.SpriteBitmap);
                                oRow.Cells[0].Value = oSpriteCopy;
                            }
                            else
                                oRow.Cells[0].Value = null;
                            oRow.Cells[1].Value = oPart.Name;
                        }

                        foreach (fpxCharacterAnimation oAnimation in oCharacter.Animations)
                        {
                            cmbAnimation.Items.Add(oAnimation.Name);
                        }

                        foreach(fpxCharacterFeature oFeature in oCharacter.Features)
                        {
                            dgvFeatures.Rows.Add(new DataGridViewRow());
                            DataGridViewRow oRow = dgvFeatures.Rows[dgvFeatures.Rows.Count - 1];
                            oRow.MinimumHeight = 96;
                            if (oFeature.SpriteBitmap != null)
                            {
                                var oSpriteCopy = new Bitmap(oFeature.SpriteBitmap);
                                oRow.Cells[0].Value = oSpriteCopy;
                            }
                            else
                                oRow.Cells[0].Value = null;
                            oRow.Cells[1].Value = oFeature.Name;
                        }

                        if (oCharacter.Sprites.Count > 0)
                        {
                            cmbPartSprite.Items.Clear();
                            cmbPartSprite.Items.Add("None");

                            cmbFeatureSprite.Items.Clear();
                            cmbFeatureSprite.Items.Add("None");

                            foreach (string sSprite in oCharacter.Sprites)
                            {
                                Bitmap spriteSnap = new Bitmap(Path.Combine(Utility.CHARACTER_PATH, sSprite));

                                int iSmallWidth = spriteSnap.Width;
                                int iSmallHeight = spriteSnap.Height;

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

                                dgvSprites.Rows.Add(new DataGridViewRow());
                                DataGridViewRow oRow = dgvSprites.Rows[dgvSprites.Rows.Count - 1];
                                oRow.MinimumHeight = 96;
                                oRow.Cells[0].Value = spriteSnap;
                                oRow.Cells[1].Value = sSprite;

                                cmbPartSprite.Items.Add(sSprite);
                                cmbFeatureSprite.Items.Add(sSprite);
                            }

                            if (dgvSprites.Rows.Count > 0)
                            {
                                dgvSprites.Rows[0].Selected = true;
                            }
                        }

                        if (dgvParts.Rows.Count > 0)
                            dgvParts.Rows[0].Selected = true;

                        if (cmbAnimation.Items.Count > 0)
                            cmbAnimation.SelectedIndex = 0;    
                        
                        if(dgvFeatures.Rows.Count > 0)
                            dgvFeatures.Rows[0].Selected = true;
                    }
                }

                ediCharacter.Invalidate();
            }
        }
        private void chkSkeleton_Click(object sender, EventArgs e)
        {
            if (cmbCharacter.SelectedIndex > -1)
                gCharacters[cmbCharacter.SelectedIndex].UseSkeleton = chkSkeleton.Checked;

            ValidateCharacters();
        }

        private void chkAnimated_Click(object sender, EventArgs e)
        {
            if (cmbCharacter.SelectedIndex > -1)
                gCharacters[cmbCharacter.SelectedIndex].Animated = chkAnimated.Checked;

            ValidateCharacters();
        }
        private void btnAddSprite_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CHARACTER_PATH);
                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CHARACTER_PATH))
                    {
                        MessageBox.Show("Please select image within 'Characters' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    string sName = opf.SafeFileName;

                    if (!gCharacters[cmbCharacter.SelectedIndex].Sprites.Contains(sName))
                    {
                        gCharacters[cmbCharacter.SelectedIndex].Sprites.Add(sName);

                        Bitmap spriteSnap = new Bitmap(opf.FileName);

                        int iSmallWidth = spriteSnap.Width;
                        int iSmallHeight = spriteSnap.Height;

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

                        dgvSprites.Rows.Add(new DataGridViewRow());
                        DataGridViewRow oRow = dgvSprites.Rows[dgvSprites.Rows.Count - 1];
                        oRow.MinimumHeight = 96;
                        oRow.Cells[0].Value = spriteSnap;
                        oRow.Cells[1].Value = sName;

                        cmbPartSprite.Items.Add(sName);
                        cmbFeatureSprite.Items.Add(sName);

                        if (dgvSprites.SelectedRows.Count < 1)
                        {
                            int iIndex = Math.Max(dgvSprites.Rows.Count - 1, 0);

                            dgvSprites.Rows[iIndex].Selected = true;
                        }

                        if (gCurrSprite != null)
                        {
                            gCurrSprite.Dispose();
                            gCurrSprite = null;
                        }

                        gCurrSprite = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CHARACTER_PATH, sName));

                        bValid = true;

                    }
                    else
                    {
                        MessageBox.Show("Sprite already exists. Please select a different image.");
                    }
                }
                else
                {
                    bValid = true;
                }
            }

            ValidateCharacters();
        }

        private void btnDeleteSprite_Click(object sender, EventArgs e)
        {
            if (gCurrSprite != null)
            {
                gCurrSprite.Dispose();
                gCurrSprite = null;
            }

            string sName = dgvSprites.SelectedRows[0].Cells[1].Value.ToString();

            gCharacters[cmbCharacter.SelectedIndex].Sprites.Remove(sName);
            dgvSprites.Rows.Remove(dgvSprites.SelectedRows[0]);

            ValidateCharacters();
            ediCharacter.Invalidate();
        }

        private void dgvSprites_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSprites.SelectedRows.Count > 0)
            {
                if (dgvSprites.SelectedRows[0].Cells[1].Value != null)
                {
                    string sName = dgvSprites.SelectedRows[0].Cells[1].Value.ToString();

                    if (gCurrSprite != null)
                    {
                        gCurrSprite.Dispose();
                        gCurrSprite = null;
                    }

                    gCurrSprite = new Bitmap(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CHARACTER_PATH, sName));
                }
            }

            ediCharacter.Invalidate();
        }
        private void numSkeletonWidth_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCharacter.SelectedIndex != -1)
            {
                gCharacters[cmbCharacter.SelectedIndex].FrameWidth = (int)numSkeletonWidth.Value;
                ediCharacter.Width = (int)numSkeletonWidth.Value;

                ValidateCharacters();
            }
        }

        private void numSkeletonHeight_ValueChanged(object sender, EventArgs e)
        {
            if (cmbCharacter.SelectedIndex != -1)
            {
                gCharacters[cmbCharacter.SelectedIndex].FrameHeight = (int)numSkeletonHeight.Value;
                ediCharacter.Height = (int)numSkeletonHeight.Value;

                ValidateCharacters();
            }
        }

        private void btnAddPart_Click(object sender, EventArgs e)
        {
            var oSkeletonDialog = new fpxSkeletonDialog();

            bool bValid = false;

            while (!bValid)
            {
                try
                {
                    fpxCharacter oChar = gCharacters[cmbCharacter.SelectedIndex];
                    fpxSkeletonPart oPart = new fpxSkeletonPart();
                    oPart.Name = "Part" + dgvParts.Rows.Count;

                    if (oChar.Skeleton.Count == 0)
                    {
                        oPart.Type = "Head";

                        fpxSkeletonPoint oStartPoint = new fpxSkeletonPoint();
                        oStartPoint.ID = Guid.NewGuid();
                        oStartPoint.Location = new Point(50, 50);
                        fpxSkeletonPoint oEndPoint = new fpxSkeletonPoint();
                        oEndPoint.ID = Guid.NewGuid();
                        oEndPoint.Location = new Point(50, 70);

                        oPart.StartPoint = oStartPoint;
                        oPart.EndPoint = oEndPoint;
                    }
                    else
                    {
                        var oPoints = new Dictionary<Guid, fpxSkeletonPoint>();

                        foreach (fpxSkeletonPart oExistingPart in oChar.Skeleton)
                        {
                            if (!oPoints.ContainsKey(oExistingPart.StartPoint.ID))
                                oPoints.Add(oExistingPart.StartPoint.ID, oExistingPart.StartPoint);

                            if (!oPoints.ContainsKey(oExistingPart.EndPoint.ID))
                                oPoints.Add(oExistingPart.EndPoint.ID, oExistingPart.EndPoint);
                        }

                        oSkeletonDialog.SetSkeleton(oPoints.Values.ToList());
                        oSkeletonDialog.ShowDialog();

                        if (oSkeletonDialog.DialogResult == DialogResult.Cancel)
                            return;

                        if (oSkeletonDialog.DialogResult == DialogResult.OK)
                        {
                            oPart.Type = oSkeletonDialog.PartType;

                            oPart.StartPoint = oPoints[oSkeletonDialog.PointID];

                            oPart.EndPoint = new fpxSkeletonPoint();
                            oPart.EndPoint.ID = Guid.NewGuid();
                            oPart.EndPoint.Location = new Point(oPart.StartPoint.Location.X + 10, oPart.StartPoint.Location.Y + 10);
                        }
                    }


                    gCharacters[cmbCharacter.SelectedIndex].Skeleton.Add(oPart);

                    dgvParts.Rows.Add(new DataGridViewRow());
                    DataGridViewRow oRow = dgvParts.Rows[dgvParts.Rows.Count - 1];
                    oRow.MinimumHeight = 96;
                    oRow.Cells[0].Value = null;
                    oRow.Cells[1].Value = oPart.Name;

                    cmbOriginPoint.Items.Clear();

                    for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count; i++)
                    {
                        fpxSkeletonPart part = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i];

                        if (i == 0)
                            cmbOriginPoint.Items.Add(part.StartPoint.ID);

                        if (part.EndPoint != oPart.EndPoint)
                            cmbOriginPoint.Items.Add(part.EndPoint.ID);
                    }

                    bValid = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting new part. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (dgvParts.SelectedRows.Count > 0)
                cmbOriginPoint.SelectedItem = gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].StartPoint.ID;

            ValidateCharacters();
        }

        private void btnDeletePart_Click(object sender, EventArgs e)
        {
            if (dgvParts.Rows.Count > 1 && dgvParts.SelectedRows[0].Index != 0)
            {
                if (gCurrSprite != null)
                {
                    gCurrSprite.Dispose();
                    gCurrSprite = null;
                }

                fpxSkeletonPoint oNewStart = gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].StartPoint;
                fpxSkeletonPoint oRemovedPoint = gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].EndPoint;

                gCharacters[cmbCharacter.SelectedIndex].Skeleton.RemoveAt(dgvParts.SelectedRows[0].Index);

                dgvParts.Rows.Remove(dgvParts.SelectedRows[0]);

                for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count; i++)
                {
                    fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i];

                    if (oPart.StartPoint.ID == oRemovedPoint.ID)
                    {
                        gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].StartPoint = oNewStart;
                    }
                }
            }

            ValidateCharacters();
            ediCharacter.Invalidate();
        }

        private void dgvParts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvParts.SelectedRows.Count > 0)
            {
                fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index];

                txtPartName.Text = oPart.Name;

                cmbOriginPoint.Items.Clear();

                for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count; i++)
                {
                    fpxSkeletonPart part = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i];

                    if (i == 0)
                        cmbOriginPoint.Items.Add(part.StartPoint.ID);

                    if (part.EndPoint != oPart.EndPoint)
                        cmbOriginPoint.Items.Add(part.EndPoint.ID);
                }

                cmbOriginPoint.SelectedItem = oPart.StartPoint.ID;
                txtEndPoint.Text = oPart.EndPoint.ID.ToString();
                cmbPartType.SelectedItem = oPart.Type;

                if (oPart.Sprite == "")
                    cmbPartSprite.SelectedItem = "None";
                else
                    cmbPartSprite.SelectedItem = oPart.Sprite;


                numZIndex.Value = oPart.ZIndex;
                numOffsetX.Value = oPart.OffsetX;
                numOffsetY.Value = oPart.OffsetY;

                ManageControls();
            }
        }

        private void txtPartName_TextChanged(object sender, EventArgs e)
        {
            if (dgvParts.SelectedRows.Count > 0)
            {
                gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].Name = txtPartName.Text;
                dgvParts.SelectedRows[0].Cells[1].Value = txtPartName.Text;

                ValidateCharacters();
            }
        }
        private void cmbOriginPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count; i++)
            {
                if (i == 0)
                {
                    if (gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].StartPoint.ID.ToString() == cmbOriginPoint.SelectedItem.ToString())
                    {
                        gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].StartPoint = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].StartPoint;
                        break;
                    }
                }
                if (gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].EndPoint.ID.ToString() == cmbOriginPoint.SelectedItem.ToString())
                {
                    gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].StartPoint = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].EndPoint;
                    break;
                }
            }

            ValidateCharacters();
        }

        private void cmbPartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].Type = cmbPartType.SelectedItem.ToString();

            ValidateCharacters();
        }
        private void cmbPartSprite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPartSprite.SelectedItem.ToString() == "None")
            {
                gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].Sprite = "";
                pbSpritePreview.Image = null;

                if (gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].SpriteBitmap != null)
                {
                    gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].SpriteBitmap.Dispose();
                    gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].SpriteBitmap = null;
                }

                dgvParts.SelectedRows[0].Cells[0].Dispose();
                dgvParts.SelectedRows[0].Cells[0].Value = null;
            }
            else
            {
                fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index];

                oPart.Sprite = cmbPartSprite.SelectedItem.ToString();

                string sSprite = gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].Sprite;

                if (pbSpritePreview.Image != null)
                {
                    pbSpritePreview.Image.Dispose();
                    pbSpritePreview.Image = null;
                }

                if (oPart.SpriteBitmap != null)
                {
                    oPart.SpriteBitmap.Dispose();
                    oPart.SpriteBitmap = null;
                }

                Bitmap oSprite = GetCachedImage(sSprite);

                int x1 = oPart.StartPoint.Location.X;
                int y1 = oPart.StartPoint.Location.Y;
                int x2 = oPart.EndPoint.Location.X;
                int y2 = oPart.EndPoint.Location.Y;

                int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                float xDiff = x2 - x1;
                float yDiff = y2 - y1;
                float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                oPart.RelWidth = oSprite.Width;
                oPart.RelHeight = oSprite.Height;

                gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index] = Utility.RotatePart(oPart, oSprite, angle);


                Rectangle oCrop = new Rectangle(0, 0, oSprite.Width, oSprite.Height);
                Bitmap spriteSnap = Utility.GetSprite(GetCachedImage(sSprite), oCrop);

                int iSmallWidth = oSprite.Width;
                int iSmallHeight = oSprite.Height;

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

                dgvParts.SelectedRows[0].Cells[0].Dispose();
                dgvParts.SelectedRows[0].Cells[0].Value = spriteSnap;

                pbSpritePreview.Image = Utility.ResizeImage(new Bitmap(Path.Combine(Utility.CHARACTER_PATH, sSprite)), pbSpritePreview.Width, pbSpritePreview.Height);

            }

            ValidateCharacters();
        }
        private void numZIndex_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].ZIndex = (int)numZIndex.Value;

            ValidateCharacters();
        }

        private void numOffsetStart_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].OffsetX = (int)numOffsetX.Value;

            ValidateCharacters();
        }

        private void numOffsetEnd_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Skeleton[dgvParts.SelectedRows[0].Index].OffsetY = (int)numOffsetY.Value;

            ValidateCharacters();
        }
        private void btnNewAnimation_Click(object sender, EventArgs e)
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

                    if (!cmbAnimation.Items.Contains(sName))
                    {
                        fpxCharacterAnimation oAnimation = new fpxCharacterAnimation();
                        oAnimation.Name = sName;
                        gCharacters[cmbCharacter.SelectedIndex].Animations.Add(oAnimation);

                        cmbAnimation.Items.Add(sName);

                        bValid = true;

                        if (cmbAnimation.SelectedIndex < 0)
                        {
                            int iIndex = Math.Max(cmbAnimation.Items.Count - 1, 0);

                            cmbAnimation.SelectedIndex = iIndex;

                            ediCharacter.Size = new Size(5, 5);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Animation already exists. Please use a different name.");
                    }
                }
            }

            oAddDialog.Close();
            ValidateCharacters();
        }

        private void btnRemoveAnimation_Click(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Animations.RemoveAt(cmbAnimation.SelectedIndex);
            cmbAnimation.Items.Remove(cmbAnimation.SelectedItem);

            if (cmbAnimation.Items.Count > 0)
                cmbAnimation.SelectedIndex = 0;
            else
                cmbAnimation.SelectedIndex = -1;

            ValidateCharacters();
        }
        private void cmbAnimation_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearAnimationProperties();

            fpxCharacterAnimation oAnim = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex];

            numFrameWidth.Value = oAnim.Width;
            numFrameHeight.Value = oAnim.Height;
            numFrameSpeed.Value = oAnim.Speed;

            for (int i = 0; i < oAnim.Frames.Count; i++)
            {
                var listViewItem = lvwFrames.Items.Add("Frame " + i);
                listViewItem.ImageKey = "FrameKey";
            }

            if (lvwFrames.Items.Count > 0)
            {
                lvwFrames.Items[0].Selected = true;
            }

            ediCharacter.Invalidate();
        }
        private void numFrameWidth_ValueChanged(object sender, EventArgs e)
        {
            if (cmbAnimation.SelectedIndex != -1 && gCharacters[cmbCharacter.SelectedIndex].Animations.Count > 0)
                gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Width = (int)numFrameWidth.Value;

            ValidateCharacters();
        }

        private void numFrameHeight_ValueChanged(object sender, EventArgs e)
        {
            if (cmbAnimation.SelectedIndex != -1 && gCharacters[cmbCharacter.SelectedIndex].Animations.Count > 0)
                gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Height = (int)numFrameHeight.Value;

            ValidateCharacters();
        }

        private void numFrameSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (cmbAnimation.SelectedIndex != -1 && gCharacters[cmbCharacter.SelectedIndex].Animations.Count > 0)
                gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Speed = numFrameSpeed.Value;

            ValidateCharacters();
        }
        private void btnAddFrame_Click(object sender, EventArgs e)
        {
            var listViewItem = lvwFrames.Items.Add("Frame " + (lvwFrames.Items.Count + 1));
            listViewItem.ImageKey = "FrameKey";

            fpxAnimationFrame oFrame = new fpxAnimationFrame();

            fpxCharacterAnimation oAnimation = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex];

            if (oAnimation.Frames.Count > 0)
            {
                int iFrame = oAnimation.Frames.Count - 1;

                if (oAnimation.Frames[iFrame].Skeleton.Count > 0)
                {
                    oFrame.Skeleton = new List<fpxSkeletonPart>();

                    foreach (fpxSkeletonPart oPart in oAnimation.Frames[iFrame].Skeleton)
                    {
                        int iIndex = oAnimation.Frames[iFrame].Skeleton.IndexOf(oPart);

                        fpxSkeletonPart newPart = new fpxSkeletonPart
                        {
                            Name = oPart.Name,
                            StartPoint = new fpxSkeletonPoint
                            {
                                ID = oPart.StartPoint.ID,
                                Location = oPart.StartPoint.Location
                            },
                            EndPoint = new fpxSkeletonPoint
                            {
                                ID = oPart.EndPoint.ID,
                                Location = oPart.EndPoint.Location
                            },
                            RelWidth = oPart.RelWidth,
                            RelHeight = oPart.RelHeight,
                            OffsetX = oPart.OffsetX,
                            OffsetY = oPart.OffsetY,
                            Sprite = oPart.Sprite,
                            Type = oPart.Type,
                            ZIndex = oPart.ZIndex
                        };

                        oFrame.Skeleton.Add(newPart);

                        if (!string.IsNullOrWhiteSpace(newPart.Sprite))
                        {
                            int x1 = oPart.StartPoint.Location.X;
                            int y1 = oPart.StartPoint.Location.Y;
                            int x2 = oPart.EndPoint.Location.X;
                            int y2 = oPart.EndPoint.Location.Y;

                            int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                            int xDiff = x2 - x1;
                            int yDiff = y2 - y1;
                            float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                            Bitmap oChildSprite = GetCachedImage(oFrame.Skeleton[iIndex].Sprite);

                            oFrame.Skeleton[iIndex] = Utility.RotatePart(newPart, oChildSprite, angle);
                        }
                    }
                }
            }

            if (oFrame.Skeleton.Count < 1)
            {
                if (gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count > 0)
                {
                    oFrame.Skeleton = new List<fpxSkeletonPart>();

                    foreach (fpxSkeletonPart oPart in gCharacters[cmbCharacter.SelectedIndex].Skeleton)
                    {
                        int iIndex = gCharacters[cmbCharacter.SelectedIndex].Skeleton.IndexOf(oPart);

                        fpxSkeletonPart newPart = new fpxSkeletonPart
                        {
                            Name = oPart.Name,
                            StartPoint = new fpxSkeletonPoint
                            {
                                ID = oPart.StartPoint.ID,
                                Location = oPart.StartPoint.Location
                            },
                            EndPoint = new fpxSkeletonPoint
                            {
                                ID = oPart.EndPoint.ID,
                                Location = oPart.EndPoint.Location
                            },
                            RelWidth = oPart.RelWidth,
                            RelHeight = oPart.RelHeight,
                            OffsetX = oPart.OffsetX,
                            OffsetY = oPart.OffsetY,
                            Sprite = oPart.Sprite,
                            Type = oPart.Type,
                            ZIndex = oPart.ZIndex
                        };

                        oFrame.Skeleton.Add(newPart);

                        if (!string.IsNullOrWhiteSpace(newPart.Sprite))
                        {
                            int x1 = oPart.StartPoint.Location.X;
                            int y1 = oPart.StartPoint.Location.Y;
                            int x2 = oPart.EndPoint.Location.X;
                            int y2 = oPart.EndPoint.Location.Y;

                            int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                            int xDiff = x2 - x1;
                            int yDiff = y2 - y1;
                            float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                            Bitmap oChildSprite = GetCachedImage(oFrame.Skeleton[iIndex].Sprite);

                            oFrame.Skeleton[iIndex] = Utility.RotatePart(newPart, oChildSprite, angle);
                        }
                    }
                }
            }

            oAnimation.Frames.Add(oFrame);
            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex] = oAnimation;

            ValidateCharacters();
            ediCharacter.Invalidate();
        }

        private void btnResetSkeleton_Click(object sender, EventArgs e)
        {
            fpxAnimationFrame oFrame = new fpxAnimationFrame();

            if (gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count > 0)
            {
                oFrame.Skeleton = new List<fpxSkeletonPart>();

                foreach (fpxSkeletonPart oPart in gCharacters[cmbCharacter.SelectedIndex].Skeleton)
                {
                    fpxSkeletonPart newPart = new fpxSkeletonPart
                    {
                        StartPoint = new fpxSkeletonPoint
                        {
                            ID = oPart.StartPoint.ID,
                            Location = oPart.StartPoint.Location
                        },
                        EndPoint = new fpxSkeletonPoint
                        {
                            ID = oPart.EndPoint.ID,
                            Location = oPart.EndPoint.Location
                        },
                        RelWidth = oPart.RelWidth,
                        RelHeight = oPart.RelHeight,
                        OffsetX = oPart.OffsetX,
                        OffsetY = oPart.OffsetY,
                        Sprite = oPart.Sprite,
                        Type = oPart.Type,
                        ZIndex = oPart.ZIndex
                    };

                    oFrame.Skeleton.Add(newPart);
                }
            }

            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedIndices[0]] = oFrame;

            ValidateCharacters();
        }

        private void btnRemoveFrame_Click(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames.RemoveAt(lvwFrames.SelectedIndices[0]);
            lvwFrames.Items.RemoveAt(lvwFrames.SelectedIndices[0]);

            for (int i = 0; i < lvwFrames.Items.Count; i++)
            {
                lvwFrames.Items[i].Text = "Frame " + (i + 1);
            }

            ValidateCharacters();
        }
        private void lvwFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwFrames.SelectedIndices.Count > 0)
            {
                dgvSkeletonParts.Rows.Clear();

                var oSkeleton = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedIndices[0]].Skeleton;

                foreach (var oPart in oSkeleton)
                {
                    dgvSkeletonParts.Rows.Add(new DataGridViewRow());
                    DataGridViewRow oRow = dgvSkeletonParts.Rows[dgvSkeletonParts.Rows.Count - 1];
                    oRow.MinimumHeight = 96;
                    if (oPart.SpriteBitmap != null)
                    {
                        var oSpriteCopy = new Bitmap(oPart.SpriteBitmap);
                        oRow.Cells[0].Value = oSpriteCopy;
                    }
                    else
                        oRow.Cells[0].Value = null;
                    oRow.Cells[1].Value = oPart.Name;
                }

                if (dgvSkeletonParts.SelectedRows.Count > 0)
                {
                    dgvSkeletonParts.Rows[0].Selected = true;
                }

                ManageControls();
                ediCharacter.Invalidate();
            }
        }
        private void tsbPlayAnim_Click(object sender, EventArgs e)
        {
            bPlayAnimation = true;

            AnimationPlayer.Interval = (int)Math.Max(1000 * gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Speed, 0.01m);
            AnimationPlayer.Enabled = bPlayAnimation;

            ManageControls();
        }

        private void tsbPauseAnim_Click(object sender, EventArgs e)
        {
            bPlayAnimation = false;

            iFrame = 0;
            AnimationPlayer.Enabled = bPlayAnimation;

            ManageControls();
        }
        private void dgvSkeletonParts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSkeletonParts.SelectedRows.Count > 0)
            {
                var oPart = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedIndices[0]].Skeleton[dgvSkeletonParts.SelectedRows[0].Index];

                txtPartName2.Text = oPart.Name;

                if (pbPartPreview.Image != null)
                {
                    pbPartPreview.Image.Dispose();
                    pbPartPreview.Image = null;
                }

                if (oPart.SpriteBitmap != null)
                {
                    var oSpriteCopy = new Bitmap(oPart.SpriteBitmap);
                    pbPartPreview.Image = Utility.ResizeImage(oSpriteCopy, pbPartPreview.Width, pbPartPreview.Height);
                }

                numPartZIndex.Value = oPart.ZIndex;
                numPartOffsetX.Value = oPart.OffsetX;
                numPartOffsetY.Value = oPart.OffsetY;
            }

            ManageControls();
        }

        private void numPartZIndex_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedIndices[0]].Skeleton[dgvSkeletonParts.SelectedRows[0].Index].ZIndex = (int)numPartZIndex.Value;

            ValidateCharacters();
            ediCharacter.Invalidate();
        }

        private void numPartOffsetX_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedIndices[0]].Skeleton[dgvSkeletonParts.SelectedRows[0].Index].OffsetX = (int)numPartOffsetX.Value;

            ValidateCharacters();
            ediCharacter.Invalidate();
        }

        private void numPartOffsetY_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedIndices[0]].Skeleton[dgvSkeletonParts.SelectedRows[0].Index].OffsetY = (int)numPartOffsetY.Value;

            ValidateCharacters();
            ediCharacter.Invalidate();
        }
        private void btnAddFeature_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                try
                {
                    fpxCharacterFeature oFeature = new fpxCharacterFeature()
                    {
                        Name = "Feature" + (dgvFeatures.Rows.Count + 1)
                    };

                    gCharacters[cmbCharacter.SelectedIndex].Features.Add(oFeature);

                    dgvFeatures.Rows.Add(new DataGridViewRow());
                    DataGridViewRow oRow = dgvFeatures.Rows[dgvFeatures.Rows.Count - 1];
                    oRow.MinimumHeight = 96;
                    oRow.Cells[0].Value = null;
                    oRow.Cells[1].Value = oFeature.Name;

                    bValid = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting new part. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ValidateCharacters();
        }

        private void btnDeleteFeature_Click(object sender, EventArgs e)
        {
            if (dgvFeatures.Rows.Count > 1 && dgvFeatures.SelectedRows[0].Index != 0)
            {
                if (gCurrSprite != null)
                {
                    gCurrSprite.Dispose();
                    gCurrSprite = null;
                }

                gCharacters[cmbCharacter.SelectedIndex].Features.RemoveAt(dgvFeatures.SelectedRows[0].Index);

                dgvFeatures.Rows.Remove(dgvFeatures.SelectedRows[0]);
            }

            ValidateCharacters();
            ediCharacter.Invalidate();
        }


        private void dgvFeatures_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFeatures.SelectedRows.Count > 0)
            {
                cmbFeaturePart.Items.Clear();

                fpxCharacterFeature oFeature = gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index];

                txtFeatureName.Text = oFeature.Name;

                for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count; i++)
                {
                    fpxSkeletonPart part = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i];

                    cmbFeaturePart.Items.Add(part.Name);
                }

                cmbFeaturePart.SelectedItem = oFeature.Part;

                if (oFeature.Sprite == "")
                    cmbFeatureSprite.SelectedItem = "None";
                else
                    cmbFeatureSprite.SelectedItem = oFeature.Sprite;

                numFeatureX.Value = oFeature.OffsetX;
                numFeatureY.Value = oFeature.OffsetY;
                numFeatureWidth.Value = oFeature.Width;
                numFeatureHeight.Value = oFeature.Height;

                ManageControls();
            }
        }

        private void Animation_Tick(object sender, EventArgs e)
        {

        }
        private void ediCharacter_MouseClick(object sender, MouseEventArgs e)
        {
            if (tsbSkeleton.Checked)
            {
                if (gTargetPart.Key != -1)
                {
                    gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(-1, null);
                    iTargetXAdjustment = 0;
                    iTargetYAdjustment = 0;

                    ValidateCharacters();
                }
                else
                {
                    for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count; i++)
                    {
                        fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i];

                        int startX = oPart.StartPoint.Location.X * iMagnitude;
                        int startY = oPart.StartPoint.Location.Y * iMagnitude;

                        int tarX = oPart.EndPoint.Location.X * iMagnitude;
                        int tarY = oPart.EndPoint.Location.Y * iMagnitude;

                        int iPointWidth = 5 * iMagnitude;

                        if (i == 0)
                        {
                            if (startX > e.Location.X - iPointWidth && startX < e.Location.X + iPointWidth && startY > e.Location.Y - iPointWidth && startY < e.Location.Y + iPointWidth)
                            {
                                gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(i, oPart);
                                iTargetXAdjustment = gTargetPart.Value.StartPoint.Location.X;
                                iTargetYAdjustment = gTargetPart.Value.StartPoint.Location.Y;
                                break;
                            }
                            else if (tarX > e.Location.X - iPointWidth && tarX < e.Location.X + iPointWidth && tarY > e.Location.Y - iPointWidth && tarY < e.Location.Y + iPointWidth)
                            {
                                gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(i + 1, oPart);
                                iTargetXAdjustment = gTargetPart.Value.EndPoint.Location.X;
                                iTargetYAdjustment = gTargetPart.Value.EndPoint.Location.Y;
                                break;
                            }
                        }

                        if (tarX > e.Location.X - iPointWidth && tarX < e.Location.X + iPointWidth && tarY > e.Location.Y - iPointWidth && tarY < e.Location.Y + iPointWidth)
                        {
                            gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(i + 1, oPart);
                            iTargetXAdjustment = gTargetPart.Value.EndPoint.Location.X;
                            iTargetYAdjustment = gTargetPart.Value.EndPoint.Location.Y;
                            break;
                        }
                    }
                }
            }
            else if (tsbAnimation.Checked)
            {
                if (gTargetPart.Key != -1)
                {
                    gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(-1, null);
                    iTargetXAdjustment = 0;
                    iTargetYAdjustment = 0;

                    ValidateCharacters();
                }
                else
                {
                    if (lvwFrames.SelectedItems.Count > 0)
                    {
                        fpxCharacterAnimation oAnimation = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex];

                        for (int i = 0; i < oAnimation.Frames[lvwFrames.SelectedItems[0].Index].Skeleton.Count; i++)
                        {
                            fpxSkeletonPart oPart = oAnimation.Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i];

                            int startX = oPart.StartPoint.Location.X * iMagnitude;
                            int startY = oPart.StartPoint.Location.Y * iMagnitude;

                            int tarX = oPart.EndPoint.Location.X * iMagnitude;
                            int tarY = oPart.EndPoint.Location.Y * iMagnitude;

                            int iPointWidth = 5 * iMagnitude;

                            if (i == 0)
                            {
                                if (startX > e.Location.X - iPointWidth && startX < e.Location.X + iPointWidth && startY > e.Location.Y - iPointWidth && startY < e.Location.Y + iPointWidth)
                                {
                                    gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(i, oPart);
                                    iTargetXAdjustment = gTargetPart.Value.StartPoint.Location.X;
                                    iTargetYAdjustment = gTargetPart.Value.StartPoint.Location.Y;
                                    break;
                                }
                                else if (tarX > e.Location.X - iPointWidth && tarX < e.Location.X + iPointWidth && tarY > e.Location.Y - iPointWidth && tarY < e.Location.Y + iPointWidth)
                                {
                                    gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(i + 1, oPart);
                                    iTargetXAdjustment = gTargetPart.Value.EndPoint.Location.X;
                                    iTargetYAdjustment = gTargetPart.Value.EndPoint.Location.Y;
                                    break;
                                }
                            }

                            if (tarX > e.Location.X - iPointWidth && tarX < e.Location.X + iPointWidth && tarY > e.Location.Y - iPointWidth && tarY < e.Location.Y + iPointWidth)
                            {
                                gTargetPart = new KeyValuePair<int, fpxSkeletonPart>(i + 1, oPart);
                                iTargetXAdjustment = gTargetPart.Value.EndPoint.Location.X;
                                iTargetYAdjustment = gTargetPart.Value.EndPoint.Location.Y;
                                break;
                            }
                        }

                        gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex] = oAnimation;
                    }
                }
            }

            ediCharacter.Invalidate();
        }
        private void ediCharacter_MouseMove(object sender, MouseEventArgs e)
        {
            if (tsbSkeleton.Checked)
            {
                if (gTargetPart.Key != -1)
                {
                    int key = 0;

                    int iNewLocationX = e.Location.X / iMagnitude;
                    int iNewLocationY = e.Location.Y / iMagnitude;

                    int xAdjustment = iTargetXAdjustment - iNewLocationX;
                    int yAdjustment = iTargetYAdjustment - iNewLocationY;

                    if (gTargetPart.Key < 1)
                    {
                        gTargetPart.Value.StartPoint.Location = new Point(iNewLocationX, iNewLocationY);
                        gCharacters[cmbCharacter.SelectedIndex].Skeleton[gTargetPart.Key] = gTargetPart.Value;

                        key = gTargetPart.Key;
                    }
                    else
                    {
                        gTargetPart.Value.EndPoint.Location = new Point(iNewLocationX, iNewLocationY);
                        gCharacters[cmbCharacter.SelectedIndex].Skeleton[gTargetPart.Key - 1] = gTargetPart.Value;

                        key = gTargetPart.Key - 1;
                    }

                    if (!string.IsNullOrWhiteSpace(gCharacters[cmbCharacter.SelectedIndex].Skeleton[key].Sprite))
                    {
                        if (gCharacters[cmbCharacter.SelectedIndex].Skeleton[key].SpriteBitmap != null)
                        {
                            gCharacters[cmbCharacter.SelectedIndex].Skeleton[key].SpriteBitmap.Dispose();
                            gCharacters[cmbCharacter.SelectedIndex].Skeleton[key].SpriteBitmap = null;
                        }

                        int x1 = gTargetPart.Value.StartPoint.Location.X;
                        int y1 = gTargetPart.Value.StartPoint.Location.Y;
                        int x2 = gTargetPart.Value.EndPoint.Location.X;
                        int y2 = gTargetPart.Value.EndPoint.Location.Y;

                        int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                        float xDiff = x2 - x1;
                        float yDiff = y2 - y1;
                        float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                        Bitmap sprite = GetCachedImage(gCharacters[cmbCharacter.SelectedIndex].Skeleton[key].Sprite);

                        fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Skeleton[key];

                        gCharacters[cmbCharacter.SelectedIndex].Skeleton[key] = Utility.RotatePart(oPart, sprite, angle);

                        for(int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Features.Count; i++)
                        {
                            fpxCharacterFeature oFeature = gCharacters[cmbCharacter.SelectedIndex].Features[i];

                            if(oFeature.Part == oPart.Name)
                            {
                                Bitmap sSprite = GetCachedImage(oFeature.Sprite);
                                gCharacters[cmbCharacter.SelectedIndex].Features[i] = Utility.RotateFeature(oFeature, sSprite, angle, oPart.Type);
                            }                              
                        }
                    }

                    for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Skeleton.Count; i++)
                    {
                        fpxSkeletonPart oChild = gCharacters[cmbCharacter.SelectedIndex].Skeleton[i];

                        if (gTargetPart.Key < 1)
                        {
                            if (oChild.StartPoint.ID == gTargetPart.Value.StartPoint.ID && oChild.EndPoint.ID != gTargetPart.Value.EndPoint.ID)
                            {
                                gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].EndPoint = gTargetPart.Value.EndPoint;
                            }
                        }
                        else
                        {
                            if (oChild.StartPoint.ID == gTargetPart.Value.EndPoint.ID)
                            {
                                gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].StartPoint = gTargetPart.Value.EndPoint;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(oChild.Sprite))
                        {
                            if (gTargetPart.Key < 1)
                            {
                                if (oChild.StartPoint.ID == gTargetPart.Value.StartPoint.ID && oChild.EndPoint.ID != gTargetPart.Value.EndPoint.ID)
                                {
                                    int x1 = oChild.StartPoint.Location.X;
                                    int y1 = oChild.StartPoint.Location.Y;
                                    int x2 = oChild.EndPoint.Location.X;
                                    int y2 = oChild.EndPoint.Location.Y;

                                    int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                    int xDiff = x2 - x1;
                                    int yDiff = y2 - y1;
                                    float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                    Bitmap oChildSprite = GetCachedImage(gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].Sprite);

                                    gCharacters[cmbCharacter.SelectedIndex].Skeleton[i] = Utility.RotatePart(oChild, oChildSprite, angle);

                                    for (int iF = 0; iF < gCharacters[cmbCharacter.SelectedIndex].Features.Count; iF++)
                                    {
                                        fpxCharacterFeature oFeature = gCharacters[cmbCharacter.SelectedIndex].Features[iF];

                                        if (oFeature.Part == oChild.Name)
                                        {
                                            Bitmap sSprite = GetCachedImage(oFeature.Sprite);
                                            gCharacters[cmbCharacter.SelectedIndex].Features[iF] = Utility.RotateFeature(oFeature, sSprite, angle, oChild.Type);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (oChild.StartPoint.ID == gTargetPart.Value.EndPoint.ID)
                                {
                                    int x1 = oChild.StartPoint.Location.X;
                                    int y1 = oChild.StartPoint.Location.Y;
                                    int x2 = oChild.EndPoint.Location.X;
                                    int y2 = oChild.EndPoint.Location.Y;

                                    int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                    int xDiff = x2 - x1;
                                    int yDiff = y2 - y1;
                                    float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                    Bitmap oChildSprite = GetCachedImage(gCharacters[cmbCharacter.SelectedIndex].Skeleton[i].Sprite);

                                    gCharacters[cmbCharacter.SelectedIndex].Skeleton[i] = Utility.RotatePart(oChild, oChildSprite, angle);

                                    for (int iF = 0; iF < gCharacters[cmbCharacter.SelectedIndex].Features.Count; iF++)
                                    {
                                        fpxCharacterFeature oFeature = gCharacters[cmbCharacter.SelectedIndex].Features[iF];

                                        if (oFeature.Part == oChild.Name)
                                        {
                                            Bitmap sSprite = GetCachedImage(oFeature.Sprite);
                                            gCharacters[cmbCharacter.SelectedIndex].Features[iF] = Utility.RotateFeature(oFeature, sSprite, angle, oChild.Type);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    ediCharacter.Invalidate();
                }
            }
            else if (tsbAnimation.Checked)
            {
                if (lvwFrames.SelectedItems.Count > 0)
                {
                    if (gTargetPart.Key != -1)
                    {
                        int key = 0;

                        int iNewLocationX = e.Location.X / iMagnitude;
                        int iNewLocationY = e.Location.Y / iMagnitude;

                        int xAdjustment = iTargetXAdjustment - iNewLocationX;
                        int yAdjustment = iTargetYAdjustment - iNewLocationY;

                        if (gTargetPart.Key < 1)
                        {
                            key = gTargetPart.Key;

                            gTargetPart.Value.StartPoint.Location = new Point(iNewLocationX, iNewLocationY);
                            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key] = gTargetPart.Value;
                        }
                        else
                        {
                            key = gTargetPart.Key - 1;

                            gTargetPart.Value.EndPoint.Location = new Point(iNewLocationX, iNewLocationY);
                            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key] = gTargetPart.Value;
                        }

                        if (!string.IsNullOrWhiteSpace(gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key].Sprite))
                        {
                            if (gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key].SpriteBitmap != null)
                            {
                                gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key].SpriteBitmap.Dispose();
                                gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key].SpriteBitmap = null;
                            }

                            int x1 = gTargetPart.Value.StartPoint.Location.X;
                            int y1 = gTargetPart.Value.StartPoint.Location.Y;
                            int x2 = gTargetPart.Value.EndPoint.Location.X;
                            int y2 = gTargetPart.Value.EndPoint.Location.Y;

                            int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                            float xDiff = x2 - x1;
                            float yDiff = y2 - y1;
                            float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                            Bitmap sprite = GetCachedImage(gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key].Sprite);

                            fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key];

                            gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[key] = Utility.RotatePart(oPart, sprite, angle);
                        }

                        for (int i = 0; i < gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton.Count; i++)
                        {
                            fpxSkeletonPart oChild = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i];

                            if (gTargetPart.Key < 1)
                            {
                                if (oChild.StartPoint.ID == gTargetPart.Value.StartPoint.ID && oChild.EndPoint.ID != gTargetPart.Value.EndPoint.ID)
                                {
                                    gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i].EndPoint = gTargetPart.Value.EndPoint;
                                }
                            }
                            else
                            {
                                if (oChild.StartPoint.ID == gTargetPart.Value.EndPoint.ID)
                                {
                                    gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i].StartPoint = gTargetPart.Value.EndPoint;
                                }
                            }



                            if (!string.IsNullOrWhiteSpace(oChild.Sprite))
                            {
                                if (gTargetPart.Key < 1)
                                {
                                    if (oChild.StartPoint.ID == gTargetPart.Value.StartPoint.ID && oChild.EndPoint.ID != gTargetPart.Value.EndPoint.ID)
                                    {
                                        int x1 = oChild.StartPoint.Location.X;
                                        int y1 = oChild.StartPoint.Location.Y;
                                        int x2 = oChild.EndPoint.Location.X;
                                        int y2 = oChild.EndPoint.Location.Y;

                                        int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                        int xDiff = x2 - x1;
                                        int yDiff = y2 - y1;
                                        float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                        Bitmap oChildSprite = GetCachedImage(gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i].Sprite);

                                        gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i] = Utility.RotatePart(oChild, oChildSprite, angle);
                                    }
                                }
                                else
                                {

                                    if (oChild.StartPoint.ID == gTargetPart.Value.EndPoint.ID)
                                    {
                                        int x1 = oChild.StartPoint.Location.X;
                                        int y1 = oChild.StartPoint.Location.Y;
                                        int x2 = oChild.EndPoint.Location.X;
                                        int y2 = oChild.EndPoint.Location.Y;

                                        int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                        int xDiff = x2 - x1;
                                        int yDiff = y2 - y1;
                                        float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                        Bitmap oChildSprite = GetCachedImage(gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i].Sprite);

                                        gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton[i] = Utility.RotatePart(oChild, oChildSprite, angle);
                                    }
                                }
                            }
                        }

                        ediCharacter.Invalidate();
                    }
                }
            }
        }
        private void ediCharacter_Paint(object sender, PaintEventArgs e)
        {
            if (tsbCharacter.Checked)
            {
                if (gCurrSprite != null)
                {
                    Graphics g = e.Graphics;

                    int iWidth = Utility.GetGameResolutionX(gCurrSprite.Width);
                    int iHeight = Utility.GetGameResolutionY(gCurrSprite.Height);


                    Rectangle rectObj = new Rectangle(0, 0, iWidth, iHeight);

                    if (rectObj.Width < 5)
                        rectObj.Width = 5;

                    if (rectObj.Height < 5)
                        rectObj.Height = 5;

                    rectObj.Width *= iMagnitude;
                    rectObj.Height *= iMagnitude;

                    ediCharacter.Size = rectObj.Size;

                    g.DrawImage(gCurrSprite, rectObj);
                }
                else
                {
                    Graphics g = e.Graphics;

                    ediCharacter.Size = new Size(5 * iMagnitude, 5 * iMagnitude);
                    g.Clear(Color.FromKnownColor(KnownColor.Control));
                }
            }
            else if (tsbSkeleton.Checked || tsbFeatures.Checked)
            {
                ediCharacter.Size = new Size((int)numSkeletonWidth.Value * iMagnitude, (int)numSkeletonHeight.Value * iMagnitude);

                int width = 5;
                int height = 5;

                if (tsbSkin.Checked)
                {
                    Graphics g = e.Graphics;
                    List<fpxSkeletonPart> oParts = gCharacters[cmbCharacter.SelectedIndex].Skeleton.OrderBy(p => p.ZIndex).ToList();
                    List<fpxCharacterFeature> oFeatures = gCharacters[cmbCharacter.SelectedIndex].Features;

                    foreach (fpxSkeletonPart oPart in oParts)
                    {
                        if (oPart.SpriteBitmap != null)
                        {                          
                            int x1 = oPart.StartPoint.Location.X;
                            int y1 = oPart.StartPoint.Location.Y;
                            int x2 = oPart.EndPoint.Location.X;
                            int y2 = oPart.EndPoint.Location.Y;

                            int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));
                            int radius = diameter / 2;

                            int centerX = (x1 + x2) / 2;
                            int centerY = (y1 + y2) / 2;

                            int trueX = x2 - x1;

                            int limbwidth = Utility.GetGameResolutionX(oPart.SpriteBitmap.Width);
                            int limbHeight = Utility.GetGameResolutionY(oPart.SpriteBitmap.Height);

                            int xOffset = oPart.OffsetX;
                            int yOffset = oPart.OffsetY;

                            float xDiff = x2 - x1;
                            float yDiff = y2 - y1;
                            float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                            if (oPart.Type == "Head")
                            {
                                var iX = (centerX - radius) * iMagnitude;
                                var iY = (centerY - radius) * iMagnitude;
                                var iWidth = (diameter) * iMagnitude;
                                var iHeight = (diameter) * iMagnitude;

                                Rectangle rectObj = new Rectangle(iX, iY, iWidth, iHeight);
                                g.DrawImage(oPart.SpriteBitmap, rectObj);
                            }
                            else if (oPart.Type == "Limb")
                            {
                                int iRelWidth = Utility.GetGameResolutionX(oPart.RelWidth);
                                int iRelHeight = Utility.GetGameResolutionY(oPart.RelHeight);

                                int adjustYBR = iRelWidth - (int)(iRelWidth * (angle / 90));
                                int adjustXTR = iRelWidth - (int)(iRelWidth * ((angle + 90) / 90));
                                int adjustYTL = iRelWidth - (int)(iRelWidth * ((angle + 180) / 90));
                                int adjustXBL = iRelWidth - (int)(iRelWidth * ((180 - angle) / 90));

                                int finalXBR = x1;
                                int finalYBR = y1 - adjustYBR;
                                int finalXTR = x1 - adjustXTR;
                                int finalYTR = y1 - limbHeight;
                                int finalXTL = (x1 - iRelHeight) + (iRelHeight - limbwidth);
                                int finalYTL = y1 - (limbHeight - adjustYTL);
                                int finalXBL = (x1 - limbwidth) + (iRelWidth - adjustXBL);
                                int finalYBL = y1;

                                // Right bottom corner
                                int offsetYBRinit = yOffset - (int)(yOffset * (angle / 90));
                                int offsetXBRinit = xOffset - (int)(xOffset * (angle / 90));
                                int offsetXBR = xOffset - offsetXBRinit - offsetYBRinit;
                                int offsetYBR = yOffset - offsetYBRinit + offsetXBRinit;
                                // Right top corner
                                int offsetYTRinit = yOffset - (int)(yOffset * ((angle + 90) / 90));
                                int offsetXTRinit = xOffset - (int)(xOffset * ((angle + 90) / 90));
                                int offsetXTR = yOffset - offsetYTRinit + offsetXTRinit;
                                int offsetYTR = xOffset - offsetXTRinit - offsetYTRinit;
                                // Left top corner
                                int offsetYTLinit = yOffset - (int)(yOffset * ((angle + 180) / 90));
                                int offsetXTLinit = xOffset - (int)(xOffset * ((angle + 180) / 90));
                                int offsetXTL = xOffset - offsetXTLinit - offsetYTLinit;
                                int offsetYTL = yOffset - offsetYTLinit + offsetXTLinit;
                                // Left bottom corner
                                int offsetYBLinit = yOffset - (yOffset - (int)(yOffset * ((180 - angle) / 90)));
                                int offsetXBLinit = xOffset - (xOffset - (int)(xOffset * ((180 - angle) / 90)));
                                int offsetXBL = yOffset - offsetYBLinit + offsetXBLinit;
                                int offsetYBL = xOffset - offsetXBLinit - offsetYBLinit;


                                var iX = finalXBR - offsetXBR;
                                var iY = finalYBR + offsetYBR;
                                var iWidth = limbwidth;
                                var iHeight = limbHeight;

                                if (angle > 90)
                                {
                                    iX = finalXBL - offsetXBL;
                                    iY = finalYBL - offsetYBL;
                                }
                                else if (angle < -90)
                                {
                                    iX = finalXTL + offsetXTL;
                                    iY = finalYTL - offsetYTL;
                                }
                                else if (angle < 0)
                                {
                                    iX = finalXTR + offsetXTR;
                                    iY = finalYTR + offsetYTR;
                                }

                                iX *= iMagnitude;
                                iY *= iMagnitude;
                                iWidth *= iMagnitude;
                                iHeight *= iMagnitude;

                                Rectangle rectObj = new Rectangle(iX, iY, iWidth, iHeight);

                                g.DrawImage(oPart.SpriteBitmap, rectObj);
                            }

                            if(tsbFace.Checked)
                            {
                                if (oFeatures.Count > 0)
                                {
                                    var colFeatures = oFeatures.Where(f => f.Part == oPart.Name && f.SpriteBitmap != null).ToList();

                                    foreach (var oFeature in colFeatures)
                                    {
                                        if (oPart.Type == "Limb")
                                        {
                                            limbwidth = Utility.GetGameResolutionX(oFeature.SpriteBitmap.Width);
                                            limbHeight = Utility.GetGameResolutionY(oFeature.SpriteBitmap.Height);

                                            xOffset = oFeature.OffsetX;
                                            yOffset = oFeature.OffsetY;

                                            int iRelWidth = Utility.GetGameResolutionX(oFeature.RelWidth);
                                            int iRelHeight = Utility.GetGameResolutionY(oFeature.RelHeight);

                                            int adjustYBR = iRelWidth - (int)(iRelWidth * (angle / 90));
                                            int adjustXTR = iRelWidth - (int)(iRelWidth * ((angle + 90) / 90));
                                            int adjustYTL = iRelWidth - (int)(iRelWidth * ((angle + 180) / 90));
                                            int adjustXBL = iRelWidth - (int)(iRelWidth * ((180 - angle) / 90));

                                            int finalXBR = x1;
                                            int finalYBR = y1 - adjustYBR;
                                            int finalXTR = x1 - adjustXTR;
                                            int finalYTR = y1 - limbHeight;
                                            int finalXTL = (x1 - iRelHeight) + (iRelHeight - limbwidth);
                                            int finalYTL = y1 - (limbHeight - adjustYTL);
                                            int finalXBL = (x1 - limbwidth) + (iRelWidth - adjustXBL);
                                            int finalYBL = y1;

                                            // Right bottom corner
                                            int offsetYBRinit = yOffset - (int)(yOffset * (angle / 90));
                                            int offsetXBRinit = xOffset - (int)(xOffset * (angle / 90));
                                            int offsetXBR = xOffset - offsetXBRinit - offsetYBRinit;
                                            int offsetYBR = yOffset - offsetYBRinit + offsetXBRinit;
                                            // Right top corner
                                            int offsetYTRinit = yOffset - (int)(yOffset * ((angle + 90) / 90));
                                            int offsetXTRinit = xOffset - (int)(xOffset * ((angle + 90) / 90));
                                            int offsetXTR = yOffset - offsetYTRinit + offsetXTRinit;
                                            int offsetYTR = xOffset - offsetXTRinit - offsetYTRinit;
                                            // Left top corner
                                            int offsetYTLinit = yOffset - (int)(yOffset * ((angle + 180) / 90));
                                            int offsetXTLinit = xOffset - (int)(xOffset * ((angle + 180) / 90));
                                            int offsetXTL = xOffset - offsetXTLinit - offsetYTLinit;
                                            int offsetYTL = yOffset - offsetYTLinit + offsetXTLinit;
                                            // Left bottom corner
                                            int offsetYBLinit = yOffset - (yOffset - (int)(yOffset * ((180 - angle) / 90)));
                                            int offsetXBLinit = xOffset - (xOffset - (int)(xOffset * ((180 - angle) / 90)));
                                            int offsetXBL = yOffset - offsetYBLinit + offsetXBLinit;
                                            int offsetYBL = xOffset - offsetXBLinit - offsetYBLinit;

                                            var iX = finalXBR - offsetXBR;
                                            var iY = finalYBR + offsetYBR;
                                            var iWidth = limbwidth;
                                            var iHeight = limbHeight;

                                            if (angle > 90)
                                            {
                                                iX = finalXBL - offsetXBL;
                                                iY = finalYBL - offsetYBL;
                                            }
                                            else if (angle < -90)
                                            {
                                                iX = finalXTL + offsetXTL;
                                                iY = finalYTL - offsetYTL;
                                            }
                                            else if (angle < 0)
                                            {
                                                iX = finalXTR + offsetXTR;
                                                iY = finalYTR + offsetYTR;
                                            }

                                            iX *= iMagnitude;
                                            iY *= iMagnitude;
                                            iWidth *= iMagnitude;
                                            iHeight *= iMagnitude;

                                            Rectangle rectObj = new Rectangle(iX, iY, iWidth, iHeight);

                                            g.DrawImage(oFeature.SpriteBitmap, rectObj);
                                        }
                                        else if (oPart.Type == "Head")
                                        {
                                            var iX = (centerX - radius) * iMagnitude;
                                            var iY = (centerY - radius) * iMagnitude;
                                            var iWidth = (oFeature.Width) * iMagnitude;
                                            var iHeight = (oFeature.Height) * iMagnitude;

                                            var iOffsetX = oFeature.OffsetX * iMagnitude;
                                            var iOffsetY = oFeature.OffsetY * iMagnitude;

                                            Rectangle rectObj = new Rectangle(iX + iOffsetX, iY + iOffsetY, iWidth, iHeight);
                                            g.DrawImage(oFeature.SpriteBitmap, rectObj);
                                        }
                                    }
                                }
                            }                     
                        }
                    }
                }

                if (tsbSkull.Checked)
                {
                    Graphics g = e.Graphics;

                    foreach (fpxSkeletonPart oPart in gCharacters[cmbCharacter.SelectedIndex].Skeleton)
                    {
                        Pen pPoint = new Pen(Color.Green, 3);
                        Pen pPart = new Pen(Color.Green, 3);

                        if (gTargetPart.Key != -1)
                        {
                            if (gTargetPart.Value.EndPoint.ID == oPart.EndPoint.ID)
                                pPoint = new Pen(Color.Blue, 3);
                            else
                                pPoint = new Pen(Color.Green, 3);
                        }

                        int x1 = oPart.StartPoint.Location.X;
                        int y1 = oPart.StartPoint.Location.Y;
                        int x2 = oPart.EndPoint.Location.X;
                        int y2 = oPart.EndPoint.Location.Y;

                        int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));
                        int radius = diameter / 2;

                        int centerX = (x1 + x2) / 2;
                        int centerY = (y1 + y2) / 2;

                        if (oPart.Type == "Head")
                        {
                            var iX = (x1 - width / 2) * iMagnitude;
                            var iY = (y1 - height / 2) * iMagnitude;
                            var iWidth = width * iMagnitude;
                            var iHeight = height * iMagnitude;

                            if (gCharacters[cmbCharacter.SelectedIndex].Skeleton.IndexOf(oPart) == 0)
                            {
                                g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);
                            }

                            iX = (x2 - width / 2) * iMagnitude;
                            iY = (y2 - height / 2) * iMagnitude;

                            g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);

                            var iXCir = (centerX - radius) * iMagnitude;
                            var iYCir = (centerY - radius) * iMagnitude;
                            var iWidthCir = (diameter) * iMagnitude;
                            var iHeightCir = (diameter) * iMagnitude;

                            g.DrawEllipse(pPart, iXCir, iYCir, iWidthCir, iHeightCir);
                        }
                        else if (oPart.Type == "Limb")
                        {
                            var iX = (x1 - width / 2) * iMagnitude;
                            var iY = y1 * iMagnitude;
                            var iWidth = width * iMagnitude;
                            var iHeight = height * iMagnitude;

                            if (gCharacters[cmbCharacter.SelectedIndex].Skeleton.IndexOf(oPart) == 0)
                            {
                                g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);
                            }

                            iX = (x2 - width / 2) * iMagnitude;
                            iY = y2 * iMagnitude;

                            g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);

                            var iPX1 = x1 * iMagnitude;
                            var iPY1 = (y1 + height / 2) * iMagnitude;
                            var iPX2 = x2 * iMagnitude;
                            var iPY2 = (y2 + height / 2) * iMagnitude;

                            g.DrawLine(pPart, new Point(iPX1, iPY1), new Point(iPX2, iPY2));
                        }
                    }
                }
            }
            else if (tsbAnimation.Checked)
            {
                Graphics g = e.Graphics;

                int width = 5;
                int height = 5;

                ediCharacter.Size = new Size((int)numFrameWidth.Value * iMagnitude, (int)numFrameHeight.Value * iMagnitude);

                if (lvwFrames.SelectedItems.Count > 0)
                {
                    if (tsbSkin.Checked)
                    {
                        List<fpxSkeletonPart> oParts = gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton.OrderBy(p => p.ZIndex).ToList();

                        foreach (fpxSkeletonPart oPart in oParts)
                        {
                            if (oPart.SpriteBitmap != null)
                            {
                                int x1 = oPart.StartPoint.Location.X;
                                int y1 = oPart.StartPoint.Location.Y;
                                int x2 = oPart.EndPoint.Location.X;
                                int y2 = oPart.EndPoint.Location.Y;

                                int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));
                                int radius = diameter / 2;

                                int centerX = (x1 + x2) / 2;
                                int centerY = (y1 + y2) / 2;

                                int trueX = x2 - x1;

                                int limbwidth = oPart.SpriteBitmap.Width;
                                int limbHeight = oPart.SpriteBitmap.Height;

                                int xOffset = oPart.OffsetX;
                                int yOffset = oPart.OffsetY;

                                float xDiff = x2 - x1;
                                float yDiff = y2 - y1;
                                float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);
                                if (oPart.Type == "Head")
                                {
                                    var iX = (centerX - radius) * iMagnitude;
                                    var iY = (centerY - radius) * iMagnitude;
                                    var iWidth = diameter * iMagnitude;
                                    var iHeight = diameter * iMagnitude;

                                    Rectangle rectObj = new Rectangle(iX, iY, iWidth, iHeight);
                                    g.DrawImage(oPart.SpriteBitmap, rectObj);
                                }
                                else if (oPart.Type == "Limb")
                                {

                                    int adjustYBR = oPart.RelWidth - (int)(oPart.RelWidth * (angle / 90));
                                    int adjustXTR = oPart.RelWidth - (int)(oPart.RelWidth * ((angle + 90) / 90));
                                    int adjustYTL = oPart.RelWidth - (int)(oPart.RelWidth * ((angle + 180) / 90));
                                    int adjustXBL = oPart.RelWidth - (int)(oPart.RelWidth * ((180 - angle) / 90));

                                    int finalXBR = x1;
                                    int finalYBR = y1 - adjustYBR;
                                    int finalXTR = x1 - adjustXTR;
                                    int finalYTR = y1 - limbHeight;
                                    int finalXTL = (x1 - oPart.RelHeight) + (oPart.RelHeight - limbwidth);
                                    int finalYTL = y1 - (limbHeight - adjustYTL);
                                    int finalXBL = (x1 - limbwidth) + (oPart.RelWidth - adjustXBL);
                                    int finalYBL = y1;

                                    // Right bottom corner
                                    int offsetYBRinit = yOffset - (int)(yOffset * (angle / 90));
                                    int offsetXBRinit = xOffset - (int)(xOffset * (angle / 90));
                                    int offsetXBR = xOffset - offsetXBRinit - offsetYBRinit;
                                    int offsetYBR = yOffset - offsetYBRinit + offsetXBRinit;
                                    // Right top corner
                                    int offsetYTRinit = yOffset - (int)(yOffset * ((angle + 90) / 90));
                                    int offsetXTRinit = xOffset - (int)(xOffset * ((angle + 90) / 90));
                                    int offsetXTR = yOffset - offsetYTRinit + offsetXTRinit;
                                    int offsetYTR = xOffset - offsetXTRinit - offsetYTRinit;
                                    // Left top corner
                                    int offsetYTLinit = yOffset - (int)(yOffset * ((angle + 180) / 90));
                                    int offsetXTLinit = xOffset - (int)(xOffset * ((angle + 180) / 90));
                                    int offsetXTL = xOffset - offsetXTLinit - offsetYTLinit;
                                    int offsetYTL = yOffset - offsetYTLinit + offsetXTLinit;
                                    // Left bottom corner
                                    int offsetYBLinit = yOffset - (yOffset - (int)(yOffset * ((180 - angle) / 90)));
                                    int offsetXBLinit = xOffset - (xOffset - (int)(xOffset * ((180 - angle) / 90)));
                                    int offsetXBL = yOffset - offsetYBLinit + offsetXBLinit;
                                    int offsetYBL = xOffset - offsetXBLinit - offsetYBLinit;


                                    var iX = finalXBR - offsetXBR;
                                    var iY = finalYBR + offsetYBR;
                                    var iWidth = limbwidth;
                                    var iHeight = limbHeight;

                                    if (angle > 90)
                                    {
                                        iX = finalXBL - offsetXBL;
                                        iY = finalYBL - offsetYBL;
                                    }
                                    else if (angle < -90)
                                    {
                                        iX = finalXTL + offsetXTL;
                                        iY = finalYTL - offsetYTL;
                                    }
                                    else if (angle < 0)
                                    {
                                        iX = finalXTR + offsetXTR;
                                        iY = finalYTR + offsetYTR;
                                    }

                                    iX *= iMagnitude;
                                    iY *= iMagnitude;
                                    iWidth *= iMagnitude;
                                    iHeight *= iMagnitude;

                                    Rectangle rectObj = new Rectangle(iX, iY, iWidth, iHeight);

                                    g.DrawImage(oPart.SpriteBitmap, rectObj);
                                }
                            }
                        }
                    }
                    if (tsbSkull.Checked)
                    {
                        if (lvwFrames.SelectedItems[0].Index - 1 != -1)
                        {
                            foreach (fpxSkeletonPart oPart in gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index - 1].Skeleton)
                            {
                                Pen pPoint = new Pen(Color.FromArgb(128, 00, 128, 00), 3);
                                Pen pPart = new Pen(Color.FromArgb(128, 00, 128, 00), 3);

                                if (gTargetPart.Key != -1)
                                {
                                    if (gTargetPart.Value.EndPoint.ID == oPart.EndPoint.ID)
                                        pPoint = new Pen(Color.Blue, 3);
                                    else
                                        pPoint = new Pen(Color.FromArgb(128, 00, 128, 00), 3);
                                }

                                int x1 = oPart.StartPoint.Location.X;
                                int y1 = oPart.StartPoint.Location.Y;
                                int x2 = oPart.EndPoint.Location.X;
                                int y2 = oPart.EndPoint.Location.Y;

                                int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));
                                int radius = diameter / 2;

                                int centerX = (x1 + x2) / 2;
                                int centerY = (y1 + y2) / 2;

                                if (oPart.Type == "Head")
                                {
                                    if (gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton.IndexOf(oPart) == 0)
                                        g.DrawRectangle(pPoint, x1 - width / 2, y1 - height / 2, width, height);

                                    g.DrawRectangle(pPoint, x2 - width / 2, y2 - height / 2, width, height);

                                    g.DrawEllipse(pPart, centerX - radius, centerY - radius, diameter, diameter);
                                }
                                else if (oPart.Type == "Limb")
                                {
                                    if (gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton.IndexOf(oPart) == 0)
                                        g.DrawRectangle(pPoint, x1 - width / 2, y1, width, height);

                                    g.DrawRectangle(pPoint, x2 - width / 2, y2, width, height);

                                    g.DrawLine(pPart, new Point(x1, y1 + height / 2), new Point(x2, y2 + height / 2));
                                }
                            }
                        }

                        foreach (fpxSkeletonPart oPart in gCharacters[cmbCharacter.SelectedIndex].Animations[cmbAnimation.SelectedIndex].Frames[lvwFrames.SelectedItems[0].Index].Skeleton)
                        {
                            Pen pPoint = new Pen(Color.Green, 3);
                            Pen pPart = new Pen(Color.Green, 3);

                            if (gTargetPart.Key != -1)
                            {
                                if (gTargetPart.Value.EndPoint.ID == oPart.EndPoint.ID)
                                    pPoint = new Pen(Color.Blue, 3);
                                else
                                    pPoint = new Pen(Color.Green, 3);
                            }

                            int x1 = oPart.StartPoint.Location.X;
                            int y1 = oPart.StartPoint.Location.Y;
                            int x2 = oPart.EndPoint.Location.X;
                            int y2 = oPart.EndPoint.Location.Y;

                            int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));
                            int radius = diameter / 2;

                            int centerX = (x1 + x2) / 2;
                            int centerY = (y1 + y2) / 2;

                            if (oPart.Type == "Head")
                            {
                                var iX = (x1 - width / 2) * iMagnitude;
                                var iY = (y1 - height / 2) * iMagnitude;
                                var iWidth = width * iMagnitude;
                                var iHeight = height * iMagnitude;

                                if (gCharacters[cmbCharacter.SelectedIndex].Skeleton.IndexOf(oPart) == 0)
                                {
                                    g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);
                                }

                                iX = (x2 - width / 2) * iMagnitude;
                                iY = (y2 - height / 2) * iMagnitude;

                                g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);

                                var iXCir = (centerX - radius) * iMagnitude;
                                var iYCir = (centerY - radius) * iMagnitude;
                                var iWidthCir = (diameter) * iMagnitude;
                                var iHeightCir = (diameter) * iMagnitude;

                                g.DrawEllipse(pPart, iXCir, iYCir, iWidthCir, iHeightCir);
                            }
                            else if (oPart.Type == "Limb")
                            {
                                var iX = (x1 - width / 2) * iMagnitude;
                                var iY = y1 * iMagnitude;
                                var iWidth = width * iMagnitude;
                                var iHeight = height * iMagnitude;

                                if (gCharacters[cmbCharacter.SelectedIndex].Skeleton.IndexOf(oPart) == 0)
                                {
                                    g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);
                                }

                                iX = (x2 - width / 2) * iMagnitude;
                                iY = y2 * iMagnitude;

                                g.DrawRectangle(pPoint, iX, iY, iWidth, iHeight);

                                var iPX1 = x1 * iMagnitude;
                                var iPY1 = (y1 + height / 2) * iMagnitude;
                                var iPX2 = x2 * iMagnitude;
                                var iPY2 = (y2 + height / 2) * iMagnitude;

                                g.DrawLine(pPart, new Point(iPX1, iPY1), new Point(iPX2, iPY2));
                            }
                        }
                    }
                }
            }

            if (tsbFeatures.Checked)
            {

            }
        }

        public void AdjustZoom(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                iMagnitude += 1;

                if (iMagnitude > 4)
                    iMagnitude = 4;
            }
            else
            {
                iMagnitude -= 1;

                if (iMagnitude < 1)
                    iMagnitude = 1;
            }

            ediCharacter.Invalidate();
        }
        #endregion

        #region Main Methods
        public void Prepare()
        {
            ediCharacter.MouseWheel += AdjustZoom;
            AnimationPlayer.Tick += Animation_Tick;

            string sXml = EditorManager.GetCharacterCollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gCharacterDoc = xLoad;

            ResetMode();
            tsbCharacter.Checked = true;

            PrepareControls();
            PrepareIcons();

            ManageControls();
        }
        public void PrepareControls()
        {
            // Sprite DGV
            DataGridViewImageColumn colImg = new DataGridViewImageColumn();
            colImg.Width = (int)(dgvSprites.Width * 0.75);
            DataGridViewTextBoxColumn colText = new DataGridViewTextBoxColumn();
            colText.Width = (int)(dgvSprites.Width * 0.25);

            dgvSprites.Columns.Add(colImg);
            dgvSprites.Columns.Add(colText);

            dgvSprites.ColumnHeadersVisible = false;
            dgvSprites.RowHeadersVisible = false;
            dgvSprites.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvSprites.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvSprites.AllowUserToAddRows = false;

            // Skeleton Part DGV
            DataGridViewImageColumn colPartImg = new DataGridViewImageColumn();
            colPartImg.Width = (int)(dgvSprites.Width * 0.75);
            DataGridViewTextBoxColumn colPartText = new DataGridViewTextBoxColumn();
            colPartText.Width = (int)(dgvSprites.Width * 0.25);

            dgvParts.Columns.Add(colPartImg);
            dgvParts.Columns.Add(colPartText);

            dgvParts.ColumnHeadersVisible = false;
            dgvParts.RowHeadersVisible = false;
            dgvParts.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvParts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvParts.AllowUserToAddRows = false;

            // Animation Part DGV
            DataGridViewImageColumn colAnimPartImg = new DataGridViewImageColumn();
            colAnimPartImg.Width = (int)(dgvSprites.Width * 0.75);
            DataGridViewTextBoxColumn colAnimPartText = new DataGridViewTextBoxColumn();
            colAnimPartText.Width = (int)(dgvSprites.Width * 0.25);

            dgvSkeletonParts.Columns.Add(colAnimPartImg);
            dgvSkeletonParts.Columns.Add(colAnimPartText);

            dgvSkeletonParts.ColumnHeadersVisible = false;
            dgvSkeletonParts.RowHeadersVisible = false;
            dgvSkeletonParts.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvSkeletonParts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvSkeletonParts.AllowUserToAddRows = false;

            // Feature Part DGV
            DataGridViewImageColumn colFeatureImg = new DataGridViewImageColumn();
            colFeatureImg.Width = (int)(dgvFeatures.Width * 0.75);
            DataGridViewTextBoxColumn colFeatureText = new DataGridViewTextBoxColumn();
            colFeatureText.Width = (int)(dgvFeatures.Width * 0.25);

            dgvFeatures.Columns.Add(colFeatureImg);
            dgvFeatures.Columns.Add(colFeatureText);

            dgvFeatures.ColumnHeadersVisible = false;
            dgvFeatures.RowHeadersVisible = false;
            dgvFeatures.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvFeatures.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvFeatures.AllowUserToAddRows = false;

            cmbPartType.Items.Add("Head");
            cmbPartType.Items.Add("Limb");

            tsbSkull.Checked = true;
        }
        public void PrepareIcons()
        {
            var imageList = new ImageList();
            imageList.ImageSize = new Size(64, 48);
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.Images.Add("FrameKey", new Bitmap(Path.Combine(Utility.GLOBAL_PATH, "FrameIcon.png")));

            lvwFrames.LargeImageList = imageList;
        }
        public void ResetMode()
        {
            tsbCharacter.Checked = false;
            tsbSkeleton.Checked = false;
            tsbAnimation.Checked = false;
            tsbAddPoint.Checked = false;
            tsbConfirm.Checked = false;
            tsbDelete.Checked = false;
            tsbHoldProperties.Checked = false;
            tsbFeatures.Checked = false;
        }
        public void Reload()
        {
            ClearCharacters();

            string sXml = EditorManager.GetCharacterCollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gCharacterDoc = xLoad;

            XmlNode oCharacters = gCharacterDoc.SelectSingleNode("//fpxCharacters");
            foreach (XmlNode nCharacter in oCharacters.ChildNodes)
            {
                fpxCharacter oCharacter = new fpxCharacter()
                {
                    Name = nCharacter.Attributes["Name"].Value,
                    UseSkeleton = bool.Parse(nCharacter.Attributes["UseSkeleton"].Value),
                    Animated = bool.Parse(nCharacter.Attributes["Animated"].Value),
                    FrameWidth = int.Parse(nCharacter.Attributes["FrameWidth"].Value),
                    FrameHeight = int.Parse(nCharacter.Attributes["FrameHeight"].Value)
                };

                foreach (XmlNode nChildNode in nCharacter.ChildNodes)
                {
                    if (nChildNode.Name == "Sprites")
                    {
                        foreach (XmlNode nSprite in nChildNode.ChildNodes)
                        {
                            oCharacter.Sprites.Add(nSprite.Attributes["Name"].Value);
                        }
                    }
                    else if (nChildNode.Name == "Skeleton")
                    {
                        foreach (XmlNode nPart in nChildNode.ChildNodes)
                        {
                            fpxSkeletonPart oPart = new fpxSkeletonPart
                            {
                                Name = nPart.Attributes["Name"].Value,
                                OffsetX = int.Parse(nPart.Attributes["OffsetX"].Value),
                                OffsetY = int.Parse(nPart.Attributes["OffsetY"].Value),
                                Sprite = nPart.Attributes["Sprite"].Value,
                                Type = nPart.Attributes["Type"].Value,
                                ZIndex = int.Parse(nPart.Attributes["ZIndex"].Value)
                            };


                            foreach (XmlNode nChildPoint in nPart.ChildNodes)
                            {
                                if (nChildPoint.Name == "StartPoint")
                                {
                                    oPart.StartPoint = new fpxSkeletonPoint()
                                    {
                                        ID = Guid.Parse(nChildPoint.Attributes["ID"].Value),
                                        Location = new Point()
                                        {
                                            X = int.Parse(nChildPoint.Attributes["X"].Value),
                                            Y = int.Parse(nChildPoint.Attributes["Y"].Value)
                                        }
                                    };
                                }
                                else if (nChildPoint.Name == "EndPoint")
                                {
                                    oPart.EndPoint = new fpxSkeletonPoint()
                                    {
                                        ID = Guid.Parse(nChildPoint.Attributes["ID"].Value),
                                        Location = new Point()
                                        {
                                            X = int.Parse(nChildPoint.Attributes["X"].Value),
                                            Y = int.Parse(nChildPoint.Attributes["Y"].Value)
                                        }
                                    };
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(oPart.Sprite))
                            {
                                int x1 = oPart.StartPoint.Location.X;
                                int y1 = oPart.StartPoint.Location.Y;
                                int x2 = oPart.EndPoint.Location.X;
                                int y2 = oPart.EndPoint.Location.Y;

                                int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                int xDiff = x2 - x1;
                                int yDiff = y2 - y1;
                                float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                Bitmap oChildSprite = GetCachedImage(oPart.Sprite);

                                oCharacter.Skeleton.Add(Utility.RotatePart(oPart, oChildSprite, angle));
                            }
                            else
                                oCharacter.Skeleton.Add(oPart);
                        }
                    }
                    else if (nChildNode.Name == "Animations")
                    {
                        foreach (XmlNode nAnimation in nChildNode.ChildNodes)
                        {
                            fpxCharacterAnimation oAnimation = new fpxCharacterAnimation()
                            {
                                Name = nAnimation.Attributes["Name"].Value,
                                Width = int.Parse(nAnimation.Attributes["Width"].Value),
                                Height = int.Parse(nAnimation.Attributes["Height"].Value),
                                Speed = decimal.Parse(nAnimation.Attributes["Speed"].Value)
                            };

                            foreach (XmlNode nFrame in nAnimation.ChildNodes)
                            {
                                fpxAnimationFrame oFrame = new fpxAnimationFrame();

                                foreach (XmlNode nPart in nFrame.ChildNodes)
                                {
                                    fpxSkeletonPart oPart = new fpxSkeletonPart
                                    {
                                        Name = nPart.Attributes["Name"].Value,
                                        OffsetX = int.Parse(nPart.Attributes["OffsetX"].Value),
                                        OffsetY = int.Parse(nPart.Attributes["OffsetY"].Value),
                                        Sprite = nPart.Attributes["Sprite"].Value,
                                        Type = nPart.Attributes["Type"].Value,
                                        ZIndex = int.Parse(nPart.Attributes["ZIndex"].Value)
                                    };

                                    foreach (XmlNode nChildPoint in nPart.ChildNodes)
                                    {
                                        if (nChildPoint.Name == "StartPoint")
                                        {
                                            oPart.StartPoint = new fpxSkeletonPoint()
                                            {
                                                ID = Guid.Parse(nChildPoint.Attributes["ID"].Value),
                                                Location = new Point()
                                                {
                                                    X = int.Parse(nChildPoint.Attributes["X"].Value),
                                                    Y = int.Parse(nChildPoint.Attributes["Y"].Value)
                                                }
                                            };
                                        }
                                        else if (nChildPoint.Name == "EndPoint")
                                        {
                                            oPart.EndPoint = new fpxSkeletonPoint()
                                            {
                                                ID = Guid.Parse(nChildPoint.Attributes["ID"].Value),
                                                Location = new Point()
                                                {
                                                    X = int.Parse(nChildPoint.Attributes["X"].Value),
                                                    Y = int.Parse(nChildPoint.Attributes["Y"].Value)
                                                }
                                            };
                                        }
                                    }

                                    if (!string.IsNullOrWhiteSpace(oPart.Sprite))
                                    {
                                        int x1 = oPart.StartPoint.Location.X;
                                        int y1 = oPart.StartPoint.Location.Y;
                                        int x2 = oPart.EndPoint.Location.X;
                                        int y2 = oPart.EndPoint.Location.Y;

                                        int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                        int xDiff = x2 - x1;
                                        int yDiff = y2 - y1;
                                        float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                        Bitmap oChildSprite = GetCachedImage(oPart.Sprite);

                                        oFrame.Skeleton.Add(Utility.RotatePart(oPart, oChildSprite, angle));
                                    }
                                    else
                                        oFrame.Skeleton.Add(oPart);
                                }

                                oAnimation.Frames.Add(oFrame);
                            }

                            oCharacter.Animations.Add(oAnimation);
                        }
                    }
                    else if (nChildNode.Name == "Features")
                    {
                        foreach (XmlNode nFeature in nChildNode.ChildNodes)
                        {
                            fpxCharacterFeature oFeature = new fpxCharacterFeature
                            {
                                Name = nFeature.Attributes["Name"].Value,
                                Part = nFeature.Attributes["Part"].Value,
                                Sprite = nFeature.Attributes["Sprite"].Value,
                                OffsetX = int.Parse(nFeature.Attributes["X"].Value),
                                OffsetY = int.Parse(nFeature.Attributes["Y"].Value),
                                Width = int.Parse(nFeature.Attributes["Width"].Value),
                                Height = int.Parse(nFeature.Attributes["Height"].Value)
                            };


                            bool bAdded = false;

                            if (!string.IsNullOrWhiteSpace(oFeature.Sprite))
                            {
                                if(!string.IsNullOrWhiteSpace(oFeature.Part))
                                {
                                    fpxSkeletonPart oPart = oCharacter.Skeleton.Where(p => p.Name == oFeature.Part).FirstOrDefault();

                                    int x1 = oPart.StartPoint.Location.X;
                                    int y1 = oPart.StartPoint.Location.Y;
                                    int x2 = oPart.EndPoint.Location.X;
                                    int y2 = oPart.EndPoint.Location.Y;

                                    int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                    int xDiff = x2 - x1;
                                    int yDiff = y2 - y1;
                                    float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                    Bitmap oChildSprite = GetCachedImage(oFeature.Sprite);

                                    oCharacter.Features.Add(Utility.RotateFeature(oFeature, oChildSprite, angle, oPart.Type));

                                    bAdded = true;
                                }                           
                            }


                            if(!bAdded)
                              oCharacter.Features.Add(oFeature);
                        }
                    }
                }

                gCharacters.Add(oCharacter);
            }

            foreach (fpxCharacter oCharacter in gCharacters)
            {
                for (int i = 0; i < oCharacter.Skeleton.Count; i++)
                {
                    fpxSkeletonPart oPart = oCharacter.Skeleton[i];

                    string sSprite = oPart.Sprite;


                    if (!string.IsNullOrWhiteSpace(sSprite))
                    {
                        if (oPart.SpriteBitmap != null)
                        {
                            oPart.SpriteBitmap.Dispose();
                            oPart.SpriteBitmap = null;
                        }

                        Bitmap oSprite = GetCachedImage(sSprite);

                        int x1 = oPart.StartPoint.Location.X;
                        int y1 = oPart.StartPoint.Location.Y;
                        int x2 = oPart.EndPoint.Location.X;
                        int y2 = oPart.EndPoint.Location.Y;

                        int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                        float xDiff = x2 - x1;
                        float yDiff = y2 - y1;
                        float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                        oPart.RelWidth = oSprite.Width;
                        oPart.RelHeight = oSprite.Height;

                        gCharacters[gCharacters.IndexOf(oCharacter)].Skeleton[i] = Utility.RotatePart(oPart, oSprite, angle);
                    }
                }

                foreach (fpxCharacterAnimation oAnimation in oCharacter.Animations)
                {
                    foreach (fpxAnimationFrame oFrame in oAnimation.Frames)
                    {
                        for (int i = 0; i < oFrame.Skeleton.Count; i++)
                        {
                            fpxSkeletonPart oPart = oFrame.Skeleton[i];

                            string sSprite = oPart.Sprite;

                            if (!string.IsNullOrWhiteSpace(sSprite))
                            {
                                if (oPart.SpriteBitmap != null)
                                {
                                    oPart.SpriteBitmap.Dispose();
                                    oPart.SpriteBitmap = null;
                                }

                                Bitmap oSprite = GetCachedImage(sSprite);

                                int x1 = oPart.StartPoint.Location.X;
                                int y1 = oPart.StartPoint.Location.Y;
                                int x2 = oPart.EndPoint.Location.X;
                                int y2 = oPart.EndPoint.Location.Y;

                                int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                                float xDiff = x2 - x1;
                                float yDiff = y2 - y1;
                                float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                                oPart.RelWidth = oSprite.Width;
                                oPart.RelHeight = oSprite.Height;

                                gCharacters[gCharacters.IndexOf(oCharacter)]
                                    .Animations[oCharacter.Animations.IndexOf(oAnimation)]
                                        .Frames[oAnimation.Frames.IndexOf(oFrame)]
                                            .Skeleton[i] = Utility.RotatePart(oPart, oSprite, angle);
                            }
                        }
                    }
                }

                for (int i = 0; i < oCharacter.Features.Count; i++)
                {
                    fpxCharacterFeature oFeature = oCharacter.Features[i];

                    string sSprite = oFeature.Sprite;
                    string sPart = oFeature.Part;

                    if (!string.IsNullOrWhiteSpace(sSprite) && !string.IsNullOrWhiteSpace(sPart))
                    {
                        if (oFeature.SpriteBitmap != null)
                        {
                            oFeature.SpriteBitmap.Dispose();
                            oFeature.SpriteBitmap = null;
                        }

                        Bitmap oSprite = GetCachedImage(sSprite);

                        fpxSkeletonPart oPart = oCharacter.Skeleton.Where(p => p.Name == sPart).FirstOrDefault();

                        if(oPart != null)
                        {
                            int x1 = oPart.StartPoint.Location.X;
                            int y1 = oPart.StartPoint.Location.Y;
                            int x2 = oPart.EndPoint.Location.X;
                            int y2 = oPart.EndPoint.Location.Y;

                            int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                            float xDiff = x2 - x1;
                            float yDiff = y2 - y1;
                            float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                            oFeature.RelWidth = oSprite.Width;
                            oFeature.RelHeight = oSprite.Height;

                            gCharacters[gCharacters.IndexOf(oCharacter)].Features[i] = Utility.RotateFeature(oFeature, oSprite, angle, oPart.Type);
                        }                     
                    }
                }
                
                cmbCharacter.Items.Add(oCharacter.Name);
            }

            if (cmbCharacter.Items.Count > 0)
                cmbCharacter.SelectedIndex = 0;

            ManageControls();
        }
        public Bitmap GetCachedImage(string sSprite)
        {
            if (!gCachedImages.ContainsKey(sSprite))
                gCachedImages.Add(sSprite, new Bitmap(Path.Combine(Utility.CHARACTER_PATH, sSprite)));

            return new Bitmap(gCachedImages[sSprite]);
        }
        public void ManageControls()
        {
            bool bCharacterMode = false;
            bool bSkeletonMode = false;
            bool bAnimationMode = false;
            bool bToolbarControls = false;
            bool bFeatureMode = false;

            bool bCharacterList = false;
            bool bCharacterRemove = false;
            bool bCharacterSelected = false;

            bool bCharacterSkeleton = false;
            bool bCharacterAnimated = false;

            bool bFeatureList = false;
            bool bFeatureSelected = false;
            bool bFeaturePartSelected = false;

            bool bSpriteAdd = false;
            bool bSpriteList = false;
            bool bSpriteDelete = false;

            bool bSkeletonStartPoint = false;
            bool bSkeletonPartSelected = false;

            bool bAnimations = false;
            bool bAnimationSelected = false;

            bool bFrameSelected = false;

            bool bPlayMode = false;

            if (cmbCharacter.Items.Count > 0)
            {
                bCharacterList = true;
                bCharacterRemove = true;

                if (cmbCharacter.SelectedIndex > -1)
                {
                    bCharacterSelected = true;

                    fpxCharacter oCharacter = gCharacters[cmbCharacter.SelectedIndex];

                    bCharacterSkeleton = oCharacter.UseSkeleton;
                    bCharacterAnimated = oCharacter.Animated;

                    bSpriteAdd = true;

                    if(dgvFeatures.Rows.Count > 0)
                    {
                        bFeatureList = true;

                        if (dgvFeatures.SelectedRows.Count > 0)
                        {
                            bFeatureSelected = true;

                            if (cmbFeaturePart.SelectedIndex != -1)
                                bFeaturePartSelected = true;
                        }
                    }

                    if (dgvSprites.Rows.Count > 0)
                    {
                        bSpriteList = true;
                        bSpriteDelete = true;
                    }

                    if (dgvParts.SelectedRows.Count > 0)
                    {
                        bSkeletonPartSelected = true;

                        if (dgvParts.SelectedRows.Count == 1)
                        {
                            if (dgvParts.SelectedRows[0].Index == 0)
                                bSkeletonStartPoint = true;
                        }
                    }

                    if (cmbAnimation.Items.Count > 0)
                    {
                        bAnimations = true;

                        if (cmbAnimation.SelectedIndex != -1)
                        {
                            bAnimationSelected = true;

                            if (lvwFrames.SelectedItems.Count > 0)
                                bFrameSelected = true;
                        }
                    }
                }
            }

            bPlayMode = bPlayAnimation;

            tsbCharacter.Enabled = !bPlayMode;
            tsbSkeleton.Enabled = bCharacterSkeleton && !bPlayMode;
            tsbAnimation.Enabled = bCharacterAnimated && !bPlayMode;
            tsbFeatures.Enabled = bCharacterSelected && !bPlayMode;
            tsbSkull.Enabled = !bPlayMode;
            tsbSkin.Enabled = !bPlayMode;
            tsbFace.Enabled = !bPlayMode;
            tsbPlayAnim.Enabled = !bPlayMode;
            tsbPauseAnim.Enabled = bPlayMode;

            bCharacterMode = tsbCharacter.Checked;
            bSkeletonMode = tsbSkeleton.Checked;
            bAnimationMode = tsbAnimation.Checked;
            bFeatureMode = tsbFeatures.Checked;

            tssCharacter1.Visible = bSkeletonMode || bAnimationMode;
            tsbSkull.Visible = bSkeletonMode || bAnimationMode;
            tsbSkin.Visible = bSkeletonMode || bAnimationMode;
            tsbFace.Visible = bSkeletonMode || bAnimationMode;
            tssAnimation1.Visible = bAnimationMode;
            tsbPlayAnim.Visible = bAnimationMode;
            tsbPauseAnim.Visible = bAnimationMode;

            cmbCharacter.Enabled = bCharacterList;
            btnDeleteCharacter.Enabled = bCharacterRemove;
            chkSkeleton.Enabled = bCharacterSelected;
            chkAnimated.Enabled = bCharacterSelected;

            dgvSprites.Enabled = bSpriteList;
            btnAddSprite.Enabled = bSpriteAdd;
            btnDeleteSprite.Enabled = bSpriteDelete;

            btnDeletePart.Enabled = bSkeletonPartSelected;
            cmbOriginPoint.Enabled = bSkeletonPartSelected;
            txtEndPoint.Enabled = bSkeletonPartSelected;
            cmbPartType.Enabled = bSkeletonPartSelected;
            cmbPartSprite.Enabled = bSkeletonPartSelected;
            numZIndex.Enabled = bSkeletonPartSelected;
            numOffsetX.Enabled = bSkeletonPartSelected;
            numOffsetY.Enabled = bSkeletonPartSelected;

            btnNewAnimation.Enabled = !bPlayMode;
            cmbAnimation.Enabled = bAnimations && !bPlayMode;
            btnRemoveAnimation.Enabled = bAnimations && !bPlayMode;
            numFrameWidth.Enabled = bAnimationSelected && !bPlayMode;
            numFrameHeight.Enabled = bAnimationSelected && !bPlayMode;
            numFrameSpeed.Enabled = bAnimationSelected && !bPlayMode;
            btnAddFrame.Enabled = bAnimationSelected && !bPlayMode;

            lvwFrames.Enabled = !bPlayMode;
            btnResetSkeleton.Enabled = bFrameSelected && !bPlayMode;
            btnRemoveFrame.Enabled = bFrameSelected && !bPlayMode;

            btnDeleteFeature.Enabled = bFeatureList;
            dgvFeatures.Enabled = bFeatureList;

            txtFeatureName.Enabled = bFeatureSelected;
            cmbFeatureSprite.Enabled = bFeatureSelected && bFeaturePartSelected;
            numFeatureX.Enabled = bFeatureSelected;
            numFeatureY.Enabled = bFeatureSelected;
            cmbFeaturePart.Enabled = bFeatureSelected;
            numFeatureWidth.Enabled = bFeatureSelected;
            numFeatureHeight.Enabled = bFeatureSelected;

            pnlCharacterProperties.Visible = bCharacterMode;
            pnlSkeletonProperties.Visible = bSkeletonMode;
            pnlFrameProperties.Visible = bAnimationMode && (bFrameSelected && !bPlayMode);
            pnlAnimFrames.Enabled = bAnimationMode && bAnimationSelected;
            pnlAnimProperties.Visible = bAnimationMode;
            pnlFeatures.Visible = bFeatureMode;

            cmbOriginPoint.Enabled = !bSkeletonStartPoint;

            tsbAddPoint.Visible = bToolbarControls;
            tsbConfirm.Visible = bToolbarControls;
            tsbDelete.Visible = bToolbarControls;
            tsbHolds.Visible = bToolbarControls;
            tsbHoldProperties.Visible = bToolbarControls;

            btnSave.Enabled = Dirty;
            btnCancel.Enabled = Dirty;

            ediCharacter.Invalidate();
        }
        public void ValidateCharacters()
        {
            // Load all current data into the character editor document.

            XmlNode oCharactersN = gCharacterDoc.CreateElement("fpxCharacters");

            foreach (fpxCharacter oCharacter in gCharacters)
            {
                XmlAttribute oCharNameN = gCharacterDoc.CreateAttribute("Name");
                oCharNameN.Value = oCharacter.Name;
                XmlAttribute oCharSkeletonN = gCharacterDoc.CreateAttribute("UseSkeleton");
                oCharSkeletonN.Value = oCharacter.UseSkeleton.ToString();
                XmlAttribute oCharAnimated = gCharacterDoc.CreateAttribute("Animated");
                oCharAnimated.Value = oCharacter.Animated.ToString();
                XmlAttribute oCharFrameWidth = gCharacterDoc.CreateAttribute("FrameWidth");
                oCharFrameWidth.Value = oCharacter.FrameWidth.ToString();
                XmlAttribute oCharFrameHeight = gCharacterDoc.CreateAttribute("FrameHeight");
                oCharFrameHeight.Value = oCharacter.FrameHeight.ToString();

                XmlNode oSpritesN = gCharacterDoc.CreateNode("element", "Sprites", "");

                foreach (string sSprite in oCharacter.Sprites)
                {
                    XmlNode oSpriteN = gCharacterDoc.CreateNode("element", "Sprite", "");
                    XmlAttribute oSpriteNameN = gCharacterDoc.CreateAttribute("Name");
                    oSpriteNameN.Value = sSprite;
                    oSpriteN.Attributes.Append(oSpriteNameN);

                    oSpritesN.AppendChild(oSpriteN);
                }

                XmlNode oSkeletonN = gCharacterDoc.CreateNode("element", "Skeleton", "");

                foreach (fpxSkeletonPart oPart in oCharacter.Skeleton)
                {
                    XmlNode oPartN = gCharacterDoc.CreateNode("element", "Part", "");
                    XmlAttribute oPartNameN = gCharacterDoc.CreateAttribute("Name");
                    oPartNameN.Value = oPart.Name;
                    XmlAttribute oPartTypeN = gCharacterDoc.CreateAttribute("Type");
                    oPartTypeN.Value = oPart.Type;
                    XmlAttribute oPartSpriteN = gCharacterDoc.CreateAttribute("Sprite");
                    oPartSpriteN.Value = oPart.Sprite;
                    XmlAttribute oPartZIndexN = gCharacterDoc.CreateAttribute("ZIndex");
                    oPartZIndexN.Value = oPart.ZIndex.ToString();
                    XmlAttribute oPartOffSetX = gCharacterDoc.CreateAttribute("OffsetX");
                    oPartOffSetX.Value = oPart.OffsetX.ToString();
                    XmlAttribute oPartOffSetY = gCharacterDoc.CreateAttribute("OffsetY");
                    oPartOffSetY.Value = oPart.OffsetY.ToString();

                    XmlNode oPartStartN = gCharacterDoc.CreateNode("element", "StartPoint", "");
                    XmlAttribute oStartIDN = gCharacterDoc.CreateAttribute("ID");
                    oStartIDN.Value = oPart.StartPoint.ID.ToString();
                    XmlAttribute oStartX = gCharacterDoc.CreateAttribute("X");
                    oStartX.Value = oPart.StartPoint.Location.X.ToString();
                    XmlAttribute oStartY = gCharacterDoc.CreateAttribute("Y");
                    oStartY.Value = oPart.StartPoint.Location.Y.ToString();
                    oPartStartN.Attributes.Append(oStartIDN);
                    oPartStartN.Attributes.Append(oStartX);
                    oPartStartN.Attributes.Append(oStartY);

                    XmlNode oPartEndN = gCharacterDoc.CreateNode("element", "EndPoint", "");
                    XmlAttribute oEndIDN = gCharacterDoc.CreateAttribute("ID");
                    oEndIDN.Value = oPart.EndPoint.ID.ToString();
                    XmlAttribute oEndX = gCharacterDoc.CreateAttribute("X");
                    oEndX.Value = oPart.EndPoint.Location.X.ToString();
                    XmlAttribute oEndY = gCharacterDoc.CreateAttribute("Y");
                    oEndY.Value = oPart.EndPoint.Location.Y.ToString();
                    oPartEndN.Attributes.Append(oEndIDN);
                    oPartEndN.Attributes.Append(oEndX);
                    oPartEndN.Attributes.Append(oEndY);


                    oPartN.Attributes.Append(oPartNameN);
                    oPartN.Attributes.Append(oPartTypeN);
                    oPartN.Attributes.Append(oPartSpriteN);
                    oPartN.Attributes.Append(oPartZIndexN);
                    oPartN.Attributes.Append(oPartOffSetX);
                    oPartN.Attributes.Append(oPartOffSetY);
                    oPartN.AppendChild(oPartStartN);
                    oPartN.AppendChild(oPartEndN);

                    oSkeletonN.AppendChild(oPartN);
                }

                XmlNode oAnimationsN = gCharacterDoc.CreateNode("element", "Animations", "");

                foreach (fpxCharacterAnimation oAnimation in oCharacter.Animations)
                {
                    XmlNode oAnimationN = gCharacterDoc.CreateNode("element", "Animation", "");
                    XmlAttribute oAnimNameN = gCharacterDoc.CreateAttribute("Name");
                    oAnimNameN.Value = oAnimation.Name;
                    XmlAttribute oAnimWidthN = gCharacterDoc.CreateAttribute("Width");
                    oAnimWidthN.Value = oAnimation.Width.ToString();
                    XmlAttribute oAnimHeightN = gCharacterDoc.CreateAttribute("Height");
                    oAnimHeightN.Value = oAnimation.Height.ToString();
                    XmlAttribute oAnimSpeedN = gCharacterDoc.CreateAttribute("Speed");
                    oAnimSpeedN.Value = oAnimation.Speed.ToString();

                    oAnimationN.Attributes.Append(oAnimNameN);
                    oAnimationN.Attributes.Append(oAnimWidthN);
                    oAnimationN.Attributes.Append(oAnimHeightN);
                    oAnimationN.Attributes.Append(oAnimSpeedN);

                    for (int i = 0; i < oAnimation.Frames.Count; i++)
                    {
                        fpxAnimationFrame oFrame = oAnimation.Frames[i];

                        XmlNode oFrameN = gCharacterDoc.CreateNode("element", "Frame", "");
                        XmlAttribute oFrameIDN = gCharacterDoc.CreateAttribute("ID");
                        oFrameIDN.Value = i.ToString();
                        oFrameN.Attributes.Append(oFrameIDN);

                        foreach (fpxSkeletonPart oPart in oFrame.Skeleton)
                        {
                            XmlNode oPartN = gCharacterDoc.CreateNode("element", "Part", "");
                            XmlAttribute oPartNameN = gCharacterDoc.CreateAttribute("Name");
                            oPartNameN.Value = oPart.Name;
                            XmlAttribute oPartTypeN = gCharacterDoc.CreateAttribute("Type");
                            oPartTypeN.Value = oPart.Type;
                            XmlAttribute oPartSpriteN = gCharacterDoc.CreateAttribute("Sprite");
                            oPartSpriteN.Value = oPart.Sprite;
                            XmlAttribute oPartZIndexN = gCharacterDoc.CreateAttribute("ZIndex");
                            oPartZIndexN.Value = oPart.ZIndex.ToString();
                            XmlAttribute oPartOffSetX = gCharacterDoc.CreateAttribute("OffsetX");
                            oPartOffSetX.Value = oPart.OffsetX.ToString();
                            XmlAttribute oPartOffSetY = gCharacterDoc.CreateAttribute("OffsetY");
                            oPartOffSetY.Value = oPart.OffsetY.ToString();

                            XmlNode oPartStartN = gCharacterDoc.CreateNode("element", "StartPoint", "");
                            XmlAttribute oStartIDN = gCharacterDoc.CreateAttribute("ID");
                            oStartIDN.Value = oPart.StartPoint.ID.ToString();
                            XmlAttribute oStartX = gCharacterDoc.CreateAttribute("X");
                            oStartX.Value = oPart.StartPoint.Location.X.ToString();
                            XmlAttribute oStartY = gCharacterDoc.CreateAttribute("Y");
                            oStartY.Value = oPart.StartPoint.Location.Y.ToString();
                            oPartStartN.Attributes.Append(oStartIDN);
                            oPartStartN.Attributes.Append(oStartX);
                            oPartStartN.Attributes.Append(oStartY);

                            XmlNode oPartEndN = gCharacterDoc.CreateNode("element", "EndPoint", "");
                            XmlAttribute oEndIDN = gCharacterDoc.CreateAttribute("ID");
                            oEndIDN.Value = oPart.EndPoint.ID.ToString();
                            XmlAttribute oEndX = gCharacterDoc.CreateAttribute("X");
                            oEndX.Value = oPart.EndPoint.Location.X.ToString();
                            XmlAttribute oEndY = gCharacterDoc.CreateAttribute("Y");
                            oEndY.Value = oPart.EndPoint.Location.Y.ToString();
                            oPartEndN.Attributes.Append(oEndIDN);
                            oPartEndN.Attributes.Append(oEndX);
                            oPartEndN.Attributes.Append(oEndY);


                            oPartN.Attributes.Append(oPartNameN);
                            oPartN.Attributes.Append(oPartTypeN);
                            oPartN.Attributes.Append(oPartSpriteN);
                            oPartN.Attributes.Append(oPartZIndexN);
                            oPartN.Attributes.Append(oPartOffSetX);
                            oPartN.Attributes.Append(oPartOffSetY);
                            oPartN.AppendChild(oPartStartN);
                            oPartN.AppendChild(oPartEndN);

                            oFrameN.AppendChild(oPartN);
                        }

                        oAnimationN.AppendChild(oFrameN);
                    }

                    oAnimationsN.AppendChild(oAnimationN);
                }

                XmlNode oFeaturesN = gCharacterDoc.CreateNode("element", "Features", "");

                foreach(fpxCharacterFeature oFeature in oCharacter.Features)
                {
                    XmlNode oFeatureN = gCharacterDoc.CreateNode("element", "Feature", "");
                    XmlAttribute oFreatureName = gCharacterDoc.CreateAttribute("Name");
                    oFreatureName.Value = oFeature.Name;
                    XmlAttribute oFeaturePart = gCharacterDoc.CreateAttribute("Part");
                    oFeaturePart.Value = oFeature.Part;
                    XmlAttribute oFeatureSprite = gCharacterDoc.CreateAttribute("Sprite");
                    oFeatureSprite.Value = oFeature.Sprite;
                    XmlAttribute oFeatureX = gCharacterDoc.CreateAttribute("X");
                    oFeatureX.Value = oFeature.OffsetX.ToString();
                    XmlAttribute oFeatureY = gCharacterDoc.CreateAttribute("Y");
                    oFeatureY.Value = oFeature.OffsetY.ToString();
                    XmlAttribute oFeatureWidth = gCharacterDoc.CreateAttribute("Width");
                    oFeatureWidth.Value = oFeature.Width.ToString();
                    XmlAttribute oFeatureHeight = gCharacterDoc.CreateAttribute("Height");
                    oFeatureHeight.Value = oFeature.Height.ToString();
                    XmlAttribute oFeatureType = gCharacterDoc.CreateAttribute("Type");
                    oFeatureType.Value = "";

                    if (oFeature.Part != "")
                        oFeatureType.Value = oCharacter.Skeleton.Where(p => p.Name == oFeature.Part).FirstOrDefault().Type;

                    oFeatureN.Attributes.Append(oFreatureName);
                    oFeatureN.Attributes.Append(oFeaturePart);
                    oFeatureN.Attributes.Append(oFeatureSprite);
                    oFeatureN.Attributes.Append(oFeatureX);
                    oFeatureN.Attributes.Append(oFeatureY);
                    oFeatureN.Attributes.Append(oFeatureWidth);
                    oFeatureN.Attributes.Append(oFeatureHeight);
                    oFeatureN.Attributes.Append(oFeatureType);

                    oFeaturesN.AppendChild(oFeatureN);
                }

                XmlNode oCharacterN = gCharacterDoc.CreateNode("element", "Character", "");
                oCharacterN.Attributes.Append(oCharNameN);
                oCharacterN.Attributes.Append(oCharSkeletonN);
                oCharacterN.Attributes.Append(oCharAnimated);
                oCharacterN.Attributes.Append(oCharFrameWidth);
                oCharacterN.Attributes.Append(oCharFrameHeight);
                oCharacterN.AppendChild(oSpritesN);
                oCharacterN.AppendChild(oSkeletonN);
                oCharacterN.AppendChild(oAnimationsN);
                oCharacterN.AppendChild(oFeaturesN);

                oCharactersN.AppendChild(oCharacterN);
            }

            gCharacterDoc.RemoveAll();
            gCharacterDoc.AppendChild(oCharactersN);

            ValidateSave();
            ManageControls();
        }
        public void ValidateSave()
        {
            bool bDirtyCheck = false;

            if (!gCharacterDoc.OuterXml.Equals(EditorManager.GetCharacterCollection().OuterXml))
                bDirtyCheck = true;

            Dirty = bDirtyCheck;
        }
        #endregion

        #region Helper Methods
        private void ClearCharacters()
        {
            cmbCharacter.Items.Clear();
            gCharacters = new List<fpxCharacter>();
            ClearCharacterProperties();
        }

        public void ClearCharacterProperties()
        {
            chkSkeleton.Checked = false;
            chkAnimated.Checked = false;

            dgvSprites.Rows.Clear();

            ClearSkeletonProperties();
            ClearAnimationProperties();

            if (gCurrSprite != null)
            {
                gCurrSprite.Dispose();
                gCurrSprite = null;
            }
        }

        public void ClearSkeletonProperties()
        {
            dgvParts.Rows.Clear();
            txtPartName.Text = "";
            cmbOriginPoint.Items.Clear();
            txtEndPoint.Text = "";
            cmbPartSprite.Items.Clear();

            if (pbSpritePreview.Image != null)
            {
                pbSpritePreview.Image.Dispose();
                pbSpritePreview.Image = null;
            }

            numZIndex.Value = 0;
            numOffsetX.Value = 0;
            numOffsetY.Value = 0;

            numSkeletonWidth.Value = 800;
            numSkeletonHeight.Value = 600;
        }

        public void ClearAnimationProperties()
        {
            lvwFrames.Items.Clear();
        }
        #endregion

        private void txtFeatureName_TextChanged(object sender, EventArgs e)
        {
            if (dgvFeatures.SelectedRows.Count > 0)
            {
                gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].Name = txtFeatureName.Text;
                dgvFeatures.SelectedRows[0].Cells[1].Value = txtFeatureName.Text;

                ValidateCharacters();
            }
        }

        private void cmbFeatureSprite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFeatureSprite.SelectedItem.ToString() == "None")
            {
                gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].Sprite = "";
                pbPreviewFeature.Image = null;

                if (gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].SpriteBitmap != null)
                {
                    gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].SpriteBitmap.Dispose();
                    gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].SpriteBitmap = null;
                }

                dgvFeatures.SelectedRows[0].Cells[0].Dispose();
                dgvFeatures.SelectedRows[0].Cells[0].Value = null;
            }
            else
            {
                fpxCharacterFeature oFeature = gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index];
                oFeature.Sprite = cmbFeatureSprite.SelectedItem.ToString();

                if (pbPreviewFeature.Image != null)
                {
                    pbPreviewFeature.Image.Dispose();
                    pbPreviewFeature.Image = null;
                }

                if (oFeature.SpriteBitmap != null)
                {
                    oFeature.SpriteBitmap.Dispose();
                    oFeature.SpriteBitmap = null;
                }


                Bitmap oSprite = GetCachedImage(oFeature.Sprite);

                fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Skeleton.Where(p => p.Name == oFeature.Part).FirstOrDefault();

                int x1 = oPart.StartPoint.Location.X;
                int y1 = oPart.StartPoint.Location.Y;
                int x2 = oPart.EndPoint.Location.X;
                int y2 = oPart.EndPoint.Location.Y;

                int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

                float xDiff = x2 - x1;
                float yDiff = y2 - y1;
                float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

                oFeature.RelWidth = oSprite.Width;
                oFeature.RelHeight = oSprite.Height;

                gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index] = Utility.RotateFeature(oFeature, oSprite, angle, oPart.Type);

                Rectangle oCrop = new Rectangle(0, 0, oSprite.Width, oSprite.Height);
                Bitmap spriteSnap = Utility.GetSprite(GetCachedImage(oFeature.Sprite), oCrop);

                int iSmallWidth = oSprite.Width;
                int iSmallHeight = oSprite.Height;

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

                dgvFeatures.SelectedRows[0].Cells[0].Dispose();
                dgvFeatures.SelectedRows[0].Cells[0].Value = spriteSnap;

                pbPreviewFeature.Image = Utility.ResizeImage(new Bitmap(Path.Combine(Utility.CHARACTER_PATH, oFeature.Sprite)), pbPreviewFeature.Width, pbPreviewFeature.Height);
            }

            ValidateCharacters();
        }

        private void numFeatureX_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].OffsetX = (int)numFeatureX.Value;

            ValidateCharacters();
        }

        private void numFeatureY_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].OffsetY = (int)numFeatureY.Value;

            ValidateCharacters();
        }

        private void cmbFeaturePart_SelectedIndexChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].Part = cmbFeaturePart.SelectedItem.ToString();

            ValidateCharacters();
        }

        private void btnRefreshFeature_Click(object sender, EventArgs e)
        {
            //if(dgvFeatures.SelectedRows.Count > 0)
            //{
            //    fpxCharacterFeature oFeature = gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index];

            //    if(gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].Part != "")
            //    {
            //        if (oFeature.SpriteBitmap != null)
            //        {
            //            fpxSkeletonPart oPart = gCharacters[cmbCharacter.SelectedIndex].Skeleton.Where(p => p.Name == oFeature.Part).First();

            //            int x1 = oPart.StartPoint.Location.X;
            //            int y1 = oPart.StartPoint.Location.Y;
            //            int x2 = oPart.EndPoint.Location.X;
            //            int y2 = oPart.EndPoint.Location.Y;

            //            int diameter = (int)Math.Sqrt((Math.Pow(x1 - x2, 2)) + (Math.Pow(y1 - y2, 2)));

            //            float xDiff = x2 - x1;
            //            float yDiff = y2 - y1;
            //            float angle = (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI);

            //            oFeature.SpriteBitmap = Utility.RotateFeature(oFeature.SpriteBitmap, angle);
            //        }
            //    }                  
            //}        
        }

        private void numFeatureWidth_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].Width = (int)numFeatureWidth.Value;

            ValidateCharacters();
        }

        private void numFeatureHeight_ValueChanged(object sender, EventArgs e)
        {
            gCharacters[cmbCharacter.SelectedIndex].Features[dgvFeatures.SelectedRows[0].Index].Height = (int)numFeatureHeight.Value;

            ValidateCharacters();
        }
    }
}
