using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodicals.Core;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using Periodicals.Infrastructure.Data;
using Periodicals.Infrastructure.Identity;
using Periodicals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Edition> _editionRepository;
        private readonly IRepository<Topic> _topicRepository;

        public HomeController()
        {
            _editionRepository = new EfRepository<Edition>(new PeriodicalDbContext());
            _topicRepository = new EfRepository<Topic>(new PeriodicalDbContext());
            TopicModel.SetTopicsList(_topicRepository.List());

        }

        public ActionResult Index()
        {
            var editions = EditionModel.ToModelList(_editionRepository.List());
            return View(editions);
        }

        public ActionResult Topics(int topicId)
        {
            var topic = _topicRepository.GetById(topicId);
            var editions = EditionModel.ToModelList(topic.Editions.ToList());
            return View("Index",editions);
        }

        public ActionResult Edition(int editionId)
        {
            var item = _editionRepository.GetById(editionId);
            EditionModel edition = EditionModel.FromEdition(item);
            if(User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                if(user!=null&&user.Subscription!=null&&user.Subscription.Contains(item))
                {
                    ViewBag.Subscpiption = true;
                }
                else
                {
                    ViewBag.Subscpiption = false;
                }
            }
            return View(edition);
        }

        //[Authorize(Roles = "Subscription")]
        public ActionResult Subscribe(int editionId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                using (var db = new PeriodicalDbContext())
                {
                    var user = db.Users.Find(userId);
                    var editionDb = db.Editions.Find(editionId);
                    user.Subscription.Add(editionDb);
                    editionDb.Subscribers.Add(user);
                    db.SaveChanges();

                }
            }
            catch { }
            return RedirectToAction("Edition", new { area = "", editionId = editionId } );
        }

        public ActionResult Unsubscribe(int editionId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                using (var db = new PeriodicalDbContext())
                {
                    var user = db.Users.Find(userId);
                    var editionDb = db.Editions.Find(editionId);
                    if (user.Subscription.Contains(editionDb))
                    {
                        user.Subscription.Remove(editionDb);
                    }
                    db.SaveChanges();

                }
            }
            catch { }
            return RedirectToAction("Edition", new { area = "", editionId = editionId });
        }

        public ActionResult SortByName(bool order)
        {
            var items = _editionRepository.List();
            var editions = EditionModel.ToModelList(items);
                editions.Sort(delegate (EditionModel x, EditionModel y)
                {
                    if (x.Name == null && y.Name == null) return 0;
                    else if (x.Name == null) return -1;
                    else if (y.Name == null) return 1;
                    else return x.Name.CompareTo(y.Name);
                });
                if (!order) editions.Reverse();

            
            return View("Index", editions);
        }

        public ActionResult SortByPrice(bool order)
        {
            var items = _editionRepository.List();
            var editions = EditionModel.ToModelList(items);
                editions.Sort(delegate (EditionModel x, EditionModel y)
                {
                    return x.Price.CompareTo(y.Price);
                });
            if (!order) editions.Reverse();
            return View("Index", editions);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}