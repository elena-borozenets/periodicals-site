using Periodicals.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Periodicals.Areas.Account.Models
{
    public class EditionAccountModel
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Title")]
        public string Name { get; set; }

        public float Price { get; set; }

        public DateTime DateNextPublication { get; set; }

        public string Language { get; set; }

        public static EditionAccountModel FromEdition(Edition item) => new EditionAccountModel()
        {
            Id = item.Id,
            Name = item.Name ?? "Noname",
            Price=item.Price,
            DateNextPublication = item.DateNextPublication,
            Language=item.Language ?? "???"
        };

        public static List<EditionAccountModel> ToModelList(IList<Edition> items)
        {
            var editions = new List<EditionAccountModel>();
            if (items != null)
            {
                foreach (var item in items)
                {
                    editions.Add(EditionAccountModel.FromEdition(item));
                }
            }
            return editions;
        }
    }
}