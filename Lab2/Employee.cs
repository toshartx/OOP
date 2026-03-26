using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal abstract class Employee : Person, IEmployee
    {
        protected Employee(string name, string email) : base(name, email)
        {
            WorkLength = new Random().Next(0, 10800);
            Salary = new Random().Next(500, 1500);
        }
        public int WorkLength { get; private set; }
        public int Salary { get; private set; }
        public abstract void DoWork();
        public override void GetFullInfo()
        {
            Console.WriteLine($"Имя работника - {Name}, стаж работы - {WorkLength} дней.\n" +
                $"Контактная информация - {Email}");
        }

        public void SalaryIncrease(int amount)
        {
            Salary += amount;
        }
    }
}
