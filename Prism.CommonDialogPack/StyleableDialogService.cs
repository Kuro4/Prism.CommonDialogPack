using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Prism.CommonDialogPack
{
    public class StyleableDialogService : DialogService
    {
        public StyleableDialogService(IContainerExtension containerExtension) : base(containerExtension)
        {
        }

        protected override void ConfigureDialogWindowProperties(IDialogWindow window, FrameworkElement dialogContent, IDialogAware viewModel)
        {
            var windowStyle = Dialog.GetWindowStyle(dialogContent);
            if (windowStyle != null)
            {
                window.Style = windowStyle;
            }
            else if (window is Window hostWindow && viewModel is IStyleableDialogAware styleableVM)
            {
                styleableVM.WindowStyle ??= new Style(typeof(Window));
                styleableVM.WindowStyle.Setters.Add(new Setter(Window.ResizeModeProperty, styleableVM.ResizeMode));
                styleableVM.WindowStyle.Setters.Add(new Setter(Window.SizeToContentProperty, styleableVM.SizeToContent));
                hostWindow.SetBinding(FrameworkElement.StyleProperty, nameof(IStyleableDialogAware.WindowStyle));
                hostWindow.WindowStartupLocation = styleableVM.StartupLocation;
                hostWindow.Width = styleableVM.Width;
                hostWindow.Height = styleableVM.Height;
            }

            window.Content = dialogContent;
            window.DataContext = viewModel;

            if (window.Owner == null)
            {
                window.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
            }
        }
    }
}
