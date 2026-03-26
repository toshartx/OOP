using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class Staff : Employee
    {
        public Staff(string name, string email) : base(name, email) {}

        public override void DoWork()
        {
            Console.WriteLine($"Я, {Name}, занимаюсь посторонней работой!");
        }
    }
}
