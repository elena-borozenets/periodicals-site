﻿using Periodicals.Core.Interfaces;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Periodicals.Core.Entities;
using Periodicals.Infrastructure.Data;

namespace Periodicals.Infrastructure.Repositories
{
    public class EditionRepository : IRepository<Edition>
    {

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
                var resultUser = (from user in db.Users where user.Id == userId select user)
                    .Include(e => e.Subscription).FirstOrDefault();
                var resultEdition = (from edition in db.Editions where edition.Id == editionId select edition)
                    .Include(e => e.Subscribers).FirstOrDefault();

                if (resultEdition!=null&&resultUser!=null&&resultUser.Credit >= resultEdition.Price)
                {
                    resultUser.Credit -= resultEdition.Price;
                    resultUser.Subscription.Add(resultEdition);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public bool RemoveSubscription(string userId, int editionId)
        {
            using (var db = new PeriodicalDbContext())
                {
                    var resultUser = (from user in db.Users where user.Id == userId select user)
                        .Include(e => e.Subscription).FirstOrDefault();
                    var resultEdition = (from edition in db.Editions where edition.Id == editionId select edition)
                        .Include(e => e.Subscribers).FirstOrDefault();
                    if (resultUser!=null&&resultUser.Subscription.Contains(resultEdition))
                    {
                        resultUser.Subscription.Remove(resultEdition);
                        //editionDb?.Subscribers.Remove(user);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
    }
}
