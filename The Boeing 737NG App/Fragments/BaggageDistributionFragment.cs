using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using CommonServiceLocator;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class BaggageDistributionFragment : Fragment
    {
        ISettingsService _settingsService;
        IBaggageDistributionService _baggageDistributionService;
        TextView
            _picesRemainingCounterTextView,
            _hold1LoadTextView,
            _hold2LoadTextView,
            _hold3LoadTextView,
            _hold4LoadTextView;
        EditText
            _totalBaggageLoadEditText,
            _totalBaggagePicesEditText,
            _hold1EditText,
            _hold2EditText,
            _hold3EditText,
            _hold4EditText;
        string _massUnit;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _baggageDistributionService = ServiceLocator.Current.GetInstance<IBaggageDistributionService>("BaggageDistributionService");
            _massUnit = _settingsService.GetSetting<bool>("UseImperialMass") ? "lbs" : "kg";
            // Create your fragment here
        }

        public static BaggageDistributionFragment NewInstance() => new BaggageDistributionFragment { Arguments = new Bundle() };

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.baggagedistribution_fragment, container, false);
            view.FindViewById<TextView>(Resource.Id.totalBaggageLoadMassUnitTextView).Text = _massUnit;
            _picesRemainingCounterTextView = view.FindViewById<TextView>(Resource.Id.picesRemainingCounterTextView);
            _hold1LoadTextView = view.FindViewById<TextView>(Resource.Id.hold1LoadTextView);
            _hold2LoadTextView = view.FindViewById<TextView>(Resource.Id.hold2LoadTextView);
            _hold3LoadTextView = view.FindViewById<TextView>(Resource.Id.hold3LoadTextView);
            _hold4LoadTextView = view.FindViewById<TextView>(Resource.Id.hold4LoadTextView);
            view.FindViewById<TextView>(Resource.Id.hold1MassUnitTextView).Text = _massUnit;
            view.FindViewById<TextView>(Resource.Id.hold2MassUnitTextView).Text = _massUnit;
            view.FindViewById<TextView>(Resource.Id.hold3MassUnitTextView).Text = _massUnit;
            view.FindViewById<TextView>(Resource.Id.hold4MassUnitTextView).Text = _massUnit;
            _totalBaggageLoadEditText = view.FindViewById<EditText>(Resource.Id.totalBaggageLoadEditText);
            _totalBaggagePicesEditText = view.FindViewById<EditText>(Resource.Id.totalBaggagePicesEditText);
            _hold1EditText = view.FindViewById<EditText>(Resource.Id.hold1EditText);
            _hold2EditText = view.FindViewById<EditText>(Resource.Id.hold2EditText);
            _hold3EditText = view.FindViewById<EditText>(Resource.Id.hold3EditText);
            _hold4EditText = view.FindViewById<EditText>(Resource.Id.hold4EditText);

            _totalBaggageLoadEditText.Text = _settingsService.GetSetting<string>("TotalBaggageLoad");
            _totalBaggagePicesEditText.Text = _settingsService.GetSetting<string>("TotalBaggagePices");
            _hold1EditText.Text = _settingsService.GetSetting<string>("Hold1Pices");
            _hold2EditText.Text = _settingsService.GetSetting<string>("Hold2Pices");
            _hold3EditText.Text = _settingsService.GetSetting<string>("Hold3Pices");
            _hold4EditText.Text = _settingsService.GetSetting<string>("Hold4Pices");
            _baggageDistributionService.CalculateBaggageDistribution(_totalBaggageLoadEditText.Text, _totalBaggagePicesEditText.Text, _hold1EditText.Text, _hold2EditText.Text, _hold3EditText.Text, _hold4EditText.Text);
            SetValues();

            _totalBaggageLoadEditText.TextChanged += (s, e) =>
            {
                _baggageDistributionService.CalculateBaggageDistribution(_totalBaggageLoadEditText.Text, _totalBaggagePicesEditText.Text, _hold1EditText.Text, _hold2EditText.Text, _hold3EditText.Text, _hold4EditText.Text);
                SetValues();
                _settingsService.SaveSetting("TotalBaggageLoad", e.Text.ToString());
            };
            _totalBaggagePicesEditText.TextChanged += (s, e) =>
            {
                _baggageDistributionService.CalculateBaggageDistribution(_totalBaggageLoadEditText.Text, _totalBaggagePicesEditText.Text, _hold1EditText.Text, _hold2EditText.Text, _hold3EditText.Text, _hold4EditText.Text);
                SetValues();
                _settingsService.SaveSetting("TotalBaggagePices", e.Text.ToString());
            };
            _hold1EditText.TextChanged += (s, e) =>
            {
                _baggageDistributionService.CalculateBaggageDistribution(_totalBaggageLoadEditText.Text, _totalBaggagePicesEditText.Text, _hold1EditText.Text, _hold2EditText.Text, _hold3EditText.Text, _hold4EditText.Text);
                SetValues();
                _settingsService.SaveSetting("Hold1Pices", e.Text.ToString());
            };
            _hold2EditText.TextChanged += (s, e) =>
            {
                _baggageDistributionService.CalculateBaggageDistribution(_totalBaggageLoadEditText.Text, _totalBaggagePicesEditText.Text, _hold1EditText.Text, _hold2EditText.Text, _hold3EditText.Text, _hold4EditText.Text);
                SetValues();
                _settingsService.SaveSetting("Hold2Pices", e.Text.ToString());
            };
            _hold3EditText.TextChanged += (s, e) =>
            {
                _baggageDistributionService.CalculateBaggageDistribution(_totalBaggageLoadEditText.Text, _totalBaggagePicesEditText.Text, _hold1EditText.Text, _hold2EditText.Text, _hold3EditText.Text, _hold4EditText.Text);
                SetValues();
                _settingsService.SaveSetting("Hold3Pices", e.Text.ToString());
            };
            _hold4EditText.TextChanged += (s, e) =>
            {
                _baggageDistributionService.CalculateBaggageDistribution(_totalBaggageLoadEditText.Text, _totalBaggagePicesEditText.Text, _hold1EditText.Text, _hold2EditText.Text, _hold3EditText.Text, _hold4EditText.Text);
                SetValues();
                _settingsService.SaveSetting("Hold4Pices", e.Text.ToString());
            };

            view.FindViewById<Button>(Resource.Id.clearBaggageDistributionButton).Click += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                _picesRemainingCounterTextView.Text = "0";
                _hold1LoadTextView.Text = "0";
                _hold2LoadTextView.Text = "0";
                _hold3LoadTextView.Text = "0";
                _hold4LoadTextView.Text = "0";
                _totalBaggageLoadEditText.Text = "";
                _totalBaggagePicesEditText.Text = "";
                _hold1EditText.Text = "";
                _hold2EditText.Text = "";
                _hold3EditText.Text = "";
                _hold4EditText.Text = "";
            };

            _baggageDistributionService.ErrorMessageEvent += (s, e) =>
            {
                AndroidX.AppCompat.App.AlertDialog.Builder alertDialogBuilder = new AndroidX.AppCompat.App.AlertDialog.Builder(Context);
                AndroidX.AppCompat.App.AlertDialog alertDialog = alertDialogBuilder.Create();
                alertDialog.SetTitle("Error");
                alertDialog.SetIcon(Resource.Drawable.ic_stat_error_outline);
                alertDialog.SetMessage((e as ErrorMessageEventArgs).ErrorMessage);
                alertDialog.SetButton((int)DialogButtonType.Positive, "OK", (sender, eventArgs) => { });
                alertDialog.Show();
            };
            return view;
        }

        private void SetValues()
        {
            _picesRemainingCounterTextView.Text = _baggageDistributionService.RemainingPices;
            _hold1LoadTextView.Text = _baggageDistributionService.Hold1Mass;
            _hold2LoadTextView.Text = _baggageDistributionService.Hold2Mass;
            _hold3LoadTextView.Text = _baggageDistributionService.Hold3Mass;
            _hold4LoadTextView.Text = _baggageDistributionService.Hold4Mass;
        }
    }
}