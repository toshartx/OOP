using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4.Models
{
    internal class Cargo(string name, double mass, double cost)
    {
        public string Name { get; set; } = name;
        public double Mass { get; set; } = mass;
        public double CostPerKg { get; set; } = cost;

        public double Cost()
        {
            return CostPerKg * Mass;
        }

        public override string ToString()
        {
            return $"{Name} - {Cost()} р";
        }
    }
}
