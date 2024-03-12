using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Repository
{
    public class ProcedureRepository : IRepository<Procedure>
    {
        private Psychological_CenterEntities db;

        public ProcedureRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Procedure> GetAll()
        {
            return db.Procedures;
        }
        public Procedure Get(int id)
        {
            return db.Procedures.Find(id);
        }
        public void Create(Procedure proc)
        {
            db.Procedures.Add(proc);
        }
        public void Update(Procedure proc)
        {
            db.Entry(proc).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Procedure proc = db.Procedures.Find(id);
            if (proc != null)
                db.Procedures.Remove(proc);
        }
    }
}
