using Android.Content;

namespace The_Boeing_737NG_App.Services
{
    public interface IFuelOrderService
    {
        void Send(Android.Support.V4.App.Fragment fragment, string flightNumber, string flightDate, string departure, string destination, string registration, string aircraft, string standOrGate, string captain, string massUnit, string totalFuel, string centerFuel, string wingFuel);
    }
    public class FuelOrderService : IFuelOrderService
    {
        public static FuelOrderService Instance() => new FuelOrderService();

        public void Send(Android.Support.V4.App.Fragment fragment, string flightNumber, string flightDate, string departure, string destination, string registration, string aircraft, string standOrGate, string captain, string massUnit, string totalFuel, string centerFuel, string wingFuel)
        {
            var emailIntent = new Intent(Intent.ActionSend);
            emailIntent.PutExtra(Intent.ExtraSubject, $"Fuel Order for Flight {flightNumber} on {flightDate} to {destination}");
            emailIntent.PutExtra(Intent.ExtraText, 
                $"Flight No ▶ {flightNumber}" + System.Environment.NewLine +
                $"Date ▶ {flightDate}" + System.Environment.NewLine +
                $"Departure ▶ {departure}" + System.Environment.NewLine +
                $"Destination ▶ {destination}" + System.Environment.NewLine +
                $"Registration ▶ {registration}" + System.Environment.NewLine +
                $"Aircraft ▶ {aircraft}" + System.Environment.NewLine +
                $"Stand / Gate ▶ {standOrGate}" + System.Environment.NewLine +
                $"Captain's Name ▶ {captain}" + System.Environment.NewLine +
                $"Total Fuel ▶" + System.Environment.NewLine +
                $"Tank 1 ▶ {wingFuel}{massUnit}" + System.Environment.NewLine +
                $"Tank 2 ▶{wingFuel}{massUnit}" + System.Environment.NewLine +
                $"Center Tank ▶{centerFuel}{massUnit}");
            emailIntent.SetType("message/rfc822");
            fragment.StartActivity(emailIntent);
        }
    }
}