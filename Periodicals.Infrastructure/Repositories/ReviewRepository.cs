using Periodicals.Core.Interfaces;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Periodicals.Core.Entities;
using Periodicals.Infrastructure.Data;

namespace Periodicals.Infrastructure.Repositories
{
    public class ReviewRepository : IRepository<Review>
    {

        public Review GetById(int id)
        {
            using (var db = new PeriodicalDbContext())
            {
                var result = (from review in db.Reviews where review.Id == id select review)
                    .Include(e => e.Edition).FirstOrDefault();

                return result;
            }
        }
        public List<Review> List()
        {
            using (var db = new PeriodicalDbContext())
            {
                return db.Reviews.Include(e => e.Edition).ToList();
            }
        }

        public Review Add(Review entity)
        {
            using (var db = new PeriodicalDbContext())
            {
                var edition = db.Editions.Find(entity.EditionId);
                if (edition != null)
                {
                    edition.Reviews.Add(entity);
                    db.SaveChanges();
                }
            }
            return entity;
        }

        public void Delete(int entityId)
        {
            using (var db = new PeriodicalDbContext())
            {
                var entity = db.Reviews.Find(entityId);
                if (entity != null)
                {
                    db.Reviews.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public void Update(Review entity)
        {
            using (var db = new PeriodicalDbContext())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

    }
}
