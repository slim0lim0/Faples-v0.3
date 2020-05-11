using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaplesEditor
{
    public partial class fpxPreviewPanel : Panel
    {
        public fpxPreviewPanel()
        {
            this.AutoScroll = true;
            this.DoubleBuffered = true;
        }
    }
}
