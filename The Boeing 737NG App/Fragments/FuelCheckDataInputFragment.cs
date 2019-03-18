using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using CommonServiceLocator;
using The_Boeing_737NG_App.Models;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelCheckDataInputFragment : Android.Support.V4.App.Fragment
    {
        TextView _fuelAmountDifferenceTextView;
        EditText
            _blockFuelEditText,
            _leftEngineFuelEditText,
            _rightEngineFuelEditText,
            _remainingFuelCheckEditText;
        DateTime _lastFuelcheck;

        IFuelCheckDataInputService _fuelCheckDataInputService;
        ISettingsService _settingsService;
        IDatabaseService _databaseService;
        IEventService _eventService;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _fuelCheckDataInputService = ServiceLocator.Current.GetInstance<IFuelCheckDataInputService>("FuelCheckDataInputService");
            _databaseService = ServiceLocator.Current.GetInstance<IDatabaseService>("DatabaseService");
            _eventService = ServiceLocator.Current.GetInstance<IEventService>("EventService");
            _databaseService.CreateTable<FuelCheck>();
            _lastFuelcheck = DateTime.MinValue;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.fuelcheck_data_input_fragment, container, false);

            _fuelAmountDifferenceTextView = view.FindViewById<TextView>(Resource.Id.fuelAmountDifferenceTextView);
            _blockFuelEditText = view.FindViewById<EditText>(Resource.Id.blockFuelEditText);
            _leftEngineFuelEditText = view.FindViewById<EditText>(Resource.Id.leftEngineFuelEditText);
            _rightEngineFuelEditText = view.FindViewById<EditText>(Resource.Id.rightEngineFuelEditText);
            _remainingFuelCheckEditText = view.FindViewById<EditText>(Resource.Id.remainingFuelCheckEditText);

            _blockFuelEditText.TextChanged += (s, e) =>
            {
                _fuelAmountDifferenceTextView.Text = _fuelCheckDataInputService.CalcualteDifference(_blockFuelEditText.Text, _leftEngineFuelEditText.Text, _rightEngineFuelEditText.Text, _remainingFuelCheckEditText.Text);
                _settingsService.SaveSetting("FuelCheckBlockFuel", e.Text.ToString());
            };
            _leftEngineFuelEditText.TextChanged += (s, e) =>
            {
                _fuelAmountDifferenceTextView.Text = _fuelCheckDataInputService.CalcualteDifference(_blockFuelEditText.Text, _leftEngineFuelEditText.Text, _rightEngineFuelEditText.Text, _remainingFuelCheckEditText.Text);
                _settingsService.SaveSetting("FuelCheckLeftEngineFuel", e.Text.ToString());
            };
            _rightEngineFuelEditText.TextChanged += (s, e) =>
            {
                _fuelAmountDifferenceTextView.Text = _fuelCheckDataInputService.CalcualteDifference(_blockFuelEditText.Text, _leftEngineFuelEditText.Text, _rightEngineFuelEditText.Text, _remainingFuelCheckEditText.Text);
                _settingsService.SaveSetting("FuelCheckRightEngineFuel", e.Text.ToString());
            };
            _remainingFuelCheckEditText.TextChanged += (s, e) =>
            {
                _fuelAmountDifferenceTextView.Text = _fuelCheckDataInputService.CalcualteDifference(_blockFuelEditText.Text, _leftEngineFuelEditText.Text, _rightEngineFuelEditText.Text, _remainingFuelCheckEditText.Text);
                _settingsService.SaveSetting("FuelCheckRemainingFuel", e.Text.ToString());
            };

            _blockFuelEditText.Text = _settingsService.GetSetting<string>("FuelCheckBlockFuel");
            _leftEngineFuelEditText.Text = _settingsService.GetSetting<string>("FuelCheckLeftEngineFuel");
            _rightEngineFuelEditText.Text = _settingsService.GetSetting<string>("FuelCheckRightEngineFuel");
            _remainingFuelCheckEditText.Text = _settingsService.GetSetting<string>("FuelCheckRemainingFuel");

            var fuelcheckToolbar = view.FindViewById<Toolbar>(Resource.Id.fuelcheckToolbar);
            fuelcheckToolbar.InflateMenu(Resource.Menu.FuelCheckInputActionBarMenu);
            fuelcheckToolbar.MenuItemClick += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                if (int.TryParse(_fuelAmountDifferenceTextView.Text, out int parsedDifference) && _lastFuelcheck.AddMinutes(1).ToUniversalTime() < DateTime.Now.ToUniversalTime())
                {
                    var fuelcheck = new FuelCheck() { Difference = parsedDifference, Time = DateTime.UtcNow.ToShortTimeString() };
                    _databaseService.Insert<FuelCheck>(fuelcheck);
                    _lastFuelcheck = DateTime.Now.ToUniversalTime();
                    _eventService.OnDatabaseUpdatedEvent(new DatabaseUpdatedEventArgs<FuelCheck>(fuelcheck, UpdateType.Add));
                };
            };

            view.FindViewById<Button>(Resource.Id.clearBlockFuelButton).Click += (s, e) =>
              {
                  view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                  _blockFuelEditText.Text = "";
                  _settingsService.SaveSetting("FuelCheckBlockFuel", "");
              };
            view.FindViewById<Button>(Resource.Id.clearUsedRemainingFuelButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                _leftEngineFuelEditText.Text = "";
                _rightEngineFuelEditText.Text = "";
                _remainingFuelCheckEditText.Text = "";
                _fuelAmountDifferenceTextView.Text = "----";
                _settingsService.SaveSetting("FuelCheckLeftEngineFuel", "");
                _settingsService.SaveSetting("FuelCheckRightEngineFuel", "");
                _settingsService.SaveSetting("FuelCheckRemainingFuel", "");
            };

            _fuelCheckDataInputService.ErrorMessageEvent += (s, e) =>
            {
                AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(Context);
                AlertDialog alertDialog = alertDialogBuilder.Create();
                alertDialog.SetTitle("Error");
                alertDialog.SetIcon(Resource.Drawable.ic_stat_error_outline);
                alertDialog.SetMessage((e as ErrorMessageEventArgs).ErrorMessage);
                alertDialog.SetButton("OK", (sender, eventArgs) => { });
                alertDialog.Show();
            };

            return view;
            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public static FuelCheckDataInputFragment NewInstance()
        {
            var fuelDataInputFragment = new FuelCheckDataInputFragment { Arguments = new Bundle() };
            return fuelDataInputFragment;
        }
    }
}