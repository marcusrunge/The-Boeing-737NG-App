
using Android.Support.V4.App;
using Android.OS;
using Android.Views;

namespace The_Boeing_737NG_App.Fragments
{
    public class LimitationsFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public static LimitationsFragment NewInstance() => new LimitationsFragment { Arguments = new Bundle() };
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.limitations_fragment, null);
            return view;
        }
    }
}