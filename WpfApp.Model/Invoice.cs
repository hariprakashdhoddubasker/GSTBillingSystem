using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WpfApp.Common;

namespace WpfApp.Model
{
    [Table("tb_invoice")]
    public class Invoice : AbstractNotifyPropertyChanged
    {
        private double myWeight;
        private double myAmount;
        private double mySGST;
        private double myCGST;
        private double myIGST;
        private double myAmountAfterTax;
        private double myRate;
        private Product myProduct;

        public Invoice()
        {
            DateOfPurchase = DateTime.Now;
        }
        [Key]
        public int InvoiceId { get; set; } 
        public int ProductId { get; set; }
        public DateTime DateOfPurchase { get; set; }

        [Required]
        public double Weight
        {
            get => this.myWeight;
            set => SetProperty(ref myWeight, value);
        }

        [Required]
        public double Rate
        {
            get => this.myRate;
            set => SetProperty(ref myRate, value);
        }

        public double AmountBeforeTax
        {
            get => this.myAmount;
            set => SetProperty(ref myAmount, value);
        }
        public double SGST
        {
            get => this.mySGST;
            set => SetProperty(ref mySGST, value);
        }
        public double CGST
        {
            get => this.myCGST;
            set => SetProperty(ref myCGST, value);
        }
        public double IGST
        {
            get => this.myIGST;
            set => SetProperty(ref myIGST, value);
        }

        [Required]
        public double AmountAfterTax
        {
            get => this.myAmountAfterTax;
            set => SetProperty(ref myAmountAfterTax, value);
        }

        public int GstBillId { get; set; }

        public string IsInvoiceAdded { get; set; }

        [NotMapped]
        public int DisplayNumber { get; set; }

        [NotMapped]
        public virtual Product Product
        {
            get => this.myProduct;
            set => SetProperty(ref myProduct, value);
        }
    }
}
