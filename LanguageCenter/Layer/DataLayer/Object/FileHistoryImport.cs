using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.Object
{
    public class FileHistoryImport
    {
        public long FileHistoryImportID { get; set; }

        public string ControllerName { get; set; }

        public long UserID { get; set; }

        public byte TypeUser { get; set; }

        public string FileName { get; set; }
    }
}