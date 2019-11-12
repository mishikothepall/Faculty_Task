using DataLayer.Context;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Factories
{
    public interface ISubjManager {
        ISubjectInfo SubjList();
    }

    public interface ISubjectInfo {
        IEnumerable<CourseSubject> SubjInfo();
    }

    class SubInfo : ISubjectInfo {
        AppIdentityDbContext db = new AppIdentityDbContext();

        public IEnumerable<CourseSubject> SubjInfo()
        {
            return db.Subjects.Include("Courses").ToList();
        }
    }

    class ConcreteSubjFactory : ISubjManager
    {
        public ISubjectInfo SubjList()
        {
            return new SubInfo();
        }
    }
    public class SubjectFactory
    {
        public ISubjManager subject = new ConcreteSubjFactory();

    }
}
