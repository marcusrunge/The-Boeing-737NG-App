using Android.OS;
using Android.Views;

namespace The_Boeing_737NG_App.Fragments
{
    public class SettingsDisclaimerFragment : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.settings_disclaimer_fragment, container, false);

            //return base.OnCreateView(inflater, container, savedInstanceState);
        }

        public static SettingsDisclaimerFragment NewInstance()
        {
            var settingsDisclaimerFragment = new SettingsDisclaimerFragment { Arguments = new Bundle() };
            return settingsDisclaimerFragment;
        }
    }
}