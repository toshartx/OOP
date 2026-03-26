using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class Department(string department, string phone, string email, string location)
    {
        public string DepartmentName { get; private set; } = department;
        public string Phone { get; set; } = phone;
        public string Email { get; set; } = email;
        public string Location { get; set; } = location;
        private List<string> _professorsNames = [];

        public void AddProfessor(string professor) 
        {
            if (!_professorsNames.Contains(professor))
            {
                Console.WriteLine($"К кафедре прикреплён новый преподаватель - {professor}");
                _professorsNames.Add(professor);
            }
        } 

        public void GetInfo()
        {
            Console.WriteLine($"{DepartmentName}\n" +
                $"Номер телефона - {Phone}\n" +
                $"Email - {Email}\n" +
                $"Адрес - {Location}\n" +
                $"Список профессоров:");
            foreach (var professor in _professorsNames) 
            {
                Console.WriteLine(professor);
            }
        }
    }
}
