using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using softub.Interfaces;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Timers;

namespace softub.Controllers
{
    internal class JetController : BackgroundService, IJetController // RelayController
    {
        ILogger<JetController> _logger;
        IConfigRepository _configRepository;
        IPinController _pinController;
        int _pinNumber = 23;
        static bool runJets = false;

        public JetController(ILogger<JetController> logger, IConfigRepository configRepository, IPinController pinController) 
        {
            _logger = logger;
            _configRepository = configRepository;
            _pinController = pinController;
            _logger.LogInformation("Jet Controller Started");
        }

        public void StartJets()
        {
            runJets = true;
            _pinController.TurnPinOn(_pinNumber, true);
        }

        public void StopJets()
        {
            runJets = false;
            _pinController.TurnPinOff(_pinNumber, true);
        }

        public bool IsOn()
        {
            return _pinController.IsOn(_pinNumber, true);
        }

        public void RunJets(int secondsToRun)
        {
            runJets = true;
            StartJets();
            Thread.Sleep(secondsToRun * 1000);
            StopJets();
            runJets = false;  
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var configValue = _configRepository.GetConfigValue();
                if (!runJets)
                {
                    if (configValue.JetsOn == 1 && !IsOn())
                    {
                        _logger.LogInformation("Jets turned on for 20 minutes");
                        RunJets(20 * 60);
                    }
                }

                await Task.Delay(2000, stoppingToken);
            }
            
        }
    }
}
