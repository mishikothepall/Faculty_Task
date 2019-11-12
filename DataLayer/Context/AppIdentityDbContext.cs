using DataLayer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Context
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext():base("name = DBContext")  { }



        static AppIdentityDbContext() {

            Database.SetInitializer<AppIdentityDbContext>(new IdenInit());

        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasKey(c=>c.CourseID);

            modelBuilder.Entity<Tutor>().HasKey(t=>t.TutorID);

            modelBuilder.Entity<Student>().HasKey(s=>s.StudentID);

            modelBuilder.Entity<Journal>().HasKey(j=>j.ResultID);

            modelBuilder.Entity<CourseSubject>().HasKey(S=>S.SubjectID);

            modelBuilder.Entity<CourseSubject>().HasMany(s => s.Courses).WithRequired(c => c.Subject);

            modelBuilder.Entity<Tutor>().HasMany(t => t.Courses).WithRequired(c=>c.Teacher);

            modelBuilder.Entity<Course>().HasRequired(c => c.Teacher).WithMany(t=>t.Courses);

            modelBuilder.Entity<Course>().HasMany(j => j.Marks).WithRequired(c=>c.Course);

            modelBuilder.Entity<Student>().HasMany(s => s.Marks).WithRequired(j=>j.Student);

            modelBuilder.Entity<Student>().HasMany(s => s.Courses).WithMany(c => c.Group).Map(g=> {

                g.MapLeftKey("StudentRefId");

                g.MapRightKey("CourseRefId");

                g.ToTable("StudentCourse");

            });

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<Tutor> Tutors { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Journal> Results { get; set; }

        public DbSet<CourseSubject> Subjects { get; set; }



        public static AppIdentityDbContext Create() {
            return new AppIdentityDbContext();
        }
    }

    public class IdenInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            
            InitData(context);


            var tutors = new List<Tutor> {
            new Tutor {TutorName="Орбинсктй Олег", IsBlocked = false },
            new Tutor { TutorName = "Куприянов Евгений", IsBlocked = false },
            new Tutor {TutorName="Борисов Николай", IsBlocked = false },
            new Tutor { TutorName = "Хайров Виктор", IsBlocked = false },
            new Tutor {TutorName="Юрченко Юрий", IsBlocked = false },
            new Tutor { TutorName = "Шаронов Владислав", IsBlocked = false }
            };
            tutors.ForEach(t => context.Tutors.Add(t));
            context.SaveChanges();

            var subject1 = context.Subjects.Add(new CourseSubject { SubjectName="Linguistics", SubjectLogo=File.ReadAllBytes("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/TempImg/depositphotos_100866232-stock-photo-linguistics-word-cloud-concept-8.jpg") });
            var subject2 = context.Subjects.Add(new CourseSubject { SubjectName="Programing", SubjectLogo=File.ReadAllBytes("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/TempImg/mailservice.png") });
            var subject3 = context.Subjects.Add(new CourseSubject {SubjectName="Mathematics", SubjectLogo=File.ReadAllBytes("C:/Users/poden/OneDrive/Рабочий стол/Fclty2/TempImg/IT-FUN.ONE_.jpg") });
            context.SaveChanges();

            var course1 = context.Courses.Add(new Course
            {
                 CourseName = "Латинский язык",
                 Teacher = tutors.FirstOrDefault(t=>t.TutorID==1),
                 BeginDate = new DateTime(2019, 03, 01).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                 EndDate = new DateTime(2019, 06, 08).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                 Description = "О курсе Курс предоставит вам возможность прикоснуться к богатствам латинского языка, который оказал влияние на становление и развитие нескольких европейских, таких как французский, итальянский испанский, английский и другие.Поэтому изучение латыни облегчит вам в будущем освоение новых иностранных языков или откроет неожиданные грани в уже известных.Так же как логика и математика, практические навыки лингвистического анализа латинских текстов положительно скажутся на любой научно - исследовательской деятельности."
            });

            var course2 = context.Courses.Add(new Course
            {
                CourseName = "Математическая статистика",
                Teacher = tutors.FirstOrDefault(t => t.TutorID == 1),
                BeginDate = new DateTime(2018, 09, 05).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                EndDate = new DateTime(2018, 12, 04).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                Description = "Математи́ческая стати́стика — наука, разрабатывающая математические методы систематизации и использования статистических данных для научных и практических выводов. Во многих своих разделах математическая статистика опирается на теорию вероятностей, дающую возможность оценить надёжность и точность выводов,делаемых на основании ограниченного статистического материала(например, оценить необходимый объём выборки для получения результатов требуемой точности при выборочном обследовании)"
            });

            var course3 = context.Courses.Add(new Course
            {
                CourseName = "Прикладная лингвистика",
                Teacher = tutors.FirstOrDefault(t => t.TutorID == 2),
                BeginDate = new DateTime(2019, 07, 02).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                EndDate = new DateTime(2019, 10, 11).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                Description = "Прикладна́я лингви́стика (прикладное языкознание) — наряду с теоретической лингвистикой является частью науки, занимающейся языком. Специализируется на решении практических задач, связанных с изучением языка, а также на практическом использовании лингвистической теории в других областях"
            });

            var course4 = context.Courses.Add(new Course
            {
                CourseName = ".NET/C#",
                Teacher = tutors.FirstOrDefault(t => t.TutorID == 1),
                BeginDate = new DateTime(2019, 11, 07).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                EndDate = new DateTime(2020, 02, 15).ToString("dd MM yyyy", CultureInfo.InvariantCulture),
                Description = "Курсы C# в Харькове Язык программирования C# можно уверенно назвать одним из самых популярных во всем мире. Это мощный, динамично развивающийся и востребованный язык. Сегодня на нем пишется подавляющее количество всевозможных приложений, начиная от компьютерных и включая приложения для веб-сервисов, которые взаимодействуют с многомиллионной аудиторией."
            });

            tutors.FirstOrDefault(t=>t.TutorID==1).Courses.Add(course1);
            tutors.FirstOrDefault(t => t.TutorID == 1).Courses.Add(course2);
            tutors.FirstOrDefault(t => t.TutorID == 1).Courses.Add(course4);

            tutors.FirstOrDefault(t => t.TutorID == 2).Courses.Add(course3);


            var student1 = context.Students.Add(new Student { StudentName = "Бондаренко Александр" });
            var student2 = context.Students.Add(new Student { StudentName = "Греховодов Богдан" });
            var student3 = context.Students.Add(new Student { StudentName = "Голуб Алина" });
            var student4 = context.Students.Add(new Student { StudentName = "Евтушенко Виктория" });
            var student5 = context.Students.Add(new Student { StudentName = "Гагарин Станислав" });
            var student6 = context.Students.Add(new Student { StudentName = "Дятлов Олег" });
            var student7 = context.Students.Add(new Student { StudentName = "Жилин Игорь" });

            subject1.Courses.Add(course3);
            subject1.Courses.Add(course1);
            subject3.Courses.Add(course2);
            subject2.Courses.Add(course4);

            
            course1.Group.Add(student1);
            course4.Group.Add(student1);
            course1.Group.Add(student2);
            course4.Group.Add(student2);
            course2.Group.Add(student3);
            course2.Group.Add(student4);
            course3.Group.Add(student5);
            course3.Group.Add(student6);
            course4.Group.Add(student7);


            student1.Courses.Add(course1);
            student1.Courses.Add(course4);
            student2.Courses.Add(course1);
            student2.Courses.Add(course4);
            student3.Courses.Add(course2);
            student4.Courses.Add(course2);
            student5.Courses.Add(course3);
            student6.Courses.Add(course3);
            student7.Courses.Add(course4);


            context.Results.Add(new Journal { Student = student3, Course = course2 });
            context.Results.Add(new Journal { Student = student4, Course = course2 });

            


            context.SaveChanges();

            base.Seed(context);
        }

        private void InitData(AppIdentityDbContext context) {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            UserRoleManager roleMgr = new UserRoleManager(new RoleStore<UserRoles>(context));

            string userName = "Admin";
            string password = "mypassword";
            string role = "admin";
            string email = "admin@mail.com";

                roleMgr.Create(new UserRoles("teacher"));
                roleMgr.Create(new UserRoles("admin"));
                roleMgr.Create(new UserRoles("student"));

            AppUser user = userMgr.FindByName(userName);

            if (!roleMgr.RoleExists(role)) {
                roleMgr.Create(new UserRoles(role));
            }

            if (user == null)
            {
                userMgr.Create(new AppUser { UserName = userName, Email = email, },
                    password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id,role)) {

                userMgr.AddToRole(user.Id, role);
            }

            
        } 
    }
}
