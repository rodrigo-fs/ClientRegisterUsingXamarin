using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace RegisterNewClient.Droid
{
    public class CheckPermissions
    {

        private FormsAppCompatActivity Activity;

        public CheckPermissions(FormsAppCompatActivity activity)
        {
            Activity = activity;
            if ((ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                || (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) != Permission.Granted))
            {
                if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                    ActivityCompat.RequestPermissions(Activity, new string[] { Manifest.Permission.WriteExternalStorage }, 1);
                if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) != Permission.Granted)
                    ActivityCompat.RequestPermissions(Activity, new string[] { Manifest.Permission.Camera }, 2);
                if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Internet) != Permission.Granted)
                    ActivityCompat.RequestPermissions(Activity, new string[] { Manifest.Permission.Internet }, 3);

            }
            CheckIfPermited();
        }



        public void CheckIfPermited()
        {
            if ((ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
               || (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) != Permission.Granted))
            {

                List<string> elements = new List<string>();

                if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
                    elements.Add("WriteExternalStorage");

                if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) != Permission.Granted)
                    elements.Add("Camera");
                if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Internet) != Permission.Granted)
                    elements.Add("Internet");

                AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                builder.SetMessage($"As seguintes permissões nao foram concedidas: { string.Format("{0}.", string.Join(", ", elements))}");
                builder.SetPositiveButton("OK", OkAction);
                var warning = builder.Create();
                warning.Show();
            }


        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            var myButton = sender as Button;
            if(myButton != null)
            {
                Activity.Finish();
            }
        }

    }
}