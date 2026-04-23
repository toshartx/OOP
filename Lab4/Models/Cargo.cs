using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR4.Models
{
    internal class Cargo
    {
        public string Name { get; set; }
        public double Mass { get; set; }
        public double CostPerKg { get; set; }

        public double Cost()
        {
            return CostPerKg * Mass;
        }
    }
}
