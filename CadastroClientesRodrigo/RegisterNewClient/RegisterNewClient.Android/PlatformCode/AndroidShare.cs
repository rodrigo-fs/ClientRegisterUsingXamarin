

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Plugin.FilePicker;
using RegisterNewClient.Infrastructure;

namespace RegisterNewClient.Droid.PlatformCode
{
    public class AndroidShare : IShare
    {
        public void Open(string filePath)
        {
            Java.IO.File file = new Java.IO.File(filePath);

            
            var t = Uri.FromFile(file);
            Intent intent = new Intent(Intent.ActionView,t);
            intent.SetDataAndType(t, "text/html");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);
            this.StartActivity(intent);


        }

        public async Task<string> Search()
        {
            var filedata = await CrossFilePicker.Current.PickFile();
            var path= filedata.FilePath;
            return path;

            /*Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("file/*");

            var activity = Android.App.Application.Context as Activity;
            activity.StartActivityForResult(intent, 1*/
        }

        public void Share(string filePath)
        {
        }
    }


    public static class ObjectExtensions
    {
        public static void StartActivity(this object o, Intent intent)
        {
            var context = o as Context;
            if (context != null)
                context.StartActivity(intent);
            else
            {
                intent.SetFlags(ActivityFlags.NewTask);
                Android.App.Application.Context.StartActivity(intent);
            }
        }


    }
}