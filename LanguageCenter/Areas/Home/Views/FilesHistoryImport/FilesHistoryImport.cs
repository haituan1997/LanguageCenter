using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Areas.Home.Views.FilesHistoryImport
{
    public class FilesHistoryImport
    {
        public string ControllerName { get; set; }
        public string FileName { get; set; }
        public byte TypeUser { get; set; }
        public long UserID { get; set; }
    }
}