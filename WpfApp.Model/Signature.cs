using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WpfApp.Common;

namespace WpfApp.Model
{
    [Table("tb_signature")]
    public class Signature : AbstractNotifyPropertyChanged
    {
        private string mySignatureType;
        private string mySignatureFilePath;

        [Key]
        public int SignatureId { get; set; }

        public string SignatureType
        {
            get => this.mySignatureType;
            set => SetProperty(ref mySignatureType, value);
        }

        public string SignatureFilePath
        {
            get => this.mySignatureFilePath;
            set => SetProperty(ref mySignatureFilePath, value);
        }
    }
}
