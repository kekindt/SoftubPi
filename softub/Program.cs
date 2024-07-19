// See https://aka.ms/new-console-template for more information
using System;
using System.CodeDom;
using System.Device.Gpio;
using System.Runtime.InteropServices;
using System.Threading;
using Iot.Device.Board;
using Iot.Device.OneWire;

using softub.Models;



ControlProcess process = new ControlProcess();
process.runLoop = true;

PosixSignalRegistration.Create(PosixSignal.SIGTERM, c => {
    process.runLoop = false;
});
PosixSignalRegistration.Create(PosixSignal.SIGQUIT, c => {
    process.runLoop = false;
});
PosixSignalRegistration.Create(PosixSignal.SIGINT, c => {
    process.runLoop = false;
});
PosixSignalRegistration.Create(PosixSignal.SIGHUP, c => {
    process.runLoop = false;
});

process.Run();

while(process.runLoop)
{
    Thread.Sleep(1500);
}