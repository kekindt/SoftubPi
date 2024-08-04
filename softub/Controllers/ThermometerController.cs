using Iot.Device.OneWire;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using softub.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace softub.Controllers
{
    internal class ThermometerController : BackgroundService, IThermometerController
    {
        ILogger<ThermometerController> _logger;
        IJetController _jetController;
        IConfigRepository _configRepository;

        public ThermometerController(ILogger<ThermometerController> logger, IJetController jetController, IConfigRepository configRepository)
        {
            _logger = logger;
            _configRepository = configRepository;
            _jetController = jetController;
        }

        public int GetCurrentTempurature()
        {
            if (!_jetController.IsOn())
            {
                _jetController.RunJets(30);
            }
            Dictionary<string, double> readTemps = new Dictionary<string, double>();
            foreach (var dev in OneWireThermometerDevice.EnumerateDevices())
            {
                var readTemp = dev.ReadTemperature().DegreesFahrenheit;
                Console.WriteLine($"Thermometer {dev.DeviceId}: {readTemp}");
                readTemps.Add(dev.DeviceId, readTemp);
            }

            return Convert.ToInt32(readTemps.Values.Average());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var configValue = _configRepository.GetConfigValue();

                int temp = GetCurrentTempurature();

                configValue.LastTemp = temp;
                _configRepository.SaveValues(configValue);

                await Task.Delay(30 * 60 * 1000, stoppingToken);
            }
        }
    }
}
