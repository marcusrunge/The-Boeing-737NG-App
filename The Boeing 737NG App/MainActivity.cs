using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;

using The_Boeing_737NG_App.Fragments;
using Android.Support.V7.App;
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using Unity;
using The_Boeing_737NG_App.Services;
using Unity.ServiceLocation;
using CommonServiceLocator;

namespace The_Boeing_737NG_App
{
    [Activity(Label = "@string/app_name", Theme = "@style/SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, Icon = "@drawable/ic_launcher", RoundIcon = "@mipmap/ic_launcher", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        DrawerLayout drawerLayout;
        NavigationView navigationView;
        NavigationView bottomNavigationView;
        IMenuItem lastMenuItem;

        IMenuItem previousItem;
        IUnityContainer unityContainer;
        IEventService eventService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetTheme(Resource.Style.Theme_BoeingTheme);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTk4NjYyQDMxMzcyZTM0MmUzMEdjdGlBeVVnWTIvSFdRenV3K2FjOHlkbGlucHJOMXVVWGIxMTRHYSt5R289");            
            //int uiOptions = (int)Window.DecorView.SystemUiVisibility;
            //uiOptions = (int)SystemUiFlags.HideNavigation | (int)SystemUiFlags.Fullscreen;
            //Window.DecorView.SystemUiVisibility = (StatusBarVisibility)uiOptions;

            unityContainer = new UnityContainer();
            unityContainer.RegisterInstance<ISettingsService>("SettingsService", SettingsService.Instance());
            unityContainer.RegisterInstance<IFuelDistributionService>("FuelDistributionService", FuelDistributionService.Instance());
            unityContainer.RegisterInstance<IFuelUpliftService>("FuelUpliftService", FuelUpliftService.Instance());
            unityContainer.RegisterInstance<IFuelOrderService>("FuelOrderService", FuelOrderService.Instance());
            unityContainer.RegisterInstance<IFuelCheckDataInputService>("FuelCheckDataInputService", FuelCheckDataInputService.Instance());
            unityContainer.RegisterInstance<IFuelCheckListService>("FuelCheckListService", FuelCheckListService.Instance());
            unityContainer.RegisterInstance<IEventService>("EventService", EventService.Instance());
            unityContainer.RegisterInstance<IDatabaseService>("DatabaseService", DatabaseService.Instance(unityContainer.Resolve<ISettingsService>("SettingsService").GetLocalFilePath("fuelchecks.db")));
            unityContainer.RegisterInstance<IBaggageDistributionService>("BaggageDistributionService", BaggageDistributionService.Instance());
            unityContainer.RegisterInstance<IBrakeCoolingService>("BrakeCoolingService", BrakeCoolingService.Instance());
            unityContainer.RegisterInstance<ICircuitBreakerService>("CircuitBreakerService", CircuitBreakerService.Instance());
            eventService = unityContainer.Resolve<IEventService>("EventService");
            UnityServiceLocator unityServiceLocator = new UnityServiceLocator(unityContainer);
            ServiceLocator.SetLocatorProvider(() => unityServiceLocator);
            SetContentView(Resource.Layout.main);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                SetSupportActionBar(toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }

            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            //Set hamburger items menu
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            //setup navigation view
            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            bottomNavigationView = FindViewById<NavigationView>(Resource.Id.nav_view_bottom);

            //handle navigation
            navigationView.NavigationItemSelected += (sender, e) =>
            {
                //(sender as NavigationView).PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                if (previousItem != null)
                    previousItem.SetChecked(false);

                navigationView.SetCheckedItem(e.MenuItem.ItemId);

                previousItem = e.MenuItem;

                switch (e.MenuItem.ItemId)
                {
                    case Resource.Id.nav_fuel_order:
                        ListItemClicked(0);
                        break;
                    case Resource.Id.nav_fuel_check:
                        ListItemClicked(1);
                        break;
                    case Resource.Id.nav_baggage_distribution:
                        ListItemClicked(2);
                        break;
                    case Resource.Id.nav_brake_cooling:
                        ListItemClicked(3);
                        break;
                    case Resource.Id.circuit_breaker:
                        ListItemClicked(4);
                        break;
                    case Resource.Id.nav_checklists:
                        ListItemClicked(5);
                        break;
                    case Resource.Id.nav_manouvers:
                        ListItemClicked(6);
                        break;
                    case Resource.Id.nav_limitations:
                        ListItemClicked(7);
                        break;                    
                }
                e.MenuItem.SetChecked(true);
                if (lastMenuItem != null && lastMenuItem.ItemId != e.MenuItem.ItemId)
                {
                    lastMenuItem.SetChecked(false);
                    lastMenuItem = e.MenuItem;
                }
                drawerLayout.CloseDrawers();
            };

            bottomNavigationView.NavigationItemSelected += (sender, e) => 
            {
                if (e.MenuItem.ItemId == Resource.Id.nav_settings)
                {
                    ListItemClicked(8);
                    e.MenuItem.SetChecked(true);
                    if (lastMenuItem != null && lastMenuItem.ItemId != e.MenuItem.ItemId)
                    {
                        lastMenuItem.SetChecked(false);
                        lastMenuItem = e.MenuItem;
                    }
                }
                drawerLayout.CloseDrawers();
            };
            //if first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                navigationView.SetCheckedItem(Resource.Id.nav_fuel_order);
                lastMenuItem = navigationView.Menu.FindItem(Resource.Id.nav_fuel_order);
                ListItemClicked(0);
            }
        }

        int oldPosition = -1;
        private void ListItemClicked(int position)
        {
            //this way we don't load twice, but you might want to modify this a bit.
            if (position == oldPosition)
                return;

            oldPosition = position;

            Android.Support.V4.App.Fragment fragment = null;
            switch (position)
            {
                case 0:
                    fragment = FuelOrderFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.fuel_order_title);
                    break;
                case 1:
                    fragment = FuelCheckFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.fuel_check_title);
                    break;
                case 2:
                    fragment = BaggageDistributionFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.baggage_distribution_title);
                    break;
                case 3:
                    fragment = BrakeCoolingFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.brake_cooling_title);
                    break;
                case 4:
                    fragment = CircuitBreakerFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.circuit_breaker_title);
                    break;
                case 5:
                    fragment = ChecklistsFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.checklists_title);
                    break;
                case 6:
                    fragment = ManouversFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.manouvers_title);
                    break;
                case 7:
                    fragment = LimitationsFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.limitations_title);
                    break;
                case 8:
                    fragment = SettingsFragment.NewInstance();
                    SupportActionBar.SetTitle(Resource.String.settings_title);
                    break;
            }
            eventService.OnForceFragmentBackStackEvent(new System.EventArgs());
            SupportFragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();

            //var fragmentManagerTransaction = SupportFragmentManager.BeginTransaction();
            //fragmentManagerTransaction.Replace(Resource.Id.content_frame, fragment);
            //fragmentManagerTransaction.AddToBackStack(null);
            //fragmentManagerTransaction.Commit();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    //navigationView.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                    drawerLayout.OpenDrawer(GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
        public override void OnBackPressed()
        {
            if (drawerLayout.IsDrawerOpen(GravityCompat.Start)) drawerLayout.CloseDrawers();
            else base.OnBackPressed();
        }
    }
}