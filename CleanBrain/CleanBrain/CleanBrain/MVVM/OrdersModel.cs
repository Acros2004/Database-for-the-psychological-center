using CleanBrain.Command;
using CleanBrain.ManagerFrame;
using CleanBrain.UoF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.MVVM
{
    public class OrdersModel : INotifyPropertyChanged
    {
        private List<Booking> bookings;
        private Booking selectBooking = new Booking();
        private UnitOfWork unit = new UnitOfWork();
        private Voucher voucher;
        public List<Booking> Bookings
        {
            get { return bookings; }
            set
            {
                bookings = value;
                OnPropertyChanged("Bookings");
            }
        }
        public Booking SelectBooking
        {
            get { return selectBooking; }
            set { selectBooking = value; OnPropertyChanged("SelectProc"); ManagerItem.Book = value; }
        }

        public OrdersModel()
        {

            List<Booking> temp = unit.Booking.GetAll().Where(item => item.Id_Client == ManagerItem.MainId).ToList();
            DateTime time = DateTime.Now;
            foreach (Booking booking in temp)
            {
                voucher = unit.Voucher.Get(booking.Id_Voucher);
                if(voucher.Date_Voucher < time)
                {
                    unit.Voucher.DeleteVoucher(voucher);
                    unit.Booking.DeleteBooking(booking);
                }
            }
            unit.Save();
            unit = new UnitOfWork();
            Bookings = unit.Booking.GetAll().Where(item => item.Id_Client == ManagerItem.MainId).ToList();

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
