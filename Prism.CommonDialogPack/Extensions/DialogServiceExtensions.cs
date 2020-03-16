using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack.Extensions
{
    public static class DialogServiceExtensions
    {
        public static void ShowNotification(this IDialogService dialogService, string message, string title, Action<IDialogResult> callBack, string buttonText = null)
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Message, message },
                { DialogParameterNames.Title, title },
            };
            if (!string.IsNullOrWhiteSpace(buttonText))
            {
                param.Add(DialogParameterNames.ButtonText, buttonText);
            }
            dialogService.ShowDialog(DialogNames.Notification, param, callBack);
        }

        public static void ShowConfirmation(this IDialogService dialogService, string message, string title, Action<IDialogResult> callBack, string okButtonText = null, string cancelButtonText = null)
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Message, message },
                { DialogParameterNames.Title, title },
            };
            if (!string.IsNullOrWhiteSpace(okButtonText))
            {
                param.Add(DialogParameterNames.OKButtonText, okButtonText);
            }
            if (!string.IsNullOrWhiteSpace(cancelButtonText))
            {
                param.Add(DialogParameterNames.CancelButtonText, cancelButtonText);
            }
            dialogService.ShowDialog(DialogNames.Confirmation, param, callBack);
        }
    }
}
