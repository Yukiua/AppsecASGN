<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_200554M_ASGN.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="fname" runat="server" Text="First Name"></asp:Label>
            <asp:TextBox ID="tfname" runat="server"></asp:TextBox>
            <asp:Label ID="lname" runat="server" Text="Last Name"></asp:Label>
            <asp:TextBox ID="tlname" runat="server"></asp:TextBox>
            <asp:Label ID="photo" runat="server" Text="Photo"></asp:Label>
            <asp:TextBox ID="tphoto" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="password" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="tpass" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="email" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="temail" runat="server"></asp:TextBox>
            <asp:Label ID="dob" runat="server" Text="Date of Birth"></asp:Label>
            <asp:TextBox ID="tdob" runat="server"></asp:TextBox>
       <br />
            <asp:Label ID="ccn" runat="server" Text="Credit Card Number"></asp:Label>
            <asp:TextBox ID="tccn" runat="server"></asp:TextBox>
            <asp:Label ID="exp" runat="server" Text="Expiry"></asp:Label> 
            <asp:TextBox ID="texp" runat="server"></asp:TextBox>
            <asp:Label ID="cvv" runat="server" Text="CVV"></asp:Label> 
            <asp:TextBox ID="tcvv" runat="server"></asp:TextBox>

        </div>
    </form>
</body>
</html>
