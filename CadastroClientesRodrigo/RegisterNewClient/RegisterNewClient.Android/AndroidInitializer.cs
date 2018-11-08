using Android.Content;
using Prism;
using Prism.Ioc;
using RegisterNewClient.Droid.PlatformCode;
using RegisterNewClient.Infrastructure;
using System.ComponentModel;



namespace RegisterNewClient.Droid
{
    public class AndroidInitializer: IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry container)
        {
            container.Register<IFileSystem, AndroidFileSystem>();
            container.Register<IPCService, AndroidLauncher>();
            container.Register<IShare, AndroidShare>();
        }
    }
}