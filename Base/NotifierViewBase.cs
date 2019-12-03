using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace WPF_StatusNotification.Base
{
    public class NotifierViewBase : WindowBindingBase
    {

        public bool IsEnabledSounds => true;

        double _RightOri = System.Windows.SystemParameters.WorkArea.Right;
        public double RightOri
        {
            get
            {
                return _RightOri;
            }
            set
            {
                _RightOri = value;
                OnPropertyChanged(nameof(RightOri));
            }
        }

        double _RightFinal = System.Windows.SystemParameters.WorkArea.Right;
        public double RightFinal
        {
            get
            {
                return _RightFinal;
            }
            set
            {
                _RightFinal = value;
                OnPropertyChanged(nameof(RightFinal));
            }
        }

        public static void Show(NotifierViewBase notifier)
        {
            if (notifier == null)
            {
                return;
            }
            if (notifier.IsEnabledSounds)
            {
                SystemSounds.Asterisk.Play();
            }
            //var screenHeight= Screen.PrimaryScreen.Bounds.Height;
            //var screenWorkHeight = System.Windows.SystemParameters.WorkArea.Height;
            //var taskHeight = screenHeight - screenWorkHeight;
            //notifier.Top = screenHeight - (taskHeight + notifier.ActualHeight);

            //notifier.UpdateLayout();
            //notifier.Top = System.Windows.SystemParameters.WorkArea.Height - notifier.ActualHeight;
            //notifier.RightFinal = System.Windows.SystemParameters.WorkArea.Right - notifier.ActualWidth;
            notifier.Show();
        }
    }
}
