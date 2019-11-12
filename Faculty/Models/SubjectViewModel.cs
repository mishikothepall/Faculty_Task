using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Faculty.Models
{
    public class SubjectViewModel
    {
        public string SubjectName { get; }

        public string SubjLogo { get; }

        public List<string> Courses { get; }

        public SubjectViewModel(string name, byte[]logo, List<string> courses) {
            SubjectName = name;

            SubjLogo = String.Format("data:image/gif;base64,{0}", Convert.ToBase64String(logo));

            Courses = courses;
        }
    }
}