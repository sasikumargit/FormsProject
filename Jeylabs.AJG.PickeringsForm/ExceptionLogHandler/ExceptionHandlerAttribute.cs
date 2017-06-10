using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Jeylabs.AJG.PickeringsForm.ExceptionLogHandler
{
    /// <summary>
    /// Custome exception handler class
    /// </summary>
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// On exception capture the exception content and write to error log file
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {           
                var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + System.DateTime.Now.ToString("dd_MM_yyyy") + "logErrors.txt";
                var sw = new System.IO.StreamWriter(filename, true);
                sw.WriteLine("Date : " + System.DateTime.Now.ToString());
                sw.WriteLine("Controller : " + filterContext.RouteData.Values["controller"].ToString());
                sw.WriteLine("Exception :" + filterContext.Exception.Message);
                sw.WriteLine("ExceptionStackTrace :" + filterContext.Exception.StackTrace);
                sw.WriteLine("InnerException :" + filterContext.Exception.InnerException);         
                sw.Close();
                filterContext.ExceptionHandled = true;

            }
        }
    }
}