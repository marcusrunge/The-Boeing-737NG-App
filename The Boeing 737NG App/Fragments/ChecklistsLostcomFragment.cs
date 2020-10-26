
using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace The_Boeing_737NG_App.Fragments
{
    public class ChecklistsLostcomFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static ChecklistsLostcomFragment NewInstance() => new ChecklistsLostcomFragment { Arguments = new Bundle() };
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.checklists_lostcom_fragment, container, false);
            return view;
        }
    }
}