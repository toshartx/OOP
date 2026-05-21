using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5
{
    public interface IRequestBuilder
    {
        IRequestBuilder WithCargo(Cargo cargo);
        IRequestBuilder OnTransport(Transport transport);
        IRequestBuilder WithDistance(double distance);
        DeliveryRequest Build();
    }
    public class DeliveryRequestBuilder : IRequestBuilder
    {
        public DeliveryRequest Request { get; private set; } = new DeliveryRequest();

        public IRequestBuilder WithCargo(Cargo cargo)
        {
            Request.Cargos.Add(cargo);
            return this;
        }

        public IRequestBuilder OnTransport(Transport transport)
        {
            Request.DelTransport = transport;
            return this;
        }

        public IRequestBuilder WithDistance(double distance)
        {
            Request.Distance = distance;
            return this;
        }

        public DeliveryRequest Build()
        {
            var rqst = Request;
            Request = new DeliveryRequest();
            return rqst;
        }
    }
}
