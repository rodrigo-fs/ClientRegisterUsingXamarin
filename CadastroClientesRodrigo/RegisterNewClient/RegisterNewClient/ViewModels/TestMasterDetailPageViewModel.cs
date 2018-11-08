using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RegisterNewClient.ViewModels
{
	public class TestMasterDetailPageViewModel : BindableBase, INavigationAware
    {
        INavigationService NavigationService;

        public TestMasterDetailPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {


        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }
    }
}
