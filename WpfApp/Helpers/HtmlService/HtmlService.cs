using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using WpfApp.Model;

namespace WpfApp.Helpers.HtmlService
{
    public class HtmlService
    {
        internal static string GenerateInvoice(Customer customer, GstBill gstBill)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-IN");
            string folderPath = Utility.GetFolderPath("InvoiceReport");
            string htmlReport = GstInvoiceReport.Get(customer, gstBill);

            htmlReport = InsertHeaderImages(htmlReport);

            List<string> fileCopyNames = new List<string>() { "Duplicate Copy", "Aditor Copy", "Original Copy" };
            Dictionary<string, string> fileDetails = new Dictionary<string, string>();

            foreach (var fileCopyName in fileCopyNames)
            {
                var filePath = $"{ folderPath }\\{customer.Name}_{customer.CustomerId}_{fileCopyName.Replace(" ", "_")}_{gstBill.GstDate:ddMMMyyyy_hh_mm_ss_tt}.html";
                var editedReport = htmlReport.Replace("|InvoiceFileType|", fileCopyName);

                fileDetails.Add(filePath, editedReport);
            }

            return CreateHtmlFile(fileDetails);
        }


        internal static void GenerateLetterPad(string letterPadContent, string signatureFilePath)
        {
            string folderPath = Utility.GetFolderPath("LetterPad");

            string htmlReport = LetterPadReport.Get(letterPadContent);

            var baseHtmlFileContent = string.Empty;
            using (StreamReader reader = new StreamReader(Constants.LetterPadHtmlFileName))
            {
                baseHtmlFileContent = reader.ReadToEnd();
            }
            baseHtmlFileContent = InsertHeaderImages(baseHtmlFileContent);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|LetterPadContent|", htmlReport);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|LetterPadSignature|", signatureFilePath);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|LetterPadDate|", $"{DateTime.Now:dd/MM/yyyy}");

            List<string> letterPadFileTypeNames = new List<string>() { "Worker Copy", "Original Copy" };
            Dictionary<string, string> fileDetails = new Dictionary<string, string>();

            foreach (var fileCopyName in letterPadFileTypeNames)
            {
                var filePath = $"{ folderPath }\\{fileCopyName.Replace(" ", "_")}_{DateTime.Now:ddMMMyyyy_hh_mm_ss_tt}.html";
                var editedReport = baseHtmlFileContent.Replace("|LetterPadCopyType|", fileCopyName);

                fileDetails.Add(filePath, editedReport);
            }

            CreateHtmlFile(fileDetails);
        }

        internal static void GenerateGstBillStatement(List<GstBill> gstBills)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-IN");
            string folderPath = Utility.GetFolderPath("GstBillStatement");
            string htmlReport = GstBillStatement.Get(gstBills);
            htmlReport = InsertHeaderImages(htmlReport);

            var filePath = $"{ folderPath }\\GstBillStatement_{DateTime.Now:ddMMMyyyy_hh_mm_ss_tt}.html";
            var dic = new Dictionary<string, string>();
            dic.Add(filePath, htmlReport);
            CreateHtmlFile(dic);
        }

        private static string CreateHtmlFile(Dictionary<string, string> fileDetails)
        {
            var fileSavePath = string.Empty;
            foreach (var fileInfo in fileDetails.Keys)
            {
                fileSavePath = fileInfo;
                using (FileStream fs = new FileStream(fileInfo, FileMode.Create))
                {
                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                    {
                        w.WriteLine(fileDetails[fileInfo]);
                    }
                }
            }

            Utility.OpenFile(fileSavePath);
            return fileSavePath;
        }

        private static string InsertHeaderImages(string baseHtmlFileContent)
        {
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceVinayagarImageData|", Utility.ImageToBase64(Constants.GanapathiImageFileName));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceCompanySymbolImageData|", Utility.ImageToBase64(Constants.WpfAppLogoImageFileName));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceOmImageData|", Utility.ImageToBase64(Constants.OmImageFileName));
            return baseHtmlFileContent;
        }
    }
}
