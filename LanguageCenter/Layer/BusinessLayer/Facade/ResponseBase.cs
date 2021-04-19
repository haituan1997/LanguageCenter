using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.BusinessLayer.Facade
{
    public class ResponseBase
    {
        public string Message { get; set; }
        public AcknowledgeType Acknowledge { get; set; }
    }
    public enum AcknowledgeType
    {
        Failure,
        Success
    }
}