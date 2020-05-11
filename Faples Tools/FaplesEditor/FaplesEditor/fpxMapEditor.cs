using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Media;
using WMPLib;

namespace FaplesEditor
{
    enum eControlMode
    {
        eObject,
        eTile,
        eHold,
        eUI
    }

    public partial class fpxMapEditor : UserControl
    {
        #region Declarations
        // Global Tools
        WindowsMediaPlayer gMusicPlayer = new WindowsMediaPlayer();

        // Map Collection variables
        XmlDocument gMapDoc = new XmlDocument();
        XmlDocument gObjDoc = new XmlDocument();
        XmlDocument gTileDoc = new XmlDocument();

        // Map Editing variables
        Bitmap gCurrSheet = null;
        Bitmap gCurrSprite = null;

        List<fpxObject> gObjects = new List<fpxObject>();
        List<fpxTile> gTiles = new List<fpxTile>();
        

        Dictionary<int, List<fpxHold>> gCurrHolds = new Dictionary<int, List<fpxHold>>();
        int iHoldGroup = 0;

        // Map Referenced variables
        List<fpxRegion> gRegions = new List<fpxRegion>();

        int iPointX = 0;
        int iPointY = 0;

        #endregion

        #region Properties
        public bool ObjectView { get; set; } = true;
        public bool TileView { get; set; } = true;
        public bool SceneryView { get; set; } = true;
        public bool HoldView { get; set; } = true;
        public bool PortalView { get; set; } = true;
        public bool MusicPlaying { get; set; } = false;
        public bool Flipped { get; set; } = false;
        public bool Dirty { get; set; } = false;
        #endregion

        #region Constructor
        public fpxMapEditor()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Event Handlers
        private void btnSave_Click(object sender, EventArgs e)
        {
            DoSave();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel your changes?", "Faples Editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result.Equals(DialogResult.Yes))
            {
                string sXml = EditorManager.GetMapCollection().OuterXml;

                XmlDocument xLoad = new XmlDocument();
                xLoad.LoadXml(sXml);
                gMapDoc = xLoad;

                ClearMaps();

                foreach (XmlNode oNode in gMapDoc.ChildNodes)
                {
                    cmbMaps.Items.Add(oNode.Attributes["Name"].Value);
                }

                if (cmbMaps.Items.Count > 0)
                    cmbMaps.SelectedIndex = 0;
                else
                    cmbMaps.SelectedIndex = -1;
            }

            ValidateSave();
            ManageControls();
        }

        private void tsbMap_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbMap.Checked;

            ResetMode();
            tsbMap.Checked = bSwapState;

            ManageControls();
        }

        private void tsbFoothold_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbFoothold.Checked;

            ResetMode();
            tsbFoothold.Checked = bSwapState;

            if(bSwapState)
            {
                tsbList.Items.Clear();

                for(int i = 0; i < gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count; i++)
                {
                    if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "foot")
                        tsbList.Items.Add("foot" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "climb")
                        tsbList.Items.Add("climb" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "seat")
                        tsbList.Items.Add("seat" + i);
                }

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;
            }

            ManageControls();
        }

        private void tsbSeathold_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbSeathold.Checked;

            ResetMode();
            tsbSeathold.Checked = bSwapState;

            if (bSwapState)
            {
                tsbList.Items.Clear();

                for (int i = 0; i < gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count; i++)
                {
                    if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "foot")
                        tsbList.Items.Add("foot" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "climb")
                        tsbList.Items.Add("climb" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "seat")
                        tsbList.Items.Add("seat" + i);
                }

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;
            }

            ManageControls();
        }

        private void tsbClimbhold_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbClimbhold.Checked;

            ResetMode();
            tsbClimbhold.Checked = bSwapState;

            if (bSwapState)
            {
                tsbList.Items.Clear();

                for (int i = 0; i < gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count; i++)
                {
                    if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "foot")
                        tsbList.Items.Add("foot" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "climb")
                        tsbList.Items.Add("climb" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "seat")
                        tsbList.Items.Add("seat" + i);
                }

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;
            }

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

            if (bSwapState)
            {
                ReloadTiles();

                tsbList.Items.Clear();

                foreach (fpxMapTile tile in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles)
                {
                    tsbList.Items.Add(tile.Tile.Name);
                }

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;
            }


            ManageControls();
        }
        private void tsbObject_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbObject.Checked;

            ResetMode();
            tsbObject.Checked = bSwapState;

            if (bSwapState)
            {
                ReloadObjects();

                tsbList.Items.Clear();

                foreach (fpxMapObject obj in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapObjects)
                {
                    tsbList.Items.Add(obj.Object.Name);
                }

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;
            }

            ManageControls();
        }
        private void tsbPortal_Click(object sender, EventArgs e)
        {
            bool bSwapState = !tsbPortal.Checked;

            ResetMode();
            tsbPortal.Checked = bSwapState;

            if(bSwapState)
            {
                tsbList.Items.Clear();

                foreach (fpxMapPortal portal in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals)
                {
                    tsbList.Items.Add(portal.Type + portal.ID);
                }

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;

                if (gCurrSprite != null)
                    gCurrSprite.Dispose();

                gCurrSprite = new Bitmap(Path.Combine(Utility.GLOBAL_PATH, "portal.png"));
            }

            ManageControls();
        }
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            tsbAdd.Checked = !tsbAdd.Checked;

            ValidateSave();
            ManageControls();
        }
        private void tsbConfirm_Click(object sender, EventArgs e)
        {
            tsbAdd.Checked = !tsbAdd.Checked;

            tsbList.Items.Clear();

            if (tsbObject.Checked)
            {
                foreach (fpxMapObject obj in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapObjects)
                {
                    tsbList.Items.Add(obj.Object.Name);
                }

                if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                    tsbList.SelectedIndex = 0;
            }
            else if(tsbTile.Checked)
            {
                foreach (fpxMapTile tile in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles)
                {
                    tsbList.Items.Add(tile.Tile.Name);
                }

                if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                    tsbList.SelectedIndex = 0;
            }
            else if(tsbPortal.Checked)
            {
                foreach (fpxMapPortal portal in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals)
                {
                    tsbList.Items.Add(portal.Type + portal.ID);
                }

                if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                    tsbList.SelectedIndex = 0;
            }
            else if(tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked)
            {
                for(int i = 0; i < gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count; i++)
                {
                    if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "foot")
                        tsbList.Items.Add("foot" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "climb")
                        tsbList.Items.Add("climb" + i);
                    else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "seat")
                        tsbList.Items.Add("seat" + i);
                }

                iHoldGroup = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count;
        
                if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                    tsbList.SelectedIndex = 0;
            }

            ValidateMaps();
        }
        private void tsbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tsbObject.Checked)
            {
                if (tsbList.SelectedIndex > -1)
                {
                    fpxMapObject oObject = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapObjects[tsbList.SelectedIndex];

                    numObjectX.Value = oObject.MapCoor.X;
                    numObjectY.Value = oObject.MapCoor.Y;
                    chkObjectDrop.Checked = oObject.CannotDrop;

                    foreach (List<fpxHold> oHoldGroup in oObject.Object.HoldGroups.Values)
                    {
                        foreach (fpxHold oHold in oHoldGroup)
                        {
                            oHold.cantDrop = oObject.CannotDrop;
                        }
                    }

                    ValidateMaps();
                    ediMap.Invalidate();
                }
            }
            else if(tsbTile.Checked)
            {
                if (tsbList.SelectedIndex > -1)
                {
                    fpxMapTile oTile = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles[tsbList.SelectedIndex];

                    numTileX.Value = oTile.MapCoor.X;
                    numTileY.Value = oTile.MapCoor.Y;
                    chkTileDrop.Checked = oTile.CannotDrop;

                    foreach (List<fpxHold> oHoldGroup in oTile.Tile.HoldGroups.Values)
                    {
                        foreach (fpxHold oHold in oHoldGroup)
                        {
                            oHold.cantDrop = oTile.CannotDrop;
                        }
                    }

                    ValidateMaps();
                    ediMap.Invalidate();
                }
            }

            ManageControls();
        }

        private void tsbProperties_Click(object sender, EventArgs e)
        {
            if (tsbList.SelectedIndex > -1)
            {
                if(tsbPortal.Checked)
                {
                    var oPortalProperties = new fpxPortalProperties();
                    oPortalProperties.LoadProperties(gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals[tsbList.SelectedIndex], gRegions);

                    oPortalProperties.ShowDialog();


                    if (oPortalProperties.DialogResult == DialogResult.Cancel)
                        return;


                    if (oPortalProperties.DialogResult == DialogResult.OK)
                    {
                        int iIndex = tsbList.SelectedIndex;

                        fpxMapPortal oPortal = oPortalProperties.SaveProperties();

                        //oPortal.MapID = gMaps[].MapID;

                        gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals[iIndex] = oPortal;


                        tsbList.Items.Clear();

                        foreach(fpxMapPortal oP in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals)
                        {
                            tsbList.Items.Add(oP.Type + oP.ID);
                        }

                        tsbList.SelectedIndex = iIndex;
                       
                       ValidateMaps();
                    }
                }
                if(tsbFoothold.Checked || tsbSeathold.Checked || tsbClimbhold.Checked)
                {
                    var oHoldProperties = new fpxHoldProperties();
                    oHoldProperties.LoadHolds(gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[tsbList.SelectedIndex]);

                    oHoldProperties.ShowDialog();

                    if (oHoldProperties.DialogResult == DialogResult.Cancel)
                        return;


                    if (oHoldProperties.DialogResult == DialogResult.OK)
                    {
                        gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[tsbList.SelectedIndex] = oHoldProperties.SaveHolds();
                        ValidateMaps();
                    }
                }       
            }
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (tsbObject.Checked)
            {
                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapObjects.RemoveAt(tsbList.SelectedIndex);
                tsbList.Items.Remove(tsbList.SelectedItem);

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;
            }
            else if(tsbTile.Checked)
            {
                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles.RemoveAt(tsbList.SelectedIndex);
                tsbList.Items.Remove(tsbList.SelectedItem);

                if (tsbList.Items.Count > 0)
                    tsbList.SelectedIndex = 0;
            }
            else if(tsbPortal.Checked)
            {
                int iIndex = tsbList.SelectedIndex;

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals.RemoveAt(iIndex);
                tsbList.Items.Clear();

                List<fpxMapPortal> portalReorder = new List<fpxMapPortal>();

                for(int i = 0; i < gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals.Count; i++)
                {
                    fpxMapPortal oPortal = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals[i];

                    oPortal.ID = i;

                    portalReorder.Add(oPortal);

                    tsbList.Items.Add(oPortal.Type + oPortal.ID);
                }

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals = portalReorder;

                if (tsbList.Items.Count > 0)
                {
                    if (tsbList.SelectedIndex < 0)
                        tsbList.SelectedIndex = 0;
                }
            }
            else if(tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked)
            {
                int iIndex = tsbList.SelectedIndex;

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Remove(iIndex);
                tsbList.Items.RemoveAt(iIndex);

                Dictionary<int, List<fpxHold>> holdReorder = new Dictionary<int, List<fpxHold>>();

                foreach (int key in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Keys)
                {
                    if (key > iIndex)
                    {
                        holdReorder.Add(key - 1, gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[key]);
                    }
                    else
                    {
                        holdReorder.Add(key, gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[key]);
                    }
                }

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds = new Dictionary<int, List<fpxHold>>(holdReorder);

                iHoldGroup = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count;

                if (tsbList.Items.Count > 0)
                {
                    if (tsbList.SelectedIndex < 0)
                        tsbList.SelectedIndex = 0;
                }
            }

            ValidateMaps();
        }

        private void cmbLayer_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cmbRegion.SelectedIndex != -1)
            {
                if (tsbObject.Checked)
                {
                    tsbList.Items.Clear();

                    foreach (fpxMapObject obj in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapObjects)
                    {
                        tsbList.Items.Add(obj.Object.Name);
                    }

                    if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                        tsbList.SelectedIndex = 0;
                }
                else if (tsbTile.Checked)
                {
                    tsbList.Items.Clear();

                    foreach (fpxMapTile tile in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles)
                    {
                        tsbList.Items.Add(tile.Tile.Name);
                    }

                    if (tsbList.Items.Count > 0 && tsbList.SelectedIndex < 0)
                        tsbList.SelectedIndex = 0;
                }
                else if (tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked)
                {
                    tsbList.Items.Clear();

                    for (int i = 0; i < gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count; i++)
                    {
                        if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "foot")
                            tsbList.Items.Add("foot" + i);
                        else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "climb")
                            tsbList.Items.Add("climb" + i);
                        else if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds[i][0].type == "seat")
                            tsbList.Items.Add("seat" + i);
                    }

                    iHoldGroup = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count;
                    if (tsbList.Items.Count > 0)
                    {
                        if (tsbList.SelectedIndex < 0)
                            tsbList.SelectedIndex = 0;
                    }
                }

                if (cmbLayer.SelectedItem != null)
                {
                    iHoldGroup = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapHolds.Count;
                    numParallaxX.Value = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].ParallaxX;
                    numParallaxY.Value = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].ParallaxY;
                    numScrollX.Value = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].ScrollX;
                    numScrollY.Value = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].ScrollY;
                    chkTileX.Checked = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].TileX;
                    chkTileY.Checked = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].TileY;
                    txtScenery.Text = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapScenery.SpriteName;
                }

                ManageControls();
            }       
        }
        private void btnMapMusic_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "MP3 files(*.mp3)| *.mp3";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.MUSIC_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.MUSIC_PATH))
                    {
                        MessageBox.Show("Please select image within 'Objects' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    txtBGMusic.Text = opf.SafeFileName;
                    bValid = true;
                }
                else
                    return;
            }
        }
        private void pbMusicPreview_Click(object sender, EventArgs e)
        {
            if (!MusicPlaying)
            {
                gMusicPlayer.URL = Path.Combine(Utility.MUSIC_PATH, txtBGMusic.Text);
                gMusicPlayer.controls.play();
            }
            else
            {
                gMusicPlayer.controls.stop();
            }

            MusicPlaying = !MusicPlaying;
        }

        private void btnRegionNew_Click(object sender, EventArgs e)
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

                    if (!cmbRegion.Items.Contains(sName))
                    {
                        fpxRegion oReg = new fpxRegion();
                        oReg.Name = sName;
                        oReg.ID = cmbRegion.Items.Count;

                        bValid = true;
                        cmbRegion.Items.Add(sName);
                        gRegions.Add(oReg);


                        if (cmbRegion.SelectedIndex < 0)
                        {
                            int iIndex = Math.Max(cmbRegion.Items.Count - 1, 0);

                            cmbRegion.SelectedIndex = iIndex;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Region already exists. Please use a different name.");
                    }
                }            
            }

            oAddDialog.Close();
            ValidateMaps();
        }

        private void btnRegionRemove_Click(object sender, EventArgs e)
        {
            if (cmbRegion.SelectedItem != null)
            {
                cmbRegion.Items.Remove(cmbRegion.SelectedItem);
                if (cmbRegion.Items.Count > 0)
                    cmbRegion.SelectedIndex = 0;
                else
                    cmbRegion.SelectedIndex = -1;                
            }

            ValidateMaps();
        }

        private void cmbRegion_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearMaps();

            if(cmbRegion.SelectedIndex > -1)
            {
                List<fpxMap> oMaps = gRegions[cmbRegion.SelectedIndex].Maps;

                foreach(fpxMap oMap in oMaps)
                {
                    cmbMaps.Items.Add(oMap.MapName);
                }

                if (cmbMaps.Items.Count > 0)
                {
                    cmbMaps.SelectedIndex = 0;
                }
            }

            ManageControls();
            ediMap.Invalidate();
        }
        private void btnMapNew_Click(object sender, EventArgs e)
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

                    if (!cmbMaps.Items.Contains(sName))
                    {
                        try
                        {
                            fpxMap map = new fpxMap();
                            map.MapID = cmbMaps.Items.Count;
                            map.RegionID = gRegions[cmbRegion.SelectedIndex].ID;
                            map.MapName = sName;

                            for (int i = 0; i < cmbLayer.Items.Count; i++)
                            {
                                fpxMapLayer oMapLayer = new fpxMapLayer();
                                map.MapLayers.Add(oMapLayer);
                            }

                            gRegions[cmbRegion.SelectedIndex].Maps.Add(map);

                            bValid = true;
                            cmbMaps.Items.Add(sName);

                            if (cmbMaps.SelectedIndex < 0)
                            {
                                int iIndex = Math.Max(cmbMaps.Items.Count - 1, 0);

                                cmbMaps.SelectedIndex = iIndex;

                                ediMap.Size = new Size(5, 5);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error inserting Map. \n\n" + ex.Message, "Editor Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Map already exists. Please use a different name.");
                    }
                }
            }

            oAddDialog.Close();
            ValidateMaps();
        }

        private void btnMapRemove_Click(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedItem != null)
            {
                gRegions[cmbRegion.SelectedIndex].Maps.RemoveAt(cmbMaps.SelectedIndex);
                cmbMaps.Items.Remove(cmbMaps.SelectedItem);

                if (cmbMaps.Items.Count > 0)
                    cmbMaps.SelectedIndex = 0;
                else
                    cmbMaps.SelectedIndex = -1;
            }

            ValidateMaps();
        }

        private void cmbMaps_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cmbMaps.SelectedIndex > -1)
            {
                fpxMap oMap = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex];

                numMapWidth.Value = oMap.MapWidth;
                numMapHeight.Value = oMap.MapHeight;
                numBoundLeft.Value = oMap.MapBoundL;
                numBoundRight.Value = oMap.MapBoundR;
                numBoundTop.Value = oMap.MapBoundT;
                numBoundBottom.Value = oMap.MapBoundB;
                txtBGMusic.Text = oMap.BackgroundMusic;
            }

            ediMap.Invalidate();
        }
        private void numMapWidth_ValueChanged(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedIndex > -1)
            {

                fpxMap oMap = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex];

                if (numMapWidth.Value != oMap.MapWidth)
                    oMap.MapWidth = (int)numMapWidth.Value;

                int iWidth = Math.Max(oMap.MapWidth, 5);
                int iHeight = Math.Max(oMap.MapHeight, 5);
                ediMap.Size = new Size(iWidth, iHeight);

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex] = oMap;

                ValidateMaps();
            }
        }

        private void numMapHeight_ValueChanged(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedIndex > -1)
            {
                fpxMap oMap = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex];

                if (numMapHeight.Value != oMap.MapHeight)
                    oMap.MapHeight = (int)numMapHeight.Value;

                int iWidth = Math.Max(oMap.MapWidth, 5);
                int iHeight = Math.Max(oMap.MapHeight, 5);
                ediMap.Size = new Size(iWidth, iHeight);

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex] = oMap;

                ValidateMaps();
            }
        }

        private void numBoundRight_ValueChanged(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedIndex > -1)
            {
                fpxMap oMap = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex];

                if (numBoundRight.Value != oMap.MapBoundR)
                    oMap.MapBoundR = (int)numBoundRight.Value;

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex] = oMap;

                ValidateMaps();
            }
        }

        private void numBoundLeft_ValueChanged(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedIndex > -1)
            {
                fpxMap oMap = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex];

                if (numBoundLeft.Value != oMap.MapBoundL)
                    oMap.MapBoundL = (int)numBoundLeft.Value;

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex] = oMap;

                ValidateMaps();
            }
        }

        private void numBoundTop_ValueChanged(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedIndex > -1)
            {
                fpxMap oMap = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex];

                if (numBoundTop.Value != oMap.MapBoundT)
                    oMap.MapBoundT = (int)numBoundTop.Value;

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex] = oMap;

                ValidateMaps();
            }
        }

        private void numBoundBottom_ValueChanged(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedIndex > -1)
            {
                fpxMap oMap = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex];

                if (numBoundBottom.Value != oMap.MapBoundB)
                    oMap.MapBoundB = (int)numBoundBottom.Value;

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex] = oMap;

                ValidateMaps();
            }
        }
        private void chkFlip_CheckedChanged(object sender, EventArgs e)
        {
            if (gCurrSprite != null)
            {
                gCurrSprite.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
        }

        private void numParallaxX_ValueChanged(object sender, EventArgs e)
        {
            if (GetLayerIndex() >= 0)
            {
                int oLayer = GetLayerIndex();

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].ParallaxX = numParallaxX.Value;
            }

            ValidateMaps();
        }

        private void numParallaxY_ValueChanged(object sender, EventArgs e)
        {
            if (GetLayerIndex() >= 0)
            {
                int oLayer = GetLayerIndex();

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].ParallaxY = numParallaxY.Value;
            }

            ValidateMaps();
        }
        private void numScrollX_ValueChanged(object sender, EventArgs e)
        {
            if (GetLayerIndex() >= 0)
            {
                int oLayer = GetLayerIndex();

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].ScrollX = numScrollX.Value;
            }

            ValidateMaps();
        }

        private void numScrollY_ValueChanged(object sender, EventArgs e)
        {
            if (GetLayerIndex() >= 0)
            {
                int oLayer = GetLayerIndex();

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].ScrollY = numScrollY.Value;
            }

            ValidateMaps();
        }

        private void chkTileX_CheckedChanged(object sender, EventArgs e)
        {
            if (GetLayerIndex() >= 0)
            {
                int oLayer = GetLayerIndex();

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].TileX = chkTileX.Checked;
            }

            ValidateMaps();
        }

        private void chkTileY_CheckedChanged(object sender, EventArgs e)
        {
            if (GetLayerIndex() >= 0)
            {
                int oLayer = GetLayerIndex();

                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].TileY = chkTileY.Checked;
            }

            ValidateMaps();
        }

        private void dgvScenery_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int oLayer = GetLayerIndex();

            string sFlip = "";

            if (chkFlip.Checked)
                sFlip += "Flipped";

            if (gCurrSprite != null)
                gCurrSprite.Dispose();

            if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites.Count > 0)
            {
                foreach (fpxSprite oSprMapping in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites)
                {
                    if (oSprMapping.SpriteSheet.Equals(txtScenery.Text) && oSprMapping.SpriteName.Equals(txtScenery.Text))
                    {
                        oSprMapping.Sprite.Dispose();
                        gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites.Remove(oSprMapping);
                        break;
                    }
                    else if (oSprMapping.SpriteSheet.Equals(txtScenery.Text) && oSprMapping.SpriteName.Equals(txtScenery.Text + "Flipped"))
                    {
                        oSprMapping.Sprite.Dispose();
                        gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites.Remove(oSprMapping);
                        break;
                    }
                }
            }

            txtScenery.Text = dgvScenery.Rows[e.RowIndex].Cells[1].Value.ToString();

            gCurrSprite = new Bitmap(Path.Combine(Utility.SCENERY_PATH, txtScenery.Text));

            fpxMapScenery oScenMapping = new fpxMapScenery();
            oScenMapping.SpriteName = txtScenery.Text;

            foreach (fpxSprite oSprMapping in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites)
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
                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites.Add(oSprMapping);
            }

            if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapScenery.Sprite != null)
                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapScenery.Sprite.Dispose();

            gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapScenery = oScenMapping;

            ValidateMaps();
        }

        private void txtBGMusic_TextChanged(object sender, EventArgs e)
        {
            if (cmbMaps.SelectedIndex > -1)
            {
                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].BackgroundMusic = txtBGMusic.Text;

                ValidateMaps();
            }
        }

        private void btnSceneryClear_Click(object sender, EventArgs e)
        {
            int oLayer = GetLayerIndex();

            txtScenery.Text = "";
            if (gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapScenery.Sprite != null)
                gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapScenery.Sprite.Dispose();

            gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapScenery = new fpxMapScenery();
            ValidateMaps();
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

                        if (chkFlip.Checked)
                            spriteFull.RotateFlip(RotateFlipType.RotateNoneFlipX);

                        gCurrSprite = spriteFull;

                        gCurrHolds = new Dictionary<int, List<fpxHold>>(oT.HoldGroups);
                    }
                }
            }

            ManageControls();
        }

        private void dgvTiles_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTiles.SelectedRows.Count > 0)
            {
                if (gCurrSprite != null)
                    gCurrSprite.Dispose();

                gCurrHolds.Clear();

                foreach (fpxTile oTile in gTiles)
                {
                    if (oTile.Name.Equals(dgvTiles.SelectedRows[0].Cells[1].Value))
                    {
                        Point oSpriteLocation = new Point(oTile.x, oTile.y);
                        Size oSpriteSize = new Size(oTile.width, oTile.height);
                        Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                        Bitmap spriteFull = Utility.GetSprite(gCurrSheet, oCrop);

                        if (chkFlip.Checked)
                            spriteFull.RotateFlip(RotateFlipType.RotateNoneFlipX);

                        gCurrSprite = spriteFull;

                        gCurrHolds = new Dictionary<int, List<fpxHold>>(oTile.HoldGroups);
                    }
                }

                ediMap.Invalidate();
            }
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

                                        if (gCurrSprite != null)
                                            gCurrSprite.Dispose();

                                        gCurrSprite = spriteFull;

                                        gCurrHolds = new Dictionary<int, List<fpxHold>>(oObj.HoldGroups);
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
                //Flipped = false;

                if (gCurrSprite != null)
                    gCurrSprite.Dispose();

                gCurrHolds.Clear();

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
                            //Flipped = true;
                        }

                        gCurrHolds = new Dictionary<int, List<fpxHold>>(oObject.HoldGroups);
                    }
                }

                ediMap.Invalidate();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            ediMap.Invalidate();
        }
        private void ediMap_MouseClick(object sender, MouseEventArgs e)
        {
            if (tsbAdd.Checked)
            {
                int oLayer = GetLayerIndex();

                if (tsbObject.Checked && dgvObjects.SelectedRows.Count > 0)
                {
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
                   
                    foreach (fpxSprite oSprMapping in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites)
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
                        gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites.Add(oSprMapping);
                    }

                    Dictionary<int, List<fpxHold>> oHolds = new Dictionary<int, List<fpxHold>>();

                    for (int i = 0; i < gObjects[dgvObjects.SelectedRows[0].Index].HoldGroups.Count; i++)
                    {
                        List<fpxHold> newGroup = new List<fpxHold>();

                        List<fpxHold> holdGroup = gObjects[dgvObjects.SelectedRows[0].Index].HoldGroups[i];
                        foreach (fpxHold hold in holdGroup)
                        {
                            fpxHold newHold = new fpxHold
                            {
                                x1 = hold.x1,
                                x2 = hold.x2,
                                y1 = hold.y1,
                                y2 = hold.y2,
                                next = hold.next,
                                prev = hold.prev,
                                previd = hold.previd,
                                nextid = hold.nextid,
                                force = hold.force,
                                cantDrop = hold.cantDrop,
                                cantMove = hold.cantMove,
                                cantPass = hold.cantPass,
                                gid = hold.gid,
                                id = hold.id,
                                lid = hold.lid,
                                type = hold.type
                            };

                            if (chkFlip.Checked)
                            {
                                newHold.x1 = (gCurrSprite.Width) - newHold.x1;
                                newHold.x2 = (gCurrSprite.Width) - newHold.x2;
                            }

                            newGroup.Add(newHold);
                        }

                        oHolds.Add(i, newGroup);
                    }

                    oObj.HoldGroups = oHolds;

                    gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapObjects.Add(oObjMapping);
                }
                else if(tsbTile.Checked && dgvTiles.SelectedRows.Count > 0)
                {
                    int iCurrSpriteWidth = Utility.GetGameResolutionX(gCurrSprite.Width);
                    int iCurrSpriteHeight = Utility.GetGameResolutionX(gCurrSprite.Height);

                    fpxMapTile oTileMapping = new fpxMapTile();
                    oTileMapping.MapCoor = new Point(iPointX - iCurrSpriteWidth / 2, iPointY - iCurrSpriteHeight / 2);
                    fpxTile oTile = new fpxTile
                    {
                        Name = gTiles[dgvTiles.SelectedRows[0].Index].Name,
                        height = gTiles[dgvTiles.SelectedRows[0].Index].height,
                        width = gTiles[dgvTiles.SelectedRows[0].Index].width,
                        x = gTiles[dgvTiles.SelectedRows[0].Index].x,
                        y = gTiles[dgvTiles.SelectedRows[0].Index].y,
                        SpriteSheet = gTiles[dgvTiles.SelectedRows[0].Index].SpriteSheet
                    };         

                    string sFlip = "";

                    if (chkFlip.Checked)
                        sFlip += "Flipped";

                    foreach (fpxSprite oSprMapping in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites)
                    {
                        if (oSprMapping.SpriteSheet.Equals(cmbTileSheets.SelectedItem) && oSprMapping.SpriteName.Equals(dgvTiles.SelectedRows[0].Cells[1].Value + sFlip))
                        {
                            if(oSprMapping.Sprite == null)
                            {
                                Rectangle oCrop = new Rectangle(oTile.x, oTile.y, oTile.width, oTile.height);
                                Bitmap newSprite = Utility.GetSprite(new Bitmap(Path.Combine(Utility.TILES_PATH, oSprMapping.SpriteSheet)), oCrop);

                                newSprite.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                oSprMapping.Sprite = newSprite;
                            }

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
                        gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapSprites.Add(oSprMapping);
                    }

                   
                    Dictionary<int, List<fpxHold>> oHolds = new Dictionary<int, List<fpxHold>>();

                    for (int i = 0; i < gTiles[dgvTiles.SelectedRows[0].Index].HoldGroups.Count; i++)
                    {
                        List<fpxHold> newGroup = new List<fpxHold>();

                        List<fpxHold> holdGroup = gTiles[dgvTiles.SelectedRows[0].Index].HoldGroups[i];
                        foreach (fpxHold hold in holdGroup)
                        {
                            fpxHold newHold = new fpxHold {
                                x1 = hold.x1,
                                x2 = hold.x2,
                                y1 = hold.y1,
                                y2 = hold.y2,
                                next = hold.next,
                                prev = hold.prev,
                                previd = hold.previd,
                                nextid = hold.nextid,
                                force = hold.force,
                                cantDrop = hold.cantDrop,
                                cantMove = hold.cantMove,
                                cantPass = hold.cantPass,
                                gid = hold.gid,
                                id = hold.id,
                                lid = hold.lid,
                                type = hold.type
                            };
                            
                            if (chkFlip.Checked)
                            {
                                newHold.x1 = (gCurrSprite.Width) - newHold.x1;
                                newHold.x2 = (gCurrSprite.Width) - newHold.x2;
                            }

                            newGroup.Add(newHold);
                        }

                        oHolds.Add(i, newGroup);
                    }

                    oTile.HoldGroups = oHolds;
                    

                    oTileMapping.Tile = oTile;
                    oTileMapping.Flipped = chkFlip.Checked;

                    gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapTiles.Add(oTileMapping);
                }
                else if(tsbPortal.Checked)
                {
                    fpxMapPortal oPortal = new fpxMapPortal();
                    oPortal.ID = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals.Count;
                    oPortal.Type = "portal";
                    oPortal.MapCoor = new Point(iPointX - gCurrSprite.Width / 2, iPointY - gCurrSprite.Height + 20);
                    oPortal.RegionID = gRegions[cmbRegion.SelectedIndex].ID;

                    gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals.Add(oPortal);
                }
                else if(tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked)
                {
                   if (!gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapHolds.ContainsKey(iHoldGroup))
                        gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapHolds.Add(iHoldGroup, new List<fpxHold>());

                    List<fpxHold> holdList = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapHolds[iHoldGroup];

                    fpxHold prevH = holdList.ElementAtOrDefault(holdList.Count - 1);

                    fpxHold hold = new fpxHold();
                    hold.id = holdList.Count;
                    hold.gid = iHoldGroup;
                    hold.x1 = e.X;
                    hold.y1 = e.Y;

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

                    gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[oLayer].MapHolds[iHoldGroup] = holdList;
                }
            }
            else
            {
                if(tsbObject.Checked)
                {
                    foreach(fpxMapTile oTile in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles)
                    {
                        if(iPointX > oTile.MapCoor.X && iPointX < oTile.MapCoor.X + oTile.Tile.width
                             && iPointY > oTile.MapCoor.Y && iPointY < oTile.MapCoor.Y + oTile.Tile.height)
                        {
                            tsbList.SelectedIndex = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles.IndexOf(oTile);
                        }
                    }
                }
                else if(tsbTile.Checked)
                {
                    foreach (fpxMapObject oObject in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapObjects)
                    {
                        if (iPointX > oObject.MapCoor.X && iPointX < oObject.MapCoor.X + oObject.Object.width
                             && iPointY > oObject.MapCoor.Y && iPointY < oObject.MapCoor.Y + oObject.Object.height)
                        {
                            tsbList.SelectedIndex = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapObjects.IndexOf(oObject);
                        }
                    }
                }
            }
        }
        private void ediMap_MouseMove(object sender, MouseEventArgs e)
        {
            iPointX = e.X;
            iPointY = e.Y;

            ediMap.Invalidate();
        }
        int GetSceneryX(decimal parallaxX, int iSpriteX)
        {
            double dScrollXVal = pnlEditor.HorizontalScroll.Value;
            double dScrollXMax = pnlEditor.HorizontalScroll.Maximum;

            int iFinal = (int)(dScrollXVal * (double)parallaxX);

            return iFinal;
        }
        int GetSceneryY(decimal parallaxY, int iSpriteHeight)
        {
            double dScrollYVal = pnlEditor.VerticalScroll.Value;
            double dScrollYMax = pnlEditor.VerticalScroll.Maximum;

            double dScrollPerc = dScrollYVal / dScrollYMax;
            double dScrollWidth = pnlEditor.VerticalScroll.Maximum - pnlEditor.VerticalScroll.LargeChange;
            double dScrollPos = ediMap.Height * dScrollPerc;

            double dPositionPerc = Math.Min(dScrollPos / dScrollWidth, 1);

            double iPosition = (ediMap.Height - iSpriteHeight);
            double dOffset =  ((iPosition * (1 - dPositionPerc)) *  (double)(1 - parallaxY));

            int iFinal = (int)(iPosition - dOffset);

            return iFinal;
        }
        private void ediMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(ediMap.BackColor);

            if(cmbRegion.SelectedIndex != -1 && cmbMaps.SelectedIndex != -1)
            {
                int iMapWidth = Utility.GetGameResolutionX(gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapWidth);
                int iMapHeight = Utility.GetGameResolutionY(gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapHeight);

                ediMap.Size = new Size(iMapWidth, iMapHeight);

                foreach (fpxMapLayer oLayer in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers)
                {
                    if (!chkHideForward.Checked || oLayer.LayerID <= int.Parse(cmbLayer.SelectedItem.ToString()))
                    {
                        if (SceneryView)
                        {
                            if (oLayer.MapScenery.Sprite != null)
                            {
                                fpxMapScenery os = oLayer.MapScenery;

                                int iTileCountX = 0;
                                int iTileCountY = 0;

                                if (oLayer.TileX)
                                    iTileCountX = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapWidth / os.Sprite.Width;
                                if(oLayer.TileY)
                                    iTileCountY = gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapHeight / os.Sprite.Height;

                                for (int iY = 0; iY <= iTileCountY; iY++)
                                {
                                    for(int iX = 0; iX <= iTileCountX; iX++)
                                    {
                                        int iWidth = os.Sprite.Width;
                                        int iHeight = os.Sprite.Height;
                                        int iTrueX = GetSceneryX(oLayer.ParallaxX, iWidth) + (iWidth * iX);
                                        int iTrueY = GetSceneryY(oLayer.ParallaxY, iHeight) - (iHeight * iY);
                                        

                                        Rectangle oMapRect = new Rectangle(iTrueX, iTrueY, iWidth, iHeight);
                                        g.DrawImage(os.Sprite, oMapRect);
                                    }
                                }
                            }
                        }

                        if (TileView)
                        {
                            for (int i = 0; i < oLayer.MapTiles.Count; i++)
                            {
                                fpxMapTile ot = oLayer.MapTiles[i];

                                int iWidth = Utility.GetGameResolutionX(ot.Tile.width);
                                int iHeight = Utility.GetGameResolutionY(ot.Tile.height);
                                int iTrueX = Utility.GetGameResolutionX(ot.MapCoor.X);
                                int iTrueY = Utility.GetGameResolutionY(ot.MapCoor.Y);

                                Rectangle oMapRect = new Rectangle(iTrueX, iTrueY, iWidth, iHeight);
                                g.DrawImage(ot.Sprite, oMapRect);


                                if (tsbTile.Checked && tsbList.SelectedIndex == i && (oLayer.LayerID == int.Parse(cmbLayer.SelectedItem.ToString())))
                                {
                                    Pen pen = new Pen(Color.Black, 3);

                                    Rectangle oSelectRect = new Rectangle(iTrueX, iTrueY, iWidth, iHeight);
                                    g.DrawRectangle(pen, oSelectRect);
                                }
                            }
                        }

                        if (ObjectView)
                        {
                            for (int i = 0; i < oLayer.MapObjects.Count; i++)
                            {
                                fpxMapObject o = oLayer.MapObjects[i];

                                int iWidth = Utility.GetGameResolutionX(o.Object.width);
                                int iHeight = Utility.GetGameResolutionY(o.Object.height);
                                int iTrueX = Utility.GetGameResolutionX(o.MapCoor.X);
                                int iTrueY = Utility.GetGameResolutionY(o.MapCoor.Y);

                                Rectangle oMapRect = new Rectangle(iTrueX, iTrueY, iWidth, iHeight);
                                g.DrawImage(o.Sprite, oMapRect);

                                if (tsbObject.Checked && tsbList.SelectedIndex == i && (oLayer.LayerID == int.Parse(cmbLayer.SelectedItem.ToString())))
                                {
                                    Pen pen = new Pen(Color.Black, 3);

                                    Rectangle oSelectRect = new Rectangle(iTrueX, iTrueY, iWidth, iHeight);
                                    g.DrawRectangle(pen, oSelectRect);
                                }
                            }
                        }

                        if (HoldView)
                        {
                            if (TileView)
                            {
                                foreach (fpxMapTile ot in oLayer.MapTiles)
                                {
                                    for (int i = 0; i < ot.Tile.HoldGroups.Count; i++)
                                    {
                                        List<fpxHold> holdGroup = ot.Tile.HoldGroups[i];
                                        foreach (fpxHold hold in holdGroup)
                                        {
                                            Pen pen = new Pen(Color.Black, 3);
                                            // Create pen.
                                            if (hold.type.Equals("foot"))
                                                pen.Color = Color.Red;
                                            else if (hold.type.Equals("climb"))
                                                pen.Color = Color.Green;
                                            else if (hold.type.Equals("seat"))
                                                pen.Color = Color.Orange;
                                            // Create location and size of rectangle.
                                            int x = Utility.GetGameResolutionX(hold.x1 + ot.MapCoor.X);
                                            int y = Utility.GetGameResolutionY(hold.y1 + ot.MapCoor.Y);
                                            int width = 5;
                                            int height = 5;

                                            g.DrawRectangle(pen, x, y, width, height);

                                            if (hold.next != null)
                                            {
                                                int x2 = Utility.GetGameResolutionX(hold.x2 + ot.MapCoor.X);
                                                int y2 = Utility.GetGameResolutionY(hold.y2 + ot.MapCoor.Y);
                                                g.DrawLine(pen, new Point(x, y), new Point(x2, y2));
                                            }                                               
                                        }
                                    }
                                }
                            }

                            if (ObjectView)
                            {
                                foreach (fpxMapObject o in oLayer.MapObjects)
                                {
                                    for (int i = 0; i < o.Object.HoldGroups.Count; i++)
                                    {
                                        List<fpxHold> holdGroup = o.Object.HoldGroups[i];
                                        foreach (fpxHold hold in holdGroup)
                                        {
                                            Pen pen = new Pen(Color.Black, 3);
                                            // Create pen.
                                            if (hold.type.Equals("foot"))
                                                pen.Color = Color.Red;
                                            else if (hold.type.Equals("climb"))
                                                pen.Color = Color.Green;
                                            else if (hold.type.Equals("seat"))
                                                pen.Color = Color.Orange;
                                            // Create location and size of rectangle.
                                            int x = Utility.GetGameResolutionX(hold.x1 + o.MapCoor.X);
                                            int y = Utility.GetGameResolutionY(hold.y1 + o.MapCoor.Y);

                                            if (o.Flipped)
                                            {
                                                x = Utility.GetGameResolutionX(o.Object.width - hold.x1 + o.MapCoor.X);
                                            }

                                            int x2 = Utility.GetGameResolutionX(hold.x2 + o.MapCoor.X);
                                            int y2 = Utility.GetGameResolutionY(hold.y2 + o.MapCoor.Y);

                                            if (o.Flipped)
                                            {
                                                x2 = Utility.GetGameResolutionX(o.Object.width - hold.x2 + o.MapCoor.X);
                                            }

                                            int width = 5;
                                            int height = 5;

                                            g.DrawRectangle(pen, x, y, width, height);

                                            if (hold.next != null)
                                                g.DrawLine(pen, new Point(x, y), new Point(x2, y2));
                                        }
                                    }
                                }
                            }

                            foreach (List<fpxHold> oHoldGroup in oLayer.MapHolds.Values)
                            {
                                foreach (fpxHold hold in oHoldGroup)
                                {
                                    Pen pen = new Pen(Color.Black, 3);
                                    // Create pen.
                                    if (hold.type.Equals("foot"))
                                        pen.Color = Color.Red;
                                    else if (hold.type.Equals("climb"))
                                        pen.Color = Color.Green;
                                    else if (hold.type.Equals("seat"))
                                        pen.Color = Color.Orange;
                                    // Create location and size of rectangle.
                                    int x = Utility.GetGameResolutionX(hold.x1);
                                    int y = Utility.GetGameResolutionY(hold.y1);
                                    int width = 5;
                                    int height = 5;

                                    g.DrawRectangle(pen, x, y, width, height);

                                    if (hold.next != null)
                                    {
                                        int x2 = Utility.GetGameResolutionX(hold.x2);
                                        int y2 = Utility.GetGameResolutionY(hold.y2);
                                        g.DrawLine(pen, new Point(x, y), new Point(x2, y2));
                                    }                             
                                }

                            }
                        }
                    }
                }

                foreach (fpxMapPortal oPortal in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapPortals)
                {
                    Bitmap portal = new Bitmap(Path.Combine(Utility.GLOBAL_PATH, oPortal.Type + Utility.PNG_EXT));

                    int iWidth = Utility.GetGameResolutionX(oPortal.MapCoor.X);
                    int iHeight = Utility.GetGameResolutionY(oPortal.MapCoor.Y);
                    int iTrueX = Utility.GetGameResolutionX(portal.Width);
                    int iTrueY = Utility.GetGameResolutionY(portal.Height);

                    Rectangle rectObj = new Rectangle(iTrueX, iTrueY, iWidth, iHeight);

                    if (rectObj.Width < 5)
                        rectObj.Width = 5;

                    if (rectObj.Height < 5)
                        rectObj.Height = 5;

                    g.DrawImage(portal, rectObj);

                    //if (tsbPortal.Checked && tsbList.SelectedItem.Equals(oPortal.Type + oPortal.ID))
                    //{
                    //    Pen pen = new Pen(Color.Black, 3);

                    //    Rectangle oSelectRect = new Rectangle(oPortal.MapCoor.X, oPortal.MapCoor.Y, portal.Width, portal.Height);
                    //    g.DrawRectangle(pen, oSelectRect);
                    //}

                    portal.Dispose();
                }
            }
         
            if (tsbAdd.Checked)
            {
                if (tsbObject.Checked || tsbTile.Checked)
                {
                    if (gCurrSprite != null)
                    {
                        int iLocX = iPointX;
                        int iLocY = iPointY;

                        if(tsbTile.Checked && cmbRegion.SelectedIndex != -1 && cmbMaps.SelectedIndex != -1)
                        {
                            foreach( fpxMapTile oTile in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].MapTiles)
                            {
                                if(chkSnapX.Checked)
                                {
                                    int iTileX = Utility.GetGameResolutionX(oTile.MapCoor.X);
                                    int iTileY = Utility.GetGameResolutionY(oTile.MapCoor.Y);
                                    int iTileWidth = Utility.GetGameResolutionX(oTile.Sprite.Width);
                                    int iTileHeight = Utility.GetGameResolutionY(oTile.Sprite.Height);

                                    int iCurrTileWidth = Utility.GetGameResolutionX(gCurrSprite.Width);
                                    int iCurrTileHeight = Utility.GetGameResolutionY(gCurrSprite.Height);

                                    if (iTileY < iLocY && iTileY + iTileHeight > iLocY)
                                    {
                                        if ((iLocX - iTileWidth / 2 < iTileX + iTileWidth) && iLocX > iTileX + iTileWidth / 2)
                                        {
                                            iLocX = iTileX + iTileWidth + iCurrTileWidth / 2;
                                            iLocY = iTileY + iCurrTileHeight / 2;
                                        }
                                        else if (iLocX + iTileWidth / 2 > iTileX && iLocX - iTileWidth / 2 < iTileX)
                                        {
                                            iLocX = iTileX - iCurrTileWidth / 2;
                                            iLocY = iTileY + iCurrTileHeight / 2;
                                        }
                                    }
                                }

                                if(chkSnapY.Checked)
                                {
                                    int iTileX = Utility.GetGameResolutionX(oTile.MapCoor.X);
                                    int iTileY = Utility.GetGameResolutionY(oTile.MapCoor.Y);
                                    int iTileWidth = Utility.GetGameResolutionX(oTile.Sprite.Width);
                                    int iTileHeight = Utility.GetGameResolutionY(oTile.Sprite.Height);

                                    int iCurrTileWidth = Utility.GetGameResolutionX(gCurrSprite.Width);
                                    int iCurrTileHeight = Utility.GetGameResolutionY(gCurrSprite.Height);

                                    if (iTileX < iLocX && iTileX + iTileWidth > iLocX)
                                    {
                                        if ((iLocY - iTileHeight / 2 < iTileY + iTileHeight) && iLocY > iTileY + iTileHeight / 2)
                                        {
                                            iLocX = iTileX + iCurrTileWidth / 2;
                                            iLocY = iTileY + iTileHeight + iCurrTileHeight / 2;
                                        }
                                        else if (iLocY + iTileHeight / 2 > iTileY && iLocY - iTileHeight / 2 < iTileY)
                                        {
                                            iLocX = iTileX + iCurrTileWidth / 2;
                                            iLocY = iTileY - iCurrTileHeight / 2;
                                        }
                                    }
                                }
                            }

                            iPointX = iLocX;
                            iPointY = iLocY;
                        }

                        int iWidth = Utility.GetGameResolutionX(gCurrSprite.Width);
                        int iHeight = Utility.GetGameResolutionY(gCurrSprite.Height);
                        int iX = iLocX;
                        int iY = iLocY;


                        Rectangle rectObj = new Rectangle(iX - iWidth / 2, iY - iHeight / 2, iWidth, iHeight);

                        if (rectObj.Width < 5)
                            rectObj.Width = 5;

                        if (rectObj.Height < 5)
                            rectObj.Height = 5;

                        g.DrawImage(gCurrSprite, rectObj);

                        for (int i = 0; i < gCurrHolds.Count; i++)
                        {
                            List<fpxHold> holdGroup = gCurrHolds[i];
                            foreach (fpxHold hold in holdGroup)
                            {
                                Pen pen = new Pen(Color.Black, 3);
                                // Create pen.
                                if (hold.type.Equals("foot"))
                                    pen.Color = Color.Red;
                                else if (hold.type.Equals("climb"))
                                    pen.Color = Color.Green;
                                else if (hold.type.Equals("seat"))
                                    pen.Color = Color.Orange;
                                // Create location and size of rectangle.


                                int x = Utility.GetGameResolutionX(hold.x1);
                                x += (iX - iWidth / 2);
                                int y = Utility.GetGameResolutionY(hold.y1);
                                y += (iY- iHeight / 2);

                                if (chkFlip.Checked)
                                {
                                    x = iWidth - Utility.GetGameResolutionX(hold.x1);
                                    x += (iX - iWidth / 2);
                                }

                                int width = 5;
                                int height = 5;

                                g.DrawRectangle(pen, x, y, width, height);

                                if (hold.next != null)
                                {
                                    int x2 = Utility.GetGameResolutionX(hold.x2);
                                    x2 += (iX - iWidth / 2);
                                    int y2 = Utility.GetGameResolutionY(hold.y2);
                                    y2 += (iY - iHeight / 2);

                                    if (chkFlip.Checked)
                                    {
                                        x2 = iWidth - Utility.GetGameResolutionX(hold.x2);
                                        x2 += (iX - iWidth / 2);
                                    }

                                    g.DrawLine(pen, new Point(x, y), new Point(x2,y2));
                                }                              
                            }
                        }
                    }
                }
                else if(tsbPortal.Checked)
                {
                    Rectangle rectObj = new Rectangle(iPointX - gCurrSprite.Width / 2, iPointY - gCurrSprite.Height + 20, gCurrSprite.Width, gCurrSprite.Height);

                    if (rectObj.Width < 5)
                        rectObj.Width = 5;

                    if (rectObj.Height < 5)
                        rectObj.Height = 5;

                    g.DrawImage(gCurrSprite, rectObj);
                }
                else if(tsbFoothold.Checked || tsbClimbhold.Checked || tsbSeathold.Checked)
                {
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
                    int x = iPointX;
                    int y = iPointY;
                    int width = Utility.GRAPHICS_POINT_RECT;
                    int height = Utility.GRAPHICS_POINT_RECT;

                    g.DrawRectangle(pen, x - width / 2, y - height / 2, width, height);
                }
            }
        }
        #endregion

        #region Main Methods
        public void Prepare()
        {
            Timer timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 100;
            timer.Tick += Timer_Tick;

            string sXml = EditorManager.GetMapCollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gMapDoc = xLoad;

            tsbMap.Checked = true;
            ediMap.Invalidate();

            PrepareControls();

            PrepareScenery();

            ManageControls();
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
        public void Reload()
        {
            ClearMaps();

            string sXml = EditorManager.GetMapCollection().OuterXml;

            XmlDocument xLoad = new XmlDocument();
            xLoad.LoadXml(sXml);
            gMapDoc = xLoad;

            XmlNode oMaps = gMapDoc.SelectSingleNode("//fpxMaps");
            foreach(XmlNode oRegion in oMaps.ChildNodes)
            {
                fpxRegion oReg = new fpxRegion();
                oReg.ID = int.Parse(oRegion.Attributes["ID"].Value);
                oReg.Name = oRegion.Attributes["Name"].Value;

                foreach (XmlNode oMap in oRegion.ChildNodes)
                {
                    fpxMap oM = new fpxMap();
                    oM.MapID = int.Parse(oMap.Attributes["ID"].Value);
                    oM.RegionID = int.Parse(oMap.Attributes["RID"].Value);
                    oM.MapName = oMap.Attributes["Name"].Value;
                    oM.MapWidth = int.Parse(oMap.Attributes["width"].Value);
                    oM.MapHeight = int.Parse(oMap.Attributes["height"].Value);
                    oM.MapBoundR = int.Parse(oMap.Attributes["boundR"].Value);
                    oM.MapBoundL = int.Parse(oMap.Attributes["boundL"].Value);
                    oM.MapBoundT = int.Parse(oMap.Attributes["boundT"].Value);
                    oM.MapBoundB = int.Parse(oMap.Attributes["boundB"].Value);
                    oM.BackgroundMusic = oMap.Attributes["BackgroundMusic"].Value;

                    foreach (XmlNode oLayer in oMap.FirstChild.ChildNodes)
                    {
                        fpxMapLayer oL = new fpxMapLayer();
                        oL.LayerID = int.Parse(oLayer.Attributes["ID"].Value);
                        oL.ParallaxX = decimal.Parse(oLayer.Attributes["ParallaxX"].Value);
                        oL.ParallaxY = decimal.Parse(oLayer.Attributes["ParallaxY"].Value);
                        oL.ScrollX = decimal.Parse(oLayer.Attributes["ScrollX"].Value);
                        oL.ScrollY = decimal.Parse(oLayer.Attributes["ScrollY"].Value);
                        oL.TileX = bool.Parse(oLayer.Attributes["TileX"].Value);
                        oL.TileY = bool.Parse(oLayer.Attributes["TileY"].Value);

                        foreach (XmlNode child in oLayer.ChildNodes)
                        {
                            if (child.Name == "Sprites")
                            {
                                foreach (XmlNode oSprite in child.ChildNodes)
                                {

                                    fpxSprite oS = new fpxSprite();
                                    oS.SpriteName = oSprite.Attributes["Name"].Value;
                                    oS.SpriteSheet = oSprite.Attributes["SpriteSheet"].Value;

                                    oL.MapSprites.Add(oS);
                                }
                            }
                            else if (child.Name == "Scenery")
                            {
                                XmlNode oScenery = child;

                                if (!string.IsNullOrWhiteSpace(oScenery.Attributes["Sprite"].Value))
                                {
                                    fpxMapScenery oMapScenery = new fpxMapScenery();
                                    oMapScenery.SpriteName = oScenery.Attributes["Sprite"].Value;
                                    oMapScenery.Flipped = bool.Parse(oScenery.Attributes["Flipped"].Value);

                                    Bitmap sheet = new Bitmap(Path.Combine(Utility.SCENERY_PATH, oMapScenery.SpriteName));
                                    Point oSpriteLocation = new Point(0, 0);
                                    Size oSpriteSize = new Size(sheet.Width, sheet.Height);
                                    Rectangle oCrop = new Rectangle(oSpriteLocation, oSpriteSize);

                                    string sFlip = "";
                                    if (bool.Parse(oScenery.Attributes["Flipped"].Value))
                                        sFlip += "Flipped";

                                    Bitmap sprite = (Bitmap)Utility.GetSprite(sheet, oCrop).Clone();

                                    if (oMapScenery.Flipped)
                                        sprite.RotateFlip(RotateFlipType.RotateNoneFlipX);

                                    foreach (fpxSprite oSprMapping in oL.MapSprites)
                                    {
                                        if (oSprMapping.SpriteSheet.Equals(oMapScenery.SpriteName) && oSprMapping.SpriteName.Equals(oMapScenery.SpriteName))
                                        {
                                            if (oSprMapping.Sprite == null)
                                                oSprMapping.Sprite = sprite;

                                            oSprMapping.UsageCount++;
                                        }
                                    }

                                    oMapScenery.Sprite = sprite;
                                    oL.MapScenery = oMapScenery;
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

                                    foreach (fpxSprite oSprMapping in oL.MapSprites)
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
                                    o.CannotDrop = bool.Parse(oObject.Attributes["CannotDrop"].Value);

                                    oL.MapObjects.Add(o);
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

                                    foreach (XmlNode oHoldGroup in oTile.SelectSingleNode("Holds").ChildNodes)
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

                                    foreach (fpxSprite oSprMapping in oL.MapSprites)
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
                                    oT.CannotDrop = bool.Parse(oTile.Attributes["CannotDrop"].Value);

                                    oL.MapTiles.Add(oT);
                                }
                            }
                            else if (child.Name == "Holds")
                            {
                                foreach (XmlNode oHoldGroup in child.ChildNodes)
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

                                    oL.MapHolds.Add(int.Parse(oHoldGroup.Attributes["id"].Value), oHG);
                                }
                            }
                        }

                        oM.MapLayers.Add(oL);
                    }

                    foreach (XmlNode oPortal in oMap.LastChild.ChildNodes)
                    {
                        fpxMapPortal oP = new fpxMapPortal();
                        oP.ID = int.Parse(oPortal.Attributes["ID"].Value);
                        oP.Type = oPortal.Attributes["type"].Value;
                        oP.RegionID = int.Parse(oPortal.Attributes["regionID"].Value);
                        oP.MapID = int.Parse(oPortal.Attributes["mapID"].Value);
                        oP.TargetID = int.Parse(oPortal.Attributes["targetID"].Value);
                        oP.MapCoor = new Point(int.Parse(oPortal.Attributes["mapCoorX"].Value), int.Parse(oPortal.Attributes["mapCoorY"].Value));

                        oM.MapPortals.Add(oP);
                    }

                    oReg.Maps.Add(oM);
                }

                gRegions.Add(oReg);
            }
           
            foreach(fpxRegion oReg in gRegions)
            {
                cmbRegion.Items.Add(oReg.Name);
            }

            if (cmbRegion.Items.Count > 0)
                cmbRegion.SelectedIndex = 0;

            if (cmbMaps.Items.Count > 0)
            {
                cmbMaps.SelectedIndex = 0;
                ValidateMaps();
            }
        }
        
        public void ResetMode()
        {
            tsbMap.Checked = false;
            tsbFoothold.Checked = false;
            tsbClimbhold.Checked = false;
            tsbSeathold.Checked = false;
            tsbScenery.Checked = false;
            tsbTile.Checked = false;
            tsbObject.Checked = false;
            tsbPortal.Checked = false;
            tsbAdd.Checked = false;
        }
        public void ValidateSave()
        {
            bool bDirtyCheck = false;

            if (!gMapDoc.OuterXml.Equals(EditorManager.GetMapCollection().OuterXml))
                bDirtyCheck = true;

            Dirty = bDirtyCheck;
        }
        public void ValidateMaps()
        {

            XmlNode oMapRoot = gMapDoc.CreateElement("fpxMaps");

            foreach (fpxRegion oRegion in gRegions)
            {
                XmlAttribute oRegionID = gMapDoc.CreateAttribute("ID");
                oRegionID.Value = oRegion.ID.ToString();
                XmlAttribute oRegionName = gMapDoc.CreateAttribute("Name");
                oRegionName.Value = oRegion.Name.ToString();

                XmlNode oRegionN = gMapDoc.CreateNode("element", "Region", "");
                oRegionN.Attributes.Append(oRegionID);
                oRegionN.Attributes.Append(oRegionName);

                foreach (fpxMap oMap in oRegion.Maps)
                {
                    XmlAttribute oMapID = gMapDoc.CreateAttribute("ID");
                    oMapID.Value = oMap.MapID.ToString();
                    XmlAttribute oMapRID = gMapDoc.CreateAttribute("RID");
                    oMapRID.Value = oMap.RegionID.ToString();
                    XmlAttribute oMapName = gMapDoc.CreateAttribute("Name");
                    oMapName.Value = oMap.MapName;
                    XmlAttribute oMapWidth = gMapDoc.CreateAttribute("width");
                    oMapWidth.Value = oMap.MapWidth.ToString();
                    XmlAttribute oMapHeight = gMapDoc.CreateAttribute("height");
                    oMapHeight.Value = oMap.MapHeight.ToString();
                    XmlAttribute oMapBoundR = gMapDoc.CreateAttribute("boundR");
                    oMapBoundR.Value = oMap.MapBoundR.ToString();
                    XmlAttribute oMapBoundL = gMapDoc.CreateAttribute("boundL");
                    oMapBoundL.Value = oMap.MapBoundL.ToString();
                    XmlAttribute oMapBoundT = gMapDoc.CreateAttribute("boundT");
                    oMapBoundT.Value = oMap.MapBoundT.ToString();
                    XmlAttribute oMapBoundB = gMapDoc.CreateAttribute("boundB");
                    oMapBoundB.Value = oMap.MapBoundB.ToString();
                    XmlAttribute oMapBGM= gMapDoc.CreateAttribute("BackgroundMusic");
                    oMapBGM.Value = oMap.BackgroundMusic;

                    XmlNode oMapN = gMapDoc.CreateNode("element", "Map", "");
                    oMapN.Attributes.Append(oMapID);
                    oMapN.Attributes.Append(oMapRID);
                    oMapN.Attributes.Append(oMapName);
                    oMapN.Attributes.Append(oMapWidth);
                    oMapN.Attributes.Append(oMapHeight);
                    oMapN.Attributes.Append(oMapBoundR);
                    oMapN.Attributes.Append(oMapBoundL);
                    oMapN.Attributes.Append(oMapBoundT);
                    oMapN.Attributes.Append(oMapBoundB);
                    oMapN.Attributes.Append(oMapBGM);

                    XmlNode oPortals = gMapDoc.CreateNode("element", "Portals", "");

                    foreach (fpxMapPortal oPortal in oMap.MapPortals)
                    {
                        XmlAttribute oPortalID = gMapDoc.CreateAttribute("ID");
                        oPortalID.Value = oPortal.ID.ToString();
                        XmlAttribute oPortalType = gMapDoc.CreateAttribute("type");
                        oPortalType.Value = oPortal.Type;
                        XmlAttribute oPortalRID = gMapDoc.CreateAttribute("regionID");
                        oPortalRID.Value = oPortal.RegionID.ToString();
                        XmlAttribute oPortalMapID = gMapDoc.CreateAttribute("mapID");
                        oPortalMapID.Value = oPortal.MapID.ToString();
                        XmlAttribute oPortalTargetID = gMapDoc.CreateAttribute("targetID");
                        oPortalTargetID.Value = oPortal.TargetID.ToString();
                        XmlAttribute oPortalMapCoorX = gMapDoc.CreateAttribute("mapCoorX");
                        oPortalMapCoorX.Value = oPortal.MapCoor.X.ToString();
                        XmlAttribute oPortalMapCoorY = gMapDoc.CreateAttribute("mapCoorY");
                        oPortalMapCoorY.Value = oPortal.MapCoor.Y.ToString();

                        XmlNode oPortalN = gMapDoc.CreateNode("element", "Portal", "");
                        oPortalN.Attributes.Append(oPortalID);
                        oPortalN.Attributes.Append(oPortalType);
                        oPortalN.Attributes.Append(oPortalRID);
                        oPortalN.Attributes.Append(oPortalMapID);
                        oPortalN.Attributes.Append(oPortalTargetID);
                        oPortalN.Attributes.Append(oPortalMapCoorX);
                        oPortalN.Attributes.Append(oPortalMapCoorY);

                        oPortals.AppendChild(oPortalN);
                    }

                    XmlNode oLayers = gMapDoc.CreateNode("element", "Layers", "");

                    for (int i = 0; i < oMap.MapLayers.Count; i++)
                    {
                        XmlNode oScenery = gMapDoc.CreateNode("element", "Scenery", "");
                        XmlNode oSprites = gMapDoc.CreateNode("element", "Sprites", "");
                        XmlNode oObjects = gMapDoc.CreateNode("element", "Objects", "");
                        XmlNode oTiles = gMapDoc.CreateNode("element", "Tiles", "");
                        XmlNode oHoldsG = gMapDoc.CreateNode("element", "Holds", "");

                        int iTrueIndex = i - 8;

                        XmlNode oLayer = gMapDoc.CreateNode("element", "Layer", "");
                        XmlAttribute oLayerID = gMapDoc.CreateAttribute("ID");
                        oLayerID.Value = (iTrueIndex).ToString();
                        XmlAttribute oLayerParallaxX = gMapDoc.CreateAttribute("ParallaxX");
                        oLayerParallaxX.Value = oMap.MapLayers[i].ParallaxX.ToString();
                        XmlAttribute oLayerParallaxY = gMapDoc.CreateAttribute("ParallaxY");
                        oLayerParallaxY.Value = oMap.MapLayers[i].ParallaxY.ToString();
                        XmlAttribute oLayerScrollX = gMapDoc.CreateAttribute("ScrollX");
                        oLayerScrollX.Value = oMap.MapLayers[i].ScrollX.ToString();
                        XmlAttribute oLayerScrollY = gMapDoc.CreateAttribute("ScrollY");
                        oLayerScrollY.Value = oMap.MapLayers[i].ScrollY.ToString();
                        XmlAttribute oLayerTileX = gMapDoc.CreateAttribute("TileX");
                        oLayerTileX.Value = oMap.MapLayers[i].TileX.ToString();
                        XmlAttribute oLayerTileY = gMapDoc.CreateAttribute("TileY");
                        oLayerTileY.Value = oMap.MapLayers[i].TileY.ToString();
                        oLayer.Attributes.Append(oLayerID);
                        oLayer.Attributes.Append(oLayerParallaxX);
                        oLayer.Attributes.Append(oLayerParallaxY);
                        oLayer.Attributes.Append(oLayerScrollX);
                        oLayer.Attributes.Append(oLayerScrollY);
                        oLayer.Attributes.Append(oLayerTileX);
                        oLayer.Attributes.Append(oLayerTileY);


                        fpxMapLayer oL = oMap.MapLayers[i];

                        XmlAttribute oScenerySprite = gMapDoc.CreateAttribute("Sprite");
                        oScenerySprite.Value = oL.MapScenery.SpriteName;
                        XmlAttribute oSceneryFlipped = gMapDoc.CreateAttribute("Flipped");
                        oSceneryFlipped.Value = oL.MapScenery.Flipped.ToString();
                        oScenery.Attributes.Append(oScenerySprite);
                        oScenery.Attributes.Append(oSceneryFlipped);

                        foreach (fpxSprite oSpr in oL.MapSprites)
                        {
                            XmlAttribute oSprName = gMapDoc.CreateAttribute("Name");
                            oSprName.Value = oSpr.SpriteName;
                            XmlAttribute oSprSheet = gMapDoc.CreateAttribute("SpriteSheet");
                            oSprSheet.Value = oSpr.SpriteSheet;

                            XmlNode oSprite = gMapDoc.CreateNode("element", "Sprite", "");
                            oSprite.Attributes.Append(oSprName);
                            oSprite.Attributes.Append(oSprSheet);


                            oSprites.AppendChild(oSprite);
                        }

                        foreach (fpxMapObject oObj in oL.MapObjects)
                        {
                            XmlAttribute oObjName = gMapDoc.CreateAttribute("Name");
                            oObjName.Value = oObj.Object.Name;
                            XmlAttribute oObjMapCoorX = gMapDoc.CreateAttribute("MapCoorX");
                            oObjMapCoorX.Value = oObj.MapCoor.X.ToString();
                            XmlAttribute oObjMapCoorY = gMapDoc.CreateAttribute("MapCoorY");
                            oObjMapCoorY.Value = oObj.MapCoor.Y.ToString();
                            XmlAttribute oObjFlipped = gMapDoc.CreateAttribute("Flipped");
                            oObjFlipped.Value = oObj.Flipped.ToString();
                            XmlAttribute oObjFlip = gMapDoc.CreateAttribute("Name");
                            oObjFlip.Value = oObj.Flipped.ToString();
                            XmlAttribute oObjSpriteName = gMapDoc.CreateAttribute("SpriteSheet");
                            oObjSpriteName.Value = oObj.Object.SpriteSheet;
                            XmlAttribute oX = gMapDoc.CreateAttribute("x");
                            oX.Value = oObj.Object.x.ToString();
                            XmlAttribute oY = gMapDoc.CreateAttribute("y");
                            oY.Value = oObj.Object.y.ToString();
                            XmlAttribute oObjWidth = gMapDoc.CreateAttribute("width");
                            oObjWidth.Value = oObj.Object.width.ToString();
                            XmlAttribute oObjHeight = gMapDoc.CreateAttribute("height");
                            oObjHeight.Value = oObj.Object.height.ToString();
                            XmlAttribute oAnimated = gMapDoc.CreateAttribute("Animated");
                            oAnimated.Value = oObj.Object.Animated.ToString();
                            XmlAttribute oCannotDrop = gMapDoc.CreateAttribute("CannotDrop");
                            oCannotDrop.Value = oObj.CannotDrop.ToString();

                            XmlNode oAnimations = gMapDoc.CreateNode("element", "Animations", "");
                            XmlAttribute oAnimationIndex = gMapDoc.CreateAttribute("Index");
                            oAnimationIndex.Value = oObj.Object.AnimationIndex.ToString();

                            if (oObj.Object.Animations.Count > 0)
                            {
                                for (int iA = 0; iA < oObj.Object.Animations.Count; iA++)
                                {
                                    if (iA == oObj.Object.AnimationIndex - 1)
                                    {
                                        fpxAnimation anim = oObj.Object.Animations[iA];

                                        XmlAttribute oAnimName = gMapDoc.CreateAttribute("Name");
                                        oAnimName.Value = anim.Name.ToString();
                                        XmlAttribute oReelHeight = gMapDoc.CreateAttribute("ReelHeight");
                                        oReelHeight.Value = anim.ReelHeight.ToString();
                                        XmlAttribute oReelIndex = gMapDoc.CreateAttribute("ReelIndex");
                                        oReelIndex.Value = anim.ReelIndex.ToString();
                                        XmlAttribute oFrameWidth = gMapDoc.CreateAttribute("FrameWidth");
                                        oFrameWidth.Value = anim.FrameWidth.ToString();
                                        XmlAttribute oTotalFrames = gMapDoc.CreateAttribute("TotalFrames");
                                        oTotalFrames.Value = anim.TotalFrames.ToString();
                                        XmlAttribute oFrameSpeed = gMapDoc.CreateAttribute("FrameSpeed");
                                        oFrameSpeed.Value = anim.FrameSpeed.ToString();

                                        XmlNode oAnim = gMapDoc.CreateNode("element", "Animation", "");
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
                            XmlNode oHolds = gMapDoc.CreateNode("element", "Holds", "");

                            for (int iH = 0; iH < oObj.Object.HoldGroups.Count; iH++)
                            {
                                XmlNode oHoldGroup = gMapDoc.CreateNode("element", "HoldGroup", "");
                                XmlAttribute oGroupId = gMapDoc.CreateAttribute("id");
                                oGroupId.Value = iH.ToString();
                                oHoldGroup.Attributes.Append(oGroupId);

                                foreach (fpxHold hold in oObj.Object.HoldGroups[iH])
                                {
                                    XmlAttribute oId = gMapDoc.CreateAttribute("id");
                                    oId.Value = hold.id.ToString();
                                    XmlAttribute oGid = gMapDoc.CreateAttribute("gid");
                                    oGid.Value = hold.gid.ToString();
                                    XmlAttribute oLid = gMapDoc.CreateAttribute("lid");
                                    oLid.Value = hold.lid.ToString();
                                    XmlAttribute oX1 = gMapDoc.CreateAttribute("x1");
                                    oX1.Value = hold.x1.ToString();
                                    XmlAttribute oX2 = gMapDoc.CreateAttribute("x2");
                                    oX2.Value = hold.x2.ToString();
                                    XmlAttribute oY1 = gMapDoc.CreateAttribute("y1");
                                    oY1.Value = hold.y1.ToString();
                                    XmlAttribute oY2 = gMapDoc.CreateAttribute("y2");
                                    oY2.Value = hold.y2.ToString();
                                    XmlAttribute oNextId = gMapDoc.CreateAttribute("nextid");
                                    oNextId.Value = hold.nextid.ToString();
                                    XmlAttribute oPrevId = gMapDoc.CreateAttribute("previd");
                                    oPrevId.Value = hold.previd.ToString();
                                    XmlAttribute oType = gMapDoc.CreateAttribute("type");
                                    oType.Value = hold.type;
                                    XmlAttribute oForce = gMapDoc.CreateAttribute("force");
                                    oForce.Value = hold.force.ToString();
                                    XmlAttribute oCantPass = gMapDoc.CreateAttribute("cantPass");
                                    oCantPass.Value = hold.cantPass.ToString();
                                    XmlAttribute oCantDrop = gMapDoc.CreateAttribute("cantDrop");
                                    oCantDrop.Value = hold.cantDrop.ToString();
                                    XmlAttribute oCantMove = gMapDoc.CreateAttribute("cantMove");
                                    oCantMove.Value = hold.cantMove.ToString();

                                    XmlNode holdPoint = gMapDoc.CreateNode("element", "Hold", "");
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


                            XmlNode oObject = gMapDoc.CreateNode("element", "Object", "");
                            oObject.Attributes.Append(oObjName);
                            oObject.Attributes.Append(oObjSpriteName);
                            oObject.Attributes.Append(oObjMapCoorX);
                            oObject.Attributes.Append(oObjMapCoorY);
                            oObject.Attributes.Append(oObjFlipped);
                            oObject.Attributes.Append(oX);
                            oObject.Attributes.Append(oY);
                            oObject.Attributes.Append(oObjWidth);
                            oObject.Attributes.Append(oObjHeight);
                            oObject.Attributes.Append(oAnimated);
                            oObject.Attributes.Append(oCannotDrop);
                            oObject.AppendChild(oAnimations);
                            oObject.AppendChild(oHolds);

                            oObjects.AppendChild(oObject);
                        }

                        foreach (fpxMapTile oTile in oL.MapTiles)
                        {
                            XmlAttribute oTileName = gMapDoc.CreateAttribute("Name");
                            oTileName.Value = oTile.Tile.Name;
                            XmlAttribute oTileMapCoorX = gMapDoc.CreateAttribute("MapCoorX");
                            oTileMapCoorX.Value = oTile.MapCoor.X.ToString();
                            XmlAttribute oTileMapCoorY = gMapDoc.CreateAttribute("MapCoorY");
                            oTileMapCoorY.Value = oTile.MapCoor.Y.ToString();
                            XmlAttribute oTileFlipped = gMapDoc.CreateAttribute("Flipped");
                            oTileFlipped.Value = oTile.Flipped.ToString();
                            XmlAttribute oTileFlip = gMapDoc.CreateAttribute("Name");
                            oTileFlip.Value = oTile.Flipped.ToString();
                            XmlAttribute oTileSpriteName = gMapDoc.CreateAttribute("SpriteSheet");
                            oTileSpriteName.Value = oTile.Tile.SpriteSheet;
                            XmlAttribute oX = gMapDoc.CreateAttribute("x");
                            oX.Value = oTile.Tile.x.ToString();
                            XmlAttribute oY = gMapDoc.CreateAttribute("y");
                            oY.Value = oTile.Tile.y.ToString();
                            XmlAttribute oTileWidth = gMapDoc.CreateAttribute("width");
                            oTileWidth.Value = oTile.Tile.width.ToString();
                            XmlAttribute oTileHeight = gMapDoc.CreateAttribute("height");
                            oTileHeight.Value = oTile.Tile.height.ToString();
                            XmlAttribute oTileCannotDrop = gMapDoc.CreateAttribute("CannotDrop");
                            oTileCannotDrop.Value = oTile.CannotDrop.ToString();

                            XmlNode oHolds = gMapDoc.CreateNode("element", "Holds", "");

                            for (int iH = 0; iH < oTile.Tile.HoldGroups.Count; iH++)
                            {
                                XmlNode oHoldGroup = gMapDoc.CreateNode("element", "HoldGroup", "");
                                XmlAttribute oGroupId = gMapDoc.CreateAttribute("id");
                                oGroupId.Value = iH.ToString();
                                oHoldGroup.Attributes.Append(oGroupId);

                                foreach (fpxHold hold in oTile.Tile.HoldGroups[iH])
                                {
                                    XmlAttribute oId = gMapDoc.CreateAttribute("id");
                                    oId.Value = hold.id.ToString();
                                    XmlAttribute oGid = gMapDoc.CreateAttribute("gid");
                                    oGid.Value = hold.gid.ToString();
                                    XmlAttribute oLid = gMapDoc.CreateAttribute("lid");
                                    oLid.Value = hold.lid.ToString();
                                    XmlAttribute oX1 = gMapDoc.CreateAttribute("x1");
                                    oX1.Value = hold.x1.ToString();
                                    XmlAttribute oX2 = gMapDoc.CreateAttribute("x2");
                                    oX2.Value = hold.x2.ToString();
                                    XmlAttribute oY1 = gMapDoc.CreateAttribute("y1");
                                    oY1.Value = hold.y1.ToString();
                                    XmlAttribute oY2 = gMapDoc.CreateAttribute("y2");
                                    oY2.Value = hold.y2.ToString();
                                    XmlAttribute oNextId = gMapDoc.CreateAttribute("nextid");
                                    oNextId.Value = hold.nextid.ToString();
                                    XmlAttribute oPrevId = gMapDoc.CreateAttribute("previd");
                                    oPrevId.Value = hold.previd.ToString();
                                    XmlAttribute oType = gMapDoc.CreateAttribute("type");
                                    oType.Value = hold.type;
                                    XmlAttribute oForce = gMapDoc.CreateAttribute("force");
                                    oForce.Value = hold.force.ToString();
                                    XmlAttribute oCantPass = gMapDoc.CreateAttribute("cantPass");
                                    oCantPass.Value = hold.cantPass.ToString();
                                    XmlAttribute oCantDrop = gMapDoc.CreateAttribute("cantDrop");
                                    oCantDrop.Value = hold.cantDrop.ToString();
                                    XmlAttribute oCantMove = gMapDoc.CreateAttribute("cantMove");
                                    oCantMove.Value = hold.cantMove.ToString();

                                    XmlNode holdPoint = gMapDoc.CreateNode("element", "Hold", "");
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


                            XmlNode oTileN = gMapDoc.CreateNode("element", "Tile", "");
                            oTileN.Attributes.Append(oTileName);
                            oTileN.Attributes.Append(oTileSpriteName);
                            oTileN.Attributes.Append(oTileMapCoorX);
                            oTileN.Attributes.Append(oTileMapCoorY);
                            oTileN.Attributes.Append(oX);
                            oTileN.Attributes.Append(oY);
                            oTileN.Attributes.Append(oTileWidth);
                            oTileN.Attributes.Append(oTileHeight);
                            oTileN.Attributes.Append(oTileFlipped);
                            oTileN.Attributes.Append(oTileCannotDrop);
                            oTileN.AppendChild(oHolds);

                            oTiles.AppendChild(oTileN);
                        }

                        for (int iH = 0; iH < oL.MapHolds.Count; iH++)
                        {
                            XmlNode oHoldGroup = gMapDoc.CreateNode("element", "HoldGroup", "");
                            XmlAttribute oGroupId = gMapDoc.CreateAttribute("id");
                            oGroupId.Value = iH.ToString();
                            oHoldGroup.Attributes.Append(oGroupId);

                            foreach (fpxHold hold in oL.MapHolds[iH])
                            {
                                XmlAttribute oId = gMapDoc.CreateAttribute("id");
                                oId.Value = hold.id.ToString();
                                XmlAttribute oGid = gMapDoc.CreateAttribute("gid");
                                oGid.Value = hold.gid.ToString();
                                XmlAttribute oLid = gMapDoc.CreateAttribute("lid");
                                oLid.Value = hold.lid.ToString();
                                XmlAttribute oX1 = gMapDoc.CreateAttribute("x1");
                                oX1.Value = hold.x1.ToString();
                                XmlAttribute oX2 = gMapDoc.CreateAttribute("x2");
                                oX2.Value = hold.x2.ToString();
                                XmlAttribute oY1 = gMapDoc.CreateAttribute("y1");
                                oY1.Value = hold.y1.ToString();
                                XmlAttribute oY2 = gMapDoc.CreateAttribute("y2");
                                oY2.Value = hold.y2.ToString();
                                XmlAttribute oNextId = gMapDoc.CreateAttribute("nextid");
                                oNextId.Value = hold.nextid.ToString();
                                XmlAttribute oPrevId = gMapDoc.CreateAttribute("previd");
                                oPrevId.Value = hold.previd.ToString();
                                XmlAttribute oType = gMapDoc.CreateAttribute("type");
                                oType.Value = hold.type;
                                XmlAttribute oForce = gMapDoc.CreateAttribute("force");
                                oForce.Value = hold.force.ToString();
                                XmlAttribute oCantPass = gMapDoc.CreateAttribute("cantPass");
                                oCantPass.Value = hold.cantPass.ToString();
                                XmlAttribute oCantDrop = gMapDoc.CreateAttribute("cantDrop");
                                oCantDrop.Value = hold.cantDrop.ToString();
                                XmlAttribute oCantMove = gMapDoc.CreateAttribute("cantMove");
                                oCantMove.Value = hold.cantMove.ToString();

                                XmlNode holdPoint = gMapDoc.CreateNode("element", "Hold", "");
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

                            oHoldsG.AppendChild(oHoldGroup);
                        }
                        

                        oLayer.AppendChild(oSprites);
                        oLayer.AppendChild(oScenery);
                        oLayer.AppendChild(oObjects);
                        oLayer.AppendChild(oTiles);
                        oLayer.AppendChild(oHoldsG);

                        oLayers.AppendChild(oLayer);
                    }

                    oMapN.AppendChild(oLayers);
                    oMapN.AppendChild(oPortals);

                    oRegionN.AppendChild(oMapN);
                }

                oMapRoot.AppendChild(oRegionN);
            }

            gMapDoc.RemoveAll();
            gMapDoc.AppendChild(oMapRoot);

            ValidateSave();
            ManageControls();
        }
        public void ManageControls()
        {

            bool bMapMode = false;
            bool bFootholdMode = false;
            bool bClimbholdMode = false;
            bool bSeatholdMode = false;
            bool bHoldMode = false;
            bool bSceneryMode = false;
            bool bTileMode = false;
            bool bObjectMode = false;
            bool bPortalMode = false;
            bool bElementListPopulated = tsbList.Items.Count > 0;
            bool bElementItemSelected = tsbList.SelectedItem != null;
            bool bBackgroundSelected = false;
            bool bRegionCollection = false;
            bool bMapCollection = false;
            bool bMusicSelected = !string.IsNullOrWhiteSpace(txtBGMusic.Text);
            bool bAdding = false;
            bool bListItemSelected = false;
            

            if(cmbRegion.SelectedIndex > -1)
            {
                bRegionCollection = true;

                if(cmbMaps.SelectedIndex > -1)
                {
                    bMapCollection = true;

                    if (cmbLayer.Items.Count > 0)
                    {
                        if (int.Parse(cmbLayer.SelectedItem.ToString()) < 0)
                            bBackgroundSelected = true;
                    }
                }
            }

            if(tsbTile.Checked || tsbObject.Checked)
            {
                if(tsbList.Items.Count > 0)
                {
                    if(tsbList.SelectedIndex > -1)
                    {
                        bListItemSelected = true;
                    }
                }
            }

            bMapMode = tsbMap.Checked;
            bFootholdMode = tsbFoothold.Checked;
            bClimbholdMode = tsbClimbhold.Checked;
            bSeatholdMode = tsbSeathold.Checked;
            bHoldMode = bFootholdMode || bClimbholdMode || bSeatholdMode;
            bSceneryMode = tsbScenery.Checked;
            bTileMode = tsbTile.Checked;
            bObjectMode = tsbObject.Checked;
            bPortalMode = tsbPortal.Checked;
            bAdding = tsbAdd.Checked;


            tsbMap.Enabled = !bHoldMode && !bAdding;
            tsbFoothold.Enabled = bMapCollection && !bHoldMode && !bAdding && !bBackgroundSelected || bFootholdMode;
            tsbClimbhold.Enabled = bMapCollection && !bHoldMode && !bAdding && !bBackgroundSelected || bClimbholdMode;
            tsbSeathold.Enabled = bMapCollection && !bHoldMode && !bAdding && !bBackgroundSelected || bSeatholdMode;
            tsbScenery.Enabled = bMapCollection && !bHoldMode && !bAdding && bBackgroundSelected;
            tsbTile.Enabled = bMapCollection && !bHoldMode && !bAdding;
            tsbObject.Enabled = bMapCollection && !bHoldMode && !bAdding;
            tsbPortal.Enabled = bMapCollection && !bHoldMode && !bAdding && !bBackgroundSelected;

            cmbLayer.Enabled = bMapCollection;
            chkHideForward.Enabled = bMapCollection;
            numParallaxX.Enabled = bBackgroundSelected;
            numParallaxY.Enabled = bBackgroundSelected;
            numScrollX.Enabled = bBackgroundSelected;
            numScrollY.Enabled = bBackgroundSelected;
            chkTileX.Enabled = bBackgroundSelected;
            chkTileY.Enabled = bBackgroundSelected;
            btnSceneryClear.Enabled = !string.IsNullOrWhiteSpace(txtScenery.Text);

            btnRegionRemove.Enabled = bRegionCollection;
            cmbRegion.Enabled = bRegionCollection;
            btnMapNew.Enabled = bRegionCollection;
            cmbMaps.Enabled = bMapCollection;
            btnMapRemove.Enabled = bMapCollection;
            numMapHeight.Enabled = bMapCollection;
            numMapWidth.Enabled = bMapCollection;
            numBoundRight.Enabled = bMapCollection;
            numBoundLeft.Enabled = bMapCollection;
            numBoundTop.Enabled = bMapCollection;
            numBoundBottom.Enabled = bMapCollection;
            btnMapMusic.Enabled = bMapCollection && cmbMaps.Items.Count > 0;
            pbMusicPreview.Enabled = bMusicSelected;

            tsbAdd.Enabled = (bHoldMode || bObjectMode || bTileMode || bPortalMode) && !bAdding;
            tsbConfirm.Enabled = (bHoldMode || bObjectMode || bTileMode || bPortalMode) && tsbAdd.Checked;
            tsbList.Enabled = (bHoldMode  || bObjectMode || bTileMode || bPortalMode) && bElementListPopulated;
            tsbProperties.Enabled = (bHoldMode || bObjectMode || bTileMode || bPortalMode) && bElementItemSelected;
            tsbDelete.Enabled = (bHoldMode || bObjectMode || bTileMode || bPortalMode) && bElementItemSelected;

            tsbAdd.Visible = bHoldMode || bObjectMode || bTileMode || bPortalMode;
            tsbConfirm.Visible = bHoldMode || bObjectMode || bTileMode || bPortalMode;
            tsbList.Visible = bHoldMode || bObjectMode || bTileMode || bPortalMode;
            tsbProperties.Visible = bHoldMode || bObjectMode || bTileMode || bPortalMode;
            tsbDelete.Visible = bHoldMode || bObjectMode || bTileMode || bPortalMode;

            pnlGlobalProperties.Enabled = !bHoldMode && !bPortalMode && !bSceneryMode && !tsbAdd.Checked;
            pnlMaps.Visible = bMapMode;
            pnlScenery.Visible = bSceneryMode;
            pnlObjects.Visible = bObjectMode;
            pnlTiles.Visible = bTileMode;

            numTileX.Enabled = bListItemSelected;
            numTileY.Enabled = bListItemSelected;
            chkTileDrop.Enabled = bListItemSelected;

            btnSave.Enabled = Dirty;
            btnCancel.Enabled = Dirty;
        }
        #endregion

        #region Helper Methods
        public void ClearMaps()
        {
            cmbMaps.Items.Clear();
            ClearMapProperties();
        }

        public void ClearMapProperties()
        {
            numMapWidth.Value = 1;
            numMapHeight.Value = 1;
            numBoundRight.Value = 0;
            numBoundLeft.Value = 0;
            numBoundTop.Value = 0;
            numBoundBottom.Value = 0;
            txtBGMusic.Text = "";
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

        public int GetLayerIndex()
        {
            return int.Parse(cmbLayer.SelectedItem.ToString()) + 8;
        }
        #endregion

        private void numTileX_ValueChanged(object sender, EventArgs e)
        {
            fpxMapTile oTile = gRegions[cmbRegion.SelectedIndex].
               Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].
                   MapTiles[tsbList.SelectedIndex];

            oTile.MapCoor = new Point((int)numTileX.Value, oTile.MapCoor.Y);

            ValidateMaps();
            ediMap.Invalidate();
        }

        private void numTileY_ValueChanged(object sender, EventArgs e)
        {
            fpxMapTile oTile = gRegions[cmbRegion.SelectedIndex].
                     Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].
                         MapTiles[tsbList.SelectedIndex];

            oTile.MapCoor = new Point(oTile.MapCoor.X, (int)numTileY.Value);

            ValidateMaps();
            ediMap.Invalidate();
        }

        private void chkCannotDrop_Click(object sender, EventArgs e)
        {
            fpxMapTile oTile = gRegions[cmbRegion.SelectedIndex].
                 Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].
                     MapTiles[tsbList.SelectedIndex];

            oTile.CannotDrop = chkTileDrop.Checked;

            foreach(List<fpxHold> oHoldGroup in oTile.Tile.HoldGroups.Values)
            {
                foreach(fpxHold oHold in oHoldGroup)
                {
                    oHold.cantDrop = oTile.CannotDrop;
                }
            }

            ValidateMaps();
        }

        private void numObjectX_ValueChanged(object sender, EventArgs e)
        {
            fpxMapObject oObj = gRegions[cmbRegion.SelectedIndex].
              Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].
                  MapObjects[tsbList.SelectedIndex];

            oObj.MapCoor = new Point((int)numObjectX.Value, oObj.MapCoor.Y);

            ValidateMaps();
            ediMap.Invalidate();
        }

        private void numObjectY_ValueChanged(object sender, EventArgs e)
        {
            fpxMapObject oObj = gRegions[cmbRegion.SelectedIndex].
             Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].
                 MapObjects[tsbList.SelectedIndex];

            oObj.MapCoor = new Point(oObj.MapCoor.X, (int)numObjectY.Value);

            ValidateMaps();
            ediMap.Invalidate();
        }

        private void chkObjectDrop_Click(object sender, EventArgs e)
        {
            fpxMapObject oObj = gRegions[cmbRegion.SelectedIndex].
            Maps[cmbMaps.SelectedIndex].MapLayers[GetLayerIndex()].
                MapObjects[tsbList.SelectedIndex];

            oObj.CannotDrop = chkObjectDrop.Checked;

            foreach (List<fpxHold> oHoldGroup in oObj.Object.HoldGroups.Values)
            {
                foreach (fpxHold oHold in oHoldGroup)
                {
                    oHold.cantDrop = oObj.CannotDrop;
                }
            }

            ValidateMaps();
        }

        private void btnRemapObjects_Click(object sender, EventArgs e)
        {

        }

        private void btnRemapTiles_Click(object sender, EventArgs e)
        {
            foreach(fpxMapLayer oLayer in gRegions[cmbRegion.SelectedIndex].Maps[cmbMaps.SelectedIndex].MapLayers)
            {
                foreach (fpxMapTile oTile in oLayer.MapTiles)
                {
                    if (gTileDoc != null)
                    {
                        foreach (XmlNode oSheet in gTileDoc.FirstChild.ChildNodes)
                        {
                            foreach(XmlNode oTileN in oSheet.ChildNodes)
                            {
                                if(oTileN.Attributes["Name"].Value == oTile.Tile.Name)
                                {
                                    oTile.Tile.x = int.Parse(oTileN.Attributes["x"].Value);
                                    oTile.Tile.y = int.Parse(oTileN.Attributes["y"].Value);
                                }
                            }
                        }
                    }
                }
            }
       

            ValidateMaps();
            DoSave();
            Reload();
            ediMap.Invalidate();
        }

        public void DoSave()
        {
            string sXml = gMapDoc.OuterXml;

            XmlDocument xSave = new XmlDocument();
            xSave.LoadXml(sXml);
            EditorManager.SetMapCollection(xSave);

            ValidateSave();
            ManageControls();
        }
    }
}
