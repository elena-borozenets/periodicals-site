using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Periodicals.Core.Entities;

namespace Periodicals.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Нужно ввести автора")]
        public string NameAuthor { get; set; }
        
        public DateTime TimeCreation { get; set; }
        
        [Required(ErrorMessage = "Нужно ввести текст отзыва")]
        public string TextReview { get; set; }

        public int EditionId { get; set; }

        public static ReviewModel FromReview(Review item) => new ReviewModel()
        {
            Id = item.Id,
            NameAuthor = item.NameAuthor,
            TimeCreation = item.TimeCreation,
            TextReview = item.TextReview,
            EditionId = item.EditionId
        };

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
