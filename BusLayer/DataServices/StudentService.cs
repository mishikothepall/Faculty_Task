using AutoMapper;
using BusLayer.DataModels;
using DataLayer.Context;
using DataLayer.Factories;
using DataLayer.Models;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataServices
{
    interface IStudentService {
        IEnumerable<StudentDTO> StudInfo();
        IEnumerable<string> CreateStudent(string name, string email, string password, IAuthenticationManager manager);
        bool CourseSubscribe(string studName, string courseName);
        bool ChangeStatus(string name);
        bool AddPhoto(string user, string path);
        void AddMark(string subj, IDictionary<string, int> marksList);
        bool StudEditor(string name, int age, string photo);
        IEnumerable<CourseDTO> CurrentCourse(string name);
        IEnumerable<CourseDTO> FutureCourse(string name);
    }
    public class StudentService : IStudentService
    {
        private StudentFactory sf = new StudentFactory();

        private CourseFactory cf = new CourseFactory();

        private UnitOfWork unit = new UnitOfWork();

        private static MapperConfiguration Config
        {

            get
            {
                return new MapperConfiguration(cfg => {
                    cfg.CreateMap<Course, CourseDTO>().ForMember(md=>md.Marks, me=>me.MapFrom(m=>m.Marks));
                    cfg.CreateMap<Tutor, TutorDTO>().ForMember(td => td.Courses, c => c.MapFrom(t => t.Courses));
                    cfg.CreateMap<Journal, JournalDTO>();
                    cfg.CreateMap<Student, StudentDTO>().ForMember(sd => sd.Courses, se => se.MapFrom(s => s.Courses));
                });
            }

        }

        IMapper mapper = new Mapper(Config);

        public void AddMark(string subj, IDictionary<string, int> marksList)
        {
            sf.student.Estimate().AddMark(subj, marksList, unit.context);
            unit.Save();
            unit.Dispose();
        }

        public bool AddPhoto(string user, string path)
        {
           path = path.Substring(path.IndexOf(',')+1);
           var photo = Convert.FromBase64String(path);
           bool stat = sf.student.UserIco().AddPhoto(user, photo, unit.context);
            if (stat==true) {
                unit.Save();
                
            }
            //unit.Dispose(); это дело в контроллер
            return stat;
        }

        public bool ChangeStatus(string name)
        {
            bool stat = sf.student.Change().ChangeStatus(name, unit.context);

            if (stat == true) {
                unit.Save();
                
            }

            return stat;
        }

        public bool CourseSubscribe(string studName, string courseName)
        {
            bool stat = sf.student.Subscribe().CourseSubscribe(studName, courseName, unit.context);
            if (stat == true) {
                unit.Save();
                
                
            }
            return stat;
        }

        public IEnumerable<string> CreateStudent(string name, string email, string password, IAuthenticationManager manager)
        {
            AppUser user = new AppUser { UserName = name, Email = email, Password = password };
            var stat = sf.student.studentCreator().CreateStudent(user, manager, unit.context);
            if (stat == null)
            {
                unit.Save();
                return stat;

            }
            return stat;
        }

        public IEnumerable<StudentDTO> StudInfo()
        {
            return mapper.Map<List<Student>, List<StudentDTO>>(sf.student.GetStud().StudInfo(unit.context).ToList());
        }

        public bool StudEditor(string name, int age, string photo)
        {
            photo = photo.Substring(photo.IndexOf(',') + 1);
            var ph = Convert.FromBase64String(photo);
            var res= sf.student.studEdit().EditStud(name, age, ph);

            unit.Save();
            return res;
            
        }

        public IEnumerable<CourseDTO> CurrentCourse(string name)
        {
            return StudInfo().FirstOrDefault(s=>s.StudentName==name).Courses.Where(dt => (DateTime.Parse(dt.EndDate)) > DateTime.Now &&
            (DateTime.Parse(dt.BeginDate)) < DateTime.Now).ToList();
        }

        public IEnumerable<CourseDTO> FutureCourse(string name) {
            return StudInfo().FirstOrDefault(s => s.StudentName == name).Courses.Where(dt => (DateTime.Parse(dt.BeginDate)) > DateTime.Now).ToList();
        }

    }
}
