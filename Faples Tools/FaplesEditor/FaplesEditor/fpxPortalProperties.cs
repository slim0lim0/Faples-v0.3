using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaplesEditor
{
    public partial class fpxPortalProperties : Form
    {
        private List<fpxRegion> gRegions = new List<fpxRegion>();
        private fpxMapPortal gPortal = new fpxMapPortal();

        public fpxPortalProperties()
        {
            InitializeComponent();
            Prepare();
        }

        private void Prepare()
        {
            PrepareTypes();
        }

        private void PrepareTypes()
        {
            cmbPortalType.Items.Add("portal");
            cmbPortalType.Items.Add("spawnEnter");
            cmbPortalType.Items.Add("spawnExit");
            cmbPortalType.Items.Add("secret");
            cmbPortalType.Items.Add("hidden");

            cmbPortalType.SelectedIndex = 0;
        }

        public void LoadProperties(fpxMapPortal oPortal, List<fpxRegion> oRegions)
        {
            gRegions = oRegions;
            gPortal = oPortal;

            foreach(fpxRegion oRegion in gRegions)
            {
                cmbRegion.Items.Add(oRegion.Name);
            }

            cmbPortalType.SelectedItem = gPortal.Type;
            cmbRegion.SelectedIndex = gPortal.RegionID;

            if (gPortal.Type == "spawnEnter")
            {
                cmbRegion.Enabled = false;
                cmbMapName.Enabled = false;
            }
            else
            {
                cmbRegion.Enabled = true;
                cmbMapName.Enabled = true;
            }
        }

        public fpxMapPortal SaveProperties()
        {
            return gPortal;
        }

        private void cmbPortalType_SelectedValueChanged(object sender, EventArgs e)
        {
            gPortal.Type = cmbPortalType.SelectedItem.ToString();

            if (gPortal.Type == "spawnEnter")
            {
                cmbRegion.Enabled = false;
                cmbMapName.Enabled = false;
            }
            else
            {
                cmbRegion.Enabled = true;
                cmbMapName.Enabled = true;
            }
        }

        private void cmbRegion_SelectedValueChanged(object sender, EventArgs e)
        {
            gPortal.RegionID = cmbRegion.SelectedIndex;

            cmbMapName.Items.Clear();

            foreach (fpxMap oMap in gRegions[cmbRegion.SelectedIndex].Maps)
            {
                cmbMapName.Items.Add(oMap.MapName);
            }

            if (cmbMapName.Items.Count > 0)
            {
                cmbMapName.SelectedIndex = gPortal.MapID;
            }
        }

        private void cmbMapName_SelectedValueChanged(object sender, EventArgs e)
        {
            gPortal.MapID = cmbMapName.SelectedIndex;

            cmbTargetPortal.Items.Clear();

            foreach (fpxMapPortal oPortal in gRegions[cmbRegion.SelectedIndex].Maps[cmbMapName.SelectedIndex].MapPortals)
            {
                cmbTargetPortal.Items.Add(oPortal.ID);
            }

            if (cmbTargetPortal.Items.Count > 0)
            {
                cmbTargetPortal.SelectedIndex = gPortal.TargetID;
            }
        }
        private void cmbTargetPortal_SelectedValueChanged(object sender, EventArgs e)
        {
            gPortal.TargetID = cmbTargetPortal.SelectedIndex;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        public void ManageControls()
        {
            bool bMaps = false;
            bool bTargets = false;

            if (cmbRegion.SelectedIndex > -1)
            {
                bMaps = true;
                if (cmbMapName.SelectedIndex > -1)
                    bTargets = true;

            }

            cmbMapName.Enabled = bMaps;
            cmbTargetPortal.Enabled = bTargets;
        }
    }
}
