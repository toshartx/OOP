using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR5.Models
{
    public abstract class Transport
    {
        public string Name { get; set; }
        public double ConsumptionPerKm { get; set; }
        public double Velocity { get; set; }
        public double OverheadCost(double distance)
        {
            return ConsumptionPerKm * distance;
        }

        public double DeliveryTime(double distance)
        {
            return distance / Velocity;
        }
    }

    public abstract class GroundTransport : Transport
    {
        protected GroundTransport()
        {
            Name = "(земля)";
        }
    }

    public class Truck : GroundTransport
    {
        public Truck() : base()
        {
            Name = Name.Insert(0, "Грузовик ");
            ConsumptionPerKm = 15.0;
            Velocity = 80;
        }
    }

    public class Train : GroundTransport
    {
        public Train() : base()
        {
            Name = Name.Insert(0, "Поезд ");
            ConsumptionPerKm = 5.0;
            Velocity = 60;
        }
    }
    public abstract class AirTransport : Transport
    {
        protected AirTransport()
        {
            Name = "(воздух)";
        }
    }

    public class Plane : AirTransport
    {
        public Plane() : base()
        {
            Name = Name.Insert(0, "Самолёт ");
            ConsumptionPerKm = 150.0;
            Velocity = 850;
        }
    }

    public class Helicopter : AirTransport
    {
        public Helicopter() : base()
        {
            Name = Name.Insert(0, "Вертолёт ");
            ConsumptionPerKm = 200.0;
            Velocity = 250;
        }
    }

    public abstract class WaterTransport : Transport
    {
        protected WaterTransport()
        {
            Name = "(вода)";
        }
    }

    public class Tanker : WaterTransport
    {
        public Tanker() : base()
        {
            Name = Name.Insert(0, "Танкер ");
            ConsumptionPerKm = 2.5;
            Velocity = 35;
        }
    }
}
