using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;

namespace The_Boeing_737NG_App.Fragments
{
    public class ManouversTitlesFragment : ListFragment
    {
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            var manouverItems = new String[]
            {
                "Approach to Stall or Stall Recovery",
                "Rejected Takeoff",
                "Ground Proximity Warning (GPWS) Response",
                "Traffic Avoidance",
                "Upset Recovery",
                "Windshear"
            };
            ListAdapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItem1, manouverItems);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            var fragmentTransaction = FragmentManager.BeginTransaction();
            switch (position)
            {
                case 0:
                    fragmentTransaction.Replace(Resource.Id.manouverContentFrameLayout, FragmentInflater.Instance(Resource.Layout.approach_stall_recovery_fragment));
                    break;
                case 1:
                    fragmentTransaction.Replace(Resource.Id.manouverContentFrameLayout, FragmentInflater.Instance(Resource.Layout.rejected_takeoff_fragment));
                    break;
                case 2:
                    fragmentTransaction.Replace(Resource.Id.manouverContentFrameLayout, FragmentInflater.Instance(Resource.Layout.gpws_response_fragment));
                    break;
                case 3:
                    fragmentTransaction.Replace(Resource.Id.manouverContentFrameLayout, FragmentInflater.Instance(Resource.Layout.traffic_avoidance_fragment));
                    break;
                case 4:
                    fragmentTransaction.Replace(Resource.Id.manouverContentFrameLayout, FragmentInflater.Instance(Resource.Layout.upset_recovery_fragment));
                    break;
                case 5:
                    fragmentTransaction.Replace(Resource.Id.manouverContentFrameLayout, FragmentInflater.Instance(Resource.Layout.windshear_fragment));
                    break;
                default:
                    break;
            }
            fragmentTransaction.SetTransition(4099);
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }

        public static ManouversTitlesFragment NewInstance() => new ManouversTitlesFragment { Arguments = new Bundle() };
    }
}