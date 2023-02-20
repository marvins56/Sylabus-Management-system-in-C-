using SMIS.Models;
using SMIS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMIS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private SMISEntities db = new SMISEntities();
        public ActionResult Index()

        {
            var students = get_Students();
            var classes  = get_Classes();
            var subjects = get_subjects();
            var Teacher = get_teachers();

            Overal_Dashboard dashboard_content = new Overal_Dashboard();
            dashboard_content.students= students;
            dashboard_content.classes = classes;
            dashboard_content.subjects = subjects;
            dashboard_content.Teachers = Teacher;


            return View(dashboard_content);
        }
        public List<StudentsTable> get_Students()
        {
            var result = db.StudentsTables.ToList();
            return result;
        }

        public List<Teacher> get_teachers()
        {
            var result = db.Teachers.ToList();
            return result;
        }
        public List<ClassTable> get_Classes()
        {
            var result = db.ClassTables.ToList();
            return result;
        }
        public List<SubjectTable> get_subjects()
        {
            var result = db.SubjectTables.ToList();
            return result;
        }
    }
}