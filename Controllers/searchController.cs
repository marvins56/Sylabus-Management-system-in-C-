//using SMIS.Models;
//using SMIS.Models.ViewModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using PagedList.Mvc;
//using PagedList;

//namespace SMIS.Controllers
//{
//    public class searchController : Controller
//    {
//        private SMISEntities db = new SMISEntities();
//        // GET: search
//        public PartialViewResult searchBar(string sortOrder, string currentFilter, string searchString, int? page)
//        {
//            try
//            {
//                var weeksdetails = Get_weeks_info();
//                var topicsdetails = Get_Topics_info();

//                var search_text = new searchBar();

//                search_text.Weeks = weeksdetails;
//                search_text.Topics = topicsdetails;

//                //ViewBag.CurrentSort = sortOrder;
//                //ViewBag.inquirydesc = String.IsNullOrEmpty(sortOrder) ? "inquirydesc" : "";
//                //ViewBag.Date = sortOrder == "Date" ? "date_desc" : "Date";
//                if (searchString != null)
//                {
//                    page = 1;
//                }
//                else
//                {
//                    searchString = currentFilter;
//                }

//                ViewBag.CurrentFilter = searchString;

//                var result = from s in search_text select s;
//                if (!String.IsNullOrEmpty(searchString))
//                {
//                    result = result.Where(s => s.inquirry.Contains(searchString)
//                                           || s.UserId.Contains(searchString));
//                }


//                switch (sortOrder)
//                {
//                    case "Description_desc":
//                        result = result.OrderByDescending(s => s.inquirry);
//                        break;
//                    case "Date":
//                        result = result.OrderBy(s => s.Dateposteed);
//                        break;
//                    case "date_desc":
//                        result = result.OrderByDescending(s => s.Dateposteed);
//                        break;
//                    default:
//                        result = result.OrderBy(s => s.Dateposteed);
//                        break;
//                }

//                int pageSize = 6;
//                int pageNumber = (page ?? 1);
                
//                return PartialView("searchBar", search_text.ToPagedList(pageNumber, pageSize));
//            }catch(Exception ex)
//            {
//                TempData["error"] = ex.Message;
//            }
//            return PartialView("searchBar", null);
//        }

//        public List<WeeksTable> Get_weeks_info()
//        {
//            return (db.WeeksTables.ToList());
//        }

//        public List<topicstat> Get_stats_info()
//        {
//           var res =db.topicstats.ToList();
//            return res;
//        }
//        public List<TopicsTable> Get_Topics_info()
//        {
//            var Res = db.TopicsTables.ToList();
//            return Res;

//        }
//        public List<SubTopicsTable> Get_subtopics_info()
//        {
//            var res = db.SubTopicsTables.ToList();
//            return res;
//        }
//    }
//}