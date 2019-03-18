using System;

namespace The_Boeing_737NG_App.Services
{
    public interface IBrakeCoolingService
    {
        string BrakeCoolingMessage(bool steel, bool reverse, bool flight, bool useFahrenheit, bool useImperialMass, int eventIndex, string mass, string speed, string altitude, string temperature);
        string BrakeCoolingMessage(bool steel, bool flight, string brakeTemperatureIndication);
        event EventHandler ErrorMessageEvent;
    }
    public class BrakeCoolingService : IBrakeCoolingService
    {
        const double _kgTolbs = 2.204623;
        public event EventHandler ErrorMessageEvent;
        #region Methods
        public static BrakeCoolingService Instance() => new BrakeCoolingService();
        public string BrakeCoolingMessage(bool steel, bool reverse, bool flight, bool useFahrenheit, bool useImperialMass, int eventIndex, string mass, string speed, string altitude, string temperature)
        {
            if (int.TryParse(mass, out int parsedMass) && int.TryParse(speed, out int parsedSpeed) && int.TryParse(altitude, out int parsedAltitude) && int.TryParse(temperature, out int parsedTemperature))
            {
                if (useFahrenheit) parsedTemperature = (int)Math.Ceiling((parsedTemperature - 32) * (5.0 / 9.0));
                if (useImperialMass) parsedMass = (int)Math.Ceiling(parsedMass / _kgTolbs);
                double referenceBrakeEnergy = ReferenceBrakeEnergy(parsedMass, parsedSpeed, altitudeIndex(parsedAltitude), temperatureIndex(parsedTemperature));
                int referenceBrakeEnergyIndex = ReferenceBrakeEnergyIndex(referenceBrakeEnergy);
                double eventAdjustedBrakeEnergy = reverse ? TwoEngineDetentReverseThrustEventAdjustedBrakeEnergy(referenceBrakeEnergyIndex, eventIndex) : NoReverseThrustEventAdjustedBrakeEnergy(referenceBrakeEnergyIndex, eventIndex);
                int eventAdjustedBrakeEnergyIndex = EventAdjustedBrakeEnergyIndex(eventAdjustedBrakeEnergy);
                string coolingMessage = steel ? SteelBrakesCooling(flight ? 0 : 1, eventAdjustedBrakeEnergyIndex) : CarbonBrakesCooling(flight ? 0 : 1, eventAdjustedBrakeEnergyIndex);
                return coolingMessage;
            }
            else if (String.IsNullOrEmpty(mass) || String.IsNullOrEmpty(speed) || String.IsNullOrEmpty(altitude) || String.IsNullOrEmpty(temperature)) return "Cooling Time";
            else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
            return "Cooling Time";
        }

        public string BrakeCoolingMessage(bool steel, bool flight, string brakeTemperatureIndication)
        {
            var indexOfPoint = brakeTemperatureIndication.IndexOf('.');            
            if (indexOfPoint > 0) brakeTemperatureIndication = brakeTemperatureIndication.Replace('.', ',');
            if (Double.TryParse(brakeTemperatureIndication, out double parsedBrakeTemperatureIndication))
            {
                int brakeTemperatureIndicationIndex = steel ? SteelBrakeTemperatureIndicationIndex(parsedBrakeTemperatureIndication) : CarbonBrakeTemperatureIndicationIndex(parsedBrakeTemperatureIndication);
                string coolingMessage = steel ? SteelBrakesCooling(flight ? 0 : 1, brakeTemperatureIndicationIndex) : CarbonBrakesCooling(flight ? 0 : 1, brakeTemperatureIndicationIndex);
                return coolingMessage;
            }
            else if (String.IsNullOrEmpty(brakeTemperatureIndication)) return "Cooling Time";
            else OnErrorMessageEvent(new ErrorMessageEventArgs("Only Numbers!"));
            return "Cooling Time";
        }

        double ReferenceBrakeEnergy(int mass, int speed, int altitudeIndex, int temperatureIndex)
        {
            if (mass <= 40000)
            {
                if (speed <= 80)
                {
                    return W40S80(altitudeIndex, temperatureIndex);
                }

                else if (speed > 80 & speed <= 100)
                {
                    return W40S100(altitudeIndex, temperatureIndex);
                }

                else if (speed > 100 & speed <= 120)
                {
                    return W40S120(altitudeIndex, temperatureIndex);
                }

                else if (speed > 120 & speed <= 140)
                {
                    return W40S140(altitudeIndex, temperatureIndex);
                }

                else if (speed > 140 & speed <= 160)
                {
                    return W40S160(altitudeIndex, temperatureIndex);
                }

                else if (speed > 160 & speed <= 180)
                {
                    return W40S180(altitudeIndex, temperatureIndex);
                }

            }

            else if (mass > 40000 & mass <= 50000)
            {
                if (speed <= 80)
                {
                    return W50S80(altitudeIndex, temperatureIndex);
                }

                else if (speed > 80 & speed <= 100)
                {
                    return W50S100(altitudeIndex, temperatureIndex);
                }

                else if (speed > 100 & speed <= 120)
                {
                    return W50S120(altitudeIndex, temperatureIndex);
                }

                else if (speed > 120 & speed <= 140)
                {
                    return W50S140(altitudeIndex, temperatureIndex);
                }

                else if (speed > 140 & speed <= 160)
                {
                    return W50S160(altitudeIndex, temperatureIndex);
                }

                else if (speed > 160 & speed <= 180)
                {
                    return W50S180(altitudeIndex, temperatureIndex);
                }
            }

            else if (mass > 50000 & mass <= 60000)
            {
                if (speed <= 80)
                {
                    return W60S80(altitudeIndex, temperatureIndex);
                }

                else if (speed > 80 & speed <= 100)
                {
                    return W60S100(altitudeIndex, temperatureIndex);
                }

                else if (speed > 100 & speed <= 120)
                {
                    return W60S120(altitudeIndex, temperatureIndex);
                }

                else if (speed > 120 & speed <= 140)
                {
                    return W60S140(altitudeIndex, temperatureIndex);
                }

                else if (speed > 140 & speed <= 160)
                {
                    return W60S160(altitudeIndex, temperatureIndex);
                }

                else if (speed > 160 & speed <= 180)
                {
                    return W60S180(altitudeIndex, temperatureIndex);
                }
            }

            else if (mass > 60000 & mass <= 70000)
            {
                if (speed <= 80)
                {
                    return W70S80(altitudeIndex, temperatureIndex);
                }

                else if (speed > 80 & speed <= 100)
                {
                    return W70S100(altitudeIndex, temperatureIndex);
                }

                else if (speed > 100 & speed <= 120)
                {
                    return W70S120(altitudeIndex, temperatureIndex);
                }

                else if (speed > 120 & speed <= 140)
                {
                    return W70S140(altitudeIndex, temperatureIndex);
                }

                else if (speed > 140 & speed <= 160)
                {
                    return W70S160(altitudeIndex, temperatureIndex);
                }

                else if (speed > 160 & speed <= 180)
                {
                    return W70S180(altitudeIndex, temperatureIndex);
                }
            }

            else if (mass > 70000 & mass <= 80000)
            {
                if (speed <= 80)
                {
                    return W80S80(altitudeIndex, temperatureIndex);
                }

                else if (speed > 80 & speed <= 100)
                {
                    return W80S100(altitudeIndex, temperatureIndex);
                }

                else if (speed > 100 & speed <= 120)
                {
                    return W80S120(altitudeIndex, temperatureIndex);
                }

                else if (speed > 120 & speed <= 140)
                {
                    return W80S140(altitudeIndex, temperatureIndex);
                }

                else if (speed > 140 & speed <= 160)
                {
                    return W80S160(altitudeIndex, temperatureIndex);
                }

                else if (speed > 160 & speed <= 180)
                {
                    return W80S180(altitudeIndex, temperatureIndex);
                }
            }
            return 0;
        }

        string SteelBrakesCooling(int GearIndex, int EventAdjustedBrakeEnergyIndex)
        {
            string[,] CoolingTime = new string[9, 2];

            CoolingTime[0, 0] = "NO SPECIAL PROC REQ";
            CoolingTime[0, 1] = "NO SPECIAL PROC REQ";
            CoolingTime[1, 0] = "Cooling time: 1min";
            CoolingTime[1, 1] = "Cooling time: 10min";
            CoolingTime[2, 0] = "Cooling time: 2min";
            CoolingTime[2, 1] = "Cooling time: 20min";
            CoolingTime[3, 0] = "Cooling time: 3min";
            CoolingTime[3, 1] = "Cooling time: 30min";
            CoolingTime[4, 0] = "Cooling time: 4min";
            CoolingTime[4, 1] = "Cooling time: 40min";
            CoolingTime[5, 0] = "Cooling time: 5min";
            CoolingTime[5, 1] = "Cooling time: 50min";
            CoolingTime[6, 0] = "Cooling time: 6min";
            CoolingTime[6, 1] = "Cooling time: 60min";
            CoolingTime[7, 0] = "CAUTION";
            CoolingTime[7, 1] = "CAUTION";
            CoolingTime[8, 0] = "FUSE PLUG MELT ZONE";
            CoolingTime[8, 1] = "FUSE PLUG MELT ZONE";

            return CoolingTime[EventAdjustedBrakeEnergyIndex, GearIndex];
        }

        string CarbonBrakesCooling(int GearIndex, int EventAdjustedBrakeEnergyIndex)
        {
            string[,] CoolingTime = new string[9, 2];

            CoolingTime[0, 0] = "NO SPECIAL PROC REQ";
            CoolingTime[0, 1] = "NO SPECIAL PROC REQ";
            CoolingTime[1, 0] = "Cooling time: 1min";
            CoolingTime[1, 1] = "Cooling time: 6.7min";
            CoolingTime[2, 0] = "Cooling time: 4min";
            CoolingTime[2, 1] = "Cooling time: 16min";
            CoolingTime[3, 0] = "Cooling time: 5min";
            CoolingTime[3, 1] = "Cooling time: 24.1min";
            CoolingTime[4, 0] = "Cooling time: 6min";
            CoolingTime[4, 1] = "Cooling time: 34.2min";
            CoolingTime[5, 0] = "Cooling time: 7min";
            CoolingTime[5, 1] = "Cooling time: 45.9min";
            CoolingTime[6, 0] = "Cooling time: 7.6min";
            CoolingTime[6, 1] = "Cooling time: 53.3min";
            CoolingTime[7, 0] = "CAUTION";
            CoolingTime[7, 1] = "CAUTION";
            CoolingTime[8, 0] = "FUSE PLUG MELT ZONE";
            CoolingTime[8, 1] = "FUSE PLUG MELT ZONE";

            return CoolingTime[EventAdjustedBrakeEnergyIndex, GearIndex];
        }
        #endregion

        #region Indexer
        int MassIndex(int mass)
        {
            if (mass <= 40000) return 0;
            else if (mass > 40000 & mass <= 50000) return 1;
            else if (mass > 50000 & mass <= 60000) return 2;
            else if (mass > 60000 & mass <= 70000) return 3;
            //else if (mass > 70000) return 4;
            return 4;
        }

        int SpeedIndex(int speed)
        {
            if (speed < 80) return 0;
            else if (speed > 80 & speed <= 100) return 1;
            else if (speed > 100 & speed <= 120) return 2;
            else if (speed > 120 & speed <= 140) return 3;
            else if (speed > 140 & speed <= 160) return 4;
            //else if (s > 160) return 5;
            return 5;
        }

        int temperatureIndex(int temperature)
        {
            if (temperature <= 0) return 0;
            else if (temperature > 0 & temperature <= 10) return 1;
            else if (temperature > 10 & temperature <= 15) return 2;
            else if (temperature > 15 & temperature <= 20) return 3;
            else if (temperature > 20 & temperature <= 30) return 4;
            else if (temperature > 30 & temperature <= 40) return 5;
            //else if (temperature > 40) return 6;
            return 6;
        }

        int altitudeIndex(int altitude)
        {
            if (altitude == 0) return 0;
            else if (altitude > 0 & altitude <= 5000) return 1;
            //else if (altitude > 5000 & altitude <= 10000) return 2;
            return 2;
        }

        int ReferenceBrakeEnergyIndex(double referenceBrakeEnergy)
        {
            if (referenceBrakeEnergy <= 10) return 0;
            else if (referenceBrakeEnergy > 10 & referenceBrakeEnergy <= 20) return 1;
            else if (referenceBrakeEnergy > 20 & referenceBrakeEnergy <= 30) return 2;
            else if (referenceBrakeEnergy > 30 & referenceBrakeEnergy <= 40) return 3;
            else if (referenceBrakeEnergy > 40 & referenceBrakeEnergy <= 50) return 4;
            else if (referenceBrakeEnergy > 50 & referenceBrakeEnergy <= 60) return 5;
            else if (referenceBrakeEnergy > 60 & referenceBrakeEnergy <= 70) return 6;
            else if (referenceBrakeEnergy > 70 & referenceBrakeEnergy <= 80) return 7;
            //else if (referenceBrakeEnergy > 80) return 8;
            return 8;
        }

        int EventAdjustedBrakeEnergyIndex(double eventAdjustedBrakeEnergy)
        {
            if (eventAdjustedBrakeEnergy <= 16) return 0;
            else if (eventAdjustedBrakeEnergy > 16 & eventAdjustedBrakeEnergy <= 17) return 1;
            else if (eventAdjustedBrakeEnergy > 17 & eventAdjustedBrakeEnergy <= 20) return 2;
            else if (eventAdjustedBrakeEnergy > 20 & eventAdjustedBrakeEnergy <= 23) return 3;
            else if (eventAdjustedBrakeEnergy > 23 & eventAdjustedBrakeEnergy <= 25) return 4;
            else if (eventAdjustedBrakeEnergy > 25 & eventAdjustedBrakeEnergy <= 28) return 5;
            else if (eventAdjustedBrakeEnergy > 28 & eventAdjustedBrakeEnergy <= 32) return 6;
            else if (eventAdjustedBrakeEnergy > 32 & eventAdjustedBrakeEnergy <= 48) return 7;
            //else if (eventAdjustedBrakeEnergy > 48) return 8;
            return 8;
        }

        int EventAdjustedCarbonBrakeEnergy(double eventAdjustedBrakeEnergy)
        {
            if (eventAdjustedBrakeEnergy <= 16) return 0;
            else if (eventAdjustedBrakeEnergy > 16 & eventAdjustedBrakeEnergy <= 17) return 1;
            else if (eventAdjustedBrakeEnergy > 17 & eventAdjustedBrakeEnergy <= 19) return 2;
            else if (eventAdjustedBrakeEnergy > 19 & eventAdjustedBrakeEnergy <= 20.9) return 3;
            else if (eventAdjustedBrakeEnergy > 20.9 & eventAdjustedBrakeEnergy <= 23.5) return 4;
            else if (eventAdjustedBrakeEnergy > 23.5 & eventAdjustedBrakeEnergy <= 26.9) return 5;
            else if (eventAdjustedBrakeEnergy > 26.9 & eventAdjustedBrakeEnergy <= 29.4) return 6;
            else if (eventAdjustedBrakeEnergy > 29.4 & eventAdjustedBrakeEnergy <= 41) return 7;
            //else if (eventAdjustedBrakeEnergy > 41) return 8;
            return 8;
        }

        int SteelBrakeTemperatureIndicationIndex(double brakeTemperatureIndication)
        {
            if (brakeTemperatureIndication <= 2.4) return 0;
            else if (brakeTemperatureIndication > 2.4 & brakeTemperatureIndication <= 2.6) return 1;
            else if (brakeTemperatureIndication > 2.6 & brakeTemperatureIndication <= 3.1) return 2;
            else if (brakeTemperatureIndication > 3.1 & brakeTemperatureIndication <= 3.5) return 3;
            else if (brakeTemperatureIndication > 3.5 & brakeTemperatureIndication <= 3.9) return 4;
            else if (brakeTemperatureIndication > 3.9 & brakeTemperatureIndication <= 4.4) return 5;
            else if (brakeTemperatureIndication > 4.4 & brakeTemperatureIndication <= 4.9) return 6;
            else if (brakeTemperatureIndication > 4.9 & brakeTemperatureIndication <= 7.5) return 7;
            //else if (brakeTemperatureIndication > 7.5) return 8;
            return 8;
        }

        int CarbonBrakeTemperatureIndicationIndex(double brakeTemperatureIndication)
        {
            if (brakeTemperatureIndication <= 2.5) return 0;
            else if (brakeTemperatureIndication > 2.5 & brakeTemperatureIndication <= 2.6) return 1;
            else if (brakeTemperatureIndication > 2.6 & brakeTemperatureIndication <= 3.0) return 2;
            else if (brakeTemperatureIndication > 3.0 & brakeTemperatureIndication <= 3.3) return 3;
            else if (brakeTemperatureIndication > 3.3 & brakeTemperatureIndication <= 3.8) return 4;
            else if (brakeTemperatureIndication > 3.8 & brakeTemperatureIndication <= 4.5) return 5;
            else if (brakeTemperatureIndication > 4.5 & brakeTemperatureIndication <= 4.9) return 6;
            else if (brakeTemperatureIndication > 4.9 & brakeTemperatureIndication <= 7.1) return 7;
            //else if (brakeTemperatureIndication > 7.1) return 8;
            return 8;
        }
        #endregion

        #region Tables
        double W40S80(int altitudeIndex, int temperatureIndex)
        {
            double[,] W40S80 = new double[3, 7];

            W40S80[0, 0] = 9.6;
            W40S80[0, 1] = 10.0;
            W40S80[0, 2] = 10.1;
            W40S80[0, 3] = 10.2;
            W40S80[0, 4] = 10.5;
            W40S80[0, 5] = 10.6;
            W40S80[0, 6] = 10.6;

            W40S80[1, 0] = 10.8;
            W40S80[1, 1] = 11.2;
            W40S80[1, 2] = 11.4;
            W40S80[1, 3] = 11.5;
            W40S80[1, 4] = 11.8;
            W40S80[1, 5] = 11.9;
            W40S80[1, 6] = 11.9;

            W40S80[2, 0] = 12.3;
            W40S80[2, 1] = 12.7;
            W40S80[2, 2] = 12.9;
            W40S80[2, 3] = 13.1;
            W40S80[2, 4] = 13.4;
            W40S80[2, 5] = 13.5;
            W40S80[2, 6] = 13.5;

            return W40S80[altitudeIndex, temperatureIndex];
        }

        double W40S100(int altitudeIndex, int temperatureIndex)
        {
            double[,] W40S100 = new double[3, 7];

            W40S100[0, 0] = 13.5;
            W40S100[0, 1] = 14.0;
            W40S100[0, 2] = 14.2;
            W40S100[0, 3] = 14.4;
            W40S100[0, 4] = 14.8;
            W40S100[0, 5] = 14.9;
            W40S100[0, 6] = 14.9;

            W40S100[1, 0] = 15.2;
            W40S100[1, 1] = 15.8;
            W40S100[1, 2] = 16.0;
            W40S100[1, 3] = 16.2;
            W40S100[1, 4] = 16.6;
            W40S100[1, 5] = 16.8;
            W40S100[1, 6] = 16.8;

            W40S100[2, 0] = 17.3;
            W40S100[2, 1] = 17.9;
            W40S100[2, 2] = 18.1;
            W40S100[2, 3] = 18.4;
            W40S100[2, 4] = 18.9;
            W40S100[2, 5] = 19.1;
            W40S100[2, 6] = 19.1;

            return W40S100[altitudeIndex, temperatureIndex];
        }

        double W40S120(int altitudeIndex, int temperatureIndex)
        {
            double[,] W40S120 = new double[3, 7];

            W40S120[0, 0] = 17.9;
            W40S120[0, 1] = 18.5;
            W40S120[0, 2] = 18.8;
            W40S120[0, 3] = 19.1;
            W40S120[0, 4] = 19.6;
            W40S120[0, 5] = 19.8;
            W40S120[0, 6] = 19.8;

            W40S120[1, 0] = 20.2;
            W40S120[1, 1] = 20.9;
            W40S120[1, 2] = 21.2;
            W40S120[1, 3] = 21.5;
            W40S120[1, 4] = 22.1;
            W40S120[1, 5] = 22.3;
            W40S120[1, 6] = 22.3;

            W40S120[2, 0] = 23.0;
            W40S120[2, 1] = 23.8;
            W40S120[2, 2] = 24.1;
            W40S120[2, 3] = 24.5;
            W40S120[2, 4] = 25.1;
            W40S120[2, 5] = 25.4;
            W40S120[2, 6] = 25.5;

            return W40S120[altitudeIndex, temperatureIndex];
        }

        double W40S140(int altitudeIndex, int temperatureIndex)
        {
            double[,] W40S140 = new double[3, 7];

            W40S140[0, 0] = 22.8;
            W40S140[0, 1] = 23.6;
            W40S140[0, 2] = 23.9;
            W40S140[0, 3] = 24.2;
            W40S140[0, 4] = 24.9;
            W40S140[0, 5] = 25.2;
            W40S140[0, 6] = 25.2;

            W40S140[1, 0] = 25.8;
            W40S140[1, 1] = 26.6;
            W40S140[1, 2] = 27.0;
            W40S140[1, 3] = 27.4;
            W40S140[1, 4] = 28.1;
            W40S140[1, 5] = 28.4;
            W40S140[1, 6] = 28.6;

            W40S140[2, 0] = 29.4;
            W40S140[2, 1] = 30.4;
            W40S140[2, 2] = 30.8;
            W40S140[2, 3] = 31.3;
            W40S140[2, 4] = 32.1;
            W40S140[2, 5] = 32.5;
            W40S140[2, 6] = 32.7;

            return W40S140[altitudeIndex, temperatureIndex];
        }

        double W40S160(int altitudeIndex, int temperatureIndex)
        {
            double[,] W40S160 = new double[3, 7];

            W40S160[0, 0] = 28.1;
            W40S160[0, 1] = 29.0;
            W40S160[0, 2] = 29.4;
            W40S160[0, 3] = 29.8;
            W40S160[0, 4] = 30.6;
            W40S160[0, 5] = 31.0;
            W40S160[0, 6] = 31.1;

            W40S160[1, 0] = 31.8;
            W40S160[1, 1] = 32.8;
            W40S160[1, 2] = 33.3;
            W40S160[1, 3] = 33.8;
            W40S160[1, 4] = 34.6;
            W40S160[1, 5] = 35.1;
            W40S160[1, 6] = 35.3;

            W40S160[2, 0] = 36.4;
            W40S160[2, 1] = 37.6;
            W40S160[2, 2] = 38.2;
            W40S160[2, 3] = 38.7;
            W40S160[2, 4] = 39.7;
            W40S160[2, 5] = 40.2;
            W40S160[2, 6] = 40.6;

            return W40S160[altitudeIndex, temperatureIndex];
        }

        double W40S180(int altitudeIndex, int temperatureIndex)
        {
            double[,] W40S180 = new double[3, 7];

            W40S180[0, 0] = 33.7;
            W40S180[0, 1] = 34.8;
            W40S180[0, 2] = 35.3;
            W40S180[0, 3] = 35.8;
            W40S180[0, 4] = 36.7;
            W40S180[0, 5] = 37.2;
            W40S180[0, 6] = 37.5;

            W40S180[1, 0] = 38.2;
            W40S180[1, 1] = 39.5;
            W40S180[1, 2] = 40.0;
            W40S180[1, 3] = 40.6;
            W40S180[1, 4] = 41.6;
            W40S180[1, 5] = 42.2;
            W40S180[1, 6] = 42.6;

            W40S180[2, 0] = 43.9;
            W40S180[2, 1] = 45.4;
            W40S180[2, 2] = 46.0;
            W40S180[2, 3] = 46.6;
            W40S180[2, 4] = 47.8;
            W40S180[2, 5] = 48.6;
            W40S180[2, 6] = 49.1;

            return W40S180[altitudeIndex, temperatureIndex];
        }

        double W50S80(int altitudeIndex, int temperatureIndex)
        {
            double[,] W50S80 = new double[3, 7];

            W50S80[0, 0] = 11.0;
            W50S80[0, 1] = 11.3;
            W50S80[0, 2] = 11.5;
            W50S80[0, 3] = 11.6;
            W50S80[0, 4] = 11.9;
            W50S80[0, 5] = 12.1;
            W50S80[0, 6] = 12.0;

            W50S80[1, 0] = 12.3;
            W50S80[1, 1] = 12.7;
            W50S80[1, 2] = 12.9;
            W50S80[1, 3] = 13.1;
            W50S80[1, 4] = 13.4;
            W50S80[1, 5] = 13.6;
            W50S80[1, 6] = 13.9;

            W50S80[2, 0] = 14.0;
            W50S80[2, 1] = 14.4;
            W50S80[2, 2] = 14.7;
            W50S80[2, 3] = 14.9;
            W50S80[2, 4] = 15.2;
            W50S80[2, 5] = 15.4;
            W50S80[2, 6] = 15.4;

            return W50S80[altitudeIndex, temperatureIndex];
        }

        double W50S100(int altitudeIndex, int temperatureIndex)
        {
            double[,] W50S100 = new double[3, 7];

            W50S100[0, 0] = 15.7;
            W50S100[0, 1] = 16.3;
            W50S100[0, 2] = 16.5;
            W50S100[0, 3] = 16.7;
            W50S100[0, 4] = 17.2;
            W50S100[0, 5] = 17.3;
            W50S100[0, 6] = 17.3;

            W50S100[1, 0] = 17.7;
            W50S100[1, 1] = 18.3;
            W50S100[1, 2] = 18.6;
            W50S100[1, 3] = 18.9;
            W50S100[1, 4] = 19.3;
            W50S100[1, 5] = 19.5;
            W50S100[1, 6] = 19.6;

            W50S100[2, 0] = 20.2;
            W50S100[2, 1] = 20.8;
            W50S100[2, 2] = 21.1;
            W50S100[2, 3] = 21.4;
            W50S100[2, 4] = 22.0;
            W50S100[2, 5] = 22.2;
            W50S100[2, 6] = 22.3;

            return W50S100[altitudeIndex, temperatureIndex];
        }

        double W50S120(int altitudeIndex, int temperatureIndex)
        {
            double[,] W50S120 = new double[3, 7];

            W50S120[0, 0] = 21.2;
            W50S120[0, 1] = 21.9;
            W50S120[0, 2] = 22.2;
            W50S120[0, 3] = 22.5;
            W50S120[0, 4] = 23.1;
            W50S120[0, 5] = 23.4;
            W50S120[0, 6] = 23.4;

            W50S120[1, 0] = 23.9;
            W50S120[1, 1] = 24.7;
            W50S120[1, 2] = 25.1;
            W50S120[1, 3] = 25.4;
            W50S120[1, 4] = 26.1;
            W50S120[1, 5] = 26.4;
            W50S120[1, 6] = 26.5;

            W50S120[2, 0] = 27.3;
            W50S120[2, 1] = 28.2;
            W50S120[2, 2] = 28.6;
            W50S120[2, 3] = 29.0;
            W50S120[2, 4] = 29.7;
            W50S120[2, 5] = 30.1;
            W50S120[2, 6] = 30.3;

            return W50S120[altitudeIndex, temperatureIndex];
        }

        double W50S140(int altitudeIndex, int temperatureIndex)
        {
            double[,] W50S140 = new double[3, 7];

            W50S140[0, 0] = 27.2;
            W50S140[0, 1] = 28.1;
            W50S140[0, 2] = 28.6;
            W50S140[0, 3] = 28.9;
            W50S140[0, 4] = 29.7;
            W50S140[0, 5] = 30.1;
            W50S140[0, 6] = 30.2;

            W50S140[1, 0] = 30.8;
            W50S140[1, 1] = 31.8;
            W50S140[1, 2] = 32.3;
            W50S140[1, 3] = 32.8;
            W50S140[1, 4] = 33.6;
            W50S140[1, 5] = 34.0;
            W50S140[1, 6] = 34.2;

            W50S140[2, 0] = 35.3;
            W50S140[2, 1] = 36.5;
            W50S140[2, 2] = 37.0;
            W50S140[2, 3] = 37.5;
            W50S140[2, 4] = 38.4;
            W50S140[2, 5] = 39.0;
            W50S140[2, 6] = 39.3;

            return W50S140[altitudeIndex, temperatureIndex];
        }

        double W50S160(int altitudeIndex, int temperatureIndex)
        {
            double[,] W50S160 = new double[3, 7];

            W50S160[0, 0] = 33.8;
            W50S160[0, 1] = 34.9;
            W50S160[0, 2] = 35.4;
            W50S160[0, 3] = 35.9;
            W50S160[0, 4] = 36.8;
            W50S160[0, 5] = 37.4;
            W50S160[0, 6] = 37.6;

            W50S160[1, 0] = 38.3;
            W50S160[1, 1] = 39.6;
            W50S160[1, 2] = 40.2;
            W50S160[1, 3] = 40.7;
            W50S160[1, 4] = 41.8;
            W50S160[1, 5] = 42.4;
            W50S160[1, 6] = 42.8;

            W50S160[2, 0] = 44.1;
            W50S160[2, 1] = 45.5;
            W50S160[2, 2] = 46.2;
            W50S160[2, 3] = 46.8;
            W50S160[2, 4] = 48.0;
            W50S160[2, 5] = 48.8;
            W50S160[2, 6] = 49.3;

            return W50S160[altitudeIndex, temperatureIndex];
        }

        double W50S180(int altitudeIndex, int temperatureIndex)
        {
            double[,] W50S180 = new double[3, 7];

            W50S180[0, 0] = 40.9;
            W50S180[0, 1] = 42.2;
            W50S180[0, 2] = 42.8;
            W50S180[0, 3] = 43.4;
            W50S180[0, 4] = 44.5;
            W50S180[0, 5] = 45.2;
            W50S180[0, 6] = 45.7;

            W50S180[1, 0] = 46.4;
            W50S180[1, 1] = 48.0;
            W50S180[1, 2] = 48.7;
            W50S180[1, 3] = 49.3;
            W50S180[1, 4] = 50.6;
            W50S180[1, 5] = 51.4;
            W50S180[1, 6] = 52.1;

            W50S180[2, 0] = 53.6;
            W50S180[2, 1] = 55.4;
            W50S180[2, 2] = 56.2;
            W50S180[2, 3] = 56.9;
            W50S180[2, 4] = 58.4;
            W50S180[2, 5] = 59.4;
            W50S180[2, 6] = 60.3;

            return W50S180[altitudeIndex, temperatureIndex];
        }

        double W60S80(int altitudeIndex, int temperatureIndex)
        {
            double[,] W60S80 = new double[3, 7];

            W60S80[0, 0] = 12.3;
            W60S80[0, 1] = 12.7;
            W60S80[0, 2] = 12.9;
            W60S80[0, 3] = 13.1;
            W60S80[0, 4] = 13.4;
            W60S80[0, 5] = 13.6;
            W60S80[0, 6] = 13.5;

            W60S80[1, 0] = 13.9;
            W60S80[1, 1] = 14.3;
            W60S80[1, 2] = 14.6;
            W60S80[1, 3] = 14.8;
            W60S80[1, 4] = 15.1;
            W60S80[1, 5] = 15.3;
            W60S80[1, 6] = 15.3;

            W60S80[2, 0] = 15.7;
            W60S80[2, 1] = 16.3;
            W60S80[2, 2] = 16.5;
            W60S80[2, 3] = 16.7;
            W60S80[2, 4] = 17.2;
            W60S80[2, 5] = 17.3;
            W60S80[2, 6] = 17.3;

            return W60S80[altitudeIndex, temperatureIndex];
        }

        double W60S100(int altitudeIndex, int temperatureIndex)
        {
            double[,] W60S100 = new double[3, 7];

            W60S100[0, 0] = 18.0;
            W60S100[0, 1] = 18.5;
            W60S100[0, 2] = 18.8;
            W60S100[0, 3] = 19.1;
            W60S100[0, 4] = 19.6;
            W60S100[0, 5] = 19.8;
            W60S100[0, 6] = 19.8;

            W60S100[1, 0] = 20.3;
            W60S100[1, 1] = 20.9;
            W60S100[1, 2] = 21.2;
            W60S100[1, 3] = 21.5;
            W60S100[1, 4] = 22.1;
            W60S100[1, 5] = 22.3;
            W60S100[1, 6] = 22.4;

            W60S100[2, 0] = 23.1;
            W60S100[2, 1] = 23.8;
            W60S100[2, 2] = 24.2;
            W60S100[2, 3] = 24.5;
            W60S100[2, 4] = 25.1;
            W60S100[2, 5] = 25.4;
            W60S100[2, 6] = 25.5;

            return W60S100[altitudeIndex, temperatureIndex];
        }

        double W60S120(int altitudeIndex, int temperatureIndex)
        {
            double[,] W60S120 = new double[3, 7];

            W60S120[0, 0] = 24.4;
            W60S120[0, 1] = 25.2;
            W60S120[0, 2] = 25.6;
            W60S120[0, 3] = 26.0;
            W60S120[0, 4] = 26.6;
            W60S120[0, 5] = 26.9;
            W60S120[0, 6] = 27.0;

            W60S120[1, 0] = 27.6;
            W60S120[1, 1] = 28.5;
            W60S120[1, 2] = 29.0;
            W60S120[1, 3] = 29.4;
            W60S120[1, 4] = 30.1;
            W60S120[1, 5] = 30.5;
            W60S120[1, 6] = 30.6;

            W60S120[2, 0] = 31.6;
            W60S120[2, 1] = 32.6;
            W60S120[2, 2] = 33.1;
            W60S120[2, 3] = 33.5;
            W60S120[2, 4] = 34.4;
            W60S120[2, 5] = 34.9;
            W60S120[2, 6] = 35.1;

            return W60S120[altitudeIndex, temperatureIndex];
        }

        double W60S140(int altitudeIndex, int temperatureIndex)
        {
            double[,] W60S140 = new double[3, 7];

            W60S140[0, 0] = 31.7;
            W60S140[0, 1] = 32.7;
            W60S140[0, 2] = 33.2;
            W60S140[0, 3] = 33.6;
            W60S140[0, 4] = 34.5;
            W60S140[0, 5] = 35.0;
            W60S140[0, 6] = 35.2;

            W60S140[1, 0] = 35.9;
            W60S140[1, 1] = 37.1;
            W60S140[1, 2] = 37.6;
            W60S140[1, 3] = 38.1;
            W60S140[1, 4] = 39.1;
            W60S140[1, 5] = 39.7;
            W60S140[1, 6] = 40.0;

            W60S140[2, 0] = 41.2;
            W60S140[2, 1] = 42.6;
            W60S140[2, 2] = 43.2;
            W60S140[2, 3] = 43.8;
            W60S140[2, 4] = 44.9;
            W60S140[2, 5] = 45.6;
            W60S140[2, 6] = 46.0;

            return W60S140[altitudeIndex, temperatureIndex];
        }

        double W60S160(int altitudeIndex, int temperatureIndex)
        {
            double[,] W60S160 = new double[3, 7];

            W60S160[0, 0] = 39.6;
            W60S160[0, 1] = 40.9;
            W60S160[0, 2] = 41.5;
            W60S160[0, 3] = 42.0;
            W60S160[0, 4] = 43.1;
            W60S160[0, 5] = 43.8;
            W60S160[0, 6] = 44.2;

            W60S160[1, 0] = 45.0;
            W60S160[1, 1] = 46.5;
            W60S160[1, 2] = 47.1;
            W60S160[1, 3] = 47.8;
            W60S160[1, 4] = 49.0;
            W60S160[1, 5] = 49.8;
            W60S160[1, 6] = 50.4;

            W60S160[2, 0] = 51.8;
            W60S160[2, 1] = 53.6;
            W60S160[2, 2] = 54.4;
            W60S160[2, 3] = 55.1;
            W60S160[2, 4] = 56.5;
            W60S160[2, 5] = 57.5;
            W60S160[2, 6] = 58.3;

            return W60S160[altitudeIndex, temperatureIndex];
        }

        double W60S180(int altitudeIndex, int temperatureIndex)
        {
            double[,] W60S180 = new double[3, 7];

            W60S180[0, 0] = 48.1;
            W60S180[0, 1] = 49.7;
            W60S180[0, 2] = 50.4;
            W60S180[0, 3] = 51.1;
            W60S180[0, 4] = 52.3;
            W60S180[0, 5] = 53.2;
            W60S180[0, 6] = 53.9;

            W60S180[1, 0] = 54.8;
            W60S180[1, 1] = 56.6;
            W60S180[1, 2] = 57.4;
            W60S180[1, 3] = 58.2;
            W60S180[1, 4] = 59.6;
            W60S180[1, 5] = 60.7;
            W60S180[1, 6] = 61.7;

            W60S180[2, 0] = 63.5;
            W60S180[2, 1] = 65.6;
            W60S180[2, 2] = 66.5;
            W60S180[2, 3] = 67.4;
            W60S180[2, 4] = 69.1;
            W60S180[2, 5] = 70.5;
            W60S180[2, 6] = 71.9;

            return W60S180[altitudeIndex, temperatureIndex];
        }

        double W70S80(int altitudeIndex, int temperatureIndex)
        {
            double[,] W70S80 = new double[3, 7];

            W70S80[0, 0] = 13.7;
            W70S80[0, 1] = 14.2;
            W70S80[0, 2] = 14.4;
            W70S80[0, 3] = 14.6;
            W70S80[0, 4] = 14.9;
            W70S80[0, 5] = 15.1;
            W70S80[0, 6] = 15.1;

            W70S80[1, 0] = 15.4;
            W70S80[1, 1] = 15.9;
            W70S80[1, 2] = 16.2;
            W70S80[1, 3] = 16.4;
            W70S80[1, 4] = 16.8;
            W70S80[1, 5] = 17.0;
            W70S80[1, 6] = 17.0;

            W70S80[2, 0] = 17.5;
            W70S80[2, 1] = 18.1;
            W70S80[2, 2] = 18.4;
            W70S80[2, 3] = 18.6;
            W70S80[2, 4] = 19.1;
            W70S80[2, 5] = 19.3;
            W70S80[2, 6] = 19.3;

            return W70S80[altitudeIndex, temperatureIndex];
        }

        double W70S100(int altitudeIndex, int temperatureIndex)
        {
            double[,] W70S100 = new double[3, 7];

            W70S100[0, 0] = 20.2;
            W70S100[0, 1] = 20.8;
            W70S100[0, 2] = 21.1;
            W70S100[0, 3] = 21.4;
            W70S100[0, 4] = 22.0;
            W70S100[0, 5] = 22.2;
            W70S100[0, 6] = 22.3;

            W70S100[1, 0] = 22.8;
            W70S100[1, 1] = 23.5;
            W70S100[1, 2] = 23.9;
            W70S100[1, 3] = 24.2;
            W70S100[1, 4] = 24.8;
            W70S100[1, 5] = 25.1;
            W70S100[1, 6] = 25.2;

            W70S100[2, 0] = 26.0;
            W70S100[2, 1] = 26.8;
            W70S100[2, 2] = 27.2;
            W70S100[2, 3] = 27.6;
            W70S100[2, 4] = 28.3;
            W70S100[2, 5] = 28.6;
            W70S100[2, 6] = 28.8;

            return W70S100[altitudeIndex, temperatureIndex];
        }

        double W70S120(int altitudeIndex, int temperatureIndex)
        {
            double[,] W70S120 = new double[3, 7];

            W70S120[0, 0] = 27.7;
            W70S120[0, 1] = 28.6;
            W70S120[0, 2] = 29.0;
            W70S120[0, 3] = 29.4;
            W70S120[0, 4] = 30.2;
            W70S120[0, 5] = 30.5;
            W70S120[0, 6] = 30.7;

            W70S120[1, 0] = 31.3;
            W70S120[1, 1] = 32.4;
            W70S120[1, 2] = 32.8;
            W70S120[1, 3] = 33.3;
            W70S120[1, 4] = 34.1;
            W70S120[1, 5] = 34.6;
            W70S120[1, 6] = 34.8;

            W70S120[2, 0] = 35.9;
            W70S120[2, 1] = 37.1;
            W70S120[2, 2] = 37.6;
            W70S120[2, 3] = 38.1;
            W70S120[2, 4] = 39.1;
            W70S120[2, 5] = 39.6;
            W70S120[2, 6] = 40.0;

            return W70S120[altitudeIndex, temperatureIndex];
        }

        double W70S140(int altitudeIndex, int temperatureIndex)
        {
            double[,] W70S140 = new double[3, 7];

            W70S140[0, 0] = 36.1;
            W70S140[0, 1] = 37.3;
            W70S140[0, 2] = 37.8;
            W70S140[0, 3] = 38.4;
            W70S140[0, 4] = 39.3;
            W70S140[0, 5] = 39.9;
            W70S140[0, 6] = 40.2;

            W70S140[1, 0] = 41.0;
            W70S140[1, 1] = 42.3;
            W70S140[1, 2] = 43.0;
            W70S140[1, 3] = 43.5;
            W70S140[1, 4] = 44.6;
            W70S140[1, 5] = 45.3;
            W70S140[1, 6] = 45.8;

            W70S140[2, 0] = 47.2;
            W70S140[2, 1] = 48.7;
            W70S140[2, 2] = 49.4;
            W70S140[2, 3] = 50.1;
            W70S140[2, 4] = 51.4;
            W70S140[2, 5] = 52.2;
            W70S140[2, 6] = 52.9;

            return W70S140[altitudeIndex, temperatureIndex];
        }

        double W70S160(int altitudeIndex, int temperatureIndex)
        {
            double[,] W70S160 = new double[3, 7];

            W70S160[0, 0] = 45.3;
            W70S160[0, 1] = 46.8;
            W70S160[0, 2] = 47.5;
            W70S160[0, 3] = 48.1;
            W70S160[0, 4] = 49.3;
            W70S160[0, 5] = 50.1;
            W70S160[0, 6] = 50.7;

            W70S160[1, 0] = 51.6;
            W70S160[1, 1] = 53.3;
            W70S160[1, 2] = 54.0;
            W70S160[1, 3] = 54.8;
            W70S160[1, 4] = 56.1;
            W70S160[1, 5] = 57.1;
            W70S160[1, 6] = 58.0;

            W70S160[2, 0] = 59.7;
            W70S160[2, 1] = 61.6;
            W70S160[2, 2] = 62.5;
            W70S160[2, 3] = 63.4;
            W70S160[2, 4] = 64.9;
            W70S160[2, 5] = 66.2;
            W70S160[2, 6] = 67.4;

            return W70S160[altitudeIndex, temperatureIndex];
        }

        double W70S180(int altitudeIndex, int temperatureIndex)
        {
            double[,] W70S180 = new double[3, 7];

            W70S180[0, 0] = 54.9;
            W70S180[0, 1] = 56.7;
            W70S180[0, 2] = 57.5;
            W70S180[0, 3] = 58.3;
            W70S180[0, 4] = 59.8;
            W70S180[0, 5] = 60.9;
            W70S180[0, 6] = 61.8;

            W70S180[1, 0] = 62.7;
            W70S180[1, 1] = 64.8;
            W70S180[1, 2] = 65.7;
            W70S180[1, 3] = 66.5;
            W70S180[1, 4] = 68.2;
            W70S180[1, 5] = 69.6;
            W70S180[1, 6] = 70.9;

            W70S180[2, 0] = 72.9;
            W70S180[2, 1] = 75.4;
            W70S180[2, 2] = 76.4;
            W70S180[2, 3] = 77.4;
            W70S180[2, 4] = 79.4;
            W70S180[2, 5] = 81.2;
            W70S180[2, 6] = 83.0;

            return W70S180[altitudeIndex, temperatureIndex];
        }

        double W80S80(int altitudeIndex, int temperatureIndex)
        {
            double[,] W80S80 = new double[3, 7];

            W80S80[0, 0] = 15.1;
            W80S80[0, 1] = 15.6;
            W80S80[0, 2] = 15.8;
            W80S80[0, 3] = 16.0;
            W80S80[0, 4] = 16.4;
            W80S80[0, 5] = 16.6;
            W80S80[0, 6] = 16.6;

            W80S80[1, 0] = 17.0;
            W80S80[1, 1] = 17.6;
            W80S80[1, 2] = 17.8;
            W80S80[1, 3] = 18.1;
            W80S80[1, 4] = 18.5;
            W80S80[1, 5] = 18.7;
            W80S80[1, 6] = 18.7;

            W80S80[2, 0] = 19.3;
            W80S80[2, 1] = 20.0;
            W80S80[2, 2] = 20.2;
            W80S80[2, 3] = 20.5;
            W80S80[2, 4] = 21.1;
            W80S80[2, 5] = 21.3;
            W80S80[2, 6] = 21.3;

            return W80S80[altitudeIndex, temperatureIndex];
        }

        double W80S100(int altitudeIndex, int temperatureIndex)
        {
            double[,] W80S100 = new double[3, 7];

            W80S100[0, 0] = 22.4;
            W80S100[0, 1] = 23.1;
            W80S100[0, 2] = 23.5;
            W80S100[0, 3] = 23.8;
            W80S100[0, 4] = 24.4;
            W80S100[0, 5] = 24.7;
            W80S100[0, 6] = 24.8;

            W80S100[1, 0] = 25.3;
            W80S100[1, 1] = 26.1;
            W80S100[1, 2] = 26.5;
            W80S100[1, 3] = 26.9;
            W80S100[1, 4] = 27.6;
            W80S100[1, 5] = 27.9;
            W80S100[1, 6] = 28.0;

            W80S100[2, 0] = 28.9;
            W80S100[2, 1] = 29.8;
            W80S100[2, 2] = 30.3;
            W80S100[2, 3] = 30.7;
            W80S100[2, 4] = 31.5;
            W80S100[2, 5] = 31.9;
            W80S100[2, 6] = 32.1;

            return W80S100[altitudeIndex, temperatureIndex];
        }

        double W80S120(int altitudeIndex, int temperatureIndex)
        {
            double[,] W80S120 = new double[3, 7];

            W80S120[0, 0] = 30.9;
            W80S120[0, 1] = 31.9;
            W80S120[0, 2] = 32.4;
            W80S120[0, 3] = 32.8;
            W80S120[0, 4] = 33.7;
            W80S120[0, 5] = 34.1;
            W80S120[0, 6] = 34.3;

            W80S120[1, 0] = 35.0;
            W80S120[1, 1] = 36.2;
            W80S120[1, 2] = 36.7;
            W80S120[1, 3] = 37.2;
            W80S120[1, 4] = 38.2;
            W80S120[1, 5] = 38.7;
            W80S120[1, 6] = 39.0;

            W80S120[2, 0] = 40.2;
            W80S120[2, 1] = 41.5;
            W80S120[2, 2] = 42.1;
            W80S120[2, 3] = 42.7;
            W80S120[2, 4] = 43.8;
            W80S120[2, 5] = 44.4;
            W80S120[2, 6] = 44.9;

            return W80S120[altitudeIndex, temperatureIndex];
        }

        double W80S140(int altitudeIndex, int temperatureIndex)
        {
            double[,] W80S140 = new double[3, 7];

            W80S140[0, 0] = 40.4;
            W80S140[0, 1] = 41.8;
            W80S140[0, 2] = 42.4;
            W80S140[0, 3] = 42.9;
            W80S140[0, 4] = 44.0;
            W80S140[0, 5] = 44.7;
            W80S140[0, 6] = 45.2;

            W80S140[1, 0] = 45.9;
            W80S140[1, 1] = 47.5;
            W80S140[1, 2] = 48.2;
            W80S140[1, 3] = 48.8;
            W80S140[1, 4] = 50.0;
            W80S140[1, 5] = 50.9;
            W80S140[1, 6] = 51.5;

            W80S140[2, 0] = 53.0;
            W80S140[2, 1] = 54.8;
            W80S140[2, 2] = 55.6;
            W80S140[2, 3] = 56.3;
            W80S140[2, 4] = 57.7;
            W80S140[2, 5] = 58.8;
            W80S140[2, 6] = 59.7;

            return W80S140[altitudeIndex, temperatureIndex];
        }

        double W80S160(int altitudeIndex, int temperatureIndex)
        {
            double[,] W80S160 = new double[3, 7];

            W80S160[0, 0] = 50.8;
            W80S160[0, 1] = 52.5;
            W80S160[0, 2] = 53.3;
            W80S160[0, 3] = 54.0;
            W80S160[0, 4] = 55.3;
            W80S160[0, 5] = 56.3;
            W80S160[0, 6] = 57.1;

            W80S160[1, 0] = 57.9;
            W80S160[1, 1] = 59.9;
            W80S160[1, 2] = 60.7;
            W80S160[1, 3] = 61.5;
            W80S160[1, 4] = 63.1;
            W80S160[1, 5] = 64.3;
            W80S160[1, 6] = 65.4;

            W80S160[2, 0] = 67.3;
            W80S160[2, 1] = 69.5;
            W80S160[2, 2] = 70.5;
            W80S160[2, 3] = 71.4;
            W80S160[2, 4] = 73.2;
            W80S160[2, 5] = 74.8;
            W80S160[2, 6] = 76.3;

            return W80S160[altitudeIndex, temperatureIndex];
        }

        double W80S180(int altitudeIndex, int temperatureIndex)
        {
            double[,] W80S180 = new double[3, 7];

            W80S180[0, 0] = 60.8;
            W80S180[0, 1] = 62.8;
            W80S180[0, 2] = 63.7;
            W80S180[0, 3] = 64.6;
            W80S180[0, 4] = 66.2;
            W80S180[0, 5] = 67.5;
            W80S180[0, 6] = 68.7;

            W80S180[1, 0] = 69.6;
            W80S180[1, 1] = 71.9;
            W80S180[1, 2] = 72.9;
            W80S180[1, 3] = 73.9;
            W80S180[1, 4] = 75.7;
            W80S180[1, 5] = 77.4;
            W80S180[1, 6] = 79.0;

            W80S180[2, 0] = 81.2;
            W80S180[2, 1] = 83.9;
            W80S180[2, 2] = 85.1;
            W80S180[2, 3] = 86.2;
            W80S180[2, 4] = 88.4;
            W80S180[2, 5] = 90.5;
            W80S180[2, 6] = 92.9;

            return W80S180[altitudeIndex, temperatureIndex];
        }

        double NoReverseThrustEventAdjustedBrakeEnergy(int referenceBrakeEnergyIndex, int eventIndex)
        {
            double[,] EventAdjustedBrakeEnergy = new double[9, 6];

            EventAdjustedBrakeEnergy[0, 0] = 6.7;
            EventAdjustedBrakeEnergy[0, 1] = 7.0;
            EventAdjustedBrakeEnergy[0, 2] = 7.3;
            EventAdjustedBrakeEnergy[0, 3] = 7.5;
            EventAdjustedBrakeEnergy[0, 4] = 7.8;
            EventAdjustedBrakeEnergy[0, 5] = 10.0;

            EventAdjustedBrakeEnergy[1, 0] = 13.1;
            EventAdjustedBrakeEnergy[1, 1] = 13.8;
            EventAdjustedBrakeEnergy[1, 2] = 14.7;
            EventAdjustedBrakeEnergy[1, 3] = 15.4;
            EventAdjustedBrakeEnergy[1, 4] = 16.3;
            EventAdjustedBrakeEnergy[1, 5] = 20.0;

            EventAdjustedBrakeEnergy[2, 0] = 19.2;
            EventAdjustedBrakeEnergy[2, 1] = 20.5;
            EventAdjustedBrakeEnergy[2, 2] = 22.3;
            EventAdjustedBrakeEnergy[2, 3] = 23.6;
            EventAdjustedBrakeEnergy[2, 4] = 25.3;
            EventAdjustedBrakeEnergy[2, 5] = 30.0;

            EventAdjustedBrakeEnergy[3, 0] = 25.3;
            EventAdjustedBrakeEnergy[3, 1] = 27.4;
            EventAdjustedBrakeEnergy[3, 2] = 30.2;
            EventAdjustedBrakeEnergy[3, 3] = 32.4;
            EventAdjustedBrakeEnergy[3, 4] = 34.7;
            EventAdjustedBrakeEnergy[3, 5] = 40.0;

            EventAdjustedBrakeEnergy[4, 0] = 31.8;
            EventAdjustedBrakeEnergy[4, 1] = 34.8;
            EventAdjustedBrakeEnergy[4, 2] = 38.6;
            EventAdjustedBrakeEnergy[4, 3] = 41.8;
            EventAdjustedBrakeEnergy[4, 4] = 44.7;
            EventAdjustedBrakeEnergy[4, 5] = 50.0;

            EventAdjustedBrakeEnergy[5, 0] = 38.8;
            EventAdjustedBrakeEnergy[5, 1] = 42.7;
            EventAdjustedBrakeEnergy[5, 2] = 47.6;
            EventAdjustedBrakeEnergy[5, 3] = 51.8;
            EventAdjustedBrakeEnergy[5, 4] = 55.0;
            EventAdjustedBrakeEnergy[5, 5] = 60.0;

            EventAdjustedBrakeEnergy[6, 0] = 46.6;
            EventAdjustedBrakeEnergy[6, 1] = 51.5;
            EventAdjustedBrakeEnergy[6, 2] = 57.4;
            EventAdjustedBrakeEnergy[6, 3] = 62.5;
            EventAdjustedBrakeEnergy[6, 4] = 65.7;
            EventAdjustedBrakeEnergy[6, 5] = 70.0;

            EventAdjustedBrakeEnergy[7, 0] = 55.4;
            EventAdjustedBrakeEnergy[7, 1] = 61.3;
            EventAdjustedBrakeEnergy[7, 2] = 68.1;
            EventAdjustedBrakeEnergy[7, 3] = 74.1;
            EventAdjustedBrakeEnergy[7, 4] = 76.6;
            EventAdjustedBrakeEnergy[7, 5] = 80.0;

            EventAdjustedBrakeEnergy[8, 0] = 65.5;
            EventAdjustedBrakeEnergy[8, 1] = 72.4;
            EventAdjustedBrakeEnergy[8, 2] = 80.0;
            EventAdjustedBrakeEnergy[8, 3] = 86.5;
            EventAdjustedBrakeEnergy[8, 4] = 87.9;
            EventAdjustedBrakeEnergy[8, 5] = 90.0;

            return EventAdjustedBrakeEnergy[referenceBrakeEnergyIndex, eventIndex];
        }

        double TwoEngineDetentReverseThrustEventAdjustedBrakeEnergy(int referenceBrakeEnergyIndex, int eventIndex)
        {
            double[,] EventAdjustedBrakeEnergy = new double[9, 6];
            EventAdjustedBrakeEnergy[0, 0] = 1.8;
            EventAdjustedBrakeEnergy[0, 1] = 2.5;
            EventAdjustedBrakeEnergy[0, 2] = 4.3;
            EventAdjustedBrakeEnergy[0, 3] = 5.8;
            EventAdjustedBrakeEnergy[0, 4] = 7.0;
            EventAdjustedBrakeEnergy[0, 5] = 10.0;

            EventAdjustedBrakeEnergy[1, 0] = 3.8;
            EventAdjustedBrakeEnergy[1, 1] = 5.6;
            EventAdjustedBrakeEnergy[1, 2] = 9.2;
            EventAdjustedBrakeEnergy[1, 3] = 12.3;
            EventAdjustedBrakeEnergy[1, 4] = 14.6;
            EventAdjustedBrakeEnergy[1, 5] = 20.0;

            EventAdjustedBrakeEnergy[2, 0] = 6.1;
            EventAdjustedBrakeEnergy[2, 1] = 9.1;
            EventAdjustedBrakeEnergy[2, 2] = 14.7;
            EventAdjustedBrakeEnergy[2, 3] = 19.5;
            EventAdjustedBrakeEnergy[2, 4] = 22.8;
            EventAdjustedBrakeEnergy[2, 5] = 30.0;

            EventAdjustedBrakeEnergy[3, 0] = 8.8;
            EventAdjustedBrakeEnergy[3, 1] = 13.1;
            EventAdjustedBrakeEnergy[3, 2] = 20.7;
            EventAdjustedBrakeEnergy[3, 3] = 27.2;
            EventAdjustedBrakeEnergy[3, 4] = 31.4;
            EventAdjustedBrakeEnergy[3, 5] = 40.0;

            EventAdjustedBrakeEnergy[4, 0] = 11.9;
            EventAdjustedBrakeEnergy[4, 1] = 17.8;
            EventAdjustedBrakeEnergy[4, 2] = 27.2;
            EventAdjustedBrakeEnergy[4, 3] = 35.6;
            EventAdjustedBrakeEnergy[4, 4] = 40.5;
            EventAdjustedBrakeEnergy[4, 5] = 50.0;

            EventAdjustedBrakeEnergy[5, 0] = 15.5;
            EventAdjustedBrakeEnergy[5, 1] = 23.0;
            EventAdjustedBrakeEnergy[5, 2] = 34.4;
            EventAdjustedBrakeEnergy[5, 3] = 44.5;
            EventAdjustedBrakeEnergy[5, 4] = 49.9;
            EventAdjustedBrakeEnergy[5, 5] = 60.0;

            EventAdjustedBrakeEnergy[6, 0] = 19.6;
            EventAdjustedBrakeEnergy[6, 1] = 28.8;
            EventAdjustedBrakeEnergy[6, 2] = 42.0;
            EventAdjustedBrakeEnergy[6, 3] = 53.9;
            EventAdjustedBrakeEnergy[6, 4] = 59.7;
            EventAdjustedBrakeEnergy[6, 5] = 70.0;

            EventAdjustedBrakeEnergy[7, 0] = 24.4;
            EventAdjustedBrakeEnergy[7, 1] = 35.2;
            EventAdjustedBrakeEnergy[7, 2] = 50.2;
            EventAdjustedBrakeEnergy[7, 3] = 63.7;
            EventAdjustedBrakeEnergy[7, 4] = 69.8;
            EventAdjustedBrakeEnergy[7, 5] = 80.0;

            EventAdjustedBrakeEnergy[8, 0] = 29.8;
            EventAdjustedBrakeEnergy[8, 1] = 42.3;
            EventAdjustedBrakeEnergy[8, 2] = 59.0;
            EventAdjustedBrakeEnergy[8, 3] = 74.1;
            EventAdjustedBrakeEnergy[8, 4] = 80.0;
            EventAdjustedBrakeEnergy[8, 5] = 90.0;

            return EventAdjustedBrakeEnergy[referenceBrakeEnergyIndex, eventIndex];
        }
        #endregion

        private void OnErrorMessageEvent(ErrorMessageEventArgs eventArgs)
        {
            ErrorMessageEvent?.Invoke(this, eventArgs);
        }
    }
}