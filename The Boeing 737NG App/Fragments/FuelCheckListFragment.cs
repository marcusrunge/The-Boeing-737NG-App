using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using The_Boeing_737NG_App.Models;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class FuelCheckListFragment : Fragment, ActionMode.ICallback
    {
        IFuelCheckListService _fuelCheckListService;
        ISettingsService _settingsService;
        IDatabaseService _databaseService;
        IEventService _eventService;
        FuelChecksRecyclerViewAdapter _fuelChecksRecyclerViewAdapter;
        List<FuelCheck> _fuelCheckList;
        View _createdView;
        ActionMode _actionMode;
        int _actionModeItemId;
        View _actionModeItemView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _settingsService = ServiceLocator.Current.GetInstance<ISettingsService>("SettingsService");
            _fuelCheckListService = ServiceLocator.Current.GetInstance<IFuelCheckListService>("FuelCheckListService");
            _databaseService = ServiceLocator.Current.GetInstance<IDatabaseService>("DatabaseService");
            _eventService = ServiceLocator.Current.GetInstance<IEventService>("EventService");
            _fuelCheckList = new List<FuelCheck>();
            _fuelCheckList = _databaseService.SelectTableAsList<FuelCheck>();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fuelcheck_list_fragment, container, false);
            var fuelcheckListToolbar = view.FindViewById<Android.Widget.Toolbar>(Resource.Id.fuelcheckListToolbar);
            var fuelchecksRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.fuelchecksRecyclerView);
            var layoutManager = new LinearLayoutManager(Context);
            _fuelChecksRecyclerViewAdapter = new FuelChecksRecyclerViewAdapter(_fuelCheckList);
            fuelchecksRecyclerView.SetLayoutManager(layoutManager);
            fuelchecksRecyclerView.SetAdapter(_fuelChecksRecyclerViewAdapter);

            fuelcheckListToolbar.InflateMenu(Resource.Menu.FuelCheckListActionBarMenu);
            fuelcheckListToolbar.MenuItemClick += (s, e) =>
            {
                view.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
                _databaseService.ClearTable<FuelCheck>();
                _fuelCheckList.Clear();
                _eventService.OnDatabaseUpdatedEvent(new DatabaseUpdatedEventArgs<FuelCheck>(null, UpdateType.Delete));
            };

            _eventService.DatabaseUpdatedEvent += (s, e) =>
            {
                if ((e as DatabaseUpdatedEventArgs<FuelCheck>).Update == UpdateType.Add) _fuelCheckList.Add((e as DatabaseUpdatedEventArgs<FuelCheck>).Item);
                _fuelChecksRecyclerViewAdapter.NotifyDataSetChanged();
            };

            _fuelChecksRecyclerViewAdapter.ItemLongClick += (s, e) =>
            {
                _actionModeItemView = (e.Item1 as LinearLayout);
                _actionModeItemView.SetBackgroundColor(Color.ParseColor("#80ff0000"));
                _actionModeItemView.PerformHapticFeedback(FeedbackConstants.LongPress, FeedbackFlags.IgnoreGlobalSetting);
                _actionModeItemId = e.Item2;
                if (_actionMode != null) return;
                fuelcheckListToolbar.Visibility = ViewStates.Gone;
                _actionMode = view.StartActionMode(this);
            };

            _createdView = view;
            return view;
        }

        public static FuelCheckListFragment NewInstance()
        {
            var fuelCheckListFragment = new FuelCheckListFragment { Arguments = new Bundle() };
            return fuelCheckListFragment;
        }

        public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        {
            //_createdView.PerformHapticFeedback(FeedbackConstants.VirtualKey, FeedbackFlags.IgnoreGlobalSetting);
            if (item.TitleFormatted.ToString().Equals("Delete"))
            {
                _databaseService.Delete(_fuelCheckList[_actionModeItemId]);
                _fuelCheckList.RemoveAt(_actionModeItemId);
                _eventService.OnDatabaseUpdatedEvent(new DatabaseUpdatedEventArgs<FuelCheck>(null, UpdateType.Delete));
                _fuelChecksRecyclerViewAdapter.ClickedItem = -1;
                mode.Finish();
            }
            return true;
        }

        public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        {
            mode.MenuInflater.Inflate(Resource.Menu.action_menu, menu);
            return true;
        }

        public void OnDestroyActionMode(ActionMode mode)
        {
            _actionModeItemView.SetBackgroundColor(Color.Transparent);
            _createdView.FindViewById<Android.Widget.Toolbar>(Resource.Id.fuelcheckListToolbar).Visibility = ViewStates.Visible;
            _actionMode = null;
            _fuelChecksRecyclerViewAdapter.ClickedItem = -1;
        }

        public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        {
            return false;
        }
    }

    class FuelChecksRecyclerViewAdapter : RecyclerView.Adapter
    {
        List<FuelCheck> _fuelCheckListList;
        public event EventHandler<Tuple<object, int>> ItemLongClick;
        public int ClickedItem { get; set; }
        View _previousView;

        public FuelChecksRecyclerViewAdapter(List<FuelCheck> fuelCheckList)
        {
            ClickedItem = -1;
            _fuelCheckListList = fuelCheckList;
            ItemLongClick += (s, e) =>
            {
                if (_previousView != null) _previousView.SetBackgroundColor(Color.Transparent);
                ClickedItem = e.Item2;
                _previousView = e.Item1 as LinearLayout;
            };
        }

        public override int ItemCount => _fuelCheckListList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var fuelCheckViewHolder = holder as FuelCheckViewHolder;
            fuelCheckViewHolder.Number.Text = _fuelCheckListList[position].Id.ToString();
            fuelCheckViewHolder.Time.Text = _fuelCheckListList[position].Time + " UTC";
            fuelCheckViewHolder.DeltaFuel.Text = _fuelCheckListList[position].Difference.ToString();
            holder.ItemView.SetBackgroundColor(ClickedItem == position ? Color.ParseColor("#80ff0000") : Color.Transparent);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.fuelcheck_listitem_datatemplate, parent, false);
            return new FuelCheckViewHolder(view, OnItemLongClick);
        }

        public void OnItemLongClick(Tuple<object, int> e) => ItemLongClick?.Invoke(this, e);

        private class FuelCheckViewHolder : RecyclerView.ViewHolder
        {
            public TextView Number { get; set; }
            public TextView Time { get; set; }
            public TextView DeltaFuel { get; set; }
            public FuelCheckViewHolder(View view, Action<Tuple<object, int>> action) : base(view)
            {
                Number = view.FindViewById<TextView>(Resource.Id.fuelcheckNumberTextView);
                Time = view.FindViewById<TextView>(Resource.Id.fuelcheckTimeTextView);
                DeltaFuel = view.FindViewById<TextView>(Resource.Id.fuelcheckDeltafuelTextView);
                view.LongClick += (s, e) =>
                {
                    view.SetBackgroundColor(Color.ParseColor("#80ff0000"));
                    action(new Tuple<object, int>(view, LayoutPosition));
                };
            }
        }
    }
}