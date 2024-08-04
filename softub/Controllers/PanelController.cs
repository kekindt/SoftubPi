using Microsoft.Extensions.Hosting;
using softub.Interfaces;
using softub.Utilities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softub.Controllers
{
    internal class PanelController : BackgroundService, IPanelController
    {
        const int TEMP_UP = 75;
        const int TEMP_DOWN = 135;
        const int JETS = 30;
        const int LIGHT = 45;

        IConfigRepository _configRepository;

        static SerialPort port = null;
        int temp = 100;
        static bool heatOn = false;
        static bool filterOn = false;
        byte[] displayBuffer = [0x02, 0x00, 0x01, 0x00, 0x00, 0x01, 0xFF];

        public PanelController(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
            if (port == null)
            {
                port = new SerialPort("/dev/serial0", 2400);
                port.Open();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var configValues = _configRepository.GetConfigValue();

                if (port.IsOpen)
                {
                    var readByte = port.ReadByte();
                    switch (readByte)
                    {
                        //case 75:
                        case TEMP_UP:
                            Console.WriteLine("Temp Up"); // up
                            temp++;
                            configValues.TargetTemp = temp;
                            break;
                        case TEMP_DOWN:
                        //case 135:
                            Console.WriteLine("Temp Down"); // down
                            temp--;
                            configValues.TargetTemp = temp;
                            break;
                        //case 30:
                        case JETS:
                            Console.WriteLine("Jets"); // jets
                                                       //displayBuffer[(int)bufferPosition.LEDS] = (byte)(displayBuffer[(int)bufferPosition.LEDS] | 0x10);
                            //heatOn = !heatOn;
                            configValues.JetsOn = 1;
                            //SetHeatLight();
                            break;
                        //case 45:
                        case LIGHT:
                            Console.WriteLine("Light"); // light
                                                        //displayBuffer[(int)bufferPosition.LEDS] = (byte)(displayBuffer[(int)bufferPosition.LEDS] | 0x20);
                            //filterOn = !filterOn;
                            configValues.LightsOn = 1;
                            //SetFilterLight();
                            break;
                        default:
                            break;
                    }

                    SetTempForScreen(temp, ref displayBuffer);
                    SetLEDs();
                    WriteToPanel(ref displayBuffer);

                }

                await Task.Delay(500, stoppingToken);
            }
        }

        public void TurnOnHeatLight()
        {
            heatOn = true;
        }
        public void TurnOffHeatLight()
        {
            heatOn = false;
        }
        public void TurnOnFilterLight()
        {
            filterOn = true;
        }
        public void TurnOffFilterLight()
        {
            filterOn = false;
        }

        void SetLEDs()
        {
            if (heatOn && filterOn)
            {
                displayBuffer[(int)BufferPosition.LEDS] = 0x30;
            }
            else if (heatOn && !filterOn)
            {
                displayBuffer[(int)BufferPosition.LEDS] = 0x20;
            }
            else if (!heatOn && filterOn)
            {
                displayBuffer[(int)BufferPosition.LEDS] = 0x10;
            }
            else if (!heatOn && !filterOn)
            {
                displayBuffer[(int)BufferPosition.LEDS] = 0x00;
            }
        }


        /// <summary>
        /// Set display buffer values for the tempurature
        /// 
        /// 0x0A sets a blank value to the panel screen
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="displayBuffer"></param>
        void SetTempForScreen(int temp, ref byte[] displayBuffer)
        {
            byte p1, p2, p3;
            if (temp >= 100)
            {
                p1 = 1;
            }
            else
            {
                p1 = 0x0A;
            }
            p2 = (byte)(temp / 10 % 10);
            p3 = (byte)(temp % 10);
            /* 
                        # 0A is blank
                        h = 0x0A if temp < 100 else temp // 100
                    self.display_set_digits(h, int((temp // 10) % 10), int(temp % 10))
            */

            displayBuffer[(int)BufferPosition.Temp1] = p1;
            displayBuffer[(int)BufferPosition.Temp2] = p2;
            displayBuffer[(int)BufferPosition.Temp3] = p3;
        }
        /// <summary>
        /// Calcuate the checksum byte and send to panel
        /// 
        /// for i in range(1, 5):
        /// sum += self.display_buffer[i]
        /// self.display_buffer[5] = sum & 0xFF
        /// </summary>
        /// <param name="displayBuffer"></param>
        void WriteToPanel(ref byte[] displayBuffer)
        {
            byte sum = 0;
            for (int i = 1; i < 5; i++)
            {
                sum += displayBuffer[i];
            }
            displayBuffer[(int)BufferPosition.Checksum] = (byte)(sum & 0xFF);
            port.Write(displayBuffer, 0, displayBuffer.Length);
        }
    }
}
