using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Timers;

namespace softub
{
    internal class JetController : RelayController
    {
        public JetController(GpioController gpioController, int pinNumber) : base(gpioController, pinNumber)
        {
            
        }

        public bool StartJets(int secondsToRun)
        {
            System.Timers.Timer timer = new System.Timers.Timer(secondsToRun);
            timer.Elapsed += Timer_Elapsed;
            _gpioController.StartPump(_pinNumber);
            timer.Start();
            return true;
        }

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            base._gpioController.StopPump(_pinNumber);
        }
    }
}
