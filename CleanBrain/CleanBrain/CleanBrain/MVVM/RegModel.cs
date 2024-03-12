using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows;
using CleanBrain.Users;
using System.IO;
using CleanBrain.Pages;
using System.Security;

namespace CleanBrain.MVVM
{
    public class RegModel : INotifyPropertyChanged
    {
        
        private byte[] imageBytesBuffer;
        private Client regClient = new Client();
        private Client findClient;
        private bool emailFlag = false;
        private readonly string allowedchar = "'";
        private bool loginFlag = false;
        private bool passwordFlag = false;
        private UnitOfWork unit = new UnitOfWork();
        public BitmapImage correctEmail;
        public BitmapImage correctPassword;
        public BitmapImage correctLogin;
        private List<Client> clients;

        private SecureString password;

        public SecureString Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged("Password"); }
        }

        public RegModel()
        {
            clients = new List<Client>(unit.Client.GetAll());
        }

        public BitmapImage CorrectEmail
        {
            get { return correctEmail; }
            set { correctEmail = value; }
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
        public Client RegClient 
        {
            get { return regClient; }
            set { regClient = value; }
        }

        private RelayCommand clickButton;

        public RelayCommand RegButton
        {
            get
            {
                return clickButton ??
                    (clickButton = new RelayCommand(obj =>
                    {
                        RegClient.Password_Client = ManagerItem.PasswordCheck;
                        if ((regClient.Login_Client != "логин") && (regClient.Password_Client != "пароль") && (regClient.Mail_Client != "электронная почта") && (regClient.Login_Client != "login") && (regClient.Password_Client != "password") && (regClient.Mail_Client != "email") && emailFlag && passwordFlag && loginFlag)
                        {
                            try
                            {
                                using (FileStream fs = new FileStream(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\bear.png", FileMode.Open, FileAccess.Read))
                                {
                                    imageBytesBuffer = new byte[fs.Length];
                                    fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                                }
                                regClient.Photo_Client = imageBytesBuffer;
                                regClient.Password_Client = Encoder.EncoderL.HashPassword(regClient.Password_Client);
                                unit.Client.Create(regClient);
                                unit.Save();
                                
                                clients = new List<Client>(unit.Client.GetAll());
                                foreach (Client client in clients)
                                {
                                    if(client.Login_Client == regClient.Login_Client)
                                    {
                                        findClient = client;
                                    }
                                }
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
                            catch(Exception e)
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
        private RelayCommand emailGot;
        public RelayCommand EmailGot
        {
            get
            {
                return emailGot ?? (emailGot = new RelayCommand(obj =>
                {
                    if (regClient.Mail_Client == "электронная почта" || regClient.Mail_Client == "email")
                    {
                        regClient.Mail_Client = "";
                    }
                    OnPropertyChanged("CorrectEmail");
                }));
            }
        }

        private RelayCommand emailLost;
        public RelayCommand EmailLost
        {
            get
            {
                return emailLost ?? (emailLost = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(regClient.Mail_Client))
                    {
                        if (ManagerItem.isRussian)
                            regClient.Mail_Client = "электронная почта";
                        else
                            regClient.Mail_Client = "email";
                        CorrectEmail = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                    }
                    OnPropertyChanged("CorrectEmail");
                }));
            }
        }

        private RelayCommand emailChange;
        public RelayCommand EmailChange
        {
            get
            {
                return emailChange ?? (emailChange = new RelayCommand(obj =>
                {
                    string emails = regClient.Mail_Client;
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(emails);
                    if ((match.Success))
                    {
                        CorrectEmail = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\correct.png", UriKind.Absolute));
                        emailFlag = true;
                    }
                    if ((match.Success == false))
                    {
                        if (regClient.Mail_Client == "электронная почта" || regClient.Mail_Client == "email")
                            return;
                        CorrectEmail = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                        emailFlag = false;
                    }
                    OnPropertyChanged("CorrectEmail");
                }));
            }
        }

        private RelayCommand loginGot;
        public RelayCommand LoginGot
        {
            get
            {
                return loginGot ?? (loginGot = new RelayCommand(obj =>
                {
                    if (regClient.Login_Client == "логин" || regClient.Login_Client == "login")
                    {
                        regClient.Login_Client = "";
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
                    if (string.IsNullOrWhiteSpace(regClient.Login_Client))
                    {
                        if (ManagerItem.isRussian)
                            regClient.Login_Client = "логин";
                        else
                            regClient.Login_Client = "login";
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
                    Match match = regex.Match(regClient.Login_Client);

                    if ((regClient.Login_Client.ToString().Contains(allowedchar)) || string.IsNullOrWhiteSpace(regClient.Login_Client) || (match.Success == false))
                    {
                        CorrectLogin = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                        OnPropertyChanged("CorrectLogin");
                        loginFlag = false;
                        return;
                    }
                    foreach (Client client in clients)
                    {
                        if (client.Login_Client == this.regClient.Login_Client)
                        {
                            CorrectLogin = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                            OnPropertyChanged("CorrectLogin");
                            loginFlag = false;
                            return;
                        }
                    }
                    if (regClient.Login_Client == "логин" || regClient.Login_Client == "login")
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
                    if (regClient.Password_Client == "пароль" || regClient.Password_Client == "password")
                    {
                        regClient.Password_Client = "";
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
                    if (string.IsNullOrWhiteSpace(regClient.Password_Client))
                    {
                        if (ManagerItem.isRussian)
                            regClient.Password_Client = "пароль";
                        else
                            regClient.Password_Client = "password";
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
                    RegClient.Password_Client = ManagerItem.PasswordCheck;
                    Regex regex = new Regex(@"^[a-zA-Z0-9]{1,12}$");
                    Match match = regex.Match(regClient.Password_Client);
                    if ((regClient.Password_Client.ToString().Contains(allowedchar)) || string.IsNullOrWhiteSpace(regClient.Password_Client) || (match.Success == false))
                    {
                        CorrectPassword = new BitmapImage(new Uri(@"C:\Users\nikit\Desktop\univer\2cource2sem\CleanBrain\CleanBrain\CleanBrain\Photos\uncorrect.png", UriKind.Absolute));
                        passwordFlag = false;
                        OnPropertyChanged("CorrectPassword");
                        return;
                    }
                    else
                    {
                        if (regClient.Password_Client == "пароль" || regClient.Password_Client == "password")
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

        private RelayCommand authPage;
        public RelayCommand GetAuthorizationPage
        {
            get
            {
                return authPage ?? (authPage = new RelayCommand(obj =>
                {
                    Manager.MainFrame.Navigate(new Authorization());
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

