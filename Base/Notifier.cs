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

        public static void ShowInfo(string header,string content,bool isAutoClose=true,int showTimeIfAutoCloseMS=5000)
        {
            NotifierViewModel viewModel = new NotifierViewModel()
            {
                Header = header,
                Context = content,
                ImageSource = new BitmapImage(new Uri("/WPF_StatusNotification;component/Resource/info.png", UriKind.Relative))
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
    }
}
