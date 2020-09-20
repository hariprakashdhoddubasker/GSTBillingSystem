using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WpfApp.Common;

namespace WpfApp.Model
{
    [Table("tb_customer")]
    public class Customer : AbstractNotifyPropertyChanged
    {
        private string myName;
        private DateTime myDOB = DateTime.Now;
        private long myMobileNumber;
        private DateTime myDOJ;
        private int myCustomerId;
        private int myReferredBy;
        private double myBalanceAmount;
        private double myTotalAmountPaid;

        [Key]
        public int CustomerId
        {
            get => this.myCustomerId;
            set => SetProperty(ref myCustomerId, value);
        }

        public string Name
        {
            get => this.myName;
            set => SetProperty(ref myName, value);
        }

        public long MobileNumber
        {
            get => this.myMobileNumber;
            set => SetProperty(ref myMobileNumber, value);
        }

        public DateTime DOB
        {
            get => this.myDOB;
            set => SetProperty(ref myDOB, value);
        }

        public DateTime DOJ
        {
            get => this.myDOJ;
            set => SetProperty(ref myDOJ, value);
        }

        public double BalanceAmount
        {
            get => this.myBalanceAmount;
            set => SetProperty(ref myBalanceAmount, value);
        }

        public double TotalAmountPaid
        {
            get => this.myTotalAmountPaid;
            set => SetProperty(ref myTotalAmountPaid, value);
        }

        public int EmployeeId
        {
            get => this.myReferredBy;
            set => SetProperty(ref myReferredBy, value);
        }

        //public virtual ICollection<Invoice> Invoices { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }
    }
}
