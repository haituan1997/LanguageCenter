﻿using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Repository
{
    public class TeacherRepository
    {
        TeacherFacade teacherFacade= new TeacherFacade();
        public TeacherRepository()
        {
            teacherFacade = new TeacherFacade();
        }

        public IEnumerable<Teacher> Get_Teacheres()
        {
            try
            {
                return teacherFacade.Get_Teacheres();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Insert(Teacher teacher)
        {
            try
            {
                var response = teacherFacade.Insert(teacher);
                if (response.Acknowledge == AcknowledgeType.Failure)
                {
                    throw new Exception(response.Message);
                }
                return response.TeacherID.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}