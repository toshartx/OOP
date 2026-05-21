using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Filter
{
    interface IFilter
    {
        bool Apply(DeliveryResult result);
    }
}
