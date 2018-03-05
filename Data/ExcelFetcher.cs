using POCQL;
using POCQL.DAO;
using POCQL.DAO.ConnectionObject;
using POCQL.Model.MapAttribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPF.Common.Library.Model;

namespace WPF.Common.Library.Data
{
    public class ExcelFetcher : PropertyBinder<ExcelFetcher>
    {
        public string FilePath { get { return _PropBind.GetValue<string>(); } }

        /// <summary>
        /// DAO Factory
        /// </summary>
        private static Factory DaoFactory
        {
            get
            {
                if (!File.Exists(_Entity.FilePath)) throw new FileNotFoundException();

                return new OledbFactory
                {
                    DataSource = _Entity.FilePath,
                    Provider = "Microsoft.ACE.OLEDB.12.0",
                    ExtendedProperties = @"Excel 12.0;HDR=Yes;IMEX=1"
                };
            }
        }

        /// <summary>
        /// Data Access Object
        /// </summary>
        private static DataAccess Access { get { return new POCQL.DAO.DataAccess(DaoFactory); } }

        /// <summary>
        /// Fetch Excel Sheet of Datas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> FetchSheetDatas<T>()
            where T : class, IExcelSheet
        {
            Type type = typeof(T);
            string table = type.GetCustomAttribute<EntityMapperAttribute>().MainTable;
            return Select.Columns<T>()
                     .From(table, true)
                     .Where(type.FilterCondition())
                     .Query<T>(Access);
        }
    }
}
