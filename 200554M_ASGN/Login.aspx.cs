using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
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
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=SECERTKEY &response=" + captchaResponse);
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
        protected bool Auth(string email)
        {
            SqlConnection connection = new SqlConnection(DBConnectionString);
            string sql = "select isLocked, RetryAttempts,LockedDateTime FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", email);
            try
            {
                int attempt = 3;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int retry = Convert.ToInt32(reader["RetryAttempts"].ToString());
                        if (Convert.ToBoolean(reader["isLocked"].ToString()))
                        {
                            lblmessage.Text = "Account locked. Please wait for a few minutes";
                            lblmessage.ForeColor = Color.Red;
                        }
                        else if (retry > 0)
                        {
                            attempt = retry;
                        }
                        if(retry == 0)
                        {
                            try
                            {
                                using (SqlConnection sqlb = new SqlConnection(DBConnectionString))
                                {
                                    using (SqlCommand cmd = new SqlCommand("UPDATE Account SET isLocked = @Lock, LockedDateTime = @LockTime WHERE Email = @Email"))
                                    {
                                        using (SqlDataAdapter sda = new SqlDataAdapter())
                                        {
                                            cmd.CommandType = CommandType.Text;
                                            cmd.Parameters.AddWithValue("@Lock", true);
                                            cmd.Parameters.AddWithValue("@LockTime", DateTime.Now.AddMinutes(1));
                                            cmd.Parameters.AddWithValue("@Email", email);
                                            cmd.Connection = sqlb;
                                            sqlb.Open();
                                            cmd.ExecuteNonQuery();
                                            sqlb.Close();
                                        }
                                    }
                                }
                                
                                return false;
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                            }
                        }
                    }
                    reader.Close();
                }
                connection.Close();
                try
                {
                    using (SqlConnection sqla = new SqlConnection(DBConnectionString))
                    {
                        using (SqlCommand cmda = new SqlCommand("UPDATE Account SET RetryAttempts = RetryAttempts - 1 WHERE Email = @Email"))
                        {
                            using (SqlDataAdapter sdaa = new SqlDataAdapter())
                            {
                                cmda.CommandType = CommandType.Text;
                                cmda.Parameters.AddWithValue("@Email", email);
                                cmda.Connection = sqla;
                                sqla.Open();
                                cmda.ExecuteNonQuery();
                                sqla.Close();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected void Landing(object sender,EventArgs e)
        {
            Response.Redirect("Landing.aspx", false);
        }
        public bool time()
        {
            if(Session["Time"] != null)
            {
                if(DateTime.Parse(Session["Time"].ToString()) < DateTime.Now)
                        {
                    return false;
                }
                else { return true; }
            }
            return true;
        }
        protected void btn_submit(object sender,EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string pw = HttpUtility.HtmlEncode(tb_password.Text.ToString().Trim());
                string email = HttpUtility.HtmlEncode(tb_email.Text.ToString().Trim());
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
                        if (userhash.Equals(dbhash) && time())
                        {
                            Session["User"] = tb_email.Text.Trim();
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Homepage.aspx", false);
                        }
                        else
                        {
                            if (Session["Retry"]==null) Session["Retry"] = "1";
                            if (Session["Retry"].ToString() != "111")
                            {
                                lblmessage.Text = "Email or Password is wrong. Please try again.";
                                lblmessage.ForeColor = Color.Red;
                                Session["Retry"] += "1";
                            }
                            else
                            {
                            lblmessage.Text = "Account is locked. PLease wait a minute";
                            lblmessage.ForeColor = Color.Red;
                                if (Session["Time"] == null)
                                {
                                Session["Time"] = DateTime.Now.AddSeconds(15);
                                }
                            }

                        }

                    }
                    else
                    {
                        lblmessage.Text = "uhhh";
                        lblmessage.ForeColor = Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(pw + email);
                    throw new Exception(ex.ToString());
                }
                finally {  }
            }
            else
            {
                lblmessage.Text = "Wrong captcha. try again";
                lblmessage.ForeColor = Color.Red;
            }
            
        }
    }
}