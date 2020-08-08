using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelOrderFragment : Fragment
    {
        ViewPager viewPager;
        FuelOrderTabsAdapter fuelOrderTabsAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static FuelOrderFragment NewInstance()
        {
            var fuelOrderFragment = new FuelOrderFragment { Arguments = new Bundle() };
            return fuelOrderFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fuel_order_fragment, null);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.fuelOrderViewPager);
            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.fuelOrderTabLayout);
            fuelOrderTabsAdapter = new FuelOrderTabsAdapter(view.Context, FragmentManager);
            viewPager.Adapter = fuelOrderTabsAdapter;
            tabLayout.SetupWithViewPager(viewPager);
            //view.FindViewById<AppBarLayout>(Resource.Id.fuelOrderAppBarLayout).Click+=(s,e)=> view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
            return view;
        }
    }

    class FuelOrderTabsAdapter : FragmentStatePagerAdapter
    {
        string[] fuelCheckSections;
        Context _context;
        public override int Count => fuelCheckSections.Length;

        public FuelOrderTabsAdapter(Context context, FragmentManager fragmentManager) : base(fragmentManager)
        {
            _context = context;
            fuelCheckSections = context.Resources.GetTextArray(Resource.Array.fuel_order_sections);
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
                    return FuelOrderDistributionFragment.NewInstance();
                case 1:
                    return FuelOrderUpliftFragment.NewInstance();
                case 2:
                    return FuelOrderOrderFragment.NewInstance();
            }
            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag)
        {
            return PositionNone;
        }
    }
}