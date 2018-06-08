using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Periodicals.Core;
using Periodicals.Core.Identity;
using Periodicals.Core.Interfaces;
using Periodicals.Infrastructure.Data;

namespace Periodicals.Services
{
    public class EditionServices
    {
        private readonly IRepository<Edition> _editionRepository;

        public EditionServices(IRepository<Edition> editionRepository)
        {
            _editionRepository = editionRepository;
        }
        public List<Edition> GetEditionsByLanguage(string language)
        {
            //List < Edition > dbEditions = new List<Edition>();
            var dbEditions = _editionRepository.List().Where(e => e.Language == language).ToList();
            /*using (var db = new PeriodicalDbContext())
            {
                dbEditions = db.Editions.Where(e => e.Language == language);
            }*/
            return dbEditions;
        }





        public List<Edition> SortByName(bool order)
        {
            var items = _editionRepository.List();
            items.Sort(delegate (Edition x, Edition y)
            {
                if (x.Name == null && y.Name == null) return 0;
                else if (x.Name == null) return -1;
                else if (y.Name == null) return 1;
                else return x.Name.CompareTo(y.Name);
            });
            if (!order) items.Reverse();
            return items;
        }

        public List<Edition> SortByPrice(bool order)
        {
            var items = _editionRepository.List();
            items.Sort(delegate (Edition x, Edition y)
            {
                return x.Price.CompareTo(y.Price);
            });
            if (!order) items.Reverse();
            return items;
        }

        public List<Edition> SearchByName(string search)
        {
            var searchResult =(from edition in _editionRepository.List() where  edition.Name.ToUpper().Contains(search.ToUpper()) select edition).ToList();
            return searchResult;

            /*using (var db = new PeriodicalDbContext())
            {
                var searchResult = db.Editions.Where(a => a.Name.Contains(search)).ToList();
                editions = EditionModel.ToModelList(searchResult);
            }*/
        }

    }
}
