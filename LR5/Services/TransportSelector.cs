using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5
{
    class TransportSelector
    {
        private readonly TransportFactoryRegistry _factoryRegistry;

        public TransportSelector(TransportFactoryRegistry factoryRegistry)
        {
            _factoryRegistry = factoryRegistry;
        }

        public List<Transport> SelectTransports(List<Cargo> cargos, double distance)
        {
            var selectedTypes = new HashSet<string>();

            // По расстоянию
            if (distance > 1000)
            {
                selectedTypes.Add("Самолёт");
                selectedTypes.Add("Вертолёт");
            }
            else if (distance > 200)
                selectedTypes.Add("Поезд");
            else
                selectedTypes.Add("Грузовик");

            // По весу
            double totalWeight = cargos.Sum(c => c.Mass);
            if (totalWeight > 5000)
                selectedTypes.Add("Поезд");
            else if (totalWeight > 1000)
                selectedTypes.Add("Грузовик");

            return selectedTypes.Select(t => _factoryRegistry.Create(t)).ToList();
        }
    }
}
