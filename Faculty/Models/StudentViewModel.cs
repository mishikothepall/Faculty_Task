using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faculty.Models
{
    public class StudentViewModel
    {
        public string StudentName { get; set; }

        public int Age { get; set; }

        public byte[] StudentPhoto { get; set; }

        public string Path { get; set; }

        public IEnumerable<string> Courses { get;  }

        public List<JournalViewModel> Journal { get; set; }

        public StudentViewModel() { }

        public StudentViewModel(string name, IEnumerable<string> courses, byte[]photo) {

            StudentName = name;

            Courses = courses;

            StudentPhoto = photo;

        }
    }
}