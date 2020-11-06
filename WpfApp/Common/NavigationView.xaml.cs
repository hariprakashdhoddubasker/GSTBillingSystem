using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp.Common
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView : UserControl
    {
        readonly DispatcherTimer timer;

        double panelWidth;
        bool hidden;

        public NavigationView()
        {
            InitializeComponent();
            timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 10)
            };
            timer.Tick += Timer_Tick;

            panelWidth = sidePanel.Width;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                sidePanel.Width += 15;
                if (sidePanel.Width >= panelWidth)
                {
                    timer.Stop();
                    hidden = false;
                }
            }
            else
            {
                sidePanel.Width -= 15;
                if (sidePanel.Width <= 55)
                {
                    timer.Stop();
                    hidden = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}
