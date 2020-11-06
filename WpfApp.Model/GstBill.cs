using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WpfApp.Common;

namespace WpfApp.Model
{
    [Table("tb_gst_bill")]
    public class GstBill : AbstractNotifyPropertyChanged
    {
        private DateTime myGstDate;
        private string myIsTaxPayableReverse;
        private string myBillSerialNumber;

        public GstBill()
        {
            myGstDate = DateTime.Now;
        }

        [Key]
        public int GstBillId { get; set; }

        public string BillSerialNumber
        {
            get => this.myBillSerialNumber;
            set => SetProperty(ref myBillSerialNumber, value);
        }

        public DateTime GstDate
        {
            get => this.myGstDate;
            set => SetProperty(ref myGstDate, value);
        }

        public string IsTaxPayableReverse
        {
            get => this.myIsTaxPayableReverse;
            set => SetProperty(ref myIsTaxPayableReverse, value);
        }
        
        public double AmountBeforeTax { get; set; }
        public double SGST { get; set; }
        public double CGST { get; set; }
        public double IGST { get; set; }
        public double TotalTaxAmount { get; set; }
        public double RoundOff { get; set; }
        public double AmountAfterTax { get; set; }
        public string AmountAfterTaxString { get; set; }
        public int CustomerId { get; set; }
        public string GstBillFilePath  { get; set; }

        [NotMapped]
        public string SignatureFilePath { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
