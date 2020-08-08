using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using CommonServiceLocator;
using System;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelOrderUpliftFragment : Android.Support.V4.App.Fragment
    {
        IFuelUpliftService _fuelUpliftService;
        ISettingsService _settingsService;
        IEventService _eventService;
        string _massUnit;
        string _volumeUnit;
        double _fuelDensity;
        TextView
            _fuelUpliftDifferenceTextView,
            _calculatedFuelUpliftTextView,
            _expectedFuelUpliftTextView;
        EditText
            _remainingFuelEditText,
            _calculatedBlockFuelEditText,
            _actualBlockFuelEditText,
            _actualFuelUpliftEditText;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _fuelUpliftService = ServiceLocator.Current.GetInstance<IFuelUpliftService>("FuelUpliftService");
            _eventService = ServiceLocator.Current.GetInstance<IEventService>("EventService");
            _massUnit = _settingsService.GetSetting<bool>("UseImperialMass") ? "lbs" : "kg";
            _volumeUnit = _settingsService.GetSetting<bool>("UseImperialDensity") ? "USG" : "ltr";
            _fuelDensity = ProgressToDensity(_settingsService.GetSetting<int>("FuelDensityProgress") == int.MinValue ? 43 : _settingsService.GetSetting<int>("FuelDensityProgress"));
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fuelorder_uplift_fragment, container, false);

            view.FindViewById<TextView>(Resource.Id.remainingFuelMassUnitTextView).Text = _massUnit;
            view.FindViewById<TextView>(Resource.Id.calculatedBlockFuelMassUnitTextView).Text = _massUnit;
            _fuelUpliftDifferenceTextView = view.FindViewById<TextView>(Resource.Id.fuelUpliftDifferenceTextView);
            _calculatedFuelUpliftTextView = view.FindViewById<TextView>(Resource.Id.calculatedFuelUpliftTextView);
            view.FindViewById<TextView>(Resource.Id.calculatedUpliftVolumeUnitTextView).Text = _volumeUnit;
            view.FindViewById<TextView>(Resource.Id.actualBlockFuelMassUnitTextView).Text = _massUnit;
            _expectedFuelUpliftTextView = view.FindViewById<TextView>(Resource.Id.expectedFuelUpliftTextView);
            view.FindViewById<TextView>(Resource.Id.expectedFuelUpliftVolumeUnitTextView).Text = _volumeUnit;
            view.FindViewById<TextView>(Resource.Id.actualFuelUpliftVolumeUnitTextView).Text = _volumeUnit;
            _remainingFuelEditText = view.FindViewById<EditText>(Resource.Id.remainingFuelEditText);
            _calculatedBlockFuelEditText = view.FindViewById<EditText>(Resource.Id.calculatedBlockFuelEditText);
            _actualBlockFuelEditText = view.FindViewById<EditText>(Resource.Id.actualBlockFuelEditText);
            _actualFuelUpliftEditText = view.FindViewById<EditText>(Resource.Id.actualFuelUpliftEditText);

            _remainingFuelEditText.Text = _settingsService.GetSetting<string>("RemainingFuel");
            _remainingFuelEditText.TextChanged += (s, e) =>
            {
                _settingsService.SaveSetting("RemainingFuel", e.Text.ToString());
            };

            _calculatedBlockFuelEditText.TextChanged += (s, e) =>
            {
                _fuelUpliftService.CalculateFuelUplift(_remainingFuelEditText.Text, e.Text.ToString(), _massUnit.Equals("lbs"), _volumeUnit.Equals("USG"), _fuelDensity);
                _calculatedFuelUpliftTextView.Text = _fuelUpliftService.CalculatedFuelUplift;
                _settingsService.SaveSetting("CalculatedBlockFuel", e.Text.ToString());
            };
            _calculatedBlockFuelEditText.Text = _settingsService.GetSetting<string>("CalculatedBlockFuel");


            _actualBlockFuelEditText.TextChanged += (s, e) =>
            {
                _fuelUpliftService.CalculateExpectedFuelUplift(_remainingFuelEditText.Text, e.Text.ToString(), _massUnit.Equals("lbs"), _volumeUnit.Equals("USG"), _fuelDensity);
                _expectedFuelUpliftTextView.Text = _fuelUpliftService.ExpectedFuelUplift;
                _settingsService.SaveSetting("ActualBlockFuel", e.Text.ToString());
            };
            _actualBlockFuelEditText.Text = _settingsService.GetSetting<string>("ActualBlockFuel");


            _actualFuelUpliftEditText.TextChanged += (s, e) =>
            {
                _fuelUpliftService.CalculateFuelUpliftDifference(_remainingFuelEditText.Text, _actualBlockFuelEditText.Text, e.Text.ToString(), _massUnit.Equals("lbs"), _volumeUnit.Equals("USG"), _fuelDensity);
                _fuelUpliftDifferenceTextView.Text = _fuelUpliftService.FuelUpliftDifference;
                _settingsService.SaveSetting("ActualFuelUplift", e.Text.ToString());
            };
            _actualFuelUpliftEditText.Text = _settingsService.GetSetting<string>("ActualFuelUplift");

            view.FindViewById<Button>(Resource.Id.clearFuelUpliftButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                //view.PlaySoundEffect(SoundEffects.Click);
                _remainingFuelEditText.Text = "";
                _calculatedBlockFuelEditText.Text = "";
                _actualBlockFuelEditText.Text = "";
                _actualFuelUpliftEditText.Text = "";
                _expectedFuelUpliftTextView.Text = "";
                _calculatedFuelUpliftTextView.Text = "";
                _fuelUpliftDifferenceTextView.Text = "";
            };

            _fuelUpliftService.ErrorMessageEvent += (s, e) =>
            {
                AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(Context);
                AlertDialog alertDialog = alertDialogBuilder.Create();
                alertDialog.SetTitle("Error");
                alertDialog.SetIcon(Resource.Drawable.ic_stat_error_outline);
                alertDialog.SetMessage("Numbers Only");
                alertDialog.SetButton("OK", (se, ev) => { });
                alertDialog.Show();
                return;
            };

            _eventService.ProgressChangedEvent += (s, e) =>
            {
                _fuelUpliftService.CalculateFuelUplift(_remainingFuelEditText.Text, _calculatedBlockFuelEditText.Text, _massUnit.Equals("lbs"), _volumeUnit.Equals("USG"), ProgressToDensity((e as ProgressChangedEventArgs).Progress));
                _fuelUpliftService.CalculateExpectedFuelUplift(_remainingFuelEditText.Text, _actualBlockFuelEditText.Text, _massUnit.Equals("lbs"), _volumeUnit.Equals("USG"), ProgressToDensity((e as ProgressChangedEventArgs).Progress));
                _fuelUpliftService.CalculateFuelUpliftDifference(_remainingFuelEditText.Text, _actualBlockFuelEditText.Text, _actualFuelUpliftEditText.Text, _massUnit.Equals("lbs"), _volumeUnit.Equals("USG"), ProgressToDensity((e as ProgressChangedEventArgs).Progress));
                _expectedFuelUpliftTextView.Text = _fuelUpliftService.ExpectedFuelUplift;
                _calculatedFuelUpliftTextView.Text = _fuelUpliftService.CalculatedFuelUplift;
                _fuelUpliftDifferenceTextView.Text = _fuelUpliftService.FuelUpliftDifference;
            };

            return view;
        }

        public static FuelOrderUpliftFragment NewInstance()
        {
            var fuelOrderUpliftFragment = new FuelOrderUpliftFragment { Arguments = new Bundle() };
            return fuelOrderUpliftFragment;
        }

        private double ProgressToDensity(int progress)
        {
            if (_volumeUnit.Equals("ltr")) return Math.Round((775 + (double)progress * (100 / 65)) / 1000, 3);
            else return Math.Round((775 + (double)progress * (100 / 65)) * 8.345404452 / 1000, 3);
        }
    }
}