using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
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
    public class SettingsModel : INotifyPropertyChanged
    {
        private RelayCommand changeAccount;
        private bool flag = false;
        private string image = "../Photos/Rus.png";
        public string Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
        }
        public SettingsModel()
        {
            if (ManagerItem.isRussian)
                Image = "../Photos/Rus.png";
            else
                Image = "../Photos/Eng.png";
        }

        public RelayCommand ChangeAccount
        {
            get
            {
                return changeAccount ?? (changeAccount = new RelayCommand(obj =>
                {
                    MainWindow mainwindow = new MainWindow();
                    //WindowApp app = obj as WindowApp;
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is WindowApp)
                        {
                            window.Close();
                            break;
                        }
                    }
                    mainwindow.Show();
                    //app.Close();
                }));
            }
        }

        private RelayCommand changeTheme;

        public RelayCommand ChangeTheme
        {
            get
            {
                return changeTheme ?? (changeTheme = new RelayCommand(obj =>
                {
                    if (!flag)
                    {
                        Application.Current.Resources.MergedDictionaries[1].Source = new Uri("C:\\Users\\nikit\\Desktop\\univer\\2cource2sem\\CleanBrain\\CleanBrain\\CleanBrain\\Theme\\Blue.xaml", UriKind.RelativeOrAbsolute);
                        flag = true;
                    }
                    else
                    {
                        Application.Current.Resources.MergedDictionaries[1].Source = new Uri("C:\\Users\\nikit\\Desktop\\univer\\2cource2sem\\CleanBrain\\CleanBrain\\CleanBrain\\Theme\\Default.xaml", UriKind.RelativeOrAbsolute);
                        flag = false;
                    }
                        
                }));
            }
        }

        private RelayCommand changeLan;

        public RelayCommand ChangeLan
        {
            get
            {
                return changeLan ?? (changeLan = new RelayCommand(obj =>
                {
                    if (!ManagerItem.isRussian)
                    {
                        Application.Current.Resources.MergedDictionaries[2].Source = new Uri("C:\\Users\\nikit\\Desktop\\univer\\2cource2sem\\CleanBrain\\CleanBrain\\CleanBrain\\Languages\\Rus.xaml", UriKind.RelativeOrAbsolute);
                        Image = "../Photos/Rus.png";
                        ManagerItem.isRussian = true;
                    }
                    else
                    {
                        Application.Current.Resources.MergedDictionaries[2].Source = new Uri("C:\\Users\\nikit\\Desktop\\univer\\2cource2sem\\CleanBrain\\CleanBrain\\CleanBrain\\Languages\\Eng.xaml", UriKind.RelativeOrAbsolute);
                        Image = "../Photos/Eng.png";
                        ManagerItem.isRussian = false;
                    }

                }));
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
