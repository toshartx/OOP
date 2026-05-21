using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Sort
{
    public abstract class SortHandler
    {
        protected SortHandler? _next_handler;
        public abstract List<DeliveryResult> Sort(List<DeliveryResult> results);

        public SortHandler ThenBy(SortHandler next) { _next_handler = next; return this; }
    }


}
