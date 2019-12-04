using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF_StatusNotification.Base;

namespace WPF_StatusNotification.ViewModel
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
    }
}
