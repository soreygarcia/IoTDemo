using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Content.PM;

namespace IoTDemo.Droid
{
    [Activity(Label = "IoTDemo", Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ThreadPool.QueueUserWorkItem(o => LoadActivity());
        }
        private void LoadActivity()
        {
            RunOnUiThread(() =>
            {
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            });
        }
    }
}