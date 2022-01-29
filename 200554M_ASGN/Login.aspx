<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_200554M_ASGN.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3">Email</td>
                    <td>
                        <asp:TextBox ID="tb_email" onkeyup="javascript:emailvalidate()" runat="server" TextMode="Email"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Password</td>
                    <td>
                        <asp:TextBox ID="tb_password" onkeyup="javascript:pwchecker()" runat="server" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                                <tr>
                    <td class="auto-style3">&nbsp;</td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btn_submit" />
                        <asp:Label ID="errormsg" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
