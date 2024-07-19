using Iot.Device.OneWire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace softub
{
    internal class ThermometerController
    {
        JetController _jetController;
        public ThermometerController(JetController jetController) 
        {
            _jetController = jetController;
        }

        int GetCurrentTempurature()
        {
            if (!_jetController.IsOn())
            {
                _jetController.StartJets(45);
                System.Timers.Timer timer = new System.Timers.Timer(30);
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }
            return 0;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Dictionary<string, double> readTemps = new Dictionary<string, double>();
            foreach (var dev in OneWireThermometerDevice.EnumerateDevices())
            {
                var readTemp = dev.ReadTemperature().DegreesFahrenheit;
                Console.WriteLine($"Thermometer {dev.DeviceId}: {readTemp}");
                readTemps.Add(dev.DeviceId, readTemp);
            }

            //return readTemps.Values.Average();
        }
    }
}
