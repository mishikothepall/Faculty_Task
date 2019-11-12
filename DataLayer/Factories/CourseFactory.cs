using DataLayer.Context;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Factories
{
    public interface ICourseManager {

        ICourseInfo GetInfo();
        ICourseCreator AddCourse();
        IJournal GetMarks(); 
    }

    public interface ICourseInfo {

        IEnumerable<Course> CourseInfo(AppIdentityDbContext db); 
    }

    public interface IJournal {

        IEnumerable<Journal> GetJournal(AppIdentityDbContext db);

    }

    public interface ICourseCreator {

        bool CreateCourse(Course info, string tut, string subj, AppIdentityDbContext db);

    }

    class Grades : IJournal
    {

        public IEnumerable<Journal> GetJournal(AppIdentityDbContext db)
        {
            return db.Results.Include("Student").Include("Course").ToList();
        }
    }

    class CourseCreator : ICourseCreator {


        public bool CreateCourse(Course info, string tut, string subj, AppIdentityDbContext db)
        {
            var isExist = db.Courses.FirstOrDefault(c=>c.CourseName==info.CourseName);
            var tutor = db.Tutors.FirstOrDefault(t=>t.TutorName==tut);
            var subject = db.Subjects.FirstOrDefault(s=>s.SubjectName==subj);
            if (isExist == null) {
                db.Courses.Add(new Course {
                    CourseName = info.CourseName,
                    BeginDate = info.BeginDate,
                    EndDate=info.EndDate,
                    Teacher = tutor,
                    Description = info.Description,
                    Subject = subject
                });
                
                return true;
            }
            return false;
        }
    }

    class Info : ICourseInfo
    {

        public IEnumerable<Course> CourseInfo(AppIdentityDbContext db)
        {
            return db.Courses.Include("Teacher").Include("Group")
                .Include("Marks").Include("Subject").Where(f => f.IsBlock == false).ToList();
        }
    }

    class ConcreteCourseFactory : ICourseManager
    {
        public ICourseCreator AddCourse()
        {
            return new CourseCreator();
        }

        public ICourseInfo GetInfo()
        {
            return new Info();
        }

        public IJournal GetMarks()
        {
            return new Grades();
        }
    }

   public class CourseFactory
    {
        public ICourseManager cm = new ConcreteCourseFactory();
    }
}
