using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace WPF_StatusNotification.Base
{
    public class NotifierViewBase : Window
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
        Duration AnamitionDurationTime { get; set; } = new Duration(TimeSpan.FromMilliseconds(500));

        public static void Show(NotifierViewBase notifier)
        {
            if (notifier == null)
            {
                return;
            }
            if (notifier.IsEnabledSounds)
            {
                SystemSounds.Asterisk.Play();
            }

            notifier.Loaded += OverrideLoaded;

            notifier.Show();
        }

        static void OverrideLoaded(object sender, RoutedEventArgs e)
        {
            var notifier = sender as NotifierViewBase;

            notifier.UpdateLayout();

            if (notifier.BeforeAnimation != null)
            {
                notifier.BeforeAnimation.Invoke();
            }

            DoubleAnimation animation = new DoubleAnimation();
            if (double.IsNaN(notifier.NotifierTop))
            {
                notifier.Top = System.Windows.SystemParameters.WorkArea.Height - notifier.ActualHeight;
            }
            else
            {
                notifier.Top = notifier.NotifierTop;
            }

            if (double.IsNaN(notifier.RightTo))
                notifier.RightTo = System.Windows.SystemParameters.WorkArea.Right - notifier.ActualWidth;

            if (double.IsNaN(notifier.RightFrom))
                notifier.RightFrom = System.Windows.SystemParameters.WorkArea.Right;

            animation.Duration = notifier.AnamitionDurationTime;
            animation.From = notifier.RightFrom;
            animation.To = notifier.RightTo;

            notifier.BeginAnimation(Window.LeftProperty, animation);


            if (notifier.IsAutoClose)
            {
                Task.Factory.StartNew(delegate
                {
                    int seconds = 5;//通知持续5s后消失
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                //Invoke到主进程中去执行
                notifier.Dispatcher.Invoke(new Action(() =>
                    {
                        {
                            animation = new DoubleAnimation();
                            animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                            animation.Completed += (s, a) => { notifier.Close(); };
                            animation.From = notifier.RightTo;
                            animation.To = notifier.RightFrom;
                            notifier.BeginAnimation(Window.LeftProperty, animation);
                        }
                    }));
                });
            }
        }
    }
}
