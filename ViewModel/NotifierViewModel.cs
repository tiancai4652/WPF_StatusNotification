using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ToastNotification.Base;

namespace ToastNotification.ViewModel
{
    public class NotifierViewModel : WindowBindingBase
    {
        String _Header = "Header";
        public String Header
        {
            get { return _Header; }
            set { _Header = value; OnPropertyChanged(nameof(Header)); }
        }

        String _Context = "Context";
        public String Context
        {
            get { return _Context; }
            set { _Context = value; OnPropertyChanged(nameof(Context)); }
        }

        double _Opacity = 0.9;
        public double Opacity
        {
            get { return _Opacity; }
            set { _Opacity = value; OnPropertyChanged(nameof(Opacity)); }
        }

        string _ImageSourcePath= "/ToastNotification;component/Resource/empty.png";
        public string ImageSourcePath
        {
            get { return _ImageSourcePath; }
            set
            {
                _ImageSourcePath = value;
                OnPropertyChanged(nameof(ImageSourcePath));
                string path = System.IO.Path.GetFullPath(_ImageSourcePath);
                ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
            }
        }

        ImageSource _ImageSource= new BitmapImage(new Uri("/ToastNotification;component/Resource/empty.png", UriKind.Relative));
        /// <summary>
        /// 请设置ImageSourcePath而不是这个
        /// </summary>
        public ImageSource ImageSource
        {
            get { return _ImageSource; }
            set { _ImageSource = value; OnPropertyChanged(nameof(ImageSource)); }
        }


    }
}
