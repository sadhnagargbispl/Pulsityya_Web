using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Shopinv
{
    public static class LogHelper
    {
        public static void ErrorLog(string sPathName, string sErrMsg)
        {
            if (!Directory.Exists(sPathName))
                Directory.CreateDirectory(sPathName);


            string sLogFormat = (DateTime.Now.ToShortDateString().ToString() + (" "
                        + (DateTime.Now.ToLongTimeString().ToString() + " ==> ")));
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            string sErrorTime = (sYear
                        + (sMonth + sDay));
            StreamWriter sw = new StreamWriter((sPathName + sErrorTime), true);
            sw.WriteLine((sLogFormat + sErrMsg));
            sw.Flush();
            sw.Close();
        }
    }
}