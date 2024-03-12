using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Repository
{
    public class PsychologistRepository : IRepository<Psychologist>
    {
        private Psychological_CenterEntities db;

        public PsychologistRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Psychologist> GetAll()
        {
            return db.Psychologists;
        }
        public Psychologist Get(int id)
        {
            return db.Psychologists.Find(id);
        }
        public void Create(Psychologist psycholog)
        {
            db.Psychologists.Add(psycholog);
        }
        public void Update(Psychologist psycholog)
        {
            db.Entry(psycholog).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Psychologist psycholog = db.Psychologists.Find(id);
            if (psycholog != null)
                db.Psychologists.Remove(psycholog);
        }
    }
}
