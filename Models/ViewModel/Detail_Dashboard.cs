﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMIS.Models.ViewModel
{
    public class Detail_Dashboard
    {
        public List<WeeksTable> Weeks { get; set; }
        public List<TopicsTable> Topics { get; set; }
        public List<SubTopicsTable> subtopics { get; set; }


    }
}