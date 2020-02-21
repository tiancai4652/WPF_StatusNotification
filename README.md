# ToastNotification
Windows更新之路的特色之一就是消息提示由气泡变成了通知窗口，效果简直不要太好。最近公司有这方面的需求，需要在xp，win7系统上给出提示，由此做了一个仿win10的ToastNotification，给出代码供大家参考：


## 效果图：

![image](https://github.com/tiancai4652/ReadME_Images/blob/master/effect.gif)




开源库中也有很多的ToastNotification实现，但是我发现几乎都是基于一个应用的:当你打开一个应用，弹出的界面会向上堆叠，但是当你打开多个应用程序，弹出的界面会重叠在一起。后来通过共享内存文件的方式使他们互相通知我已经画到哪里了(主要是没用过进程间通讯，学习一下，哈哈)



### 支持

·WPF

·WINFORM

·控制台程序

·显示界面可自定义

·自由控制动画

## 原理:

原理非常简单：

消息界面是wpf窗口，在窗口的Loaded和Closing事件添加滑入滑出动画：

```
 view.Loaded += OverrideLoaded;
            view.ShowInTaskbar = false;
            if (!view.Options.IsAutoClose)
            {
                view.Closing += OverrideClosing;
            }
```

Loaded函数中的动画

```
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
```

Closing

```
    DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = notifier.Options.AnamitionDurationTime;
            animation.Completed += (s, a) => { notifier.Closing -= OverrideClosing;};
            animation.From = notifier.Options.RightTo;
            animation.To = notifier.Options.RightFrom;
            notifier.BeginAnimation(Window.LeftProperty, animation);
```

**需要注意的是在控制台程序中，由于没有消息循环，你需要将窗口Showdialog（）**

**（大约是这样，类似于控制台程序的App.Run(window)没有消息循环而无法通知）**



### 用法：

封装了几个接口,可以直接用，效果如效果图:

```
private void ButtonWarn_Click(object sender, RoutedEventArgs e)
        {
            Notifier.ShowWarn(Header.Text, GetString(), IsAutoClose.IsChecked == null ? false : (bool)IsAutoClose.IsChecked, int.Parse(Time.Text));
        }

        private void ButtonSuccess_Click(object sender, RoutedEventArgs e)
        {
            Notifier.ShowSuccess(Header.Text, GetString(), IsAutoClose.IsChecked == null ? false : (bool)IsAutoClose.IsChecked, int.Parse(Time.Text));
        }

        private void ButtonError_Click(object sender, RoutedEventArgs e)
        {
            Notifier.ShowError(Header.Text, GetString(), IsAutoClose.IsChecked == null ? false : (bool)IsAutoClose.IsChecked, int.Parse(Time.Text));
        }
```



自定义窗口只需要继承NotifierViewBase，设计好样式就好了，然后调用Notifier.ShowView（NotifierViewBase view, ShowOptions options）

```
<baseView:NotifierViewBase x:Class="ToastNotification_Demo.CuttomView"
        xmlns:baseView="clr-namespace:ToastNotification.Base;assembly=ToastNotification"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:ToastNotification_Demo"
       
        mc:Ignorable="d"
        Height="150" Width="500" Topmost="True" AllowsTransparency="True" Background="Transparent" WindowStyle="None">
    <Grid>
        <Label FontSize="72">Custom View</Label>
    </Grid>
</baseView:NotifierViewBase>
```

介绍一下ShowOptions 类，里面主要是动画效果的属性，可以ShowView的时候传进去：

```
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
```

Github code+Sample地址：

https://github.com/tiancai4652/WPF_StatusNotification
