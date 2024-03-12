using CleanBrain.MVVM;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CleanBrain.Users
{
    /// <summary>
    /// Логика взаимодействия для ReviewClient.xaml
    /// </summary>
    public partial class ReviewClient : Page
    {
        public ReviewClient(int id)
        {
            InitializeComponent();
            DataContext = new ReviewModel(id);
        }
    }
}
