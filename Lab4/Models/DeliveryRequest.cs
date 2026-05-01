using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4.Models
{
    class DeliveryRequest
    {
        public List<Cargo> Cargos { get; set; } = [];
        public Transport? DelTransport { get; set; }
        public double Distance { get; set; }

        public override string ToString()
        {
            return $"[{string.Join("; ", Cargos.Select(c => c))}] - список товаров\n" +
                $"{DelTransport.Name} - транспорт";
        }
    }
}
