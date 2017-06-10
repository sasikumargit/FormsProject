using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Jeylabs.AJG.PickeringsForm.Helpers
{
    public static class WriteCustomErrorLog
    {

       public static void WriteLog(string customErrorMessage)
        {              
            var filename = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + System.DateTime.Now.ToString("dd_MM_yyyy") + "logErrors.txt";
            var sw = new System.IO.StreamWriter(filename, true);
            sw.WriteLine("Date : " + System.DateTime.Now.ToString());
            sw.WriteLine("Custom Error : " + customErrorMessage.ToString());
            sw.Close();

        }

    }
}