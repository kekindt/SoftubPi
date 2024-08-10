using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using softub.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace softub.Controllers
{
    internal class HeaterController : BackgroundService, IHeaterController
    {
        ILogger<HeaterController> _logger;
        IJetController _jetController;
        IConfigRepository _configRepository;
        IPanelController _panelController;

        public HeaterController(ILogger<HeaterController> logger, IJetController jetController, IConfigRepository configRepository, IPanelController panelController) 
        { 
            _configRepository = configRepository;
            _jetController = jetController;
            _panelController = panelController;
            _logger = logger;
            _logger.LogInformation("Heater Controller Started");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var configValue = _configRepository.GetConfigValue();

                if (configValue.LastTemp >= configValue.FailSafeHot)
                {
                    // Stop reguardless of Jets Status
                    _jetController.StopJets();
                    _logger.LogWarning($"Stopped Jets due to fail safe temp {configValue.FailSafeHot}");
                }

                if (configValue.LastTemp < configValue.TargetTemp)
                {
                    if (!_jetController.IsOn())
                    {
                        _jetController.StartJets();
                        _panelController.TurnOnHeatLight();
                        _logger.LogInformation("Temp not at target, starting jets");
                    }
                }
                if (configValue.LastTemp >= configValue.TargetTemp)
                {
                    if (_jetController.IsOn())
                    {
                        _jetController.StopJets();
                        _panelController.TurnOffFilterLight();
                        _logger.LogInformation("Temp at target, stoping jets");
                    }
                }

                await Task.Delay(10 * 60 * 1000, stoppingToken);
            }
        }
    }
}
