using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Script.Serialization;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Web;

namespace _200554M_ASGN
{
    public partial class Registration : System.Web.UI.Page
    {
            string DBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            static string finalHash;
            static string salt;
            byte[] Key;
            byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void TextBox1_TextChanged(object sender,EventArgs e)
        {

        }
        protected void btn_checkPasswordclick(object sender, EventArgs e)
        {
            int scores = checkpw(HttpUtility.HtmlEncode(tb_pw.Text));
            string status = "";
            switch(scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Very Strong";
                    break;
                default:
                    break;
            }
            l_pwdchecker.Text = "Status: " + status;
            if(scores<4) { l_pwdchecker.ForeColor = Color.Red;return; }
            l_pwdchecker.ForeColor = Color.Green;
        }
        private int checkpw(string password)
        {
            int score = 0;
            if (password.Length < 12)
            {
                return 1;
            }
            else score = 1;
            if(Regex.IsMatch(password,"[a-z]"))
            {
                score++;
            }
            if(Regex.IsMatch(password,"[A-Z]"))
            {
                score++;
            }
            if(Regex.IsMatch(password,"[0-9]"))
            {
                score++;
            }
            if(Regex.IsMatch(password,"[!@#$%^&*]"))
            {
                score++;
            }return score;
        }
        protected void createAccount()
        {
            try
            {
                using(SqlConnection con = new SqlConnection(DBConnectionString))
                {
                    using(SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email,@FName,@LName,@PasswordHash," +
                        "@PasswordSalt,@DateBirth,@Photo,@CardNum,@CardExp,@CardCvv,@Key,@IV,@RetryAttempts,@IsLocked,@LockedDateTime)"))
                    {
                        using(SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", HttpUtility.HtmlEncode(tb_email.Text.Trim()));
                            cmd.Parameters.AddWithValue("@FName", HttpUtility.HtmlEncode(tb_fname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@LName", HttpUtility.HtmlEncode(tb_lname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateBirth", HttpUtility.HtmlEncode(tb_dob.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Photo", file.FileName);
                            //cmd.Parameters.AddWithValue("@Nric", encryptData(tb_nric.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CardNum", encryptdata(HttpUtility.HtmlEncode(tb_ccn.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardExp", encryptdata(HttpUtility.HtmlEncode(tb_exp.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardCvv", encryptdata(HttpUtility.HtmlEncode(tb_cvv.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Key", Key);
                            cmd.Parameters.AddWithValue("@IV", IV);
                            cmd.Parameters.AddWithValue("@RetryAttempts", 3);
                            cmd.Parameters.AddWithValue("@IsLocked", false);
                            cmd.Parameters.AddWithValue("@LockedDateTime", DateTime.Now);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        protected byte[] encryptdata(string data)
        {
            byte[] ciphertext = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encrypt = cipher.CreateEncryptor();
                byte[] plaintext = Encoding.UTF8.GetBytes(data);
                ciphertext = encrypt.TransformFinalBlock(plaintext, 0, plaintext.Length);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return ciphertext;
        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=SECRETKEY &response=" + captchaResponse);
            try
            {
                using(WebResponse wreponse = req.GetResponse())
                {
                    using(StreamReader read = new StreamReader(wreponse.GetResponseStream()))
                    {
                        string json = read.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObj = js.Deserialize<MyObject>(json);
                        result = Convert.ToBoolean(jsonObj.success);
                    }
                }
                return result;
            }
            catch(WebException ex)
            {
                throw ex;
            }
        }
        private List<string> Servervalidate()
        {
            List<string> smt = new List<string>();
            smt.Add("false");
            if (HttpUtility.HtmlEncode(tb_pw.Text.Trim()) != null && HttpUtility.HtmlEncode(tb_fname.Text.Trim()) != null && HttpUtility.HtmlEncode(tb_lname.Text.Trim()) != null && HttpUtility.HtmlEncode(tb_email.Text.Trim()) != null && HttpUtility.HtmlEncode(tb_dob.Text.Trim()) != null && HttpUtility.HtmlEncode(tb_ccn.Text.Trim()) != null && HttpUtility.HtmlEncode(tb_exp.Text.Trim()) != null && HttpUtility.HtmlEncode(tb_cvv.Text.Trim()) != null)
            {
                if (HttpUtility.HtmlEncode(tb_pw.Text.Trim()).Length < 12 && !Regex.IsMatch(HttpUtility.HtmlEncode(tb_pw.Text.Trim()), "[0-9]") && !Regex.IsMatch(HttpUtility.HtmlEncode(tb_pw.Text.Trim()), "[A-Z]") && !Regex.IsMatch(HttpUtility.HtmlEncode(tb_pw.Text.Trim()), "[a-z]") && !Regex.IsMatch(HttpUtility.HtmlEncode(tb_pw.Text.Trim()), "[a-z]") && !Regex.IsMatch(HttpUtility.HtmlEncode(tb_pw.Text.Trim()), "[^A-Za-z0-9]"))
                {
                    smt.Add( "pw");
                    return smt;
                }
                else if (!Regex.IsMatch(HttpUtility.HtmlEncode(tb_fname.Text.Trim()), "^[A-Za-z]+$"))
                {
                    smt.Add( "fname");
                    return smt;
                }
                else if (!Regex.IsMatch(HttpUtility.HtmlEncode(tb_lname.Text.Trim()), "^[A-Za-z]+$"))
                {
                    smt.Add( "lname");
                    return smt;
                }
                else if (!Regex.IsMatch(HttpUtility.HtmlEncode(tb_email.Text.Trim()), @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$"))
                {
                    smt.Add( "email");
                    return smt;
                }
                else if (!Regex.IsMatch(HttpUtility.HtmlEncode(tb_dob.Text.Trim()), @"^(19|20)\d\d[- \/.](0[1-9]|1[012])[- \/.](0[1-9]|[12][0-9]|3[01])$"))
                {
                    smt.Add(tb_dob.Text);
                    return smt;
                }
                else if (!Regex.IsMatch(HttpUtility.HtmlEncode(tb_ccn.Text.Trim()), @"^4[0-9]{12}(?:[0-9]{3})?$"))
                {
                    smt.Add( "ccn");
                    return smt;
                }
                else if (!Regex.IsMatch(HttpUtility.HtmlEncode(tb_exp.Text.Trim()), @"^(0[1-9]|1[0-2])\/?([0-9]{2})$"))
                {
                    smt.Add( "exp");
                    return smt;
                }
                else if (!Regex.IsMatch(HttpUtility.HtmlEncode(tb_cvv.Text.Trim()), @"^[0-9]{3}$"))
                {
                    smt.Add( "cvv");
                    return smt;
                }
                else
                {
                    smt.Clear();
                    smt.Add("true");
                    return smt;
                }
            }
            else
            {
                smt.Add( "null");
                return smt;
            }
        }
        static async Task Execute(int validate)
        {
            var client = new SendGridClient("SENDGRIDKEY");
            var from = new EmailAddress("setokurushi@gmail.com", "Example User");
            var subject = "Verification Code:";
            var to = new EmailAddress("setokurushi@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = string.Format("<strong>Code: {0} </strong>",validate);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
        protected void Landing(object sender,EventArgs e)
        {
            Response.Redirect("Landing.aspx", false);
        }
        protected void btn_submit(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                if(Servervalidate()[0].Equals("true"))
                {
                    if (file.HasFile)
                    {
                        string pwd = HttpUtility.HtmlEncode(tb_pw.Text.ToString()).Trim();
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltbyte = new byte[8];
                        rng.GetBytes(saltbyte);
                        salt = Convert.ToBase64String(saltbyte);
                        SHA512Managed hashing = new SHA512Managed();
                        string pwdsalt = pwd + salt;
                        byte[] plainhash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                        byte[] hashsalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdsalt));
                        finalHash = Convert.ToBase64String(hashsalt);
                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.GenerateKey();
                        Key = cipher.Key;
                        IV = cipher.IV;
                        createAccount();
                        file.SaveAs(Server.MapPath("/Uploads/" + file.FileName));
                        Random r = new Random();
                        int validate = r.Next();
                        _ = Execute(validate);
                        Session["ex"] = validate;
                        Response.Redirect("Verify.aspx",false);
                    }
                    else
                    {
                        lblmessage.Text = "No Fiie Uploaded";
                        lblmessage.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblmessage.Text = "Try Again"+Servervalidate()[1];
                    lblmessage.ForeColor = Color.Red;
                }
            }
            else
            {
                lblmessage.Text = "Wrong captcha. try again";
                lblmessage.ForeColor = Color.Red;
            }
        }
    }
}