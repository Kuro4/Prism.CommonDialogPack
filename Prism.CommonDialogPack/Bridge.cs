using Prism.CommonDialogPack.ViewModels;
using Prism.CommonDialogPack.Views;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack
{
    public static class Bridge
    {
        public static void RegisterDialogs(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<NotificationDialog, NotificationDialogViewModel>();
            containerRegistry.RegisterDialog<ConfirmationDialog, ConfirmationDialogViewModel>();
        }

        public static void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDialogService, StyleableDialogService>();
        }
    }
}
