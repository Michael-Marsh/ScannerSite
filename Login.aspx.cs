using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace ScannerSite
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            ((Main)Master).lblName.Text = "";
            ((Main)Master).lblSite.Text = "";
            txtUser.Focus();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!txtUser.Text.Contains("_FA"))
            {
                try
                {
                    if (Membership.ValidateUser($"{txtUser.Text}@contiwan.com", txtPassword.Text))
                    {
                        FormsAuthentication.SetAuthCookie(txtUser.Text, true);
                        FormsAuthentication.RedirectFromLoginPage(txtUser.Text, true);
                    }
                    else
                    {
                        var _user = Membership.GetUser($"{txtUser.Text}@contiwan.com");
                        if (_user == null)
                        {
                            lblLoginFailure.Visible = true;
                            lblLoginFailure.Text = "Invalid Account.";
                            lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                        }
                        else if (_user.IsLockedOut)
                        {
                            lblLoginFailure.Visible = true;
                            lblLoginFailure.Text = "Account is Locked.";
                            lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                        }
                        else if (_user.IsApproved == false)
                        {
                            lblLoginFailure.Visible = true;
                            lblLoginFailure.Text = "Account is disabled.";
                            lblLoginFailure.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            txtPassword.Text = "";
                            lblLoginFailure.Visible = true;
                            lblLoginFailure.Text = "Invalid credentials.";
                            lblLoginFailure.ForeColor = System.Drawing.Color.Red;
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
    }
}
