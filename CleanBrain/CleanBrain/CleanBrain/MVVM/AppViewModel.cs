using CleanBrain.Command;
using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CleanBrain.MVVM
{
    public class AppViewModel : INotifyPropertyChanged
    {
        private RelayCommand closeCommandApp;
        public RelayCommand CloseApp
        {
            get
            {
                return closeCommandApp ??
                    (closeCommandApp = new RelayCommand(obj =>
                    {
                        MainWindow app = obj as MainWindow;
                        app.Close();
                    }
                    ));
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
