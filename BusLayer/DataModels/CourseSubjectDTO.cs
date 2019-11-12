
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BusLayer.DataModels
{
   public class CourseSubjectDTO
    {
        public int SubjectID { get; set; }

        public string SubjectName { get; set; }

        public byte[] SubjectLogo { get; set; }

        public List<CourseDTO> Courses { get; set; }

        public CourseSubjectDTO()
        {
            Courses = new List<CourseDTO>();
        }
    }
}