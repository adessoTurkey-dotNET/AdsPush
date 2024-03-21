using System;
using System.Collections.Generic;
using System.Text;

namespace AdsPush.Abstraction.HMS.Android
{
    public class LigthSettings
    {
        public Color Color { get; set; }

        /// <summary>
        /// Interval when a breathing light is on
        /// </summary>
        public string LightOnDuration { get; set; }

        /// <summary>
        /// Interval when a breathing light is off
        /// </summary>
        public string LightOffDuration { get; set; }
    }
}
