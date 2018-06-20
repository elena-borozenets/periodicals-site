
using System;
using System.Collections.Generic;
using System.Linq;
using Periodicals.Core.Entities;
using Periodicals.Core.Interfaces;

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
            var dbEditions = _editionRepository.List().Where(e => e.Language == language).ToList();
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
                else return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
            });
            if (!order) items.Reverse();
            return items;
        }

        public List<Edition> SortByPrice(bool order)
        {
            var items = _editionRepository.List();
            items.Sort((x, y) => x.Price.CompareTo(y.Price));
            if (!order) items.Reverse();
            return items;
        }

        public List<Edition> SearchByName(string search)
        {
            var searchResult =(from edition in _editionRepository.List()
                where  edition.Name.ToUpper().Contains(search.ToUpper())
                select edition).ToList();
            return searchResult;

        }

    }
}
