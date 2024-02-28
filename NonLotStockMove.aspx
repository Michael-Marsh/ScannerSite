<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="NonLotStockMove.aspx.cs" Inherits="ScannerSite.NonLotStockMove" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Non Lot Stock Move</title>
    <meta name="keywords" content="Non Lot Stock Move" />
	<meta name="description" content="Non Lot Stock Move" />
	<style type="text/css">
	  TD#topnav1 A:link { BACKGROUND: #000; COLOR: #fff  }
	  TD#topnav1 A:visited { BACKGROUND: #000; COLOR: #fff }
	  TD#topnav2 A:link { BACKGROUND: #000; COLOR: #fff }
	  TD#topnav2 A:visited { BACKGROUND: #000; COLOR: #fff }
	  .col1 { text-align: right; }
    </style>
    <script type="text/javascript">
    function clearAcceptMessage() 
    {
       document.getElementById("ctl00_ContentPlaceHolder1_acceptMsg").innerHTML = "";
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <table>
            <tr>
                <td class="heading">Non Lot Stock Move</td>
                <td><asp:Label ID="lblSpace" runat="server" Text="Empty space" Visible="false" /></td>
                <td>
                    <asp:Button ID="btnLogOff" runat="server" Text="Log Off" onclick="btnLogOff_Click"/>
                </td>
            </tr>
        </table>
<table>
    <tr><td><asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </td></tr></table>
<table>
    <tr>
        <td class="col1">Part Number:</td>
        <td onkeydown="clearAcceptMessage()">
            <asp:TextBox ID="tbPartNbr" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="col1">Location From:</td>
        <td onkeydown="clearAcceptMessage()">
            <asp:TextBox ID="tbFromLoc" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="col1">Location To:</td>
        <td onkeydown="clearAcceptMessage()">
            <asp:TextBox ID="tbToLoc" runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="col1">Quantity:</td>
        <td onkeydown="clearAcceptMessage()">
            <asp:TextBox ID="tbQty" runat="server"></asp:TextBox>
        </td>
    </tr>
</table>

<table style="height: 0px; width: 200px">
    <tr><td><asp:Label ID="lblSpace2" runat="server" Text="Empty" Width="50" Visible="false" /></td></tr>
    <tr>
        <td style="width:50px"></td>
        <td class="style3">
            <asp:Button ID="btnClear" runat="server" Text="Clear" Width="54px" onclick="btnClear_Click" />
        </td>
        <td><asp:Label ID="lblSpace3" runat="server" Text="Empty space" Width="100" Visible="false" /></td>
   <td class="style3">
       <asp:Button ID="btnSubmit" runat="server" Text="Submit" 
           onclick="btnSubmit_Click" />
    </td>

    <td style="width:50px"></td>
</tr>
<tr><td id="acceptMsg" runat="server" colspan="4"></td></tr>
<tr>
<td colspan="4">&nbsp;</td>
</tr>
</table>
</asp:Content>
