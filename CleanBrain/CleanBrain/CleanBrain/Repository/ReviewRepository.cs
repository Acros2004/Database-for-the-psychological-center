using CleanBrain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrain.Repository
{
    public class ReviewRepository : IRepository<Review>
    {
        private Psychological_CenterEntities db;

        public ReviewRepository(Psychological_CenterEntities context)
        {
            this.db = context;
        }

        public IEnumerable<Review> GetAll()
        {
            return db.Reviews;
        }
        public Review Get(int id)
        {
            return db.Reviews.Find(id);
        }
        public void Create(Review review)
        {
            db.Reviews.Add(review);
        }
        public void Update(Review review)
        {
            db.Entry(review).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(int id)
        {
            Review review = db.Reviews.Find(id);
            if (review != null)
                db.Reviews.Remove(review);
        }
    }
}
