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
    public interface ITransportFactory
    {
        Transport Create();
    }

    public class TruckTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Truck();
        }
    }

    public class TankerTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Tanker();
        }
    }

    public class TrainTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Train();
        }
    }

    public class PlaneTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Models.Plane();
        }
    }

    public class HelicopterTransportFactory : ITransportFactory
    {
        public Transport Create()
        {
            return new Helicopter();
        }
    }
}
