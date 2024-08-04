namespace softub.Interfaces
{
    internal interface IPinController
    {
        void TurnPinOn(int pinNumber, bool lowOn);
        void TurnPinOff(int pinNumber, bool lowOn);
        bool IsOn(int pinNumber, bool lowOn);
    }
}