using WpfApp.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp.Model
{
    [Table("tb_employee")]
    public class Employee : AbstractNotifyPropertyChanged
    {
        private string myUserName;
        private string myPassword;
        private long myMobileNumber;
        private string myRole;

        [Key]
        public int EmployeeId { get; set; }

        public string UserName
        {
            get => this.myUserName;
            set => SetProperty(ref myUserName, value);
        }

        public string Password
        {
            get => this.myPassword;
            set => SetProperty(ref myPassword, value);
        }

        public long MobileNumber
        {
            get => this.myMobileNumber;
            set => SetProperty(ref myMobileNumber, value);
        }

        public string Role
        {
            get => this.myRole;
            set => SetProperty(ref myRole, value);
        }

        public override string ToString()
        {
            return UserName;
        }
    }
}
