using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace FaplesEditor
{
    public partial class fpxControlDialog : Form
    {
        #region Declarations
        public fpxControl g_oCurrentControl = new fpxControl();
        public fpxControl g_oBackupControl = new fpxControl();
        #endregion

        #region Properties
        public string ControlType { get; set; } = "";
        public fpxControl CurrentControl
        {
            get { return g_oCurrentControl; }
            set { g_oCurrentControl = value; }
        }
        public fpxControl BackupControl {
            get { return g_oBackupControl; }
            set { g_oBackupControl = value; }
        }

        public bool Preparing { get; set; } = true;
        #endregion

        #region Constructor
        public fpxControlDialog()
        {
            InitializeComponent();
            Prepare();
        }
        #endregion

        #region Main Methods
        public void Prepare()
        {
            PrepareControls();
        }

        public void PrepareControls()
        {
            pnlTextbox.Visible = false;
            pnlButton.Visible = false;
            pnlPercentBar.Visible = false;
            pnlCheckRadio.Visible = false;
            pnlCombobox.Visible = false;

            // Choice Options
            DataGridViewTextBoxColumn colText = new DataGridViewTextBoxColumn();
            colText.Width = (int)(dgvOptions.Width);

            dgvOptions.Columns.Add(colText);

            dgvOptions.ColumnHeadersVisible = false;
            dgvOptions.RowHeadersVisible = false;
            dgvOptions.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvOptions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvOptions.AllowUserToAddRows = false;

            // Combobox Options
            DataGridViewTextBoxColumn colText2 = new DataGridViewTextBoxColumn();
            colText2.Width = (int)(dgvComboOptions.Width);

            dgvComboOptions.Columns.Add(colText2);

            dgvComboOptions.ColumnHeadersVisible = false;
            dgvComboOptions.RowHeadersVisible = false;
            dgvComboOptions.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvComboOptions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvComboOptions.AllowUserToAddRows = false;
        }
        public void RefreshControls()
        {
            txtFontPreview.ReadOnly = false;
            txtFontPreview.Font = new Font(CurrentControl.Font, CurrentControl.Size);
            txtFontPreview.ForeColor = CurrentControl.Color;
            txtFontPreview.Update();
            txtFontPreview.ReadOnly = true;
        }
        public void ResizeForm(Size oSize)
        {
            // 20px height clearance for clean sizing.
            flpContainer.Size = new Size(oSize.Width, oSize.Height+ 20);
            Size = new Size(Size.Width, oSize.Height + 100);
        }
        public void PrepareBackup()
        {
            BackupControl.Name = CurrentControl.Name;
            BackupControl.Type = CurrentControl.Type;
            BackupControl.LocX = CurrentControl.LocX;
            BackupControl.LocY = CurrentControl.LocY;
            BackupControl.Width = CurrentControl.Width;
            BackupControl.Height = CurrentControl.Height;
            BackupControl.Multiline = CurrentControl.Multiline;
            BackupControl.Font = CurrentControl.Font;
            BackupControl.Size = CurrentControl.Size;
            BackupControl.Color = CurrentControl.Color;
            BackupControl.Label = CurrentControl.Label;
            BackupControl.LabelValue = CurrentControl.LabelValue;
            BackupControl.SpriteBase = CurrentControl.SpriteBase;
            BackupControl.SpriteBaseName = CurrentControl.SpriteBaseName;
            BackupControl.SpriteHover = CurrentControl.SpriteHover;
            BackupControl.SpriteHoverName = CurrentControl.SpriteHoverName;
            BackupControl.SpriteClick = CurrentControl.SpriteClick;
            BackupControl.SpriteClickName = CurrentControl.SpriteClickName;
            BackupControl.SpriteFocus = CurrentControl.SpriteFocus;
            BackupControl.SpriteFocusName = CurrentControl.SpriteFocusName;
            BackupControl.PercentBar = CurrentControl.PercentBar;
            BackupControl.PercentBarName = CurrentControl.PercentBarName;
            BackupControl.PercentValue = CurrentControl.PercentValue;
            BackupControl.Options = CurrentControl.Options;
            BackupControl.Radio = CurrentControl.Radio;
            BackupControl.FlowDirection = CurrentControl.FlowDirection;
            BackupControl.ComboDisplays = CurrentControl.ComboDisplays;
            BackupControl.ComboValues = CurrentControl.ComboValues;
        }
        #endregion

        #region Event Handlers
        private void fpxControlDialog_Shown(object sender, EventArgs e)
        {
            if(ControlType.Equals("Textbox"))
            {
                pnlTextbox.Visible = true;

                foreach(var font in Utility.GetFontCollection())
                {
                    cmbFont.Items.Add(font);
                }

                cmbFont.SelectedItem = CurrentControl.Font.Name;
                numFontSize.Value = CurrentControl.Size;
                numFontR.Value = CurrentControl.Color.R;
                numFontG.Value = CurrentControl.Color.G;
                numFontB.Value = CurrentControl.Color.B;

                chkLabel.Checked = CurrentControl.Label;
                txtTextboxValue.Text = CurrentControl.LabelValue;

                ResizeForm(pnlTextbox.Size);
            }
            else if(ControlType.Equals("Button"))
            {
                pnlButton.Visible = true;
                if(CurrentControl.SpriteBase != null)
                {
                    //Bitmap oBmp = Utility.ResizeImage(CurrentControl.SpriteBase, pbButtonDefault.Width, pbButtonDefault.Height);
                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteBaseName));
                    pbButtonDefault.Image = oBmp;
                }
                    
                if (CurrentControl.SpriteHover != null)
                {
                    //Bitmap oBmp = Utility.ResizeImage(CurrentControl.SpriteHover, pbButtonDefault.Width, pbButtonDefault.Height);
                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteHoverName));
                    pbButtonHover.Image = oBmp;
                }
                if (CurrentControl.SpriteClick != null)
                {
                   // Bitmap oBmp = Utility.ResizeImage(CurrentControl.SpriteClick, pbButtonDefault.Width, pbButtonDefault.Height);
                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteClickName));
                    pbButtonClick.Image = oBmp;
                }
                if (CurrentControl.SpriteFocus != null)
                {
                   // Bitmap oBmp = Utility.ResizeImage(CurrentControl.SpriteFocus, pbButtonDefault.Width, pbButtonDefault.Height);
                    Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteFocusName));
                    pbButtonFocus.Image = oBmp;
                }

                ResizeForm(pnlButton.Size);
            }
            else if(ControlType.Equals("Percentage Bar"))
            {
                pnlPercentBar.Visible = true;
                if (CurrentControl.PercentBar != null)
                    pbPercentBar.Image = CurrentControl.PercentBar;
                numPercent.Value = CurrentControl.PercentValue;

                ResizeForm(pnlPercentBar.Size);
            }
            else if(ControlType.Equals("Options"))
            {
                pnlCheckRadio.Visible = true;
                cmbFlowDirection.Items.Add("Horizontal");
                cmbFlowDirection.Items.Add("Vertical");

                cmbFlowDirection.SelectedItem = CurrentControl.FlowDirection;

                if (CurrentControl.Radio)
                    rbtnTypeRadio.Checked = true;
                else
                    rbtnTypeCheckbox.Checked = true;

                for (int i = 0; i < CurrentControl.Options.Count; i++)
                {
                    dgvOptions.Rows.Add(new DataGridViewRow());
                    DataGridViewRow oRow = dgvOptions.Rows[dgvOptions.Rows.Count - 1];
                    oRow.Cells[0].Value = "Option" + i;
                }                

                if (dgvOptions.SelectedRows.Count < 1 && dgvOptions.Rows.Count > 0)
                {
                    int iIndex = Math.Max(dgvOptions.Rows.Count - 1, 0);

                    dgvOptions.Rows[iIndex].Selected = true;
                }

                ResizeForm(pnlCheckRadio.Size);
            }
            else if(ControlType.Equals("Combobox"))
            {
                pnlCombobox.Visible = true;

                for (int i = 0; i < CurrentControl.Options.Count; i++)
                {
                    dgvOptions.Rows.Add(new DataGridViewRow());
                    DataGridViewRow oRow = dgvOptions.Rows[dgvComboOptions.Rows.Count - 1];
                    oRow.Cells[0].Value = CurrentControl.ComboDisplays[i];
                }

                if (dgvOptions.SelectedRows.Count < 1 && dgvOptions.Rows.Count > 0)
                {
                    int iIndex = Math.Max(dgvOptions.Rows.Count - 1, 0);

                    dgvOptions.Rows[iIndex].Selected = true;
                }

                ResizeForm(pnlCombobox.Size);
            }

            Preparing = false;
            PrepareBackup();
            RefreshControls();
        }
        private void cmbFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!Preparing)
            {
                CurrentControl.Font = Utility.GetFontFamilyByName(cmbFont.SelectedItem.ToString());
                RefreshControls();
            }
        }
        private void numFontSize_ValueChanged(object sender, EventArgs e)
        {
            if(!Preparing)
            {
                CurrentControl.Size = (int)numFontSize.Value;
                RefreshControls();
            }
        }

        private void numFontR_ValueChanged(object sender, EventArgs e)
        {
            if(!Preparing)
            {
                byte[] color = { (byte)numFontR.Value, (byte)numFontG.Value, (byte)numFontB.Value };
                CurrentControl.Color = ColorTranslator.FromHtml(string.Concat("#", BitConverter.ToString(color).Replace("-", string.Empty)));
                RefreshControls();
            }
        }

        private void numFontG_ValueChanged(object sender, EventArgs e)
        {
            if (!Preparing)
            {
                byte[] color = { (byte)numFontR.Value, (byte)numFontG.Value, (byte)numFontB.Value };
                CurrentControl.Color = ColorTranslator.FromHtml(string.Concat("#", BitConverter.ToString(color).Replace("-", string.Empty)));
                RefreshControls();
            }
        }

        private void numFontB_ValueChanged(object sender, EventArgs e)
        {
            if (!Preparing)
            {
                byte[] color = { (byte)numFontR.Value, (byte)numFontG.Value, (byte)numFontB.Value };
                CurrentControl.Color = ColorTranslator.FromHtml(string.Concat("#", BitConverter.ToString(color).Replace("-", string.Empty)));
                RefreshControls();
            }
        }
        private void chkLabel_CheckedChanged(object sender, EventArgs e)
        {
            if (!Preparing)
            {
                CurrentControl.Label = chkLabel.Checked;
                RefreshControls();
            }
        }

        private void txtTextboxValue_TextChanged(object sender, EventArgs e)
        {
            if (!Preparing)
            {
                CurrentControl.LabelValue = txtTextboxValue.Text;
                RefreshControls();
            }
        }
        private void btnDefaultPic_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH))
                    {
                        MessageBox.Show("Please select image within 'Controls' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    Image oBmp = Image.FromFile(opf.FileName, true);
                    CurrentControl.SpriteBaseName = opf.SafeFileName;

                    oBmp = Utility.ResizeImage(oBmp, pbButtonDefault.Width, pbButtonDefault.Height);
                    pbButtonDefault.Image = oBmp;

                    bValid = true;
                }
            }
        }
        private void btnHoverPic_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH))
                    {
                        MessageBox.Show("Please select image within 'Controls' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    Image oBmp = Image.FromFile(opf.FileName, true);
                    CurrentControl.SpriteHoverName  = opf.SafeFileName;

                    oBmp = Utility.ResizeImage(oBmp, pbButtonHover.Width, pbButtonHover.Height);
                    pbButtonHover.Image = oBmp;

                    bValid = true;
                }
            }
        }
        private void btnClickPic_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH))
                    {
                        MessageBox.Show("Please select image within 'Controls' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    Image oBmp = Image.FromFile(opf.FileName, true);
                    CurrentControl.SpriteClickName = opf.SafeFileName;

                    oBmp = Utility.ResizeImage(oBmp, pbButtonClick.Width, pbButtonClick.Height);
                    pbButtonClick.Image = oBmp;

                    bValid = true;
                }
            }
        }
        private void btnFocusPic_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH))
                    {
                        MessageBox.Show("Please select image within 'Controls' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    Image oBmp = Image.FromFile(opf.FileName, true);
                    CurrentControl.SpriteFocusName = opf.SafeFileName;

                    oBmp = Utility.ResizeImage(oBmp, pbButtonFocus.Width, pbButtonFocus.Height);
                    pbButtonFocus.Image = oBmp;

                    bValid = true;
                }
            }
        }

        private void btnPercentBar_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH))
                    {
                        MessageBox.Show("Please select image within 'Controls' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    Image oBmp = Image.FromFile(opf.FileName, true);
                    CurrentControl.PercentBarName = opf.SafeFileName;

                    oBmp = Utility.ResizeImage(oBmp, pbPercentBar.Width, pbPercentBar.Height);
                    pbPercentBar.Image = oBmp;

                    bValid = true;
                }
            }
        }
        private void numPercent_ValueChanged(object sender, EventArgs e)
        {
            if (!Preparing)
            {
                CurrentControl.PercentValue = numPercent.Value;
                RefreshControls();
            }
        }
        private void dgvOptions_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOptions.SelectedRows.Count > 0)
            {
                fpxOption oOption = CurrentControl.Options[dgvOptions.SelectedRows[0].Index];

                if (oOption.SpriteCheckOn != null)
                    pbCheckOn.Image = Utility.ResizeImage(oOption.SpriteCheckOn, pbCheckOn.Width, pbCheckOn.Height);
                else
                    pbCheckOn.Image = null;

                if (oOption.SpriteCheckOff != null)
                    pbCheckOff.Image = Utility.ResizeImage(oOption.SpriteCheckOff, pbCheckOff.Width, pbCheckOff.Height);
                else
                    pbCheckOn.Image = null;
            }
        }

        private void btnOptionAdd_Click(object sender, EventArgs e)
        {
            fpxOption oOption = new fpxOption();
            CurrentControl.Options.Add(oOption);

            dgvOptions.Rows.Add(new DataGridViewRow());
            DataGridViewRow oRow = dgvOptions.Rows[dgvOptions.Rows.Count - 1];
            oRow.Cells[0].Value = "Option" + (dgvOptions.Rows.Count - 1);
        }

        private void btnOptionRemove_Click(object sender, EventArgs e)
        {
            CurrentControl.Options.RemoveAt(dgvOptions.Rows.Count - 1);
            dgvOptions.Rows.RemoveAt(dgvOptions.Rows.Count - 1);
        }

        private void cmbFlowDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentControl.FlowDirection = cmbFlowDirection.SelectedItem.ToString();
        }

        private void rbtnTypeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CurrentControl.Radio = rbtnTypeCheckbox.Checked;
        }

        private void rbtnTypeRadio_CheckedChanged(object sender, EventArgs e)
        {
            CurrentControl.Radio = rbtnTypeRadio.Checked;
        }

        private void btnCheckOn_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH))
                    {
                        MessageBox.Show("Please select image within 'Controls' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    Image oBmp = Image.FromFile(opf.FileName, true);
                    CurrentControl.Options[dgvOptions.SelectedRows[0].Index].SpriteCheckOnName = opf.FileName;
                    CurrentControl.Options[dgvOptions.SelectedRows[0].Index].SpriteCheckOn = (Bitmap)oBmp;

                    Bitmap oPreviewOn = Utility.ResizeImage(oBmp, pbCheckOn.Width, pbCheckOn.Height);
                    pbCheckOn.Image = oPreviewOn;

                    bValid = true;
                }
            }
        }

        private void btnCheckOff_Click(object sender, EventArgs e)
        {
            bool bValid = false;

            while (!bValid)
            {
                OpenFileDialog opf = new OpenFileDialog();
                opf.Filter = "PNG files(*.png)| *.png| JPG files(*.jpg)| *.jpg";

                opf.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH);

                if (opf.ShowDialog() != DialogResult.Cancel)
                {
                    while (Path.GetDirectoryName(opf.FileName) != Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Utility.CONTROL_PATH))
                    {
                        MessageBox.Show("Please select image within 'Controls' resource folder.", "Wrong folder", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        opf.ShowDialog();
                    }

                    Image oBmp = Image.FromFile(opf.FileName, true);
                    CurrentControl.Options[dgvOptions.SelectedRows[0].Index].SpriteCheckOffName = opf.FileName;
                    CurrentControl.Options[dgvOptions.SelectedRows[0].Index].SpriteCheckOff = (Bitmap)oBmp;

                    Bitmap oPreviewOff = Utility.ResizeImage(oBmp, pbCheckOff.Width, pbCheckOff.Height);
                    pbCheckOff.Image = oPreviewOff;

                    bValid = true;
                }
            }
        }
        private void btnComboAdd_Click(object sender, EventArgs e)
        {
            int iOptionCount = 1;

            bool bValid = false;

            while(!bValid)
            {
                if (!CurrentControl.ComboDisplays.Contains("Option" + iOptionCount))
                {
                    string sValue = "Option" + iOptionCount;

                    CurrentControl.ComboDisplays.Add(sValue);
                    CurrentControl.ComboValues.Add(sValue);

                    dgvComboOptions.Rows.Add(new DataGridViewRow());
                    DataGridViewRow oRow = dgvComboOptions.Rows[dgvComboOptions.Rows.Count - 1];
                    oRow.Cells[0].Value = sValue;

                    bValid = true;
                }
                else
                    iOptionCount++;             
            }

            dgvComboOptions.Rows[dgvComboOptions.Rows.Count - 1].Selected = true;
        }

        private void btnComboRemove_Click(object sender, EventArgs e)
        {
            CurrentControl.ComboDisplays.RemoveAt(dgvComboOptions.Rows.Count - 1);
            CurrentControl.ComboValues.RemoveAt(dgvComboOptions.Rows.Count - 1);
            dgvComboOptions.Rows.RemoveAt(dgvComboOptions.Rows.Count - 1);
        }

        private void dgvComboOptions_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvComboOptions.SelectedRows.Count > 0)
            {
                txtComboDisplay.Text = CurrentControl.ComboDisplays[dgvComboOptions.SelectedRows[0].Index];
                txtComboValue.Text = CurrentControl.ComboValues[dgvComboOptions.SelectedRows[0].Index];
            }
        }
        private void txtComboDisplay_TextChanged(object sender, EventArgs e)
        {
            if (dgvComboOptions.SelectedRows.Count > 0)
            {
                CurrentControl.ComboDisplays[dgvComboOptions.SelectedRows[0].Index] = txtComboDisplay.Text;
            }
        }

        private void txtComboValue_TextChanged(object sender, EventArgs e)
        {
            if (dgvComboOptions.SelectedRows.Count > 0)
            {
                CurrentControl.ComboValues[dgvComboOptions.SelectedRows[0].Index] = txtComboValue.Text;
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(CurrentControl.SpriteBaseName))
            {
                if(CurrentControl.SpriteBase != null)
                {
                    CurrentControl.SpriteBase.Dispose();
                    CurrentControl.SpriteBase    = null;
                }

                Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteBaseName));
                CurrentControl.SpriteBase = new Bitmap(oBmp, new Size(CurrentControl.Width, CurrentControl.Height));
                oBmp.Dispose();
                oBmp = null;
            }

            if (!string.IsNullOrWhiteSpace(CurrentControl.SpriteClickName))
            {
                if (CurrentControl.SpriteClick != null)
                {
                    CurrentControl.SpriteClick.Dispose();
                    CurrentControl.SpriteClick = null;
                }

                Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteClickName));
                CurrentControl.SpriteClick = new Bitmap(oBmp, new Size(CurrentControl.Width, CurrentControl.Height));
                oBmp.Dispose();
                oBmp = null;
            }

            if (!string.IsNullOrWhiteSpace(CurrentControl.SpriteFocusName))
            {
                if (CurrentControl.SpriteFocus != null)
                {
                    CurrentControl.SpriteFocus.Dispose();
                    CurrentControl.SpriteFocus = null;
                }

                Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteFocusName));
                CurrentControl.SpriteFocus = new Bitmap(oBmp, new Size(CurrentControl.Width, CurrentControl.Height));
                oBmp.Dispose();
                oBmp = null;
            }

            if(!string.IsNullOrWhiteSpace(CurrentControl.SpriteHoverName))
            {
                if(CurrentControl.SpriteHover != null)
                {
                    CurrentControl.SpriteHover.Dispose();
                    CurrentControl.SpriteHover = null;
                }

                Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.SpriteHoverName));
                CurrentControl.SpriteHover = new Bitmap(oBmp, new Size(CurrentControl.Width, CurrentControl.Height));
                oBmp.Dispose();
                oBmp = null;
            }

            if (!string.IsNullOrWhiteSpace(CurrentControl.PercentBarName))
            {
                if (CurrentControl.PercentBar != null)
                {
                    CurrentControl.PercentBar.Dispose();
                    CurrentControl.PercentBar = null;
                }

                Bitmap oBmp = new Bitmap(Path.Combine(Utility.CONTROL_PATH, CurrentControl.PercentBarName));
                CurrentControl.PercentBar = new Bitmap(oBmp, new Size(CurrentControl.Width, CurrentControl.Height));
                oBmp.Dispose();
                oBmp = null;
            }

            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }
        #endregion
    }
}
