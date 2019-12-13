using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ToastNotification.Windows
{
    public class WindowInfo
    {
        public IntPtr Handle { get; }

        public WindowInfo(IntPtr handle)
        {
            Handle = handle;
        }
    }
}
