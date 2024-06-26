<%@ Page Title="Product Query" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ProductQuery.aspx.cs" Inherits="ScannerSite.ProductQuery" %>
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
	<table>
		<tr>
			<td>
				<asp:Label ID="lblPartNumberHeader" runat="server">Part Number:</asp:Label>
				<asp:Label ID="lblPartNumberData" runat="server"></asp:Label>
			</td>
			<td style="width:50px">&nbsp;</td>
			<td><asp:Button ID="Button1" runat="server" Text="Back" onclick="btnBack_Click"/></td>
		</tr>
		<tr>
			<td colspan="3"><asp:GridView ID="gvProduct" runat="server" /></td>
		</tr>
	</table>

	<asp:Label ID="lblReturnId" runat="server" Visible="false"></asp:Label>
	<asp:Label ID="lblReturnType" runat="server" Visible="false"></asp:Label>

</asp:Content>
