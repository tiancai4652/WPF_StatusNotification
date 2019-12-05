using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace WPF_StatusNotification.Base
{
    public class NotifierViewBase : Window
    {
        public ShowOptions Options { get; set; } = new ShowOptions();

        MyRectangular myRectangular;

        internal static void OverrideLoaded(object sender, RoutedEventArgs e)
        {
            var notifier = sender as NotifierViewBase;

            notifier.UpdateLayout();
            notifier.myRectangular = new MyRectangular() { Height = notifier.ActualHeight };

            DoubleAnimation animation = new DoubleAnimation();


            var top = System.Windows.SystemParameters.WorkArea.Height - GetStackHeight();
            AddToStack(notifier);

            if (double.IsNaN(notifier.Options.NotifierTop))
            {
                notifier.Top = top - notifier.ActualHeight;
            }
            else
            {
                notifier.Top = notifier.Options.NotifierTop;
            }

            if (double.IsNaN(notifier.Options.RightTo))
                notifier.Options.RightTo = System.Windows.SystemParameters.WorkArea.Right - notifier.ActualWidth;

            if (double.IsNaN(notifier.Options.RightFrom))
                notifier.Options.RightFrom = System.Windows.SystemParameters.WorkArea.Right;

            animation.Duration = notifier.Options.AnamitionDurationTime;
            animation.From = notifier.Options.RightFrom;
            animation.To = notifier.Options.RightTo;

            notifier.BeginAnimation(Window.LeftProperty, animation);


            if (notifier.Options.IsAutoClose)
            {
                Task.Factory.StartNew(delegate
                {
                    int seconds = notifier.Options.AutoCloseShowTimeMS;
                    System.Threading.Thread.Sleep(TimeSpan.FromMilliseconds(seconds));
                    notifier.Dispatcher.Invoke(new Action(() =>
                        {
                            {
                                animation = new DoubleAnimation();
                                animation.Duration = notifier.Options.AnamitionDurationTime;
                                animation.Completed += (s, a) =>
                                {
                                    notifier.CloseFun();
                                };
                                animation.From = notifier.Options.RightTo;
                                animation.To = notifier.Options.RightFrom;
                                notifier.BeginAnimation(Window.LeftProperty, animation);
                            }
                        }));
                });
            }
        }

        internal static void OverrideClosing(object sender, CancelEventArgs e)
        {
            var notifier = sender as NotifierViewBase;
            e.Cancel = true;
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = notifier.Options.AnamitionDurationTime;
            animation.Completed += (s, a) => { notifier.Closing -= OverrideClosing; notifier.CloseFun(); };
            animation.From = notifier.Options.RightTo;
            animation.To = notifier.Options.RightFrom;
            notifier.BeginAnimation(Window.LeftProperty, animation);
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseFun();
        }

        public void CloseFun()
        {
            var id = myRectangular?.ID;
            RemoveFromStack(id);
            Close();
          
        }

        static double GetStackHeight()
        {
            var helper = MemoryMapFileHelper<List<MyRectangular>>.GetHelper();
            var list = helper.Read();
            if (list != null)
            {
                double topExisted = list.Select(t => t.Height).Sum() + list.Select(t => t.Space).Sum();
                return topExisted;
            }
            return 0;
        }

        static void AddToStack(NotifierViewBase notifier)
        {
            var helper = MemoryMapFileHelper<List<MyRectangular>>.GetHelper();
            var list = helper.Read();
            if (list == null)
            {
                list = new List<MyRectangular>();
            }
            list.Add(notifier.myRectangular);
            helper.Write(list);
        }

        static void RemoveFromStack(string id)
        {
            var helper = MemoryMapFileHelper<List<MyRectangular>>.GetHelper();
            var list = helper.Read();
            if (list != null)
            {
                var rec = list.FirstOrDefault(t => t.ID.Equals(id));
                if (rec != null)
                {
                    rec.IsEmpty = true;
                    if (list.All(t => t.IsEmpty))
                    {
                        list.Clear();
                    }
                    helper.Write(list);
                }
            }
        }
    }
}
