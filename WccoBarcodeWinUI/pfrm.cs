using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wccoBarcodeBL;

namespace WccoBarcodeWinUI
{
    public partial class pfrm : Form
    {
        wccoBarcodeBL.BarcodeSystem bSystem;
        pobs po = new pobs();
        List<string> lot_num = new List<string>();
        List<string> lot_qty = new List<string>();
        string check = "";
        int total = 0;

        public pfrm()
        {
            InitializeComponent();
        }


        #region setting date time at form loading
        private void pfrm_Load(object sender, EventArgs e)
        {

            textBox8.Text = DateTime.Now.ToString();
            bSystem = new BarcodeSystem();
            String message = bSystem.initializeSystem();

            if (message.Substring(0, 18) == "Connection Failed:")
            {
                MessageBox.Show(message);
                this.Close();
            }
        }
        #endregion   


        # region Employee validation
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {

         comboBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            comboBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            label7.Text = "";

            if (textBox1.Text != "")
                check = po.validate_employee(textBox1.Text);

            if (check == "valid user")
            {               
               // textBox2.Enabled = true;
               // textBox3.Enabled = true;
                comboBox1.Enabled = true;
                comboBox1.Select();
               // textBox6.Enabled = true;
               // textBox4.Select(); 
            }
            
            else if (check == "invalid user")
            {              
                textBox1.Select();
            }

            label7.Text = check;
        }
#endregion 

        # region Clear
        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;

            textBox1.Select();

            comboBox1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            label7.Text = "";

            lot_num.Clear();
            lot_qty.Clear();
        }
        #endregion

        #region Close
        private void button1_Click(object sender, EventArgs e)
        {
        
            bSystem.disconnectFromM2k();
            this.Close();
        }
        #endregion

        #region Okay
        private void button2_Click(object sender, EventArgs e)
        {

        if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != ""  && textBox6.Text != "" )
            {
                if (po.traceability(po.partnbr) == "T")
                {
                    if (textBox7.Text == "y" || textBox7.Text == "Y")
                    {
                        lot_num.Add(textBox5.Text);
                        lot_qty.Add(textBox6.Text);
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        textBox5.Select();
                    }
                    else if (textBox7.Text == "n" || textBox7 .Text == "N")
                    {
                        lot_num.Add(textBox5.Text);
                        lot_qty.Add(textBox6.Text);

                        for (int i = 0; i < lot_qty.Count(); i++)
                            Console.Write(lot_qty[i] + "  ");

                        po.update_manage();

                        button3_Click(null, null);
                        lot_num.Clear();
                        lot_qty.Clear();
                    }
                    else
                    {
                        label7.Text = "Please mention if you want to continue to enter";
                    }
                }
                else
                { 
                    lot_qty.Add (textBox6 .Text);
                    button3_Click(null, null );
                    for (int i = 0; i < lot_qty.Count(); i++)
                        Console.Write(lot_qty[i] + "  ");
                       po.update_manage();                    
                }
            }
            else 
            {
                label7.Text = "Kindly fill all the required information";
            }
        }
        #endregion

        #region receiving location validation
        private void comboBox1_Validating(object sender, CancelEventArgs e)
        {

            textBox4.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox4.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";

            if (comboBox1.Text == "RS-Door 6"||comboBox1 .Text == "RS-Door 7"||comboBox1 .Text == "Front Desk"|| comboBox1 .Text == "CSI" || comboBox1 .Text == "QLS"||comboBox1 .Text == "FTZ"|| comboBox1 .Text== "Productive Alternatives" || comboBox1 .Text == "Dakota Finishing" )
            {
                textBox4.Enabled = true;
                textBox4.Select();
            }
            else
            {
                label7.Text = "This is not a valid receiving location";
            }
        }
        #endregion

        #region Stock location validation
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            
            if (textBox4.Text != "")
                check = po.validate_receivingLocation(textBox4.Text);

            if (check == "To location found")
            {
                textBox3.Enabled = true;
                textBox3.Select();
            }
            
            label7.Text = check;
        }
        #endregion

        #region packslip number validation
        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            textBox2.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox2.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";

            if (textBox3.Text != "")
            {
                textBox2.Enabled = true;
                textBox2.Select();
            }
        }
        #endregion

        #region purchase order line number validation 
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";

            if (textBox2.Text != "")
            {
                check = po.validate_ponumber(textBox2.Text);
                label7.Text = check;

                if (check != "Invalid purchase order line number")
                {
                    check = po.traceability(po.partnbr);
                    if (check == "T")
                    {
                        textBox5.Enabled = true;
                        //textBox6.Enabled = false;
                        textBox5.Text = "";
                        textBox5.Select();
                    }
                    else 
                    {
                        //textBox5.Enabled = false;
                        textBox6.Enabled = true;
                        textBox6.Select();
                    }
                }
            }
        }
        #endregion

        #region lot number validation
        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox6.Text = "";
            textBox7.Text = "";

            if (textBox5.Text != "")
            {
                textBox6.Enabled = true;
                textBox6.Select();
            }
        }
        #endregion

        #region quantity validation
        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
             textBox7.Enabled = false;
             textBox7.Text = "";

            for (int i = 0; i < lot_qty.Count; i++)
                total += Convert.ToInt32(lot_qty[i]);



                if (textBox6.Text != "" && po.traceability(po.partnbr) == "T")
                {
                    textBox7.Enabled = true;
                    textBox7.Select();
                }
            if (textBox6.Text != "")
            {
                check = po.validate_qty(textBox2.Text, total.ToString());                
                    label7.Text = check;               
            }
        }
        #endregion

        private void pofrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bSystem.disconnectFromM2k();
        }
    }
}
