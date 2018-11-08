using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using RegisterNewClient.Database;
using RegisterNewClient.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace RegisterNewClient.ViewModels
{
	public class RegisterPageViewModel : BindableBase, INavigationAware
    {
        INavigationService NavigationService;
        IPersonService Service;
        IPageDialogService PageDialog;


        private ImageSource photo;
        public ImageSource Photo
        {
            get { return photo; }
            set { SetProperty(ref photo, value); }
        }

        private string namePerson;
        public string NamePerson
        {
            get { return namePerson; }
            set { SetProperty(ref namePerson, value); }
        }
        private int agePerson;
        public int AgePerson
        {
            get { return agePerson; }
            set { SetProperty(ref agePerson, value); }
        }



        private string telephonePerson;
        public string TelephonePerson
        {
            get { return telephonePerson; }
            set { SetProperty(ref telephonePerson, value); }
        }

        private string imagePathPerson;
        public string ImagePathPerson
        {
            get { return imagePathPerson; }
            set { SetProperty(ref imagePathPerson, value); }
        }


        public ICommand AdicionaBDCommand { get; set; }
        public ICommand TakePictureCommand { get; set; }
        public RegisterPageViewModel(INavigationService navigationService,IPageDialogService dialogService, IPersonService service)
        {
            NavigationService = navigationService;
            PageDialog = dialogService;
            AdicionaBDCommand = new DelegateCommand(AddBD);
            TakePictureCommand = new DelegateCommand(TakePicture);
            Service = service;

            Photo = UriImageSource.FromFile("userIcon.png");
            ImagePathPerson = "userIcon.png";

            //Photo = UriImageSource.FromUri(new Uri("http://upload.wikimedia.org/wikipedia/commons/5/53/Golden_Lion_Tamarin_Leontopithecus_rosalia.jpg"));

        }

        private async void AddBD()
        {
            if (string.IsNullOrWhiteSpace(NamePerson) || AgePerson == 0 || string.IsNullOrWhiteSpace(TelephonePerson))
            {
                List<string> elements = new List<string>();
                if (String.IsNullOrWhiteSpace(NamePerson))
                    elements.Add("Nome ");
                if (AgePerson == 0)
                    elements.Add("Idade ");
                if (String.IsNullOrWhiteSpace(TelephonePerson))
                    elements.Add("Telefone");

                await PageDialog.DisplayAlertAsync("Insucesso", $"Os seguintes campos estão vazios: {string.Format("{0}.", string.Join(", ", elements))}", "Ok");
            }
            else {
                var x = TelephonePerson.Count();
                if (x < 14)
                {
                    await PageDialog.DisplayAlertAsync("Insucesso", "Telefone inválido!", "Ok");
                }
                else
                {
                    Person p = new Person() { Name = NamePerson, Age = AgePerson, Telephone = TelephonePerson, ImagePath = ImagePathPerson };
                    var i = Service.AddPersons(p);
                    if (i > 0)
                    {
                        await PageDialog.DisplayAlertAsync("Sucesso", "Registro inserido com sucesso", "OK");

                    }
                    else
                    {
                        await PageDialog.DisplayAlertAsync("Insucesso", "Não foi possivel inserir o registro", "OK");
                    }

                    await NavigationService.NavigateAsync(new Uri("/TestMasterDetailPage/NavigationPage/RegisterPage", UriKind.Absolute));

                }
            }
        }
        private async void TakePicture()
        {
            var Aux = Service.GetAllPersons();
            var Count = Aux.Count();
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await PageDialog.DisplayAlertAsync("No Camera", ":( No camera available.", "OK");
                return;
            }
            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Images",
                    Name = $"ImageClientId{Count}",
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 50,
                    SaveToAlbum = true,
                });

                ImagePathPerson = file.Path;
                Photo = UriImageSource.FromFile(ImagePathPerson);
            }
            catch(Exception e)
            {
                await PageDialog.DisplayAlertAsync("Insucesso", $"{e.Message}", "OK");

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
