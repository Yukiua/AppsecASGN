using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _200554M_ASGN
{
    public partial class Verify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btn_submit(object sender,EventArgs e)
        {
            int verif = Int32.Parse(Session["ex"].ToString());
            if (verify.Text.ToString() == verif.ToString())
            {
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                lblmessage.Text = "Verification Code is wrong";
                lblmessage.ForeColor = Color.Red;
            }
        }
    }
}