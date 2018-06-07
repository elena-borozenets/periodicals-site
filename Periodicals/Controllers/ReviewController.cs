using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Periodicals.Core;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using Periodicals.Infrastructure.Data;
using Periodicals.Models;

namespace Periodicals.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<Edition> _editionRepository;


        // GET: Review

        public ReviewController()
        {
            _reviewRepository = new EfRepository<Review>(new PeriodicalDbContext());
            _editionRepository = new EfRepository<Edition>(new PeriodicalDbContext());
        }
        public ActionResult Reviews(int editionId)
        {
            List<ReviewModel> reviews = new List<ReviewModel>();
            var edition = _editionRepository.GetById(editionId);
            if (edition != null)
                reviews = ReviewModel.ToModelList(edition.Reviews.ToList());
            /*using (var db = new PeriodicalDbContext())
            {
                var edition = db.Editions.Find(editionId);
                 if(edition!=null)   
                reviews = ReviewModel.ToModelList(edition.Reviews.ToList());
            }*/

            ViewBag.EditionId = editionId;
                return PartialView("_Reviews", reviews);
        }

        [HttpPost]
        public ActionResult AddNew(ReviewModel newReview)
        {
            if (ModelState.IsValid)
            {
            var newItem = newReview;
            newItem.TimeCreation = DateTime.Now;

                /*var edition = _editionRepository.GetById(newReview.EditionId);
                if (edition != null)
                {
                    newItem.EditionId = newReview.EditionId;
                    var item = newItem.ToReview();
                    item.Edition = edition;
                    _reviewRepository.Add(item);
                }*/
                using (var db = new PeriodicalDbContext())
                {
                    var edition = db.Editions.Find(newReview.EditionId);
                    if (edition != null)
                    {
                        edition.Reviews.Add(newItem.ToReview());
                        db.SaveChanges();
                    }
                }
                   // _reviewRepository.Add(newItem.ToReview());
            }

            return RedirectToAction("Edition", "Home", new{editionId=newReview.EditionId});
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult DeleteReview(int reviewId, int editionId)
        {
           var review= _reviewRepository.GetById(reviewId);
            _reviewRepository.Delete(review);
            return RedirectToAction("Edition", "Home", new { editionId = editionId});
        }
    }
}