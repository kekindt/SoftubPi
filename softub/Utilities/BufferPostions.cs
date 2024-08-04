using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Utilities
{
    internal enum BufferPosition
    {
        Header,
        LEDS,
        Temp1,
        Temp2,
        Temp3,
        Checksum,
        Trailer
    }
}
