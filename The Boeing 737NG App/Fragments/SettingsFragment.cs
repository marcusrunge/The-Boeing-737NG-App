using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;

namespace The_Boeing_737NG_App.Fragments
{
    public class SettingsFragment : Fragment
    {
        ViewPager viewPager;
        SettingsTabsAdapter settingsTabsAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static SettingsFragment NewInstance()
        {
            var settingsFragment = new SettingsFragment { Arguments = new Bundle() };
            return settingsFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.settings_fragment, null);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.settingsViewPager);
            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.settingsTabLayout);
            settingsTabsAdapter = new SettingsTabsAdapter(view.Context, FragmentManager);
            viewPager.Adapter = settingsTabsAdapter;
            tabLayout.SetupWithViewPager(viewPager);
            return view;
        }
    }

    class SettingsTabsAdapter : FragmentStatePagerAdapter
    {
        string[] settingsSections;

        public override int Count
        {
            get
            {
                return settingsSections.Length;
            }
        }

        public SettingsTabsAdapter(Context context, FragmentManager fragmentManager) : base(fragmentManager)
        {
            settingsSections = context.Resources.GetTextArray(Resource.Array.settings_sections);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(settingsSections[position]);
        }

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return SettingsSettingsFragment.NewInstance();
                case 1:
                    return SettingsDisclaimerFragment.NewInstance();
                case 2:
                    return SettingsAboutFragment.NewInstance();
            }
            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag)
        {
            return PositionNone;
        }
    }
}