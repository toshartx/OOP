using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR5.Models
{
    public class DeliveryResult
    {
        public double TotalTime { get; set; }
        public double TotalCost { get; set; }
        public required DeliveryRequest Request { get; set; }
        public override string ToString()
        {
            return "================\n" +
                "Заказ\n" +
                $"{TotalTime} - время доставки\n" +
                $"{TotalCost} - стоимость доставки\n" +
                $"{Request}\n" +
                "================";
        }
    }
}
