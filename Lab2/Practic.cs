using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class Practic : Classroom
    {
        public Practic(int number) : base(number) { }
        public override void RepairClassroom()
        {
            if (Status != ClassroomStatus.Fine)
            {
                Console.WriteLine($"На ремонт аудитории {Number} уйдёт мало времени - {Status}");
                Status = ClassroomStatus.UnderRepair;
                Console.WriteLine($"Начало ремонта, статус - {Status}");
                Thread.Sleep(500);
            }
            Console.WriteLine($"Аудитория для ПЗ №{Number} в полном порядке - {Status}");
        }
    }
}
