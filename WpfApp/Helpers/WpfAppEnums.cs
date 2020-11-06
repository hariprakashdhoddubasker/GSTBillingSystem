using System.ComponentModel;

namespace WpfApp.Helpers
{
    public enum WpfAppForms
    {
        [Description("Customer")]
        Customer,
        [Description("Product")]
        Product,
        [Description("State")]
        State,
        [Description("Invoice")]
        Invoice,
        [Description("Report")]
        CustomerInvoiceReport,
        [Description("BackUp")]
        BackUp,
        [Description("Letter Pad")]
        LetterPad,
        [Description("Signature")]
        Signature
    }

    public enum WpfAppRoles
    {
        Admin,
        Parnter
    }
}
