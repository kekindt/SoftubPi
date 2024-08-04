using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using softub.Interfaces;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Controllers
{
    internal class LightController : BackgroundService
    {
        ILogger<LightController> _logger;
        IConfigRepository _configRepository;
        IPinController _pinController;
        protected int _pinNumber = 18;

        public LightController(ILogger<LightController> logger, IConfigRepository configRepository, IPinController pinController)
        {
            _logger = logger;
            _configRepository = configRepository;
            _pinController = pinController;
        }

        public bool IsOn()
        {
            return _pinController.IsOn(_pinNumber, true);
        }

        public void TurnOn()
        {
            _pinController.TurnPinOn(_pinNumber, true);
        }

        public void TurnOff()
        {
            _pinController.TurnPinOff(_pinNumber, true);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var configValue = _configRepository.GetConfigValue();

                if(configValue.LightsOn == 1 && !IsOn())
                {
                    _logger.LogInformation("Turn on light");
                    TurnOn();
                }
                if(configValue.LightsOn == 0 && IsOn())
                {
                    _logger.LogInformation("Turn off light");
                    TurnOff();
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
