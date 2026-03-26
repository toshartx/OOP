using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LR2
{
    internal class Professor : Employee 
    {
        public Professor(string name, string email) : base(name, email) 
        {
            degree = new AcademicDegree("доктор", "мат. наук", "МГУ", new DateTime(2010, 10, 12));
        }
        
        public AcademicDegree degree { get; set; }
        public override void DoWork()
        {
            Console.WriteLine($"Я, {Name}, веду пары");
        }
        
        public void DoWork(int count)
        {
            Console.WriteLine($"Я, {Name}, веду {count} пар(-ы)");
        }
      
        public void SetMarkForStudent(Student student)
        {
            int newMark = new Random().Next(1, 10);
            student.GetMarkForLabWork(newMark);
        }

        public void RunLabInGroup(StudyGroup group)
        {
            Console.WriteLine($"{Name} проводит лабораторное занятие у {group.Id}");
            foreach (Student student in group.GetStudents()) 
            {
                SetMarkForStudent(student);
            }
        }
    }
}
