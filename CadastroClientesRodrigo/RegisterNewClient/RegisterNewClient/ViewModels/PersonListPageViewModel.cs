using RegisterNewClient.Database;
using System;
using System.Collections.Generic;
using System.Text;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using RegisterNewClient.Service;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using RegisterNewClient.Infrastructure;
using System.Collections;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RegisterNewClient.ViewModels
{
    public class PersonListPageViewModel: BindableBase, INavigationAware
    {
        private IQueryable<Person> persons;
        public IQueryable<Person> Persons
        {
            get { return persons; }
            set { SetProperty(ref persons, value); }
        }

        private bool visibilityActivity;
        public bool VisibilityActivity
        {
            get { return visibilityActivity; }
            set { SetProperty(ref visibilityActivity, value);}
        }

        private bool visibilityList;
        public bool VisibilityList
        {
            get { return visibilityList; }
            set { SetProperty(ref visibilityList, value); }
        }

        INavigationService NavigationService;
        IRestService RestService;
        IPageDialogService PageDialog;
        IPersonService PersonService;
        IFileSystem FileSystem;

        public ICommand DeletePeopleCommand { get; set; }
        public ICommand SendPeopleCommand { get; set; }
        public ICommand RefreshDataPeopleCommand { get; set; }



        public PersonListPageViewModel(INavigationService navigationService, IRestService restService, IPageDialogService dialogService, IPersonService service, IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            PersonService = service;
            PageDialog = dialogService;
            NavigationService = navigationService;
            RestService = restService;
            SendPeopleCommand = new DelegateCommand(SendPeople);
            DeletePeopleCommand = new DelegateCommand(DeletePeople);
            RefreshDataPeopleCommand = new DelegateCommand(RefreshDataPeople);
        }



        
        public async void DeletePeople()
        { 
            
            VisibilityList = false;
            VisibilityActivity = true;
            var aux = await RestService.DeleteAllPeopleAsync();
            if (aux == 0)
            {
                await PageDialog.DisplayAlertAsync("Erro", "Problemas de Conexão!", "Ok");
            }
            else if (aux == -1)
            {
                await PageDialog.DisplayAlertAsync("Erro", "Não há nenhum registro no servidor!", "Ok");
            }
            else
            {
                await PageDialog.DisplayAlertAsync("Sucesso", "Registros deletados do servidor!", "Ok");
            }
            VisibilityActivity = false;

        }

        public async void SendPeople()
        {
            var all = PersonService.GetAllPersons();
            VisibilityList = false;
            
            if (all.Count() == 0)
            {
                await PageDialog.DisplayAlertAsync("Erro", "Nenhum registro para ser enviado!", "Ok");

            }
            else
            {
                VisibilityActivity = true;
                var aux = await RestService.SendPeopleAsync(all);
                if (aux == 0)
                {
                    await PageDialog.DisplayAlertAsync("Erro", "Problemas de Conexão!", "Ok");
                }
                else
                {
                    await PageDialog.DisplayAlertAsync("Sucesso", "Registros enviados ao servidor!", "Ok");
                }
                VisibilityActivity = false;
            }

        } 

        public async void RefreshDataPeople()
        {
            VisibilityActivity = true;
            var aux = await RestService.RefreshDataAsync();
            if (aux == 0)
            {
                await PageDialog.DisplayAlertAsync("Erro", "Problemas de Conexão!", "Ok");
            }
            else if(aux == -1)
            {
                await PageDialog.DisplayAlertAsync("Erro", "Não há nenhum registro no servidor!", "Ok");
            }
            else{
                await PageDialog.DisplayAlertAsync("Sucesso", "Registros atualizados no aparelho!", "Ok");
            }
            VisibilityActivity = false;
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
