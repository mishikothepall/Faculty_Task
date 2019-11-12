using BusLayer;
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
    [Authorize(Roles = "student")]

    public class StudentController : Controller
    {
       
       
            UnitOfWork unit = new UnitOfWork();
            StudentService ss = new StudentService();
        CourseService cs = new CourseService();



            public ActionResult AccountInfo()
            {
                var data = ss.StudInfo().FirstOrDefault(s => s.StudentName ==User.Identity.Name);
                var svm = new StudentViewModel(
                    data.StudentName,
                    data.Courses.Select(c => c.CourseName),
                    data.UserPhoto
                    )
                {
                    Age=data.Age
                };
                TempData["Photo"] = svm.StudentPhoto;
            
                
                return View(svm);
            }

        public ActionResult FinishedCourses()
        {
            var data = ss.StudInfo().FirstOrDefault(s=>s.StudentName==User.Identity.Name);
            var jvm = new CourseViewModel{Journal= data.Marks.Where(m => m.SubjectMark > 0).Select(m=>new JournalViewModel(
                m.Course.CourseName,
                m.Student.StudentName,
                m.SubjectMark
                )).ToList()};

            return View(jvm);
        }


        public ActionResult FutureCourses() {
            var data = ss.FutureCourse(User.Identity.Name);
            var cvm = data.Select(d => new CourseViewModel
            {
                CourseName = d.CourseName
            });

            return View(cvm.ToList());
        }

        public ActionResult CurrentCourses() {
            var data = ss.CurrentCourse(User.Identity.Name);
            var cvm = data.Select(d => new CourseViewModel
            {
                CourseName=d.CourseName
            });

            return View(cvm.ToList());
        }

            public ActionResult EditProfile(StudentViewModel model, string ph)
            {
            ss.StudEditor(model.StudentName, model.Age, ph);
            var data = ss.StudInfo().FirstOrDefault(s => s.StudentName == User.Identity.Name);
                var svm = new StudentViewModel(
                    data.StudentName,
                    data.Courses.Select(c => c.CourseName),
                    data.UserPhoto
                    );
            unit.Dispose();
            return PartialView(svm.StudentPhoto);
            }

            public ActionResult ChangeStatus()
        {
            var isChanged = ss.ChangeStatus(User.Identity.Name);
                if (isChanged == true)
                {
                    return RedirectToAction("AccountInfo");
                }
                else
                {
                    ModelState.AddModelError("", "Произошла ошибка");
                }
                return View();
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
