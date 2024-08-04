using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Controllers
{
    internal class RelayController
    {
        protected GpioController _gpioController;
        protected int _pinNumber;

        public RelayController(GpioController gpioController, int pinNumber)
        {
            _gpioController = gpioController;
            _pinNumber = pinNumber;
        }

        public bool IsOn()
        {
            if (_gpioController.IsPinOpen(_pinNumber) && _gpioController.Read(_pinNumber) == PinValue.Low)
                return true;
            else
                return false;
        }
    }
}
