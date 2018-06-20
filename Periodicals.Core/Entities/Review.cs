using System;
using Periodicals.Core.SharedKernel;

namespace Periodicals.Core.Entities
{
    public class Review: BaseEntity
    {
        public string NameAuthor { get; set; }
        public DateTime TimeCreation { get; set; }
        public string TextReview { get; set; }
        public int EditionId { get; set; }
        public Edition Edition { get; set; }
    }
}
