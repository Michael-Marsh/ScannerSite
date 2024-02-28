using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Security;
using System.Web.UI;

namespace ScannerSite
{
    public partial class Default : Page
    {
        public static string UserName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            UserName = string.Empty;
        }
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Main master = (Main)Page.Master;
            master.lblName.Text = "";
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!txtUser.Text.Contains("_FA"))
            {
                try
                {
                    using (PrincipalContext pContext = GetPrincipal(txtUser.Text))
                    {
                        using (UserPrincipal uPrincipal = UserPrincipal.FindByIdentity(pContext, txtUser.Text))
                        {
                            using (DirectoryEntry dEntry = uPrincipal.GetUnderlyingObject() as DirectoryEntry)
                            {
                                var _expireDate = !uPrincipal.PasswordNeverExpires ? Convert.ToDateTime(dEntry.InvokeGet("PasswordExpirationDate")) : DateTime.Today.AddDays(1);
                                if (_expireDate <= DateTime.Today && _expireDate != new DateTime(1970, 1, 1))
                                {
                                    lblLoginFailure.Visible = true;
                                    lblLoginFailure.Text = "Expired Password.";
                                    lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                                }
                                else if (uPrincipal.IsAccountLockedOut())
                                {
                                    lblLoginFailure.Visible = true;
                                    lblLoginFailure.Text = "Account is Locked.";
                                    lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                                }
                                else if (uPrincipal.Enabled == false)
                                {
                                    lblLoginFailure.Visible = true;
                                    lblLoginFailure.Text = "Account is disabled.";
                                    lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                                }
                                else if (!pContext.ValidateCredentials(txtUser.Text, txtPassword.Text, ContextOptions.Negotiate))
                                {
                                    txtPassword.Text = "";
                                    lblLoginFailure.Visible = true;
                                    lblLoginFailure.Text = "Invalid credentials.";
                                    lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    Main.UserName = txtUser.Text;
                                    FormsAuthentication.RedirectFromLoginPage(txtUser.Text, false);
                                    Response.Redirect("~/LotStockMove.aspx");
                                    lblLoginFailure.ForeColor = System.Drawing.Color.Black;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    lblLoginFailure.Visible = true;
                    lblLoginFailure.Text = "Account does not exist.";
                    lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        /// <summary>
        /// Get a Dynamic PrincipalContext based on the username submitted
        /// </summary>
        /// <param name="username">User name</param>
        /// <returns>Dynamic PrincipalContext</returns>
        public static PrincipalContext GetPrincipal(string username)
        {
            if (username.Contains("\\"))
            {
                var uSplit = username.Split('\\');
                return new PrincipalContext(ContextType.Domain, uSplit[0]);
            }
            else
            {
                return new PrincipalContext(ContextType.Domain, "tiretech2.contiwan.com", $"OU=wak1,OU=us,OU=lda,DC=tiretech2,DC=contiwan,DC=com");
            }
        }
    }
}
