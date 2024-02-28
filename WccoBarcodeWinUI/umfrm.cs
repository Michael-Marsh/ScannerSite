using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wccoBarcodeBL;

namespace wccoBarcode
{
    public partial class umfrm : Form
    {        
        umbs u = new umbs();
        bool flag = false;
        wccoBarcodeBL.BarcodeSystem bSystem;

        public umfrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bSystem.disconnectFromM2k(); 
            this.Close();
        }
                
        private void button2_Click(object sender, EventArgs e)
        {
            string lotnbr = textBox6.Text + "|P";
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                u.updatemanage(textBox1.Text, textBox3.Text, textBox2.Text, textBox4.Text, textBox5.Text, lotnbr );

            else
                label8.Text = "Kindly fill all the required information";
        }

        // validate part number
        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            pictureBox11.Visible = false;
            pictureBox12.Visible = false;

            textBox2.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;

            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

            string trace = u.lot_traceability(textBox3.Text);
            string check = "";
            if (textBox3.Text != "")
            {
                check = u.validate_partnbr(textBox3.Text);
                label8.Text = check + " with traceability =   " +u.lot_traceability(textBox3.Text);
            }
            //else if (textBox3.Text == "" && flag==false  )
            //{
             //   label8.Text = "Field can't be empty";
              //  textBox3.Select();
           // }

            if (check == "part found")
            {
                pictureBox3.Visible = true;
                pictureBox4.Visible = false;
                
            }
            else if (check == "part not found" && textBox3.Text != "")
            {
                pictureBox4.Visible = true;
                pictureBox3.Visible = false;
                textBox3.Text = "";
                textBox3.Select();
            }

            if (trace == "T")
            {
                textBox6.Enabled = true;
                textBox6.Select();
            }
            else if (trace == "N")
            {
                textBox2.Enabled = true;
                textBox2.Select();
            }
        }

        //validate lot number
        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            pictureBox11.Visible = false;
            pictureBox12.Visible = false;

            textBox2.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;

            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";

            string check = "";

            if (textBox6.Text != "")
            {
                string lotnbr = textBox6.Text + "|P";
                check = u.validate_lotnbr(lotnbr , textBox3.Text);
                label8 .Text = check;
            }
         //   else if (textBox6.Text == "" && flag==false)
          //  {
           //     label8.Text = "Field can 't be empty";
            //    textBox6.Select();
           // }

            if (check == "lot found")
            {
                pictureBox5.Visible = true;
                pictureBox6.Visible = false;
                textBox2.Enabled = true;
                textBox2.Select();
            }
            else if (check == "Lot not found for this part number " && textBox6.Text != "")
            {
                pictureBox6.Visible = true;
                pictureBox5.Visible = false;
                textBox6.Text = "";
                textBox6.Select();
            }
        }

        //validate from location
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            pictureBox11.Visible = false;
            pictureBox12.Visible = false;

            textBox4.Enabled = false;
            textBox5.Enabled = false;

            textBox4.Text = "";
            textBox5.Text = "";
            
            string check="";

            if (textBox2.Text != "")
            {
                string lotnbr = textBox6.Text + "|P";
                check = u.validate_from_location(textBox2.Text, textBox3.Text, lotnbr);
                label8.Text = check;
            }
          //  else if (textBox2.Text == "")
           // {
            //    label8.Text = "Field can't be left empty";
             //   textBox2.Select();
           // }
            if (check == "valid from_location")
            {
                pictureBox7.Visible = true;
                pictureBox8.Visible = false;
                textBox4.Enabled = true;
                textBox4.Select();
            }
            else if (check == "This from_location doesn't contain this part number" && textBox2.Text != "")
            {
                pictureBox7.Visible = false;
                pictureBox8.Visible = true;
                textBox2.Text = "";
                textBox2.Select();
            }
        }

        //validate to location
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            pictureBox11.Visible = false;
            pictureBox12.Visible = false;

            textBox5.Enabled = false;

            textBox5.Text = "";

            string check="";

            if (textBox4.Text != "" && textBox4.Text != textBox2.Text)
            {
                check = u.validate_to_location(textBox4.Text);
                label8.Text = check;
            }
           // else if (textBox4.Text == "")
           // {
            //    label8.Text = "Field can't be left empty";
             //   textBox4.Select();
           // }
            if (check == "To location found" )
            {
                pictureBox9.Visible = true;
                pictureBox10.Visible = false;
                textBox5.Enabled = true;
                textBox5.Select();
            }
            else if (check == "To location not found" && textBox4.Text != "")
            {
                pictureBox10.Visible = true;
                pictureBox9.Visible = false;
                textBox4.Text = "";
                textBox4.Select();
            }
            if (textBox4.Text == textBox2.Text)
            {
                label8.Text = "sorry! from_location and to_location are the same";
            }
        }

        //validate quantity
        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            pictureBox11.Visible = false;
            pictureBox12.Visible = false;

            string check="";
            
        //    if (textBox5.Text == "")
          //  {
           //     //check = "quantity not supported";
            //    label8 .Text = "Field can't be empty";
             //   textBox5.Select();
            //    // check = u.validate_quantity("0", textBox3.Text, textBox6.Text);
          //      //Console.WriteLine(check);
           // }
            if(textBox5.Text != "") 
            {
                string lotnbr = textBox6.Text + "|P";
                check = u.validate_quantity(textBox5.Text, textBox3.Text, lotnbr);
                label8.Text = check;
            }

            if (check == "valid quantity")
            {
                pictureBox11.Visible = true;
                pictureBox12.Visible = false;                
            }
            else if (check == "quantity not supported" && textBox5.Text != "")
            {
                pictureBox11.Visible = false;
                pictureBox12.Visible = true;
                textBox5.Text = "";
                textBox5.Select();
            }
        }

       private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            pictureBox11.Visible = false;
            pictureBox12.Visible = false;

            textBox3.Enabled = false;
            textBox2.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;

            textBox3.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";

            string check = "";

            if (textBox1.Text != "")
            {
                check = u.validate_employee(textBox1.Text);
                label8.Text = check;
            }
            //else if (textBox1.Text == "" )
            //{
             //   label8.Text = "Field can't be empty";
              //  textBox1.Select();
           // }
            if (check == "valid user")
            {
                pictureBox1.Visible = false;
                pictureBox2.Visible = true;
                textBox3.Enabled = true;
                textBox3.Select();
            }
            else if (check == "invalid user" && textBox1.Text != "")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = false;
                textBox1.Text = "";
                textBox1.Select();
            }
        }

       private void button3_Click(object sender, EventArgs e)
       {
           flag = true;
           textBox1.Text = "";
           textBox3.Text = "";
           textBox2.Text = "";
           textBox4.Text = "";
           textBox5.Text = "";
           textBox6.Text = "";
           label8.Text = "";

           textBox1.Select();

           textBox3.Enabled = false;
           textBox2.Enabled = false;
           textBox4.Enabled = false;
           textBox5.Enabled = false;
           textBox6.Enabled = false;

           pictureBox1.Visible = false;
           pictureBox2.Visible = false;
           pictureBox3.Visible = false;
           pictureBox4.Visible = false;
           pictureBox5.Visible = false;
           pictureBox6.Visible = false;
           pictureBox7.Visible = false;
           pictureBox8.Visible = false;
           pictureBox9.Visible = false;
           pictureBox10.Visible = false;
           pictureBox11.Visible = false;
           pictureBox12.Visible = false;

       }

       private void umfrm_Load(object sender, EventArgs e)
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

       private void umfrm_FormClosing(object sender, FormClosingEventArgs e)
       {
           bSystem.disconnectFromM2k();
       }

      
    }
}
