using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Areas.Home.Models.NewsFeedModel
{
    public class NewsFeedModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }

        public long NewFeedID { get; set; }

        public string Code { get; set; }
        public DateTime Createdate { get; set; }

        public string Title1 { get; set; }
        public string Thumb { get; set; }
      
        [AllowHtml]
        public string Description { get; set; }
    }
}