using Periodicals.Core.SharedKernel;
using System.Collections.Generic;

namespace Periodicals.Core.Entities
{
    public class Topic: BaseEntity
    {
        public string TopicName { get; set; }
        public IList<Edition> Editions { get; set; }
    }
}
