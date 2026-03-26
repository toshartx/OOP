using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR1
{
    internal class ScheduleItem(TimeSpan time, string name)
    {
        public string Name { get; private set; } = name;
        public TimeSpan Start { get; private set; } = time;
        public TimeSpan Duration { get; set; } = TimeSpan.FromHours(1);

        public bool IsRunning(TimeSpan currentTime)
        {
            if (currentTime > Start + Duration || currentTime < Start)
            {
                Console.WriteLine($"Пара {Name} сейчас не проходит");
                return false;
            }
            Console.WriteLine($"Пара {Name} идёт прямо сейчас!");
            return true;
        }
        public void Reschedule(TimeSpan newTime)
        {
            TimeSpan Start = newTime;
            Console.WriteLine($"Новое время начала пары {Name} - {Start}");
        }
        public void GetFullInfo()
        {
            Console.WriteLine($"{Name}\n" +
                $"Начало - {Start}\n" +
                $"Конец - {Start + Duration}");
        }
    }
}
