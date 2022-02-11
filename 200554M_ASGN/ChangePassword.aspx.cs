using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _200554M_ASGN
{
    public partial class ChangePassword : System.Web.UI.Page
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
                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
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
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
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
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
        protected void Updatedb(string newpassword,string dbsalt,string email)
        {
            SHA512Managed hashing = new SHA512Managed();
            string pwdsalt = newpassword + dbsalt;
            byte[] plainhash = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpassword));
            byte[] hashsalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdsalt));
            string finalHash = Convert.ToBase64String(hashsalt);
            try
            {
                using(SqlConnection sql = new SqlConnection(DBConnectionString))
                {
                    using(SqlCommand cmd = new SqlCommand("UPDATE Account SET PasswordHash = @Hash WHERE Email = @Email"))
                    {
                        using(SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Hash", finalHash);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Connection = sql;
                            sql.Open();
                            cmd.ExecuteNonQuery();
                            sql.Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        protected List<string> Regexed(string oldpassword,string newpassword,string checknew)
        {
            List<string> retu = new List<string>();
            retu.Add("false");
            if (oldpassword != null)
            {
                if(!Regex.IsMatch(oldpassword,"[0-9]") && !Regex.IsMatch(oldpassword, "[A-Z]") && !Regex.IsMatch(oldpassword, "[a-z]") && !Regex.IsMatch(oldpassword, "[a-z]") && !Regex.IsMatch(oldpassword, "[^A-Za-z0-9]"))
                {
                    retu.Add("Old password is not of certain type");
                    return retu;
                }
            }
            else
            {
                retu.Add("Old Password cannot be null!");
                return retu;
            }
            if (newpassword != null)
            {
                if (!Regex.IsMatch(newpassword, "[0-9]") && !Regex.IsMatch(newpassword, "[A-Z]") && !Regex.IsMatch(newpassword, "[a-z]") && !Regex.IsMatch(newpassword, "[a-z]") && !Regex.IsMatch(newpassword, "[^A-Za-z0-9]"))
                {
                    retu.Add("New password is not of certain type");
                    return retu;
                }
            }
            else
            {
                retu.Add("New Password cannot be null!");
                return retu;
            }
            if (checknew != null)
            {
                if (!Regex.IsMatch(checknew, "[0-9]") && !Regex.IsMatch(checknew, "[A-Z]") && !Regex.IsMatch(checknew, "[a-z]") && !Regex.IsMatch(checknew, "[a-z]") && !Regex.IsMatch(checknew, "[^A-Za-z0-9]"))
                {
                    retu.Add("Repeat password is not of certain type");
                    return retu;
                }
            }
            else
            {
                retu.Add("Repeat Password cannot be null!");
                return retu;
            }
            retu.Clear();
            retu.Add("true");
            return retu;
        }
        protected void btn_submit(object sender,EventArgs e)
        {
            string oldpassword = HttpUtility.HtmlEncode(tb_olpw.Text.Trim());
            string newpassword = HttpUtility.HtmlEncode(tb_newpw.Text.Trim());
            string checknew = HttpUtility.HtmlEncode(tb_checknewpw.Text.Trim());
            if(newpassword != checknew)
            {
                errormsg.Text = "Password do not match!";
                errormsg.ForeColor = Color.Red;
            }
            else if(oldpassword == newpassword)
            {
                errormsg.Text = "Old password cannot be new password!";
                errormsg.ForeColor = Color.Red;
            }
            else
            {
                if (Regexed(oldpassword,newpassword,checknew)[0] == "true")
                {
                    string email = (string)Session["User"];
                    SHA512Managed hash = new SHA512Managed();
                    string dbhash = getdbhash(email);
                    string dbsalt = getdbsalt(email);
                    try
                    {
                        if (dbsalt != null && dbsalt.Length > 0 && dbhash != null && dbhash.Length > 0)
                        {
                            string pwdsalt = newpassword + dbsalt;
                            byte[] hashsalt = hash.ComputeHash(Encoding.UTF8.GetBytes(pwdsalt));
                            string userhash = Convert.ToBase64String(hashsalt);
                            //takes new password + salt, then hashes the whole thing, then check if the new hash doesnt match the dbhash, if so, update it
                            //DBSALT SHOULDNT CHANGE
                            if(!userhash.Equals(dbhash))
                            {
                                Updatedb(newpassword,dbsalt,email);
                                Response.Redirect("Homepage.aspx", false);
                            }
                            else
                            {
                                errormsg.Text = "Old password cannot be new password!";
                                errormsg.ForeColor = Color.Red;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally { }
                }
                else
                {
                    errormsg.Text = Regexed(oldpassword,newpassword,checknew)[1];
                    errormsg.ForeColor = Color.Red;
                }
            }

        }
        protected void Homepage(object sender,EventArgs e)
        {
            Response.Redirect("Homepage.aspx", false);
        }
    }
}