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
    public partial class fpxHoldProperties : Form
    {
        #region Declarations
        private List<fpxHold> gHolds = new List<fpxHold>();
        #endregion

        #region Constructor
        public fpxHoldProperties()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Event Handlers
        private void dgvHolds_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHolds.SelectedRows.Count > 0)
            {
                fpxHold hold = gHolds[dgvHolds.SelectedRows[0].Index];

                numId.Value = hold.id;
                numGid.Value = hold.gid;
                numNextId.Value = hold.nextid;
                numX2.Value = hold.x2;
                numY2.Value = hold.y2;
                numPrevId.Value = hold.previd;
                txtType.Text = hold.type;
                numX1.Value = hold.x1;
                numY1.Value = hold.y1;
                numForce.Value = hold.force;
                cmbCantPass.SelectedIndex = hold.cantPass ? 0 : 1;
                cmbCantDrop.SelectedIndex = hold.cantDrop ? 0 : 1;
                cmbCantMove.SelectedIndex = hold.cantMove ? 0 : 1;
            }
        }

        private void numX1_ValueChanged(object sender, EventArgs e)
        {
            if (dgvHolds.SelectedRows.Count > 0)
            {
                fpxHold hold = gHolds[dgvHolds.SelectedRows[0].Index];
                fpxHold prevH = gHolds.ElementAtOrDefault(dgvHolds.SelectedRows[0].Index - 1);
                hold.x1 = (int)numX1.Value;

                gHolds[dgvHolds.SelectedRows[0].Index] = hold;

                if (prevH != null)
                {
                    prevH.x2 = hold.x1;

                    gHolds[dgvHolds.SelectedRows[0].Index - 1] = prevH;
                }
            }
        }

        private void numY1_ValueChanged(object sender, EventArgs e)
        {
            if (dgvHolds.SelectedRows.Count > 0)
            {
                fpxHold hold = gHolds[dgvHolds.SelectedRows[0].Index];
                fpxHold prevH = gHolds.ElementAtOrDefault(dgvHolds.SelectedRows[0].Index - 1);
                hold.y1 = (int)numY1.Value;

                gHolds[dgvHolds.SelectedRows[0].Index] = hold;

                if (prevH != null)
                {
                    prevH.y2 = hold.y1;

                    gHolds[dgvHolds.SelectedRows[0].Index - 1] = prevH;
                }
            }
        }

        private void numForce_ValueChanged(object sender, EventArgs e)
        {
            if (dgvHolds.SelectedRows.Count > 0)
            {
                fpxHold hold = gHolds[dgvHolds.SelectedRows[0].Index];
                hold.force = (int)numForce.Value;

                gHolds[dgvHolds.SelectedRows[0].Index] = hold;
            }
        }

        private void cmbCantPass_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvHolds.SelectedRows.Count > 0)
            {
                fpxHold hold = gHolds[dgvHolds.SelectedRows[0].Index];
                hold.cantPass = cmbCantPass.SelectedIndex == 0 ? true : false;

                gHolds[dgvHolds.SelectedRows[0].Index] = hold;
            }
        }

        private void cmbCantDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvHolds.SelectedRows.Count > 0)
            {
                fpxHold hold = gHolds[dgvHolds.SelectedRows[0].Index];
                hold.cantDrop = cmbCantDrop.SelectedIndex == 0 ? true : false;

                gHolds[dgvHolds.SelectedRows[0].Index] = hold;
            }
        }

        private void cmbCantMove_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvHolds.SelectedRows.Count > 0)
            {
                fpxHold hold = gHolds[dgvHolds.SelectedRows[0].Index];
                hold.cantMove = cmbCantMove.SelectedIndex == 0 ? true : false;

                gHolds[dgvHolds.SelectedRows[0].Index] = hold;
            }
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
            PrepareControls();
        }

        private void PrepareControls()
        {
            DataGridViewTextBoxColumn colText = new DataGridViewTextBoxColumn();
            colText.Width = (int)(dgvHolds.Width);

            dgvHolds.Columns.Add(colText);

            dgvHolds.ColumnHeadersVisible = false;
            dgvHolds.RowHeadersVisible = false;
            dgvHolds.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvHolds.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvHolds.AllowUserToAddRows = false;
        }
        public void LoadHolds(List<fpxHold> holds)
        {
            gHolds = holds;

            for (int i = 0; i < gHolds.Count; i++)
            {
                dgvHolds.Rows.Add(new DataGridViewRow());
                DataGridViewRow oRow = dgvHolds.Rows[dgvHolds.Rows.Count - 1];
                oRow.Cells[0].Value = "Point" + i;
            }

            if (dgvHolds.SelectedRows.Count < 1)
            {
                int iIndex = Math.Max(dgvHolds.Rows.Count - 1, 0);

                dgvHolds.Rows[iIndex].Selected = true;
            }
        }

        public List<fpxHold> SaveHolds()
        {
            return gHolds;
        }
        #endregion
    }
}
