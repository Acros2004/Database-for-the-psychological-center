using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CleanBrain.MVVM
{
    public class ProfileModel : INotifyPropertyChanged
    {
        private Client client;
        private string name;
        private string surname;
        private string mail;
        byte[] imageBytes;
        private bool isFrameVisible;
        private Brush correctName = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private Brush correctSurname = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private Brush correctEmail = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        private UnitOfWork unit = new UnitOfWork();


        private BitmapImage image;
        public ProfileModel( int id, bool frameVis ) 
        {
            Client = unit.Client.Get(id);
            Image = GetImage(client.Photo_Client);
            NameProf = client.Name_Client;
            SurnameProf = client.Surname_Client;
            MailProf = client.Mail_Client;
            isFrameVisible = frameVis;
        }
        public bool IsFrameVisible
        {
            get { return isFrameVisible; }
            set
            {
                isFrameVisible = value;
                OnPropertyChanged("IsFrameVisible");
            }
        }

        public Brush CorrectName
        {
            get { return correctName; }
            set 
            { 
                correctName = value;
                OnPropertyChanged("CorrectName");
            }
        }
        public Brush CorrectSurname
        {
            get { return correctSurname; }
            set
            {
                correctSurname = value;
                OnPropertyChanged("CorrectSurname");
            }
        }
        public Brush CorrectEmail
        {
            get { return correctEmail; }
            set
            {
                correctEmail = value;
                OnPropertyChanged("CorrectEmail");
            }
        }
        public string NameProf
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged("NameProf");
            }
        }
        public string SurnameProf
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("SurnameProf");
            }
        }
        public string MailProf
        {
            get { return mail; }
            set 
            {
                mail = value;
                OnPropertyChanged("MailProf");
            }
        }


        public BitmapImage Image
        {
            get { return image; }
            set 
            { 
                image = value;
                OnPropertyChanged("Image");
            }
        }

        public Client Client 
        { 
            get { return client; }
            set { this.client = value; }
        }

        private RelayCommand changeImage;

        public RelayCommand ChangeImage
        {
            get
            {
                return changeImage ?? (changeImage = new RelayCommand(obj =>
                {
                    string path;
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

                    if (openFile.ShowDialog() == true)
                    {
                        path = openFile.FileName;
                        byte[] imageBytesBuffer;

                        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            imageBytesBuffer = new byte[fs.Length];
                            fs.Read(imageBytesBuffer, 0, imageBytesBuffer.Length);
                        }
                        imageBytes = imageBytesBuffer;
                        Image = GetImage(imageBytesBuffer);
                    }
                }));
            }
        }

        private RelayCommand saveProf;

        public RelayCommand SaveProf
        {
            get
            {
                return saveProf ?? (saveProf = new RelayCommand(obj =>
                {
                    if (CorrectName is SolidColorBrush namecolor && CorrectSurname is SolidColorBrush surnameColor && CorrectEmail is SolidColorBrush mailColor)
                        if (namecolor.Color.Equals(Colors.Black) && surnameColor.Color.Equals(Colors.Black) && mailColor.Color.Equals(Colors.Black))
                        {
                            if (NameProf == Client.Name_Client && SurnameProf == Client.Surname_Client && MailProf == Client.Mail_Client && imageBytes == null)
                            {
                                Manager.ProfileFrame.Content = null;

                            }
                            else
                            {
                                Client.Name_Client = NameProf;
                                Client.Surname_Client = SurnameProf;
                                Client.Mail_Client = MailProf;
                                if(imageBytes != null)
                                Client.Photo_Client = imageBytes;
                                unit.Client.Update(Client);
                                unit.Save();

                                Manager.ProfileFrame.Content = null;
                            }
                        }
                        else
                        {
                            WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Введённые данные не валидны" : "The entered data is not valid");
                            window.Show();
                            return;
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
                    if (MailProf == Client.Mail_Client)
                    {
                        MailProf = "";
                    }
                    OnPropertyChanged("MailProf");
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
                    if (string.IsNullOrWhiteSpace(MailProf))
                    {
                        MailProf = Client.Mail_Client;
                        CorrectEmail = new SolidColorBrush(Color.FromRgb(0,0,0));
                    }
                    OnPropertyChanged("MailProf");
                    OnPropertyChanged("CorrectEmail");
                }));
            }
        }

        private RelayCommand changeMail;

        public RelayCommand ChangeMail
        {
            get 
            {
                return changeMail ?? (changeMail = new RelayCommand(obj =>
                {
                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regex.Match(MailProf);
                    if ((match.Success))
                    {
                        CorrectEmail = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        if (MailProf == Client.Mail_Client)
                            return;
                        CorrectEmail = new SolidColorBrush(Color.FromRgb(176,42,42));
                    }
                    OnPropertyChanged("CorrectEmail");
                }));
            }
        }

        private RelayCommand surnameGot;
        public RelayCommand SurnameGot
        {
            get
            {
                return surnameGot ?? (surnameGot = new RelayCommand(obj =>
                {
                    if (SurnameProf == Client.Surname_Client)
                    {
                        SurnameProf = "";
                    }
                }));
            }
        }

        private RelayCommand surnameLost;
        public RelayCommand SurnameLost
        {
            get
            {
                return surnameLost ?? (surnameLost = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(SurnameProf))
                    {
                        SurnameProf = Client.Surname_Client;
                        CorrectSurname = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                }));
            }
        }

        private RelayCommand changeSurname;

        public RelayCommand ChangeSurname
        {
            get
            {
                return changeSurname ?? (changeSurname = new RelayCommand(obj =>
                {
                    Regex regex = new Regex(@"^[a-zA-Zа-яА-Я]{1,15}$");
                    Match match = regex.Match(SurnameProf);
                    if ((match.Success))
                    {
                        CorrectSurname = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        if (SurnameProf == Client.Surname_Client)
                            return;
                        CorrectSurname = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }

        private RelayCommand nameGot;
        public RelayCommand NameGot
        {
            get
            {
                return nameGot ?? (nameGot = new RelayCommand(obj =>
                {
                    if (NameProf == Client.Name_Client)
                    {
                        NameProf = "";
                    }
                }));
            }
        }

        private RelayCommand nameLost;
        public RelayCommand NameLost
        {
            get
            {
                return nameLost ?? (nameLost = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(NameProf))
                    {
                        NameProf = Client.Name_Client;
                        CorrectName = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                }));
            }
        }

        private RelayCommand changeName;

        public RelayCommand ChangeName
        {
            get
            {
                return changeName ?? (changeName = new RelayCommand(obj =>
                {
                    Regex regex = new Regex(@"^[a-zA-Zа-яА-Я]{1,15}$");
                    Match match = regex.Match(NameProf);
                    if ((match.Success))
                    {
                        CorrectName = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        if (NameProf == Client.Name_Client)
                            return;
                        CorrectName = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }


        private BitmapImage GetImage(byte[] mass)
        {
            using (var ms = new System.IO.MemoryStream(mass))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
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
