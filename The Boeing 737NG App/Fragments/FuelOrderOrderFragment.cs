using Android.OS;
using Android.Views;
using Android.Widget;
using CommonServiceLocator;
using System;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelOrderOrderFragment : Android.Support.V4.App.Fragment
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(FuelOrderOrderFragment).Name.ToUpper();

        IFuelOrderService _fuelOrderService;
        ISettingsService _settingsService;
        TextView _departureDateTextView;
        EditText
            _flightNumberEditText,
            _departureEditText,
            _destinationEditText,
            _registrationEditText,
            _aircraftEditText,
            _standOrGateEditText,
            _captainEditText;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _fuelOrderService = ServiceLocator.Current.GetInstance<IFuelOrderService>("FuelOrderService");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fuelorder_order_fragment, container, false);
            _departureDateTextView = view.FindViewById<TextView>(Resource.Id.departureDateTextView);
            _flightNumberEditText = view.FindViewById<EditText>(Resource.Id.flightNumberEditText);
            _departureEditText = view.FindViewById<EditText>(Resource.Id.departureEditText);
            _destinationEditText = view.FindViewById<EditText>(Resource.Id.destinationEditText);
            _registrationEditText = view.FindViewById<EditText>(Resource.Id.registrationEditText);
            _aircraftEditText = view.FindViewById<EditText>(Resource.Id.aircraftEditText);
            _standOrGateEditText = view.FindViewById<EditText>(Resource.Id.standOrGateEditText);
            _captainEditText = view.FindViewById<EditText>(Resource.Id.captainEditText);
            _departureDateTextView.Text = String.IsNullOrEmpty(_settingsService.GetSetting<string>("FlightDate")) ? DateTime.Today.ToShortDateString() : _settingsService.GetSetting<string>("FlightDate");
            _flightNumberEditText.Text = _settingsService.GetSetting<string>("FlightNumber");
            _departureEditText.Text = _settingsService.GetSetting<string>("Departure");
            _destinationEditText.Text = _settingsService.GetSetting<string>("Destination");
            _registrationEditText.Text = _settingsService.GetSetting<string>("Registration");
            _aircraftEditText.Text = _settingsService.GetSetting<string>("Aircraft");
            _standOrGateEditText.Text = _settingsService.GetSetting<string>("StandOrGate");
            _captainEditText.Text = _settingsService.GetSetting<string>("Captain");
            _departureDateTextView.Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                DatePickerFragment datePickerFragment = DatePickerFragment.NewInstance(delegate (DateTime dateTime)
                {
                    _departureDateTextView.Text = dateTime.ToShortDateString();
                    _settingsService.SaveSetting("FlightDate", dateTime.ToShortDateString());
                });
                datePickerFragment.Show(FragmentManager, DatePickerFragment.TAG);
            };
            _flightNumberEditText.TextChanged += (s, e) => _settingsService.SaveSetting("FlightNumber", e.Text.ToString());
            _departureEditText.TextChanged += (s, e) => _settingsService.SaveSetting("Departure", e.Text.ToString());
            _destinationEditText.TextChanged += (s, e) => _settingsService.SaveSetting("Destination", e.Text.ToString());
            _registrationEditText.TextChanged += (s, e) => _settingsService.SaveSetting("Registration", e.Text.ToString());
            _aircraftEditText.TextChanged += (s, e) => _settingsService.SaveSetting("Aircraft", e.Text.ToString());
            _standOrGateEditText.TextChanged += (s, e) => _settingsService.SaveSetting("StandOrGate", e.Text.ToString());
            _captainEditText.TextChanged += (s, e) => _settingsService.SaveSetting("Captain", e.Text.ToString());
            view.FindViewById<Button>(Resource.Id.clearFuelOrderButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                //view.PlaySoundEffect(SoundEffects.Click);
                _departureDateTextView.Text = DateTime.Today.ToShortDateString();
                _settingsService.SaveSetting("FlightDate", DateTime.Today.ToShortDateString());
                _flightNumberEditText.Text = "";
                _departureEditText.Text = "";
                _destinationEditText.Text = "";
                _registrationEditText.Text = "";
                _aircraftEditText.Text = "";
                _standOrGateEditText.Text = "";
                _captainEditText.Text = "";
            };
            view.FindViewById<Button>(Resource.Id.sendFuelOrderButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                //view.PlaySoundEffect(SoundEffects.Click);
                _fuelOrderService.Send(FuelOrderOrderFragmentInstance, _flightNumberEditText.Text, _departureDateTextView.Text, _departureEditText.Text, _destinationEditText.Text, _registrationEditText.Text, $"B737-{_aircraftEditText.Text}", _standOrGateEditText.Text, _captainEditText.Text, _settingsService.GetSetting<bool>("UseImperialMass") ? "lbs" : "kg", _settingsService.GetSetting<string>("TotalFuel"), _settingsService.GetSetting<string>("FuelQuantityCenter"), _settingsService.GetSetting<string>("FuelQuantityWing"));
            };
            return view;
        }

        public static FuelOrderOrderFragment FuelOrderOrderFragmentInstance;
        public static FuelOrderOrderFragment NewInstance()
        {
            var fuelOrderOrderFragment = new FuelOrderOrderFragment { Arguments = new Bundle() };
            FuelOrderOrderFragmentInstance = fuelOrderOrderFragment;
            return fuelOrderOrderFragment;
        }
    }
}