using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using CommonServiceLocator;
using System;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelOrderDistributionFragment : Fragment
    {
        IFuelDistributionService _fuelDistributionService;
        ISettingsService _settingsService;
        IEventService _eventService;
        string _massUnit;
        string _densityUnit;
        string _totalFuel;
        int _progress;
        TextView
            _fuelQuantity1TextView,
            _fuelQuantity2TextView,
            _maximumFuelQuantity1TextView,
            _maximumFuelQuantity2TextView,
            _fuelQuantityCenterTextView,
            _maximumFuelQuantityCenterTextView,
            _maximumTotalFuelQuantityTextView,
            _fuelDensityTextView;
        EditText _totalFuelEditText;
        SeekBar _fuelDensitySeekBar;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _fuelDistributionService = ServiceLocator.Current.GetInstance<IFuelDistributionService>("FuelDistributionService");
            _eventService = ServiceLocator.Current.GetInstance<IEventService>("EventService");

            _massUnit = _settingsService.GetSetting<bool>("UseImperialMass") ? "lbs" : "kg";
            _densityUnit = _settingsService.GetSetting<bool>("UseImperialDensity") ? "lbs/USG" : "kg/ltr";
            _totalFuel = _settingsService.GetSetting<string>("TotalFuel");
            _progress = _settingsService.GetSetting<int>("FuelDensityProgress") == int.MinValue ? 43 : _settingsService.GetSetting<int>("FuelDensityProgress");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fuelorder_distribution_fragment, container, false);

            _fuelQuantity1TextView = view.FindViewById<TextView>(Resource.Id.fuelQuantity1TextView);
            _fuelQuantity2TextView = view.FindViewById<TextView>(Resource.Id.fuelQuantity2TextView);
            view.FindViewById<TextView>(Resource.Id.tank1MassUnitTextView).Text = _massUnit;
            view.FindViewById<TextView>(Resource.Id.tank2MassUnitTextView).Text = _massUnit;
            _maximumFuelQuantity1TextView = view.FindViewById<TextView>(Resource.Id.maximumFuelQuantity1TextView);
            _maximumFuelQuantity2TextView = view.FindViewById<TextView>(Resource.Id.maximumFuelQuantity2TextView);
            _fuelQuantityCenterTextView = view.FindViewById<TextView>(Resource.Id.centerFuelQuantityTextView);
            view.FindViewById<TextView>(Resource.Id.centerTankMassUnitTextView).Text = _massUnit;
            _maximumFuelQuantityCenterTextView = view.FindViewById<TextView>(Resource.Id.maximumCenterFuelQuantityTextView);
            view.FindViewById<TextView>(Resource.Id.totalFuelMassUnitTextView).Text = _massUnit;
            _maximumTotalFuelQuantityTextView = view.FindViewById<TextView>(Resource.Id.maximumTotalFuelQuantityTextView);
            _fuelDensityTextView = view.FindViewById<TextView>(Resource.Id.fuelDensityTextView);
            view.FindViewById<TextView>(Resource.Id.fuelDensityUnitTextView).Text = _densityUnit;
            _totalFuelEditText = view.FindViewById<EditText>(Resource.Id.totalFuelEditText);
            _fuelDensitySeekBar = view.FindViewById<SeekBar>(Resource.Id.fuelDensitySeekBar);

            _totalFuelEditText.TextChanged += (s, e) =>
            {
                _fuelDistributionService.CalculateFuelDistribution(e.Text.ToString(), _massUnit.Equals("lbs"), _densityUnit.Equals("lbs/USG"), _fuelDensitySeekBar.Progress);
                _settingsService.SaveSetting("TotalFuel", e.Text.ToString());
                SetValues();
            };

            _fuelDensitySeekBar.Progress = _progress;
            _fuelDensityTextView.Text = ProgressToDensity(_progress);
            _fuelDistributionService.CalculateMaximumFuel(_massUnit.Equals("lbs"), _densityUnit.Equals("lbs/USG"), _fuelDensitySeekBar.Progress);
            SetValues();

            if (!String.IsNullOrEmpty(_totalFuel) || String.IsNullOrWhiteSpace(_totalFuel)) _totalFuelEditText.Text = _totalFuel;

            _fuelDensitySeekBar.ProgressChanged += (s, e) =>
            {
                _fuelDensityTextView.Text = ProgressToDensity(e.Progress);
                _fuelDistributionService.CalculateMaximumFuel(_massUnit.Equals("lbs"), _densityUnit.Equals("lbs/USG"), _fuelDensitySeekBar.Progress);
                _fuelDistributionService.CalculateFuelDistribution(_totalFuelEditText.Text, _massUnit.Equals("lbs"), _densityUnit.Equals("lbs/USG"), e.Progress);
                _settingsService.SaveSetting("FuelDensityProgress", e.Progress);
                _eventService.OnProgressChangedEvent(new ProgressChangedEventArgs(e.Progress));
                SetValues();
            };

            view.FindViewById<Button>(Resource.Id.clearTotalFuelButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                //view.PlaySoundEffect(SoundEffects.Click);
                _totalFuelEditText.Text = "";
                _settingsService.SaveSetting("FuelQuantityWing", "");
                _settingsService.SaveSetting("FuelQuantityCenter", "");
            };

            _fuelDistributionService.ErrorMessageEvent += (s, e) =>
            {
                AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(Context);
                AlertDialog alertDialog = alertDialogBuilder.Create();
                alertDialog.SetTitle("Error");
                alertDialog.SetIcon(Resource.Drawable.ic_stat_error_outline);
                alertDialog.SetMessage((e as ErrorMessageEventArgs).ErrorMessage);
                alertDialog.SetButton((int)DialogButtonType.Positive, "OK", (se, ev) =>
                 {
                     _totalFuelEditText.Text = "";
                     _settingsService.SaveSetting("TotalFuel", "");
                 });
                alertDialog.Show();
            };

            return view;
        }

        private void SetValues()
        {
            _fuelQuantity1TextView.Text = _fuelDistributionService.FuelQuantityWing;
            _fuelQuantity2TextView.Text = _fuelDistributionService.FuelQuantityWing;
            _maximumFuelQuantity1TextView.Text = _fuelDistributionService.MaximumFuelQuantityWing + _massUnit;
            _maximumFuelQuantity2TextView.Text = _fuelDistributionService.MaximumFuelQuantityWing + _massUnit;
            _fuelQuantityCenterTextView.Text = _fuelDistributionService.FuelQuantityCenter;
            _maximumFuelQuantityCenterTextView.Text = _fuelDistributionService.MaximumFuelQuantityCenter + _massUnit;
            _maximumTotalFuelQuantityTextView.Text = _fuelDistributionService.MaximumFuelQuantityTotal + _massUnit;
            _settingsService.SaveSetting("FuelQuantityWing", _fuelDistributionService.FuelQuantityWing);
            _settingsService.SaveSetting("FuelQuantityCenter", _fuelDistributionService.FuelQuantityCenter);
        }

        private string ProgressToDensity(int progress)
        {
            if (_densityUnit.Equals("kg/ltr")) return Math.Round((775 + (double)progress * (100 / 65)) / 1000, 3).ToString();
            else return Math.Round((775 + (double)progress * (100 / 65)) * 8.345404452 / 1000, 3).ToString();
        }

        public static FuelOrderDistributionFragment NewInstance()
        {
            var fuelOrderDistributionFragment = new FuelOrderDistributionFragment { Arguments = new Bundle() };
            return fuelOrderDistributionFragment;
        }
    }
}