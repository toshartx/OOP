using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR2
{
    internal class StudyGroup : IGetInfo
    {
        public StudyGroup(int course, string specialty)
        {
            Course = course % 5;
            SpecialtyName = specialty;
            _schedule = [];
            _students = [];
            _exams = [];
        }
        public int Id { get; private set; } = new Random().Next(500000, 600000);
        public int Course { get; private set; }
        public string SpecialtyName { get; private set; }

        private readonly List<Student> _students;
        private readonly List<ScheduleItem> _schedule;
        private readonly List<Exam> _exams;

        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
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
        public bool IsTimeToWork(TimeSpan currentTime)
        {
            foreach (var item in _schedule)
            {
                if (item.IsRunning(currentTime))
                {
                    return true;
                }
            }
            return false;
        }
        public void AddStudent(Student student)
        {
            if (!_students.Contains(student))
            {
                Console.WriteLine($"В группе {Id} новый студент - {student.Name}");
                _students.Add(student);
            }
        }
        public void AddScheduleItem(ScheduleItem item)
        {
            if (!_schedule.Contains(item))
            {
                _schedule.Add(item);
            }
        }
        public void AddExam(Exam exam)
        {
            if (!_exams.Contains(exam))
            {
                _exams.Add(exam);
            }
        }
        public void RemoveStudent(Student student)
        {
            if (_students.Contains(student))
            {
                Console.WriteLine($"В группе {Id} удалён студент - {student.Name}");
                _students.Remove(student);
            }
        }
        public void RemoveScheduleItem(ScheduleItem item)
        {
            if (_schedule.Contains(item))
            {
                Console.WriteLine($"В группе {Id} удалена пара из расписания - {item.Name}");
                _schedule.Remove(item);
            }
        }
        private void CloseExam(Exam exam)
        {
            if (_exams.Contains(exam))
            {
                foreach (var student in _students)
                {
                    exam.Pass(student);
                }
                Console.WriteLine($"Экзамен {exam.Name} завершён!");
                //_exams.Remove(exam);
            }
            else
            {
                Console.WriteLine("Данные об экзамене не найдены");
            }
        }

        public void CloseSession()
        {
            Console.WriteLine("---------- Начало экзаменационной сессии! ----------");
            foreach (var exam in _exams)
            {
                CloseExam(exam);
            }
            Console.WriteLine("----------- Экзаменационная сессия завершена! ---------");
        }
        public void CloseLessons()
        {
            Console.WriteLine("------ Начало учебного дня!");
            foreach (var item in _schedule)
            {
                Console.WriteLine("Начало пары:");
                item.GetFullInfo();
                Thread.Sleep(150);
                Console.WriteLine("Конец пары");
            }
            Console.WriteLine("------ Конец учебного дня!");
        }

        private ScheduleItem GetScheduleItem(string name, TimeSpan time)
        {
            return new ScheduleItem(time, name);
        }
        private Student GetStudent(string name)
        {
            return new Student(name, $"{name.ToLower().Replace(" ", "")}@email.com");
        }
        private Exam GetExam(string name)
        {
            return new Exam(name);
        }
    }
}
