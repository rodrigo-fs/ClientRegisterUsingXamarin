using Prism.Commands;
using Prism.Common;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using RegisterNewClient.Database;
using RegisterNewClient.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace RegisterNewClient.ViewModels
{
    public class ConsultPageViewModel : BindableBase, INavigationAware
    {

        INavigationService NavigationService;
        IPersonService Service;
        IPageDialogService PageDialog;

        private int indexPicker;
        public int IndexPicker
        {
            get { return indexPicker; }
            set { SetProperty(ref indexPicker, value);
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


        public ICommand SearchPCommand { get; set; }


        public ConsultPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IPersonService service)
        {
            var Aux = new List<string>();
            Aux.Add("Procura por ID");
            Aux.Add("Procura por Nome");
            Selected = Aux;
            LabelTypeOfSearch = "ID";
            IndexPicker = 0;
            NavigationService = navigationService;
            PageDialog = dialogService;
            SearchPCommand = new DelegateCommand(SearchPerson);
            Service = service;
        }

        private async void SearchPerson()
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
                if (IndexPicker == 0)
                {
                    int id;
                    try
                    {
                        id = int.Parse(Identifier);
                    }
                    catch (Exception ex)
                    {
                        id = -1;
                    }
                    if (id >= 0)
                    {
                        var lista = Service.GetPersons(Identifier, IndexPicker);
                        bool anyRegister = lista.Any();
                        if (anyRegister)
                        {
                            Persons = lista;

                        }
                        else
                        {
                            await PageDialog.DisplayAlertAsync("Insucesso", "Nenhum registro encontrado", "OK");
                            await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/ConsultPage", UriKind.Absolute));

                        }
                    }
                    else
                    {
                        await PageDialog.DisplayAlertAsync("Insucesso", "Indíce inserido inválido", "OK");
                        await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/ConsultPage", UriKind.Absolute));

                    }

                }
                else
                {
                    var list = Service.GetPersons(Identifier, IndexPicker);
                    bool anyRegister = list.Any();
                    if (anyRegister)
                    {
                        Persons = list;
                    }
                    else
                    {
                        await PageDialog.DisplayAlertAsync("Insucesso", "Nenhum registro encontrado", "OK");
                        await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/ConsultPage", UriKind.Absolute));


                    }
                }
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
