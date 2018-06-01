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

        public List<EditionModel> ToModelList(List<Edition> items)
        {
            var editions = new List<EditionModel>();
            foreach (var item in items)
            {
                editions.Add(EditionModel.FromEdition(item));
            }

            return editions;
        }

        public ActionResult Index()
        {
            var editions = ToModelList(_editionRepository.List());
            return View(editions);
        }

        public ActionResult Topics(int topicId)
        {
            var topic = _topicRepository.GetById(topicId);
            var editions = ToModelList(topic.Editions.ToList());
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
                try
                {
                    int k = user.Prop;
                }
                catch { };
                if(user.Subscription!=null&&user.Subscription.Contains(item))
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
                var user = userManager.FindById(userId);
                var edition = _editionRepository.GetById(editionId);
                if (user.Subscription == null) user.Subscription = new List<Edition>();
                user.Subscription.Add(edition);
                user.Prop = 11;

                edition.Subscribers.Add(user);
                //userManager.Update(user);
                /*using (var dbContext = new PeriodicalDbContext())
                {
                    dbContext.SaveChanges();
                }*/
                _editionRepository.Update(edition);
            }
            catch { }
            return RedirectToAction("Edition", new { area = "", editionId = editionId } );
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