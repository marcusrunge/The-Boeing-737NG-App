using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelCheckFragment : Fragment
    {
        ViewPager viewPager;
        FuelCheckTabsAdapter fuelCheckTabsAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public static FuelCheckFragment NewInstance()
        {
            var fuelCheckFragment = new FuelCheckFragment { Arguments = new Bundle() };
            return fuelCheckFragment;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fuel_check_fragment, null);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.fuelCheckViewPager);
            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.fuelCheckTabLayout);
            fuelCheckTabsAdapter = new FuelCheckTabsAdapter(view.Context, FragmentManager);
            viewPager.Adapter = fuelCheckTabsAdapter;
            tabLayout.SetupWithViewPager(viewPager);
            return view;
        }
    }

    class FuelCheckTabsAdapter : FragmentStatePagerAdapter
    {
        string[] fuelCheckSections;

        public override int Count
        {
            get
            {
                return fuelCheckSections.Length;
            }
        }

        public FuelCheckTabsAdapter(Context context, FragmentManager fragmentManager) : base(fragmentManager)
        {
            fuelCheckSections = context.Resources.GetTextArray(Resource.Array.fuel_check_sections);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(fuelCheckSections[position]);
        }

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return FuelCheckDataInputFragment.NewInstance();
                case 1:
                    return FuelCheckListFragment.NewInstance();
                case 2:
                    return FuelChecksChartFragment.NewInstance();
            }
            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag)
        {
            return PositionNone;
        }
    }
}