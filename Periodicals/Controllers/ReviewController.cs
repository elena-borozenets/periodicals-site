using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;
using Periodicals.Exceptions;
using Periodicals.Models;

namespace Periodicals.Controllers
{
    [PeriodicalsException]
    [IndexOutOfRangePeriodicalsException]
    [ArgumentPeriodicalsException]
    [NullReferencePeriodicalsException]
    [InvalidOperationPeriodicalsException]
    [ArgumentNullPeriodicalsException]
    [ArgumentOutOfRangePeriodicalsException]
    public class ReviewController : Controller
    {
        private readonly IRepository<Review> _reviewRepository;
        private readonly IRepository<Edition> _editionRepository;


        // GET: Review

        public ReviewController(IRepository<Review> reviewRepository, IRepository<Edition> editionRepository)
        {
            _reviewRepository = reviewRepository;
            _editionRepository = editionRepository;
        }
        public ActionResult Reviews(int editionId)
        {
            List<ReviewModel> reviews = new List<ReviewModel>();
            var edition = _editionRepository.GetById(editionId);
            if (edition != null) reviews = ReviewModel.ToModelList(edition.Reviews.ToList());
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
                var review = newItem.ToReview();
                review.EditionId = newReview.EditionId;
                _reviewRepository.Add(review);
            }

            return RedirectToAction("Edition", "Edition", new{editionId=newReview.EditionId});
        }

        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult DeleteReview(int reviewId, int editionId)
        {
            _reviewRepository.Delete(reviewId);
            return RedirectToAction("Edition", "Edition", new { editionId = editionId});
        }
    }
}