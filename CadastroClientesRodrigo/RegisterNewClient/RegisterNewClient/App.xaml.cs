using RegisterNewClient.Database;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Unity;
using Prism;
using Prism.Modularity;
using Prism.Ioc;
using RegisterNewClient.ViewModels;
using Prism.Navigation;
using RegisterNewClient.Service;
using System.Linq;
using RegisterNewClient.Infrastructure;
using RegisterNewClient.Json;
using RegisterNewClient.Views;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace RegisterNewClient
{
    public partial class App : PrismApplication
    {
        //PersonContext Db;

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override void OnInitialized()
        {

            InitializeComponent();
            var dbPath = Container.Resolve<IFileSystem>().GetDatabasePath();
            var jsonPath = Container.Resolve<IFileSystem>().GetJsonPath();
            JsonContext Json = new JsonContext(jsonPath);
            PersonContext Context = new PersonContext(dbPath);
            NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage", UriKind.Relative));

        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<ConsultPage, ConsultPageViewModel>();
            containerRegistry.RegisterForNavigation<DeletePage, DeletePageViewModel>();
            containerRegistry.RegisterForNavigation<ShowDataPage, ShowDataPageViewModel>();
            containerRegistry.RegisterForNavigation<PersonPage, PersonListPageViewModel>();
            containerRegistry.RegisterForNavigation<TestMasterDetailPage, TestMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<NavigationPage>();


            containerRegistry.RegisterInstance(new PersonContext());
            containerRegistry.RegisterInstance(new JsonContext());
            containerRegistry.Register<IPersonService, PersonService>();
            containerRegistry.Register<IJsonService, JsonService>();
            containerRegistry.Register<IRestService, RestService>();
            
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle IApplicationLifecycle
            base.OnSleep();

            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle IApplicationLifecycle
            base.OnResume();

            // Handle when your app resumes
        }      
    }
}
