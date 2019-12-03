
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF_StatusNotification.Base;

namespace WPF_StatusNotification.Model
{
    public class NotifierModel : WindowBindingBase
    {
        string _Header;
        /// <summary>
        /// Header
        /// </summary>
        public string Header
        {
            get { return _Header; }
            set
            {
                _Header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        string _Context;
        /// <summary>
        /// Context
        /// </summary>
        public string Context
        {
            get { return _Context; }
            set
            {
                _Context = value;
                OnPropertyChanged(nameof(Context));
            }
        }

        double _Height;
        /// <summary>
        /// Height
        /// </summary>
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        double _Width;
        /// <summary>
        /// Width
        /// </summary>
        public double Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                OnPropertyChanged(nameof(Width));
            }
        }

    }
}
