using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Repository
{
    public class DegreeRepository : IRepositoryString<Academic_Degree>
    {
        private Psychological_CenterEntities db;

        public DegreeRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Academic_Degree> GetAll()
        {
            return db.Academic_Degree;
        }
        public Academic_Degree Get(string value)
        {
            return db.Academic_Degree.Find(value);
        }
        public void Create(Academic_Degree degree)
        {
            db.Academic_Degree.Add(degree);
        }
        public void Update(Academic_Degree degree)
        {
            db.Entry(degree).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(string id)
        {
            Academic_Degree degree = db.Academic_Degree.Find(id);
            if (degree != null)
                db.Academic_Degree.Remove(degree);
        }
    }
}
