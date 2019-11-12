using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
   public class Journal
    {
        public int ResultID { get; set; }

        public int SubjectMark { get; set; }

        public Student Student { get; set; }

        public Course Course { get; set; }
    }
}
