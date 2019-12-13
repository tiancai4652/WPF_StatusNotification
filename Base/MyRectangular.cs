using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToastNotification.Base
{
    [Serializable]
    public class MyRectangular
    {
        public int Handle { get; set; }

        public double Height { get; set; }

        public double Space { get; set; } = 5;

        public string ID { get; set; } = Guid.NewGuid().ToString("N");

        public bool IsEmpty { get; set; } = false;
    }
}
