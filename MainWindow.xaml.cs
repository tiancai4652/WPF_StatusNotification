using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_StatusNotification.Base;
using WPF_StatusNotification.View;
using WPF_StatusNotification.ViewModel;

namespace WPF_StatusNotification
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NotifierViewModel viewModel = new NotifierViewModel();
            viewModel.Header = "ACal校准软件";
            viewModel.Context = "ACal用户您好，您机器上安装了360安全卫士、360杀毒等产品，它有时会误报并阻止软件正常操作，为了不影响您的工作，强烈建议您卸载360软件。 ";
            //相对路径
            viewModel.ImageSourcePath = "../../Resource/A (1).png";
            //绝对路径
            //viewModel.ImageSourcePath = @"D:\GitHub2019\StatusNotification\Resource\A (1).png";
            ShowOptions showOptions = new ShowOptions();
            showOptions.IsAutoClose = false;
            showOptions.AutoCloseShowTimeMS = 2000;
            Notifier.ShowView (new NotifierView(viewModel), showOptions);
        }
    }
}
