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
using RegisterNewClient.Infrastructure;

namespace RegisterNewClient.Droid.PlatformCode
{
    class AndroidLauncher : IPCService
    {
        public void Launch(string command)
        {
            var launchIntent = Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(command);
            if (launchIntent != null)
            {
                Android.App.Application.Context.StartActivity(launchIntent);
            }
        }
    }
}