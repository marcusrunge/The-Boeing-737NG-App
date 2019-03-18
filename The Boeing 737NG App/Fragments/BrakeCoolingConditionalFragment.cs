using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CommonServiceLocator;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class BrakeCoolingConditionalFragment : Android.Support.V4.App.Fragment
    {
        ISettingsService _settingsService;
        IBrakeCoolingService _brakeCoolingService;
        string _massUnit;
        string _temperatureUnit;
        TextView _brakeCoolingTimeTextView;
        EditText
            _aircraftMassEditText,
            _brakesOnSpeedEditText,
            _pressureAltitudeEditText,
            _outsideAirTemperatureEditText;
        SwitchCompat
            _brakeTypeSwitch,
            _inFlightSwitch,
            _reverseSwitch;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _brakeCoolingService = ServiceLocator.Current.GetInstance<IBrakeCoolingService>("BrakeCoolingService");
            _massUnit = _settingsService.GetSetting<bool>("UseImperialMass") ? "lbs" : "kg";
            _temperatureUnit = _settingsService.GetSetting<bool>("UseFahrenheit") ? "°F" : "°C";
        }

        public static BrakeCoolingConditionalFragment NewInstance() => new BrakeCoolingConditionalFragment { Arguments = new Bundle() };

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.brakecooling_conditional_fragment, container, false);
            view.FindViewById<TextView>(Resource.Id.aircraftMassTextView).Text = $"Aircraft Mass in {_massUnit}:";
            view.FindViewById<TextView>(Resource.Id.outsideAirTemperatureTextView).Text = $"Outside Air Temperature in {_temperatureUnit}:";
            _aircraftMassEditText = view.FindViewById<EditText>(Resource.Id.aircraftMassEditText);
            _brakesOnSpeedEditText = view.FindViewById<EditText>(Resource.Id.brakesOnSpeedEditText);
            _pressureAltitudeEditText = view.FindViewById<EditText>(Resource.Id.pressureAltitudeEditText);
            _outsideAirTemperatureEditText = view.FindViewById<EditText>(Resource.Id.outsideAirTemperatureEditText);
            _brakeTypeSwitch = view.FindViewById<SwitchCompat>(Resource.Id.brakeTypeSwitch);
            _inFlightSwitch = view.FindViewById<SwitchCompat>(Resource.Id.inFlightSwitch);
            _reverseSwitch = view.FindViewById<SwitchCompat>(Resource.Id.reverseSwitch);
            _brakeCoolingTimeTextView = view.FindViewById<TextView>(Resource.Id.brakeCoolingTimeTextView);

            int brakeLevel = 1;

            view.FindViewById<RadioGroup>(Resource.Id.brakeLevelRadioGroup).CheckedChange += (s, e) =>
            {
                var javaLangString = (Java.Lang.String)view.FindViewById<RadioButton>(e.CheckedId).Tag;
                brakeLevel = int.Parse(javaLangString.ToString());
                _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text);
            };

            _aircraftMassEditText.TextChanged += (s, e) => { _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text); };
            _brakesOnSpeedEditText.TextChanged += (s, e) => { _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text); };
            _pressureAltitudeEditText.TextChanged += (s, e) => { _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text); };
            _outsideAirTemperatureEditText.TextChanged += (s, e) => { _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text); };
            _brakeTypeSwitch.CheckedChange += (s, e) => { _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text); };
            _inFlightSwitch.CheckedChange += (s, e) => { _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text); };
            _reverseSwitch.CheckedChange += (s, e) => { _brakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_brakeTypeSwitch.Checked, _reverseSwitch.Checked, _inFlightSwitch.Checked, _settingsService.GetSetting<bool>("UseImperialMass"), _settingsService.GetSetting<bool>("UseFahrenheit"), brakeLevel, _aircraftMassEditText.Text, _brakesOnSpeedEditText.Text, _pressureAltitudeEditText.Text, _outsideAirTemperatureEditText.Text); };

            view.FindViewById<Button>(Resource.Id.clearBrakeCoolingButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                _aircraftMassEditText.Text = "";
                _brakesOnSpeedEditText.Text = "";
                _pressureAltitudeEditText.Text = "";
                _outsideAirTemperatureEditText.Text = "";
            };
            return view;
        }
    }
}