using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LR2
{
    internal abstract class Person : IPerson, IGetInfo
    {
        protected Person(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public string Name { get; protected set; }
        public string Email { get; protected set; }

        public void UpdateContactInfo(string newEmail)
        {
            Email = newEmail;
            Console.WriteLine($"Контактная информация обновлена.\n" +
                              $"Новый адрес электронной почты:{Email}");
           
        }

        public abstract void GetFullInfo();
    }
}
