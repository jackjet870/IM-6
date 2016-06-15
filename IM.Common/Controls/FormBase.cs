using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IM.Common.Controls
{
    public partial class FormBase : Form
    {
        public FormBase()
        {
            InitializeComponent();
            this.Load += FormBase_Load;
        }

        private void FormBase_Load(object sender, EventArgs e)
        {
            this.menuItem_Close.Click += menuItem_Close_Click;
            this.menuItem_About.Click+=menuItem_About_Click;
        }

        private void menuItem_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItem_About_Click(object sender, EventArgs e)
        {
            AboutForm _about = new AboutForm();
            _about.ShowDialog();
        }
    }
}
