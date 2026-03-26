using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class AcademicDegree(string title, string field, string institution, DateTime award)
    {
        public string Title { get; private set; } = title;
        public string Field { get; private set; } = field;
        public string Institution {  get; private set; } = institution;
        public DateTime AwardDate { get; private set; } = award;

        public void GetFullTitle()
        {
            Console.WriteLine($"{Title} {Field} наук\n" +
                $"Присвоена {AwardDate.Day}.{AwardDate.Month}.{AwardDate.Year}\n" +
                $"в {Institution}");
        }
    }
}
