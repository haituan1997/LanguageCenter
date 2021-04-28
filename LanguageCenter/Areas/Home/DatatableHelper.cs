using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using PropertyAttributes = System.Data.PropertyAttributes;

// ReSharper disable PossibleNullReferenceException

namespace LanguageCenter.Code.Helper.DatatableHelper
{

    public class DatatableHelper
    {
        public class DataTableParams
        {
            public int PageIndex { get; set; }
            public int PageSize { get; set; }
            public string OrderBy { get; set; }
            public string SearchBy { get; set; }
            public string OrderColumn { get; set; }
            public bool IsFilter { get; set; }
        }

        /// <summary>
        /// Gets the parameters from request.
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="requestForm">The request form.</param>
        /// <param name="vocationalSchoolId">The vocational school identifier.</param>
        /// <param name="managementUnitId">The management unit identifier.</param>
        /// <returns></returns>
        public static DataTableParams GetParamsFromRequest([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, NameValueCollection requestForm, long? vocationalSchoolId = null, long? managementUnitId = null)
        {
            var datatableParams = new DataTableParams()
            {
                SearchBy = "",
            };

            // Order by
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            var orderByString = string.Empty;
            foreach (var column in sortedColumns)
            {
                if (column.Name.Contains('#'))
                {
                    var customColumnCode = column.Name.Split('#')[1];
                    orderByString += orderByString != string.Empty ? "," : "";
                    orderByString += (customColumnCode) +
                                     (column.SortDirection ==
                                      Column.OrderDirection.Ascendant ? " asc" : " desc");
                    datatableParams.OrderColumn = customColumnCode;
                }
                else
                {
                    orderByString += orderByString != string.Empty ? "," : "";
                    orderByString += (column.Data) +
                                     (column.SortDirection ==
                                      Column.OrderDirection.Ascendant ? " asc" : " desc");
                    datatableParams.OrderColumn = column.Data;
                }

            }
            datatableParams.OrderBy = orderByString;


            // PageSize
            datatableParams.PageSize = requestModel.Length;

            // PageIndex
            var startRow = requestModel.Start;
            var pageIndex = (startRow / requestModel.Length) + 1;
            datatableParams.PageIndex = pageIndex;

            // Search By
            var whereClause = string.Empty;
            string searchWord = null;
            string columnCode = null;
            string columnName = null;
            bool isFilter = false;
            for (var i = 0; i < requestModel.Columns.Count(); i++)
            {
                if (requestForm == null) continue;

                // ReSharper disable once PossibleNullReferenceException
                foreach (var value in requestForm?.GetValues($"columns[{i}][search][value]"))
                {
                    searchWord = value.Trim();
                    break;
                }
                foreach (var value in requestForm.GetValues($"columns[{i}][data]"))
                {
                    columnCode = value;
                    break;
                }
                foreach (var value in requestForm.GetValues($"columns[{i}][name]"))
                {
                    columnName = value;
                    break;
                }

                if (string.IsNullOrEmpty(searchWord.Trim())) continue;

                if (columnName.ToLower().Contains("date"))
                {
                    whereClause += $" and {columnCode} = convert(date, '{searchWord}', 103)";
                    isFilter = true;
                }

                else if (columnName.ToLower().Contains("select"))
                {
                    if (columnName.ToLower().Contains("vocationalschoolbranchspecialselect"))
                    {
                        if (searchWord == "999") continue;
                        isFilter = true;
                        whereClause += $" and {columnCode} = '{searchWord}'"; continue;
                    }
                    if (columnCode.ToLower() == "isactive")
                    {
                        if (searchWord == "999") continue;
                        isFilter = true;
                        whereClause += $" and {columnCode} = '{searchWord}'"; continue;
                    }
                    if (columnCode.Equals("IsBusinessAffiliatedWithSchoolName"))
                    {
                        if (searchWord == "999") continue;
                        isFilter = true;
                        whereClause += $" and {columnCode} = '{searchWord}'"; continue;
                    }

                    if (searchWord == "0") continue;
                    if (columnName.ToLower().Contains("listview"))
                    {
                        columnName = columnName.Replace("ListView", "");
                    }
                    if (columnName.ToLower().Contains("#"))
                    {
                        var customColumnCode = columnName.Split('#')[1];
                        whereClause += $" and {customColumnCode} = '{searchWord}'";
                        isFilter = true;
                        continue;
                    }
                    isFilter = true;
                    whereClause += $" and {columnCode} = '{searchWord}'";
                }

                else
                {
                    whereClause += $" and {columnCode} like N'%{searchWord}%'";
                    isFilter = true;
                }
            }

            datatableParams.IsFilter = isFilter;

            if (!string.IsNullOrEmpty(whereClause))
            {
                datatableParams.SearchBy = whereClause.Trim();
            }

            if (vocationalSchoolId != null && managementUnitId == null)
            {
                datatableParams.SearchBy += $" AND VocationalSchoolID = {vocationalSchoolId} ";
            }

            if (managementUnitId != null && vocationalSchoolId == null)
            {
                datatableParams.SearchBy += $" AND ManagementUnitID = {managementUnitId} ";
            }

            // Return params
            return datatableParams;
        }

        /// <summary>
        /// Gets the header filter.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetHeaderFilter()
        {
            var whereClause = string.Empty;

            return whereClause;
        }

        public static DataTable ToPivotTable<T, TColumn, TRow, TData>(
           IEnumerable<T> source,
           Func<T, TColumn> columnSelector,
           Expression<Func<T, TRow>> rowSelector,
           Func<IEnumerable<T>, TData> dataSelector)
        {
            DataTable table = new DataTable();
            var rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            table.Columns.Add(new DataColumn(rowName));
            var columns = source.Select(columnSelector).Distinct();

            foreach (var column in columns)
                table.Columns.Add(new DataColumn(column.ToString()));

            var rows = source.GroupBy(rowSelector.Compile())
                .Select(rowGroup => new
                {
                    Key = rowGroup.Key,
                    Values = columns.GroupJoin(
                        rowGroup,
                        c => c,
                        r => columnSelector(r),
                        (c, columnGroup) => dataSelector(columnGroup))
                });

            foreach (var row in rows)
            {
                var dataRow = table.NewRow();
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                dataRow.ItemArray = items.ToArray();
                table.Rows.Add(dataRow);
            }

            return table;
        }

        public static class MyTypeBuilder
        {
            public static System.Collections.IList AddListToTree(IList list)
            {
                var orderedList = list.Cast<Type>().OrderBy(x => x);
                return orderedList.ToList();
                // 
            }

            public static System.Collections.IList CreateInstanceIList(Type _type)
            {
                Type customList = typeof(List<>).MakeGenericType(_type);
                var result = (System.Collections.IList)Activator.CreateInstance(customList);
                return result;
            }
            public static Type CompileResultType(Dictionary<string, Type> listOfFields)
            {
                TypeBuilder tb = GetTypeBuilder();
                foreach (var field in listOfFields)
                    CreateProperty(tb, field.Key, field.Value);

                Type objectType = tb.CreateType();

                return objectType;
            }

            private static TypeBuilder GetTypeBuilder()
            {
                var typeSignature = "MyDynamicType_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                var an = new AssemblyName(typeSignature);
                AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
                TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                        TypeAttributes.Public |
                        TypeAttributes.Class |
                        TypeAttributes.AutoClass |
                        TypeAttributes.AnsiClass |
                        TypeAttributes.BeforeFieldInit |
                        TypeAttributes.AutoLayout,
                        null);
                return tb;
            }

            private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
            {
                FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

                PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);
                MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                ILGenerator getIl = getPropMthdBldr.GetILGenerator();

                getIl.Emit(OpCodes.Ldarg_0);
                getIl.Emit(OpCodes.Ldfld, fieldBuilder);
                getIl.Emit(OpCodes.Ret);

                MethodBuilder setPropMthdBldr =
                    tb.DefineMethod("set_" + propertyName,
                      MethodAttributes.Public |
                      MethodAttributes.SpecialName |
                      MethodAttributes.HideBySig,
                      null, new[] { propertyType });

                ILGenerator setIl = setPropMthdBldr.GetILGenerator();
                Label modifyProperty = setIl.DefineLabel();
                Label exitSet = setIl.DefineLabel();

                setIl.MarkLabel(modifyProperty);
                setIl.Emit(OpCodes.Ldarg_0);
                setIl.Emit(OpCodes.Ldarg_1);
                setIl.Emit(OpCodes.Stfld, fieldBuilder);

                setIl.Emit(OpCodes.Nop);
                setIl.MarkLabel(exitSet);
                setIl.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getPropMthdBldr);
                propertyBuilder.SetSetMethod(setPropMthdBldr);
            }
        }

    }
}