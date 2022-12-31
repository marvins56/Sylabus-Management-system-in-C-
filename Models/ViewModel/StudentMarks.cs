using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMIS.Models.ViewModel
{
    public class StudentMarks
    {
        public List<MidtermMarksTable> stdntMarks { get; set; }
        public List<StudentsTable> student { get; set; }

    }
}