using System;

namespace The_Boeing_737NG_App.Services
{
    public interface IFuelCheckDataInputService
    {
        string CalcualteDifference(string blockFuel, string leftEngineFuel, string rightEngineFuel, string remainingFuel);
        event EventHandler ErrorMessageEvent;
    }
    public class FuelCheckDataInputService : IFuelCheckDataInputService
    {
        public event EventHandler ErrorMessageEvent;

        public static FuelCheckDataInputService Instance() => new FuelCheckDataInputService();

        public string CalcualteDifference(string blockFuel, string leftEngineFuel, string rightEngineFuel, string remainingFuel)
        {
            if (!(String.IsNullOrEmpty(blockFuel) && String.IsNullOrEmpty(leftEngineFuel) && String.IsNullOrEmpty(rightEngineFuel) && String.IsNullOrEmpty(remainingFuel)))
            {
                if (int.TryParse(blockFuel, out int parsedBlockFuel) && int.TryParse(leftEngineFuel, out int parsedLeftEngineFuel) && int.TryParse(rightEngineFuel, out int parsedRightEngineFuel) && int.TryParse(remainingFuel, out int parsedRemainingFuel))
                {
                    return (parsedBlockFuel-(parsedRemainingFuel + parsedLeftEngineFuel + parsedRightEngineFuel)).ToString();
                }
                else if (String.IsNullOrEmpty(blockFuel) || String.IsNullOrEmpty(leftEngineFuel) || String.IsNullOrEmpty(rightEngineFuel) || String.IsNullOrEmpty(remainingFuel)) return "";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
            }
            return "----";
        }

        private void OnErrorMessageEvent(ErrorMessageEventArgs eventArgs)
        {
            ErrorMessageEvent?.Invoke(this, eventArgs);
        }
    }
}