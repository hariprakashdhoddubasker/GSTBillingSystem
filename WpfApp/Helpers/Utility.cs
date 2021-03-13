using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WpfApp.Helpers.HtmlService;

namespace WpfApp.Helpers
{
    public class Utility
    {
        public static WpfAppForms GetNatureBoxForm(string formName)
        {
            WpfAppForms natureBoxForms;
            foreach (string name in Enum.GetNames(typeof(WpfAppForms)))
            {
                natureBoxForms = (WpfAppForms)Enum.Parse(typeof(WpfAppForms), name);
                if (formName == natureBoxForms.GetDescription())
                {
                    return natureBoxForms;
                }
            }
            return WpfAppForms.Invoice;
        }

        public static string GetAvailableDrivePath()
        {
            //Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            var drivePath = string.Empty;
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.Name != @"C:\")
                {
                    drivePath = drive.Name;
                    break;
                }
            }
            var wpfAppPath = drivePath + Constants.WfpAppName.Replace(" ", string.Empty) + "\\";
            Directory.CreateDirectory(wpfAppPath);
            return wpfAppPath;
        }

        public static (DateTime, DateTime) GetFinancialYear(DateTime curDate)
        {
            int CurrentYear = curDate.Year;
            int PreviousYear = curDate.Year - 1;
            int NextYear = curDate.Year + 1;
            DateTime financialYearStartDate;
            DateTime financialYearEndDate;

            int startYear;
            int endYear;
            if (curDate.Month > 3)
            {
                startYear = CurrentYear;
                endYear = NextYear;
            }
            else
            {
                startYear = PreviousYear;
                endYear = CurrentYear;
            }
            financialYearStartDate = new DateTime(startYear, 4, 1, 0, 0, 01);
            financialYearEndDate = new DateTime(endYear, 3, 31, 23, 59, 59);
            return (financialYearStartDate, financialYearEndDate);
        }

        public static void OpenFile(string fileSavePath)
        {
            Process.Start(new ProcessStartInfo(fileSavePath) { UseShellExecute = true });
        }

        public static void OpenHtmlFile(string fileSavePath)
        {
            Task.Run(() =>
            {
                ChromeDriverHelper chromeDriverService = new ChromeDriverHelper();
                chromeDriverService.OpenHtmlFile(fileSavePath);
            });
        }


        public static string ParseStringWithKeyAndPoint(string content, string key, string endPoint = "")
        {
            var value = string.Empty;
            try
            {
                if (content.IndexOf(key, StringComparison.Ordinal) == -1)
                {
                    return string.Empty;
                }

                var pointFrom = content.IndexOf(key, StringComparison.Ordinal) + key.Length;
                var actualKeyDate = content.Substring(pointFrom + 1);
                var pointTo = actualKeyDate.IndexOf(!string.IsNullOrEmpty(endPoint) ? endPoint : "\n", StringComparison.Ordinal);
                value = actualKeyDate.Substring(0, pointTo);
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
            }
            return value;
        }


        public static string GetFolderPath(string reportType)
        {
            string drivePath = Utility.GetAvailableDrivePath();
            var folderPath = drivePath + reportType;
            Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        public static string ImageToBase64(string path)
        {
            string base64String = null;
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
    }
}
