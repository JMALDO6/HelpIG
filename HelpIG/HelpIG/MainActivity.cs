using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Wearable.Views;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Support.Wearable.Activity;
using Java.Interop;
using Android.Views.Animations;
using Android.Hardware;

namespace HelpIG
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : WearableActivity, ISensorEventListener
    {
        private TextView _textView;
        private SensorManager mSensorManager;
        private float currentValue = 0f;

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            //throw new NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.HeartRate && e.Values.Count > 0)    
            {
                float newValue = (float)Math.Round(e.Values[0]);
                //Log.d(LOG_TAG,sensorEvent.sensor.getName() + " changed to: " + newValue);
                // only do something if the value differs from the value before and the value is not 0.
                if (currentValue != newValue && newValue != 0)
                {
                    // save the new value
                    currentValue = newValue;
                    _textView.Text = currentValue.ToString();
                }
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.activity_main);

            _textView = FindViewById<TextView>(Resource.Id.text);
            //SetAmbientEnabled();

            mSensorManager = (SensorManager)GetSystemService(Context.SensorService);
            Sensor mHeartRate = mSensorManager.GetDefaultSensor(SensorType.HeartRate);
            mSensorManager.RegisterListener(this, mHeartRate, SensorDelay.Fastest);
        }

        protected override void OnResume()
        {
            base.OnResume();
            // Listen sensor manger on resume activity of app
            mSensorManager.RegisterListener(this, mSensorManager.GetDefaultSensor(SensorType.HeartRate), SensorDelay.Fastest);
        }

        protected override void OnPause()
        {
            base.OnPause();
            // stop listening if app is no longer used. This is importent step otherwise it will drain your watch battery.
            mSensorManager.UnregisterListener(this);
        }
    }
}


