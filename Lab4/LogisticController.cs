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
        private readonly DeliveryRequestBuilder _deliveryRequestBuilder;

        public LogisticsController()
        {
            _calculator = new LogisticCalculator();
            _transportFactory = new TransportFactory();
            _deliveryRequestBuilder = new DeliveryRequestBuilder();
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
            foreach (var item in cargoItems)
            {
                var cargo = new Cargo(item.name, item.mass, item.cost);
                _deliveryRequestBuilder.WithCargo(cargo);
            }

            var transport = _transportFactory.CreateTransport(transportType);
            _deliveryRequestBuilder.OnTransport(transport);
            _deliveryRequestBuilder.WithDistance(distance);

            return _deliveryRequestBuilder.Build();
        }
    }
}
