using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        public string StudentName { get; set; }

        public int Age { get; set; }

        public bool IsBlocked { get; set; }

        public bool ChangeStatus { get; set; }

        public byte[] UserPhoto { get; set; }

        public List<Course> Courses { get; set; }

        public List<Journal> Marks { get; set; }

        public Student() {

            Courses = new List<Course>();

            Marks = new List<Journal>();

            IsBlocked = false;

            ChangeStatus = false;
        }
    }
}
