using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Windows;

namespace Prism.CommonDialogPack
{
    public class StyleableDialogService : DialogService
    {
        public StyleableDialogService(IContainerExtension containerExtension) : base(containerExtension)
        {
        }

        protected override void ConfigureDialogWindowContent(string dialogName, IDialogWindow window, IDialogParameters parameters)
        {
            base.ConfigureDialogWindowContent(dialogName, window, parameters);

            if (!(window is Window hostWindow) || !(window.DataContext is IStyleableDialogAware styleableVM))
                return;
            if (styleableVM.WindowStyle is null)
            {
                styleableVM.WindowStyle = new Style(typeof(Window));
                styleableVM.WindowStyle.Setters.Add(new Setter(Window.ResizeModeProperty, styleableVM.ResizeMode));
                styleableVM.WindowStyle.Setters.Add(new Setter(Window.SizeToContentProperty, styleableVM.SizeToContent));
                styleableVM.WindowStyle.Setters.Add(new Setter(Window.WidthProperty, styleableVM.Width));
                styleableVM.WindowStyle.Setters.Add(new Setter(Window.HeightProperty, styleableVM.Height));
            }
            hostWindow.SetBinding(FrameworkElement.StyleProperty, nameof(IStyleableDialogAware.WindowStyle));
            hostWindow.WindowStartupLocation = styleableVM.StartupLocation;
        }
    }
}
