using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR4.Models;

namespace LR4.Creation
{
    class CargoBuilder
    {
        public Cargo Cargo { get; set; } = new Cargo();
        public CargoBuilder WithMass(double mass)
        {
            Cargo.Mass = mass;
            return this;
        }
        public CargoBuilder WithCost(double cost)
        {
            Cargo.CostPerKg = cost;
            return this;
        }
        public CargoBuilder WithName(string name)
        {
            Cargo.Name = name;
            return this;
        }
        public Cargo Build()
        {
            var cargo = Cargo;
            Cargo = new Cargo();
            return cargo;
        }
    }
}
