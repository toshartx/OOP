using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR4.Models;

namespace LR4.Creation
{
    interface ITransportFactory
    {
        public Transport CreateTransport(string name);
    }

    class TransportFactory : ITransportFactory
    {
        public Transport CreateTransport(string name)
        {
            switch (name)
            {
                case "Грузовик":
                    return new Truck();
                case "Поезд":
                    return new Train();
                case "Танкер":
                    return new Tanker();
                case "Самолёт":
                    return new Plane();
                case "Вертолёт":
                    return new Helicopter();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
