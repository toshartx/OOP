using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Filter
{
    public class TransportNameFilter : IFilter
    {
        private readonly string _name;
        public TransportNameFilter(string name) => _name = name;
        public bool Apply(DeliveryResult result) => result.Request.DelTransport!.Name == _name;
    }
}
