using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class Student(string name, string email)
    {
        public string Name { get; private set; } = name;
        public string Email { get; private set; } = email;

        private static int _lastID = 45350000;
        public int ID { get; private set; } = _lastID;

        private readonly List<int> _marks = [];

        public void UpdateContactInfo(string newEmail)
        {
            Email = newEmail;
            Console.WriteLine($"Контактная информация обновлена.\n" +
                              $"Новый адрес электронной почты:{Email}");
        }

        public void GetMarkForLabWork(int mark)
        {
            _marks.Add(mark);
            Console.WriteLine($"Лабораторная работа №{_marks.Count} сдана! Оценка: {_marks.Last()}");
        }

        public void GetFullInfo()
        {
            Console.WriteLine($"Имя студента: {Name}\n" +
                                $"Адрес электронной почты: {Email}\n" +
                                $"Уникальный ID студента: {ID}\n" +
                                $"Оценки за лабораторные работы: {PrintMarks()}");
        }

        private string PrintMarks()
        {
            string marks = "[ ";
            foreach (var mark in _marks)
            {
                marks += $"{mark} ";
            }
            marks += "]";
            return marks;
        }
    }
}
