
using System;

namespace WpfApp.Helpers
{
    public static class Constants
    {
        public const string WfpAppName = "RISHI SILVERS";

        internal static string HtmlStartTag => "<!DOCTYPE html><html>";
        internal static string HtmlEndTag => "</html>";
        internal static string HeadStartTag => "<head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">";
        internal static string HeadEndTag => "</head>";
        internal static string BodyStartTag => "<body>";
        internal static string BodyEndTag => "</body>";
        internal static string H1Tag => "<h1 id=\"heading\">{0}</h1>";
        internal static string H2Tag => "<h2>{0}</h2>";
        internal static string Break => "<br>";
        internal static string TableStartTag => "<table {0}><tbody>";
        internal static string TableHeaderTag => "<th>{1}</th>";
        internal static string TableRowStartTag => "<tr {0}>";
        internal static string TableRowEndTag => "</tr>";
        internal static string CellStartTag => "<td {0}>";
        internal static string CellEndTag => "</td>";
        internal static string CellTag => "<td {0}></td>";
        internal static string EmptyCellTag => "<td width=\"3%\">&nbsp;</td>";
        internal static string TableEndTag => "</tbody></table>";
        internal static string TableStyle => "style=\"border: 1px solid #999999;\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" ";
        internal static string DivTag => "<div {0}>{1}</div>";
        internal static string DivStartTag => "<div {0}>";
        internal static string DivEndTag => "</div>";

        internal static string SpanTag => "<span {0}>{1}</span>";
        internal static string StrongStartTag => "<strong {0}>";
        internal static string StrongEndTag => "</strong>";
        internal static string ImageTag => "<img align=\"center\" width=\"100%\" height=\"100%\" src=\"{0}\" />";

        internal static string StyleTag => "<style  type=\"text/css\">.print { width:140px; height:35px; line-height:32px; " +
            "text-align:center;  border:none;  border-radius:20px;  background:#f60;  margin-bottom:20px;" +
            "cursor:pointer; color:#fff; font-family: Helvetica, Arial, sans-serif; }" +
            ".pageText { font-family: Helvetica, Arial, sans-serif; font-size:13px; padding:5px; color:#000; }" +
            ".pageTextBold { font-family: Helvetica, Arial, sans-serif; font-size:13px; padding:5px; font-weight:800;" +
            " color:#000;}" +
            ".subHeadingText { font-family: Helvetica, Arial, sans-serif; font-size:15px; padding:5px; color:#d04e00;" +
            "font-weight:800; }" +
            " td { height: 30px; }</style>";

        internal static string ScriptTag => "<script> function printDiv() {" +
            "var mywindow = window.open();" +
            "var is_chrome = Boolean(mywindow.chrome);" +
            "var tab = document.getElementById('panel');" +
            "mywindow.document.write('<html>');" +
            "mywindow.document.write('<body><h4>|GSTInvoiceCopyType|</h4><br>'); " +
            "mywindow.document.write(tab.outerHTML);" +
            "mywindow.document.write('</body></html>');" +
            "if (is_chrome) {" +
            "    setTimeout(function () {" +
            "            mywindow.document.close();" +
            "            mywindow.focus();" +
            "            mywindow.print();" +
            "            mywindow.close();" +
            "    }, 250);   }" +
            "else {" +
            "    mywindow.document.close();" +
            "    mywindow.focus();" +
            "    mywindow.print();" +
            "    mywindow.close();}" +
            " return true;" +
            "} </script> ";
        internal static string ProductDetailsTag => "<tr class=\"pageText\"><td width=\"10%\" align=\"center\"><span>|InvoiceProductSno|</span></td><td width=\"40%\" align=\"center\"><span>|InvoiceProductName|</span></td><td width=\"12.5%\" align=\"center\"><span>|InvoiceProductCode|</span></td><td width=\"12.5%\" align=\"center\"><span>|InvoiceProductWeight|</span></td><td width=\"12.5%\" align=\"center\"><span>|InvoiceProductRate|</span></td><td width=\"12.5%\" align=\"center\"><span>|InvoiceProductAmount|</span></td></tr>";
        internal static string GstBillDetailsTag => "<tr>" +
            "<td>|GstBillDate|</td><td>|GstBillNo|</td><td>|GstBillCustomerName|</td><td>|GstBillAmountBeforeTax|</td><td>|GstBillIGST|</td><td>|GstBillCGST|</td><td>|GstBillSGST|</td><td>|GstBillAmountAfterTax|</td></tr>";

        internal static string AssertsFilePath => AppDomain.CurrentDomain.BaseDirectory + @"Assets\";

        internal static string InvoiceHtmlFileName => AssertsFilePath + "InvoiceReport.html";
        internal static string LetterPadHtmlFileName => AssertsFilePath + "LetterPad.html";
        internal static string GstBillStatementHtmlFileName => AssertsFilePath + "GstBillStatement.html";
        internal static string GanapathiImageFileName => AssertsFilePath + "Ganapathi_full.jpg";
        internal static string WpfAppLogoImageFileName => AssertsFilePath + "Company_Symbol_small_full.png";
        internal static string OmImageFileName => AssertsFilePath + "OM.jpg";
    }
}
