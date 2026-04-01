using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class Laboratory : Classroom
    {
        public Laboratory(int number) : base(number) { }
        public override void Repair()
        {
            if (Status != ClassroomStatus.Fine)
            {
                Console.WriteLine($"На ремонт лаборатории {Number} уйдёт немного времени - {Status}");
                Status = ClassroomStatus.UnderRepair;
                Console.WriteLine($"Начало ремонта, статус - {Status}");
                Thread.Sleep(1000);
            }
            Console.WriteLine($"Лаборатория {Number} в полном порядке - {Status}");
        }

        public void SetupEquipment()
        {
            Console.WriteLine($"Обновление устаревшего оборудования в лаборатории №{Number}");
            Console.WriteLine("Обновление прошло успешно!");
        }
    }
}
