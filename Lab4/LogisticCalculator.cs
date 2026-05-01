using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR4.Models;

namespace LR4
{
    internal class LogisticCalculator
    {
        public DeliveryResult CalculateDelivery(DeliveryRequest request)
        {
            double cargoCost, overheadCost;
            overheadCost = request.DelTransport!.OverheadCost(request.Distance);
            cargoCost = request.Cargos.Sum(c => c.Cost());

            return new DeliveryResult
            {
                TotalCost = cargoCost + overheadCost,
                TotalTime = request.DelTransport.DeliveryTime(request.Distance),
                Request = request
            };

        }
    }
}
