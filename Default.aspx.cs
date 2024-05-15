using M2kClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace ScannerSite
{
    public partial class Default : Page
    {
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
                ResetScreenFull();
            }
        }

        /// <summary>
        /// Reset the current page layout to base default
        /// </summary>
        private void ResetScreenBase()
        {
            lblScanError.Text = "";
            lblPartNumberHeader.Visible = false;
            lblPartNumberData.Text = "";
            lblLocFromHeader.Visible = false;
            lblLocFromData.Text = "";
            tbLocFromData.Visible = false;
            lblLocToHeader.Visible = false;
            tbLocToData.Visible = false;
            lblQtyHeader.Visible = false;
            tbQtyData.Visible = false;
            lblQtyData.Text = "";
            lblUomData.Text = "";
            btnQuery.Visible = false;
            btnSubmit.Visible = false;
        }

        /// <summary>
        /// Full reset of the current page layout to default
        /// </summary>
        private void ResetScreenFull()
        {
            lblScanError.Text = "";
            tbProductId.Text = "";
            lblPartNumberHeader.Visible = false;
            lblPartNumberData.Text = "";
            lblLocFromHeader.Visible = false;
            lblLocFromData.Text = "";
            tbLocFromData.Visible = false;
            lblLocToHeader.Visible = false;
            tbLocToData.Visible = false;
            lblQtyHeader.Visible = false;
            tbQtyData.Visible = false;
            lblQtyData.Text = "";
            lblUomData.Text = "";
            btnQuery.Visible = false;
            btnSubmit.Visible = false;
            tbProductId.Focus();
        }

        /// <summary>
        /// Load the page after successful product has been entered
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productType"></param>
        public void ProductPageLoad(string productId, string productType)
        {
            switch (productType)
            {
                case "PN":
                    tbProductId.Text = productId;
                    lblLocFromHeader.Visible = true;
                    tbLocFromData.Visible = true;
                    tbLocFromData.Text = "";
                    lblLocToHeader.Visible = true;
                    tbLocToData.Visible = true;
                    tbLocToData.Text = "";
                    lblQtyHeader.Visible = true;
                    tbQtyData.Visible = true;
                    tbQtyData.Text = "";
                    lblUomData.Text = SQLCommand.GetUom(productId);
                    btnQuery.Visible = true;
                    btnSubmit.Visible = true;
                    tbLocFromData.Focus();
                    break;
                case "LN":
                    var _productData = SQLCommand.GetProductByLot(productId);
                    if (_productData.Count > 1)
                    {
                        lblPartNumberHeader.Visible = true;
                        lblPartNumberData.Text = _productData[0];
                        lblLocFromHeader.Visible = true;
                        lblLocFromData.Text = _productData[1];
                        lblLocToHeader.Visible = true;
                        tbLocToData.Visible = true;
                        tbLocToData.Text = "";
                        lblQtyHeader.Visible = true;
                        lblQtyData.Text = _productData[2];
                        lblUomData.Text = _productData[3];
                        btnQuery.Visible = true;
                        btnSubmit.Visible = true;
                        tbLocToData.Focus();
                    }
                    else if (_productData.Count == 1)
                    {
                        lblScanError.Text = _productData[0];
                    }
                    else
                    {
                        lblScanError.Text = "Unknown error.";
                    }
                    tbProductId.Text = productId;
                    break;
            }
        }

        /// <summary>
        /// Validate all user input for any errors
        /// </summary>
        /// <returns>validation as the key and the error in the value</returns>
        public IReadOnlyDictionary<bool, string> ValidateUserInput()
        {
            var _rtnDict = new Dictionary<bool, string>();

            if(string.IsNullOrEmpty(tbLocToData.Text))
            {
                _rtnDict.Add(false, "LT");
                return _rtnDict;
            }

            switch (lblPartNumberHeader.Visible)
            {
                case true:
                    if (!SQLCommand.ValidateLocation(tbLocToData.Text.ToUpper()))
                    {
                        _rtnDict.Add(false, "NT");
                    }
                    else
                    {
                        tbLocToData.Text = tbLocToData.Text.ToUpper();
                        _rtnDict.Add(true, "");
                    }
                    break;
                case false:
                    if (string.IsNullOrEmpty(tbLocFromData.Text))
                    {
                        _rtnDict.Add(false, "LF");
                    }
                    else if (string.IsNullOrEmpty(tbQtyData.Text))
                    {
                        _rtnDict.Add(false, "QT");
                    }
                    else if (!SQLCommand.ValidateLocation(tbLocFromData.Text.ToUpper()))
                    {
                        _rtnDict.Add(false, "NF");
                    }
                    else if (!SQLCommand.ValidateLocation(tbLocToData.Text.ToUpper()))
                    {
                        _rtnDict.Add(false, "NT");
                    }
                    else if (tbLocFromData.Text == tbLocToData.Text)
                    {
                        _rtnDict.Add(false, "ML");
                    }
                    else
                    {
                        if (int.TryParse(tbQtyData.Text, out int i))
                        {
                            var _qtyValid = SQLCommand.ValidatePartQuantity(tbProductId.Text, tbLocFromData.Text.ToUpper(), i);
                            if (!_qtyValid.FirstOrDefault().Key)
                            {
                                _rtnDict.Add(false, _qtyValid.FirstOrDefault().Value);
                            }
                            else
                            {
                                tbLocFromData.Text = tbLocFromData.Text.ToUpper();
                                tbLocToData.Text = tbLocToData.Text.ToUpper();
                                _rtnDict.Add(true, "");
                            }
                        }
                        else
                        {
                            _rtnDict.Add(false, "NQ");
                        }
                    }
                    break;
            }

            return _rtnDict;
        }

        #region Page Interaction Event Code

        protected void tbProductId_TextChanged(object sender, EventArgs e)
        {
            ResetScreenBase();
            var _productId = tbProductId.Text.Contains("|") ? tbProductId.Text : $"{tbProductId.Text}|01";
            var _rDict = SQLCommand.ProductIdExists(_productId);
            if (_rDict.FirstOrDefault().Key)
            {
                var _productType = _rDict.FirstOrDefault().Value;
                switch (_productType)
                {
                    case "PN":
                        tbProductId.Text = _productId;
                        break;
                    case "LN":
                        var _productSplit = _productId.Split('|');
                        _productId = $"{_productSplit[0]}|P|{_productSplit[1]}";
                        tbProductId.Text = _productId;
                        break;
                }
                ProductPageLoad(_productId, _productType);
            }
            else
            {
                lblScanError.Text = _rDict.FirstOrDefault().Value;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var _validationResult = ValidateUserInput();
            if (_validationResult.FirstOrDefault().Key)
            {
                if (lblPartNumberHeader.Visible)
                {
                    var _qty = int.Parse(lblQtyData.Text);
                    var _part = lblPartNumberData.Text.Split('|');
                    var _lot = tbProductId.Text.Split('|');
                    M2kCommand.InventoryMove(((Main)Master).lblName.Text, _part[0], _lot[0], lblUomData.Text, lblLocFromData.Text, tbLocToData.Text, _qty, "", "01", new M2kConnection("WAXAS001", "uig72089", "vQYRZ2s54q", Database.CONTI, 1));
                }
                else
                {
                    var _qty = int.Parse(tbQtyData.Text);
                    var _part = tbProductId.Text.Split('|');
                    M2kCommand.InventoryMove(((Main)Master).lblName.Text, _part[0], "", lblUomData.Text, tbLocFromData.Text, tbLocToData.Text, _qty, "", "01", new M2kConnection("WAXAS001", "uig72089", "vQYRZ2s54q", Database.CONTI, 1));
                }
                lblScanError.Text = "Successful move.";
                tbProductId.Text = "";
                tbProductId.Focus();
            }
            else
            {
                var _error = "";
                switch (_validationResult.FirstOrDefault().Value)
                {
                    case "LT":
                        _error = "Where is the material going?";
                        tbLocToData.Focus();
                        break;
                    case "LF":
                        _error = "Where is the material?";
                        tbLocFromData.Focus();
                        break;
                    case "QT":
                        _error = "How much are you moving?";
                        tbQtyData.Focus();
                        break;
                    case "NF":
                        _error = "Invalid from location.";
                        tbLocFromData.Text = "";
                        tbLocFromData.Focus();
                        break;
                    case "NT":
                        _error = "Invalid to location.";
                        tbLocToData.Text = "";
                        tbLocToData.Focus();
                        break;
                    case "ML":
                        _error = "Duplicate locations entered.";
                        tbLocFromData.Text = "";
                        tbLocToData.Text = "";
                        tbLocFromData.Focus();
                        break;
                    case "NQ":
                        _error = "Invalid quantity.";
                        tbQtyData.Text = "";
                        tbQtyData.Focus();
                        break;
                    default:
                        _error = _validationResult.FirstOrDefault().Value;
                        break;
                }
                lblScanError.Text = _error;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ResetScreenFull();
        }

        #endregion
    }
}
