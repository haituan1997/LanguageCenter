using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class FilterPrice : Payment
    {
        public int Paged { get; set; }
        public int PagedSize { get; set; }
        public string SearchBy { get; set; }
        public string OrderByColumn { get; set; }
    }
}