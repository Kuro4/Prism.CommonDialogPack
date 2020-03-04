using Prism.Ioc;
using Prism.CommonDialogPack_Sample.Views;
using System.Windows;
using Prism.CommonDialogPack;

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

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            Bridge.RegisterRequiredTypes(containerRegistry);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            Bridge.RegisterDialogs(containerRegistry);
        }
    }
}
