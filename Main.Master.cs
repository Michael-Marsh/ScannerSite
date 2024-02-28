using System;
using System.Web.UI;

namespace ScannerSite
{
    public partial class Main : MasterPage
    {
        public static M2kClient.M2kConnection ErpConnection { get; set; }
        public static string UserName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblName.Text = UserName;
            if (ErpConnection == null)
            {
                ErpConnection = new M2kClient.M2kConnection("WAXAS001", "uig72089", "vQYRZ2s54q", M2kClient.Database.CONTI, 1);
            }
            lblAccount.Text = ErpConnection.UniAccount;
        }
    }
}
