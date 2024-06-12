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
        public string ProductId { get; set; }
        public string ProductType { get; set; }

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
            ProductId = Request.QueryString["RequestProduct"];
            ProductType = Request.QueryString["ProductType"] == "Lot" ? "LN" : "PN";
            var _productId = Request.QueryString["ProductId"];
            lblPartNumberData.Text = _productId;
            var _productType = Request.QueryString["ProductType"];
            using (DataTable _dt = SQLCommand.GetProductTable(_productId, _productType))
            {
                if(_dt == null || _dt.Rows.Count == 0)
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Default.aspx?ProductId={ProductId}&ProductType={ProductType}");
        }
    }
}
