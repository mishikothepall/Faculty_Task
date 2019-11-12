using Faculty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faculty.Filters
{
    public class ExceptionLogAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string name = "Guest";

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                name = filterContext.HttpContext.User.Identity.Name;
            }
            if (!filterContext.ExceptionHandled) {
                ExceptionLogModel logger = new ExceptionLogModel() {
                    UserName = name,
                    ExceptionMessage = filterContext.Exception.Message,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    LogTime = DateTime.Now
                };
                XmlFileManager manager = new XmlFileManager();
                manager.XmlSave(logger);
            }
        }
    }
}