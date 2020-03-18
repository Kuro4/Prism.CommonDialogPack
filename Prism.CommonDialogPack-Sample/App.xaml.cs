using Prism.Ioc;
using Prism.CommonDialogPack_Sample.Views;
using System.Windows;
using Prism.CommonDialogPack;
using Prism.Modularity;

namespace Prism.CommonDialogPack_Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<CommonDialogPackModule>();
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            CommonDialogPackModule.RegisterRequiredTypes(containerRegistry);
        }
    }
}
