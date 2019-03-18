using Android.OS;
using Android.Views;
using Android.Widget;
using CommonServiceLocator;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class SettingsSettingsFragment : Android.Support.V4.App.Fragment
    {
        ISettingsService _settingsService;
        bool _useImperialMass;
        bool _useImperialDensity;
        bool _useFahrenheit;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _useImperialDensity = _settingsService.GetSetting<bool>("UseImperialDensity");
            _useImperialMass = _settingsService.GetSetting<bool>("UseImperialMass");
            _useFahrenheit = _settingsService.GetSetting<bool>("UseFahrenheit");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.settings_settings_fragment, container, false);
            var massMeasurementSystemSwitch = view.FindViewById<Switch>(Resource.Id.massMeasurementSystemSwitch);
            var densityMeasurementSystemSwitch = view.FindViewById<Switch>(Resource.Id.densityMeasurementSystemSwitch);
            var temperatureMeasurementSystemSwitch = view.FindViewById<Switch>(Resource.Id.temperatureMeasurementSystemSwitch);
            massMeasurementSystemSwitch.Checked = _useImperialMass;
            densityMeasurementSystemSwitch.Checked = _useImperialDensity;
            temperatureMeasurementSystemSwitch.Checked = _useFahrenheit;
            massMeasurementSystemSwitch.CheckedChange += (s, e) => _settingsService.SaveSetting("UseImperialMass", e.IsChecked);
            densityMeasurementSystemSwitch.CheckedChange += (s, e) => _settingsService.SaveSetting("UseImperialDensity", e.IsChecked);
            temperatureMeasurementSystemSwitch.CheckedChange += (s, e) => _settingsService.SaveSetting("UseFahrenheit", e.IsChecked);
            return view;

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public static SettingsSettingsFragment NewInstance()
        {
            var settingsSettingsFragment = new SettingsSettingsFragment { Arguments = new Bundle() };
            return settingsSettingsFragment;
        }
    }
}