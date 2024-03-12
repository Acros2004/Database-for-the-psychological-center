using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.MVVM
{
    public class GreetingModel : INotifyPropertyChanged
    {
        private RelayCommand getReviews;
        public RelayCommand GetReview
        {
            get
            {
                return getReviews ??
                    (getReviews = new RelayCommand(obj =>
                    {
                        Manager.PreviewFrame.Navigate(new ReviewsPage(ManagerItem.MainId));
                    }
                    ));
            }
        }
        private RelayCommand getPsy;
        public RelayCommand GetPsy
        {
            get
            {
                return getPsy ??
                    (getPsy = new RelayCommand(obj =>
                    {
                        Manager.PreviewFrame.Navigate(new PsychologistsPage());
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
