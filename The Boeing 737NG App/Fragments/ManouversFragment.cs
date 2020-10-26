using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace The_Boeing_737NG_App.Fragments
{
    public class ManouversFragment : Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public static ManouversFragment NewInstance() => new ManouversFragment { Arguments = new Bundle() };
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.manouvers_fragment, null);
            var fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Resource.Id.manouverContentFrameLayout, ManouversTitlesFragment.NewInstance());
            fragmentTransaction.SetTransition(4099);
            fragmentTransaction.Commit();
            return view;
        }
    }
}