using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;
using LR5.Services;

namespace LR5
{
    public class DeliveryController
    {
        private readonly ImporterFactory _importerFactory;
        private readonly ILogisticCalculator _calculator;
        private readonly TransportSelector _selector;

        public DeliveryController(TransportFactoryRegistry registry)
        {
            _importerFactory = new ImporterFactory(registry);
            _calculator = new LogisticCalculator();
            _selector = new TransportSelector(registry);
        }

        public List<DeliveryResult> ProcessDelivery(string inputFile)
        {
            var request = _importerFactory.Import(inputFile);

            if (request.Distance <= 0)
                throw new ArgumentException("Distance must be greater than 0");

            if (request.DelTransport != null)
            {
                return [_calculator.CalculateDelivery(request)];
            }

            var transports = _selector.SelectTransports(request.Cargos, request.Distance);
            var results = new List<DeliveryResult>();

            foreach (var transport in transports)
            {
                var tempRequest = new DeliveryRequest
                {
                    Cargos = request.Cargos,
                    DelTransport = transport,
                    Distance = request.Distance
                };
                results.Add(_calculator.CalculateDelivery(tempRequest));
            }

            return results;
        }
    }
}
