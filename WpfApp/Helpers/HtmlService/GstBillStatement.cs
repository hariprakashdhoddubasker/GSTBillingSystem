using System.Collections.Generic;
using System.Globalization;
using System.IO;
using WpfApp.Model;

namespace WpfApp.Helpers.HtmlService
{
    public class GstBillStatement
    {
        internal static string Get(List<GstBill> gstBills)
        {
            var baseHtmlFileContent = string.Empty;

            using (StreamReader reader = new StreamReader(Constants.GstBillStatementHtmlFileName))
            {
                baseHtmlFileContent = reader.ReadToEnd();
            }
            var gstBillDetails = string.Empty;
            foreach (var gstBill in gstBills)
            {
               var gstBillDetail = Constants.GstBillDetailsTag.Replace("|GstBillDate|", gstBill.GstDate.ToString("dd/MM/yyyy"));
                gstBillDetail = gstBillDetail.Replace("|GstBillNo|", gstBill.BillSerialNumber.ToString());
                gstBillDetail = gstBillDetail.Replace("|GstBillCustomerName|", gstBill.Customer.Name);
                gstBillDetail = gstBillDetail.Replace("|GstBillAmountBeforeTax|", gstBill.AmountBeforeTax.ToString("C2", CultureInfo.CurrentCulture));
                gstBillDetail = gstBillDetail.Replace("|GstBillIGST|", gstBill.IGST.ToString("C2", CultureInfo.CurrentCulture));
                gstBillDetail = gstBillDetail.Replace("|GstBillSGST|", gstBill.SGST.ToString("C2", CultureInfo.CurrentCulture));
                gstBillDetail = gstBillDetail.Replace("|GstBillCGST|", gstBill.CGST.ToString("C2", CultureInfo.CurrentCulture));
                gstBillDetail = gstBillDetail.Replace("|GstBillAmountAfterTax|", gstBill.AmountAfterTax.ToString("C2", CultureInfo.CurrentCulture));
                gstBillDetails += gstBillDetail;
            }
            baseHtmlFileContent = baseHtmlFileContent.Replace("|GstBillDetails|", gstBillDetails);
            return baseHtmlFileContent;
        }
    }
}
