using System;

namespace The_Boeing_737NG_App.Services
{
    public interface IFuelUpliftService
    {
        string CalculatedFuelUplift { get; set; }
        string ExpectedFuelUplift { get; set; }
        string FuelUpliftDifference { get; set; }
        void CalculateFuelUplift(string remainingFuel, string calculatedBlockFuel, bool useImperialMass, bool useImperialDensity, double density);
        void CalculateExpectedFuelUplift(string remainingFuel, string actualBlockFuel, bool useImperialMass, bool useImperialDensity, double density);
        void CalculateFuelUpliftDifference(string remainingFuel, string actualBlockFuel, string actualFuelUplift, bool useImperialMass, bool useImperialDensity, double density);
        event EventHandler ErrorMessageEvent;
    }
    public class FuelUpliftService : IFuelUpliftService
    {
        const double _kgTolbs = 2.204623;
        const double _metricToImperialDensity = 8.345404452;
        const double _lToUSG = 0.264172;
        public string CalculatedFuelUplift { get; set; }
        public string ExpectedFuelUplift { get; set; }
        public string FuelUpliftDifference { get; set; }
        public event EventHandler ErrorMessageEvent;

        public static FuelUpliftService Instance() => new FuelUpliftService();

        public void CalculateExpectedFuelUplift(string remainingFuel, string actualBlockFuel, bool useImperialMass, bool useImperialDensity, double density)
        {
            if (!(String.IsNullOrEmpty(remainingFuel) && String.IsNullOrEmpty(actualBlockFuel)))
            {
                if (int.TryParse(remainingFuel, out int parsedRemainingFuel) && int.TryParse(actualBlockFuel, out int parsedActualBlockFuel))
                {
                    double expectedFuelUplift = parsedActualBlockFuel - parsedRemainingFuel;
                    if (!useImperialMass && useImperialDensity) expectedFuelUplift = expectedFuelUplift * _kgTolbs;
                    else if (useImperialMass && !useImperialDensity) expectedFuelUplift = expectedFuelUplift / _kgTolbs;
                    ExpectedFuelUplift = Math.Ceiling(expectedFuelUplift / density).ToString();
                }
                else if (String.IsNullOrEmpty(remainingFuel) || String.IsNullOrEmpty(actualBlockFuel)) ExpectedFuelUplift = "";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
            }
        }

        public void CalculateFuelUplift(string remainingFuel, string calculatedBlockFuel, bool useImperialMass, bool useImperialDensity, double density)
        {

            if (!(String.IsNullOrEmpty(remainingFuel) && String.IsNullOrEmpty(calculatedBlockFuel)))
            {
                if (int.TryParse(remainingFuel, out int parsedRemainingFuel) && int.TryParse(calculatedBlockFuel, out int parsedCalculatedBlockFuel))
                {
                    double calculatedFuelFuelUplift = parsedCalculatedBlockFuel - parsedRemainingFuel;
                    if (!useImperialMass && useImperialDensity) calculatedFuelFuelUplift = calculatedFuelFuelUplift * _kgTolbs;
                    else if (useImperialMass && !useImperialDensity) calculatedFuelFuelUplift = calculatedFuelFuelUplift / _kgTolbs;
                    CalculatedFuelUplift = Math.Ceiling(calculatedFuelFuelUplift / density).ToString();
                }
                else if (String.IsNullOrEmpty(remainingFuel) || String.IsNullOrEmpty(calculatedBlockFuel)) CalculatedFuelUplift = "";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
            }
        }

        public void CalculateFuelUpliftDifference(string remainingFuel, string actualBlockFuel, string actualFuelUplift, bool useImperialMass, bool useImperialDensity, double density)
        {
            if (!(String.IsNullOrEmpty(remainingFuel) && String.IsNullOrEmpty(actualBlockFuel) && String.IsNullOrEmpty(actualFuelUplift)))
            {
                if (int.TryParse(remainingFuel, out int parsedRemainingFuel) && int.TryParse(actualBlockFuel, out int parsedActualBlockFuel) && int.TryParse(actualFuelUplift, out int parsedActualFuelUplift))
                {
                    double expectedFuelUplift = parsedActualBlockFuel - parsedRemainingFuel;
                    if (!useImperialMass && useImperialDensity) expectedFuelUplift = expectedFuelUplift * _kgTolbs;
                    else if (useImperialMass && !useImperialDensity) expectedFuelUplift = expectedFuelUplift / _kgTolbs;
                    FuelUpliftDifference = Math.Round(Math.Abs((100 - 100 * density * parsedActualFuelUplift / expectedFuelUplift)), 2).ToString();
                }
                else if (String.IsNullOrEmpty(remainingFuel) || String.IsNullOrEmpty(actualBlockFuel) || String.IsNullOrEmpty(actualFuelUplift)) FuelUpliftDifference = "";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
            }
        }

        private void OnErrorMessageEvent(ErrorMessageEventArgs eventArgs)
        {
            ErrorMessageEvent?.Invoke(this, eventArgs);
        }
    }
}