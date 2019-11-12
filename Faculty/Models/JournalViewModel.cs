using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faculty.Models
{
    public class JournalViewModel
    {
        public string CourseName { get; set; }

        public string StudentName { get; set; }

        public int Mark { get; set; }

        public JournalViewModel() { }

        public JournalViewModel(string name, string course, int mark) {
            StudentName = name;

            CourseName = course;

            Mark = mark;
        }
    }
}