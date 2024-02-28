using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WccoBarcodeWinUI;


namespace wccoBarcode
{
    public partial class mainfrm : Form
    {
        public mainfrm()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sqfrm sq = new sqfrm();
            sq.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            umfrm um = new umfrm();
            um.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pofrm po = new pofrm();
                        
            po.ShowDialog();
        }
    }
}
