
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Collections.Generic;
using Prism;
using Prism.Ioc;
using Android;
using Xamarin.Forms.PlatformConfiguration;
using Android.Content;
using System;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.CurrentActivity;

namespace RegisterNewClient.Droid
{
    [Activity(Label = "RegisterNewClient", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            CrossCurrentActivity.Current.Init(this, bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

         
            var permissions = new CheckPermissions(this);
            var platformInitializer = new AndroidInitializer();
            var application = new App(platformInitializer);
            LoadApplication(application);
           
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
