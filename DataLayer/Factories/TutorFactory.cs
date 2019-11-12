using DataLayer.Context;
using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Factories
{
    public interface ITutManager {
        ITutorInfo GetInfo();
        IUnlockTutor TutorUnlock();
        IAddCourse AddToCourse();
    }

    public interface ITutorInfo {
        IEnumerable<Tutor> GetTutor(AppIdentityDbContext db);
    }


    public interface IUnlockTutor {
        bool unlockTutor(string name, AppIdentityDbContext db);
    }

    public interface IAddCourse {
        bool AddCourse(string tut, string course, AppIdentityDbContext d);
    }

    class Attach : IAddCourse {
        

        public bool AddCourse(string tut, string subj, AppIdentityDbContext db)
        {
            var tutor = db.Tutors.FirstOrDefault(t=>t.TutorName==tut);
            var course = db.Courses.FirstOrDefault(c=>c.CourseName==subj);

            if (tutor != null) {

                    tutor.Courses.Add(course);
                    course.Teacher = tutor;
                    db.Entry(tutor).State = EntityState.Modified;
                    db.Entry(course).State = EntityState.Modified;

                    return true;
                
            }
            return false;
        }
    }

    class UnlockTut : IUnlockTutor {

        public bool unlockTutor(string name, AppIdentityDbContext db)
        {
            var tutor = db.Tutors.FirstOrDefault(t=>t.TutorName==name);
            var student = db.Students.FirstOrDefault(s => s.StudentName == name);

            if (student!=null)
            {
                AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(db));
                UserRoleManager roleMgr = new UserRoleManager(new RoleStore<UserRoles>(db));

                var user = userMgr.Users.FirstOrDefault(u=>u.UserName == name);

                if (user!=null)
                {
                    userMgr.RemoveFromRole(user.Id, "student");
                    IdentityResult role = userMgr.AddToRole(user.Id, "teacher");
                    Tutor tut = new Tutor() { TutorName=user.UserName, IsBlocked=false};

                    db.Tutors.Add(tut);
                    db.Students.Remove(student);

                    return true;
                }
                else
                {
                    return false;
                }

            }
            else {
                return false;
            }
        }
    }


    class TutInfo : ITutorInfo {

        public IEnumerable<Tutor> GetTutor(AppIdentityDbContext db)
        {
            return db.Tutors.Include("Courses").Where(t => t.IsBlocked == false).ToList() ;
        }
    }

    class ConcreteTutorFactory : ITutManager
    {
        public IAddCourse AddToCourse()
        {
            return new Attach();
        }

        public ITutorInfo GetInfo()
        {
            return new TutInfo();
        }


        public IUnlockTutor TutorUnlock()
        {
            return new UnlockTut();
        }
    }

    public class TutorFactory
    {
        public ITutManager tutor = new ConcreteTutorFactory();
        
    }
}
