using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using static Code.Enumerator.Enumerator;

namespace LanguageCenter.Areas.Home
{
    public class BaseController : Controller
    {
        // GET: Home/Base
        public BaseController()
        {
        }
        public string StaffID
        {
            get
            {
                try
                {
                    var staffID = User.Identity.GetUserId();
                    return staffID;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        public long StudentID
        {
            get
            {
                try
                {
                    if (((ClaimsIdentity)User.Identity).FindFirst("TypeUser").Value == "1") // học sinh = 1
                    {
                        var studentId = long.Parse(((ClaimsIdentity)User.Identity).FindFirst("UserID").Value);
                        return studentId;
                    }
                    else
                        return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        public long TeacherID
        {
            get
            {
                try
                {
                    if (((ClaimsIdentity)User.Identity).FindFirst("TypeUser").Value == "2") // giáo viên = 2
                    {
                        var teacherId = long.Parse(((ClaimsIdentity)User.Identity).FindFirst("UserID").Value);
                        return teacherId;
                    }
                    else
                        return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }
        public long UserID
        {
            get
            {
                try
                {
                    if (((ClaimsIdentity)User.Identity).FindFirst("TypeUser").Value == "3") // admin = 3
                    {
                        var userId = long.Parse(((ClaimsIdentity)User.Identity).FindFirst("UserID").Value);
                        Functions = new Tuple<int, int>((int)TypeOfPermission.Type1, (int)TypeOfPermission.Type1);
                        return userId;
                    }
                    else
                        return -1;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public byte TypeUser
        {
            get
            {
                try
                {
                    var typeUser = long.Parse(((ClaimsIdentity)User.Identity).FindFirst("TypeUser").Value);
                    return typeUser;
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public Tuple<int, int> Functions // tuple[0]: thêm ; tuple[1]: sửa || type : bit
        {
            set { ViewBag.Function = value; }
        }
    }
}