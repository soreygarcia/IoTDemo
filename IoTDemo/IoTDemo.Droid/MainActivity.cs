using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Gcm.Client;

namespace IoTDemo.Droid
{
    [Activity(Icon = "@drawable/icon", Label = "IoTDemo", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new IoTDemo.AppShared());

            RegisterToNotification();

        }

        public void RegisterToNotification()
        {
            try
            {
                string SenderID = "376158416384"; // Google API Project Number
                // Check to ensure everything's setup right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);
                GcmClient.Register(this, SenderID);
            }
            catch (Exception ex)
            {
                
                //throw;
            }
        }

        public void UnRegisterToNotification()
        {
            GcmClient.UnRegister(this);
        }
    }
}

