using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using RegisterNewClient.Database;
using RegisterNewClient.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace RegisterNewClient.ViewModels
{
	public class DeletePageViewModel : BindableBase,INavigationAware
    {

        INavigationService NavigationService;
        IPageDialogService PageDialog;
        IPersonService Service;

        private int indexPicker;
        public int IndexPicker
        {
            get { return indexPicker; }
            set
            {
                SetProperty(ref indexPicker, value);
                if (IndexPicker == 0)
                    LabelTypeOfSearch = "ID";
                else
                    LabelTypeOfSearch = "Nome";

            }
        }

        private string labelTypeOfSearch;
        public string LabelTypeOfSearch
        {
            get { return labelTypeOfSearch; }
            set { SetProperty(ref labelTypeOfSearch, value); }
        }

        private List<string> selected;
        public List<string> Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        private string identifier;
        public string Identifier
        {
            get { return identifier; }
            set { SetProperty(ref identifier, value); }
        }
   
        private IQueryable<Person> persons;

        public IQueryable<Person> Persons
        {
            get { return persons; }
            set { SetProperty(ref persons, value); }
        }

        private bool personVisibility;
        public bool PersonVisibility
        {
            get { return personVisibility; }
            set { SetProperty(ref personVisibility, value); }
        }

        private Person personSelected;
        public Person PersonSelected
        {
            get { return personSelected; }
            set
            {
                SetProperty(ref personSelected, value);
            }
        }

        public ICommand DeletePCommand { get; set; }
        public ICommand OnBackButtonCommand { get; set; }
        public ICommand OnDeleteButtonCommand { get; set; }
        public ICommand OnTappedListCommand { get; set; }



        public DeletePageViewModel(INavigationService navigationService, IPageDialogService dialogService, IPersonService service)
        {
            var Aux = new List<string>();
            Aux.Add("Remove por ID");
            Aux.Add("Remove por Nome");
            Selected = Aux;
            LabelTypeOfSearch = "ID";
            IndexPicker = 0;
            NavigationService = navigationService;
            PageDialog = dialogService;
            DeletePCommand = new DelegateCommand(DeletePerson);
            OnBackButtonCommand = new DelegateCommand(OnBackButton);
            OnDeleteButtonCommand = new DelegateCommand(OnDeleteButton);
            OnTappedListCommand = new DelegateCommand(OnTappedList);

            Service = service;
            Persons = Service.GetAllPersons();
        }




        private async void DeletePerson()
        {
            if (string.IsNullOrEmpty(Identifier))
            {
                if (IndexPicker == 0)
                    await PageDialog.DisplayAlertAsync("Erro", "Preencha o campo ID!", "Ok");

                else
                    await PageDialog.DisplayAlertAsync("Erro", "Preencha o campo nome!", "Ok");
            }
            else
            {


                var Verify = Service.DeletePersons(Identifier, indexPicker);
                if (Verify == 1)
                {
                    await PageDialog.DisplayAlertAsync("Sucesso", "Registro removido com sucesso!", "Ok");
                    Persons = Service.GetAllPersons();
                }
                else if (Verify == -1)
                {
                    await PageDialog.DisplayAlertAsync("Insucesso", "Índice inserido inválido", "OK");
                }
                else
                {
                    await PageDialog.DisplayAlertAsync("Insucesso", "Registro não encotrado", "Ok");
                }
                await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/DeletePage", UriKind.Absolute));



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

        private async void OnDeleteButton()
        {
            var personid = PersonSelected.PersonId.ToString();
            var Verify = Service.DeletePersons(personid, 0);
            if (Verify == 1)
            {
                await PageDialog.DisplayAlertAsync("Sucesso", "Registro removido com sucesso!", "Ok");
                await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/DeletePage", UriKind.Absolute));


            }
            else
            {
               await PageDialog.DisplayAlertAsync("Insucesso", "Registro não encotrado", "Ok");
            }

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
