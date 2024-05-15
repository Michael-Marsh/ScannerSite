<%@ Page Title="Scanner Home Page" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ScannerSite.Default" %>
<asp:Content ID="HomeContent" ContentPlaceHolderID="head" runat="server">
    <title>Scanner Home Page</title>
    <meta name="keywords" content="ContiTech Scanner Home Page" />
	<meta name="description" content="ContiTech Scanner Home Page" />
	<meta http-equiv="Content-type" content="text/html; charset=iso-8859-1" />
	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR" />
	<meta content="C#" name="CODE_LANGUAGE" />
	<meta content="JavaScript" name="vs_defaultClientScript" />
	<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
	<link href="Styles.css" type="text/css" rel="stylesheet" />
	<style type="text/css">
	    TD#topnav1 A:link { BACKGROUND: #000; COLOR: #fff  }
	    TD#topnav1 A:visited { BACKGROUND: #000; COLOR: #fff }
	    TD#topnav2 A:link { BACKGROUND: #000; COLOR: #fff }
	    TD#topnav2 A:visited { BACKGROUND: #000; COLOR: #fff }
    </style>
</asp:Content>
<asp:Content ID="Home" ContentPlaceHolderID="PageContentHolder" runat="server">

    <asp:Panel ID="ProductPanel" runat="server" DefaultButton="btnSubmit">

		<table>
            <tr>
                <td class="heading">Product Inventory Move</td>
                <td style="width:50px" />
                <td>
                    <asp:Button ID="btnQuery" runat="server" Text="Query" onclick="btnQuery_Click"/>
                </td>
            </tr>
        </table>

        <table>
            <tr>
                <td><asp:Label ID="lblScanError" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td class="col1">Product ID:</td>
                <td>
                    <asp:TextBox ID="tbProductId" runat="server" OnTextChanged="tbProductId_TextChanged" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblPartNumberHeader" runat="server">Part Number:</asp:Label></td>
                <td><asp:Label ID="lblPartNumberData" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLocFromHeader" runat="server">Location From:</asp:Label></td>
                <td>
                    <asp:Label ID="lblLocFromData" runat="server"></asp:Label>
                    <asp:TextBox ID="tbLocFromData" runat="server" Width="75"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><asp:Label ID="lblLocToHeader" runat="server">Location To:</asp:Label></td>
                <td><asp:TextBox ID="tbLocToData" runat="server" Width="75"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblQtyHeader" runat="server">Quantity:</asp:Label></td>
                <td>
                    <asp:Label ID="lblQtyData" runat="server">Quantity:</asp:Label>
                    <asp:TextBox ID="tbQtyData" Width="50" runat="server"></asp:TextBox>
                    <asp:Label ID="lblUomData" Width="50" runat="server"></asp:Label>
                </td>
            </tr>
        </table>

        <table style="height: 0px; width: 200px">
            <tr>
                <td colspan="4">&nbsp;</td>
            </tr>
            <tr>
                <td style="width:10px"></td>
                <td class="style3">
                    <asp:Button ID="btnClear" runat="server" Text="Clear" Width="54px" onclick="btnClear_Click" />
                </td>
                <td style="width:50px">&nbsp;</td>
                <td class="style3">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" />
                </td>
            </tr>
            <tr>
                <td id="acceptMsg" runat="server" colspan="4"/>
            </tr>
        </table>

    </asp:Panel>

</asp:Content>
