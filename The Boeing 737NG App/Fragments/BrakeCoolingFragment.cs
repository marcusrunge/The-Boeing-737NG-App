using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using Android.Support.Design.Widget;

namespace The_Boeing_737NG_App.Fragments
{
    public class BrakeCoolingFragment : Fragment
    {
        ViewPager viewPager;
        BrakeCoolingTabsAdapter brakeCoolingTabsAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static BrakeCoolingFragment NewInstance() => new BrakeCoolingFragment { Arguments = new Bundle() };

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.brake_cooling_fragment, null);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.brakeCoolingViewPager);
            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.brakeCoolingTabLayout);
            brakeCoolingTabsAdapter = new BrakeCoolingTabsAdapter(view.Context, FragmentManager);
            viewPager.Adapter = brakeCoolingTabsAdapter;
            tabLayout.SetupWithViewPager(viewPager);
            return view;
        }
    }

    class BrakeCoolingTabsAdapter : FragmentStatePagerAdapter
    {
        string[] brakeCoolingSections;

        public override int Count => brakeCoolingSections.Length;

        public BrakeCoolingTabsAdapter(Context context, FragmentManager fragmentManager) : base(fragmentManager) => brakeCoolingSections = context.Resources.GetTextArray(Resource.Array.brake_cooling_sections);

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position) => new Java.Lang.String(brakeCoolingSections[position]);

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return BrakeCoolingConditionalFragment.NewInstance();
                case 1:
                    return BrakeCoolingIndicationalFragment.NewInstance();
            }
            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag)
        {
            return PositionNone;
        }
    }
}