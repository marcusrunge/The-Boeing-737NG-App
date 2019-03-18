using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CommonServiceLocator;
using System;
using System.Collections.Generic;
using The_Boeing_737NG_App.Models;
using The_Boeing_737NG_App.Services;

namespace The_Boeing_737NG_App.Fragments
{
    public class CircuitBreakerFragment : Android.Support.V4.App.Fragment
    {
        ICircuitBreakerService _circuitBreakerService;
        Android.Widget.SearchView _circuitbreakerSearchView;
        RecyclerView _circuitBreakerRecyclerView;
        CircuitBreakerRecyclerViewAdapter _circuitBreakerRecyclerViewAdapter;
        List<CircuitBreaker> _circuitBreakers;
        LinearLayout
            _circuitbreakerPanel6LinearLayout,
            _circuitbreakerPanel61LinearLayout,
            _circuitbreakerPanel62LinearLayout,
            _circuitbreakerPanel63LinearLayout,
            _circuitbreakerPanel64LinearLayout,
            _circuitbreakerPanel611LinearLayout,
            _circuitbreakerPanel612LinearLayout,
            _circuitbreakerPanel180LinearLayout,
            _circuitbreakerPanel181LinearLayout,
            _circuitbreakerPanel182LinearLayout,
            _circuitbreakerPanel183LinearLayout;
        TextView _circuitbreakerEECompartmentTextView;
        int _previousItemId = -1;
        View _previousView;

        public enum Panel { None, P6, P61, P62, P63, P64, P611, P612, P180, P181, P182, P183, P9X };
        Panel _panel;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _circuitBreakerService = ServiceLocator.Current.GetInstance<ICircuitBreakerService>("CircuitBreakerService");
            _circuitBreakers = _circuitBreakerService.Search(null);
        }
        public static CircuitBreakerFragment NewInstance() => new CircuitBreakerFragment { Arguments = new Bundle() };
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.circuit_breaker_fragment, null);
            _circuitbreakerPanel6LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel6LinearLayout);
            _circuitbreakerPanel61LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel61LinearLayout);
            _circuitbreakerPanel62LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel62LinearLayout);
            _circuitbreakerPanel63LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel63LinearLayout);
            _circuitbreakerPanel64LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel64LinearLayout);
            _circuitbreakerPanel611LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel611LinearLayout);
            _circuitbreakerPanel612LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel612LinearLayout);
            _circuitbreakerPanel180LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel180LinearLayout);
            _circuitbreakerPanel181LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel181LinearLayout);
            _circuitbreakerPanel182LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel182LinearLayout);
            _circuitbreakerPanel183LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.circuitbreakerPanel183LinearLayout);
            _circuitbreakerEECompartmentTextView = view.FindViewById<TextView>(Resource.Id.circuitbreakerEECompartmentTextView);
            _circuitbreakerSearchView = view.FindViewById<Android.Widget.SearchView>(Resource.Id.circuitbreakerSearchView);
            _circuitBreakerRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.circuitBreakerRecyclerView);
            var layoutManager = new LinearLayoutManager(Context);
            var circuitBreakerItemDecorator = new CircuitBreakerItemDecorator(Context);
            _circuitBreakerRecyclerViewAdapter = new CircuitBreakerRecyclerViewAdapter(_circuitBreakers);
            _circuitBreakerRecyclerView.SetLayoutManager(layoutManager);
            _circuitBreakerRecyclerView.SetAdapter(_circuitBreakerRecyclerViewAdapter);
            _circuitBreakerRecyclerView.AddItemDecoration(circuitBreakerItemDecorator);
            _circuitbreakerSearchView.Click += (s, e) =>
            {
                (s as Android.Widget.SearchView).SetIconifiedByDefault(false);
                //InputMethodManager inputMethodManager = Activity.GetSystemService(Context.InputMethodService) as InputMethodManager;
                //inputMethodManager.ShowSoftInput(view, ShowFlags.Forced);
                //inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
            };
            _circuitbreakerSearchView.QueryTextChange += (s, e) =>
            {
                if (String.IsNullOrEmpty(e.NewText) || String.IsNullOrWhiteSpace(e.NewText)) ResetHighLightedSelectedPanel();
                _circuitBreakers.Clear();
                _circuitBreakerService.Search(e.NewText.Trim()).ForEach(x => _circuitBreakers.Add(x));
                _circuitBreakerRecyclerViewAdapter.NotifyDataSetChanged();
            };

            _circuitBreakerRecyclerViewAdapter.ItemClick += (s, e) =>
            {
                if (_previousItemId == -1 || _previousItemId == e.Item2) ((View)e.Item1).SetBackgroundColor(Color.ParseColor("#80388e3c"));
                else
                {
                    _previousView.SetBackgroundColor(Color.Transparent);
                    ((View)e.Item1).SetBackgroundColor(Color.ParseColor("#80388e3c"));
                }
                var circuitBreaker = (CircuitBreaker)_circuitBreakerRecyclerViewAdapter.GetItem(e.Item2);
                ResetHighLightedSelectedPanel();
                if (circuitBreaker.Panel.Equals("P6")) _panel = Panel.P6;
                if (circuitBreaker.Panel.Equals("P6-1")) _panel = Panel.P61;
                if (circuitBreaker.Panel.Equals("P6-2")) _panel = Panel.P62;
                if (circuitBreaker.Panel.Equals("P6-3")) _panel = Panel.P63;
                if (circuitBreaker.Panel.Equals("P6-4")) _panel = Panel.P64;
                if (circuitBreaker.Panel.Equals("P6-11")) _panel = Panel.P611;
                if (circuitBreaker.Panel.Equals("P6-12")) _panel = Panel.P612;
                if (circuitBreaker.Panel.Equals("P18-0")) _panel = Panel.P180;
                if (circuitBreaker.Panel.Equals("P18-1")) _panel = Panel.P181;
                if (circuitBreaker.Panel.Equals("P18-2")) _panel = Panel.P182;
                if (circuitBreaker.Panel.Equals("P18-3")) _panel = Panel.P183;
                if (circuitBreaker.Panel.Equals("P91") | circuitBreaker.Panel.Equals("P92")) _panel = Panel.P9X;
                HighLightSelectedPanel();
                _previousView = (View)e.Item1;
                _previousItemId = e.Item2;
            };

            /*_circuitbreakerPanel6LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P6)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel );
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };*/

            _circuitbreakerPanel61LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P61)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            _circuitbreakerPanel62LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P62)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            _circuitbreakerPanel63LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P63)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            _circuitbreakerPanel64LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P64)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            _circuitbreakerPanel611LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P611)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            _circuitbreakerPanel612LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P612)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            /*_circuitbreakerPanel180LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P180)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };*/

            _circuitbreakerPanel181LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P181)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            _circuitbreakerPanel182LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P182)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            _circuitbreakerPanel183LinearLayout.Click += (s, e) =>
            {
                if (_panel == Panel.P183)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };

            /*_circuitbreakerEECompartmentTextView.Click += (s, e) =>
            {
                if (_panel == Panel.P9X)
                {
                    Bundle bundle = new Bundle();
                    bundle.PutInt("Panel", (int)_panel);
                    bundle.PutString("Location", _circuitBreakers[_previousItemId].Location);
                    var fragmentTransaction = FragmentManager.BeginTransaction();
                    var circuitBreakerPanelDialogFragment = CircuitBreakerPanelDialogFragment.NewInstance();
                    circuitBreakerPanelDialogFragment.Arguments = bundle;
                    circuitBreakerPanelDialogFragment.Show(fragmentTransaction, "CircuitBreakerPanelDialogFragment");
                }
            };*/

            return view;
        }

        private void ResetHighLightedSelectedPanel()
        {
            switch (_panel)
            {
                case Panel.None:
                    break;
                case Panel.P6:
                    _circuitbreakerPanel6LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P61:
                    _circuitbreakerPanel61LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P62:
                    _circuitbreakerPanel62LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P63:
                    _circuitbreakerPanel63LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P64:
                    _circuitbreakerPanel64LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P611:
                    _circuitbreakerPanel611LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P612:
                    _circuitbreakerPanel612LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P180:
                    _circuitbreakerPanel180LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P181:
                    _circuitbreakerPanel181LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P182:
                    _circuitbreakerPanel182LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P183:
                    _circuitbreakerPanel183LinearLayout.SetBackgroundResource(Resource.Xml.border);
                    break;
                case Panel.P9X:
                    _circuitbreakerEECompartmentTextView.SetBackgroundResource(Resource.Xml.border);
                    break;
                default:
                    break;
            }
        }

        private void HighLightSelectedPanel()
        {
            switch (_panel)
            {
                case Panel.None:
                    break;
                case Panel.P6:
                    _circuitbreakerPanel6LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P61:
                    _circuitbreakerPanel61LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P62:
                    _circuitbreakerPanel62LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P63:
                    _circuitbreakerPanel63LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P64:
                    _circuitbreakerPanel64LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P611:
                    _circuitbreakerPanel611LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P612:
                    _circuitbreakerPanel612LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P180:
                    _circuitbreakerPanel180LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P181:
                    _circuitbreakerPanel181LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P182:
                    _circuitbreakerPanel182LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P183:
                    _circuitbreakerPanel183LinearLayout.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                case Panel.P9X:
                    _circuitbreakerEECompartmentTextView.SetBackgroundResource(Resource.Xml.green_bordercell);
                    break;
                default:
                    break;
            }
        }
    }

    /*public static class JavaLangObjectCaster
    {
        public static T Cast<T>(this Java.Lang.Object javaLangObject) where T : class
        {
            var instanceProperty = javaLangObject.GetType().GetProperty("Instance");
            return instanceProperty == null ? null : instanceProperty.GetValue(javaLangObject, null) as T;
        }
    }*/

    public class CircuitBreakerRecyclerViewAdapter : RecyclerView.Adapter
    {
        List<CircuitBreaker> _circuitBreakerList;
        public event EventHandler<Tuple<object, int>> ItemClick;
        int _clickedItem = -1;
        View _previousView;
        public CircuitBreakerRecyclerViewAdapter(List<CircuitBreaker> circuitBreakerList)
        {
            _circuitBreakerList = circuitBreakerList;
            ItemClick += (s, e) =>
            {
                if (_previousView != null && _previousView.Id != (e.Item1 as LinearLayout).Id) _previousView.SetBackgroundColor(Color.Transparent);
                _clickedItem = e.Item2;
                _previousView = e.Item1 as LinearLayout;
            };
        }

        public override int ItemCount => _circuitBreakerList.Count;
        public Object GetItem(int position)
        {
            return _circuitBreakerList[position];
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var circuitBreakerViewHolder = holder as CircuitBreakerViewHolder;
            circuitBreakerViewHolder.System.Text = _circuitBreakerList[position].System;
            circuitBreakerViewHolder.Panel.Text = _circuitBreakerList[position].Panel;
            circuitBreakerViewHolder.Location.Text = _circuitBreakerList[position].Location;
            holder.ItemView.SetBackgroundColor(_clickedItem == position ? Color.ParseColor("#80388e3c") : Color.Transparent);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.circuitbreaker_listitem_datatemplate, parent, false);
            return new CircuitBreakerViewHolder(view, OnItemClick);
        }

        public void OnItemClick(Tuple<object, int> e) => ItemClick?.Invoke(this, e);

        class CircuitBreakerViewHolder : RecyclerView.ViewHolder
        {
            public TextView System { get; set; }
            public TextView Panel { get; set; }
            public TextView Location { get; set; }
            public CircuitBreakerViewHolder(View view, Action<Tuple<object, int>> action) : base(view)
            {
                System = view.FindViewById<TextView>(Resource.Id.circuitBreakerSystemTextView);
                Panel = view.FindViewById<TextView>(Resource.Id.circuitBreakerPanelTextView);
                Location = view.FindViewById<TextView>(Resource.Id.circuitBreakerLocationTextView);
                view.Click += (s, e) =>
                {
                    //view.SetBackgroundColor(Color.ParseColor("#80388e3c"));
                    action(new Tuple<object, int>(view, LayoutPosition));
                };
            }
        }
    }

    class CircuitBreakerItemDecorator : RecyclerView.ItemDecoration
    {
        Drawable drawable;
        public CircuitBreakerItemDecorator(Context context)
        {
            drawable = context.GetDrawable(Resource.Drawable.line_divider);
        }
        public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
        {
            base.OnDraw(c, parent, state);
            int left = parent.PaddingLeft;
            int right = parent.Width - parent.PaddingRight;
            for (int i = 0; i < parent.ChildCount; i++)
            {
                View child = parent.GetChildAt(i);

                ViewGroup.MarginLayoutParams parameters = (ViewGroup.MarginLayoutParams)child.LayoutParameters;

                int top = child.Bottom + parameters.BottomMargin;
                int bottom = top + drawable.IntrinsicHeight;

                drawable.SetBounds(left, top, right, bottom);
                drawable.Draw(c);
            }
        }
    }
}