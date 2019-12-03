using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF_StatusNotification.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {

        }

        public NotifierViewModel NotifierVM
        {
            get
            {
                return new NotifierViewModel();
            }
        }
    }
}
