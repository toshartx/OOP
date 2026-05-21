using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5.Import
{
    public abstract class BaseImporter
    {
        protected readonly TransportFactoryRegistry _factoryRegistry;

        protected BaseImporter(TransportFactoryRegistry factoryRegistry)
        {
            _factoryRegistry = factoryRegistry;
        }

        public abstract bool CanHandle(string filePath);
        public abstract DeliveryRequest Read(string filePath);

        protected DeliveryRequest CreateRequest(ImportData dto)
        {
            var request = new DeliveryRequest
            {
                Cargos = dto.Cargos ?? new List<Cargo>(),
                Distance = dto.Distance
            };

            if (!string.IsNullOrEmpty(dto.TransportType))
            {
                request.DelTransport = _factoryRegistry.Create(dto.TransportType);
            }

            return request;
        }
    }

}
