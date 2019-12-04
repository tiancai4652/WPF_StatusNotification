using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPF_StatusNotification.Base
{
    public class ShowOptions
    {
        public bool IsEnabledSounds { get; set; } = true;
        public bool IsAutoClose { get; set; } = true;
        /// <summary>
        /// 配合IsAutoClose，window显示时间
        /// </summary>
        public int ShowTimeMS { get; set; } = 500;
        public Action BeforeAnimation { get; set; }
        public Action AfterAnimation { get; set; }
        public double NotifierTop { get; set; } = double.NaN;
        public double RightFrom { get; set; } = double.NaN;
        public double RightTo { get; set; } = double.NaN;
        public Duration AnamitionDurationTime { get; set; } = new Duration(TimeSpan.FromMilliseconds(500));
    }
}
