using Android.OS;
using Android.Views;
using CommonServiceLocator;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class FragmentInflater : Android.Support.V4.App.Fragment
    {
        int _resourceId;
        IEventService _eventService;
        public FragmentInflater(int resourceId) : base()
        {
            _resourceId = resourceId;
            _eventService = ServiceLocator.Current.GetInstance<IEventService>("EventService");
        }
        public static FragmentInflater Instance(int resourceId) => new FragmentInflater(resourceId) { Arguments = new Bundle() };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _eventService.ForceFragmentBackStackEvent += (s, e) =>
            {
                if (FragmentManager != null && FragmentManager.Fragments.Count > 0) FragmentManager.PopBackStack();
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(_resourceId, container, false);
        }
    }
}