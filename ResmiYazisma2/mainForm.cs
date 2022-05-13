using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResmiYazisma2
{
    public partial class mainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnBagliKurum_ItemClick(object sender, ItemClickEventArgs e)
        {
            Form form = new Form1();
            form.MdiParent = this;
            form.Show();
        }
    }
}