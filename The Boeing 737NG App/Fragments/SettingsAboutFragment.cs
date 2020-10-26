using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;

namespace The_Boeing_737NG_App.Fragments
{
    public class SettingsAboutFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.settings_about_fragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            var textView4 = view.FindViewById<TextView>(Resource.Id.contactTextView);
            view.FindViewById<TextView>(Resource.Id.versionNameTextView).Text = Context.PackageManager.GetPackageInfo(Context.PackageName, 0).VersionName;
            textView4.TextFormatted = Html.FromHtml("<a href=\"mailto:code_m@outlook.de\">technical support/feedback email</a>", FromHtmlOptions.ModeLegacy);
            textView4.MovementMethod = LinkMovementMethod.Instance;
        }

        public static SettingsAboutFragment NewInstance()
        {
            var settingsAboutFragment = new SettingsAboutFragment { Arguments = new Bundle() };
            return settingsAboutFragment;
        }
    }
}