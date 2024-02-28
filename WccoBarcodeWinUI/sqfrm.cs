using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wccoBarcodeBL;
using wccoBarcodeDB;

namespace wccoBarcode
{
    public partial class sqfrm : Form
    {
        wccoBarcodeBL.BarcodeSystem bSystem;

        public sqfrm()
        {
            InitializeComponent();
        }

        private void frmStockQuery_Load(object sender, EventArgs e)
        {
            
            bSystem = new BarcodeSystem();
            String message = bSystem.initializeSystem();
            try
            {
                if (message.Substring(0, 18) == "Connection Failed:")
                {
                    MessageBox.Show(message);
                    this.Close();
                }
            }
            catch (Exception ex) { }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string partNbr = textBox1.Text;

            try
            {
                label3.Text = "";



                 partInfo part = bSystem.getPart(partNbr);
                 label2.Text = part.Um;
                 label4.Text = part.Desc;
                dataGridView1.DataSource = part.Locations;

                if (label1.Text == "" || label2.Text == "")
                    label3.Text = "Info not available for blank fields";


            }
                            
            
            catch (Exception ex)
            {
                MessageBox.Show("Error!");
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            bSystem.disconnectFromM2k();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bSystem.disconnectFromM2k();
            this.Close();                       
        }
    }
}
