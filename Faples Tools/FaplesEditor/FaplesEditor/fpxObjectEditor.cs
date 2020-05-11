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
    public partial class fpxObjectEditor : UserControl
    {
        #region Declarations
        XmlDocument gCollections = null;
        List<fpxObject> gObjects = null;
        Bitmap gCurrSheet = null;
        Bitmap gCurrObject = null;
        int iFrame = 0;

        Dictionary<int, List<fpxHold>> gHolds = new Dictionary<int, List<fpxHold>>();
        int iHoldGroup = 0;
        int iPointX = 0;
        int iPointY = 0;
        #endregion

        #region Properties
        bool Dirty { get; set; } = false;
        Timer AnimationPlayer { get; } = new Timer();
        #endregion

        #region Constructor
        public fpxObjectEditor()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Event Handlers
        private void btnAddCollection_Click(object sender, EventArgs e)
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
                    XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
                    if (Utility.XmlNodeExists(oCollections, sName))
                    {
                        try
                        {
                            XmlAttribute oName = gCollections.CreateAttribute("Name");
                            oName.Value = sName;

                            XmlNode oCollection = gCollections.CreateNode("element", "Collection", "");
                            oCollection.Attributes.Append(oName);
                            oCollections.AppendChild(oCollection);
                            gCollections.ReplaceChild(oCollections, oCollections);
                            bValid = true;
                            cmbCollection.Items.Add(sName);

                            if (cmbCollection.SelectedIndex < 0)
                            {
                                int iIndex = Math.Max(cmbCollection.Items.Count - 1, 0);

                                cmbCollection.SelectedIndex = iIndex;
                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Error inserting Collection. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }        
                    }
                    else
                    {
                        MessageBox.Show("Collection already exists. Please use a different name.");
                    }
                }
            }

            oAddDialog.Close();
            ValidateSave();
            ManageControls();
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
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
                    XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
                    XmlNode oCollection = null;

                    foreach (XmlNode oNode in oCollections.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                        {
                            oCollection = oNode;                  
                            break;
                        }
                    }

                    if (oCollection == null)
                        return;

                    if (Utility.XmlNodeExists(oCollection, sName))
                    {
                        try
                        {
                            XmlAttribute oName = gCollections.CreateAttribute("Name");
                            oName.Value = sName;
                            XmlNode oCategory = gCollections.CreateNode("element", "Category", "");
                            oCategory.Attributes.Append(oName);
                            oCollection.AppendChild(oCategory);  
                            oCollections.ReplaceChild(oCollection, oCollection);
                            gCollections.ReplaceChild(oCollections, oCollections);
                            bValid = true;
                            cmbCategory.Items.Add(sName);

                            if (cmbCategory.SelectedIndex < 0)
                            {
                                int iIndex = Math.Max(cmbCategory.Items.Count - 1, 0);

                                cmbCategory.SelectedIndex = iIndex;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inserting Category. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Category already exists. Please use a different name.");
                    }
                }
            }

            oAddDialog.Close();
            ValidateSave();
            ManageControls();
        }
        private void btnAddSheet_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.OBJECTS_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.OBJECTS_PATH))
                    {
                        MessageBox.Show("Please select image within 'Objects' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    string sName = opf.SafeFileName;
                    XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
                    XmlNode oCollection = null;
                    XmlNode oCategory = null;

                    foreach (XmlNode oNode in oCollections.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                        {
                            oCollection = oNode;
                            break;
                        }
                    }

                    foreach (XmlNode oNode in oCollection.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                        {
                            oCategory = oNode;
                            break;
                        }
                    }

                    if (oCollection == null)
                        return;

                    if (Utility.XmlNodeExists(oCategory, sName))
                    {
                        try
                        {
                            XmlAttribute oName = gCollections.CreateAttribute("Name");
                            oName.Value = sName;

                            XmlNode oSheet = gCollections.CreateNode("element", "Sheet", "");
                            oSheet.Attributes.Append(oName);

                            oCategory.AppendChild(oSheet);
                            oCollection.ReplaceChild(oCategory, oCategory);
                            oCollections.ReplaceChild(oCollection, oCollection);
                            gCollections.ReplaceChild(oCollections, oCollections);
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
                            MessageBox.Show("Error inserting Sheet. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                    return;
            }

            ValidateSave();
            ManageControls();
        }
        private void btnAddObject_Click(object sender, EventArgs e)
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
                    XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
                    XmlNode oCollection = null;
                    XmlNode oCategory = null;
                    XmlNode oSheet = null;

                    foreach (XmlNode oNode in oCollections.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                        {
                            oCollection = oNode;
                            break;
                        }
                    }

                    foreach (XmlNode oNode in oCollection.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                        {
                            oCategory = oNode;
                            break;
                        }
                    }

                    foreach (XmlNode oNode in oCategory.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                        {
                            oSheet = oNode;
                            break;
                        }
                    }

                    if (oCollection == null)
                        return;

                    if (Utility.XmlNodeExists(oSheet, sName))
                    {
                        try
                        {
                            fpxObject oObj = new fpxObject();
                            oObj.Name = sName;
                            oObj.SpriteSheet = oSheet.Attributes["Name"].Value;

                            XmlAttribute oName = gCollections.CreateAttribute("Name");
                            oName.Value = oObj.Name;
                            XmlAttribute oX = gCollections.CreateAttribute("x");
                            oX.Value = oObj.x.ToString();
                            XmlAttribute oY = gCollections.CreateAttribute("y");
                            oY.Value = oObj.y.ToString();
                            XmlAttribute oWidth = gCollections.CreateAttribute("width");
                            oWidth.Value = oObj.width.ToString();
                            XmlAttribute oHeight = gCollections.CreateAttribute("height");
                            oHeight.Value = oObj.height.ToString();
                            XmlAttribute oAnimated = gCollections.CreateAttribute("Animated");
                            oAnimated.Value = oObj.Animated.ToString();
                            XmlNode oAnimations = gCollections.CreateNode("element", "Animations", "");
                            XmlNode oHolds = gCollections.CreateNode("element", "Holds", "");
                            XmlAttribute oAnimationIndex = gCollections.CreateAttribute("Index");
                            oAnimationIndex.Value = oObj.AnimationIndex.ToString();
                            oAnimations.Attributes.Append(oAnimationIndex);
                            XmlNode oObject = gCollections.CreateNode("element", "Object", "");
                            oObject.Attributes.Append(oName);
                            oObject.Attributes.Append(oX);
                            oObject.Attributes.Append(oY);
                            oObject.Attributes.Append(oWidth);
                            oObject.Attributes.Append(oHeight);
                            oObject.Attributes.Append(oAnimated);
                            oObject.AppendChild(oAnimations);
                            oObject.AppendChild(oHolds);

                            oSheet.AppendChild(oObject);
                            oCategory.ReplaceChild(oSheet, oSheet);
                            oCollection.ReplaceChild(oCategory, oCategory);
                            oCollections.ReplaceChild(oCollection, oCollection);
                            gCollections.ReplaceChild(oCollections, oCollections);
                            bValid = true;

                            gObjects.Add(oObj);

                            dgvObjects.Rows.Add(new DataGridViewRow());
                            DataGridViewRow oRow = dgvObjects.Rows[dgvObjects.Rows.Count - 1];
                            oRow.MinimumHeight = 96;
                            oRow.Cells[0].Value = new Bitmap(1, 1);
                            oRow.Cells[1].Value = sName;

                            if (dgvObjects.SelectedRows.Count < 1)
                            {
                                int iIndex = Math.Max(dgvObjects.Rows.Count - 1, 0);

                                dgvObjects.Rows[iIndex].Selected = true;
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
        private void btnAddAnim_Click(object sender, EventArgs e)
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
                    XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
                    XmlNode oCollection = null;
                    XmlNode oCategory = null;
                    XmlNode oSheet = null;
                    XmlNode oObject = null;
                    XmlNode oAnimations = null;

                    foreach (XmlNode oNode in oCollections.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                        {
                            oCollection = oNode;
                            break;
                        }
                    }

                    foreach (XmlNode oNode in oCollection.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                        {
                            oCategory = oNode;
                            break;
                        }
                    }

                    foreach (XmlNode oNode in oCategory.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                        {
                            oSheet = oNode;
                            break;
                        }
                    }

                    foreach (XmlNode oNode in oSheet.ChildNodes)
                    {
                        if (oNode.Attributes["Name"].Value.Equals(dgvObjects.SelectedRows[0].Cells[1].Value))
                        {
                            oObject = oNode;
                            oAnimations = oObject.FirstChild;
                            break;
                        }
                    }

                    if (oCollection == null)
                        return;

                    if (Utility.XmlNodeExists(oAnimations, sName))
                    {
                        try
                        {
                            fpxAnimation anim = new fpxAnimation();
                            anim.Name = sName;
                            anim.ReelHeight = (int) numReelHeight.Value;


                            XmlAttribute oName = gCollections.CreateAttribute("Name");
                            oName.Value = anim.Name;
                            XmlAttribute oReelHeight = gCollections.CreateAttribute("ReelHeight");
                            oReelHeight.Value = anim.ReelHeight.ToString();
                            XmlAttribute oReelIndex = gCollections.CreateAttribute("ReelIndex");
                            oReelIndex.Value = anim.ReelIndex.ToString();
                            XmlAttribute oFrameWidth = gCollections.CreateAttribute("FrameWidth");
                            oFrameWidth.Value = anim.FrameWidth.ToString();
                            XmlAttribute oTotalFrames = gCollections.CreateAttribute("TotalFrames");
                            oTotalFrames.Value = anim.TotalFrames.ToString();
                            XmlAttribute oFrameSpeed = gCollections.CreateAttribute("FrameSpeed");
                            oFrameSpeed.Value = anim.FrameSpeed.ToString();

                            XmlNode oAnimation = gCollections.CreateNode("element", "Animation", "");
                            oAnimation.Attributes.Append(oName);
                            oAnimation.Attributes.Append(oReelHeight);
                            oAnimation.Attributes.Append(oReelIndex);
                            oAnimation.Attributes.Append(oFrameWidth);
                            oAnimation.Attributes.Append(oTotalFrames);
                            oAnimation.Attributes.Append(oFrameSpeed);
                            oAnimations.AppendChild(oAnimation);


                           
                            oObject.ReplaceChild(oAnimations, oAnimations);
                            oSheet.ReplaceChild(oObject, oObject);
                            oCategory.ReplaceChild(oSheet, oSheet);
                            oCollection.ReplaceChild(oCategory, oCategory);
                            oCollections.ReplaceChild(oCollection, oCollection);
                            gCollections.ReplaceChild(oCollections, oCollections);
                            bValid = true;

                            fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                            oObj.Animations.Add(anim);

                            lvwAnimations.Items.Add(anim.Name);

                            if (lvwAnimations.SelectedItems.Count < 1)
                            {
                                int iIndex = Math.Max(lvwAnimations.Items.Count - 1, 0);
                                
                                lvwAnimations.Items[iIndex].Selected = true;

                                oObj.AnimationIndex = iIndex;
                            }

                            gObjects[dgvObjects.SelectedRows[0].Index] = oObj;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inserting Animation. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Animation already exists. Please use a different name.");
                    }
                }
            }

            oAddDialog.Close();
            ValidateSave();
            ManageControls();
        }
        private void btnDeleteCollection_Click(object sender, EventArgs e)
        {
            ClearCategories();

            string sName = cmbCollection.SelectedItem.ToString();
            XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");

            if (!Utility.XmlNodeExists(oCollections, sName))
            {
                foreach(XmlNode oNode in oCollections)
                {
                    if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                    {
                        oCollections.RemoveChild(oNode);
                        gCollections.ReplaceChild(oCollections, oCollections);
                        cmbCollection.Items.Remove(cmbCollection.SelectedItem);               
                        break;
                    }
                }
            }

            if (cmbCollection.Items.Count > 0)
                cmbCollection.SelectedIndex = 0;
            else
                cmbCollection.SelectedIndex = -1;

            ValidateSave();
            ManageControls();
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            ClearSheets();

            string sName = cmbCategory.SelectedItem.ToString();
            XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
            XmlNode oCollection = null;

            foreach (XmlNode oNode in oCollections.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    oCollection = oNode;
                    break;
                }
            }

            if (oCollection == null)
                return;

            if (!Utility.XmlNodeExists(oCollection, sName))
            {
                foreach (XmlNode oNode in oCollection)
                {
                    if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                    {
                        oCollection.RemoveChild(oNode);
                        oCollections.ReplaceChild(oCollection, oCollection);
                        gCollections.ReplaceChild(oCollections, oCollections);
                        cmbCategory.Items.Remove(cmbCategory.SelectedItem);
                        break;
                    }
                }

                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;
                else
                    cmbCategory.SelectedIndex = -1;
            }

            ValidateSave();
            ManageControls();
        }

        private void btnDeleteSheet_Click(object sender, EventArgs e)
        {
            string sName = cmbSheet.SelectedItem.ToString();
            XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
            XmlNode oCollection = null;
            XmlNode oCategory = null;

            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            if (gCurrObject != null)
                gCurrObject.Dispose();

            gObjects = new List<fpxObject>();
            gCurrObject = null;

            ClearObjects();

            foreach (XmlNode oNode in oCollections.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    oCollection = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oCollection.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                {
                    oCategory = oNode;
                    break;
                }
            }

            if (oCollection == null)
                return;

            if (!Utility.XmlNodeExists(oCategory, sName))
            {
                foreach (XmlNode oNode in oCategory)
                {
                    if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                    {
                        oCategory.RemoveChild(oNode);
                        oCollection.ReplaceChild(oCategory, oCategory);
                        oCollections.ReplaceChild(oCollection, oCollection);
                        gCollections.ReplaceChild(oCollections, oCollections);
                        cmbSheet.Items.Remove(cmbSheet.SelectedItem);
                        break;
                    }
                }

                if (cmbSheet.Items.Count > 0)
                    cmbSheet.SelectedIndex = 0;
                else
                    cmbSheet.SelectedIndex = -1;
            }

            ValidateSave();
            ManageControls();
        }
        private void btnDeleteObject_Click(object sender, EventArgs e)
        {
            ClearObjectProperties();

            string sName = dgvObjects.SelectedRows[0].Cells[1].Value.ToString();
            XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
            XmlNode oCollection = null;
            XmlNode oCategory = null;
            XmlNode oSheet = null;

            foreach (XmlNode oNode in oCollections.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    oCollection = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oCollection.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                {
                    oCategory = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oCategory.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                {
                    oSheet = oNode;
                    break;
                }
            }

            if (oCollection == null)
                return;

            if (!Utility.XmlNodeExists(oSheet, sName))
            {
                foreach (XmlNode oNode in oSheet)
                {
                    if (oNode.Attributes["Name"].Value.Equals(dgvObjects.SelectedRows[0].Cells[1].Value))
                    {
                        oSheet.RemoveChild(oNode);
                        oCategory.ReplaceChild(oSheet, oSheet);
                        oCollection.ReplaceChild(oCategory, oCategory);
                        oCollections.ReplaceChild(oCollection, oCollection);
                        gCollections.ReplaceChild(oCollections, oCollections);
                        dgvObjects.Rows.Remove(dgvObjects.SelectedRows[0]);
                        break;
                    }
                }

                if (dgvObjects.Rows.Count > 0)
                {
                    dgvObjects.Rows[0].Selected = true;

                    fpxObject oObj = gObjects[0];

                    Point oSpriteLocation = new Point(oObj.x, oObj.y);
                    Size oSpriteSize = new Size(oObj.width, oObj.height);
                    Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);
                    Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                    if (gCurrObject != null)
                        gCurrObject.Dispose();

                    gCurrObject = spriteFull;

                    numX.Value = oObj.x;
                    numY.Value = oObj.y;
                    numWidth.Value = oObj.width;
                    numHeight.Value = oObj.height;
                    chkAnimated.Checked = oObj.Animated;

                    foreach(fpxAnimation anim in oObj.Animations)
                    {
                        lvwAnimations.Items.Add(anim.Name);
                    }

                    if(lvwAnimations.Items.Count > 0)
                    {
                        lvwAnimations.Items[0].Selected = true;

                        numReelHeight.Value = oObj.Animations[0].ReelHeight;
                    }
                    
                }
                else
                {
                    if(gCurrObject != null)
                    {
                        gCurrObject.Dispose();
                        gCurrObject = null;
                    }
                }
            }

            ValidateSave();
            ManageControls();
        }
        private void btnRemoveAnim_Click(object sender, EventArgs e)
        {
            ClearAnimationProperties();

            string sName = lvwAnimations.SelectedItems[0].Text;

            XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
            XmlNode oCollection = null;
            XmlNode oCategory = null;
            XmlNode oSheet = null;
            XmlNode oObject = null;
            XmlNode oAnimations = null;

            foreach (XmlNode oNode in oCollections.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    oCollection = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oCollection.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                {
                    oCategory = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oCategory.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                {
                    oSheet = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oSheet.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(dgvObjects.SelectedRows[0].Cells[1].Value))
                {
                    oObject = oNode;
                    oAnimations = oObject.FirstChild;
                    break;
                }
            }

            if (oCollection == null)
                return;

            if (!Utility.XmlNodeExists(oAnimations, sName))
            {
                foreach (XmlNode oNode in oAnimations)
                {
                    if (oNode.Attributes["Name"].Value.Equals(lvwAnimations.SelectedItems[0].Text))
                    {
                        oAnimations.RemoveChild(oNode);
                        oObject.ReplaceChild(oAnimations, oAnimations);
                        oSheet.ReplaceChild(oObject, oObject);
                        oCategory.ReplaceChild(oSheet, oSheet);
                        oCollection.ReplaceChild(oCategory, oCategory);
                        oCollections.ReplaceChild(oCollection, oCollection);
                        gCollections.ReplaceChild(oCollections, oCollections);
                        gObjects[dgvObjects.SelectedRows[0].Index].Animations.RemoveAt(lvwAnimations.SelectedItems[0].Index);
                        lvwAnimations.Items.Remove(lvwAnimations.SelectedItems[0]);
                        break;
                    }
                }

                if (lvwAnimations.Items.Count > 0)
                {
                    lvwAnimations.Items[0].Selected = true;

                    fpxAnimation anim = gObjects[dgvObjects.SelectedRows[0].Index].Animations[0];


                    numReelIndex.Value = anim.ReelIndex;
                    numFrameWidth.Value = anim.FrameWidth;
                    numTotalFrames.Value = anim.TotalFrames;
                    numPlaySpeed.Value = anim.FrameSpeed;
                }
            }
            ValidateSave();
            ManageControls();
        }
        private void cmbCollection_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbCollection.SelectedIndex == -1)
                cmbCollection.Items.Clear();

            ClearCategories();

            foreach(XmlNode oCollection in gCollections.SelectSingleNode("//fpxObjects").ChildNodes)
            {
                if(oCollection.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    cmbCategory.Items.Clear();
                   
                    foreach(XmlNode oCategory in oCollection.ChildNodes)
                    {
                        cmbCategory.Items.Add(oCategory.Attributes["Name"].Value);
                    }

                    if(cmbCategory.Items.Count > 0)
                    {
                        cmbCategory.SelectedIndex = 0;
                    }
                }
            }

            ManageControls();
        }

        private void cmbCategory_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex == -1)
                cmbCategory.Items.Clear();

            ClearSheets();

            foreach (XmlNode oCollection in gCollections.SelectSingleNode("//fpxObjects").ChildNodes)
            {
                if (oCollection.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    foreach (XmlNode oCategory in oCollection.ChildNodes)
                    {
                        if (oCategory.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                        {
                            cmbSheet.Items.Clear();

                            foreach (XmlNode oSheet in oCategory.ChildNodes)
                            {
                                cmbSheet.Items.Add(oSheet.Attributes["Name"].Value);
                            }

                            if (cmbSheet.Items.Count > 0)
                            {
                                cmbSheet.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }

            ManageControls();
        }


        private void cmbSheet_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearObjects();

            if (cmbSheet.SelectedIndex == -1)
                cmbSheet.Items.Clear();

            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            if (gCurrObject != null)
                gCurrObject.Dispose();

            gCurrSheet = new Bitmap(Path.Combine(Utility.OBJECTS_PATH, cmbSheet.SelectedItem.ToString()));
            gObjects = new List<fpxObject>();
            gCurrObject = null;

            foreach (XmlNode oCollection in gCollections.SelectSingleNode("//fpxObjects").ChildNodes)
            {
                if (oCollection.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    foreach (XmlNode oCategory in oCollection.ChildNodes)
                    {
                        if (oCategory.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                        {
                            foreach (XmlNode oSheet in oCategory.ChildNodes)
                            {
                                if (oSheet.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
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

                                        XmlNode oHolds = oObject.LastChild;

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

                                        Dictionary<int, List<fpxHold>> colGroups = new Dictionary<int, List<fpxHold>>();
                                        foreach(XmlNode oGroupHold in oHolds)
                                        {
                                            List<fpxHold> colHolds = new List<fpxHold>();
                                            foreach(XmlNode oHold in oGroupHold)
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

                                            for(int i = 0; i < colHolds.Count; i++)
                                            {
                                                fpxHold currH = colHolds.ElementAtOrDefault(i);
                                                fpxHold prevH = colHolds.ElementAtOrDefault(i - 1);

                                                if(prevH != null)
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
                                        oObj.HoldGroups = colGroups;

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

                                        if (gCurrObject != null)
                                            gCurrObject.Dispose();

                                        gCurrObject = spriteFull;

                                        numX.Value = oObj.x;
                                        numY.Value = oObj.y;
                                        numWidth.Value = oObj.width;
                                        numHeight.Value = oObj.height;
                                        chkAnimated.Checked = oObj.Animated;

                                        if(oObj.Animations.Count > 0)
                                        {
                                            foreach(fpxAnimation anim in oObj.Animations)
                                            {
                                                lvwAnimations.Items.Add(anim.Name);
                                            }

                                            fpxAnimation curr = oObj.Animations[oObj.AnimationIndex - 1];
                                            lvwAnimations.Items[oObj.AnimationIndex - 1].Selected = true;
                                            numReelHeight.Value = curr.ReelHeight;
                                            numReelIndex.Value = curr.ReelIndex;
                                            numFrameWidth.Value = curr.FrameWidth;
                                            numTotalFrames.Value = curr.TotalFrames;
                                            numPlaySpeed.Value = curr.FrameSpeed;
                                        }

                                        gHolds = new Dictionary<int, List<fpxHold>>(oObj.HoldGroups);
                                        tsbHolds.Items.Clear();
                                        iHoldGroup = 0;

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

                                       // ValidateObject();
                                    }
                                    else
                                    {
                                        numX.Value = numX.Minimum;
                                        numY.Value = numY.Minimum;
                                        numWidth.Value = numWidth.Minimum;
                                        numHeight.Value = numHeight.Minimum;
                                        chkAnimated.Checked = false;
                                        numReelHeight.Value = numReelHeight.Minimum;
                                        lvwAnimations.Clear();
                                        numReelIndex.Value = numReelIndex.Minimum;
                                        numFrameWidth.Value = numFrameWidth.Minimum;
                                        numTotalFrames.Value = numTotalFrames.Minimum;
                                        numPlaySpeed.Value = 1.00m;
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
                lvwAnimations.Items.Clear();
                tsbHolds.Enabled = true;
                tsbHolds.Items.Clear();
                tsbHolds.SelectedItem = null;
                tsbHolds.Enabled = false;
                iHoldGroup = 0;
                gHolds.Clear();


                foreach (fpxObject oObject in gObjects)
                {
                    if (oObject.Name.Equals(dgvObjects.SelectedRows[0].Cells[1].Value))
                    {
                        numX.Value = oObject.x;
                        numY.Value = oObject.y;
                        numWidth.Value = oObject.width;
                        numHeight.Value = oObject.height;
                        chkAnimated.Checked = oObject.Animated;

                        if(oObject.Animations.Count > 0)
                        {
                            foreach(fpxAnimation anim in oObject.Animations)
                            {
                                lvwAnimations.Items.Add(anim.Name);
                            }

                            lvwAnimations.Items[oObject.AnimationIndex].Selected = true;

                            fpxAnimation currAnim = oObject.Animations[oObject.AnimationIndex];

                            numReelHeight.Value = currAnim.ReelHeight;
                            numReelIndex.Value = currAnim.ReelIndex;
                            numFrameWidth.Value = currAnim.FrameWidth;
                            numTotalFrames.Value = currAnim.TotalFrames;
                            numPlaySpeed.Value = currAnim.FrameSpeed;
                        }

                        gHolds = new Dictionary<int, List<fpxHold>>(oObject.HoldGroups);
                        tsbHolds.Items.Clear();
                        iHoldGroup = 0;

                        for(int i = 0; i < gHolds.Count; i++)
                        {
                            List<fpxHold> holds = gHolds[i];
                            iHoldGroup++;

                            switch(holds[0].type)
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

                ediObject.Invalidate();
            }
        }
        private void lvwAnimations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwAnimations.SelectedItems.Count > 0)
            {
                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                foreach (fpxAnimation anim in oObj.Animations)
                {
                    if (anim.Name.Equals(lvwAnimations.SelectedItems[0].Text))
                    {
                        numReelHeight.Value = anim.ReelHeight;
                        numReelIndex.Value = anim.ReelIndex;
                        numFrameWidth.Value = anim.FrameWidth;
                        numTotalFrames.Value = anim.TotalFrames;
                        numPlaySpeed.Value = anim.FrameSpeed;
                    }
                }

                ediObject.Invalidate();
            }
        }
        private void numX_ValueChanged(object sender, EventArgs e)
        {
            if(dgvObjects.SelectedRows.Count > 0)
            {
                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                if (numX.Value != oObj.x)
                    oObj.x = (int)numX.Value;

                ValidateObject();
            }
        }
        private void numY_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                if (numY.Value != oObj.y)
                    oObj.y = (int)numY.Value;

                ValidateObject();
            }
        }

        private void numWidth_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                if (numWidth.Value != oObj.width)
                    oObj.width = (int)numWidth.Value;

                ValidateObject();
            }
        }

        private void numHeight_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                if (numHeight.Value != oObj.height)
                    oObj.height = (int)numHeight.Value;

                ValidateObject();
            }
        }

        private void chkAnimated_CheckedChanged(object sender, EventArgs e)
        {
            bool bRunPlayer = false;

            if (dgvObjects.SelectedRows.Count > 0)
            {
                if (chkAnimated.Checked)
                {
                    iFrame = 0;
                    bRunPlayer = true;
                    AnimationPlayer.Interval = (int)Math.Max(1000 * numPlaySpeed.Value, 0.01m);
                }

                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                oObj.Animated = chkAnimated.Checked;

                ValidateObject();
            }

            AnimationPlayer.Enabled = bRunPlayer;
        }

        private void numReelHeight_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                int iIndex = (int) numReelIndex.Value;
                while (numReelHeight.Value * iIndex > gCurrSheet.Height)
                    iIndex--;
                numReelIndex.Value = Math.Max(iIndex, 1);

                foreach(fpxAnimation anim in oObj.Animations)
                {
                    if(numReelHeight.Value > gCurrSheet.Height)
                        numReelHeight.Value = gCurrSheet.Height;

                    anim.ReelHeight = (int) numReelHeight.Value;
                }

                ValidateObject();
            }
        }

        private void numReelIndex_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                if (lvwAnimations.SelectedItems.Count > 0)
                {
                    fpxAnimation anim = gObjects[dgvObjects.SelectedRows[0].Index].Animations[lvwAnimations.SelectedItems[0].Index];

                    int iIndex = (int)numReelIndex.Value;

                    while (iIndex * numReelHeight.Value > gCurrSheet.Height)
                        iIndex--;

                    numReelIndex.Value = Math.Max(iIndex, 1);
                    anim.ReelIndex = (int)numReelIndex.Value;

                    ValidateObject();
                }
            }
        }

        private void numFrameWidth_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                if (lvwAnimations.SelectedItems.Count > 0)
                {

                    int iIndex = (int)numTotalFrames.Value;
                    while (numFrameWidth.Value * iIndex > gCurrSheet.Width)
                        iIndex--;
                    numTotalFrames.Value = Math.Max(iIndex, 1);

                    fpxAnimation anim = gObjects[dgvObjects.SelectedRows[0].Index].Animations[lvwAnimations.SelectedItems[0].Index];

                    if (numFrameWidth.Value > gCurrSheet.Width)
                        numFrameWidth.Value = gCurrSheet.Width;

                    anim.FrameWidth = (int)numFrameWidth.Value;

                    ValidateObject();
                }
            }
        }

        private void numTotalFrames_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                if (lvwAnimations.SelectedItems.Count > 0)
                {
                    AnimationPlayer.Enabled = false;

                    fpxAnimation anim = gObjects[dgvObjects.SelectedRows[0].Index].Animations[lvwAnimations.SelectedItems[0].Index];

                    int iIndex = (int)numTotalFrames.Value;

                    while (iIndex * numFrameWidth.Value > gCurrSheet.Width)
                        iIndex--;

                    numTotalFrames.Value = Math.Max(iIndex, 1);
                    anim.TotalFrames = (int)numTotalFrames.Value;

                    ValidateObject();

                    AnimationPlayer.Enabled = true;
                }
            }      
        }

        private void numPlaySpeed_ValueChanged(object sender, EventArgs e)
        {
            if (dgvObjects.SelectedRows.Count > 0)
            {
                if (lvwAnimations.SelectedItems.Count > 0)
                {
                    fpxAnimation anim = gObjects[dgvObjects.SelectedRows[0].Index].Animations[lvwAnimations.SelectedItems[0].Index];

                    anim.FrameSpeed = numPlaySpeed.Value;

                    ValidateObject();

                    AnimationPlayer.Interval = (int)Math.Max(1000 * numPlaySpeed.Value, 0.01m);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sXml = gCollections.OuterXml;

            XmlDocument xSave = new XmlDocument();
            xSave.LoadXml(sXml);
            EditorManager.SetObjectCollection(xSave);

            ValidateSave();
            ManageControls();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel your changes?", "Faples Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result.Equals(DialogResult.Yes))
            {
                string sXml = EditorManager.GetObjectCollection().OuterXml;

                XmlDocument xLoad = new XmlDocument();
                xLoad.LoadXml(sXml);
                gCollections = xLoad;

                ClearCollections();

                foreach(XmlNode oNode in gCollections.FirstChild.ChildNodes)
                {
                    cmbCollection.Items.Add(oNode.Attributes["Name"].Value);
                }

                if (cmbCollection.Items.Count > 0)
                    cmbCollection.SelectedIndex = 0;
                else
                    cmbCollection.SelectedIndex = -1;
            }

            ValidateSave();
            ManageControls();
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

            if (gHolds.ContainsKey(iHoldGroup))
            {
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

                gObjects[dgvObjects.SelectedRows[0].Index].HoldGroups = new Dictionary<int, List<fpxHold>>(gHolds);
            }

            ValidateObject();
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            gHolds = new Dictionary<int, List<fpxHold>>(gObjects[dgvObjects.SelectedRows[0].Index].HoldGroups);
            int iIndex = tsbHolds.SelectedIndex;

            if (gHolds[iIndex].Count > 0)
            {
                tsbHolds.Items.RemoveAt(iIndex);
                gHolds.Remove(iIndex);

                Dictionary<int, List<fpxHold>> holdReorder = new Dictionary<int, List<fpxHold>>();
                
                foreach(int key in gHolds.Keys)
                {
                    if(key > iIndex)
                    {
                        holdReorder.Add(key-1, gHolds[key]);
                    }
                    else
                    {
                        holdReorder.Add(key, gHolds[key]);
                    }
                }

                gHolds = new Dictionary<int, List<fpxHold>>(holdReorder);

                iHoldGroup = gHolds.Count;

                if(tsbHolds.Items.Count > 0)
                {
                    if (tsbHolds.SelectedIndex < 0)
                        tsbHolds.SelectedIndex = 0;
                }
            }

            gObjects[dgvObjects.SelectedRows[0].Index].HoldGroups = new Dictionary<int, List<fpxHold>>(gHolds);

            ValidateObject();
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
                    gObjects[dgvObjects.SelectedRows[0].Index].HoldGroups = new Dictionary<int, List<fpxHold>>(gHolds);
                    ValidateObject();
                }
            }
        }

        private void tsbHolds_SelectedIndexChanged(object sender, EventArgs e)
        {
            ediObject.Invalidate();
        }
        private void ediObject_MouseMove(object sender, MouseEventArgs e)
        {
            iPointX = e.X;
            iPointY = e.Y;

            ediObject.Invalidate();
        }

        private void ediObject_MouseClick(object sender, MouseEventArgs e)
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
                hold.y1 = Utility.GetGameResolutionX(e.Y);

                if (tsbFoothold.Checked)
                    hold.type = "foot";
                else if(tsbClimbhold.Checked)
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

                ediObject.Invalidate();
            }
        }
        private void ediObject_Paint(object sender, PaintEventArgs e)
        {        
            if (gCurrObject != null)
            {
                Graphics g = e.Graphics;

                int iWidth = gCurrObject.Width;
                int iHeight = gCurrObject.Height;

                Rectangle rectObj = new Rectangle(0, 0, iWidth, iHeight);

                if (rectObj.Width < 5)
                    rectObj.Width = 5;

                if (rectObj.Height < 5)
                    rectObj.Height = 5;

                ediObject.Size = rectObj.Size;

                g.DrawImage(gCurrObject, rectObj);

                for(int i = 0; i < gHolds.Count; i++)
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

                        if (tsbHolds.SelectedIndex == i && (tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked))
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
                            e.Graphics.DrawLine(pen, new Point(x - width / 2, y - height / 2), new Point(x2- width / 2, y2 - height / 2));
                        }                     
                    }
                }
            }
            else
            {
                Graphics g = e.Graphics;

                ediObject.Size = new Size(5, 5);
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

                // Create location and size of rectangle.
                int x = iPointX;
                int y = iPointY;
                int width = Utility.GetGameResolutionX(Utility.GRAPHICS_POINT_RECT);
                int height = Utility.GetGameResolutionY(Utility.GRAPHICS_POINT_RECT);

                g.DrawRectangle(pen, x - width/2, y - height/2, width, height);
            }
        }

        private void Animation_Tick(object sender, EventArgs e)
        {
            int iIndexF = (int)numTotalFrames.Value;
            int iIndexR = (int)numReelIndex.Value;

            while (numFrameWidth.Value * iIndexF > gCurrSheet.Width)
                iIndexF--;

            while (numReelHeight.Value * iIndexR > gCurrSheet.Height)
                iIndexR--;

            numReelIndex.Value = Math.Max(iIndexR, 1);
            numTotalFrames.Value = Math.Max(iIndexF, 1);

            int frameX = (int)Math.Ceiling(numFrameWidth.Value * (iFrame));
            int frameY = (int)Math.Ceiling(numReelHeight.Value * (numReelIndex.Value - 1));

            int frameWidth = decimal.ToInt32(numFrameWidth.Value);
            int frameHeight = decimal.ToInt32(numReelHeight.Value);


            Point oSpriteLocation = new Point(frameX, frameY);
            Size oSpriteSize = new Size(frameWidth, frameHeight);
            Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

            if (oCrop.X + oCrop.Width > gCurrSheet.Width)
            {
                oCrop.Width = gCurrSheet.Width;
                oCrop.X = 0;
            }


            if (oCrop.Y + oCrop.Height > gCurrSheet.Height)
            {
                oCrop.Height = gCurrSheet.Height;
                oCrop.Y = 0;
            }


            Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

            if (gCurrObject != null)
                gCurrObject.Dispose();

            gCurrObject = spriteFull;

            if (iFrame < (numTotalFrames.Value - 1))
                iFrame++;
            else
                iFrame = 0;

            ediObject.Invalidate();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Z))
            {
                if(tsbAddPoint.Checked)
                {
                    if (gHolds.Count > 0)
                    {
                        List<fpxHold> holdGroup = gHolds[iHoldGroup];

                        if (holdGroup.Count > 0)
                        {
                            fpxHold prevH = holdGroup.ElementAtOrDefault(holdGroup.Count - 2);

                            if (prevH != null)
                            {
                                prevH.next = null;
                                prevH.nextid = 0;
                                prevH.x2 = 0;
                                prevH.y2 = 0;

                                holdGroup[holdGroup.Count - 2] = prevH;
                            }

                            holdGroup.RemoveAt(holdGroup.Count - 1);
                        }


                        gHolds[iHoldGroup] = holdGroup;
                    }
                }
            }

            ediObject.Invalidate();

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Main Methods
        public void Prepare()
        {
            AnimationPlayer.Tick += Animation_Tick;

            string sXml = EditorManager.GetObjectCollection().OuterXml;
        
            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gCollections = xLoad;

            PrepareControls();

            ManageControls();
        }
        public void PrepareControls()
        {
            DataGridViewImageColumn colImg = new DataGridViewImageColumn();
            colImg.Width = (int)(dgvObjects.Width * 0.75);
            DataGridViewTextBoxColumn colText = new DataGridViewTextBoxColumn();
            colText.Width = (int)(dgvObjects.Width * 0.25);

            dgvObjects.Columns.Add(colImg);
            dgvObjects.Columns.Add(colText);

            dgvObjects.ColumnHeadersVisible = false;
            dgvObjects.RowHeadersVisible = false;
            dgvObjects.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvObjects.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvObjects.AllowUserToAddRows = false;
        }
        public void ValidateSave()
        {
            bool bDirtyCheck = false;

            if (!gCollections.OuterXml.Equals(EditorManager.GetObjectCollection().OuterXml))
                bDirtyCheck = true;

            Dirty = bDirtyCheck;
        }

        public void ValidateObject()
        {
            string sName = dgvObjects.SelectedRows[0].Cells[1].Value.ToString();
            XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");
            XmlNode oCollection = null;
            XmlNode oCategory = null;
            XmlNode oSheet = null;

            foreach (XmlNode oNode in oCollections.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCollection.SelectedItem.ToString()))
                {
                    oCollection = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oCollection.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbCategory.SelectedItem.ToString()))
                {
                    oCategory = oNode;
                    break;
                }
            }

            foreach (XmlNode oNode in oCategory.ChildNodes)
            {
                if (oNode.Attributes["Name"].Value.Equals(cmbSheet.SelectedItem.ToString()))
                {
                    oSheet = oNode;
                    break;
                }
            }

            if (oCollection == null)
                return;

            if (!Utility.XmlNodeExists(oSheet, sName))
            {
                fpxObject oObj = gObjects[dgvObjects.SelectedRows[0].Index];

                foreach (XmlNode oNode in oSheet)
                {
                    if (oNode.Attributes["Name"].Value.Equals(oObj.Name))
                    {
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

                        dgvObjects.SelectedRows[0].Cells[0].Dispose();
                        dgvObjects.SelectedRows[0].Cells[0].Value = spriteSnap;

                        XmlAttribute oName = gCollections.CreateAttribute("Name");
                        oName.Value = oObj.Name;
                        XmlAttribute oX = gCollections.CreateAttribute("x");
                        oX.Value = oObj.x.ToString();
                        XmlAttribute oY = gCollections.CreateAttribute("y");
                        oY.Value = oObj.y.ToString();
                        XmlAttribute oWidth = gCollections.CreateAttribute("width");
                        oWidth.Value = oObj.width.ToString();
                        XmlAttribute oHeight = gCollections.CreateAttribute("height");
                        oHeight.Value = oObj.height.ToString();
                        XmlAttribute oAnimated = gCollections.CreateAttribute("Animated");
                        oAnimated.Value = oObj.Animated.ToString();

                        XmlNode oAnimations = gCollections.CreateNode("element", "Animations", "");
                        XmlAttribute oAnimationIndex = gCollections.CreateAttribute("Index");
                        oAnimationIndex.Value = "0";

                        if (lvwAnimations.Items.Count > 0)
                        {
                            if (lvwAnimations.SelectedItems.Count > 0)
                                oAnimationIndex.Value = (lvwAnimations.SelectedItems[0].Index + 1).ToString();

                            foreach(fpxAnimation anim in oObj.Animations)
                            {
                                XmlAttribute oAnimName = gCollections.CreateAttribute("Name");
                                oAnimName.Value = anim.Name.ToString();
                                XmlAttribute oReelHeight = gCollections.CreateAttribute("ReelHeight");
                                oReelHeight.Value = anim.ReelHeight.ToString();
                                XmlAttribute oReelIndex = gCollections.CreateAttribute("ReelIndex");
                                oReelIndex.Value = anim.ReelIndex.ToString();
                                XmlAttribute oFrameWidth = gCollections.CreateAttribute("FrameWidth");
                                oFrameWidth.Value = anim.FrameWidth.ToString();
                                XmlAttribute oTotalFrames = gCollections.CreateAttribute("TotalFrames");
                                oTotalFrames.Value = anim.TotalFrames.ToString();
                                XmlAttribute oFrameSpeed = gCollections.CreateAttribute("FrameSpeed");
                                oFrameSpeed.Value = anim.FrameSpeed.ToString();

                                XmlNode oAnim = gCollections.CreateNode("element", "Animation", "");
                                oAnim.Attributes.Append(oAnimName);
                                oAnim.Attributes.Append(oReelHeight);
                                oAnim.Attributes.Append(oReelIndex);
                                oAnim.Attributes.Append(oFrameWidth);
                                oAnim.Attributes.Append(oTotalFrames);
                                oAnim.Attributes.Append(oFrameSpeed);
                                oAnimations.AppendChild(oAnim);
                            }
                        }

                        oAnimations.Attributes.Append(oAnimationIndex);
                        XmlNode oHolds = gCollections.CreateNode("element", "Holds", "");

                        for(int i = 0; i < oObj.HoldGroups.Count; i++)
                        {
                            XmlNode oHoldGroup = gCollections.CreateNode("element", "HoldGroup", "");
                            XmlAttribute oGroupId = gCollections.CreateAttribute("id");
                            oGroupId.Value = i.ToString();
                            oHoldGroup.Attributes.Append(oGroupId);

                            foreach (fpxHold hold in oObj.HoldGroups[i])
                            {
                                XmlAttribute oId = gCollections.CreateAttribute("id");
                                oId.Value = hold.id.ToString();
                                XmlAttribute oGid = gCollections.CreateAttribute("gid");
                                oGid.Value = hold.gid.ToString();
                                XmlAttribute oLid = gCollections.CreateAttribute("lid");
                                oLid.Value = hold.lid.ToString();
                                XmlAttribute oX1 = gCollections.CreateAttribute("x1");
                                oX1.Value = hold.x1.ToString();
                                XmlAttribute oX2 = gCollections.CreateAttribute("x2");
                                oX2.Value = hold.x2.ToString();
                                XmlAttribute oY1 = gCollections.CreateAttribute("y1");
                                oY1.Value = hold.y1.ToString();
                                XmlAttribute oY2 = gCollections.CreateAttribute("y2");
                                oY2.Value = hold.y2.ToString();
                                XmlAttribute oNextId = gCollections.CreateAttribute("nextid");
                                oNextId.Value = hold.nextid.ToString();
                                XmlAttribute oPrevId = gCollections.CreateAttribute("previd");
                                oPrevId.Value = hold.previd.ToString();
                                XmlAttribute oType = gCollections.CreateAttribute("type");
                                oType.Value = hold.type;
                                XmlAttribute oForce = gCollections.CreateAttribute("force");
                                oForce.Value = hold.force.ToString();
                                XmlAttribute oCantPass = gCollections.CreateAttribute("cantPass");
                                oCantPass.Value = hold.cantPass.ToString();
                                XmlAttribute oCantDrop = gCollections.CreateAttribute("cantDrop");
                                oCantDrop.Value = hold.cantDrop.ToString();
                                XmlAttribute oCantMove = gCollections.CreateAttribute("cantMove");
                                oCantMove.Value = hold.cantMove.ToString();

                                XmlNode holdPoint = gCollections.CreateNode("element", "Hold", "");
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

                        if(!oObj.Animated)
                        {
                            Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                            if (gCurrObject != null)
                                gCurrObject.Dispose();

                            gCurrObject = spriteFull;
                        }
                       
                       

                        XmlNode oObject = gCollections.CreateNode("element", "Object", "");
                        oObject.Attributes.Append(oName);
                        oObject.Attributes.Append(oX);
                        oObject.Attributes.Append(oY);
                        oObject.Attributes.Append(oWidth);
                        oObject.Attributes.Append(oHeight);
                        oObject.Attributes.Append(oAnimated);
                        oObject.AppendChild(oAnimations);
                        oObject.AppendChild(oHolds);


                        oSheet.ReplaceChild(oObject, oNode);
                        oCategory.ReplaceChild(oSheet, oSheet);
                        oCollection.ReplaceChild(oCategory, oCategory);
                        oCollections.ReplaceChild(oCollection, oCollection);
                        gCollections.ReplaceChild(oCollections, oCollections);
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

            bool bCollection = false;
            bool bCollectionAdd = false;
            bool bCollectionDel = false;

            bool bCategory = false;
            bool bCategoryAdd = false;
            bool bCategoryDel = false;

            bool bSheet = false;
            bool bSheetAdd = false;
            bool bSheetDel = false;

            bool bObject = false;
            bool bObjectAdd = false;
            bool bObjectDel = false;

            // Display object properties
            bool bObjectSelected = false;
            bool bObjectAnimated = false;

            // Display Animation Properties
            bool bAnimationSelected = false;

            bool bPointMode = false;
            bool bAddingPoint = false;



            // Enable Buttons that are always displayed
            bCollectionAdd = true;

            // Validate dependant controls
            if (cmbCollection.Items.Count > 0)
            {
                bCollection = true;
                bCategoryAdd = true;

                if (cmbCollection.SelectedItem != null)
                {
                    bCollectionDel = true;
                }

                if (cmbCategory.Items.Count > 0)
                {
                    bCategory = true;
                    bSheetAdd = true;

                    if (cmbCategory.SelectedItem != null)
                    {
                        bCategoryDel = true;
                    }

                    if (cmbSheet.Items.Count > 0)
                    {
                        bSheet = true;
                        bObjectAdd = true;

                        if (cmbSheet.SelectedItem != null)
                        {
                            bSheetDel = true;
                        }

                        if (dgvObjects.Rows.Count > 0)
                        {
                            bObject = true;

                            if (dgvObjects.SelectedRows.Count > 0)
                            {
                                bObjectDel = true;
                                bObjectSelected = true;

                                bObjectAnimated = gObjects[dgvObjects.SelectedRows[0].Index].Animated;

                                if (bObjectAnimated)
                                {
                                    if (lvwAnimations.Items.Count > 0)
                                    {
                                        if (lvwAnimations.SelectedItems.Count > 0)
                                        {
                                            bAnimationSelected = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Check Toggle visibility
            if(tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked)
            {
                bPointMode = true;

                if (tsbAddPoint.Checked)
                {
                    bAddingPoint = true; ;
                }
            }



            pnlAnimations.Enabled = !bPointMode && !bAddingPoint;
            pnlToolbox.Enabled = !bPointMode && !bAddingPoint;
            pnlProperties.Enabled = !bPointMode && !bAddingPoint;

            tsbAddPoint.Enabled = bPointMode;
            tsbHolds.Enabled = bPointMode && !bAddingPoint && tsbHolds.Items.Count > 0;
            tsbDelete.Enabled = bPointMode && !bAddingPoint && tsbHolds.Items.Count > 0;
            tsbHoldProperties.Enabled = bPointMode && !bAddingPoint && tsbHolds.Items.Count > 0;
            tsbConfirm.Enabled = bAddingPoint;

            // Enable/Disable All controls

            cmbCollection.Enabled = bCollection;
            btnAddCollection.Enabled = bCollectionAdd;
            btnDeleteCollection.Enabled = bCollectionDel;

            cmbCategory.Enabled = bCategory;
            btnAddCategory.Enabled = bCategoryAdd;
            btnDeleteCategory.Enabled = bCategoryDel;

            cmbSheet.Enabled = bSheet;
            btnAddSheet.Enabled = bSheetAdd;
            btnDeleteSheet.Enabled = bSheetDel;

            dgvObjects.Enabled = bObject;
            btnAddObject.Enabled = bObjectAdd;
            btnDeleteObject.Enabled = bObjectDel;

            tsbFoothold.Enabled = bObjectSelected && !tsbClimbhold.Checked && !tsbSeathold.Checked && !bAddingPoint;
            tsbClimbhold.Enabled = bObjectSelected && !tsbFoothold.Checked && !tsbSeathold.Checked && !bAddingPoint;
            tsbSeathold.Enabled = bObjectSelected && !tsbFoothold.Checked && !tsbClimbhold.Checked && !bAddingPoint;

            numX.Enabled = bObjectSelected;
            numY.Enabled = bObjectSelected;
            numWidth.Enabled = bObjectSelected;
            numHeight.Enabled = bObjectSelected;
            chkAnimated.Enabled = bObjectSelected;

            lvwAnimations.Enabled = bObjectAnimated;
            btnAddAnim.Enabled = bObjectAnimated;
            numReelHeight.Enabled = bObjectAnimated;

            btnRemoveAnim.Enabled = bAnimationSelected;
            numReelIndex.Enabled = bAnimationSelected;
            numFrameWidth.Enabled = bAnimationSelected;
            numTotalFrames.Enabled = bAnimationSelected;
            numPlaySpeed.Enabled = bAnimationSelected;

            btnSave.Enabled = Dirty;
            btnCancel.Enabled = Dirty;

            ediObject.Invalidate();
        }

        public void ClearCollections()
        {
            cmbCollection.Items.Clear();
            ClearCategories();
        }
        public void ClearCategories()
        {
            cmbCategory.Items.Clear();
            ClearSheets();
        }
        public void ClearSheets()
        {
            cmbSheet.Items.Clear();
            if (gCurrSheet != null)
                gCurrSheet.Dispose();

            gCurrSheet = null;

            ClearObjects();
        }
        public void ClearObjects()
        {
            dgvObjects.Rows.Clear();
            gObjects = new List<fpxObject>();
            if (gCurrObject != null)
                gCurrObject.Dispose();

            gCurrObject = null;

            ClearObjectProperties();
        }

        public void ClearObjectProperties()
        {
            numX.Value = numX.Minimum;
            numY.Value = numY.Minimum;
            numWidth.Value = numWidth.Minimum;
            numHeight.Value = numHeight.Minimum;
            chkAnimated.Checked = false;
            numReelHeight.Value = numReelHeight.Minimum;
            ClearAnimationProperties();
            ClearHoldProperties();
        }

        public void ClearAnimationProperties()
        {
            numReelIndex.Value = numReelIndex.Minimum;
            numFrameWidth.Value = numFrameWidth.Minimum;
            numTotalFrames.Value = numTotalFrames.Minimum;
            numPlaySpeed.Value = numPlaySpeed.Minimum;
        }

        public void ClearHoldProperties()
        {
            gHolds = new Dictionary<int, List<fpxHold>>();
            tsbHolds.Items.Clear();
            iHoldGroup = 0;
        }

        public void Reload()
        {
            ClearCollections();

            string sXml = EditorManager.GetObjectCollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gCollections = xLoad;

            XmlNode oCollections = gCollections.SelectSingleNode("//fpxObjects");

            foreach(XmlNode oCollection in oCollections.ChildNodes)
            {
                cmbCollection.Items.Add(oCollection.Attributes["Name"].Value);
            }

            if (cmbCollection.Items.Count > 0)
                cmbCollection.SelectedIndex = 0;
        }
        #endregion
    }
}
