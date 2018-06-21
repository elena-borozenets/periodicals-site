using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Periodicals.Infrastructure.Identity;
using Periodicals.Core.Identity;
using Periodicals.Core.Entities;

namespace Periodicals.Infrastructure.Data
{
    //public class PeriodicalContextInitializer : DropCreateDatabaseIfModelChanges<PeriodicalDbContext>
    public class PeriodicalContextInitializer : DropCreateDatabaseAlways<PeriodicalDbContext>
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
                Language="eng",
                Reviews = new List<Review>(),
                Image = "daily_news.jpg"

            });
            db.SaveChanges();
            db.Editions.Add(new Edition()
            {
                Name = "The Guardian",
                Price = 1,
                ISSN = "0261-3077",
                DateNextPublication = DateTime.UtcNow.AddDays(1),
                Type = "Newspaper",
                Topic = new Topic() { TopicName = "Politics" },
                Periodicity = 1,
                Description = "The Guardian is a British daily newspaper. It was known from " +
                              "1821 until 1959 as the Manchester Guardian. Along with its sister " +
                              "papers The Observer and the Guardian Weekly, The Guardian is part " +
                              "of the Guardian Media Group, owned by the Scott Trust. The Trust " +
                              "was created in 1936 to secure the financial and editorial " +
                              "independence of the Guardian in perpetuity and to safeguard " +
                              "the journalistic freedom and liberal values of the Guardian" +
                              " free from commercial or political interference." +
                              " The Scott Trust was converted into a limited company in 2008, " +
                              "with a constitution written so as to project the same protections " +
                              "for the Guardian as were originally built into the very structure of " +
                              "the Trust by its creators. Profits are reinvested in journalism rather " +
                              "than to benefit an owner or shareholders.",
                Language = "eng",
                Reviews = new List<Review>(),
                Image = "The_Guardian.jpg"

            });
            db.SaveChanges();
            db.Editions.Add(new Edition()
            {
                Name = "The Wall Street Journal",
                Price = 1.2f,
                ISSN = "0099-9660",
                DateNextPublication = DateTime.UtcNow.AddDays(1),
                Type = "Newspaper",
                Topic = db.Topics.FirstOrDefault(t => t.TopicName == "Politics"),
                Periodicity = 1,
                Description = "The Wall Street Journal is a U.S. business-focused, " +
                              "English-language international daily newspaper based in " +
                              "New York City. The Journal, along with its Asian and European " +
                              "editions, is published six days a week by Dow Jones & Company, " +
                              "a division of News Corp. The newspaper is published in the broadsheet format and online." +
                              "The Wall Street Journal is the largest newspaper in the United States" +
                              " by circulation.According to News Corp, in its June 2017 10 - K filing " +
                              "with the Securities and Exchange Commission, " +
                              "the Journal had a circulation of about 2.277 million " +
                              "copies(including nearly 1, 270, 000 digital " +
                              "subscriptions) as of June 2017, compared with USA Today's " +
                              "1.7 million. The newspaper, which has won 40 Pulitzer Prizes through 2017, " +
                              "derives its name from Wall Street in the heart of the Financial " +
                              "District of Lower Manhattan.The Journal has been printed continuously " +
                              "since its inception on July 8, 1889, by Charles Dow, Edward Jones, " +
                              "and Charles Bergstresser. The Journal also publishes the " +
                              "luxury news and lifestyle magazine WSJ. The Journal launched " +
                              "an online version in 1996, which has been accessible " +
                              "only to subscribers since it began.",
                Language = "eng",
                Reviews = new List<Review>(),
                Image = "the_wall_street_journal.jpg"

            });
            db.Editions.Add(new Edition()
            {
                Name = "Reader’s Digest",
                Price = 10,
                ISSN = "0034-0375",
                DateNextPublication = DateTime.UtcNow.AddDays(12),
                Type = "Magazine",
                Topic = new Topic() { TopicName = "General-interest family" },
                Periodicity = 30,
                Description = "Reader's Digest is an American general-interest family magazine, " +
                              "published ten times a year. Formerly based in Chappaqua, New York, " +
                              "it is now headquartered in Midtown Manhattan. The magazine was founded " +
                              "in 1920, by DeWitt Wallace and Lila Bell Wallace. For many years, " +
                              "Reader's Digest was the best-selling consumer magazine in the United " +
                              "States; it lost the distinction in 2009 to Better Homes and Gardens. " +
                              "According to Mediamark Research (2006), Reader's Digest reaches more " +
                              "readers with household incomes of $100,000+ than Fortune, The Wall " +
                              "Street Journal, Business Week, and Inc. combined.",
                Language = "eng",
                Reviews = new List<Review>(),
                Image = "readers_digest.jpg"

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
                Language = "eng",
                Reviews = new List<Review>(),
                Image = "cosmopolitan.jpg"
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
            Language = "eng",
            Reviews = new List<Review>() {
                new Review() {NameAuthor = "Scientist", TextReview = "Like this journal!", TimeCreation = DateTime.UtcNow},
                new Review() {NameAuthor = "Someone", TextReview = "Don't understand some teories", TimeCreation = DateTime.UtcNow}},
            Image = "science.jpg"
        });
            db.Editions.Add(new Edition()
            {
                Name = "Факты и комментарии",
                Price = 0.4f,
                ISSN = "0736-8075",
                DateNextPublication = DateTime.UtcNow.AddDays(1),
                Type = "Newspaper",
                Topic = db.Topics.FirstOrDefault(t=>t.TopicName == "News"),
                Periodicity = 1,
                Description = "«Факты и комментарии» («Факты») — pусскоязычная всеукраинская " +
                              "ежедневная газета, издаётся с августа 1997 года. «Факты» " +
                              "выходят пять раз в неделю, кроме воскресенья и понедельника. " +
                              "Главный редактор и владелец — Александр Швец, до июня 2016 " +
                              "года издание принадлежало EastOne Group. Печатается в Днепре, " +
                              "Донецке, Киеве, Луганске, Львове, Николаеве, Ровно, Харькове и в Крыму",
                Language = "ru",
                Reviews = new List<Review>(),
                Image = "facts_ua.jpg"
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
            manager.Create(new ApplicationUser()
            {
                UserName = "Moderator",
                Id = "555"

            }, "moder0000");
            var roleCreateResult2 = roleManager.Create(new PeriodicalsRole()
            {
                Id = "333",
                Name = "Moderator",
                Users = { new IdentityUserRole() { RoleId = "333", UserId = "555" } }
            });
            var roleCreateResult1 = roleManager.Create(new PeriodicalsRole("Subscriber"));
            //var roleCreateResult2 = roleManager.Create(new PeriodicalsRole("Moderator"));
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
