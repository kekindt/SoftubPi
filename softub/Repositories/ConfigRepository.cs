using softub.Interfaces;
using softub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Repositories
{
    internal class ConfigRepository : IConfigRepository
    {
        public string LockObject = "locked";

        public ConfigValue GetConfigValue()
        {
            lock (LockObject)
            {
                using (var db = new ConfigContext())
                {
                    var config = db.ConfigValues.ToList();
                    if (config == null || config.Count == 0)
                    {
                        // Add default row
                        ConfigValue cv = new ConfigValue()
                        {
                            TargetTemp = 100,
                            FailSafeCold = 40,
                            FailSafeHot = 110,
                            JetsOn = 0,
                            LightsOn = 0,
                            LastTemp = 0
                        };
                        db.ConfigValues.Add(cv);
                        db.SaveChanges();
                        return cv;
                    }
                    return config.FirstOrDefault();
                }
            }
        }

        public void SaveValues(ConfigValue updated)
        {
            lock (LockObject)
            {
                using (var db = new ConfigContext())
                {
                    var dbValues = db.ConfigValues.ToList();
                    var dbValue = dbValues.FirstOrDefault();

                    dbValue.LastTemp = updated.LastTemp; // Convert.ToInt32(currentTemp);
                    db.ConfigValues.Update(dbValue);
                    db.SaveChanges();
                }
            }
        }
    }
}
