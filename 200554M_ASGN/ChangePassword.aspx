<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="_200554M_ASGN.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script>
        function newpw() {
            var str = document.getElementById('<%=tb_newpw.ClientID%>').value;
                    if (str.length < 12) {
                        document.getElementById("newpw").innerHTML = "Password Length must be at least 12 characters";
                        document.getElementById("newpw").style.color = "Red";
                        return ("too short");
                    }
                    else if (str.search(/[0-9]/) == -1) {
                        document.getElementById("newpw").innerHTML = "Password require at least 1 number";
                        document.getElementById("newpw").style.color = "Red";
                        return ("no_number");
                    }
                    else if (str.search(/[A-Z]/) == -1) {
                        document.getElementById("newpw").innerHTML = "Password require at least 1 uppercase characeter";
                        document.getElementById("newpw").style.color = "Red";
                        return ("no_uppercase");
                    }
                    else if (str.search(/[a-z]/) == -1) {
                        document.getElementById("newpw").innerHTML = "Password require at least 1 lowercase characeter";
                        document.getElementById("newpw").style.color = "Red";
                        return ("no_lowercase");
                    }
                    else if (str.search(/[^A-Za-z0-9]/) == -1) {
                        document.getElementById("newpw").innerHTML = "Password require at least 1 special characeter";
                        document.getElementById("newpw").style.color = "Red";
                        return ("no_specialchar");
                    }

                    document.getElementById("newpw").innerHTML = "Good"
                    document.getElementById("newpw").style.color = "Blue";
        }
        function checknewpw() {
            var str1 = document.getElementById('<%=tb_checknewpw.ClientID%>').value;
            var str2 = document.getElementById('<%=tb_newpw.ClientID%>').value;
            if (str1 != str2) {
                document.getElementById("newpw").innerHTML = "Password must match";
                document.getElementById("newpw").style.color = "Red";
                return ("unmatched pw");
            }
            document.getElementById("newpw").innerHTML = "Good"
            document.getElementById("newpw").style.color = "Blue";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Login</h1>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3">Old Password</td>
                    <td>
                        <asp:TextBox ID="tb_olpw" onkeyup="javascript:oldpw()" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:Label ID="olpw" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">New Password</td>
                    <td>
                        <asp:TextBox ID="tb_newpw" onkeyup="javascript:newpw()" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:Label ID="newpw" runat="server" Text="&nbsp;"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Repeat New Password</td>
                    <td>
                        <asp:TextBox ID="tb_checknewpw" onkeyup="javascript:checknewpw()" runat ="server" TextMode="Password"></asp:TextBox>
                        <asp:Label ID="checknewpw" runat="server" Text="&nbsp;"></asp:Label>
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
