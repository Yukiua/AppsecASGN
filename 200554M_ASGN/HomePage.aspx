<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="_200554M_ASGN.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>This is da homepage for the selected</h1>
            <h3>hello mr <asp:Label ID="lblname" runat="server"></asp:Label></h3>
            <asp:Button ID="change" runat="server" Text="Change Password" OnClick="ChangePw" Visible="true" />
            <asp:Button ID="buttonlogout" runat="server" Text="Logout" OnClick="Logout" Visible="false" />
        </div>
    </form>
</body>
</html>
