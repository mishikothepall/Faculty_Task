using BusLayer.DataServices;
using Faculty.Filters;
using Faculty.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Faculty.Controllers
{
    [Authorize(Roles = "teacher")]
    [ExceptionLogAttribute]
    [UserActionLogger]
    public class TutorController : Controller
    {
        CourseService cs = new CourseService();
        TutorService ts = new TutorService();
        StudentService ss = new StudentService();


        [HttpPost]
        public ActionResult EstimateStudents(CourseViewModel model)
        {
            ss.AddMark(model.CourseName, model.Journal.ToDictionary((m => m.StudentName), m => m.Mark));
            return RedirectToAction("CourseJournal");
        }

        public ActionResult TutorInfo()
        {
            var data = ts.GetByName(User.Identity.Name);

            var tvm = new TutorsViewModel(
                data.TutorID,
                data.TutorName,
                data.Courses.Select(c=>c.CourseName)
                );
          
            return View(tvm);
        }

        public ActionResult CourseJournal(string subj)
        {
            var data = cs.CourseInfo().FirstOrDefault(c => c.CourseName == subj);

            var model = new CourseViewModel(
                data.CourseName,
                $"{data.Teacher.TutorName}",
                data.Teacher.TutorID,
                data.Group.Where(b => b.IsBlocked == false).Select(s => $"{s.StudentName}"),
                data.BeginDate,
                data.EndDate,
                data.Marks.Select(m => new JournalViewModel(
                    m.Student.StudentName,
                    m.Course.CourseName,
                    m.SubjectMark
                )).ToList(),
                data.Description
                );
            return View(model);
        }


    }
}