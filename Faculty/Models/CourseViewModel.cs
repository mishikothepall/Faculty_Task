using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faculty.Models
{
    public class CourseViewModel
    {
        public string CourseName { get; set; }

        public int TutorId { get; set; }

        public string Tutor { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public int Mark { get; set; }

        public string Description { get; set; }

        public string SelectedSubject { get; set; }

        public IEnumerable<string> Group { get; set; }

        public List<JournalViewModel> Journal { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }

        public CourseViewModel() { }

        public CourseViewModel(IEnumerable<SubjectViewModel> subjects) {

            Subjects = subjects.Select(s=> new SelectListItem { Text = s.SubjectName});

        }

        public CourseViewModel(string name, string teacher, int tutorid, IEnumerable<string> group,
            string begin, string end, List<JournalViewModel> journal, string desc)
        {

            CourseName = name;

            Tutor = teacher;

            TutorId = tutorid;

            Group = group;

            BeginDate = begin;

            EndDate = end;

            Journal = journal;

            Description = desc;

        }
    }
}