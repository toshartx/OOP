using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class Lecture : Classroom
    {
        public Lecture(int number) : base(number) {}
        public override void RepairClassroom()
        {
            if (Status != ClassroomStatus.Fine)
            {
                Console.WriteLine($"На ремонт лекционной {Number} уйдёт много времени - {Status}");
                Status = ClassroomStatus.UnderRepair;
                Console.WriteLine($"Начало ремонта, статус - {Status}");
                Thread.Sleep(1500);
            }
            Console.WriteLine($"Лекционная {Number} в полном порядке - {Status}");
        }
    }
}
