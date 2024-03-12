using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using CleanBrain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CleanBrain.MVVM
{
    public class LogModel : INotifyPropertyChanged
    {
        private Client logClient = new Client();
        private List<Client> clients;
        private Client findClient;
        private readonly string allowedchar = "'";
        private bool loginFlag = false;
        private bool passwordFlag = false;
        private UnitOfWork unit = new UnitOfWork();
        public BitmapImage correctPassword;
        public BitmapImage correctLogin;
        
        public Client LogClient
        {
            get { return logClient; }
            set { logClient = value; }
        }
        public BitmapImage CorrectPassword
        {
            get { return correctPassword; }
            set { correctPassword = value; }
        }
        public BitmapImage CorrectLogin
        {
            get { return correctLogin; }
            set { correctLogin = value; }
        }
        public LogModel()
        {
            this.clients = new List<Client>(unit.Client.GetAll());
        }

        private RelayCommand loginGot;
        public RelayCommand LoginGot
        {
            get
            {
                return loginGot ?? (loginGot = new RelayCommand(obj =>
                {
                    if (LogClient.Login_Client == "логин" || LogClient.Login_Client == "login")
                    {
                        LogClient.Login_Client = "";
                    }
                    OnPropertyChanged("CorrectLogin");
                }));
            }
        }
        private RelayCommand loginLost;
        public RelayCommand LoginLost
        {
            get
            {
                return loginLost ?? (loginLost = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(LogClient.Login_Client))
                    {
                        if (ManagerItem.isRussian)
                            LogClient.Login_Client = "логин";
                        else
                            LogClient.Login_Client = "login";
                        CorrectLogin = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                        OnPropertyChanged("CorrectLogin");
                        loginFlag = false;
                        return;
                    }
                }));
            }
        }

        private RelayCommand loginChange;
        public RelayCommand LoginChange
        {
            get
            {
                return loginChange ?? (loginChange = new RelayCommand(obj =>
                {

                    Regex regex = new Regex(@"^[a-zA-Z0-9]{1,12}$");
                    Match match = regex.Match(LogClient.Login_Client);
                    if ((LogClient.Login_Client.ToString().Contains(allowedchar)) || string.IsNullOrWhiteSpace(LogClient.Login_Client) || (match.Success == false))
                    {
                        CorrectLogin = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                        OnPropertyChanged("CorrectLogin");
                        loginFlag = false;
                        return;
                    }
                    if (LogClient.Login_Client == "логин" || LogClient.Login_Client == "login")
                        return;
                    CorrectLogin = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\correct.png", UriKind.Absolute));
                    loginFlag = true;
                    OnPropertyChanged("CorrectLogin");
                }));
            }
        }

        private RelayCommand passwordGot;
        public RelayCommand PasswordGot
        {
            get
            {
                return passwordGot ?? (passwordGot = new RelayCommand(obj =>
                {
                    if (LogClient.Password_Client == "пароль" || LogClient.Password_Client == "password")
                    {
                        LogClient.Password_Client = "";
                    }
                    OnPropertyChanged("CorrectPassword");
                }));
            }
        }

        private RelayCommand passwordLost;
        public RelayCommand PasswordLost
        {
            get
            {
                return passwordLost ?? (passwordLost = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(LogClient.Password_Client))
                    {
                        if (ManagerItem.isRussian)
                            LogClient.Password_Client = "пароль";
                        else
                            LogClient.Password_Client = "password";
                        CorrectPassword = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                        passwordFlag = false;
                    }
                    OnPropertyChanged("CorrectPassword");
                }));
            }
        }

        private RelayCommand passwordChange;
        public RelayCommand PasswordChange
        {
            get
            {
                return passwordChange ?? (passwordChange = new RelayCommand(obj =>
                {
                    LogClient.Password_Client = ManagerItem.PasswordCheck;
                    Regex regex = new Regex(@"^[a-zA-Z0-9]{1,12}$");
                    Match match = regex.Match(LogClient.Password_Client);
                    if ((LogClient.Password_Client.ToString().Contains(allowedchar)) || string.IsNullOrWhiteSpace(LogClient.Password_Client) || (match.Success == false))
                    {
                        CorrectPassword = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                        passwordFlag = false;
                        OnPropertyChanged("CorrectPassword");
                        return;
                    }
                    else
                    {
                        if (LogClient.Password_Client == "пароль" || LogClient.Password_Client == "password")
                        {
                            CorrectPassword = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                            passwordFlag = false;
                            OnPropertyChanged("CorrectPassword");
                            return;
                        }
                        CorrectPassword = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\correct.png", UriKind.Absolute));
                        passwordFlag = true;
                    }
                    OnPropertyChanged("CorrectPassword");
                }));
            }
        }

        private RelayCommand clickButton;

        public RelayCommand LogButton
        {
            get
            {
                return clickButton ??
                    (clickButton = new RelayCommand(obj =>
                    {
                        LogClient.Password_Client = ManagerItem.PasswordCheck;
                        if ((LogClient.Login_Client != "логин") && (LogClient.Password_Client != "пароль") && (LogClient.Login_Client != "login") && (LogClient.Password_Client != "password") && passwordFlag && loginFlag)
                        {
                            try
                            {
                                string tempLogin;
                                if (LogClient.Login_Client != "nik")
                                    tempLogin = Encoder.EncoderL.HashPassword(LogClient.Password_Client);
                                else
                                    tempLogin = LogClient.Password_Client;
                                foreach (Client client in clients)
                                {
                                    if (client.Login_Client == LogClient.Login_Client && client.Password_Client == tempLogin)
                                    {
                                        findClient = client;
                                        break;
                                    }
                                }
                                if (findClient != null)
                                {
                                    WindowApp mainWindow = new WindowApp(findClient);
                                    mainWindow.Show();
                                    foreach (Window window in Application.Current.Windows)
                                    {
                                        if (window is MainWindow)
                                        {
                                            window.Close();
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Не найдено" : "Not found");
                                    window.Show();
                                }
                                    
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show(e.Message);
                            }

                        }
                        /* unit.Client.Create(regClient);
                     unit.Save();*/
                        

                    })
                    );
            }
        }

        private RelayCommand getGuest;

        public RelayCommand GetGuest
        {
            get
            {
                return getGuest ?? (getGuest = new RelayCommand(obj =>
                {
                    findClient = unit.Client.Get(11);
                    WindowApp mainWindow = new WindowApp(findClient);
                    mainWindow.Show();
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is MainWindow)
                        {
                            window.Close();
                            break;
                        }
                    }
                }));
            }
        }

        private RelayCommand regPage;
        public RelayCommand GetRegistrationPage
        {
            get
            {
                return regPage ?? (regPage = new RelayCommand(obj =>
                {
                    Manager.MainFrame.Navigate(new Registration());
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
