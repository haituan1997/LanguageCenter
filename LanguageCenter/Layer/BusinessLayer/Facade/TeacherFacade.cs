﻿using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class TeacherFacade
    {
        SqlServerTeacher sqlServerTeacher = new SqlServerTeacher();
        public IEnumerable<Teacher> Get_Teacheres()
        {
            return sqlServerTeacher.Get_Teacheres();
        }

        public TeacherResponse Insert(Teacher teacher)
        {
            var response = new TeacherResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerTeacher.Insert(teacher);

                response.TeacherID = teacher.TeacherID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public class TeacherResponse : ResponseBase
        {
            public long TeacherID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}