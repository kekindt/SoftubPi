using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using softub.Interfaces;

namespace softub.Controllers
{
    internal class PinController : IPinController, IDisposable
    {
        ILogger<PinController> _logger;
        GpioController _gpioController;
        public PinController(ILogger<PinController> logger)
        {
            _logger = logger;
            _gpioController = new GpioController();
            _gpioController.OpenPin(18, PinMode.Output);
            _gpioController.OpenPin(23, PinMode.Output);
            _logger.LogInformation("PinController Started");
        }

        public void TurnPinOn(int pinNumber, bool lowOn = true)
        {
            //_logger.LogInformation($"Turn Pin On {pinNumber}");
            if (!IsOn(pinNumber, lowOn))
            {
                _gpioController.Write(pinNumber, lowOn ? PinValue.Low : PinValue.High);
            }
        }

        public void TurnPinOff(int pinNumber, bool lowOn = true)
        {
            //_logger.LogInformation($"Turn Pin Off {pinNumber}");
            if (IsOn(pinNumber, lowOn))
            {
                _gpioController.Write(pinNumber, lowOn ? PinValue.High : PinValue.Low);
            }
        }

        public bool IsOn(int pinNumber, bool lowOn = true)
        {
            //_logger.LogInformation($"Is Pin On {pinNumber}");
            PinValue on = lowOn ? PinValue.Low : PinValue.High;
            if (_gpioController.IsPinOpen(pinNumber) && _gpioController.Read(pinNumber) == on)
                return true;
            else
                return false;
        }

        public void Dispose()
        {
            TurnPinOff(18, true);
            TurnPinOff(23, true);
            _gpioController.ClosePin(18);
            _gpioController.ClosePin(23);
        }
    }
}
