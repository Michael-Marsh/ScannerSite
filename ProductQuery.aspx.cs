using M2kClient;
using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace ScannerSite
{
    public partial class ProductQuery : Page
    {
        public static string ReturnId { get; set; }
        public static string ReturnType { get; set; }

        /// <summary>
        /// Page Constructor
        /// </summary>
        /// <param name="sender">sending page</param>
        /// <param name="e">passed page arguements</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                ((Main)Master).lblName.Text = SQLCommand.GetUserName(HttpContext.Current.User.Identity.Name);
                ((Main)Master).lblSite.Text = "Wahpeton (01)";
            }
            if (!string.IsNullOrEmpty(Default.ProductNumber))
            {
                lblPartNumberData.Text = Default.ProductNumber;
                lblReturnId.Text = Default.ProductId;
                lblReturnType.Text = Default.ProductType;
                Default.ProductId = null;
                Default.ProductNumber = null;
                Default.ProductType = null;
                using (DataTable _dt = SQLCommand.GetProductTable(lblPartNumberData.Text, lblReturnType.Text))
                {
                    if (_dt == null || _dt.Rows.Count == 0)
                    {
                        //TODO: add in error handling
                    }
                    else
                    {
                        gvProduct.DataSource = _dt;
                        gvProduct.DataBind();
                    }
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ReturnId = lblReturnId.Text;
            ReturnType = lblReturnType.Text == "Lot" ? "LN" : "PN";
            Response.Redirect($"~/Default.aspx");
        }
    }
}
