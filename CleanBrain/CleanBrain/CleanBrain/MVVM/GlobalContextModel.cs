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

namespace CleanBrain.MVVM
{
    public class GlobalContextModel : INotifyPropertyChanged
    {
        private static bool visProcGlobal = false;
        public static bool VisProcGlobal
        {
            get { return visProcGlobal; }
            set { visProcGlobal = value; }
        }

        private RelayCommand selectPsyEvent;

        public RelayCommand SelectPsyEvent
        {
            get
            {
                return selectPsyEvent ?? (selectPsyEvent = new RelayCommand(obj =>
                {
                    if (ManagerItem.ImGuest)
                    {
                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Для начала зарегистрируйтесь" : "To get started, register");
                        window.Show();
                        return;
                    }
                    if (ManagerItem.Booking && ManagerItem.FindProc == true)
                    {
                        if (ManagerItem.ImClient)
                        {
                            Manager.ReviewFrame.Navigate(new AddBooking());
                        }
                        else
                        {
                            WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Вы администратор" : "You are the administrator");
                            window.Show();
                            return;
                        }
                    }                  
                    else
                        Manager.PreviewFrame.Navigate(new ProceduresPage(ManagerItem.Psy.Id_Psychologist));
                }));
            }
        }

        private RelayCommand selectProcEvent;

        public RelayCommand SelectProcEvent
        {
            get
            {
                return selectProcEvent ?? (selectProcEvent = new RelayCommand(obj =>
                {
                    if (ManagerItem.ImGuest)
                    {
                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Для начала зарегистрируйтесь" : "To get started, register");
                        window.Show();
                        return;
                    }
                    if (ManagerItem.Booking && ManagerItem.FindPsy == true)
                    {
                        if(ManagerItem.ImClient)
                        Manager.ReviewFrame.Navigate(new AddBooking());
                        else
                        {
                            WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Вы администратор" : "You are the administrator");
                            window.Show();
                            return;
                        }
                    }
                    else
                        Manager.PreviewFrame.Navigate(new PsychologistsPage(ManagerItem.Proc.Id_Procedure));
                }));
            }
        }

        private RelayCommand delReview;
        public RelayCommand DelReview
        {
            get
            {
                return delReview ?? (delReview = new RelayCommand(obj =>
                {
                    UnitOfWork unit = new UnitOfWork();
                    if (ManagerItem.ImClient)
                    {
                        if(ManagerItem.Rev.Id_Client == ManagerItem.MainId)
                        {
                            unit.Review.Delete(ManagerItem.Rev.Id_Review);
                            unit.Save();
                            Manager.PreviewFrame.Navigate(new ReviewsPage(ManagerItem.MainId));
                        }
                    }
                    else
                    {
                        unit.Review.Delete(ManagerItem.Rev.Id_Review);
                        unit.Save();
                        Manager.PreviewFrame.Navigate(new ReviewsPage(ManagerItem.MainId));
                    }
                    
                }));
            }
        }

        private RelayCommand delOrder;
        public RelayCommand DelOrder
        {
            get
            {
                return delOrder ?? (delOrder = new RelayCommand(obj =>
                {
                    UnitOfWork unit = new UnitOfWork();
                    int id = ManagerItem.Book.Id_Voucher;
                    unit.Booking.Delete(ManagerItem.Book.ID_Booking);
                    unit.Voucher.Delete(id);
                    unit.Save();
                    Manager.PreviewFrame.Navigate(new OrdersPage());
                }));
            }
        }
        private RelayCommand openEdit;

        public RelayCommand OpenEdit
        {
            get
            {
                return openEdit ?? (openEdit = new RelayCommand(obj =>
                {
                    Manager.PreviewFrame.Navigate(new DoctorPage(ManagerItem.Psy.Id_Psychologist));
                }));
            }
        }
        private RelayCommand openEditProc;

        public RelayCommand OpenEditProc
        {
            get
            {
                return openEditProc ?? (openEditProc = new RelayCommand(obj =>
                {
                    Manager.ReviewFrame.Navigate(new ProcedurePage(ManagerItem.Proc.Id_Procedure));
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
