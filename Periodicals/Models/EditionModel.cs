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
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Title")]
        public string Name { get; set; }
        //[Required]
        public float Price { get; set; }

        public DateTime DateNextPublication { get; set; }

        public string ISSN { get; set; }
        public string TopicName { get; set; }
        public int Periodicity { get; set; }

        [Display(Name = "Description")]
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
            TopicName=item.Topic.TopicName,
            Periodicity=item.Periodicity,
            Description=item.Description,
            Type=item.Type,
            Language=item.Language,
            //SubscribersNames= (from i in item.Subscribers
                           //select i.UserName).ToList<string>()
        };

        public Edition ToEdition()
        {
            
            var newEdition = new Edition();
            newEdition.Name = (this.Name == null) ? "Noname" : this.Name;
            newEdition.Price = this.Price;
            newEdition.DateNextPublication = DateTime.Now;
            newEdition.ISSN = (this.ISSN == null) ? "####-####" : this.ISSN;
            //newEdition.Topic = new Topic(){TopicName = (this.TopicName==null)? "Without topic": this.TopicName};
            newEdition.Periodicity = this.Periodicity;
            newEdition.Description = (this.Description == null) ? "None" : this.Description;
            newEdition.Type = (this.Type == null) ? "DefaultType" : this.Type;
            newEdition.Language = (this.Type == null) ? "???" : this.Language;
            newEdition.Reviews = new List<Review>();
            //newEdition.Subscribers=new List<ApplicationUser>();

            return newEdition;
        }

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