using System;

namespace The_Boeing_737NG_App.Services
{
    public interface IBaggageDistributionService
    {
        event EventHandler ErrorMessageEvent;
        string RemainingPices { get; set; }
        string Hold1Mass { get; set; }
        string Hold2Mass { get; set; }
        string Hold3Mass { get; set; }
        string Hold4Mass { get; set; }
        void CalculateBaggageDistribution(string totalBaggageMass, string totalPices, string hold1Pices, string hold2Pices, string hold3Pices, string hold4Pices);
    }
    public class BaggageDistributionService : IBaggageDistributionService
    {
        public event EventHandler ErrorMessageEvent;
        public string RemainingPices { get; set; }
        public string Hold1Mass { get; set; }
        public string Hold2Mass { get; set; }
        public string Hold3Mass { get; set; }
        public string Hold4Mass { get; set; }
        public static BaggageDistributionService Instance() => new BaggageDistributionService();
        public void CalculateBaggageDistribution(string totalBaggageMass, string totalPices, string hold1Pices, string hold2Pices, string hold3Pices, string hold4Pices)
        {
            if (int.TryParse(totalBaggageMass, out int parsedTotalBaggageMass) && int.TryParse(totalPices, out int parsedTotalPices))
            {
                double averageBaggagePiceMass = 0;
                if (parsedTotalPices != 0) averageBaggagePiceMass = parsedTotalBaggageMass / parsedTotalPices;
                if (int.TryParse(hold1Pices, out int parsedHold1Pices))
                {
                    Hold1Mass = Math.Ceiling(parsedHold1Pices * averageBaggagePiceMass).ToString();
                    parsedTotalPices = parsedTotalPices - parsedHold1Pices;
                }
                else if (String.IsNullOrEmpty(hold1Pices)) Hold1Mass = "0";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
                if (int.TryParse(hold2Pices, out int parsedHold2Pices))
                {
                    Hold2Mass = Math.Ceiling(parsedHold2Pices * averageBaggagePiceMass).ToString();
                    parsedTotalPices = parsedTotalPices - parsedHold2Pices;
                }
                else if (String.IsNullOrEmpty(hold2Pices)) Hold2Mass = "0";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
                if (int.TryParse(hold3Pices, out int parsedHold3Pices))
                {
                    Hold3Mass = Math.Ceiling(parsedHold3Pices * averageBaggagePiceMass).ToString();
                    parsedTotalPices = parsedTotalPices - parsedHold3Pices;
                }
                else if (String.IsNullOrEmpty(hold3Pices)) Hold3Mass = "0";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
                if (int.TryParse(hold4Pices, out int parsedHold4Pices))
                {
                    Hold4Mass = Math.Ceiling(parsedHold4Pices * averageBaggagePiceMass).ToString();
                    parsedTotalPices = parsedTotalPices - parsedHold4Pices;
                }
                else if (String.IsNullOrEmpty(hold4Pices)) Hold4Mass = "0";
                else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
                if (parsedTotalPices < 0) OnErrorMessageEvent(new ErrorMessageEventArgs($"{Math.Abs(parsedTotalPices)} negative Pice(s) detected!"));
                RemainingPices = parsedTotalPices.ToString();
            }
            else if (String.IsNullOrEmpty(totalBaggageMass) || String.IsNullOrEmpty(totalPices)) { }
            else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
        }

        private void OnErrorMessageEvent(ErrorMessageEventArgs eventArgs)
        {
            ErrorMessageEvent?.Invoke(this, eventArgs);
        }
    }
}