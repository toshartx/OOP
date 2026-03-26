using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class Student : Person
    {
        public Student(string name, string email) : base(name, email)
        {
            ID = _lastID++;
        }

        private static int _lastID = 45350000;
        public int ID { get; private set; }
        public int ExamMark { get; set; }
        private readonly List<int> _marks = [];
        public void GetMarkForLabWork(int mark)
        {
            _marks.Add(mark);
            Console.WriteLine($"Лабораторная работа №{_marks.Count} сдана! Оценка: {_marks.Last()}");
        }
        

        private string StrMarks()
        {
            string marks = "[ ";
            foreach (var mark in _marks)
            {
                marks += $"{mark} ";
            }
            marks += "]";
            return marks;
        }

        public override void GetFullInfo()
        {
            Console.WriteLine($"Имя студента - {Name}\n" +
                            $"ID студента - {ID}\n" +
                            $"email студента - {Email}\n" +
                            $"Оценки студента за лабораторные - {StrMarks()}");

        }
    }
}
