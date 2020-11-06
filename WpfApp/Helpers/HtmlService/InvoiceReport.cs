using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using WpfApp.Model;

namespace WpfApp.Helpers.HtmlService
{
    internal class InvoiceReport
    {
        private static GstBill myGstBill = new GstBill();
        private static Customer myCustomer = new Customer();

        internal static string Get(Customer customer, GstBill gstBill)
        {
            myCustomer = customer;
            myGstBill = gstBill;

            StringBuilder stringBuilder = new StringBuilder();

            GeneratePageStart(stringBuilder);

            GenerateHeaderRow(stringBuilder);

            GenerateBlankRow(stringBuilder);

            GenerateReverseTaxRow(stringBuilder);

            GenerateInvoiceRow(stringBuilder);

            GenerateBlankRow(stringBuilder);

            GenerateClientDetails(stringBuilder);

            GenerateBlankRow(stringBuilder);

            GenerateGstCalculations(stringBuilder);

            GenerateBlankRow(stringBuilder);

            GeneratePoliciesRow(stringBuilder);

            GenerateBlankRow(stringBuilder);

            GeneratePageEndTags(stringBuilder);

            return stringBuilder.ToString();
        }

        private static void GeneratePoliciesRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\"");

            stringBuilder.AppendFormat(Constants.TableStartTag, "style=\"border: 1px solid #999999;\" width=\"100%\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\"");
            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"75%\" style=\"padding-Left:10px;\" align=\"Left\" rowspan=\"2\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, "style=\"color:#d04e00;\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Terms and Conditions for Sales");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.AppendFormat(Constants.SpanTag, "class=\"pageText\"", "<p>" +
                "1. Subject to Salem Jurisdiction<br>" +
                "2. Goods once sold cannot be taken back<br>" +
                "3. Payment of the invoice can be made be cheque / NEFT / RTGS / Cash upto 10,000<br>" +
                "4. As per Serial No 4 & 5 of Annexure to Rule No 138(14) of CGST Rules, 2017 E way bill not required to be generated for items included in this invoice.</p>");
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" valign=\"top\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, "class=\"pageText\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "For RISHI SILVERS");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.CellStartTag, "valign=\"bottom\" style=\"border-top: none; padding:5px; height:100px !important;\"");
            stringBuilder.AppendFormat(Constants.ImageTag, myGstBill.SignatureFilePath);
            stringBuilder.AppendFormat(Constants.StrongStartTag, "class=\"pageText\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Authorised Signature");
            stringBuilder.Append(Constants.StrongEndTag);
          
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.Append(Constants.TableEndTag);

            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GeneratePageStart(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Constants.HtmlStartTag);
            stringBuilder.Append(Constants.HeadStartTag);
            stringBuilder.Append(Constants.StyleTag);
            stringBuilder.Append(Constants.ScriptTag);
            stringBuilder.Append(Constants.HeadEndTag);

            stringBuilder.Append(Constants.BodyStartTag);
            stringBuilder.AppendFormat(Constants.DivTag, "onclick=\"printDiv()\" class=\"print\"", "Print Invoice");
            stringBuilder.AppendFormat(Constants.DivStartTag, "id=\"panel\"");

            stringBuilder.AppendFormat(Constants.TableStartTag, Constants.TableStyle + "class=\"tb\"");
        }

        private static void GenerateGstCalculations(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\" align=\"right\" class=\"pageText\" width=\"30%\"");

            stringBuilder.AppendFormat(Constants.TableStartTag, "style=\"border: 1px solid #999999;\" width=\"100%\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\"");

            GenerateGstHeaderRow(stringBuilder);
            GenerateProductDetailsRow(stringBuilder);
            GenerateGstAmountInWords(stringBuilder);
            GenerateIgstRow(stringBuilder);


            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");
            GenerateBankDetails(stringBuilder);
            GenerateStateGst(stringBuilder);
            stringBuilder.Append(Constants.TableRowEndTag);

            GenerateTotalAmounts(stringBuilder);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.Append(Constants.TableEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateTotalAmounts(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-Left: 10px;\" align=\"Left\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Total Tax Amount");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-right: 10px;\" align=\"right\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.TotalTaxAmount);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-Left: 10px;\" align=\"Left\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Round Off");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-right: 10px;\" align=\"right\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.RoundOff);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-Left: 10px;\" align=\"Left\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Total Amount after Tax");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-right: 10px;\" align=\"right\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.AmountAfterTax);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateStateGst(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-Left: 10px;\" align=\"Left\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "SGST : 1.5 %");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-right: 10px;\" align=\"right\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.SGST);
            stringBuilder.Append(Constants.CellEndTag);
            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");
            //GenerateBankDetails(stringBuilder);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-Left: 10px;\" align=\"Left\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "CGST : 1.5 %");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-right: 10px;\" align=\"right\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.CGST);
            stringBuilder.Append(Constants.CellEndTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateBankDetails(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"50%\" rowspan=\"5\" colspan=\"2\" align=\"center\"");

            stringBuilder.AppendFormat(Constants.TableStartTag, "width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"padding-left: 10px;\"");

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"35%\" align=\"left\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "<u>Bank Details</u>");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);
            stringBuilder.Append("<td width=\"50%\">&nbsp;</td>");
            stringBuilder.Append(Constants.TableRowEndTag);


            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Name");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "HDFC Bank");
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);


            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Branch");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Shevapet");
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "A/c No");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "50200025339691");
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "IFSC");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "HDFC 0003675");
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);
            stringBuilder.Append(Constants.TableEndTag);
            stringBuilder.Append(Constants.CellEndTag);
        }

        private static void GenerateIgstRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" align=\"Left\" style=\"padding-Left: 10px;\" colspan=\"2\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "IGST : 3%");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-right: 10px;\" align=\"right\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.IGST);
            stringBuilder.Append(Constants.CellEndTag);
        }

        private static void GenerateGstAmountInWords(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"50%\" align=\"center\" rowspan=\"2\" colspan=\"2\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Rupees : " + myGstBill.AmountAfterTaxString);
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\" colspan=\"2\" style=\"padding-Left: 10px;\" align=\"Left\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Total Amount Before Tax");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\"  colspan=\"2\" style=\"padding-Right: 10px;\" align=\"Right\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.AmountBeforeTax);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateProductDetailsRow(StringBuilder stringBuilder)
        {
            foreach (var invoice in myGstBill.Invoices)
            {
                stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"pageText\"");

                stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"10%\" align=\"center\"");
                stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
                stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, invoice.DisplayNumber);
                stringBuilder.Append(Constants.StrongEndTag);
                stringBuilder.Append(Constants.CellEndTag);

                stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"40%\" align=\"center\"");
                stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
                stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, invoice.Product.ProductName);
                stringBuilder.Append(Constants.StrongEndTag);
                stringBuilder.Append(Constants.CellEndTag);

                stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
                stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
                stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, invoice.Product.HsnCode);
                stringBuilder.Append(Constants.StrongEndTag);
                stringBuilder.Append(Constants.CellEndTag);

                stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
                stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
                stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, invoice.Weight);
                stringBuilder.Append(Constants.StrongEndTag);
                stringBuilder.Append(Constants.CellEndTag);

                stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
                stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
                stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, invoice.Rate);
                stringBuilder.Append(Constants.StrongEndTag);
                stringBuilder.Append(Constants.CellEndTag);

                stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
                stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
                stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, invoice.AmountBeforeTax);
                stringBuilder.Append(Constants.StrongEndTag);
                stringBuilder.Append(Constants.CellEndTag);

                stringBuilder.Append(Constants.TableRowEndTag);
            }
        }

        private static void GenerateGstHeaderRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, "class=\"subHeadingText\"");

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"10%\" align=\"center\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "S.No");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"40%\" align=\"center\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Description of Goods");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "HSN Code");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Weight (in Kg)");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Rate");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"12.5%\" align=\"center\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Amount");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateBlankRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateClientDetails(StringBuilder stringBuilder)
        {
            GenerateReceiverDetailHeaderRow(stringBuilder);
            GnenerateClientNameRow(stringBuilder);
            GenerateAddressRow(stringBuilder);
            GenerateStateDetailsRow(stringBuilder);
        }

        private static void GenerateStateDetailsRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\"");

            stringBuilder.AppendFormat(Constants.TableStartTag, Constants.TableStyle + "class=\"pageText\"");
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "State");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myCustomer.State);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"25%\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "State Code&nbsp;");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "32");
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"35%\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "GSTIN No&nbsp;");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myCustomer.GstNumber);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);
            stringBuilder.Append(Constants.TableEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateAddressRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\"");

            stringBuilder.AppendFormat(Constants.TableStartTag, Constants.TableStyle + "class=\"pageText\"");
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Address");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"80%\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myCustomer.Address);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.TableRowEndTag);
            stringBuilder.Append(Constants.TableEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GeneratePageEndTags(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Constants.TableEndTag);
            stringBuilder.Append(Constants.CellEndTag);
            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
            stringBuilder.Append(Constants.TableEndTag);
            stringBuilder.Append(Constants.BodyEndTag);
            stringBuilder.Append(Constants.HtmlEndTag);
        }

        private static void GnenerateClientNameRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\" valign=\"bottom\"");

            stringBuilder.AppendFormat(Constants.TableStartTag, Constants.TableStyle + "class=\"pageText\"");
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Client Name");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"80%\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myCustomer.Name);
            stringBuilder.Append(Constants.CellEndTag);
            stringBuilder.Append(Constants.TableRowEndTag);
            stringBuilder.Append(Constants.TableEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateReceiverDetailHeaderRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\" valign=\"bottom\" class=\"subHeadingText\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, "style=\"color:#d04e00;\"");
            stringBuilder.Append("Details of Receiver(Billed to)");
            stringBuilder.Append(Constants.StrongEndTag);

            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateInvoiceRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\" align=\"right\" class=\"pageText\" width=\"30%\"");
            stringBuilder.AppendFormat(Constants.TableStartTag, Constants.TableStyle + "class=\"tb\"");

            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Invoice Serial");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.BillSerialNumber);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.StrongStartTag, string.Empty);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Invoice Date");
            stringBuilder.Append(Constants.StrongEndTag);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "width=\"20%\"");
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, myGstBill.GstDate.ToString("dd-MMM-yyyy"));
            stringBuilder.Append(Constants.CellEndTag);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.Append(Constants.TableRowEndTag);

            stringBuilder.Append(Constants.TableEndTag);

            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateReverseTaxRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "colspan=\"2\" height=\"36\" align=\"right\" class=\"pageTextBold\" width=\"30%\" style=\"padding-right: 10px;\"");
            stringBuilder.Append("Tax is Payable on Reverse Charge : " + myGstBill.IsTaxPayableReverse);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }

        private static void GenerateHeaderRow(StringBuilder stringBuilder)
        {
            stringBuilder.AppendFormat(Constants.TableRowStartTag, string.Empty);
            stringBuilder.Append(Constants.EmptyCellTag);

            stringBuilder.AppendFormat(Constants.CellStartTag, "height=\"35\" colspan=\"2\" align=\"center\" class=\"txt\" style=\"border-bottom:1px solid #ddd; color:#d04e00; font-weight:800; font-family: Helvetica, Arial, sans-serif;\"");
            stringBuilder.AppendFormat(Constants.SpanTag, "style=\"font-size: 40px; color:#d04e00;\" class=\"code char\"", "ॐ");
            stringBuilder.Append(Constants.Break);
            stringBuilder.AppendFormat(Constants.SpanTag, "style=\"font-size: 20px; color:#d04e00;\"", "Tax Invoice");
            stringBuilder.Append(Constants.Break);
            stringBuilder.AppendFormat(Constants.SpanTag, "style=\"font-size: 50px; color:#d04e00;\"", Constants.WfpAppName);
            stringBuilder.Append(Constants.Break);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "Fancy Silver Leg Chain Ornaments Manufacturers & Order Supplie");
            stringBuilder.Append(Constants.Break);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "25, Kumarasamy Street, Shevapet, Salem, Tamil Nadu - 636002.");
            stringBuilder.Append(Constants.Break);
            stringBuilder.AppendFormat(Constants.SpanTag, string.Empty, "GSTIN No : 33AGPR9649R1Z2 &nbsp; Cell : 9003344881");

            stringBuilder.Append(Constants.Break);
            stringBuilder.Append(Constants.CellEndTag);

            stringBuilder.Append(Constants.EmptyCellTag);
            stringBuilder.Append(Constants.TableRowEndTag);
        }
    }
}
