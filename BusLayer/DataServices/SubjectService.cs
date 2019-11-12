using AutoMapper;
using BusLayer.DataModels;
using DataLayer.Factories;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusLayer.DataServices
{
    interface ISubjService {
        IEnumerable<CourseSubjectDTO> SubjectInfo();
        CourseSubjectDTO PickSubject(string name);

    }
   public class SubjectService : ISubjService
    {
        private SubjectFactory sf = new SubjectFactory();

        private static MapperConfiguration Config
        {

            get
            {
                return new MapperConfiguration(cfg => {
                    cfg.CreateMap<Course, CourseDTO>();
                    cfg.CreateMap<Tutor, TutorDTO>().ForMember(td => td.Courses, c => c.MapFrom(t => t.Courses));
                    cfg.CreateMap<Journal, JournalDTO>();
                    cfg.CreateMap<Student, StudentDTO>().ForMember(sd => sd.Courses, se => se.MapFrom(s => s.Courses));
                    cfg.CreateMap<CourseSubject, CourseSubjectDTO>().ForMember(cs => cs.Courses, sc => sc.MapFrom(s => s.Courses));
                });
            }

        }

        IMapper mapper = new Mapper(Config);

        public IEnumerable<CourseSubjectDTO> SubjectInfo()
        {
            return mapper.Map<List<CourseSubject>, List<CourseSubjectDTO>>(sf.subject.SubjList().SubjInfo().ToList());
        }

        public CourseSubjectDTO PickSubject(string name)
        {
            return mapper.Map<List<CourseSubject>, List<CourseSubjectDTO>>(sf.subject.SubjList().SubjInfo().ToList())
                .FirstOrDefault(s=>s.SubjectName==name);
        }
    }
}
