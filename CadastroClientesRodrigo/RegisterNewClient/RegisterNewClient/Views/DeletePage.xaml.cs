using RegisterNewClient.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterNewClient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeletePage : ContentPage
	{
       

        public DeletePage ()
		{
   

            InitializeComponent();
           

        }

        private void PickerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var x = PickerList.SelectedIndex;
            if (x == 0)
            {
                Search.Keyboard = Keyboard.Numeric;

            }
            else
            {
                Search.Keyboard = Keyboard.Text;
            }
        }





        
    }






}
