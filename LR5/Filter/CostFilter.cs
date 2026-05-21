using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Filter
{
    public class CostFilter : IFilter
    {
        private readonly double _min, _max;
        public CostFilter(double min, double max) => (_min, _max) = (min, max);
        public bool Apply(DeliveryResult result) => result.TotalCost >= _min && result.TotalCost <= _max;
    }
}
