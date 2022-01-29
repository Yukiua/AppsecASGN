using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
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
        protected void btn_submit(object sender,EventArgs e)
        {
            string pw = tb_password.Text.ToString().Trim();
            string email = tb_email.Text.ToString().Trim();
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
                        Response.Redirect("Homepage.aspx", false);
                    }
                    else
                    {
                        errormsg.Text = "Email or Password is not valid. Please try again";
                        Response.Redirect("Login.aspx");
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
    }
}