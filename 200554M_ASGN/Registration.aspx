<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="_200554M_ASGN.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=SITEKEY"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.maskedinput/1.4.1/jquery.maskedinput.js"></script>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_pw.ClientID%>').value;
            if (str.length < 12) {
                document.getElementById("l_pwdchecker").innerHTML = "Password Length must be at least 12 characters";
                document.getElementById("l_pwdchecker").style.color = "Red";
                return ("too short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("l_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("l_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("l_pwdchecker").innerHTML = "Password require at least 1 uppercase characeter";
                document.getElementById("l_pwdchecker").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("l_pwdchecker").innerHTML = "Password require at least 1 lowercase characeter";
                document.getElementById("l_pwdchecker").style.color = "Red";
                return ("no_lowercase");
            }
            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("l_pwdchecker").innerHTML = "Password require at least 1 special characeter";
                document.getElementById("l_pwdchecker").style.color = "Red";
                return ("no_specialchar");
            }

            document.getElementById("l_pwdchecker").innerHTML = "Good"
            document.getElementById("l_pwdchecker").style.color = "Blue";
        }
        function fnamevalidate() {
            var str1 = document.getElementById('<%=tb_fname.ClientID%>').value;
            if (str1.search(/^[A-Za-z]+$/) == -1) {
                document.getElementById("fnamechecker").innerHTML = "Name must only contain letters";
                document.getElementById("fnamechecker").style.color = "Red";
                return ("only_letters")
            }
            document.getElementById("fnamechecker").innerHTML = "Good"
            document.getElementById("fnamechecker").style.color = "Blue"
        }
        function lnamevalidate() {
            var str2 = document.getElementById('<%=tb_lname.ClientID%>').value;
            if (str2.search(/^[A-Za-z]+$/) == -1) {
                document.getElementById("lnamechecker").innerHTML = "Name must only contain letters";
                document.getElementById("lnamechecker").style.color = "Red";
                return ("only_letters")
            }
            document.getElementById("lnamechecker").innerHTML = "Good"
            document.getElementById("lnamechecker").style.color = "Blue"
        }
        function emailfilter() {
            var str = document.getElementById('<%=tb_email.ClientID%>').value;
            if (str.search(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/) == -1) {
                document.getElementById("emailchecker").innerHTML = "Invalid email format";
                document.getElementById("emailchecker").style.color = "Red"
                return ("invalid_email")
            }
            document.getElementById("emailchecker").innerHTML = "Good"
            document.getElementById("emailchecker").style.color = "Blue"

        }
        function datecheck() {
            var str = document.getElementById('<%=tb_dob.ClientID%>').value;
            if (str.search(/^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/) == -1) {
                document.getElementById("datechecker").innerHTML = "Invalid date format";
                document.getElementById("datechecker").style.color = "Red";
                return ("invalid_date")
            }
            document.getElementById("datechecker").innerHTML = "Good"
            document.getElementById("datechecker").style.color = "Blue";

        }
        function cardvalidate() {
            var str = document.getElementById('<%=tb_ccn.ClientID%>').value;
            if (str.search(/^4[0-9]{12}(?:[0-9]{3})?$/) == -1) {
                document.getElementById("ccn").innerHTML = "Invalid card format";
                document.getElementById("ccn").style.color = "Red";
                return ("invalid_card")
            }
            document.getElementById("ccn").innerHTML = "Good"
            document.getElementById("ccn").style.color = "Blue"
        }
        function expvalidate() {
            var str = document.getElementById('<%=tb_exp.ClientID%>').value;
            if (str.search(/^(0[1-9]|1[0-2])\/?([0-9]{2})$/) == -1) {
                document.getElementById("exp").innerHTML = "Invalid expiry date format";
                document.getElementById("exp").style.color = "Red";
                return ("invalid_exp")
            }
            document.getElementById("exp").innerHTML = "Good"
            document.getElementById("exp").style.color = "Blue"
        }
        function cvvvalidate() {
            var str = document.getElementById('<%=tb_cvv.ClientID%>').value;
            if (str.search(/^[0-9]{3}$/) == -1) {
                document.getElementById("cvv").innerHTML = "Invalid cvv format";
                document.getElementById("cvv").style.color = "Red";
                return ("invalid_cvv")
            }
            document.getElementById("cvv").innerHTML = "Good"
            document.getElementById("cvv").style.color = "Blue"
        }
        function filevalidation() {
            var file = document.getElementById('<%=file.ClientID%>')
            var filepath = file.value
            var allowed = /(\.jpg|\.jpeg|\.png|\.gif)$/i;
            if (!allowed.exec(filepath)) {
                document.getElementById("filecheck").innerHTML = "Invalid file type"
                document.getElementById("filecheck").style.color = "Red"
                file.value = '';
                return false;
            }
            else {
                if (file.files && file.files[0]) {
                    var read = new FileReader()
                    read.onload = function (e) {
                        document.getElementById('imagepreview').innerHTML = '<img style="width:150px;height:150px;" src="' + e.target.result + '"/>';
                    }
                    read.readAsDataURL(file.files[0])
                    document.getElementById("filecheck").innerHTML = "Good"
                    document.getElementById("filecheck").style.color = "Blue"
                }
            }
        }

        grecaptcha.ready(function () {
            grecaptcha.execute('SITEKEY', { action: 'Registration' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            })
        })
    </script>
</head>
<body>
    <asp:Label ID="pwdchecker" runat="server"></asp:Label>
    <form id="form1" runat="server">
        <div>
            <h1>Register</h1>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style3">First Name</td>
                    <td>
                        <asp:TextBox ID="tb_fname" onkeyup="fnamevalidate()" runat="server"></asp:TextBox>
                        <asp:Label ID="fnamechecker" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Last Name</td>
                    <td>
                        <asp:TextBox ID="tb_lname" onkeyup="javascript:lnamevalidate()" runat="server"></asp:TextBox>
                        <asp:Label ID="lnamechecker" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:FileUpload ID="file" onchange="filevalidation()" runat="server" />
                        <div id="imagepreview"></div>

                    </td>
                    <td>

                        <asp:Label ID="filecheck" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Password</td>
                    <td>
                        <asp:TextBox ID="tb_pw" onkeyup="javascript:validate()" OnTextChanged="TextBox1_TextChanged" TextMode="Password" runat="server"></asp:TextBox>
                        <asp:Label ID="l_pwdchecker" runat="server" Text="&nbsp;"></asp:Label>
                        <asp:Button ID="Button2" OnClick="btn_checkPasswordclick" runat="server" Text="Check Password" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Email</td>
                    <td>
                        <asp:TextBox ID="tb_email" onkeyup="javascript:emailfilter()" runat="server" TextMode="Email"></asp:TextBox>
                        <asp:Label ID="emailchecker" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Date Of Birth</td>
                    <td>
                        <asp:TextBox ID="tb_dob" onkeyup="javascript:datecheck()" runat="server" TextMode="Date"></asp:TextBox>
                        <asp:Label ID="datechecker" runat="server" Text="&nbsp;"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Credit Card Number</td>
                    <td>
                        <asp:TextBox ID="tb_ccn" onkeyup="javascript:cardvalidate()" runat="server" onkeydown="return (/[0-9]|\./.test(String.fromCharCode(event.keyCode)) || (event.key == 'Backspace'))"></asp:TextBox>
                        <asp:Label ID="ccn" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">Exipiry Date</td>
                    <td>
                        <asp:TextBox ID="tb_exp" onkeyup="javascript:expvalidate()" runat="server" onkeydown="return (/[0-9]|\./.test(String.fromCharCode(event.keyCode)) || (event.key == 'Backspace'))"></asp:TextBox>
                        <asp:Label ID="exp" runat="server" Text="&nbsp;"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">CVV</td>
                    <td>
                        <asp:TextBox ID="tb_cvv" onkeyup="javascript:cvvvalidate()" runat="server" onkeydown="return (/[0-9]|\./.test(String.fromCharCode(event.keyCode)) || (event.key == 'Backspace'))" ></asp:TextBox>
                        <asp:Label ID="cvv" runat="server" Text="&nbsp;"></asp:Label>
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
                    <td></td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="btn_submit" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
                                    <asp:Button ID="change" runat="server" Text="Back to Landing" OnClick="Landing" Visible="true" />

        </div>
    </form>
</body>

</html>
