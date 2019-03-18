using Android.OS;
using Android.Views;
using Com.Syncfusion.Charts;
using The_Boeing_737NG_App.Services;
using The_Boeing_737NG_App.Models;
using System.Collections.ObjectModel;
using CommonServiceLocator;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelChecksChartFragment : Android.Support.V4.App.Fragment
    {
        ISettingsService _settingsService;
        IDatabaseService _databaseService;
        IEventService _eventService;
        SfChart _fuelCheckSfChart;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _databaseService = ServiceLocator.Current.GetInstance<IDatabaseService>("DatabaseService");
            _eventService = ServiceLocator.Current.GetInstance<IEventService>("EventService");
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.fuelchecks_chart_fragment, container, false);
            _fuelCheckSfChart = view.FindViewById<SfChart>(Resource.Id.fuelCheckSfChart);
            _fuelCheckSfChart.SetBackgroundColor(Android.Graphics.Color.ParseColor("#546e7a"));
            CategoryAxis categoryAxis = new CategoryAxis();
            _fuelCheckSfChart.PrimaryAxis = categoryAxis;
            NumericalAxis numericalAxis = new NumericalAxis();
            _fuelCheckSfChart.SecondaryAxis = numericalAxis;
            _fuelCheckSfChart.PrimaryAxis.LabelStyle.TextColor = Android.Graphics.Color.ParseColor("#cfd8dc");
            _fuelCheckSfChart.SecondaryAxis.LabelStyle.TextColor = Android.Graphics.Color.ParseColor("#cfd8dc");
            _fuelCheckSfChart.PrimaryAxis.EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Shift;
            var observableCollection = new ObservableCollection<FuelCheck>(_databaseService.SelectTableAsList<FuelCheck>());
            _fuelCheckSfChart.Series.Add(new LineSeries()
            {
                ItemsSource = observableCollection,
                XBindingPath = "Time",
                YBindingPath = "Difference",
                Color = Android.Graphics.Color.ParseColor("#d500f9")
            });

            _eventService.DatabaseUpdatedEvent += (s, e) =>
            {
                if ((e as DatabaseUpdatedEventArgs<FuelCheck>).Update == UpdateType.Add) observableCollection.Add((e as DatabaseUpdatedEventArgs<FuelCheck>).Item);
                else if ((e as DatabaseUpdatedEventArgs<FuelCheck>).Update == UpdateType.Delete) observableCollection.Remove((e as DatabaseUpdatedEventArgs<FuelCheck>).Item);

            };
            return view;
        }

        public static FuelChecksChartFragment NewInstance()
        {
            var fuelChecksChartFragment = new FuelChecksChartFragment { Arguments = new Bundle() };
            return fuelChecksChartFragment;
        }
    }
}