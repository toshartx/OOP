using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Services
{
    public interface ILogisticCalculator
    {
        DeliveryResult CalculateDelivery(DeliveryRequest request);
    }

    public class LogisticCalculator : ILogisticCalculator
    {
        public DeliveryResult CalculateDelivery(DeliveryRequest request)
        {
            double cargoCost, overheadCost;
            overheadCost = request.DelTransport!.OverheadCost(request.Distance);
            cargoCost = request.Cargos.Sum(c => c.Cost());

            return new DeliveryResult
            {
                TotalCost = cargoCost + overheadCost,
                TotalTime = Math.Round(request.DelTransport.DeliveryTime(request.Distance), 2),
                Request = request
            };

        }
    }
}
