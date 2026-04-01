using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LR2
{
    internal class University<TEmployee, TClassroom> 
                    where TEmployee : IEmployee
                    where TClassroom : Classroom
    {
        public University() 
        {
            _departments = [];
            _studyGroups = [];
            _employees = [];
            _classrooms = [];
        }

        private readonly List<Department> _departments;
        private readonly List<StudyGroup> _studyGroups;
        private readonly List<TEmployee> _employees;
        private readonly List<TClassroom> _classrooms;

        //public Department GetDepartment(string depName)
        //{
        //    foreach (var department in _departments) 
        //    {
        //        if (department.DepartmentName == depName)
        //        {
        //            return department;
        //        }
        //    }
        //    return new Department(depName, $"+37529{new Random().Next(1000000, 9999999)}", $"{depName.ToLower().Replace(" ","")}@email.com", "");
        //}

        public void AddDepartment(Department department)
        {
            if (!_departments.Contains(department))
            {
                _departments.Add(department);
            }
        }

        public void RemoveDepartment(Department department)
        {
            if (_departments.Contains(department))
            {
                _departments.Remove(department);
            }
        }

        //public StudyGroup GetStudyGroup(string specialty, int course)
        //{
        //    foreach (var group in _studyGroups)
        //    {
        //        if (group.Course == course && group.SpecialtyName == specialty)
        //        {
        //            return group;
        //        }
        //    }
        //    return new StudyGroup(course, specialty);
        //}

        public void AddStudyGroup(StudyGroup group)
        {
            if (!_studyGroups.Contains(group))
            {
                _studyGroups.Add(group);
            }
        }

        public void RemoveStudyGroup(StudyGroup group)
        {
            if (!_studyGroups.Contains(group))
            {
                _studyGroups.Remove(group);
            }
        }

        public void AddEmployee(TEmployee employee)
        {
            if (!_employees.Contains(employee))
            {
                _employees.Add(employee);
            }
        }

        public void RemoveEmployee(TEmployee employee) 
        {
            _employees.Remove(employee);
        }

        public void ProcessEmployeeWork()
        {
            foreach (dynamic empl in _employees)
            {
                try
                {
                    empl.DoWork();
                }
                catch (Exception) { }
            }
        }

        public void AddClassroom(TClassroom classroom)
        {
            if (!_classrooms.Contains(classroom))
            { 
                _classrooms.Add(classroom);
            }
        }

        public void RefreshUniversity()
        {
            foreach (dynamic classroom in _classrooms)
            {
                try
                {
                    classroom.RepairClassroom();
                    classroom.SetupEquipment();
                }
                catch (Exception) { }
            }
        }

        public void MoveToNextYear()
        {
            Console.WriteLine("Следующий учебный год...");
            foreach (var group in _studyGroups)
            {
                group.PassCourse();
            }
        }

        public void ExamSession()
        {
            foreach (var group in _studyGroups)
            {
                group.CloseSession();
            }
        }

        public void ProcessLessons()
        {
            foreach (var group in _studyGroups)
            {
                group.CloseLessons();
            }
        }

        //public void RunLabWork(string specialty, int course, string depName, int currentHour, int currentMinute, string profName, string studName)
        //{
        //    StudyGroup group = GetStudyGroup(specialty, course);
        //    Student? student = group.GetStudentByName(studName);
        //    Professor? professor = GetDepartment(depName).GetProfessorByName(profName);
        //    if (professor is not null && student is not null)
        //    {
        //        TimeSpan currentTime = new TimeSpan(currentHour, currentMinute, 0); 
        //        if (group.IsTimeToWork(currentTime)) 
        //            professor.SetMarkForStudent(student);
        //        else
        //            Console.WriteLine("Пока нет пар, нет возможности сдать лабораторную работу.");
        //    } else
        //    {
        //        Console.WriteLine("Не удалось найти нужную информацию о студенте");
        //    }
        //}
    }
}
