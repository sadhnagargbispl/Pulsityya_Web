using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
//using iTextSharp.text;

namespace Shopinv.Models
{
    public static class Extension
    {

        public static IEnumerable<T> Join<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null)
            {
                return second;
            }
            if (second == null)
            {
                return first;
            }

            return first.Concat(second).ToList();
        }

        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _Properties =
        new Dictionary<Type, ICollection<PropertyInfo>>();
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }





        public static IEnumerable<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var objType = typeof(T);
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }

                var list = new List<T>(table.Rows.Count);

                foreach (var row in table.AsEnumerable())
                {
                    var obj = new T();

                    foreach (var prop in properties)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = row[prop.Name] == null ? null : Convert.ChangeType(row[prop.Name], propType);

                            prop.SetValue(obj, safeValue, null);
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }


        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }


            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }


        public static string IEnumerableToXML<T>(this IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }


            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            MemoryStream str = new MemoryStream();
            tb.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }

        public static List<T> ToNonNullList<T>(this IEnumerable<T> obj)
        {

            return obj == null ? new List<T>() : obj.ToList();
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static string RenderRazorViewToString(ControllerContext controllerContext, string viewName, object model)
        {
            controllerContext.Controller.ViewData.Model = model;

            using (var stringWriter = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, stringWriter);
                viewResult.View.Render(viewContext, stringWriter);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return stringWriter.GetStringBuilder().ToString();
            }
        }
        public static byte[] Get_ImageByTe(string ImageName, string Linkno, int emplinkno)
        {
            System.IO.FileStream fs = null;
            BinaryReader br = null;
            byte[] imgbyte = null;
            try
            {
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/SaveFile//Images//" + ImageName.ToString().Trim() + ".JPG")))
                {
                    fs = new FileStream(HttpContext.Current.Server.MapPath("~/SaveFile//Images//" + ImageName.ToString().Trim() + ".JPG"), FileMode.Open);
                }
                else
                {
                    fs = new FileStream(HttpContext.Current.Server.MapPath("~/SaveFile//Images//NoPhoto.PNG"), FileMode.Open);
                }
                br = new BinaryReader(fs);
                imgbyte = new byte[fs.Length + 1];
                imgbyte = br.ReadBytes(Convert.ToInt32((fs.Length)));
                br.Close();
                fs.Close();
            }
            catch
            {
                br.Close();
                fs.Close();
            }
            return imgbyte;
        }

        //public static void ConvertImageToPdf(string srcFilename, string dstFilename)
        //{
        //    if (File.Exists(dstFilename))
        //    {
        //        File.Delete(dstFilename);
        //    }
        //    iTextSharp.text.Rectangle pageSize = null;

        //    using (var srcImage = new System.Drawing.Bitmap(srcFilename))
        //    {
        //        pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
        //    }
        //    using (var ms = new MemoryStream())
        //    {
        //        Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 50f, 50f, 10f, 10f);
        //        document.NewPage();
        //        iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();

        //        document.Open();
        //        var image = iTextSharp.text.Image.GetInstance(srcFilename);
        //        image.ScaleToFit(500, 700);
        //        document.Add(image);
        //        document.Close();

        //        File.WriteAllBytes(dstFilename, ms.ToArray());

        //        ms.Close();
        //        ms.Dispose();
        //    }
        //}

        public static void Create_Logs(string UserID, string IPAddress, string LoginDateTime, string LoginStatus, string LogoutDateTime, string Action, string Path, string ActionDate)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/Log")))
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Log"));
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt")))
                System.IO.File.Create(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt"));

            // Compose a string that consists of three lines.
            string lines = UserID + "|" + IPAddress + "|" + LoginDateTime + "|" + LoginStatus + "|" + LogoutDateTime + "|" + Action + "|" + Path + "|" + ActionDate;
            // Write the string to a file.
            // System.IO.StreamWriter file = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt"),);
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt")))
            {
                System.IO.FileStream f = System.IO.File.Create(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt"));
                f.Close();
            }
            //var outStream = new FileStream(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt"), FileMode.Open,FileAccess.Write, FileShare.Write);
            using (System.IO.StreamWriter sw = new StreamWriter(new FileStream(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt"), FileMode.Open, FileAccess.Write, FileShare.Write)))
            {
                sw.WriteLine(lines);
                //   sw = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("~/Log/auditlog.txt"));
                sw.WriteLine(lines);
                sw.Close();
                sw.Dispose();
            }

        }

        public static DateTime AddMonthsCustom(DateTime date, int months)
        {

            // Check if we are done quickly.
            if (months == 0)
                return date;

            // Lookup the target month and its last day.
            var targetMonth = new DateTime(date.Year, date.Month, 1).AddMonths(months);
            var lastDay = DateTime.DaysInMonth(targetMonth.Year, targetMonth.Month);

            // If we are starting out on the last day of the current month, then jump
            // to the last day of the target month.
            if (date.Day == DateTime.DaysInMonth(date.Year, date.Month))
                return new DateTime(targetMonth.Year, targetMonth.Month, lastDay);

            // If the target month cannot accomodate the current day, jump to the 
            // last day of the target month.
            if (date.Day > lastDay)
                return new DateTime(targetMonth.Year, targetMonth.Month, lastDay);

            // Simply jump to the current day in the target month.
            return new DateTime(targetMonth.Year, targetMonth.Month, date.Day);
        }
        public static int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }

        
    }
}