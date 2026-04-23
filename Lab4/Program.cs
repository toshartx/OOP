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
            var controller = new LogisticsController();

            RunDemo(controller);
        }

        static void RunDemo(LogisticsController controller)
        {
            var cargoItems = new List<(string, double, double)>
            {
                ("Электроника", 1.5, 50),
                ("Одежда", 0.8, 20),
                ("Оборудование", 120.0, 15),
                ("Скоропортящиеся продукты", 10.0, 100)
            };

            var result = controller.ProcessDelivery(cargoItems, "Грузовик", 48);

            Console.WriteLine(result);
        }
    }
}
