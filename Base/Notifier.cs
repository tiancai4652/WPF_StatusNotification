using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Media.Imaging;
using ToastNotification.View;
using ToastNotification.ViewModel;
using System.Windows;

namespace ToastNotification.Base
{
    public class Notifier : NotifierViewBase
    {
        internal static List<NotifierViewBase> ListCreatedNotifier = new List<NotifierViewBase>();

        static string infoURI = "/ToastNotification;component/Resource/info.png";
        static string errorURI = "/ToastNotification;component/Resource/error.png";
        static string successURI = "/ToastNotification;component/Resource/success.png";
        static string warnURI = "/ToastNotification;component/Resource/warn.png";


        public static void ShowView(NotifierViewBase view, ShowOptions options = null, bool showDialog = false)
        {
            if (view == null)
            {
                return;
            }
            if (options != null)
            {
                view.Options = options;
            }
            if (view.Options.IsEnabledSounds)
            {
                SystemSounds.Asterisk.Play();
            }
            if (view.Options.IsFixedSizeOrContentToWH)
            {
                view.SizeToContent = SizeToContent.Manual;
                view.Width = view.Options.Width;
                view.Height = view.Options.Height;
            }
            else
            {
                view.SizeToContent = SizeToContent.WidthAndHeight;
            }
            view.Loaded += OverrideLoaded;
            view.ShowInTaskbar = false;
            if (!view.Options.IsAutoClose)
            {
                view.Closing += OverrideClosing;
            }
            if (showDialog)
            {
                view.ShowDialog();
            }
            else
            {
                view.Show();
            }
        }

        internal static void Show(string header, string content, string ImageUri, bool isAutoClose = true, int showTimeIfAutoCloseMS = 5000)
        {
            NotifierViewModel viewModel = new NotifierViewModel()
            {
                Header = header,
                Context = content,
                ImageSource = new BitmapImage(new Uri(ImageUri, UriKind.Relative))
            };
            ShowOptions showOptions = new ShowOptions()
            {
                IsAutoClose = isAutoClose,
                AutoCloseShowTimeMS = showTimeIfAutoCloseMS
            };

            var view = new NotifierView(viewModel);
            Notifier.ShowView(view, showOptions);
        }

        public static void ShowInfo(string header, string content, bool isAutoClose = true, int showTimeIfAutoCloseMS = 5000)
        {
            Show(header, content, infoURI, isAutoClose, showTimeIfAutoCloseMS);
        }

        public static void ShowError(string header, string content, bool isAutoClose = true, int showTimeIfAutoCloseMS = 5000)
        {
            Show(header, content, errorURI, isAutoClose, showTimeIfAutoCloseMS);
        }

        public static void ShowSuccess(string header, string content, bool isAutoClose = true, int showTimeIfAutoCloseMS = 5000)
        {
            Show(header, content, successURI, isAutoClose, showTimeIfAutoCloseMS);
        }

        public static void ShowWarn(string header, string content, bool isAutoClose = true, int showTimeIfAutoCloseMS = 5000)
        {
            Show(header, content, warnURI, isAutoClose, showTimeIfAutoCloseMS);
        }

        /// <summary>
        /// Bug:多线程打开的view不能被发现
        /// </summary>
        public static void CloseAllNotifier()
        {
            Window[] childArray = Application.Current.Windows.Cast<Window>().ToArray();
            for (int i = childArray.Length; i-- > 0;)
            {
                Window item = childArray[i];
                if (item is NotifierViewBase)
                {
                    item.Close();
                }
            }
        }

        public static void CloseThisNotifier()
        {
            foreach (var item in ListCreatedNotifier)
            {
                try
                {
                    item.Dispatcher.VerifyAccess();
                    item.Close();
                }
                catch(Exception ex)
                {
                    item.Dispatcher.Invoke(new Action(() => { item.Close(); }));
                }
            }

        }
    }
}
