using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _200554M_ASGN
{
    public partial class Landing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Register(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx");
        }
        protected void Login(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}