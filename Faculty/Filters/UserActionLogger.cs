using Faculty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faculty.Filters
{
    public class UserActionLogger : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            string name = "Guest";

            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
                name = filterContext.HttpContext.User.Identity.Name;
            }

            LogData data = new LogData
            {
                UserName = name,
                UserAction = request.RawUrl,
                Date = DateTime.Now
            };

            XmlFileManager xmlFileManager = new XmlFileManager();
            xmlFileManager.XmlSave(data);

            base.OnActionExecuting(filterContext);
        }
    }
}