using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using softub.Controllers;
using softub.Models;

namespace softub
{
    internal class ProcessController : BackgroundService
    {
        int LED_PIN = 18;
        int RELAY_PIN = 23;
        bool RUNNING = true;

        GpioController gpioController;
        public ProcessController() 
        {
            gpioController = new GpioController();

            gpioController.OpenPin(LED_PIN, PinMode.Output);
            gpioController.OpenPin(RELAY_PIN, PinMode.Output);
        }

        public void Startup() 
        {
            AppDomain.CurrentDomain.ProcessExit += Shutdown;
            RUNNING = true;
        }

        public void Run()
        {
            Task LightTask = Task.Run(() => { ProcessLights(); });
            Task JetTask = Task.Run(() => { ProcessJets(); });
            Task HeaterTask = Task.Run(() => { ProcessHeater(); });

            while (RUNNING)
            {
                LightTask = LightTask.ContinueWith(t => { Thread.Sleep(2000); Task.Run(() => { ProcessLights(); }); });
                JetTask = JetTask.ContinueWith(t => { Thread.Sleep(2000); Task.Run(() => { ProcessJets(); }); });
                HeaterTask = HeaterTask.ContinueWith(t => { Thread.Sleep(2000); Task.Run(() => { ProcessHeater(); }); });
            }

        }

        public void ProcessLights() 
        {
            //LightController lc = new LightController(gpioController, LED_PIN);
            //var config = GetConfigValue();
            //if(config.LightsOn == 1 && !lc.IsOn())
            //{
            //    lc.TurnOn();
            //}
            //if(config.LightsOn == 0 && lc.IsOn())
            //{
            //    lc.TurnOff();
            //}
        }

        public void ProcessJets()
        {
            //JetController jc = new JetController(gpioController, RELAY_PIN);
            //var config = GetConfigValue();
            //if(config.JetsOn == 1 && !jc.IsOn())
            //{
            //    jc.StartJets();
            //}
            //if (config.JetsOn == 0 && jc.IsOn())
            //{
            //    jc.StopJets();
            //}
        }

        public void ProcessHeater()
        {
            //JetController jc = new JetController(gpioController, RELAY_PIN);
            //ThermometerController tc = new ThermometerController(jc);
            //var config = GetConfigValue();

            //int currentTemp = tc.GetCurrentTempurature();
            //config.LastTemp = currentTemp;
            //SaveValues(config);

            //if(currentTemp < config.TargetTemp)
            //{
            //    if (config.JetsOn == 0)
            //    {
            //        jc.StartJets();
            //    }

            //}
            //else if(currentTemp >= config.TargetTemp)
            //{
            //    if(config.JetsOn == 0)
            //    {
            //        jc.StopJets();
            //    }
            //}
            //if(currentTemp >= config.FailSafeHot)
            //{
            //    // Stop reguardless of Jets Status
            //    jc.StopJets();
            //}
        }

        public void Shutdown(object? sender, EventArgs e) 
        {
            //RUNNING = false;
            //JetController jc = new JetController(gpioController, RELAY_PIN);
            //jc.StopJets();
            //LightController lc = new LightController(gpioController, LED_PIN);
            //lc.TurnOff();
        }

       

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
