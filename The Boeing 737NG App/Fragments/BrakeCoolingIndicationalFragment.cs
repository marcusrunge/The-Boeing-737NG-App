using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CommonServiceLocator;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class BrakeCoolingIndicationalFragment : Android.Support.V4.App.Fragment
    {
        ISettingsService _settingsService;
        IBrakeCoolingService _brakeCoolingService;
        TextView _indicationalBrakeCoolingTimeTextView;
        EditText _brakeTemperatureValueEditText;
        SwitchCompat 
            _indicationalBrakeTypeSwitch,
            _indicationalInFlightSwitch;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _brakeCoolingService = ServiceLocator.Current.GetInstance<IBrakeCoolingService>("BrakeCoolingService");
        }

        public static BrakeCoolingIndicationalFragment NewInstance() => new BrakeCoolingIndicationalFragment { Arguments = new Bundle() };

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            var view = inflater.Inflate(Resource.Layout.brakecooling_indicational_fragment, container, false);
            _indicationalBrakeCoolingTimeTextView = view.FindViewById<TextView>(Resource.Id.indicationalBrakeCoolingTimeTextView);
            _brakeTemperatureValueEditText = view.FindViewById<EditText>(Resource.Id.brakeTemperatureValueEditText);
            _indicationalBrakeTypeSwitch = view.FindViewById<SwitchCompat>(Resource.Id.indicationalBrakeTypeSwitch);
            _indicationalInFlightSwitch = view.FindViewById<SwitchCompat>(Resource.Id.indicationalInFlightSwitch);

            _brakeTemperatureValueEditText.TextChanged += (s, e) => _indicationalBrakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_indicationalBrakeTypeSwitch.Checked, _indicationalInFlightSwitch.Checked, e.Text.ToString());
            _indicationalBrakeTypeSwitch.CheckedChange += (s, e) => _indicationalBrakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!e.IsChecked, _indicationalInFlightSwitch.Checked, _brakeTemperatureValueEditText.Text);
            _indicationalInFlightSwitch.CheckedChange += (s, e) => _indicationalBrakeCoolingTimeTextView.Text = _brakeCoolingService.BrakeCoolingMessage(!_indicationalBrakeTypeSwitch.Checked, e.IsChecked, _brakeTemperatureValueEditText.Text);
            view.FindViewById<Button>(Resource.Id.indicationalClearBrakeCoolingButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                _brakeTemperatureValueEditText.Text = "";
            };
            return view;
        }
    }
}