using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataModels
{
   public class TutorDTO
    {
        public int TutorID { get; set; }

        public string TutorName { get; set; }

        public bool IsBlocked { get; set; }

        public List<CourseDTO> Courses { get; set; }

        public TutorDTO()
        {
            IsBlocked = true;
            Courses = new List<CourseDTO>();

        }
    }
}
