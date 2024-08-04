using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Interfaces
{
    internal interface IPanelController
    {
        void TurnOnHeatLight();
        void TurnOffHeatLight();
        void TurnOnFilterLight();
        void TurnOffFilterLight();
    }
}
