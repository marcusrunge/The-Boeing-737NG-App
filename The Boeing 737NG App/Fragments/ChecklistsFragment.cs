
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;

namespace The_Boeing_737NG_App.Fragments
{
    public class ChecklistsFragment : Fragment
    {
        ViewPager viewPager;
        ChecklistsTabsAdapter checklistsTabsAdapter;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static ChecklistsFragment NewInstance() => new ChecklistsFragment { Arguments = new Bundle() };
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.checklists_fragment, null);
            viewPager = view.FindViewById<ViewPager>(Resource.Id.checklistsViewPager);
            var tabLayout = view.FindViewById<TabLayout>(Resource.Id.checklistsTabLayout);
            checklistsTabsAdapter = new ChecklistsTabsAdapter(view.Context, FragmentManager);
            viewPager.Adapter = checklistsTabsAdapter;
            tabLayout.SetupWithViewPager(viewPager);
            return view;
        }
    }

    class ChecklistsTabsAdapter : FragmentStatePagerAdapter
    {
        string[] checklistSections;

        public override int Count => checklistSections.Length;

        public ChecklistsTabsAdapter(Context context, FragmentManager fragmentManager) : base(fragmentManager) => checklistSections = context.Resources.GetTextArray(Resource.Array.checklists_sections);

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position) => new Java.Lang.String(checklistSections[position]);

        public override Fragment GetItem(int position)
        {
            switch (position)
            {
                case 0:
                    return ChecklistsQuickActionFragment.NewInstance();
                case 1:
                    return ChecklistsLostcomFragment.NewInstance();
            }
            return null;
        }

        public override int GetItemPosition(Java.Lang.Object frag)
        {
            return PositionNone;
        }
    }
}