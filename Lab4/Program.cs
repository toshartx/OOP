using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR4.Creation;
using LR4.Models;

namespace LR4
{
    internal class Program
    {
        static void Main()
        {
            var calculator = new LogisticCalculator();
            var cargoBuilder = new CargoBuilder();
            var transportFactory = new TransportFactory();

            var request = new DeliveryRequest
            {
                Cargos = [
                    cargoBuilder.WithName("Электроника").WithMass(1.5).WithCost(50).Build(),
                    cargoBuilder.WithName("Одежда").WithMass(0.8).WithCost(20).Build(),
                    cargoBuilder.WithName("Оборудование").WithMass(120.0).WithCost(15).Build(),
                    cargoBuilder.WithName("Скоропортящиеся продукты").WithMass(10.0).WithCost(100).Build(),
                    ],
                DelTransport = transportFactory.CreateTransport("Грузовик"),
                Distance = 48
            };

            Console.WriteLine(calculator.CalculateDelivery(request));
        }
        
    }
}
