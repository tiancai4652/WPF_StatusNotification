using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_StatusNotification.Base;
using WPF_StatusNotification.ViewModel;

namespace WPF_StatusNotification.View
{
    /// <summary>
    /// NotifierView.xaml 的交互逻辑
    /// </summary>
    public partial class NotifierView : NotifierViewBase
    {
        public NotifierView()
        {
            InitializeComponent();

        }

        private void notifierViewBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateLayout();

            double right = System.Windows.SystemParameters.WorkArea.Right;//工作区最右边的值
            this.Top = System.Windows.SystemParameters.WorkArea.Height - this.ActualHeight;
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));//NotifyTimeSpan是自己定义的一个int型变量，用来设置动画的持续时间
            animation.From = right;
            animation.To = right - this.ActualWidth;//设定通知从右往左弹出
            this.BeginAnimation(Window.LeftProperty, animation);//设定动画应用于窗体的Left属性
            Task.Factory.StartNew(delegate
            {
                int seconds = 5;//通知持续5s后消失
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(seconds));
                //Invoke到主进程中去执行
                this.Dispatcher.Invoke(new Action(() =>
                {
                    {
                        animation = new DoubleAnimation();
                        animation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                        animation.Completed += (s, a) => { this.Close(); };//动画执行完毕，关闭当前窗体
                        animation.From = right - this.ActualWidth;
                        animation.To = right;//通知从左往右收回
                        this.BeginAnimation(Window.LeftProperty, animation);
                    }
                }));
            });
        }
    }
}
