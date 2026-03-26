using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    enum ClassroomStatus
    {
        Fine,
        RepairsRequired,
        UnderRepair
    }

    enum ClassroomType
    {
        Lecture,
        Laboratory,
        Seminar
    }

    internal class Classroom
    {
        public Classroom(int number, ClassroomType type) 
        {
            Number = number;
            Type = type;

            int status = new Random().Next(1, 10);
            if (status >= 5) 
            {
                Status = ClassroomStatus.Fine;   
            } else
            {
                Status = ClassroomStatus.RepairsRequired;
            }
        }
        public int Number { get; private set; }
        public ClassroomStatus Status { get; private set; }
        public ClassroomType Type { get; private set; }
        public void RepairClassroom()
        {
            Console.WriteLine($"Состояние аудитории - {Status}");
            if (Status == ClassroomStatus.RepairsRequired)
            {
                Status = ClassroomStatus.UnderRepair;
                Console.WriteLine($"Начало ремонта. Состояние - {Status}");
                Thread.Sleep(1000);
                Status = ClassroomStatus.Fine;
                Console.WriteLine($"Аудитория отремонтирована и готова к занятиям, статус - {ClassroomStatus.Fine}");
            }
        }
    }
}
