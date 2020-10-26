using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
using System;

namespace The_Boeing_737NG_App.Fragments
{
    public class CircuitBreakerPanelDialogFragment : DialogFragment
    {
        CircuitBreakerFragment.Panel _panel;
        string _location;
        readonly string[] rowLetters = { "A", "B", "C", "D", "E", "F" };

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            if (Arguments != null)
            {
                _location = Arguments.GetString("Location");
                _panel = (CircuitBreakerFragment.Panel)Arguments.GetInt("Panel");
            }
        }

        public static CircuitBreakerPanelDialogFragment NewInstance() => new CircuitBreakerPanelDialogFragment { Arguments = new Bundle() };

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            var panelLayout = GetPanelLayout();
            var view = inflater.Inflate(Resource.Layout.circuit_breaker_panel_fragment, container, false);
            GridLayout circuitBreakerPanelRowIdGridLayout = view.FindViewById<GridLayout>(Resource.Id.circuitBreakerPanelRowIdGridLayout);
            GridLayout circuitBreakerPanelGridLayout = view.FindViewById<GridLayout>(Resource.Id.circuitBreakerPanelGridLayout);
            circuitBreakerPanelRowIdGridLayout.RowCount = panelLayout.Item2 + 1;
            circuitBreakerPanelGridLayout.RowCount = panelLayout.Item2 + 1;
            circuitBreakerPanelGridLayout.ColumnCount = panelLayout.Item1;
            Tuple<int, int> tuple = GetSelectedCircuitBreaker();
            bool valid = (tuple.Item1 > -1 && tuple.Item2 > -1);

            for (int i = 0; i < panelLayout.Item2 + 1; i++)
            {
                for (int j = 0; j < panelLayout.Item1; j++)
                {
                    if (i == 0)
                    {
                        TextView columNumber = new TextView(Context)
                        {
                            Text = (j + 1).ToString(),
                            Gravity = GravityFlags.Center
                        };
                        GridLayout.LayoutParams columNumberLayoutParams = new GridLayout.LayoutParams() { ColumnSpec = GridLayout.InvokeSpec(j), RowSpec = GridLayout.InvokeSpec(i) };
                        columNumberLayoutParams.SetGravity(GravityFlags.CenterHorizontal);
                        columNumber.LayoutParameters = columNumberLayoutParams;
                        circuitBreakerPanelGridLayout.AddView(columNumber);
                    }
                    else if (i != 0)
                    {
                        ImageView imageView = new ImageView(Context);
                        if (valid && i == tuple.Item1 + 1 && j == tuple.Item2 - 1) imageView.SetImageResource(Resource.Drawable.ic_green_cb);
                        else imageView.SetImageResource(Resource.Drawable.ic_black_cb);
                        GridLayout.LayoutParams layoutParams = new GridLayout.LayoutParams() { ColumnSpec = GridLayout.InvokeSpec(j), RowSpec = GridLayout.InvokeSpec(i) };
                        imageView.LayoutParameters = layoutParams;
                        imageView.SetBackgroundResource(Resource.Xml.cb_grid_divider);
                        circuitBreakerPanelGridLayout.AddView(imageView);
                    }
                }
            }

            circuitBreakerPanelGridLayout.GetChildAt((panelLayout.Item1 * panelLayout.Item2) - 1).LayoutChange += (s, e) =>
            {
                for (int i = 0; i < panelLayout.Item2 + 1; i++)
                {
                    if (i > 0)
                    {
                        TextView rowLetter = new TextView(Context)
                        {
                            Text = rowLetters[i - 1]
                        };
                        GridLayout.LayoutParams rowLetterLayoutParams = new GridLayout.LayoutParams() { ColumnSpec = GridLayout.InvokeSpec(0), RowSpec = GridLayout.InvokeSpec(i) };
                        rowLetterLayoutParams.SetGravity(GravityFlags.Center);
                        rowLetter.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.blueGrey100)));
                        rowLetterLayoutParams.Height = (s as View).Height;
                        rowLetter.SetTextSize(Android.Util.ComplexUnitType.Sp, 14);
                        rowLetter.LayoutParameters = rowLetterLayoutParams;
                        //rowLetter.SetBackgroundResource(Resource.Xml.cb_grid_divider);
                        circuitBreakerPanelRowIdGridLayout.AddView(rowLetter);
                    }
                }
            };

            return view;
        }

        private Tuple<int, int> GetPanelLayout()
        {
            int horizonalElementsCount = 0;
            int verticalElementsCount = 0;
            switch (_panel)
            {
                case CircuitBreakerFragment.Panel.None:
                    horizonalElementsCount = 0;
                    verticalElementsCount = 0;
                    break;
                case CircuitBreakerFragment.Panel.P6:
                    horizonalElementsCount = 0;
                    verticalElementsCount = 0;
                    break;
                case CircuitBreakerFragment.Panel.P61:
                    horizonalElementsCount = 17;
                    verticalElementsCount = 5;
                    break;
                case CircuitBreakerFragment.Panel.P62:
                    horizonalElementsCount = 24;
                    verticalElementsCount = 4;
                    break;
                case CircuitBreakerFragment.Panel.P63:
                    horizonalElementsCount = 18;
                    verticalElementsCount = 6;
                    break;
                case CircuitBreakerFragment.Panel.P64:
                    horizonalElementsCount = 18;
                    verticalElementsCount = 6;
                    break;
                case CircuitBreakerFragment.Panel.P611:
                    horizonalElementsCount = 9;
                    verticalElementsCount = 4;
                    break;
                case CircuitBreakerFragment.Panel.P612:
                    horizonalElementsCount = 9;
                    verticalElementsCount = 4;
                    break;
                case CircuitBreakerFragment.Panel.P180:
                    horizonalElementsCount = 1;
                    verticalElementsCount = 1;
                    break;
                case CircuitBreakerFragment.Panel.P181:
                    horizonalElementsCount = 7;
                    verticalElementsCount = 5;
                    break;
                case CircuitBreakerFragment.Panel.P182:
                    horizonalElementsCount = 12;
                    verticalElementsCount = 5;
                    break;
                case CircuitBreakerFragment.Panel.P183:
                    horizonalElementsCount = 19;
                    verticalElementsCount = 6;
                    break;
                case CircuitBreakerFragment.Panel.P9X:
                    break;
                default:
                    break;
            }
            return new Tuple<int, int>(horizonalElementsCount, verticalElementsCount);
        }

        private Tuple<int, int> GetSelectedCircuitBreaker()
        {
            int row = 0;
            int column = 0;

            string rowLetter = _location.Substring(0, 1);
            switch (rowLetter)
            {
                case "A":
                    row = 0;
                    break;
                case "B":
                    row = 1;
                    break;
                case "C":
                    row = 2;
                    break;
                case "D":
                    row = 3;
                    break;
                case "E":
                    row = 4;
                    break;
                case "F":
                    row = 5;
                    break;
                default:
                    row = -1;
                    break;
            }
            if (int.TryParse(_location.Substring(1, 1), out int i))
            {
                column = i;
            }
            else column = -1;
            if (_location.Length > 2 && int.TryParse(_location.Substring(2, 1), out int j))
            {
                column = (column * 10) + j;
            }

            return new Tuple<int, int>(row, column);
        }
    }
}