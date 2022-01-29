using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            int scores = checkpw(tb_pw.Text);
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
                        "@PasswordSalt,@DateBirth,@Photo,@CardNum,@CardExp,@CardCvv,@Key,@IV)"))
                    {
                        using(SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", tb_email.Text.Trim());
                            cmd.Parameters.AddWithValue("@FName", tb_fname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LName", tb_lname.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateBirth", tb_dob.Text.Trim());
                            FileInfo fi = new FileInfo(file.FileName);
                            string ext = fi.Extension;
                            cmd.Parameters.AddWithValue("@Photo", file.FileName.Trim() +ext );
                            //cmd.Parameters.AddWithValue("@Nric", encryptData(tb_nric.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CardNum", encryptdata(tb_ccn.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CardExp", encryptdata(tb_exp.Text.Trim()));
                            cmd.Parameters.AddWithValue("@CardCvv", encryptdata(tb_cvv.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Key", Key);
                            cmd.Parameters.AddWithValue("@IV", IV);
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
                throw new Exception(ex.ToString());
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
        protected void btn_submit(object sender, EventArgs e)
        {
            string pwd = tb_pw.Text.ToString().Trim();
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
            Response.Redirect("Login.aspx");
        }
    }
}