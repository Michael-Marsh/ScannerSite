<%@ Page Title="LoginPage" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ScannerSite.Login" %>
<asp:Content ID="LoginHeader" ContentPlaceHolderID="head" runat="server">
    <title>Login Page</title>
    <meta name="keywords" content="ContiTech Scanner Login Page" />
	<meta name="description" content="ContiTech Scanner Login Page" />
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
<asp:Content ID="LoginPage" ContentPlaceHolderID="PageContentHolder" runat="server">
    <div align="center">
    <div style="text-align:left">&nbsp;</div>
    <div class="heading">Handheld Scanner Login</div>

        <asp:Panel ID="btnPanel" runat="server" DefaultButton="btnLogin">

			 <table id="tbllogin" cellspacing="0" cellpadding="0" border="0" width="290" style="width: 290px; height: 136px">
                <tr>
                    <td style="height: 7px">
                        <asp:Label ID="lblUserName" runat="server" Width="72px">Username:</asp:Label></td>
                    <td style="height: 7px">
                        <asp:TextBox ID="txtUser" TabIndex="1" runat="server" Width="176px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblPassword" runat="server" Width="72px">Password:</asp:Label></td>
                    <td>
                        <asp:TextBox ID="txtPassword" TabIndex="2" runat="server" Width="176px" TextMode="Password"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnLogin" TabIndex="3" runat="server" Text="Login" OnClick="btnLogin_Click" Height="30px" Width="100px" Font-Size="Medium" Font-Bold="true"></asp:Button>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblLoginFailure" runat="server" Width="233px" Visible="false" ForeColor="Red"></asp:Label></td>
                </tr>
            </table>

        </asp:Panel>
    </div>
</asp:Content>
