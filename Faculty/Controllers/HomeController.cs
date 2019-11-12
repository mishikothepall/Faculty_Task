using Faculty.Filters;
using Faculty.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using BusLayer.DataServices;
using BusLayer;
using BusLayer.DataModels;

namespace Faculty.Controllers
{
    [ExceptionLogAttribute]
    [UserActionLogger]
    public class HomeController : Controller
    {

        CourseService cs = new CourseService();
        TutorService ts = new TutorService();
        UserService us = new UserService();
        StudentService ss = new StudentService();
        SubjectService sbs = new SubjectService();


        public ActionResult CourseDesc(string name) {
            
            var data = cs.CourseInfo().FirstOrDefault(c => c.CourseName == name);
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

        public ActionResult Subscribe(string reg) {
            var data = reg.Split('/');
            var isRegistered = ss.CourseSubscribe(data[0], data[1]);
            if (isRegistered == false) {
                ModelState.AddModelError("", "Во время регистрации возникла ошибка!");
            }
            return RedirectToAction("HomePage");
        }
     
        public ActionResult HomePage()
        {
            

            SearchTemplate st = new SearchTemplate(sbs.SubjectInfo().Select(s => new SubjectViewModel(
                s.SubjectName,
                s.SubjectLogo,
                s.Courses.Select(c => c.CourseName).ToList()
                )).ToList(),
                cs.CourseInfo().Select(d => new CourseViewModel(
               d.CourseName,
               $"{d.Teacher.TutorName}",
               d.Teacher.TutorID,
               d.Group.Where(b => b.IsBlocked == false).Select(s => $"{s.StudentName}"),
               d.BeginDate,
               d.EndDate,
               null,
               d.Description
               )).ToList()
                );

            return View(st);
        }


        public ActionResult HomePartial( SearchTemplate model, string name)
        {
            List<CourseDTO> course = new List<CourseDTO>();
            if (!string.IsNullOrEmpty(name)) {
                course = cs.CourseInfo().Where(c => c.Subject.SubjectName == name).ToList();
            } else
            if (string.IsNullOrEmpty(model.Search))
            {
                course = cs.SortCourse(model.SelectedFilter).ToList();
            }
            else
            {
                course = cs.SortCourse(model.SelectedFilter, model.Search).ToList();
            }
            var cvm = course.Select(d => new CourseViewModel(
                   d.CourseName,
                   $"{d.Teacher.TutorName}",
                   d.Teacher.TutorID,
                   d.Group.Where(b => b.IsBlocked == false).Select(s => $"{s.StudentName}"),
                   d.BeginDate,
                   d.EndDate,
                   null,
                   d.Description
                   )).ToList();

            return PartialView(cvm);

        }

        [HttpGet]

        public ActionResult TutorsCourses(int id)
        {

            var data = ts.GetSpecTutor(id);

            var tvm = new TutorsViewModel(
                data.TutorID,
                $"{data.TutorName}",
                data.Courses.Select(c => c.CourseName)
                );

            return View(tvm);
        }

        //Список кандидатов на пост преподавателя (в админку)
        [Authorize(Roles = "admin")]
        public ActionResult TestView()
        {
            return View(ts.BlockedTutors());
        }
        //ЛогДата, тоже в админку
        public ActionResult XmlData() {
            XmlFileManager data = new XmlFileManager();

            return View(data.GetXmlData());
        }

        //Залогинится
        public ActionResult Login(string returnUrl) {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]

        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            var isCreated = us.Login(model.UserName, model.Password, AuthManager);

            if (isCreated.Equals(string.Empty))
            {
                if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("HomePage");
            }
                return Redirect(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", isCreated);
            }

                return View(model);
           
        }

       //Регистрация
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            if (ModelState.IsValid && !model.Password.Contains("12345") && !model.Email.Contains(".ru"))
            {
               var res= ss.CreateStudent(model.Name, model.Email, model.Password, AuthManager);
                if (res == null)
                {
                    return RedirectToAction("HomePage");
                }
                else
                {
                    AddResultErrors(res);
                }
            }
            ModelState.AddModelError(string.Empty, "Пароль не должен содержать последовательность чисел и адресс почтового ящика не должен заканчиваться на .ru");
            return View(model);
        }


        //Разлогиниться 
        public ActionResult LogOut() {
            AuthManager.SignOut();
            Session.Abandon();
            return RedirectToAction("HomePage", "Home");
        }

        private void AddResultErrors(IEnumerable<string> errors) {

            foreach (string error in errors) {
                ModelState.AddModelError(string.Empty, error);
            }
        }

        //Объект менеджера аутентификации
        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
   
        
    }
}