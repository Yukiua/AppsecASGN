<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verify.aspx.cs" Inherits="_200554M_ASGN.Verify" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form method="post" runat="server">
        <div>
            <h1>Verify Email</h1>
            <asp:TextBox ID="verify" runat="server" onkeydown="return (/[0-9]|\./.test(String.fromCharCode(event.keyCode)) || (event.key == 'Backspace'))"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btn_submit" />
                                    <asp:Label ID="lblmessage" runat="server" Text="&nbsp;" EnableViewState="false"></asp:Label>

        </div>
    </form>
</body>
</html>
