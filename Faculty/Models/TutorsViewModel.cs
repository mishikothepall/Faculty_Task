using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faculty.Models
{
    public class TutorsViewModel
    {
        public int TutorId { get; set; }

        public string TutorName { get; set; }

        public IEnumerable<string> Courses { get; set; }

        public TutorsViewModel(int id, string name, IEnumerable<string> courses)
        {

            TutorId = id;

            TutorName = name;

            Courses = courses;

        }
    }
}