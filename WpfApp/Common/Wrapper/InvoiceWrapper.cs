using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WpfApp.Model;

namespace WpfApp.Common.Wrapper
{
    public class InvoiceWrapper : ModelWrapper<Invoice>
    {
        public InvoiceWrapper(Invoice model) : base(model)
        {
            InvoiceId = model.InvoiceId;
        }

        public int InvoiceId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [Required]
        public int ProductId
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [Required]
        public DateTime DateOfPurchase { get; set; }

        [Required]
        public double Weight
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        [Required]
        public double Rate
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        [Required]
        public double AmountBeforeTax
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public double SGST
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public double CGST
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public double IGST
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        [Required]
        public double AmountAfterTax
        {
            get { return GetValue<double>(); }
            set { SetValue(value); }
        }

        public Product Product
        {
            get { return GetValue<Product>(); }
            set { SetValue(value); }
        }

        [Required]
        public int GstBillId { get; set; }

        public bool IsHandlerSubscribed { get; set; }


        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Rate):
                    var rateValue = Rate.ToString().Replace("₹", "");

                    if (double.TryParse(rateValue, out _) && rateValue.Length > 10)
                    {
                        yield return "Please enter a smaller value.";
                    }
                    break;

                case nameof(AmountAfterTax):
                    var amountValue = AmountAfterTax.ToString().Replace("₹", "");

                    if (double.TryParse(amountValue, out _) && amountValue.Length > 10)
                    {
                        yield return "Amount value is too High.";
                    }
                    break;

                case nameof(Weight):
                    var weightValue = Weight.ToString().Replace("₹", "");

                    if (double.TryParse(weightValue, out _) && weightValue.Length > 10)
                    {
                        yield return "Please enter a smaller value.";
                    }
                    break;
            }
        }

        public Invoice Clone(int serialNo)
        {
            return new Invoice
            {
                DisplayNumber = serialNo,
                ProductId = this.ProductId,
                Product = this.Product,
                Weight = this.Weight,
                Rate = this.Rate,
                AmountBeforeTax = this.AmountBeforeTax,
                SGST = this.SGST,
                IGST = this.IGST,
                CGST = this.CGST,
                AmountAfterTax = this.AmountAfterTax
            };
        }

        public void ClearCalculations()
        {
            this.Weight = 0;
            this.Rate = 0;
            this.SGST = 0;
            this.IGST = 0;
            this.CGST = 0;
            this.AmountAfterTax = 0;
            this.AmountBeforeTax = 0;
        }
    }

}
