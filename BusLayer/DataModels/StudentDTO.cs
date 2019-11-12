using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataModels
{
   public class StudentDTO
    {
        public int StudentID { get; set; }

        public string StudentName { get; set; }

        public int Age { get; set; }

        public bool IsBlocked { get; set; }

        public bool ChangeStatus { get; set; }

        public byte[] UserPhoto { get; set; }

        public List<CourseDTO> Courses { get; set; }

        public List<JournalDTO> Marks { get; set; }

        public StudentDTO()
        {

            Courses = new List<CourseDTO>();

            Marks = new List<JournalDTO>();

            IsBlocked = false;

            ChangeStatus = false;
        }
    }
}
