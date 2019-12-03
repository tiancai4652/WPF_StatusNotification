﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
            this.DataContext = this;
        }

        private void notifierViewBase_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateLayout();
            this.Top = System.Windows.SystemParameters.WorkArea.Height - this.ActualHeight;
            this.RightFinal = System.Windows.SystemParameters.WorkArea.Right - this.ActualWidth;
        }
    }
}
