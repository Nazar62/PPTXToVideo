using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PptxToVideo
{
    /// <summary>
    /// Interaction logic for AddApiKey.xaml
    /// </summary>
    public partial class AddApiKey : Window
    {
        public string ApiKey { get; set; }
        public AddApiKey()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            ApiKey = txtMessage.Text;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void CloseBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseBtn.Background = new SolidColorBrush(Colors.DarkRed);
        }

        private void CloseBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseBtn.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
