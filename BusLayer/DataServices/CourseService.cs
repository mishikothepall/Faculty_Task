using AutoMapper;
using BusLayer.DataModels;
using DataLayer.Context;
using DataLayer.Factories;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataServices
{
    interface ICourseService {
        IEnumerable<CourseDTO> CourseInfo();
        IEnumerable<CourseDTO> SortCourse(string sort);
        IEnumerable<CourseDTO> SortCourse(string sort, string search);
        IEnumerable<JournalDTO> GetGrades(string course);
        string CreateCourse(string tut, string name, string begin, string end, string desc, string subject);
    }

   public class CourseService:ICourseService
    {
        private CourseFactory cf = new CourseFactory();

        private UnitOfWork unit = new UnitOfWork();

        private static MapperConfiguration Config {

            get {
                return new MapperConfiguration(cfg => {
                    cfg.CreateMap<Course, CourseDTO>();
                    cfg.CreateMap<Tutor, TutorDTO>().ForMember(td => td.Courses, c => c.MapFrom(t => t.Courses));
                    cfg.CreateMap<Journal, JournalDTO>();
                    cfg.CreateMap<Student, StudentDTO>().ForMember(sd => sd.Courses, se => se.MapFrom(s => s.Courses));
                    cfg.CreateMap<CourseSubject, CourseSubjectDTO>().ForMember(cs=>cs.Courses, sc=>sc.MapFrom(s=>s.Courses));
                });
            }

        }

        IMapper mapper = new Mapper(Config);

        public IEnumerable<CourseDTO> CourseInfo()
        {

            return  mapper.Map<List<Course>, List<CourseDTO>>(cf.cm.GetInfo().CourseInfo(unit.context).ToList());
        }



        public string CreateCourse(string tut, string name, string begin, string end, string desc, string subject)
        {
            var course = new Course()
            {
                CourseName = name,
                BeginDate = begin,
                EndDate = end,
                Description = desc
            };
            bool subj = cf.cm.AddCourse().CreateCourse(course, tut, subject, unit.context);
            if (subj == true)
            {

                unit.Save();
                return string.Empty;
            }
            return $"Преподаватель с именем {tut} не найден";
        }



        public IEnumerable<JournalDTO> GetGrades(string course)
        {
            return mapper.Map<List<Journal>, List<JournalDTO>>(cf.cm.GetMarks().GetJournal(unit.context).ToList()).Where(j=>j.Course.CourseName==course);
        }


        public IEnumerable<CourseDTO> SortCourse(string sort)
        {
            var course = mapper.Map<List<Course>, List<CourseDTO>>(cf.cm.GetInfo().CourseInfo(unit.context).ToList());
            if (sort.Equals("z-a"))
            {
                course = course.OrderByDescending(c => c.CourseName).ToList();
            }

            else if (sort.Equals("Самые длительные"))
            {

                course = course.OrderByDescending(c => (DateTime.Parse(c.EndDate)).Subtract(DateTime.Parse(c.BeginDate))).ToList();
            }

            else if (sort.Equals("Самые короткие"))
            {
                course = course.OrderBy(c => (DateTime.Parse(c.EndDate)).Subtract(DateTime.Parse(c.BeginDate))).ToList();
            }

            else if (sort.Equals("Самые популярные"))
            {
                course = course.OrderByDescending(c => c.Group.Count()).ToList();
            }

            return course;
        }
        

        public IEnumerable<CourseDTO> SortCourse(string sort, string search)
        {
            return SortCourse(sort).Where(c => c.CourseName.Contains(search));
        }
    }
}
