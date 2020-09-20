using WpfApp.Dialog;
using WpfApp.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace WpfApp.Helpers
{
    public static class UIService
    {
        public static DispatcherSynchronizationContext DispatcherSynchronizationContext;

        public static IDialogService DialogService { get; set; }
        public static Employee CurrentUser { get; set; }

        public static bool? ShowMessage(string error, Visibility visibility = Visibility.Collapsed)
        {
            bool? df = false;
            DispatcherSynchronizationContext.Send
                (x =>
                    df = DialogService.ShowDialog(new DialogViewModel(error, visibility.ToString())).Value
                , null);
            return df;
        }

        public static string GetDescription(this Enum e)
        {
            var attribute =
                e.GetType()
                    .GetTypeInfo()
                    .GetMember(e.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault()
                    as DescriptionAttribute;

            return attribute?.Description ?? e.ToString();
        }
    }
}
