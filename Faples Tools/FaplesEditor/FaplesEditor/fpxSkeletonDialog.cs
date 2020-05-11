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
    public partial class fpxSkeletonDialog : Form
    {
        #region Properties
        public List<fpxSkeletonPoint> CurrentPoint { get; private set; }
        public Guid PointID { get; private set; } = new Guid();
        public string PartType { get; private set; } = "Limb";

        public void SetSkeleton(List<fpxSkeletonPoint> oPoints)
        {
            cmbConnectPoint.Items.Clear();
            CurrentPoint = oPoints;

            for (int i = 0; i < CurrentPoint.Count; i++)
            {
                cmbConnectPoint.Items.Add("Point" + i);
            }

            if (cmbConnectPoint.Items.Count > 0)
                cmbConnectPoint.SelectedIndex = 0;
        }
        #endregion

        #region Constructor
        public fpxSkeletonDialog()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Event Handler
        private void cmbConnectPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            PointID = CurrentPoint[cmbConnectPoint.SelectedIndex].ID;
        }
        private void cmbPartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PartType = cmbPartType.SelectedItem.ToString();
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
        #endregion

        #region Main Methods
        private void Prepare()
        {
            PrepareTypes();
        }
        private void PrepareTypes()
        {
            cmbPartType.Items.Add("Head");
            cmbPartType.Items.Add("Limb");

            cmbPartType.SelectedIndex = 0;
        }
        #endregion
    }
}
