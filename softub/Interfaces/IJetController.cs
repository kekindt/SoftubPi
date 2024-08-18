using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Interfaces
{
    internal interface IJetController
    {
        bool IsOn();
        void RunJets(int seconds);
        void StartJets();
        void StopJets(bool overRide);
    }
}
