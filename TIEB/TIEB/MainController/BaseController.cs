using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TIEB.MainController
{
    public class BaseController : System.Web.Mvc.Controller
    {
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserName"] == null)
            {
                filterContext.Result = new RedirectResult("~/LoginAndLogout/Login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}