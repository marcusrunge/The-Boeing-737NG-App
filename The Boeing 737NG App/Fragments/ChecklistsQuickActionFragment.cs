using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace The_Boeing_737NG_App.Fragments
{
    public class ChecklistsQuickActionFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public static ChecklistsQuickActionFragment NewInstance() => new ChecklistsQuickActionFragment { Arguments = new Bundle() };
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.checklists_quickaction_fragment, container, false);
            var quickActionTitlesFragmentInstance = QuickActionTitlesFragment.NewInstance();
            var fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.quickactionContentFrameLayout, quickActionTitlesFragmentInstance);
            fragmentTransaction.SetTransition(4099);
            fragmentTransaction.Commit();
            return view;
        }
    }
}