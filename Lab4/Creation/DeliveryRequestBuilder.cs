using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR4.Models;

namespace LR4.Creation
{
    //interface IRequestBuilder
    //{
    //    IRequestBuilder WithCargo(Cargo cargo);
    //    IRequestBuilder OnTransport(Transport transport);
    //    IRequestBuilder WithDistance(double distance);
    //}
    class DeliveryRequestBuilder //: IRequestBuilder
    {
        public DeliveryRequest Request { get; private set; } = new DeliveryRequest();

        public DeliveryRequestBuilder WithCargo(Cargo cargo)
        {
            Request.Cargos.Add(cargo);
            return this;
        }

        public DeliveryRequestBuilder OnTransport(Transport transport)
        {
            Request.DelTransport = transport;
            return this;
        }

        public DeliveryRequestBuilder WithDistance(double distance)
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
