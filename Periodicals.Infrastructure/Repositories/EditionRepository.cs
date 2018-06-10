using Periodicals.Core.Interfaces;
using Periodicals.Core.SharedKernel;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Periodicals.Core;
using Periodicals.Core.Entities;
using Periodicals.Core.Identity;
using Periodicals.Infrastructure.Data;

namespace Periodicals.Infrastructure.Repositories
{
    public class EditionRepository : IRepository<Edition>
    {
        /*private readonly PeriodicalDbContext _dbContext;

        public EditionRepository(PeriodicalDbContext dbContext)
        {
            _dbContext = dbContext;
        }*/

        public Edition GetById(int id)
        {
            using (var db = new PeriodicalDbContext())
            {
                var result = (from edition in db.Editions where edition.Id == id select edition)
                    .Include(e => e.Reviews)
                    .Include(e => e.Topic)
                    .Include(e => e.Subscribers).FirstOrDefault();

                return result;
            }
        }
        public List<Edition> List()
        {
            using (var db = new PeriodicalDbContext())
            {
                return db.Editions.Include(e => e.Reviews)
                    .Include(e=>e.Topic)
                    .Include(e => e.Subscribers).ToList();
            }
        }

        public Edition Add(Edition entity)
        {
            using (var db = new PeriodicalDbContext())
            {
                var topic = db.Topics.FirstOrDefault(t => t.TopicName == entity.Topic.TopicName);

                if (topic == null)
                {
                    topic = new Topic() {TopicName = entity.Topic.TopicName};
                    db.Topics.Add(topic);
                }
                
                entity.Topic = topic;
                db.Editions.Add(entity);
                db.SaveChanges();
            }
            return entity;
        }

        public void Delete(int entityId)
        {
            using (var db = new PeriodicalDbContext())
            {
                var entity = db.Editions.Find(entityId);
                if(entity!=null)
                { 
                    db.Editions.Remove(entity);
                    db.SaveChanges();
                }
            }
        }

        public void Update(Edition entity)
        {
            using (var db = new PeriodicalDbContext())
            {
                db.Entry(entity).State = EntityState.Modified;
               db.SaveChanges();
            }
        }

        public bool AddSubscription(string userId, int editionId)
        {
                using (var db = new PeriodicalDbContext())
                {
                    var user = db.Users.Find(userId);
                    var editionDb = db.Editions.Find(editionId);
                    //user.Subscription.Add(editionDb);
                    editionDb?.Subscribers.Add(user);
                    db.SaveChanges();
                    return true;
                }
        }

        public bool RemoveSubscription(string userId, int editionId)
        {
            using (var db = new PeriodicalDbContext())
                {
                    var user = db.Users.Find(userId);
                    var editionDb = db.Editions.Find(editionId);
                    if (user.Subscription.Contains(editionDb))
                    {
                        user.Subscription.Remove(editionDb);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
    }
}
