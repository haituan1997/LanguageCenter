using LanguageCenter.DataLayer;
using LanguageCenter.DataLayer.Shared;
using LanguageCenter.Layer.DataLayer.Object;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace LanguageCenter.Layer.DataLayer.SqlServer
{
    public class SqlServerCourse
    {
        public int Count(string whereClause = null, bool isCreated = true)
        {
            const string procedure = "uspCount_Course";
            object[] parms = { "@WhereClause", whereClause };
            return ForeignLanguageCenterAdapter.GetCount(procedure, parms);
        }
        public IEnumerable<Course> Get_Courses(int page = 0, int pageSize = 15, string orderBy = null, string searchBy = null)
        {
            const string procedure = "uspGetPaged_Course";
            object[] parms = { "@Page", page, "@PageSize", pageSize, "@OrderByColumn", orderBy, "@SearchBy", searchBy };
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakeCourse, parms);
        }
        public IEnumerable<Course> Get_AllCourses()
        {
            const string procedure = "uspGet_AllCourses";
            return ForeignLanguageCenterAdapter.ReadList(procedure, Make);
        }
        public IEnumerable<Course> Get_CoursesAndCountClass()
        {
            const string procedure = "uspGet_CoursesAndCountClass";
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakeCountClass);
        }
        public Course GetCourseByCouresID(long id)
        {
            const string procedure = "uspGet_CourseByCourseID";
            object[] parms = { "@CourseID", id };
            return ForeignLanguageCenterAdapter.Read(procedure, MakeCourse, parms);
        }
        public void InsertCourse(Course course)
        {
            const string procedure = "uspInsert_Course";
            ForeignLanguageCenterAdapter.Insert(procedure, TakeCourse(course)).AsString();
        }
        public void UpdateCourse(Course course)
        {
            const string procedure = "uspUpdate_Course";
            ForeignLanguageCenterAdapter.Update(procedure, TakeCourse(course)).AsString();
        }
        public void DeleteCourse(long id)
        {
            const string procedure = "uspDelete_Course";
            object[] parms = { "@CourseID", id };
            ForeignLanguageCenterAdapter.Update(procedure, parms);
        }
        public IEnumerable<Category> Get_Categories()
        {
            const string procedure = "uspGet_Category";
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakeCategory);
        }
        public void InsertCategory(Category category)
        {
            const string procedure = "uspInsert_Category";
            ForeignLanguageCenterAdapter.Insert(procedure, TakeCategory(category)).AsString();
        }
        public IEnumerable<Language> Get_Languages()
        {
            const string procedure = "uspGet_Languages";
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakeLanguage);
        }
        public void InsertLanguage(Language language)
        {
            const string procedure = "uspInsert_Language";
            ForeignLanguageCenterAdapter.Insert(procedure, TakeLanguage(language)).AsString();
        }
        public IEnumerable<Level> Get_Levels()
        {
            const string procedure = "uspGet_Levels";
            return ForeignLanguageCenterAdapter.ReadList(procedure, MakeLevel);
        }
        public void InsertLevel(Level level)
        {
            const string procedure = "uspInsert_Level";
            ForeignLanguageCenterAdapter.Insert(procedure, TakeLevel(level)).AsString();
        }
        private static readonly Func<IDataReader, Course> Make = reader =>
       new Course
       {
           CourseID = reader["CourseID"].AsLong(),
           Code = reader["Code"].AsString(),
           Name = reader["Name"].AsString(),
           Description = reader["Description"].AsString(),
           LanguageID = reader["LanguageID"].AsLong(),
           LevelID = reader["LevelID"].AsLong(),
           CategoryID = reader["CategoryID"].AsLong(),

       };
        private static readonly Func<IDataReader, Course> MakeCountClass = reader =>
      new Course
      {
          CourseID = reader["CourseID"].AsLong(),
          Code = reader["Code"].AsString(),
          Name = reader["Name"].AsString(),
          Description = reader["Description"].AsString(),
          LanguageID = reader["LanguageID"].AsLong(),
          LevelID = reader["LevelID"].AsLong(),
          CategoryID = reader["CategoryID"].AsLong(),
          CountClass = reader["CountClass"].AsInt(),

      };
        private static readonly Func<IDataReader, Course> MakeCourse = reader =>
       new Course
       {
           CourseID = reader["CourseID"].AsLong(),
           Code = reader["Code"].AsString(),
           Name = reader["Name"].AsString(),
           Description = reader["Description"].AsString(),
           LanguageID = reader["LanguageID"].AsLong(),
           LevelID = reader["LevelID"].AsLong(),
           CategoryID = reader["CategoryID"].AsLong(),
           CategoryName = reader["CategoryName"].AsString(),
           LanguageName = reader["LanguageName"].AsString(),
           LevelName = reader["LevelName"].AsString(),

       };
        private static readonly Func<IDataReader, Category> MakeCategory = reader =>
           new Category
           {
               CategoryID = reader["CategoryID"].AsLong(),
               CategoryName = reader["CategoryName"].AsString(),

           };

        private static readonly Func<IDataReader, Language> MakeLanguage = reader =>
           new Language
           {
               LanguageID = reader["LanguageID"].AsLong(),
               LanguageName = reader["LanguageName"].AsString(),

           };
        private static readonly Func<IDataReader, Level> MakeLevel = reader =>
           new Level
           {
               LevelID = reader["LevelID"].AsLong(),
               Name = reader["Name"].AsString(),
               Code = reader["Code"].AsString(),

           };
        private static object[] TakeLevel(Level level)
        {
            return new object[]
            {
                "@LevelID",level.LevelID,
                "@Name",level.Name,
                "@Code",level.Code,
            };
        }
        private static object[] TakeLanguage(Language language)
        {
            return new object[]
            {
            "@LanguageID",language.LanguageID,
            "@LanguageName",language.LanguageName,
            };
        }
        private static object[] TakeCourse(Course course)
        {
            return new object[]
            {
            "@CourseID",course.CourseID,
            "@Code",course.Code,
            "@Name",course.Name,
            "@Description",course.Description,
            "@LanguageID",course.LanguageID,
            "@LevelID",course.LevelID,
            "@CategoryID",course.CategoryID,
            
            };
        }
        private static object[] TakeCategory(Category category)
        {
            return new object[]
            {
            "@CategoryID",category.CategoryID,
            "@CategoryName",category.CategoryName,
            };
        }

    }

}