using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LR5.Models;

namespace LR5
{
    interface ITransportFactory
    {
        Transport Create();
    }

    class TruckTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Truck();
        }
    }

    class TankerTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Tanker();
        }
    }

    class TrainTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Train();
        }
    }

    class PlaneTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Models.Plane();
        }
    }

    class HelicopterTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Helicopter();
        }
    }
}
