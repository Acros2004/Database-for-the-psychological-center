
using CleanBrain.ManagerFrame;
using CleanBrain.MVVM;
using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private bool flag = false;
        public Registration()
        {
            InitializeComponent();
            Password3.Password = ManagerItem.isRussian ? "пароль" : "password";
            forPassword.Text = ManagerItem.isRussian ? "пароль" : "password";


        }
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Password3.Password.ToString() == "пароль" || Password3.Password.ToString() == "password")
            {
                Password3.Visibility = Visibility.Visible;
                forPassword.Visibility = Visibility.Collapsed;
                Password3.Focus();
                Password3.Password = "";

            }
        }
        private void PasswordBox_LastFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Password3.Password))
            {
                if (ManagerItem.isRussian)
                {
                    Password3.Visibility = Visibility.Collapsed;
                    forPassword.Visibility = Visibility.Visible;
                    Password3.Password = "пароль";
                }
                else
                {
                    Password3.Visibility = Visibility.Collapsed;
                    forPassword.Visibility = Visibility.Visible;
                    Password3.Password = "password";
                }
                

            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(Password3.Password == "пароль" || Password3.Password == "password" )
            {
               Password3.Visibility= Visibility.Collapsed;
               forPassword.Visibility= Visibility.Visible;
            }
            if (!flag)
            {
                flag = true;
                CorrectPassword.Visibility = Visibility.Collapsed;
            }
            else
                CorrectPassword.Visibility = Visibility.Visible;
            ManagerItem.PasswordCheck = Password3.Password.ToString();
            
            if(DataContext is RegModel model)
            {
                model.PasswordChange.Execute(null);
            }
        }
    }
}
