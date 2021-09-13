using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class ObjectFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchBy { get; set; }
        public string OrderBy { get; set; }
    }
}