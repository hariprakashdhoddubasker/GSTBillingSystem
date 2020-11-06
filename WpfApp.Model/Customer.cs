using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WpfApp.Common;

namespace WpfApp.Model
{
    [Table("tb_customer")]
    public class Customer : AbstractNotifyPropertyChanged
    {
        private string myName;
        private long myMobileNumber;
        private int myCustomerId;
        private int myStateId;
        private string myAddress;
        private string myGSTNumber;

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

        public string Address
        {
            get => this.myAddress;
            set => SetProperty(ref myAddress, value);
        }

        public string GstNumber
        {
            get => this.myGSTNumber;
            set => SetProperty(ref myGSTNumber, value);
        }

        public int StateId
        {
            get => this.myStateId;
            set => SetProperty(ref myStateId, value);
        }

        public virtual State State { get; set; }
        public virtual ICollection<GstBill> GstBills { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", Name);
        }

        public void Clear()
        {
            CustomerId = 0;
            Name = string.Empty;
            MobileNumber = 0;
            Address = string.Empty;
            GstNumber = string.Empty;
            StateId = 0;
        }
    }
}
