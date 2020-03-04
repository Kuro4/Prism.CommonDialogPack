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
                { DialogParameterName.Message, message },
                { DialogParameterName.Title, title },
            };
            if (!string.IsNullOrWhiteSpace(buttonText))
            {
                param.Add(DialogParameterName.ButtonText, buttonText);
            }
            dialogService.ShowDialog(DialogName.Notification, param, callBack);
        }

        public static void ShowConfirmation(this IDialogService dialogService, string message, string title, Action<IDialogResult> callBack, string okButtonText = null, string cancelButtonText = null)
        {
            var param = new DialogParameters()
            {
                { DialogParameterName.Message, message },
                { DialogParameterName.Title, title },
            };
            if (!string.IsNullOrWhiteSpace(okButtonText))
            {
                param.Add(DialogParameterName.OKButtonText, okButtonText);
            }
            if (!string.IsNullOrWhiteSpace(cancelButtonText))
            {
                param.Add(DialogParameterName.CancelButtonText, cancelButtonText);
            }
            dialogService.ShowDialog(DialogName.Confirmation, param, callBack);
        }
    }
}
