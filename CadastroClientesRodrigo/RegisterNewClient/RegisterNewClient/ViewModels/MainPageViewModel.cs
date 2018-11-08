using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RegisterNewClient.Database;
using RegisterNewClient.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Ioc;
using Prism.Unity;
using Prism;
using Prism.Services;
using RegisterNewClient.Infrastructure;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RegisterNewClient.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private ImageSource photo;
        public ImageSource Photo
        {
            get { return photo; }
            set { SetProperty(ref photo, value); }
        }


        IPageDialogService PageDialog;
        IJsonService JsonService;
        INavigationService NavigationService;
        IPersonService Service;
        IShare Share;
        public ICommand GetNavigateRegisterPageCommand { get; set; }
        public ICommand GetNavigateConsultPageCommand { get; set; }
        public ICommand GetNavigateDeletePageCommand{ get; set; }
        public ICommand GetNavigateShowDataPageCommand { get; set; }
        public ICommand GetNavigatePersonPageCommand { get; set; }
        public ICommand GetJsonExport { get; set; }
        public ICommand GetJsonImport { get; set; }

        public MainPageViewModel(INavigationService navigationService,IPersonService service, IJsonService jsonService, IPageDialogService dialogService, IShare share)
        {
            GetNavigateRegisterPageCommand = new DelegateCommand(NavigationRegisterPage);
            GetNavigateConsultPageCommand = new DelegateCommand(NavigationConsultPage);
            GetNavigateDeletePageCommand = new DelegateCommand(NavigationDeletePage);
            GetNavigateShowDataPageCommand = new DelegateCommand(NavigationShowDataPage);
            GetNavigatePersonPageCommand = new DelegateCommand(NavigationPersonListPage);
            GetJsonExport = new DelegateCommand(ExportJSON);
            GetJsonImport = new DelegateCommand(ImportJSON);
            Photo = UriImageSource.FromFile("userIcon.png");


            PageDialog = dialogService;
            Service = service;
            NavigationService = navigationService;
            JsonService = jsonService;
            Share = share;
        }

        async void NavigationRegisterPage()
        {
            await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/RegisterPage", UriKind.Absolute));
        }

        async void NavigationConsultPage()
        {

            await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/ConsultPage", UriKind.Absolute));
        }

        async void NavigationDeletePage()
        {
            await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/DeletePage", UriKind.Absolute));
        }
        async void NavigationShowDataPage()
        {
            await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/ShowDataPage", UriKind.Absolute));
        }
        async void NavigationPersonListPage()
        {
            await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/PersonPage", UriKind.Absolute));

        }

        public void ExportJSON()
        {
            var text = Service.GenerateJson();
            var x = JsonService.WriteJson(text);
            if (x == 1)
            {
               
                Share.Open(JsonService.GetPath());
                PageDialog.DisplayAlertAsync("Sucesso", "Arquivo gerado na pasta", "OK");
            }
            else
                PageDialog.DisplayAlertAsync("Insucesso", "Não foi possível salvar o arquivo", "OK");

        }

        public async void ImportJSON()
        {
            string json;
            List<Person> Persons;
            try
            {
                Task<string> t = Share.Search();
                await t;
            
                json = Convert.ToString(t.Result);
                Persons = JsonService.GenerateJsonText(json);
                Service.DeleteAllPersons();
                foreach (var cliente in Persons)
                {
                    Service.AddPersons(cliente);
                }

                var query = Service.GetAllPersons();
                if (query.Count() == 0)
                    await PageDialog.DisplayAlertAsync("Insucesso", "Não foi possível importar o arquivo", "OK");
                else
                    await PageDialog.DisplayAlertAsync("Sucesso", "Arquivo importado com sucesso", "OK");
            }
            catch(NullReferenceException e)
            {
                await PageDialog.DisplayAlertAsync("Insucesso", "Escolha um arquivo!", "OK");
            }
            catch (Exception e)
            {
                await PageDialog.DisplayAlertAsync("Insucesso", "Arquivo inválido!", "OK");
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
