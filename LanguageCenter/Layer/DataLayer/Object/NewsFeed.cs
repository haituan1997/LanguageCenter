using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class NewsFeed
    { 
        public long NewFeedID { get; set; } 

        public string Code { get; set; }
        public DateTime Createdate { get; set; }
        public string CreatedateView { get; set; }
        public string Thumb { get; set; }

        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
         

    }

}