using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ToastNotification.Windows
{
    public class WindowInfo
    {
        public static readonly Version OSVersion = Environment.OSVersion.Version;

        public IntPtr Handle { get; }

        public bool IsHandleCreated => Handle != IntPtr.Zero;

        public string Text => NativeMethods.GetWindowText(Handle);

        public string ClassName => NativeMethods.GetClassName(Handle);

        public Rectangle Rectangle => GetWindowRectangle(Handle);

        public Rectangle ClientRectangle => NativeMethods.GetClientRect(Handle);

        public bool IsVisible => NativeMethods.IsWindowVisible(Handle) && !IsCloaked;

        public bool IsCloaked => NativeMethods.IsWindowCloaked(Handle);

        public WindowInfo(IntPtr handle)
        {
            Handle = handle;
        }

        public override string ToString()
        {
            return Text;
        }

        public static Rectangle GetWindowRectangle(IntPtr handle)
        {
            Rectangle rect = Rectangle.Empty;

            if (NativeMethods.IsDWMEnabled())
            {
                Rectangle tempRect;

                if (NativeMethods.GetExtendedFrameBounds(handle, out tempRect))
                {
                    rect = tempRect;
                }
            }

            if (rect.IsEmpty)
            {
                rect = NativeMethods.GetWindowRect(handle);
            }

            if (!IsWindows10OrGreater() && NativeMethods.IsZoomed(handle))
            {
                rect = NativeMethods.MaximizedWindowFix(handle, rect);
            }

            return rect;
        }

        public static bool IsWindows10OrGreater(int build = -1)
        {
            return OSVersion.Major >= 10 && OSVersion.Build >= build;
        }
    }
}
