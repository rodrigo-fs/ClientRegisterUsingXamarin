using Prism.Navigation;
using Xamarin.Forms;

namespace RegisterNewClient.Views
{
    public partial class TestMasterDetailPage : MasterDetailPage, IMasterDetailPageOptions
    {
        public TestMasterDetailPage()
        {
            InitializeComponent();
        }

        public bool IsPresentedAfterNavigation
        {
            get { return false; }
        }
    }
}