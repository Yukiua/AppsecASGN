using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _200554M_ASGN
{
    public partial class Login : System.Web.UI.Page
    {
        string DBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected string getdbhash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(DBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        if(reader["PasswordHash"] !=null)
                        {
                            if(reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }
        protected string getdbsalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(DBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", email);
            try
            {
                connection.Open();
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
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
                using (WebResponse wreponse = req.GetResponse())
                {
                    using (StreamReader read = new StreamReader(wreponse.GetResponseStream()))
                    {
                        string json = read.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObj = js.Deserialize<MyObject>(json);
                        result = Convert.ToBoolean(jsonObj.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        protected void btn_submit(object sender,EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string pw = tb_password.Text.ToString().Trim();
                string email = tb_email.Text.ToString().Trim();
                if (pw != null && pw.Length > 0 && email != null && email.Length > 0)
                {
                    lblmessage.Text = "Email or Password is not valid. Please try again";
                    Response.Redirect("Login.aspx", false);
                }
                    
                SHA512Managed hash = new SHA512Managed();
                string dbhash = getdbhash(email);
                string dbsalt = getdbsalt(email);
                try
                {
                    if (dbsalt != null && dbsalt.Length > 0 && dbhash != null && dbhash.Length > 0)
                    {
                        string pwdsalt = pw + dbsalt;
                        byte[] hashsalt = hash.ComputeHash(Encoding.UTF8.GetBytes(pwdsalt));
                        string userhash = Convert.ToBase64String(hashsalt);
                        if (userhash.Equals(dbhash))
                        {
                            Session["User"] = tb_email.Text.Trim();
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Homepage.aspx", false);
                        }
                        else
                        {
                            lblmessage.Text = "Email or Password is not valid. Please try again";
                            Response.Redirect("Login.aspx", false) ;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(pw + email);
                    throw new Exception(ex.ToString());
                }
                finally { }
            }
            else
            {
                lblmessage.Text = "Wrong captcha. try again";
                lblmessage.ForeColor = Color.Red;
            }
            
        }
    }
}