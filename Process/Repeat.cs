using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WPF.Common.Library.Process
{
    /// <summary>
    /// 重複嘗試者
    /// </summary>
    public class Repeater
    {
        /// <summary>
        /// 重複嘗試的Timer
        /// </summary>
        private Timer _Timer;
        /// <summary>
        /// 重複嘗試最高次數
        /// </summary>
        private int MaxRepeatTimes = 0;
        /// <summary>
        /// 已經嘗試次數
        /// </summary>
        private int RepeatTime = 0;

        /// <summary>
        /// 是否已經達到最高次數
        /// </summary>
        public bool IsTimeUp => RepeatTime >= MaxRepeatTimes;
        /// <summary>
        /// Timer是否已經開始
        /// </summary>
        public bool IsStarted => _Timer?.Enabled ?? false;

        public Repeater(double interval, int maxRepeat = 0)
        {
            _Timer = new Timer
            {
                Interval = interval
            };

            MaxRepeatTimes = maxRepeat;
        }

        /// <summary>
        /// 重複執行動作
        /// </summary>
        /// <param name="func">要重複執行的動作</param>
        public void Repeat(Func<bool> func)
        {
            RepeatTime = 0;
            _Timer.Elapsed += new ElapsedEventHandler((s, e) =>
            {
                // *** 如果要重複執行的動作已經成功或是達最高上限就要關閉Timer ***
                if (func() || (MaxRepeatTimes != 0 && ++RepeatTime > MaxRepeatTimes)) _Timer.Close();
            });

            _Timer.Start();
        }
    }
}
