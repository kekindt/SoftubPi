using softub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Interfaces
{
    internal interface IConfigRepository
    {
        ConfigValue GetConfigValue();
        void SaveValues(ConfigValue updated);
    }
}
