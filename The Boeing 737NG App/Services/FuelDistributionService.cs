using System;

namespace The_Boeing_737NG_App.Services
{
    public interface IFuelDistributionService
    {
        string FuelQuantityWing { get; set; }
        string FuelQuantityCenter { get; set; }
        string MaximumFuelQuantityWing { get; set; }
        string MaximumFuelQuantityCenter { get; set; }
        string MaximumFuelQuantityTotal { get; set; }
        void CalculateFuelDistribution(string totalFuelQuantity, bool useImperialMass, bool useImperialDensity, int progress);
        void CalculateMaximumFuel(bool useImperialMass, bool useImperialDensity, int progress);
        event EventHandler ErrorMessageEvent;
    }

    public class FuelDistributionService : IFuelDistributionService
    {
        const int _wingTankMaxLiters = 4876;
        const int _centerTankMaxLiters = 16273;
        const double _kgTolbs = 2.204623;
        const double _metricToImperialDensity = 8.345404452;
        const double _lToUSG = 0.264172;
        private int _wingTankMaxValue;
        private int _centerTankMaxValue;

        public string FuelQuantityWing { get; set; }
        public string FuelQuantityCenter { get; set; }
        public string MaximumFuelQuantityWing { get; set; }
        public string MaximumFuelQuantityCenter { get; set; }
        public string MaximumFuelQuantityTotal { get; set; }
        public event EventHandler ErrorMessageEvent;

        public FuelDistributionService()
        {
            FuelQuantityCenter = "-----";
            FuelQuantityWing = "----";
        }

        public static FuelDistributionService Instance() => new FuelDistributionService();

        public void CalculateFuelDistribution(string totalFuelQuantity, bool useImperialMass, bool useImperialDensity, int progress)
        {
            if (int.TryParse(totalFuelQuantity, out int totalFuelQuantityValue))
            {
                if (totalFuelQuantityValue > 2 * _wingTankMaxValue + _centerTankMaxValue)
                {
                    FuelQuantityCenter = "-----";
                    FuelQuantityWing = "----";
                    OnErrorMessageEvent(new ErrorMessageEventArgs("Capacity Exceeded!"));
                }
                else
                {
                    if (totalFuelQuantityValue > 2 * _wingTankMaxValue)
                    {
                        int c = totalFuelQuantityValue % 2 == 0 ? 0 : 1;
                        FuelQuantityWing = ((int)_wingTankMaxValue).ToString();
                        FuelQuantityCenter = (c + totalFuelQuantityValue - 2 * (int)_wingTankMaxValue).ToString();
                    }
                    else
                    {
                        FuelQuantityCenter = string.Format("{0}", totalFuelQuantityValue % 2 == 0 ? 0 : 1);
                        FuelQuantityWing = Math.Ceiling((double)(totalFuelQuantityValue / 2)).ToString();
                    }
                }
            }
            else if (String.IsNullOrEmpty(totalFuelQuantity) || String.IsNullOrWhiteSpace(totalFuelQuantity))
            {
                FuelQuantityWing = "----";
                FuelQuantityCenter = "-----";
            }
            else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
        }

        public void CalculateMaximumFuel(bool useImperialMass, bool useImperialDensity, int progress)
        {
            if (!useImperialMass)
            {
                _centerTankMaxValue = (int)(_centerTankMaxLiters * (775 + (double)progress * (100 / 65)) / 1000);
                _wingTankMaxValue = (int)(_wingTankMaxLiters * (775 + (double)progress * (100 / 65)) / 1000);
            }
            else
            {
                _centerTankMaxValue = (int)(_centerTankMaxLiters * _lToUSG * ((775 + (double)progress * (100 / 65)) / 1000) * _metricToImperialDensity);
                _wingTankMaxValue = (int)(_wingTankMaxLiters * _lToUSG * ((775 + (double)progress * (100 / 65)) / 1000) * _metricToImperialDensity);
            }
            MaximumFuelQuantityCenter = $"max {_centerTankMaxValue} ";
            MaximumFuelQuantityWing = $"max {_wingTankMaxValue} ";
            MaximumFuelQuantityTotal = $"max {2 * _wingTankMaxValue + _centerTankMaxValue} ";
        }

        private void OnErrorMessageEvent(ErrorMessageEventArgs eventArgs)
        {
            ErrorMessageEvent?.Invoke(this, eventArgs);
        }
    }

    public class ErrorMessageEventArgs : EventArgs
    {
        public ErrorMessageEventArgs(string errorMessage) => ErrorMessage = errorMessage;
        public string ErrorMessage { get; }
    }
}