using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF_StatusNotification.Base;

namespace WPF_StatusNotification.ViewModel
{
    public class NotifierViewModel : NotifierViewBase
    {
        String _Header="Header";
        public String Header
        {
            get { return _Header; }
            set { _Header = value; OnPropertyChanged(nameof(Header)); }
        }

        String _Context="Context";
        public String Context
        {
            get { return _Context; }
            set { _Context = value; OnPropertyChanged(nameof(Context)); }
        }

        public bool IsSoundEnabled => true;


        //double _Height = 150;
        //public double Height
        //{
        //    get { return _Height; }
        //    set { _Height = value; OnPropertyChanged(nameof(Height)); }
        //}

        //double _Width = 300;
        //public double Width
        //{
        //    get { return _Width; }
        //    set { _Width = value; OnPropertyChanged(nameof(Width)); }
        //}

        public NotifierViewModel()
        {
            Width = 450;
            Height = 150;
        }
    }
}
