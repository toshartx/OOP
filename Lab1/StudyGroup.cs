using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class StudyGroup(int course, string specialty)
    {
        public int Id { get; private set; } = new Random().Next(500000, 600000);
        public int Course { get; private set; } = course % 5;
        public string SpecialtyName { get; private set; } = specialty;

        private List<string> _students = [];

        public void GetFullInfo()
        {
            Console.WriteLine($"Номер группы - {Id}\n" +
                                $"Курс - {Course}\n" +
                                $"Специальность - {SpecialtyName}\n" +
                                $"Список студентов:");
            foreach (var student in _students) 
            {
                Console.WriteLine(student);
            }
        }

        public void PassCourse()
        {
            Console.WriteLine($"Студенты специальности {Id} пережили {Course} курс!");
            if (Course == 4)
            {
                Console.WriteLine($"Студенты специальности {Id} закончили университет");
                return;
            }
            Course++;
        }

        public void AddStudent(string student) 
        {
            if (!_students.Contains(student))
            {
                Console.WriteLine($"В группе {Id} новый студент - {student}");
                _students.Add(student);
            }
        }

        public void RemoveStudent(string student)
        {
            if (_students.Contains(student))
                Console.WriteLine($"В группе {Id} удалён студент - {student}");
            _students.Remove(student);
        }
    }
}
