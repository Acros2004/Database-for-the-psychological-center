using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Repository
{
    public class BookingRepository : IRepository<Booking>
    {
        private Psychological_CenterEntities db;

        public BookingRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Booking> GetAll()
        {
            return db.Bookings;
        }
        public Booking Get(int id)
        {
            return db.Bookings.Find(id);
        }
        public void Create(Booking client)
        {
            db.Bookings.Add(client);
        }
        public void Update(Booking client)
        {
            db.Entry(client).State = System.Data.Entity.EntityState.Modified;
        }
        public void DeleteBookingByIdPsy(int id)
        {
            List<Booking> list = db.Bookings.Where(item => item.Id_Psychologist == id).ToList();
            foreach(Booking book in list)
            {
                db.Bookings.Remove(book);
            }
        }
        public void DeleteBooking(Booking book)
        {
            db.Bookings.Remove(book);
        }
        public void Delete(int id)
        {
            Booking client = db.Bookings.Find(id);
            if (client != null)
                db.Bookings.Remove(client);
        }
    }
}
