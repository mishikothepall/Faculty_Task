using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataModels
{
   public class CourseDTO
    {
        public int CourseID { get; set; }

        public string CourseName { get; set; }

        public string BeginDate { get; set; }

        public string EndDate { get; set; }

        public bool IsBlock { get; set; }

        public TutorDTO Teacher { get; set; }

        public string Description { get; set; }

        public CourseSubjectDTO Subject { get; set; }

        public List<StudentDTO> Group { get; set; }

        public List<JournalDTO> Marks { get; set; }

        public CourseDTO()
        {

            Group = new List<StudentDTO>();

            Marks = new List<JournalDTO>();

            IsBlock = false;

        }
    }
}
