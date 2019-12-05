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
using ToastNotification.Base;
using ToastNotification.ViewModel;

namespace ToastNotification.View
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

        public NotifierView(NotifierViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        
    }
}
