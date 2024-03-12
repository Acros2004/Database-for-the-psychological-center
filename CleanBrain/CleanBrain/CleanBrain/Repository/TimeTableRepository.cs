using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CleanBrain.Repository
{
    public class TimeTableRepository : IRepository<Timetable>
    {
        private Psychological_CenterEntities db;

        public TimeTableRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Timetable> GetAll()
        {
            return db.Timetables;
        }
        public Timetable Get(int id)
        {
            return db.Timetables.Find(id);
        }
        public void Create(Timetable table)
        {
            db.Timetables.Add(table);
        }
        public void Update(Timetable table)
        {
              db.Entry(table).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Timetable table = db.Timetables.Find(id);
            if (table != null)
                db.Timetables.Remove(table);
        }
    }
}
