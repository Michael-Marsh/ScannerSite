using M2kClient;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Security;
using System.Web.UI;


namespace ScannerSite
{
    public partial class NonLotStockMove : Page
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
                tbPartNbr.Focus();
                acceptMsg.InnerHtml = "";
                PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
                isreadonly.SetValue(Request.QueryString, false, null);
                Request.QueryString.Clear();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = "Processing...";
            if (int.TryParse(tbQty.Text, out int _qty) && _qty > 0 && !string.IsNullOrEmpty(tbPartNbr.Text) && !string.IsNullOrEmpty(tbToLoc.Text) && !string.IsNullOrEmpty(tbFromLoc.Text))
            {
                var _uom = M2kCommand.GetUoM(tbPartNbr.Text, Main.ErpConnection);
                if (!string.IsNullOrEmpty(_uom))
                {
                    if (M2kCommand.LocationExists(tbToLoc.Text, Main.ErpConnection) && M2kCommand.LocationExists(tbFromLoc.Text, Main.ErpConnection))
                    {
                        var _result = M2kCommand.InventoryMove(Main.UserName, tbPartNbr.Text, "", _uom, tbFromLoc.Text, tbToLoc.Text, _qty, "ScanGun", "01", Main.ErpConnection);
                        if (_result.Keys.Count(o => o == 1) > 1)
                        {
                            lblMessage.Text = "Move Failed.";
                        }
                        else
                        {
                            lblMessage.Text = "Move Succeeded.";
                            tbPartNbr.Text = "";
                            tbFromLoc.Text = "";
                            tbToLoc.Text = "";
                            tbQty.Text = "";
                            tbPartNbr.Focus();
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Invalid Location Entered.";
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
            tbPartNbr.Text = "";
            tbFromLoc.Text = "";
            tbToLoc.Text = "";
            tbQty.Text = "";
            tbPartNbr.Focus();
        }

    }
}
