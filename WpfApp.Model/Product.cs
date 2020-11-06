using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WpfApp.Common;

namespace WpfApp.Model
{
    [Table("tb_product")]
    public class Product : AbstractNotifyPropertyChanged
    {
        private string myProductName;
        private int myHsnCode;

        [Key]
        public int ProductId { get; set; }

        public string ProductName
        {
            get => this.myProductName;
            set => SetProperty(ref myProductName, value);
        }

        public int HsnCode
        {
            get => this.myHsnCode;
            set => SetProperty(ref myHsnCode, value);
        }
        public override string ToString()
        {
            return string.Format("{0}", ProductName);
        }
    }
}
