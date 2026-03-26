using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LR1
{
    internal class Professor(string name, string email, string degree)
    {
        public string Name { get; } = name;
        public string Email { get; private set; } = email;
        public string Degree { get; private set; } = degree;
        public void UpdateContactInfo(string newEmail)
        {
            Email = newEmail;
            Console.WriteLine($"Контактная информация обновлена.\n" +
                              $"Новый адрес электронной почты:{Email}");
        }
        public int SetMarkForStudent()
        {
            int newMark = new Random().Next(1, 10);
            Console.WriteLine($"Преподаватель {Name} выставил новую оценку: {newMark}");
            return newMark; 
        }
        public void GetFullInfo()
        {
            Console.WriteLine($"Имя преподавателя: {Name}\n" +
                               $"Адрес электронной почты: {Email}\n" +
                               $"Учёная степень: {Degree}");
        }

    }
}
