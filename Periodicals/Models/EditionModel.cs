using Periodicals.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Periodicals.Core.Entities;
using Periodicals.Core.Identity;

namespace Periodicals.Models
{
    public class EditionModel
    {
        public int Id { get; private set; }
        
        [Required]
        public string Name { get; private set; }
        [Required]
        public float Price { get; set; }

        public DateTime DateNextPublication { get; private set; }

        public string ISSN { get; set; }
        public Topic Topic { get; set; }
        public int Periodicity { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }

        //public List<string> SubscribersNames { get; set; }

        public static EditionModel FromEdition(Edition item) => new EditionModel()
        {
            Id = item.Id,
            Name = item.Name,
            Price=item.Price,
            DateNextPublication = item.DateNextPublication,
            ISSN=item.ISSN,
            Topic=item.Topic,
            Periodicity=item.Periodicity,
            Description=item.Description,
            Type=item.Type,
            Language=item.Language,
            //SubscribersNames= (from i in item.Subscribers
                           //select i.UserName).ToList<string>()
        };

        public Edition ToEdition() => new Edition()
        {
            Id = this.Id,
            Name = this.Name,
            Price = this.Price,
            DateNextPublication = this.DateNextPublication,
            ISSN = this.ISSN,
            Topic = this.Topic,
            Periodicity = this.Periodicity,
            Description=this.Description,
            Type = this.Type,
            Language=this.Language

        };

        public static List<EditionModel> ToModelList(IList<Edition> items)
        {
            var editions = new List<EditionModel>();
            foreach (var item in items)
            {
                editions.Add(EditionModel.FromEdition(item));
            }

            return editions;
        }
    }
}