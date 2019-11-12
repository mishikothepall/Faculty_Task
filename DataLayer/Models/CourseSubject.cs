using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
   public class CourseSubject
    {
        public int SubjectID { get; set; }

        public string SubjectName { get; set; }

        public byte[] SubjectLogo { get; set; }

        public List<Course> Courses { get; set; }

        public CourseSubject() {
            Courses = new List<Course>();
        }
    }
}
