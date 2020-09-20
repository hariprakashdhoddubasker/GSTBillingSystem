using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace WpfApp.Common
{
    public class SubItemViewModel
    {
        public SubItemViewModel(string name, UserControl screen = null)
        {
            Name = name;
            Screen = screen;
        }

        public string Name { get; private set; }
        public UserControl Screen { get; private set; }
    }
}
