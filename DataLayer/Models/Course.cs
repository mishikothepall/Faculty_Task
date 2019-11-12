using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
   public class Course
    {
        public int CourseID { get; set; }

        public string CourseName { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public bool IsBlock { get; set; }

        public Tutor Teacher { get; set; }

        public string Description { get; set; }

        public CourseSubject Subject { get; set; }

        public List<Student> Group { get; set; }

        public List<Journal> Marks { get; set; }

        public Course(){

            Group = new List<Student>();

            Marks = new List<Journal>();

            IsBlock = false;

        }
    }
}
