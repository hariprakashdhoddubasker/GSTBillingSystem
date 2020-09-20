using System.ComponentModel;

namespace WpfApp.Helpers
{
    public enum WpfAppForms
    {
        [Description("Customer")]
        Customer,
        [Description("Product")]
        Product,
        [Description("Invoice")]
        Invoice,
        [Description("Customer Invoice")]
        CustomerInvoiceReport,
        [Description("BackUp")]
        BackUp
    }

    public enum WpfAppRoles
    {
        Admin,
        Parnter
    }
}
