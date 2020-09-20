using MaterialDesignThemes.Wpf;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Common.Events;
using WpfApp.Helpers;

namespace WpfApp.Common
{
    public class ItemMenuViewModel : BaseViewModel
    {
        readonly IEventAggregator myEventAggregator;
        public ItemMenuViewModel(string header, List<SubItemViewModel> subItems, PackIconKind icon, IEventAggregator eventAggregator)
        {
            myEventAggregator = eventAggregator;
            Header = header;
            SubItems = subItems;
            Icon = icon;
            IsExpanded = false;
            this.OnSelectionChangeCommand = new Command(this.OnSelectionChanged, o => true);
        }

        public ICommand OnSelectionChangeCommand { get; }
        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public List<SubItemViewModel> SubItems { get; private set; }

        private bool myIsExpanded;

        public bool IsExpanded
        {
            get
            {
                return myIsExpanded;
            }
            set
            {
                if (string.Equals(this.myIsExpanded, value)) return;
                this.myIsExpanded = value;
                OnPropertyChanged();
            }
        }

        private SubItemViewModel mySubItem;

        public SubItemViewModel SubItem
        {
            get
            {
                return mySubItem;
            }
            set
            {
                if (string.Equals(this.mySubItem, value)) return;
                this.mySubItem = value;
                OnSelectionChanged(value);
                OnPropertyChanged();
            }
        }

        public UserControl Screen { get; private set; }

        private void OnSelectionChanged(object obj)
        {
            var subItem = (SubItemViewModel)obj;
            WpfAppForms natureBoxForm = Utility.GetNatureBoxForm(subItem.Name);
            myEventAggregator.GetEvent<NavigateViewsEvent>().Publish(natureBoxForm);
        }
    }
}
