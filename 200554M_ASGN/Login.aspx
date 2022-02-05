<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_200554M_ASGN.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <script src="https://www.google.com/recaptcha/api.js?render=SITEKEY"></script>
<script>
    grecaptcha.ready(function () {
        grecaptcha.execute('SITEKEY', { action: 'Login' }).then(function (token) {
            document.getElementById("g-recaptcha-response").value = token;
        })
    })
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Login</h1>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3">Email</td>
                    <td>
                        <asp:TextBox ID="tb_email" onkeyup="javascript:emailfilter()" runat="server" TextMode="Email"></asp:TextBox>
                                                <asp:Label ID="emailchecker" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Password</td>
                    <td>
                        <asp:TextBox ID="tb_password" onkeyup="javascript:pwfilter()" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:Label ID="l_pwdchecker" runat="server" Text="&nbsp;"></asp:Label>

                    </td>
                </tr>
                                <tr>
                    <td></td>
                    <td>
                        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
                        <asp:Label ID="lblmessage" runat="server" Text="&nbsp;" EnableViewState="false"></asp:Label>
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
