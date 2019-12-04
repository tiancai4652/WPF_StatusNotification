using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPF_StatusNotification.Base
{
    public class ShowOptions
    {
        /// <summary>
        /// 是否播放提示音
        /// </summary>
        public bool IsEnabledSounds { get; set; } = true;
        /// <summary>
        /// 是否自动关闭
        /// </summary>
        public bool IsAutoClose { get; set; } = true;
        /// <summary>
        /// 配合IsAutoClose，window显示时间
        /// </summary>
        public int ShowTimeMS { get; set; } = 5000;
        /// <summary>
        /// 窗口显示位置(一般情况下不需要设置)
        /// </summary>
        public double NotifierTop { get; set; } = double.NaN;
        /// <summary>
        /// 窗口显示位置（动画初始位置）(一般情况下不需要设置)
        /// </summary>
        public double RightFrom { get; set; } = double.NaN;
        /// <summary>
        /// 窗口显示位置（动画结束位置）(一般情况下不需要设置)
        /// </summary>
        public double RightTo { get; set; } = double.NaN;
        /// <summary>
        /// 动画执行时间
        /// </summary>
        public Duration AnamitionDurationTime { get; set; } = new Duration(TimeSpan.FromMilliseconds(500));
    }
}
