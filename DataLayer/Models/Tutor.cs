using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Tutor
    {
        public int TutorID { get; set; }

        public string TutorName { get; set; }

        public bool IsBlocked { get; set; }

        public List<Course> Courses { get; set; }

        public Tutor()
        {
            IsBlocked = true;
            Courses = new List<Course>();

        }
    }
}
