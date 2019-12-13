using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ToastNotification.Windows
{
    public class WindowsList
    {
        public List<IntPtr> IgnoreWindows { get; set; }
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        private string[] ignoreList = new string[] { "Progman", "Button" };
        private List<WindowInfo> windows;

        public WindowsList()
        {
            IgnoreWindows = new List<IntPtr>();
        }

        public WindowsList(IntPtr ignoreWindow) : this()
        {
            IgnoreWindows.Add(ignoreWindow);
        }

        public List<WindowInfo> GetWindowsList()
        {
            windows = new List<WindowInfo>();
            EnumWindowsProc ewp = EvalWindows;
            EnumWindows(ewp, IntPtr.Zero);
            return windows;
        }

        public static bool IsWindowExisted(int windowHandle)
        {
            return new WindowsList().GetWindowsList().Exists(t => t.Handle.ToInt32().Equals(windowHandle));
        }

        private bool EvalWindows(IntPtr hWnd, IntPtr lParam)
        {
            if (IgnoreWindows.Any(window => hWnd == window))
            {
                return true;
            }

            windows.Add(new WindowInfo(hWnd));

            return true;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
    }
}
