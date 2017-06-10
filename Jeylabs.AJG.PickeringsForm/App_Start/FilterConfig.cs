using Jeylabs.AJG.PickeringsForm.ExceptionLogHandler;
using System.Web;
using System.Web.Mvc;

namespace Jeylabs.AJG.PickeringsForm
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionHandlerAttribute());

        }
    }
}
