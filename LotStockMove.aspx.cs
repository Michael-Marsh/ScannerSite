using M2kClient;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Security;
using System.Web.UI;


namespace ScannerSite
{
    public partial class LotStockMove : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Main.UserName))
            {
                resetScreen();
                FormsAuthentication.SignOut();
                Main.UserName = string.Empty;
                Main master = (Main)Page.Master;
                master.lblName.Text = "";
                Response.Redirect("~/Default.aspx");
            }
            if (!Page.IsPostBack)
            {
                tbLotNbr.Focus();
                acceptMsg.InnerHtml = "";
                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                isreadonly.SetValue(Request.QueryString, false, null);
                Request.QueryString.Clear();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "Processing...";
            if (int.TryParse(tbQty.Text, out int _qty) && _qty > 0 && !string.IsNullOrEmpty(lblPartID.Text) && !string.IsNullOrEmpty(tbToLoc.Text))
            {
                if (M2kCommand.LocationExists(tbToLoc.Text, Main.ErpConnection))
                {
                    var _result = M2kCommand.InventoryMove(Main.UserName, lblPartID.Text, tbLotNbr.Text, lblUom.Text, lblFromLoc.Text, tbToLoc.Text, _qty, "ScanGun", "01", Main.ErpConnection);
                    if (_result.Keys.Count(o => o == 1) > 1)
                    {
                        lblMessage.Text = "Move Failed.";
                    }
                    else
                    {
                        lblMessage.Text = "Move Succeeded.";
                        lblPartID.Text = "";
                        tbLotNbr.Text = "";
                        lblFromLoc.Text = "";
                        tbToLoc.Text = "";
                        tbQty.Text = "";
                        lblUom.Text = "";
                        tbLotNbr.Focus();
                    }
                }
                else
                {
                    lblMessage.Text = "Invalid To Location.";
                }
            }
        }

        protected void tbLotNbr_TextChanged(object sender, EventArgs e)
        {
            lblMessage.Text = "";
            if (tbLotNbr.Text.Length > 5)
            {
                var _values = M2kCommand.GetLotValues(tbLotNbr.Text, Main.ErpConnection);
                if (_values.Count() > 0)
                {
                    if (_values.Count(o => o.Contains("ERROR:")) > 0)
                    {
                        lblMessage.Text = _values[0].Replace("ERROR:", "");
                    }
                    else
                    {
                        lblPartID.Text = _values[0].Replace("|01", "");
                        lblUom.Text = _values[1];
                        lblFromLoc.Text = _values[2];
                        tbQty.Text = _values[3];
                        tbToLoc.Focus();
                    }
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            resetScreen();
        }

        protected void btnLogOff_Click(object sender, EventArgs e)
        {
            resetScreen();
            FormsAuthentication.SignOut();
            Main.UserName = string.Empty;
            Main master = (Main)Page.Master;
            master.lblName.Text = "";
            Response.Redirect("~/Default.aspx");
        }

        private void resetScreen()
        {
            lblMessage.Text = "";
            lblPartID.Text = "";
            tbLotNbr.Text = "";
            lblFromLoc.Text = "";
            tbToLoc.Text = "";
            tbQty.Text = "";
            lblUom.Text = "";
            tbLotNbr.Focus();
        }

    }
}
