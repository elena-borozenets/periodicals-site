using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodicals.Core;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using Periodicals.Infrastructure.Data;
using Periodicals.Infrastructure.Identity;
using Periodicals.Models;
using Periodicals.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Periodicals.Infrastructure.Repositories;
using Ninject;
using NLog;
using Periodicals.Exceptions;

namespace Periodicals.Controllers
{
    [PeriodicalsException]
    [IndexOutOfRangePeriodicalsException]
    [ArgumentPeriodicalsException]
    [NullReferencePeriodicalsException]
    [InvalidOperationPeriodicalsException]
    [ArgumentNullPeriodicalsException]
    [ArgumentOutOfRangePeriodicalsException]
    //
    public class HomeController : Controller
    {
        private readonly IRepository<Edition> _editionRepository;
        private readonly IRepository<Topic> _topicRepository;
        private readonly EditionServices _editionService;

        public HomeController(IRepository<Edition> editionRepository, IRepository<Topic> topicRepository)
        {



            //IKernel ninjectKernel = new StandardKernel();
            //ninjectKernel.Bind<IRepository<Edition>>().To<EditionRepository>();
            //_editionRepository = ninjectKernel.Get<IRepository<Edition>>();
            //_editionRepository = new EditionRepository();
            //_topicRepository = new EfRepository<Topic>(new PeriodicalDbContext());
            _editionRepository = editionRepository;
            _topicRepository = topicRepository;
            TopicModel.SetTopicsList(_topicRepository.List());
            _editionService = new EditionServices(_editionRepository);

        }

        
        public ActionResult Index()
        {
            var editions = EditionModel.ToModelList(_editionRepository.List());
            //throw new ArgumentOutOfRangeException();
            return View(editions);
        }

        /*public ActionResult Index(List<EditionModel> editions)
        {
            return View(editions);
        }*/

        public ActionResult Topics(int topicId)
        {
            var topic = _topicRepository.GetById(topicId);
            var editions = EditionModel.ToModelList(topic.Editions.ToList());
            return View("Index", editions);
        }

        public ActionResult Languages(string language)
        {
            //List<EditionModel> editions;
            var dbEditions = _editionService.GetEditionsByLanguage(language);
            var editions = EditionModel.ToModelList(dbEditions.ToList());

            return View("Index", editions);
        }

        public ActionResult Edition(int editionId)
        {
            var item = _editionRepository.GetById(editionId);
            EditionModel edition = EditionModel.FromEdition(item);
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                using (var db = new PeriodicalDbContext())
                {
                    user =  (from user1 in db.Users where user1.Id == userId select user1)
                        .Include(e => e.Subscription).FirstOrDefault();
                }
                    if (user != null && user.Subscription != null && user.Subscription.Contains(item))
                    {
                        ViewBag.Subscpiption = true;
                    }
                    else
                    {
                        ViewBag.Subscpiption = false;
                    }
                ViewBag.Blocked = user?.LockoutEnabled;
            }

            
            return View(edition);
        }

        [Authorize(Roles = "Subscriber")]
        public ActionResult Subscribe(int editionId)
        {
                var userId = User.Identity.GetUserId();
            (_editionRepository as EditionRepository)?.AddSubscription(userId,editionId);

            return RedirectToAction("Edition", new { area = "", editionId = editionId });
        }

        public ActionResult Unsubscribe(int editionId)
        {
            var url = HttpContext.Request.UrlReferrer;
                var userId = User.Identity.GetUserId();
                (_editionRepository as EditionRepository)?.RemoveSubscription(userId, editionId);

            if(url==null) return RedirectToAction("Edition", new { area = "", editionId = editionId });
            return Redirect(url.AbsoluteUri);
        }

        public ActionResult SortByName(bool order)
        {
            var items = _editionService.SortByName(order);
            var editions = EditionModel.ToModelList(items);


            return View("Index", editions);
        }

        public ActionResult SortByPrice(bool order)
        {
            var items = _editionService.SortByPrice(order);
            var editions = EditionModel.ToModelList(items);

            return View("Index", editions);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult DeleteEdition(int editionId)
        {
            //var item = _editionRepository.GetById(editionId);
            _editionRepository.Delete(editionId);

            foreach (var topic in _topicRepository.List())
            {
                if (topic.Editions?.Count == 0) _topicRepository.Delete(topic.Id);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult EditEdition(int editionId)
        {
            var item = _editionRepository.GetById(editionId);
            var editionModel = EditionModel.FromEdition(item);
            return View(editionModel);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPost]
        public ActionResult EditEdition(EditionModel edition, HttpPostedFileBase image)
        {
            var item = _editionRepository.GetById(edition.Id);
            if (ModelState.IsValid)
            {
                if (image != null)
                {

                    var filename = image.FileName;
                    var filePathOriginal = Server.MapPath("~/Content/dbImg");
                    //var filePathThumbnail = Server.MapPath("/Content/Uploads/Thumbnails");
                    string savedFileName = Path.Combine(filePathOriginal, filename);
                    image.SaveAs(savedFileName);
                    item.Image = filename;
                }

                item.Name = edition.Name;
                item.Description = edition.Description;
                _editionRepository.Update(item);
                return RedirectToAction("Edition", new { editionId = edition.Id });
            }
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult AddEdition()
        {
            return View();
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPost]
        public ActionResult AddEdition(EditionModel newEdition, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var edition = newEdition.ToEdition();
                edition.Topic = new Topic() { TopicName = newEdition.TopicName };

                if (image != null)
                {

                    var filename = image.FileName;
                    var filePathOriginal = Server.MapPath("~/Content/dbImg");
                    //var filePathThumbnail = Server.MapPath("/Content/Uploads/Thumbnails");
                    string savedFileName = Path.Combine(filePathOriginal, filename);
                    image.SaveAs(savedFileName);
                    edition.Image = filename;
                }
                _editionRepository.Add(edition);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newEdition);
            }
            return RedirectToAction("Index");
        }

        /*public ActionResult Search()
        {
            return View();
        }*/

        [HttpPost]
        public ActionResult Search(string search)
        {
            var result =_editionService.SearchByName(search);
            List<EditionModel> editions = EditionModel.ToModelList(result);
            //db.Books.Where(a => a.Author.Contains(name)).ToList();
            ViewBag.searchString = search;
            if (editions.Count <= 0)
            {
                //return HttpNotFound();
            }
            return PartialView("_EditionSearch", editions);
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