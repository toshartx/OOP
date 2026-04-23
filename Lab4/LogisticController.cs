using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR4.Creation;
using LR4.Models;

namespace LR4
{
    class LogisticsController
    {
        private readonly LogisticCalculator _calculator;
        private readonly TransportFactory _transportFactory;
        private readonly CargoBuilder _cargoBuilder;

        public LogisticsController()
        {
            _calculator = new LogisticCalculator();
            _transportFactory = new TransportFactory();
            _cargoBuilder = new CargoBuilder();
        }

        public DeliveryResult ProcessDelivery(
            List<(string name, double mass, double cost)> cargoItems,
            string transportType,
            double distance)
        {
            var request = CreateRequest(cargoItems, transportType, distance);

            var result = _calculator.CalculateDelivery(request);

            return result;
        }

        private DeliveryRequest CreateRequest(
            List<(string name, double mass, double cost)> cargoItems,
            string transportType,
            double distance)
        {
            var cargos = new List<Cargo>();

            foreach (var item in cargoItems)
            {
                var cargo = _cargoBuilder
                    .WithName(item.name)
                    .WithMass(item.mass)
                    .WithCost(item.cost)
                    .Build();
                cargos.Add(cargo);
            }

            return new DeliveryRequest
            {
                Cargos = cargos,
                DelTransport = _transportFactory.CreateTransport(transportType),
                Distance = distance
            };
        }
    }
}
