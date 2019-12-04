using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Media.Imaging;
using WPF_StatusNotification.View;
using WPF_StatusNotification.ViewModel;

namespace WPF_StatusNotification.Base
{
    public class Notifier:NotifierViewBase
    {
        static string infoURI = "/WPF_StatusNotification;component/Resource/info.png";
        static string errorURI = "/WPF_StatusNotification;component/Resource/error.png";
        static string successURI = "/WPF_StatusNotification;component/Resource/success.png";
        static string warnURI = "/WPF_StatusNotification;component/Resource/warn.png";


        public static void ShowView(NotifierViewBase view, ShowOptions options = null)
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

            view.Loaded += OverrideLoaded;
            if (!view.Options.IsAutoClose)
            {
                view.Closing += OverrideClosing;
            }

            view.Show();
        }

        internal static void Show(string header,string content,string ImageUri, bool isAutoClose=true,int showTimeIfAutoCloseMS=5000)
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
                ShowTimeMS = showTimeIfAutoCloseMS
            };

            var view = new NotifierView(viewModel);
            view.SizeToContent = System.Windows.SizeToContent.WidthAndHeight;
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


    }
}
