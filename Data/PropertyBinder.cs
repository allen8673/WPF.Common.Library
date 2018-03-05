using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common.Library.Data
{
    public abstract class PropertyBinder<TDerive>
    {
        protected static BindingHash _PropBind = new BindingHash();
        protected static TDerive _Entity = Activator.CreateInstance<TDerive>();

        /// <summary>
        /// Set Binding Property
        /// </summary>
        /// <param name="source">Binding Source</param>
        /// <param name="direct">Direct</param>
        public static void SetBinding(Func<object> source, Expression<Func<TDerive, object>> direct)
        {
            // *** Get the Direct property name ***
            string propName = GetPropertyName(direct);
            _PropBind.Add(propName, source);
        }

        /// <summary>
        /// Set Binding Property
        /// </summary>
        /// <param name="batchSettings"></param>
        public static void SetBinding(BindingSettings<TDerive> batchSettings)
        {
            foreach (var setting in batchSettings)
            {
                SetBinding(setting.Key, setting.Value);
            }
        }

        /// <summary>
        /// Get the Name of Lambda Expression assignee
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">Lambda Expression</param>
        /// <returns></returns>
        private static string GetPropertyName(Expression<Func<TDerive, object>> exp)
        {
            // *** 解析Lambda Expression，並取得內容資訊 ***
            MemberExpression member = exp.Body as MemberExpression;

            if (member == null)
            {
                UnaryExpression unary = exp.Body as UnaryExpression;
                member = unary.Operand as MemberExpression;
            }

            // *** 取得Property Name ***
            string propName = member.Member.Name;

            return propName;
        }
    }

    /// <summary>
    /// Data binding Hash
    /// </summary>
    public class BindingHash : Dictionary<string, Func<object>>
    {
        public T GetValue<T>([CallerMemberName]string caller = "")
        {
            return this.ContainsKey(caller) ? (T)this[caller]() : default(T);
        }
    }

    public class BindingSettings<T> : Dictionary<Func<object>, Expression<Func<T, object>>> { }

}
