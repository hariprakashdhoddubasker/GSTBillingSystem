using System;
using System.IO;

namespace WpfApp.Helpers.HtmlService
{
    public class LetterPadReport
    {
        public static string Get(string letterPadRtfContent)
        {
            var letterPadHtmlContent = string.Empty;
            SautinSoft.RtfToHtml r = new SautinSoft.RtfToHtml
            {
                OutputFormat = SautinSoft.RtfToHtml.eOutputFormat.HTML_5,
                Encoding = SautinSoft.RtfToHtml.eEncoding.UTF_8
            };

            try
            {
                r.OpenRtf(letterPadRtfContent);
                var filePath = Path.GetTempPath() + "Result.html";
                r.ToHtml(filePath);

                string line;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    line = reader.ReadToEnd();
                }
                File.Delete(filePath);
                letterPadHtmlContent = Utility.ParseStringWithKeyAndPoint(line, "<div>", "</div>");
            }
            catch (Exception ex)
            {
                LogService.LogException(ex);
            }
            return letterPadHtmlContent;
        }

    }
}
