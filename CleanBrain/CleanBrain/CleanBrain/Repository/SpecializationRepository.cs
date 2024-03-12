using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanBrain.Interfaces;

namespace CleanBrain.Repository
{
    public class SpecializationRepository : IRepositoryString<Specialization>
    {
        private Psychological_CenterEntities db;

    public SpecializationRepository(Psychological_CenterEntities context)
    {
        this.db = context;
    }

    public IEnumerable<Specialization> GetAll()
    {
        return db.Specializations;
    }
    public Specialization Get(string value)
    {
        return db.Specializations.Find(value);
    }
    public void Create(Specialization spec)
    {
        db.Specializations.Add(spec);
    }
    public void Update(Specialization spec)
    {
        db.Entry(spec).State = System.Data.Entity.EntityState.Modified;
    }
    public void Delete(string id)
    {
        Specialization spec = db.Specializations.Find(id);
        if (spec != null)
            db.Specializations.Remove(spec);
    }
}
}
