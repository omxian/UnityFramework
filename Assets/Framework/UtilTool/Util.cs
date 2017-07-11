using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unity.Framework
{
    public static class Util
    {
        public static double GetUTCTimestamp()
        {
            return (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}
