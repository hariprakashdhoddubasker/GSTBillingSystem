using System;
using System.IO;

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
            var drivePath = string.Empty;
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.Name != @"C:\")
                {
                    drivePath = drive.Name;
                    break;
                }
            }
            var natureBoxPath = drivePath + "NatureBox\\";
            Directory.CreateDirectory(natureBoxPath);
            return natureBoxPath;
        }
    }
}
