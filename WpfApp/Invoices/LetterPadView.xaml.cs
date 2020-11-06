using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp.Invoices
{
    /// <summary>
    /// Interaction logic for LetterPadView.xaml
    /// </summary>
    public partial class LetterPadView : UserControl
    {
        public LetterPadView()
        {
            InitializeComponent();
        }

        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string rtfString = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(ms, DataFormats.Rtf);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(ms))
                {
                    rtfString = sr.ReadToEnd();
                }
            }
            ((LetterPadViewModel)this.DataContext).LetterPadRtfContent = rtfString;

        }
    }
}
