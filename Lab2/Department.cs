using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class Department(string department, string phone, string email, string location) : IGetInfo
    {
        public string DepartmentName { get; private set; } = department;
        public string Phone { get; set; } = phone;
        public string Email { get; set; } = email;
        public string Location { get; set; } = location;
        private List<Professor> _professors = [];

        public void AddProfessor(Professor professor) 
        {
            if (!_professors.Contains(professor))
            {
                Console.WriteLine($"К кафедре прикреплён новый преподаватель - {professor}");
                _professors.Add(professor);
            }
        }
        public void GetFullInfo()
        {
            Console.WriteLine($"{DepartmentName}\n" +
                $"Номер телефона - {Phone}\n" +
                $"Email - {Email}\n" +
                $"Адрес - {Location}\n" +
                $"Список профессоров:");
            foreach (var professor in _professors) 
            {
                Console.WriteLine(professor.Name);
            }
        }
    }
}
