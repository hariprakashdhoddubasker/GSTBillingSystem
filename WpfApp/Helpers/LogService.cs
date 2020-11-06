using System;
using System.IO;

namespace WpfApp.Helpers
{
    public class LogService
    {
        private static string filePath = $"{Utility.GetAvailableDrivePath()}AppLog.txt";
        public static void LogException(Exception ex)
        {
            UIService.ShowMessage(ex.Message);
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();

                while (ex != null)
                {
                    writer.WriteLine(ex.GetType().FullName);
                    writer.WriteLine("Message : " + ex.Message);
                    writer.WriteLine("StackTrace : " + ex.StackTrace);

                    ex = ex.InnerException;
                }
            }
        }

        public static void LogMessage(string message)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("-----------------------------------------------------------------------------");
                writer.WriteLine("Date : " + DateTime.Now.ToString());
                writer.WriteLine();
                writer.WriteLine("Message : " + message);
            }
        }
    }
}
