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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CleanBrain.MVVM
{
    public class ReviewModel : INotifyPropertyChanged
    {
        private UnitOfWork unit = new UnitOfWork();
        private Review review = new Review();
        private Client client;
        private string textReview;
        private BitmapImage image;
        private Brush correctText = new SolidColorBrush(Color.FromRgb(176, 42, 42));
        public ReviewModel(int id)
        {
            client = unit.Client.Get(id);
            review.Id_Client = id;
            review.Name_Client = client.Name_Client;
            review.Photo_Review = client.Photo_Client;
            Image = GetImage(client.Photo_Client);
        }
        public Brush CorrectText
        {
            get { return correctText; }
            set
            {
                correctText = value;
                OnPropertyChanged("CorrectText");
            }
        }
        public string TextReview
        {
            get { return textReview; }
            set { textReview = value; OnPropertyChanged("TextReview"); }
        }
        public BitmapImage Image
        {
            get { return image; }
            set { image = value; OnPropertyChanged("Image"); }
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

        private RelayCommand changeText;
        public RelayCommand ChangeText
        {
            get
            {
                return changeText ?? (changeText = new RelayCommand(obj =>
                {

                    Regex regex = new Regex(@"^[a-zA-Zа-яА-ЯёЁ0-9\s\-.,!?():;]{1,130}$");
                    Match match = regex.Match(TextReview);
                    if ((match.Success))
                    {
                        CorrectText = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    }
                    if ((match.Success == false))
                    {
                        CorrectText = new SolidColorBrush(Color.FromRgb(176, 42, 42));
                    }
                }));
            }
        }

        private RelayCommand saveReview;

        public RelayCommand SaveReview
        {
            get
            {
                return saveReview ?? (saveReview = new RelayCommand(obj =>
                {
                    if (CorrectText is SolidColorBrush textcolor)
                        if (textcolor.Color.Equals(Colors.Black))
                        {
                            review.Review1 = TextReview;
                            unit.Review.Create(review);
                            unit.Save();
                            Manager.ReviewFrame.Content = null;
                        }
                        else
                            MessageBox.Show("Неккоректные данные");
                    
                        
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
