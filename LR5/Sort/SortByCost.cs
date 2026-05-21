using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Sort
{
    class SortByCost : SortHandler
    {
        private bool _desc;
        public SortByCost(bool descending = false) => _desc = descending;
        public override List<DeliveryResult> Sort(List<DeliveryResult> results)
        {
            var sorted = _desc ? results.OrderByDescending(r => r.TotalCost).ToList() : results.OrderBy(r => r.TotalCost).ToList();
            return _next_handler?.Sort(sorted) ?? sorted;
        }
    }
}
