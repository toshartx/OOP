using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Sort
{
    public class SortBySpeed : SortHandler
    {
        private readonly bool _desc;
        public SortBySpeed(bool descending = false) => _desc = descending;
        public override List<DeliveryResult> Sort(List<DeliveryResult> results)
        {
            var sorted = _desc ? results.OrderByDescending(r => r.Request.DelTransport?.Velocity).ToList() : results.OrderBy(r => r.Request.DelTransport?.Velocity).ToList();
            return _next_handler?.Sort(sorted) ?? sorted;
        }
    }
}
