using Periodicals.Core.Interfaces;
using Periodicals.Core.SharedKernel;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Periodicals.Core;
using Periodicals.Core.Entities;
using Periodicals.Infrastructure.Data;

namespace Periodicals.Infrastructure.Repositories
{
    public class TopicRepository : IRepository<Topic>
    {

        public Topic GetById(int id)
        {
            using (var db = new PeriodicalDbContext())
            {
                var result = (from topic in db.Topics where topic.Id == id select topic)
                    .Include(e => e.Editions).FirstOrDefault();

                return result;
            }
        }
        public List<Topic> List()
        {
            using (var db = new PeriodicalDbContext())
            {
                return db.Topics.ToList();
            }
        }

        public Topic Add(Topic entity)
        {
            using (var db = new PeriodicalDbContext())
            {
                db.Topics.Add(entity);
                db.SaveChanges();
                return entity;
            }
        }

        public void Delete(int entityId)
        {
            using (var db = new PeriodicalDbContext())
            {
                var entity = db.Topics.Find(entityId);
                if (entity != null)
                {
                    db.Topics.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public void Update(Topic entity)
        {
            using (var db = new PeriodicalDbContext())
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

    }
}
