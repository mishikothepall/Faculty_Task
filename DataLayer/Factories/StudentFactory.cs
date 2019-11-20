using DataLayer.Context;
using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Factories
{
    public interface IStudManager {
        IStudentInfo GetStud();
        ICreateStudent studentCreator();
        ISubscribe Subscribe();
        IChangeStatus Change();
        IAddMark Estimate();
        IPhoto UserIco();
        IEditStud studEdit();
    }

    public interface IStudentInfo {
        IEnumerable<Student> StudInfo(AppIdentityDbContext db);
    }

    public interface ICreateStudent {
        IEnumerable<string> CreateStudent(AppUser user, IAuthenticationManager manager, AppIdentityDbContext db);
    }

    public interface ISubscribe{
        bool CourseSubscribe(string studName,string courseName, AppIdentityDbContext db);
    }

    public interface IChangeStatus {
        bool ChangeStatus(string name, AppIdentityDbContext db);
    }

    public interface IPhoto {
        bool AddPhoto(string user, byte[] photo, AppIdentityDbContext db);
    }

    public interface IEditStud {
        bool EditStud(string name, int age,  byte[] photo);
    }

    class StudEditor : IEditStud {

        AppIdentityDbContext db = new AppIdentityDbContext();

        public bool EditStud(string name, int age, byte[]photo)
        {
            var student = db.Students.FirstOrDefault(s=>s.StudentName==name);
            if (student != null)
            {
                student.StudentName = name;
                student.UserPhoto = photo;
                student.Age = age;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            else {
                return false;
            }
        }
    }

    class Photo : IPhoto {
        

        public bool AddPhoto(string user, byte[] photo, AppIdentityDbContext db)
        {
            var student = db.Students.FirstOrDefault(s=>s.StudentName==user);
            
            if (student != null)
            {
                student.UserPhoto = photo;
                db.Entry(student).State = EntityState.Modified;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Change : IChangeStatus {
        

        public bool ChangeStatus(string name, AppIdentityDbContext db)
        {
            var student = db.Students.FirstOrDefault(s=>s.StudentName==name);

            if (student !=null && student.ChangeStatus==false) {
                student.ChangeStatus = true;
                db.Entry(student).State = EntityState.Modified;
                return true;
            }

            return false;
        }
    }

    class Subscriber : ISubscribe {
        

        public bool CourseSubscribe(string studName,string courseName, AppIdentityDbContext db)
        {
            var student = db.Students.FirstOrDefault(s=>s.StudentName==studName);
            var course = db.Courses.FirstOrDefault(c=>c.CourseName==courseName);

            if (student != null && course != null) {

                student.Courses.Add(course);
                course.Group.Add(student);
                db.Entry(student).State = EntityState.Modified;
                db.Entry(course).State = EntityState.Modified;

                //Добавление студента в журнал
                db.Results.Add(new Journal { Student = student, Course = course });
                return true;
            }
            return false;
        }
    }

//Добавление оценок
    public interface IAddMark
    {
       void AddMark(string subj, IDictionary<string, int> jr, AppIdentityDbContext db);
    }

    class MarkGen : IAddMark
    {
        

        public void AddMark(string subj, IDictionary<string, int> marksList, AppIdentityDbContext db)
        {
            var student = db.Students.Include("Courses").Include("Marks");
            var course = db.Courses.FirstOrDefault(c => c.CourseName == subj);
            var marks = db.Results.Include("Course").Include("Student").Where(c=>c.Course.CourseName==subj);

            if (marks != null)
            {
                foreach (KeyValuePair<string, int> kvp in marksList)
                {
                    var m = marks.FirstOrDefault(s => s.Student.StudentName == kvp.Key);
                    m.SubjectMark = kvp.Value;
                    var st = student.FirstOrDefault(s => s.StudentName == kvp.Key);
                    st.Marks.Add(m);
                    course.Marks.Add(m);
                    db.Entry(st).State = EntityState.Modified;
                    db.Entry(course).State = EntityState.Modified;
                }
            }
            else {
                throw new Exception();
            }
        }
    }

//Регистрация студента
    class StudentCreator : ICreateStudent
    {
       
        public IEnumerable<string> CreateStudent(AppUser user, IAuthenticationManager manager, AppIdentityDbContext db)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
            UserRoleManager roleMgr = new UserRoleManager(new RoleStore<UserRoles>(db));

            IdentityResult res = userMgr.Create(user, user.Password);
            userMgr.CheckPassword(user,user.Password);
          
            
            if (res.Succeeded && roleMgr.RoleExists("student") )
            {
                IdentityResult role = userMgr.AddToRole(user.Id, "student");
                db.Students.Add(new Student {
                StudentName=user.UserName,
                });

                ClaimsIdentity identity = userMgr.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                manager.SignOut();
                manager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = false
                }, identity);

                return null;
            }
            else
            {
                return res.Errors;
            }
        }
    }

    class StudentInfo : IStudentInfo {
       

        public IEnumerable<Student> StudInfo(AppIdentityDbContext db)
        {
            return db.Students.Include("Courses").Include("Marks").ToList();
        }
    }

    class ConcreteStudFactory : IStudManager
    {
        public IChangeStatus Change()
        {
            return new Change();
        }

        public IAddMark Estimate()
        {
            return new MarkGen();
        }

        public IStudentInfo GetStud()
        {
            return new StudentInfo();
        }

        public IEditStud studEdit()
        {
            return new StudEditor();
        }

        public ICreateStudent studentCreator()
        {
            return new StudentCreator();
        }

        public ISubscribe Subscribe()
        {
            return new Subscriber();
        }

        public IPhoto UserIco()
        {
            return new Photo();
        }
    }

   public class StudentFactory
    {
        public IStudManager student = new ConcreteStudFactory();
    }
}
