using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanBrain.Pages
{
    /// <summary>
    /// Логика взаимодействия для GreetingPage.xaml
    /// </summary>
    public partial class GreetingPage : Page
    {
        public GreetingPage()
        {
            InitializeComponent();
        }

        private void OpenVK(object sender, RoutedEventArgs e)
        {
            string url = "https://vk.com/i_s_privetom";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
            e.Handled = true;
        }
        private void OpenInst(object sender, RoutedEventArgs e)
        {
            string url = "https://www.instagram.com/goodniklucky/";

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
            e.Handled = true;
        }
      
    }
}
