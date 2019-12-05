using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ToastNotification.Base
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
        public int AutoCloseShowTimeMS { get; set; } = 5000;
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
        /// <summary>
        /// 是否是固定大小(W:300 h:150),否则为自适应大小
        /// </summary>
        public bool IsFixedSizeOrContentToWH { get; set; } = true;
        /// <summary>
        /// FixedSize时的高度，默认150
        /// </summary>
        public Double Height { get; set; } = 150;
        /// <summary>
        /// FixedSize时的宽度，默认350
        /// </summary>
        public Double Width { get; set; } = 350;
    }
}
