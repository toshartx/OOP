using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class Course(string name, string description)
    {
        public string DisciplineName { get; } = name;
        public string Description { get; set; } = description;
        public int DisciplineId { get; private set; }
        public void GetCourseInfo()
        {
            Console.WriteLine($"Название курса: {DisciplineName}\nОписание:\n{Description}");
        }

        public void AddSomethingNewToCourse(string description)
        {
            Console.WriteLine($"В учебный курс {DisciplineName} внесены изменения");
            Description += '\n' + description + '\n';
        }
    }
}
