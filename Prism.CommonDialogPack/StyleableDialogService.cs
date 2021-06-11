using Prism.Common;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;
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
            base.ConfigureDialogWindowProperties(window, dialogContent, viewModel);
            if (window is Window hostWindow && viewModel is IStyleableDialogAware styleableVM)
            {
                hostWindow.SetBinding(FrameworkElement.StyleProperty, new Binding("WindowStyle"));
                hostWindow.Width = styleableVM.Width;
                hostWindow.Height = styleableVM.Height;
            }
        }
    }
}
