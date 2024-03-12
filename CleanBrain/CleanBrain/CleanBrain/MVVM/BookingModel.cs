using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.Pages;
using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CleanBrain.MVVM
{
    public class BookingModel : INotifyPropertyChanged
    {
        private UnitOfWork unit = new UnitOfWork();
        private Voucher voucherUpd;
        private int id_client;
        private int id_psy;
        private int id_proc;
        private int id_vou;
        private string nameProc;
        private string fioPsy;
        private string selectDate;
        private string selectTime;
        private List<Voucher> vouchers;
        private List<string> dateMass = new List<string>();
        private ObservableCollection<string> timeMass = new ObservableCollection<string>();

        public string SelectDate
        {
            get { return selectDate; }
            set { selectDate = value; OnPropertyChanged("SelectDate"); }
        }
        public string SelectTime
        {
            get { return selectTime; }
            set { selectTime = value; OnPropertyChanged("SelectTime"); }
        }

        public List<string> DateMass
        {
            get { return dateMass; }
            set { dateMass = value; OnPropertyChanged("DateMass"); }
        }

        public ObservableCollection<string> TimeMass
        {
            get { return timeMass; }
            set { timeMass = value; OnPropertyChanged("TimeMass"); }
        }

        public List<Voucher> Vouchers
        {
            get { return vouchers; }
            set { vouchers = value; OnPropertyChanged("Vouchers"); }
        }
        public string NameProc
        {
            get { return nameProc; }
            set { nameProc = value; OnPropertyChanged("NameProc"); }
        }

        public string FioPsy
        {
            get { return fioPsy; }
            set { fioPsy = value;OnPropertyChanged("FioPsy"); }
        }

        public BookingModel()
        {
            if(ManagerItem.Psy != null)
            {
                unit.Voucher.DeleteOldVouchers(ManagerItem.Psy.Id_Psychologist);
                unit.Save();
            } 
            unit = new UnitOfWork();
            id_client = ManagerItem.MainId;
            id_psy = ManagerItem.Psy.Id_Psychologist;
            id_proc = ManagerItem.Proc.Id_Procedure;
            NameProc = ManagerItem.Proc.Name_Procedure;
            FioPsy =   $"{ManagerItem.Psy.Name_Psychologist} {ManagerItem.Psy.Surname_Psychologist}";
            Vouchers = unit.Voucher.GetAll().Where(item => item.Id_Psychologist == id_psy && item.Ordered.Contains("Нет")).ToList();
            if (ManagerItem.isRussian)
            {
                DateMass.Add("Выберите");
                TimeMass.Add("Выберите");
            }
            else
            {
                DateMass.Add("Choose");
                TimeMass.Add("Choose");
            }
            foreach(Voucher voucher in vouchers)
            {
                bool flag = false;
                string dateVoucher = voucher.Date_Voucher.Date.ToString("yyyy-MM-dd");
                foreach (string date in DateMass)
                {
                    if(date == dateVoucher)
                    {
                        flag = true;
                        break;
                    }
                }
                if(!flag)
                    DateMass.Add(dateVoucher);
            }

        }

        private RelayCommand changeItems;
        public RelayCommand ChangeItems
        {
            get
            {
                return changeItems ?? (changeItems = new RelayCommand(obj =>
                {
                    if(SelectDate != "Выберите" && SelectDate != "Choose")
                    {
                        TimeMass.Clear();
                        if (ManagerItem.isRussian)
                            TimeMass.Add("Выберите");
                        else
                            TimeMass.Add("Choose");
                        List<Voucher> temp = Vouchers.Where(item => item.Date_Voucher.Date.ToString("yyyy-MM-dd") == SelectDate).ToList();
                        foreach(Voucher voucher in temp)
                        {
                            TimeMass.Add($"{voucher.Time_Voucher_Start.ToString().Substring(0, 5)}-{voucher.Time_Voucher_End.ToString().Substring(0, 5)}");
                        }
                        if (ManagerItem.isRussian)
                            SelectTime = "Выберите";
                        else
                            SelectTime = "Choose";
                        OnPropertyChanged("TimeMass");
                    }
                }));
            }
        }

        private void SendMessage()
        {
            // Настройки SMTP-сервера Gmail
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("nikitakarebo810@gmail.com", "tioglimudrtemvgw");

            // Создаем сообщение
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("nikitakarebo810@gmail.com");
            Client client = unit.Client.Get(ManagerItem.MainId);
            mailMessage.To.Add(client.Mail_Client);
            mailMessage.Subject = $"Оформление сеанса: \"{NameProc}\"";
            var htmlBody = $"<html><body><h1 style='font-size: 30px'>Clean Brain</h1> <p style='font-size: 18pt;'>Оформление Сеанса.</p>" +
                $"<p style='font-size: 18pt;'>Здравствуйте, вы оформили услугу: {NameProc}.</p><p style='font-size: 18pt;'>Вашим личным психологом выступит {fioPsy}.Дата сеанса: {SelectDate},на время {SelectTime}.Не опаздывайте)</p></body></html>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            mailMessage.AlternateViews.Add(alternateView);
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
            }
        }

        private RelayCommand saveBooking;

        public RelayCommand SaveBooking
        {
            get
            {
                return saveBooking ?? (saveBooking = new RelayCommand(obj =>
                {
                    if(SelectDate != "Выберите" && SelectDate != "Choose" && SelectTime != "Выберите" && SelectTime != "Choose")
                    {
                        string timeStart = SelectTime.Substring(0, 5);
                        string timeEnd = SelectTime.Substring(6,5);
                        foreach (Voucher voucher in Vouchers)
                        {
                            if (voucher.Time_Voucher_Start.ToString().Substring(0,5) == timeStart && voucher.Time_Voucher_End.ToString().Substring(0,5) == timeEnd && voucher.Date_Voucher.Date.ToString("yyyy-MM-dd") == SelectDate)
                            {
                                voucherUpd = voucher;
                                break;
                            }
                        }
                        id_vou = voucherUpd.Id_Voucher;
                        voucherUpd.Ordered = "Да";
                        unit.Voucher.Update(voucherUpd);
                        unit.Save();
                        unit = new UnitOfWork();
                        DateTime time = DateTime.Now;
                        Booking book = new Booking();
                        book.Date_Booking = time.Date;
                        book.Time_Booking = new TimeSpan(time.Hour, time.Minute, time.Second);
                        book.Id_Client = id_client;
                        book.Id_Psychologist = id_psy;
                        book.Id_Procedure = id_proc;
                        book.Id_Voucher = id_vou;
                        unit.Booking.Create(book);
                        unit.Save();
                        SendMessage();
                        Manager.PreviewFrame.Navigate(new GreetingPage());
                    }
                    else
                    {
                        WarningWindow window = new WarningWindow(ManagerItem.isRussian ? "Заполните поля" : "Fill in the fields");
                        window.Show();
                        return;
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
