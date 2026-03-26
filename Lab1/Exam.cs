using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class Exam(string name)
    {
        public string Name { get; private set; } = name;
        public int LastGrade { get; private set; }
        public bool IsSuccessfully { get; private set; } = true;
        public void Pass(string student)
        {
            int grade = new Random().Next(0, 10);
            if (grade < 4)
            {
                IsSuccessfully = false;
                Console.WriteLine($"{student} завалил экзамен, получив оценку {grade}");
            } else
            {
                Console.WriteLine($"{student} сдал экзамен, получив оценку {grade}");
            }
            LastGrade = grade;
        }
        public void GetInfo()
        {
            Console.WriteLine($"{Name}\n" +
                $"Последняя оценка - {LastGrade}\n" +
                $"Нет ли пересдачи? - {IsSuccessfully}");
        }
    }
}
