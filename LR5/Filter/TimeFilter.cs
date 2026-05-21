using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Filter
{
    public class TimeFilter : IFilter
    {
        private readonly double _maxTime;
        public TimeFilter(double maxTime) => _maxTime = maxTime;
        public bool Apply(DeliveryResult result) => result.TotalTime <= _maxTime;
    }
}
