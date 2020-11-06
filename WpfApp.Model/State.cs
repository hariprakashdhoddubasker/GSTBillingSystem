using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WpfApp.Common;

namespace WpfApp.Model
{
    [Table("tb_state")]
    public class State : AbstractNotifyPropertyChanged
    {
        private string myStateName;
        private int myStateCode;

        [Key]
        public int StateId { get; set; }

        public string StateName
        {
            get => this.myStateName;
            set => SetProperty(ref myStateName, value);
        }

        public int StateCode
        {
            get => this.myStateCode;
            set => SetProperty(ref myStateCode, value);
        }

        public virtual ICollection<Customer> Customers { get; set; }

        public override string ToString()
        {
            return StateName;
        }
    }
}
