using RegisterNewClient.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RegisterNewClient
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConsultPage : ContentPage
	{
        public ConsultPage()
		{            
			InitializeComponent();
            Search.Text = "";
		}

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var x = PickerList.SelectedIndex;
            if (x == 0){
                Search.Keyboard=Keyboard.Numeric;

            }
            else
            {
                Search.Keyboard = Keyboard.Text;
            }
        }
    }
}
    



    
