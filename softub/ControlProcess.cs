using System.Device.Gpio;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Iot.Device.GoPiGo3.Sensors;
using Iot.Device.OneWire;
using softub.Models;

public class ControlProcess
{
    ConfigValue? config = null;

    public bool runLoop = true;

    int LED_PIN = 18;
    int RELAY_PIN = 23;

    int tempCheckInterval = 30;
    int tempLastCheck = 0;

    double currentTemp = 0;
    double requestedTemp = 100;

    GpioController? controller = null;

    

    public ControlProcess()
    {
        var configDbPath = Environment.GetEnvironmentVariable("CONFIG_DB");
    }

    public void Run()
    {
        
        //int pin = 18;
        controller = new GpioController();

        controller.OpenPin(LED_PIN, PinMode.Output);
        controller.OpenPin(RELAY_PIN, PinMode.Output);

        controller.StopPump(RELAY_PIN);
        
        currentTemp = GetCurrentTemperature(true);
        SaveValues();

        while (runLoop)
        {
            config = GetConfigValue();

            TurnLightsOn(config.LightsOn == 1 ? true : false);

            currentTemp = GetCurrentTemperature(false);
            SaveValues();


            if (config.JetsOn == 1)
            {
                
                if (currentTemp >= config.FailSafeHot)
                {
                    // stop pump
                    controller.StopPump(RELAY_PIN);
                    // alert
                }
                else
                {
                    controller.StartPump(RELAY_PIN);
                }
            }
            else
            {
                if (currentTemp >= config.FailSafeHot)
                {
                    // stop pump
                    controller.StopPump(RELAY_PIN);
                    // alert
                }

                if (currentTemp < config.TargetTemp)
                {
                    // start pump
                    controller.StartPump(RELAY_PIN);

                }
                else if (currentTemp >= config.TargetTemp)
                {
                    // stop pump
                    controller.StopPump(RELAY_PIN);
                }
            }
            Thread.Sleep(1000); // 1 seconds for debug
        }

        // Shutting Down, turn off LED and Pump
        controller.Write(LED_PIN, PinValue.Low);
        controller.StopPump(RELAY_PIN);

        // Close pin control
        controller.ClosePin(RELAY_PIN);
        controller.ClosePin(LED_PIN);
    }

    ConfigValue GetConfigValue()
    {
        using (var db = new ConfigContext())
        {
            var config = db.ConfigValues.ToList();
            if (config == null || config.Count == 0)
            {
                // Add default row
                ConfigValue cv = new ConfigValue()
                {
                    TargetTemp = 100,
                    FailSafeCold = 40,
                    FailSafeHot = 110,
                    JetsOn = 0,
                    LightsOn = 0,
                    LastTemp = 0
                };
                db.ConfigValues.Add(cv);
                db.SaveChanges();
                return cv;
            }
            return config.FirstOrDefault();
        }
    }

    void SaveValues()
    {
        using (var db = new ConfigContext())
        {
            var dbValues = db.ConfigValues.ToList();
            var dbValue = dbValues.FirstOrDefault();

            dbValue.LastTemp = Convert.ToInt32(currentTemp);
            db.ConfigValues.Update(dbValue);
            db.SaveChanges();
        }
    }

    void TurnLightsOn(bool value = true)
    {
        Console.WriteLine($"TurnLightsOn: {value}");
        if (!value)
        {
            if (controller.IsPinOpen(LED_PIN) && controller.Read(LED_PIN) != PinValue.High)
                controller.Write(LED_PIN, PinValue.High);
        }
        else
        {
            if (controller.IsPinOpen(LED_PIN) && controller.Read(LED_PIN) != PinValue.Low)
                controller.Write(LED_PIN, PinValue.Low);
        }
        Thread.Sleep(3);
    }

    double GetCurrentTemperature(bool overrideInterval = false)
    {
        Console.WriteLine("GetCurrentTemperature");
        if (tempLastCheck == tempCheckInterval || overrideInterval)
        {
            tempLastCheck = 0;
            Dictionary<string, double> readTemps = new Dictionary<string, double>();
            foreach (var dev in OneWireThermometerDevice.EnumerateDevices())
            {
                var readTemp = dev.ReadTemperature().DegreesFahrenheit;
                Console.WriteLine($"Thermometer {dev.DeviceId}: {readTemp}");
                readTemps.Add(dev.DeviceId, readTemp);
            }

            return readTemps.Values.Average();
        }
        else
        {
            Console.WriteLine($"SkipCheck {tempLastCheck}");
            tempLastCheck++;
            return currentTemp;
        }
    }
}