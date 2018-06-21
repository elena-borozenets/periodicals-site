using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using Periodicals.Infrastructure.Identity;
using Periodicals.Models;
using Periodicals.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Periodicals.Infrastructure.Repositories;
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
    [RequireHttps]
    public class EditionController : Controller
    {
        private readonly IRepository<Edition> _editionRepository;
        private readonly IRepository<Topic> _topicRepository;
        private readonly EditionServices _editionService;
        private readonly IUserRepository _userRepository;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EditionController(IRepository<Edition> editionRepository, IRepository<Topic> topicRepository, IUserRepository userRepository)
        {
            _editionRepository = editionRepository;
            _topicRepository = topicRepository;
            TopicModel.SetTopicsList(_topicRepository.List());
            _editionService = new EditionServices(_editionRepository);
            _userRepository = userRepository;
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
            return View("Index", editions);
        }

        public ActionResult Languages(string language)
        {
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
                    var user = _userRepository.GetById(userId);
                    if (user?.Subscription != null && user.Subscription.Contains(item))
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
            if ((_editionRepository as EditionRepository).AddSubscription(userId, editionId))
            {
                _logger.Info("user " + User.Identity.Name + " subscribe for " + editionId + " edition");
            }
            return RedirectToAction("Edition", new { area = "", editionId = editionId });
        }

        [Authorize(Roles = "Subscriber")]
        public ActionResult Unsubscribe(int editionId)
        {
            var url = HttpContext.Request.UrlReferrer;
                var userId = User.Identity.GetUserId();
                (_editionRepository as EditionRepository)?.RemoveSubscription(userId, editionId);
            _logger.Info("user " + User.Identity.Name + " unsubscribe for " + editionId + " edition");

            if (url==null) return RedirectToAction("Edition", new { area = "", editionId = editionId });
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
            var image = _editionRepository.GetById(editionId).Image;
            _editionRepository.Delete(editionId);

            foreach (var topic in _topicRepository.List())
            {
                if (topic.Editions?.Count == 0) _topicRepository.Delete(topic.Id);
            }

            if (!string.IsNullOrEmpty(image))
            {
                var appPath = AppDomain.CurrentDomain.BaseDirectory;
                var imagePath = appPath+ "Content\\dbImg\\" + image;
                System.IO.File.Delete(imagePath);
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
                    string savedFileName = Path.Combine(filePathOriginal, filename);
                    image.SaveAs(savedFileName);
                    item.Image = filename;
                }

                item.Name = edition.Name;
                
                item.Price = edition.Price;
                item.DateNextPublication = edition.DateNextPublication;
                if (!string.IsNullOrEmpty(edition.ISSN))
                {
                    item.ISSN = edition.ISSN;
                }
                if (edition.Periodicity != 0)
                {
                    item.Periodicity = edition.Periodicity;
                }
                if (!string.IsNullOrEmpty(edition.Description))
                {
                    item.Description = edition.Description;
                }
                if (!string.IsNullOrEmpty(edition.Type))
                {
                    item.Type = edition.Type;
                }
                if (!string.IsNullOrEmpty(edition.Language))
                {
                    item.Language = edition.Language;
                }
                if (!string.IsNullOrEmpty(edition.TopicName))
                {
                    var topicResult = (_topicRepository as TopicRepository).GetByName(edition.TopicName);
                    item.Topic = topicResult ?? new Topic() { TopicName = edition.TopicName };
                }

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
                if (!string.IsNullOrEmpty(newEdition.TopicName))
                {
                    var topicResult = ( _topicRepository as TopicRepository).GetByName(newEdition.TopicName);
                    edition.Topic = topicResult ?? new Topic() { TopicName = newEdition.TopicName };
                }
                else edition.Topic = new Topic() { TopicName =  "No topic"};
                if (image != null)
                {
                    var filename = image.FileName;
                    var filePathOriginal = Server.MapPath("~/Content/dbImg");
                    string savedFileName = Path.Combine(filePathOriginal, filename);
                    image.SaveAs(savedFileName);
                    edition.Image = filename;
                }
                else edition.Image = "default.jpg";

                _editionRepository.Add(edition);
                return RedirectToAction("Index");
            }
            else
            {
                return View(newEdition);
            }
        }

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
            ViewBag.Message = "Periodicals! ";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Leave a message and we will contact with you";
            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = HttpContext.User.Identity.GetUserId();
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(userId);
                model.Email = user.Email;
                if(string.IsNullOrEmpty(model.Name))  model.Name=User.Identity.Name;
            }

            MailAddress from = new MailAddress("elena.borozenets.applications@gmail.com");
            MailAddress to = new MailAddress("elena.borozenets.applications@gmail.com");
            MailMessage message = new MailMessage(from, to);

            
            message.Subject = (!string.IsNullOrEmpty(model.Subject))?model.Subject:"Message from Periodicals!";
            message.Body = $"(From: {model.Email} User: {model.Name}) \nMessage: {model.Text}";
            message.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("elena.borozenets.applications@gmail.com", "Some00Pass11");
            smtp.EnableSsl = true;
            smtp.Send(message);
            ViewBag.ContactMessage = "Your message has been sent successfully!";
            return View("ContactMessageResult");
        }


    }
}