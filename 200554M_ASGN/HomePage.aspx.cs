﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _200554M_ASGN
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["User"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] !=null)
            {
                if(!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                lblname.Text = Session["User"].ToString();
                buttonlogout.Visible = true;
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        protected void Logout(object sender,EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Login.aspx", false);
            if(Request.Cookies["ASP.NET_SessionId"]!=null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if(Request.Cookies["AuthToken"] !=null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }
        protected void ChangePw(object sender,EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx", false);
        }
    }
}