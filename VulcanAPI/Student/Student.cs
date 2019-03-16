using System;
using System.Collections.Generic;
using System.Text;

namespace VulcanAPI.Student
{
    public class Student
    {
        public Student(long studentId, string schoolId, string schoolSymbol, string studentName, string studentLastName)
        {
            StudentId = studentId;
            School = new School(schoolId, schoolSymbol);
            StudentName = studentName;
            StudentLastName = studentLastName;
        }

        public long StudentId { get; set; }
        public School School { get; set; }

        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFullName => $"{StudentName} {StudentLastName}";
    }
}