using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using CleanBrain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CleanBrain.MVVM
{
    public class PreviewModel : INotifyPropertyChanged
    {
        private Client selectedClient;
        private BitmapImage imageClient;
        private string previewName;
        private Uri currentPage;
        private UnitOfWork unit = new UnitOfWork();
        private bool isFrameVisible = false;

        
        

        public string PreviewName
        {
            get { return previewName; }
            set 
            { 
                previewName = value;
                OnPropertyChanged("PreviewName");
            }
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
        public Uri CurrentPage 
        { 
            get { return currentPage; }
            set { currentPage = value; }
        }
        public BitmapImage ImageClient
        {
            get { return imageClient; }
            set 
            { 
                imageClient = value;
                OnPropertyChanged("ImageClient");
            }
        }
        public Client SelectedClient
        {
            get { return selectedClient; }
            set 
            { 
                selectedClient = value;
            }
        }

        public PreviewModel(Client client)
        {
            selectedClient= client;
            if (client.Login_Client != "nik")
                ManagerItem.ImClient = true;
            else
                ManagerItem.ImClient = false;
            if (client.Login_Client != "test")
                ManagerItem.ImGuest = false;
            else
                ManagerItem.ImGuest = true;
            ImageClient = GetImage(client.Photo_Client);
            PreviewName = client.Name_Client;
            ManagerItem.MainId = client.Id_client;
            Manager.PreviewFrame.Navigate(new GreetingPage());
        }

        private RelayCommand getProcedures;
        public RelayCommand GetProcedures
        {
            get
            {
                return getProcedures ?? (getProcedures = new RelayCommand(obj =>
                {
                    Manager.PreviewFrame.Navigate(new ProceduresPage());
                }));
            }
        }

        private RelayCommand openOrdersPage;

        public RelayCommand OpenOrdersPage
        {
            get
            {
                return openOrdersPage ?? (openOrdersPage = new RelayCommand(obj =>
                {
                    if (ManagerItem.ImGuest)
                    {
                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Для начала зарегистрируйтесь" : "To get started, register");
                        window.Show();
                        return;
                    }
                    if (!ManagerItem.ImClient)
                    {
                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Вы администратор" : "You are the administrator");
                        window.Show();
                        return;
                    }
                    Manager.PreviewFrame.Navigate(new OrdersPage());
                }));
            }
        }
        private RelayCommand getAbout;

        public RelayCommand GetAbout
        {
            get
            {
                return getAbout ?? (getAbout = new RelayCommand(obj =>
                {
                    Manager.PreviewFrame.Navigate(new AboutPage());
                }));
            }
        }
       

        private RelayCommand getPsychologists;

        public RelayCommand GetPsychologists
        {
            get
            {
                return getPsychologists ?? (getPsychologists = new RelayCommand(obj =>
                {
                    Manager.PreviewFrame.Navigate(new PsychologistsPage());
                }));
            }
        }

        private RelayCommand getReviews;

        public RelayCommand GetReviews
        {
            get
            {
                return getReviews ?? (getReviews = new RelayCommand(obj =>
                {
                    Manager.PreviewFrame.Navigate(new ReviewsPage(SelectedClient.Id_client));
                }));
            }
        }

        private RelayCommand closeBorderClick;

        public RelayCommand CloseBorderClick
        {
            get
            {
                return closeBorderClick ?? (closeBorderClick = new RelayCommand(obj =>
                {
                    Manager.ProfileFrame.Content = null;
                    IsFrameVisible = false;
                }));
            }
        }

        private RelayCommand closeBorder;

        public RelayCommand CloseBorder
        {
            get
            {
                return closeBorder ?? (closeBorder = new RelayCommand(obj =>
                {
                    if(Manager.ProfileFrame.Content == null)
                    {
                        unit = new UnitOfWork();
                        IsFrameVisible = false;
                        SelectedClient = unit.Client.Get(selectedClient.Id_client);
                        ImageClient = GetImage(SelectedClient.Photo_Client);
                        PreviewName = SelectedClient.Name_Client;
                        OnPropertyChanged("ImageClient");
                    }
                        
                }));
            }
        }

        private RelayCommand profile;

        public RelayCommand OpenProfile
        {
            get
            {
                return profile ?? (profile = new RelayCommand(obj =>
                {
                    if (ManagerItem.ImGuest)
                    {
                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Для начала зарегистрируйтесь" : "To get started, register");
                        window.Show();
                        return;
                    }
                    IsFrameVisible = true;
                    Manager.ProfileFrame.Navigate(new Profile(selectedClient.Id_client, IsFrameVisible));
                }));
            }
        }

        private RelayCommand openSettings;

        public RelayCommand OpenSettings
        {
            get
            {
                return openSettings ?? (openSettings = new RelayCommand(obj =>
                {
                    IsFrameVisible = true;
                    Manager.ProfileFrame.Navigate(new Settings());
                }));
            }
        }

        
        private BitmapImage GetImage(byte[] mass)
        {
            using (var ms = new System.IO.MemoryStream(mass))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
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
