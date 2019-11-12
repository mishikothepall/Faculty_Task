using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faculty.Models
{
    public class SearchTemplate
    {
        public string Search { get; set; }

        public string SelectedFilter { get; set; }

        public string SelectedSubject { get; set; }

        public IEnumerable<SelectListItem> Filters { get; set; }

        public List<SubjectViewModel> Subjects { get; set; }

        public IEnumerable<CourseViewModel> Courses { get; set; }

        public SearchTemplate() {

            Filters = new List<string>() { "z-a", "Самые длительные", "Самые короткие", "Самые популярные" }
            .Select(f => new SelectListItem { Text = f });

        }

        public SearchTemplate(List<SubjectViewModel> subject, IEnumerable<CourseViewModel> courses)
        {

            Filters = new List<string>() { "z-a", "Самые длительные", "Самые короткие", "Самые популярные" }
            .Select(f => new SelectListItem { Text = f });
            Subjects = subject;

            Courses = courses;
        }
    }
}