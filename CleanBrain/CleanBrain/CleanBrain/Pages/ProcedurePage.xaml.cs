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

namespace CleanBrain.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProcedurePage.xaml
    /// </summary>
    public partial class ProcedurePage : Page
    {
        public ProcedurePage()
        {
            InitializeComponent();
            DataContext = new ProcedureModel();
        }
        public ProcedurePage(int id)
        {
            InitializeComponent();
            DataContext = new ProcedureModel(id);
        }
    }
}
