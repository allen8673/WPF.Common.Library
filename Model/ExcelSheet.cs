using POCQL.Model.MapAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common.Library.Model
{
    public interface IExcelSheet { }

    /// <summary>
    /// IExcelSheet Extend Class
    /// </summary>
    static class ExcelSheetExtend
    {
        /// <summary>
        /// Fetch Condition
        /// </summary>
        /// <param name="type">Type of class that implemented IExcelSheet </param>
        /// <returns></returns>
        public static string FilterCondition(this Type type)
        {
            return $"({string.Join(" OR ", type.RequiredColumns().Select(c => $"{c} <> ''"))})";
        }

        /// <summary>
        /// Get The Columns of Required Valeu at least one
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<string> RequiredColumns(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IExcelSheet)) ?
                   type.GetProperties()
                       .Where(p => Attribute.IsDefined(p, typeof(PrimaryKeyAttribute)))
                       .Select(p => p.GetCustomAttribute<ColumnMapperAttribute>()?.Column ?? "")
                       .Where(i => !string.IsNullOrEmpty(i)) :
                   new string[] { };
        }
    }
}
