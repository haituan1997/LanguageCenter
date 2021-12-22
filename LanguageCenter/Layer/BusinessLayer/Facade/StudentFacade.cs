using LanguageCenter.BusinessLayer.Facade;
using LanguageCenter.Layer.DataLayer.Object;
using LanguageCenter.Layer.DataLayer.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.BusinessLayer.Facade
{
    public class StudentFacade
    {
        SqlServerStudent sqlServerStudent = new SqlServerStudent();
        SqlServerStudentAccount sqlServerStudentAccount = new SqlServerStudentAccount();
        public IEnumerable<Student> Get_Students(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            return sqlServerStudent.Get_Students(page, pageSize, orderBy, searchBy);
        }
        public IEnumerable<Student> GetAll_Students()
        {
            return sqlServerStudent.GetAll_Students();
        }
        public IEnumerable<Student> GetAll_StudentsNotAccont()
        {
            return sqlServerStudent.GetAll_StudentsNotAccont();
        }

        public Student Get_StudentByStudentID(long studenIDl)
        {
            return sqlServerStudent.Get_StudentByStudentID(studenIDl);
        }
        public int Count(string whereClause)
        {
            return sqlServerStudent.Count(whereClause);
        }
        public StudentResponse Insert(Student student)
        {
            var response = new StudentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                student.StudentID = sqlServerStudent.GetId();
                sqlServerStudent.Insert(student);
                var studentAccount = new StudentAccount()
                {
                    IsActive = true,
                    UserLogin = $"HV{student.StudentID}",
                    PassWordLogin = "123456a",
                    StudentID = student.StudentID,
                };
                sqlServerStudentAccount.Insert(studentAccount);

                response.StudentID = student.StudentID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public StudentResponse Update(Student Student)
        {
            var response = new StudentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                sqlServerStudent.Update(Student);

                response.StudentID = Student.StudentID;
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public StudentResponse Delete(List<long> ids)
        {
            var response = new StudentResponse { Acknowledge = AcknowledgeType.Success };
            try
            {
                if (ids.Count > 0)
                {
                    var checkxoaall = true;
                    foreach (var item in ids)
                    {
                        var checkxoa = sqlServerStudentAccount.Get_StudentAccountByStudentID(item);
                        if (checkxoa == null)
                        {
                            sqlServerStudent.Delete(item);

                        }
                        else
                        {
                            checkxoaall = false;
                        }

                    }
                    if (checkxoaall == false)
                    {
                        throw new Exception("Sinh viên này đã được dùng ở chức năng khác.");
                    }
                }
            }
            catch (Exception ex)
            {
                response.Acknowledge = AcknowledgeType.Failure;
                response.Message = ex.Message;
                return response;
            }
            return response;
        }
        public class StudentResponse : ResponseBase
        {
            public long StudentID { get; set; }
            public string ResponseMessage { get; set; }
        }
    }
}