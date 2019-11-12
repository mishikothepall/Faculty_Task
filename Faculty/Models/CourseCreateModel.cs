using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faculty.Models
{
    public class CourseCreateModel
    {
        [Required(ErrorMessage = "Вевдите название курса.")]
        public string CourseName { get; set; }

        [Required(ErrorMessage = "Имя преподавателя не указано.")]
        public string Tutor { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public string Description { get; set; }

        public string SelectedSubject { get; set; }

        public List<JournalViewModel> Journal { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }

        public CourseCreateModel() { }

        public CourseCreateModel(IEnumerable<SubjectViewModel> subjects)
        {

            Subjects = subjects.Select(s => new SelectListItem { Text = s.SubjectName, Selected = false});

        }
     
    }
}