using CleanBrain.ManagerFrame;
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
    /// Логика взаимодействия для ProceduresPage.xaml
    /// </summary>
    public partial class ProceduresPage : Page
    {
        public ProceduresPage()
        {
            InitializeComponent();
            Manager.ReviewFrame = ReviewFrame;
            DataContext = new ListProceduresModel();
        }

        public ProceduresPage(int id)
        {
            InitializeComponent();
            Manager.ReviewFrame = ReviewFrame;
            DataContext = new ListProceduresModel(id);
        }
        private void OnPreviewMouseDown(object sender, RoutedEventArgs e)
        {
            ListViewItem listViewItem = sender as ListViewItem;
            if (listViewItem != null)
            {
                ListView listView = ItemsControl.ItemsControlFromItemContainer(listViewItem) as ListView;
                if (listView != null)
                {
                    // Сбросить выбор всех элементов списка
                    foreach (object item in listView.Items)
                    {
                        ListViewItem lvi = listView.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                        if (lvi != null)
                        {
                            lvi.IsSelected = false;
                        }
                    }

                    // Установить выбранный элемент
                    listViewItem.IsSelected = true;
                }
            }

        }
    }
}
