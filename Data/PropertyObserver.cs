using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Common.Library.Data
{
    /// <summary>
    /// 觀察者: Property Change
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public abstract class PropertyObserver<TModel> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private PropertyValueHash<TModel> ValueHash = new PropertyValueHash<TModel>();

        /// <summary>
        /// 觀察者Value Setter
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="newValue">value</param>
        /// <param name="callerName">caller name</param>
        /// <returns></returns>
        public bool SetValue<TValue>(TValue newValue, [CallerMemberName] string callerName = "")
        {
            if (!EqualityComparer<TValue>.Default.Equals((TValue)ValueHash[callerName], newValue))
            {
                ValueHash[callerName] = newValue;
                this.PropertyOnChange(callerName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 觀察者Value Getter
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="callerName">caller name</param>
        /// <returns></returns>
        public TValue GetValue<TValue>([CallerMemberName] string callerName = "")
        {
            return (TValue)ValueHash[callerName];
        }

        /// <summary>
        /// 觀察者Property Value Change trigger
        /// </summary>
        /// <param name="propertyName"></param>
        private void PropertyOnChange(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }

    class PropertyValueHash<TModel> : Dictionary<string, object>
    {
        public new object this[string propName]
        {
            get
            {
                CheckContainKey(propName);
                return base[propName];
            }

            set
            {
                CheckContainKey(propName);
                base[propName] = value;
            }
        }

        private void CheckContainKey(string propName)
        {
            if (!this.ContainsKey(propName))
            {
                Type propType = typeof(TModel).GetProperty(propName).PropertyType;
                this.Add(propName, propType.IsValueType ? Activator.CreateInstance(propType) : null);
            }

        }
    }
}
