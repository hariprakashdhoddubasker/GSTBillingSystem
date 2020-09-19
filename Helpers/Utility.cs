using System.IO;

namespace RishiSilvers.Helpers
{
    public class Utility
    {
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
