using System.CodeDom;
using System.Device.Gpio;

public static class Extensions
{
    public static void StartPump(this GpioController controller, int pin)
    {
        Console.WriteLine($"StartPump");
        if(controller.IsPinOpen(pin) && controller.Read(pin) != PinValue.Low)
            controller.Write(pin, PinValue.Low);
    }

    public static void StopPump(this GpioController controller, int pin)
    {
        Console.WriteLine($"StopPump");
        if(controller.IsPinOpen(pin) && controller.Read(pin) != PinValue.High)
            controller.Write(pin, PinValue.High);
    }
}