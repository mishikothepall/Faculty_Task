using BusLayer.DataServices;
using Faculty.Filters;
using Faculty.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faculty.Controllers
{
    [ExceptionLogAttribute]
    [UserActionLogger]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {

        CourseService cs = new CourseService();
        TutorService ts = new TutorService();
        StudentService ss = new StudentService();
        SubjectService sbs = new SubjectService();

        public ActionResult MainPanel()
        {
            var dataStud = ss.StudInfo().Where(s => s.ChangeStatus == true);

            var svm = dataStud.Select(d => new StudentViewModel(
                d.StudentName,
                d.Courses.Select(c => c.CourseName),
                d.UserPhoto
                ));

            var dataTut = ts.TutorInfo();

            var tvm = dataTut.Select(d => new TutorsViewModel(
                d.TutorID,
                d.TutorName,
                d.Courses.Select(c => c.CourseName)
                ));

            TempData["Candidates"] = svm;
            TempData["Tutors"] = tvm;

            return View();
        }

        public ActionResult TutorCandidates() {
            var data = ss.StudInfo().Where(s=>s.ChangeStatus==true);

            var svm = data.Select(d=>new StudentViewModel(
                d.StudentName,
                d.Courses.Select(c => c.CourseName),
                d.UserPhoto
                ));

            return PartialView(svm);
        }

        public ActionResult TutorReady(string name) {

            var data = ts.TutorInfo();
            var isRegistered = ts.unlockTutor(name);

            if (isRegistered == false)
            {
                ModelState.AddModelError("", "Преподаватель не зарегестрирован");
            }

            var tvm = data.Select(d=>new TutorsViewModel(
                d.TutorID,
                d.TutorName,
                d.Courses.Select(c=>c.CourseName)
                ));

            return PartialView(tvm);
        }

        public ActionResult AddCourse() {
            var data = sbs.SubjectInfo();
            var cvm = new CourseCreateModel(data.Select(d=>new SubjectViewModel(
                d.SubjectName,
                d.SubjectLogo,
                d.Courses.Select(c=>c.CourseName).ToList()
                )));
            return View(cvm);

        }

        [HttpPost]
        [ActionName("AddCourse")]
        public ActionResult AddCourse(CourseCreateModel model) {

            if (ModelState.IsValid)
            {
                string isCreated = cs.CreateCourse(model.Tutor, model.CourseName, model.BeginDate,
                    model.EndDate, model.Description, model.SelectedSubject);

                if (isCreated.Equals(string.Empty))
                {
                    return RedirectToAction("MainPanel");
                }

            }
            else
            {

                return View(model);

            }

            return View(model);
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}