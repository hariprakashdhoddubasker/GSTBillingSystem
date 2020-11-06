using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WpfApp.Helpers
{
    public class SMSData
    {
        public string UserName { get; } = "absolute";
        public string Password { get; } = "hari@123";
        public string SenderId { get; set; } = "PINGER";
        public long MobileNumber { get; set; }
        public string Message { get; set; }
    }

    public class SmsService
    {
        private readonly SMSData mySMS;

        public SmsService()
        {
            mySMS = new SMSData();
        }

        public string SendReferralMessage(string Name, long mobileNumber, string message)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Dear Hari,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"App Development referrence");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"Name : {Name}{Environment.NewLine}Mobile No : {mobileNumber}");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append($"Message : {message}");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Thank You,");
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("Nature Box");

            mySMS.Message = stringBuilder.ToString();
            mySMS.MobileNumber = 8089947074;

            return Send();
        }

        private string Send()
        {
            string result;

            //New API Link
            //var url = $"http://text.pinger.co.in/index.php/smsapi/httpapi/?uname={mySMS.UserName}&password={mySMS.Password}&sender={mySMS.SenderId}&receiver={mySMS.MobileNumber}&route=TA&msgtype=1&sms={mySMS.Message}";

            //Old API link
            string url = $"http://sms.pinger.co.in/http-api.php?username={mySMS.UserName}&password={mySMS.Password}&senderid={mySMS.SenderId}&route=1&number={mySMS.MobileNumber}&message={mySMS.Message}";

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                sr.Close();
            }
            return result;
        }
    }
}
