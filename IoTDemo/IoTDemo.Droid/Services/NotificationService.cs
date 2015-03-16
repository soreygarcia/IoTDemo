using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;

//VERY VERY VERY IMPORTANT NOTE!!!!
// Your package name MUST NOT start with an uppercase letter.
// Android does not allow permissions to start with an upper case letter
// If it does you will get a very cryptic error in logcat and it will not be obvious why you are crying!
// So please, for the love of all that is kind on this earth, use a LOWERCASE first letter in your Package Name!!!!
using ByteSmith.WindowsAzure.Messaging;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

[assembly: Permission(Name = "com.avanet.iotdemo.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.avanet.iotdemo.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

namespace IoTDemo.Droid.Services
{
    
    //You must subclass this!
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "com.avanet.iotdemo" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "com.avanet.iotdemo" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "com.avanet.iotdemo" })]
    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<NotificationService>
    {
        public const string SenderID = "376158416384"; // Google API Project Number

        //IMPORTANT: Change this to your own Sender ID!
        //The SENDER_ID is your Google API Console App Project ID.
        //  Be sure to get the right Project ID from your Google APIs Console.  It's not the named project ID that appears in the Overview,
        //  but instead the numeric project id in the url: eg: https://code.google.com/apis/console/?pli=1#project:785671162406:overview
        //  where 785671162406 is the project id, which is the SENDER_ID to use!
        public static string[] SENDER_IDS = new string[] { SenderID };

        public const string TAG = "GoogleCloudMessaging";
    }


    [Service] //Must use the service tag
    public class NotificationService : GcmServiceBase
    {
        // Azure app specific URL and key
        public const string ApplicationURL = @"https://cloudrsmed.servicebus.windows.net/notificationscloudrsmed";
        public const string ApplicationKey = @"AIzaSyAnaF29PFQwsONZXz-9O0yJ5vigsA0dUAw";


        // Azure app specific connection string and hub path
        public const string ConnectionString = "Endpoint=sb://cloudrsmed.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=NIVCHT05pSmLoliGJ2jqQsGabr6Hr3XfZH4ruw+YbTM=";
        public const string NotificationHubPath = "notificationscloudrsmed";

        public static string myRegistrationID { get; private set; }
        private NotificationHub myHub { get; set; }

        public NotificationService()
            : base(PushHandlerBroadcastReceiver.SENDER_IDS)
        {
            Log.Info(PushHandlerBroadcastReceiver.TAG, "NotificationService() constructor");
        }

        protected override async void OnRegistered(Context context, string registrationId)
        {
            if (registrationId != "")
            {
                Log.Verbose(PushHandlerBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
                myRegistrationID = registrationId;



                myHub = new NotificationHub(NotificationHubPath, ConnectionString);
                try
                {
                    await myHub.UnregisterAllAsync(registrationId);
                    //await myHub.UnregisterNativeAsync();
                    //await myHub.UnregisterAsync(await myHub.GetRegistrationByIdAsync(registrationId));
                
                    var tags = new List<string>(); // create tags if you want
                    var hubRegistration = await myHub.RegisterNativeAsync(registrationId, tags);
                    Debug.WriteLine("RegistrationId:" + hubRegistration.RegistrationId);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

        }

        protected override async void OnUnRegistered(Context context, string registrationId)
        {
            if (registrationId != "")
            {
                myHub = new NotificationHub(NotificationHubPath, ConnectionString);
                
                await Task.Run(async () =>
                {
                    await myHub.UnregisterNativeAsync();
                    //await myHub.UnregisterAsync(await myHub.GetRegistrationByIdAsync(registrationId));
                });
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {

            Log.Info(PushHandlerBroadcastReceiver.TAG, "Beam");
            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            //Store the message
            var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            var edit = prefs.Edit();
            edit.PutString("last_msg", msg.ToString());
            edit.Commit();

            string message = intent.Extras.GetString("message");
            if (!string.IsNullOrEmpty(message))
            {
                createNotification("New todo item!", "Todo item: " + message);
                return;
            }

            string msg2 = intent.Extras.GetString("msg");
            if (!string.IsNullOrEmpty(msg2))
            {
                createNotification("Beam", msg2);
                return;
            }

            // createNotification("PushSharp-GCM Msg Rec'd", "Message Received for C2DM-Sharp... Tap to View!");
            createNotification("Unknown message details", msg.ToString());
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(PushHandlerBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(PushHandlerBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }

        void createNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show ui
            var uiIntent = new Intent(this, typeof(MainActivity));

            //Create the notification
            var notification = new Notification(Resource.Drawable.Icon, title);

            //Auto cancel will remove the notification once the user touches it
            notification.Flags = NotificationFlags.AutoCancel;

            //Set the notification info
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.
            notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

            //Show the notification
            notificationManager.Notify(1, notification);
        }
    }
}

