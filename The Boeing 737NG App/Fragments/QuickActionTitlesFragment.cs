using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;

namespace The_Boeing_737NG_App.Fragments
{
    public class QuickActionTitlesFragment : ListFragment
    {
        public static QuickActionTitlesFragment NewInstance() => new QuickActionTitlesFragment { Arguments = new Bundle() };
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
            var quickActionItems = new string[]
            {
                "Aborted Engine Start",
                "Airspeed Unreliable",
                "APU Fire",
                "Cabin Altitude Warning",
                "Emergency Descent",
                "Engine Fire",
                "Engine Limit or Surge or Stall",
                "Engine Overheat",
                "Engine Severe Damage or Separation",
                "Engine Tailpipe Fire",
                "Evacuation",
                "Landing Configuration",
                "Loss of Thrust on both Engines",
                "Rapid Depressurization",
                "Runaway Stabilizer",
                "Smoke, Fire or Fumes",
                "Takeoff Configuration",
                "Warning Horn (intermittent)",
                "Warning Light - Cabin Altitude or Takeoff Configuration"
            };
            ListAdapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItem1, quickActionItems);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return base.OnCreateView(inflater, container, savedInstanceState);
        }
        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            var fragmentTransaction = FragmentManager.BeginTransaction();
            switch (position)
            {
                case 0:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.aborted_engine_start_fragment));
                    break;
                case 1:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.airspeed_unreliable_fragment));
                    break;
                case 2:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.apu_fire_fragment));
                    break;
                case 3:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.cabin_altitude_warning_fragment));
                    break;
                case 4:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.emergency_decent_fragment));
                    break;
                case 5:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.engine_fire_damage_separation_fragment));
                    break;
                case 6:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.engine_limit_surge_stall_fragment));
                    break;
                case 7:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.engine_overheat_fragment));
                    break;
                case 8:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.engine_fire_damage_separation_fragment));
                    break;
                case 9:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.engine_tailpipe_fire_fragment));
                    break;
                case 10:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.evacuation_fragment));
                    break;
                case 11:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.landing_configuration_fragment));
                    break;
                case 12:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.loss_bothengines_thrust_fragment));
                    break;
                case 13:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.cabin_altitude_warning_fragment));
                    break;
                case 14:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.runaway_stabilizer_fragment));
                    break;
                case 15:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.smoke_fire_fumes_fragment));
                    break;
                case 16:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.takeoff_configuration_fragment));
                    break;
                case 17:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.warning_horn_light_fragment));
                    break;
                case 18:
                    fragmentTransaction.Replace(Resource.Id.quickactionContentFrameLayout, FragmentInflater.Instance(Resource.Layout.warning_horn_light_fragment));
                    break;
                default:
                    break;
            }
            fragmentTransaction.SetTransition(4099);
            fragmentTransaction.AddToBackStack(null);
            fragmentTransaction.Commit();
        }
    }
}