using Periodicals.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodicals.Core.Entities
{
    public class Topic: BaseEntity
    {
        public string TopicName { get; set; }
        public IList<Edition> Editions { get; set; }
    }
}
