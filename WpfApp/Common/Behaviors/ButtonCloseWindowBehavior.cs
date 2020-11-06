using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp.Common.Behaviors
{
    public static class ButtonCloseWindowBehavior
    {
        #region Private Section

        private static Window MainWindow = Application.Current.MainWindow;

        #endregion

        #region IsCloseProperty

        public static readonly DependencyProperty IsCloseProperty;

        public static void SetIsClose(DependencyObject DepObject, bool value)
        {
            DepObject.SetValue(IsCloseProperty, value);
        }

        public static bool GetIsClose(DependencyObject DepObject)
        {
            return (bool)DepObject.GetValue(IsCloseProperty);
        }

        static ButtonCloseWindowBehavior()
        {
            IsCloseProperty = DependencyProperty.RegisterAttached("IsClose",
                                                                  typeof(bool),
                                                                  typeof(ButtonCloseWindowBehavior),
                                                                  new UIPropertyMetadata(false, IsCloseTurn));
        }

        #endregion

        private static void IsCloseTurn(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool && ((bool)e.NewValue) == true)
            {
                if (MainWindow != null)
                    MainWindow.PreviewKeyDown += new KeyEventHandler(MainWindow_PreviewKeyDown);

                var button = sender as Button;

                if (button != null)
                    button.Click += new RoutedEventHandler(button_Click);
            }
        }

        private static void button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Close();
        }

        private static void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                MainWindow.Close();
        }
    }
}
