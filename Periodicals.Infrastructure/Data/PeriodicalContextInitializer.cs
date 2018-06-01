using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Periodicals.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Periodicals.Infrastructure.Identity;
using Periodicals.Core.Identity;
using Periodicals.Core.Entities;

namespace Periodicals.Infrastructure.Data
{
    public class PeriodicalContextInitializer : DropCreateDatabaseIfModelChanges<PeriodicalDbContext>
    //public class PeriodicalContextInitializer : DropCreateDatabaseAlways<PeriodicalDbContext>
    {
        protected override void Seed(PeriodicalDbContext db)
        {

            db.Editions.Add(new Edition()
            {
                Name ="Daily news",
                Price =5,
                ISSN ="1234-1234",
                DateNextPublication = DateTime.UtcNow.AddDays(4),
                Type = "Newspaper",
                Topic = new Topic() { TopicName = "News" },
                Periodicity = 1,
                Description= "an American newspaper based in New York City." +
                " As of May 2016, it was the ninth-most widely circulated " +
                "daily newspaper in the United States. It was founded " +
                "in 1919, and was the first U.S. daily printed in tabloid " +
                "format. Since 2017, it has been owned by the news publishing " +
                "company Tronc.",
                Language="eng"

            });
            db.Editions.Add(new Edition()
            {
                Name = "Cosmopolitan",
                Price = 9,
                ISSN = "0010-9541",
                DateNextPublication = DateTime.UtcNow.AddDays(6),
                Type = "Magazine",
                Topic = new Topic()
                {
                    TopicName = "Fasion"
                },
                Periodicity = 14,
                Description = "Cosmopolitan is an international fashion magazine " +
                "for women, which was formerly titled The Cosmopolitan. The magazine " +
                "was first published and distributed in 1886 in the United States as a " +
                "family magazine; it was later transformed into a literary magazine and " +
                "eventually became a women's magazine (since 1965). Often referred to as " +
                "Cosmo, its content as of 2011 includes articles discussing: relationships, " +
                "sex, health, careers, self-improvement, celebrities, fashion, and beauty. " +
                "Published by Hearst Corporation, Cosmopolitan has 64 international editions " +
                "including: Croatia, Greece, Romania, Estonia, UK, Norway, Australia, Spain, " +
                "Sweden, Malaysia, Singapore, The Middle East Region, Latin America Region, " +
                "Hungary, Finland, Netherlands, South Africa, France, Portugal, Armenia and Russia " +
                "and is printed in 35 languages, and is distributed in more than 110 countries.",
                Language = "eng"
            });
        db.Editions.Add(new Edition()
        {
            Name = "Science",
            Price =12,
            ISSN = "0036-8075",
            DateNextPublication= DateTime.UtcNow.AddDays(1),
            Type = "Scientific journal",
            Topic = new Topic()
            {
                TopicName = "Science"
            },
            Periodicity=7,
            Description = "Science, also widely referred to as Science Magazine, " +
            "is the peer-reviewed academic journal of the American Association for " +
            "the Advancement of Science (AAAS) and one of the world's top academic journals. " +
            "It was first published in 1880, is currently circulated weekly and has a print " +
            "subscriber base of around 130,000. Because institutional subscriptions and online " +
            "access serve a larger audience, its estimated readership is 570,400 people.",
            Language = "eng"
        });
            db.SaveChanges();

            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
            ApplicationUserManager manager = new ApplicationUserManager(store);
            manager.Create(new ApplicationUser()
            {
                UserName = "Admin",
                Id="444"

            }, "admin0000");

            var roleManager = new RoleManager<PeriodicalsRole>(new RoleStore<PeriodicalsRole>(db));
            var roleCreateResult = roleManager.Create(new PeriodicalsRole()
            {
                Id ="222",
                Name = "Administrator",
                Users = { new IdentityUserRole() { RoleId = "222", UserId = "444" } }
            });
            var roleCreateResult1 = roleManager.Create(new PeriodicalsRole("Subscriber"));
            var roleCreateResult2 = roleManager.Create(new PeriodicalsRole("Moderator"));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }
            if (!roleCreateResult1.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult1.Errors));
            }
            if (!roleCreateResult2.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult2.Errors));
            }
        }
    }
}
