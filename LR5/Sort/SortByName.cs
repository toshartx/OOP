using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Sort
{
    class SortByTransportName : SortHandler
    {
        public override List<DeliveryResult> Sort(List<DeliveryResult> results)
        {
            var sorted = results.OrderBy(r => r.Request.DelTransport?.Name).ToList();
            return _next_handler?.Sort(sorted) ?? sorted;
        }
    }
}
