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
    interface ITutorService {
        IEnumerable<TutorDTO> TutorInfo();

        IEnumerable<TutorDTO> BlockedTutors();

        bool unlockTutor(string name);

        string AttachCourse(string tut, string course);

        TutorDTO GetSpecTutor(int id);

        TutorDTO GetByName(string name);

    }
   public class TutorService : ITutorService
    {
        private TutorFactory tf = new TutorFactory();

        private UnitOfWork unit = new UnitOfWork();

        private static MapperConfiguration Config
        {

            get
            {
                return new MapperConfiguration(cfg => {
                    cfg.CreateMap<Course, CourseDTO>();
                    cfg.CreateMap<Tutor, TutorDTO>().ForMember(td => td.Courses, c => c.MapFrom(t => t.Courses));
                    cfg.CreateMap<Journal, JournalDTO>();
                    cfg.CreateMap<Student, StudentDTO>().ForMember(sd => sd.Courses, se => se.MapFrom(s => s.Courses));
                });
            }

        }

        IMapper mapper = new Mapper(Config);

        public string AttachCourse(string tut, string course)
        {
            bool add = tf.tutor.AddToCourse().AddCourse(tut, course, unit.context);
            if (add == true) {

                unit.Save();
                return string.Empty;
            }
            return "Такой преподаватель не найден";
        }

        public IEnumerable<TutorDTO> BlockedTutors()
        {
            return mapper.Map<List<Tutor>, List<TutorDTO>>(tf.tutor.GetInfo().GetTutor(unit.context).ToList())
                .Where(t => t.IsBlocked == true).ToList();
        }

        public TutorDTO GetByName(string name)
        {
            return mapper.Map<List<Tutor>, List<TutorDTO>>(tf.tutor.GetInfo().GetTutor(unit.context).ToList())
                .FirstOrDefault(t=>t.TutorName==name);
        }

        public TutorDTO GetSpecTutor(int id)
        {
            return mapper.Map<List<Tutor>, List<TutorDTO>>(tf.tutor.GetInfo().GetTutor(unit.context).ToList())
                .FirstOrDefault(t=>t.TutorID==id);
        }

        public IEnumerable<TutorDTO> TutorInfo()
        {
            return mapper.Map<List<Tutor>, List<TutorDTO>>(tf.tutor.GetInfo().GetTutor(unit.context).ToList());
        }

        public bool unlockTutor(string name)
        {
            bool tut = tf.tutor.TutorUnlock().unlockTutor(name, unit.context);
            if (tut == true) {
                unit.Save();
                return true;
            }
            else

                return false;
        }
    }
}
