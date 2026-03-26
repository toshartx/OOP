namespace LR2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var un = new University<Employee, Classroom>();

            Professor prof = new("Oleg", "@mail");
            Professor prof2 = new("Anna", "@gmail");
            Staff stf = new("Valera", "@email");

            Student student1 = new("Vlad", "@email");
            Student student2 = new("Lera", "@gmail");
            Student student3 = new("Max", "@mail");
            Student student4 = new("Unnamed", "@rambler");

            Exam exam1 = new("ООП");
            Exam exam2 = new("ИСП");
            Exam exam3 = new("ОИС");
            Exam exam4 = new("МА");

            ScheduleItem item1 = new(new TimeSpan(12, 00, 00), "ООП");
            ScheduleItem item2 = new(new TimeSpan(13, 35, 00), "ИСП");
            ScheduleItem item3 = new(new TimeSpan(12, 00, 00), "ОИС");
            ScheduleItem item4 = new(new TimeSpan(13, 35, 00), "МА");

            StudyGroup studyGroup1 = new(2, "ИиТП");
            StudyGroup studyGroup2 = new(3, "ИИ");

            Lecture lect = new(104);
            Practic pract = new(320);
            Laboratory lab = new(111);

            studyGroup1.AddStudent(student1);
            studyGroup2.AddStudent(student2);
            studyGroup2.AddStudent(student3);
            studyGroup1.AddStudent(student4);

            studyGroup1.AddExam(exam1);
            studyGroup1.AddExam(exam2);
            studyGroup2.AddExam(exam3);
            studyGroup2.AddExam(exam4);

            studyGroup1.AddScheduleItem(item1);
            studyGroup1.AddScheduleItem(item2);
            studyGroup2.AddScheduleItem(item3);
            studyGroup2.AddScheduleItem(item4);

            un.AddClassroom(lect);
            un.AddClassroom(pract);
            un.AddClassroom(lab);

            un.AddStudyGroup(studyGroup1);
            un.AddStudyGroup(studyGroup2);

            un.AddEmployee(prof);
            un.AddEmployee(prof2);
            un.AddEmployee(stf);

            un.RefreshUniversity();
            un.ProcessEmployeeWork();

            un.ProcessLessons();
            un.ExamSession();
            un.MoveToNextYear();

        }
    }
}
