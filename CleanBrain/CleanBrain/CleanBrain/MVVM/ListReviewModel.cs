using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.UoF;
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
    public class ListReviewModel : INotifyPropertyChanged
    {
        private int id_Client;
        private Review selectReview = new Review();
        private List<Review> _list;
        private UnitOfWork unit = new UnitOfWork();
        private bool isFrameVisible;
        private bool active = true;

        public bool Active
        {
            get { return active; }
            set { active = value; OnPropertyChanged("Active"); }
        }

        public ListReviewModel(int id)
        {
            id_Client = id;
            if (ManagerItem.ImGuest)
                Active = false;
            List<Review> list = new List<Review>(unit.Review.GetAll());
            List<Client> clients = new List<Client>(unit.Client.GetAll());
            foreach(Review review in list)
            {
                Client foundClient = null;
                foreach(Client client in clients)
                {
                    if(review.Id_Client == client.Id_client)
                    {
                        if (client.Id_client == ManagerItem.MainId)
                            Active = false;
                        foundClient = client; 
                        break;
                    }
                }
                review.Name_Client = foundClient.Name_Client;
                review.Photo_Review = foundClient.Photo_Client;
                unit.Review.Update(review);
            }
            unit.Save();
            unit = new UnitOfWork();
            List = list;
        }

        public Review SelectReview
        {
            get { return selectReview; }
            set { selectReview = value; OnPropertyChanged("SelectReview"); ManagerItem.Rev = value; }
        }
        public List<Review> List
        {
            get { return _list; }
            set { _list = value; OnPropertyChanged("List"); }
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

        private RelayCommand closeBorderClick;

        public RelayCommand CloseBorderClick
        {
            get
            {
                return closeBorderClick ?? (closeBorderClick = new RelayCommand(obj =>
                {
                    Manager.ReviewFrame.Content = null;
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
                    if (Manager.ReviewFrame.Content == null)
                    {
                        IsFrameVisible = false;
                        unit = new UnitOfWork();
                        List = new List<Review>(unit.Review.GetAll());
                        List<Review> tempCheck = unit.Review.GetAll().Where(item => item.Id_Client == ManagerItem.MainId).ToList();
                        if (tempCheck != null)
                            Active = false;
                        
                    }

                }));
            }
        }

        private RelayCommand addReview;

        public RelayCommand AddReview
        {
            get
            {
                return addReview ?? (addReview = new RelayCommand(obj =>
                {
                    Manager.ReviewFrame.Navigate(new ReviewClient(id_Client));
                    IsFrameVisible = true;
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
