using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Periodicals.Core;
using Periodicals.Core.Entities;

namespace Periodicals.Models
{
    /// <summary>
    /// Class for transfering from/to <see cref="Review"/>
    /// </summary>
    public class ReviewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Нужно ввести автора")]
        public string NameAuthor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime TimeCreation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required(ErrorMessage = "Нужно ввести текст отзыва")]
        public string TextReview { get; set; }

        public int EditionId { get; set; }

        /// <summary>
        /// Transfers <see langword="from"/> <see cref="Review"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static ReviewModel FromReview(Review item) => new ReviewModel()
        {
            Id = item.Id,
            NameAuthor = item.NameAuthor,
            TimeCreation = item.TimeCreation,
            TextReview = item.TextReview,
            EditionId = item.Edition.Periodicity
        };

        /// <summary>
        /// Transfers <see langword="to"/> <see cref="Review"/>
        /// </summary>
        /// <returns></returns>
        public Review ToReview() => new Review()
        {
            NameAuthor = this.NameAuthor,
            TimeCreation = this.TimeCreation,
            TextReview = this.TextReview
        };

        public static List<ReviewModel> ToModelList(IList<Review> items)
        {
            var reviews = new List<ReviewModel>();
            foreach (var item in items)
            {
                reviews.Add(ReviewModel.FromReview(item));
            }

            return reviews;
        }
    }
}
