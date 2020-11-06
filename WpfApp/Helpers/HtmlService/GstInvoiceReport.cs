using System.Globalization;
using System.IO;
using WpfApp.Model;

namespace WpfApp.Helpers.HtmlService
{
    public class GstInvoiceReport
    {
        internal static string Get(Customer customer, GstBill gstBill)
        {
            var baseHtmlFileContent = string.Empty;

            using (StreamReader reader = new StreamReader(Constants.InvoiceHtmlFileName))
            {
                baseHtmlFileContent = reader.ReadToEnd();
            }

            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceSerialNo|", gstBill.BillSerialNumber);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceDate|", $"{gstBill.GstDate:dd/MM/yyyy}");
            baseHtmlFileContent = baseHtmlFileContent.Replace("|IsInvoiceTaxPayableReverse|", gstBill.IsTaxPayableReverse);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceClientName|", customer.Name);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceAddress|", customer.Address);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceState|", customer.State.StateName);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceGSTIN|", customer.GstNumber);
            var productdetails = string.Empty;

            foreach (var invoice in gstBill.Invoices)
            {
                var productdetail = Constants.ProductDetailsTag.Replace("|InvoiceProductSno|", invoice.DisplayNumber.ToString());
                productdetail = productdetail.Replace("|InvoiceProductSno|", invoice.DisplayNumber.ToString());
                productdetail = productdetail.Replace("|InvoiceProductName|", invoice.Product.ProductName);
                productdetail = productdetail.Replace("|InvoiceProductCode|", invoice.Product.HsnCode.ToString());
                productdetail = productdetail.Replace("|InvoiceProductWeight|", invoice.Weight.ToString());
                productdetail = productdetail.Replace("|InvoiceProductRate|", invoice.Rate.ToString());
                productdetail = productdetail.Replace("|InvoiceProductAmount|", invoice.AmountBeforeTax.ToString());
                productdetails += productdetail;
            }

            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceProductDetails|", productdetails);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceProductAmountInWords|", gstBill.AmountAfterTaxString);
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceTotalAmountBeforeTax|", gstBill.AmountBeforeTax.ToString("C2", CultureInfo.CurrentCulture));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceIGST|", gstBill.IGST.ToString("C2", CultureInfo.CurrentCulture));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceSGST|", gstBill.SGST.ToString("C2", CultureInfo.CurrentCulture));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceCGST|", gstBill.CGST.ToString("C2", CultureInfo.CurrentCulture));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceTotalTaxAmount|", gstBill.TotalTaxAmount.ToString("C2", CultureInfo.CurrentCulture));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceRoundOff|", gstBill.RoundOff.ToString("C2", CultureInfo.CurrentCulture));
            baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceTotalAmountAfterTax|", gstBill.AmountAfterTax.ToString("C2", CultureInfo.CurrentCulture));

            if (!string.IsNullOrEmpty(gstBill.SignatureFilePath))
            {
                baseHtmlFileContent = baseHtmlFileContent.Replace("|InvoiceSignatureData|", Utility.ImageToBase64(gstBill.SignatureFilePath));
            }


            return baseHtmlFileContent;
        }
    }
}
