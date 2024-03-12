using CleanBrain.ManagerFrame;
using CleanBrain.MVVM;
using CleanBrain.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия WindowApp.xaml
    /// </summary>
    public partial class WindowApp : Window
    {
        public WindowApp(Client client)
        {
            InitializeComponent();
            Manager.ProfileFrame = ProfileFrame;
            Manager.PreviewFrame = PreviewFraim;
            DataContext = new PreviewModel(client);
            
        }

       

    }
}
