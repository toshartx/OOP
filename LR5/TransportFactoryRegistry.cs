using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5
{
    class TransportFactoryRegistry
    {
        private readonly Dictionary<string, ITransportFactory> _factories;

        public TransportFactoryRegistry()
        {
            _factories = new Dictionary<string, ITransportFactory>
            {
                ["Грузовик"] = new TruckTransportFactory(),
                ["Поезд"] = new TrainTransportFactory(),
                ["Танкер"] = new TankerTransportFactory(),
                ["Самолёт"] = new PlaneTransportFactory(),
                ["Вертолёт"] = new HelicopterTransportFactory()
            };
        }

        public Transport Create(string type)
        {
            if (_factories.TryGetValue(type, out var factory))
                return factory.Create();

            throw new ArgumentException($"Unknown transport type: {type}");
        }

        public List<string> GetAllTypes()
        {
            return _factories.Keys.ToList();
        }

        public bool HasTransport(string type)
        {
            return _factories.ContainsKey(type);
        }
    }
}
