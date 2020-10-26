﻿using Android.OS;
using Android.Util;
using Android.Widget;
using AndroidX.Fragment.App;
using System;

namespace The_Boeing_737NG_App.Fragments
{
    public class DatePickerFragment : DialogFragment, Android.App.DatePickerDialog.IOnDateSetListener
    {
        // TAG can be any string of your choice.
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();

        // Initialize this value to prevent NullReferenceExceptions.
        Action<DateTime> _dateSelectedHandler = delegate { };

        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment datePickerFragment = new DatePickerFragment
            {
                _dateSelectedHandler = onDateSelected
            };
            return datePickerFragment;
        }

        public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            Android.App.DatePickerDialog dialog = new Android.App.DatePickerDialog(Activity, this, currently.Year, currently.Month - 1, currently.Day);
            return dialog;
        }

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }
    }
}