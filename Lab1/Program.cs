namespace LR1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Student> students = [ new("Andrey","andrey@email.com"), new("Vladimir", "vova@email.com"), new("Ash", "ash@email.com") ];

            AcademicDegree candidate = new("Кандидат", "физико-математических", "МФТИ", DateTime.Now);
            AcademicDegree doctor = new("Доктор", "математических", "БГУ", DateTime.Now);

            List<Professor> professors = [ new("Александр Владимирович", "AV@email.com", doctor.Title),
                                            new("Зоя Николаевна", "ZN@email.com", candidate.Title),
                                            new("Олег Иванович", "OI@email.com", candidate.Title)   ];
            List<Classroom> classrooms = [ new(1, ClassroomType.Lecture), new(2, ClassroomType.Lecture), new(3, ClassroomType.Seminar),
                new(4, ClassroomType.Seminar), new(5, ClassroomType.Seminar), new(6, ClassroomType.Laboratory)];

            LibBook book = new("Основы ООП", "Иванов И.И.", 67);
            ScheduleItem item = new(new TimeSpan(14, 00, 00), "ООП");
            Exam exam = new("ИСП");
            StudyGroup studyGroup = new(2, "ИиТП");
            Course course = new("Избранные главы информатики", "git, docker, python, django");
            Department department = new("Кафедра информатики", "+37577171717", "informatics@email.com", "ул. Улиц 42");

            students[1].GetFullInfo();
            Console.WriteLine("----------------------");

            for (int i = 0; i < 5; ++i)
            {
                students[1].GetMarkForLabWork(professors[1].SetMarkForStudent());
            }

            Console.WriteLine("----------------------");

            students[1].GetFullInfo();

            Console.WriteLine("----------------------");

            professors[1].UpdateContactInfo("odug@email.com");
            professors[1].GetFullInfo();

            Console.WriteLine("----------------------");

            department.AddProfessor(professors[0].Name);
            department.AddProfessor(professors[1].Name);
            department.AddProfessor(professors[2].Name);

            Console.WriteLine("----------------------");
            department.GetInfo();
            Console.WriteLine("----------------------");

            foreach (var classroom in classrooms) 
            {
                classroom.RepairClassroom();
            }

            Console.WriteLine("----------------------");
            studyGroup.AddStudent(students[0].Name);
            studyGroup.AddStudent(students[1].Name);
            studyGroup.AddStudent(students[2].Name);
            studyGroup.GetFullInfo();
            studyGroup.PassCourse();
            studyGroup.GetFullInfo();
            Console.WriteLine("----------------------");
            course.GetCourseInfo();
            course.AddSomethingNewToCourse("Добавлена дополнительная лабораторная работа. Тема - основы java script");
            course.GetCourseInfo();
            Console.WriteLine("----------------------");
            book.GetFullTitle();
            Console.WriteLine($"Доступные копии - {book.AvaliableCopies}");
            book.Take();
            book.Take();
            book.Take();
            book.Return();
            Console.WriteLine("----------------------");
            item.GetFullInfo();
            item.Reschedule(new TimeSpan(12, 30, 0));
            Console.WriteLine(item.IsRunning(new TimeSpan(12, 45, 00)));
            Console.WriteLine("----------------------");
            exam.GetInfo();
            exam.Pass(students[0].Name);
            exam.Pass(students[1].Name);
            exam.Pass(students[2].Name);
            exam.GetInfo();
        }
    }
}
