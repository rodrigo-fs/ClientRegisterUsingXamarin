using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RegisterNewClient.Database;
using RegisterNewClient.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace RegisterNewClient.ViewModels
{
    public class ShowDataPageViewModel : BindableBase, INavigationAware
    {
        IPersonService Service;
        INavigationService NavigationService;

       
        
        private string selectedItem;
        public string SelectedItem
        {
            get { return selectedItem; }
            set { SetProperty(ref selectedItem, value);
            }
                            
        }

        private int indexPicker;
        public int IndexPicker
        {
            get { return indexPicker; }
            set
            { SetProperty(ref indexPicker, value);
                ShowPerson();
            }
        }

        private List<string> options;
        public List<string> Options
        {
            get { return options; }
            set { SetProperty(ref options, value); }
        }

        private IQueryable<Person> persons;
        public IQueryable<Person> Persons
        {
            get { return persons; }
            set { SetProperty(ref persons, value); }
        }

        private Person personSelected;
        public Person PersonSelected
        {
            get { return personSelected; }
            set
            { SetProperty(ref personSelected, value);
            }
        }

        private bool personVisibility;
        public bool PersonVisibility
        {
            get { return personVisibility; }
            set { SetProperty(ref personVisibility, value); }
        }

        
        public ICommand ShowPCommand { get; set; }
        public ICommand OnTappedListCommand { get; set; }
        public ICommand OnBackButtonCommand { get; set; }





        public ShowDataPageViewModel(INavigationService navigationService,IPersonService service)
        {
            Service = service;
            ShowPCommand = new DelegateCommand(ShowPerson);
            OnTappedListCommand = new DelegateCommand(OnTappedList);
            OnBackButtonCommand = new DelegateCommand(OnBackButton);
            var Aux = new List<string>();
            Aux.Add("Ordenação por ID");
            Aux.Add("Ordenação por idade");
            Options = Aux;
            SelectedItem = "Ordenação por ID";
            Persons = Service.GetAllPersons();
            NavigationService = navigationService;

        }

        private void ShowPerson() {
            if (IndexPicker == 0)
            {
              
                Persons = Service.GetAllPersons();
            }

            else if (IndexPicker == 1)
            {
                Persons = Service.GetAllPersonsOrderByAge();
            }
        }

        private void OnTappedList()
        {
            PersonVisibility = true;
        }

        private void OnBackButton()
        {
            PersonVisibility = false;
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
