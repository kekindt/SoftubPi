// See https://aka.ms/new-console-template for more information
using System;
using System.CodeDom;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using System.Threading;
using Iot.Device.Board;
using Iot.Device.OneWire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using softub.Controllers;
using softub.Interfaces;
using softub.Models;
using softub.Repositories;


internal class Program
{
    public static void Main(string[] args)
    {

        var app = CreateHostBuilder(args).Build();
        app.Run();

        
   }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSystemd()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IPinController, PinController>();
                services.AddScoped<IConfigRepository, ConfigRepository>();
                services.AddScoped<IJetController, JetController>();
                services.AddHostedService<LightController>();
                services.AddHostedService<JetController>();
                services.AddHostedService<ThermometerController>();
                services.AddHostedService<HeaterController>();
                services.AddHostedService<PanelController>();
            });
    }
}


//ControlProcess process = new ControlProcess();
//process.runLoop = true;

//ProcessController controller = new ProcessController();
//controller.Startup();
//controller.Run();

//PosixSignalRegistration.Create(PosixSignal.SIGTERM, c => {
//    process.runLoop = false;
//});
//PosixSignalRegistration.Create(PosixSignal.SIGQUIT, c => {
//    process.runLoop = false;
//});
//PosixSignalRegistration.Create(PosixSignal.SIGINT, c => {
//    process.runLoop = false;
//});
//PosixSignalRegistration.Create(PosixSignal.SIGHUP, c => {
//    process.runLoop = false;
//});

//process.Run();

//while(process.runLoop)
//{
//    Thread.Sleep(1500);
//}