using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataModels
{
   public class JournalDTO
    {
        public int ResultID { get; set; }

        public int SubjectMark { get; set; }

        public StudentDTO Student { get; set; }

        public CourseDTO Course { get; set; }
    }
}
