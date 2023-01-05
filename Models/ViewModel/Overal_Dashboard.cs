using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMIS.Models.ViewModel
{
    public class Overal_Dashboard
    {
        public List<WeeksTable> Weeks { get; set; }
        public List<TopicsTable> Topics { get; set; }
        public List<SubTopicsTable> subtopics { get; set; }
        public List<Year> years { get; set; }
        public List<ClassTable> classes { get; set; }
        public List<subject_class> subject_perclass { get; set; }
        public List<AspNetUser> users { get; set; }
        public List<StudentsTable> students { get; set; }
        public List<StudentMarks> students_marks { get; set; }
        public List<MidtermMarksTable> marks { get; set; }
        public List<SubjectTable> subjects { get; set; }
        public List<Term> terms { get; set; }
        public List<topicstat> topicstats { get; set; }

    }
}