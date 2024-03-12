using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Repository
{
    public class VoucherRepository : IRepository<Voucher>
    {
        private Psychological_CenterEntities db;

        public VoucherRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Voucher> GetAll()
        {
            return db.Vouchers;
        }
        public void DeleteVoucher(Voucher vou)
        {
            db.Vouchers.Remove(vou);
        }
        public Voucher Get(int id)
        {
            return db.Vouchers.Find(id);
        }
        public void Create(Voucher table)
        {
            db.Vouchers.Add(table);
        }
        public void Update(Voucher table)
        {
            db.Entry(table).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Voucher table = db.Vouchers.Find(id);
            if (table != null)
                db.Vouchers.Remove(table);
        }
        public void DeleteAll()
        {
            List<Voucher> vouchers = db.Vouchers.ToList();
            foreach(Voucher voucher in vouchers)
            {
                db.Vouchers.Remove(voucher);
            }
        }
        
        public void DeleteNotOrdered(int id)
        {
            List<Voucher> vouchers = db.Vouchers.Where(item => item.Ordered.Contains("Нет") && item.Id_Psychologist == id).ToList();
            foreach (Voucher voucher in vouchers)
            {
                db.Vouchers.Remove(voucher);
            }
        }
        public void DeleteOldVouchers(int id)
        {

            List<Voucher> vouchers = db.Vouchers.Where(item => item.Id_Psychologist == id && item.Date_Voucher < DateTime.Now).ToList();
            foreach(Voucher voucher in vouchers)
            {
                if (voucher.Ordered.Contains("Нет"))
                {
                    db.Vouchers.Remove(voucher);
                }
                else
                {
                    List<Booking> bookings = db.Bookings.Where(item => item.Id_Psychologist == id && item.Id_Voucher == voucher.Id_Voucher).ToList();
                    foreach(Booking book in bookings)
                    {
                        db.Bookings.Remove(book);
                    }
                    db.Vouchers.Remove(voucher);

                }
            }
        }
        public void DeleteAllById(int id)
        {
            List<Voucher> vouchers = db.Vouchers.Where(item => item.Id_Psychologist == id).ToList();
            foreach (Voucher voucher in vouchers)
            {
                db.Vouchers.Remove(voucher);
            }
        }
    }
}
