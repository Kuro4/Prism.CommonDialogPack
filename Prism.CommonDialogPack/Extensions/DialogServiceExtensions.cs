using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
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
                param.Add(DialogParameterNames.ButtonText, buttonText);
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
                param.Add(DialogParameterNames.OKButtonText, okButtonText);
            if (!string.IsNullOrWhiteSpace(cancelButtonText))
                param.Add(DialogParameterNames.CancelButtonText, cancelButtonText);
            dialogService.ShowDialog(DialogNames.Confirmation, param, callBack);
        }

        public static void ShowFolderSelectDialog(this IDialogService dialogService,
                                                  string title,
                                                  bool canMultiSelect,
                                                  Action<IDialogResult> callBack,
                                                  string folderNameText = null,
                                                  string selectButtonText = null,
                                                  string cancelButtonText = null,
                                                  ExplorerBaseTextResource textResource = null,
                                                  IEnumerable<string> rootFolders = null)
        {
            var param = new DialogParameters
            {
                { DialogParameterNames.Title, title },
                { DialogParameterNames.CanMultiSelect, canMultiSelect },
            };
            if (!string.IsNullOrEmpty(folderNameText))
                param.Add(DialogParameterNames.FolderNameText, folderNameText);
            if (!string.IsNullOrEmpty(selectButtonText))
                param.Add(DialogParameterNames.SelectButtonText, selectButtonText);
            if (!string.IsNullOrEmpty(cancelButtonText))
                param.Add(DialogParameterNames.CancelButtonText, cancelButtonText);
            if (textResource != null)
                param.Add(DialogParameterNames.TextResource, textResource);
            if (rootFolders != null)
                param.Add(DialogParameterNames.RootFolders, rootFolders);
            dialogService.ShowDialog(DialogNames.FolderSelectDialog, param, callBack);
        }

        public static void ShowFileSelectDialog(this IDialogService dialogService,
                                                string title,
                                                bool canMultiSelect,
                                                Action<IDialogResult> callBack,
                                                string fileNameText = null,
                                                string selectButtonText = null,
                                                string cancelButtonText = null,
                                                ExplorerBaseTextResource textResource = null,
                                                IEnumerable<FileFilter> filters = null,
                                                IEnumerable<string> rootFolders = null)
        {
            var param = new DialogParameters
            {
                { DialogParameterNames.Title, title },
                { DialogParameterNames.CanMultiSelect, canMultiSelect },
            };
            if (!string.IsNullOrEmpty(fileNameText))
                param.Add(DialogParameterNames.FileNameText, fileNameText);
            if (!string.IsNullOrEmpty(selectButtonText))
                param.Add(DialogParameterNames.SelectButtonText, selectButtonText);
            if (!string.IsNullOrEmpty(cancelButtonText))
                param.Add(DialogParameterNames.CancelButtonText, cancelButtonText);
            if (textResource != null)
                param.Add(DialogParameterNames.TextResource, textResource);
            if (filters != null)
                param.Add(DialogParameterNames.Filters, filters);
            if (rootFolders != null)
                param.Add(DialogParameterNames.RootFolders, rootFolders);
            dialogService.ShowDialog(DialogNames.FileSelectDialog, param, callBack);
        }

        public static void ShowFileSaveDialog(this IDialogService dialogService,
                                              string title,
                                              Action<IDialogResult> callBack,
                                              string fileNameText = null,
                                              string fileTypeText = null,
                                              string saveButtonText = null,
                                              string cancelButtonText = null,
                                              ExplorerBaseTextResource textResource = null,
                                              IEnumerable<FileFilter> filters = null,
                                              IEnumerable<string> rootFolders = null,
                                              string overwriteConfirmationTitle = null,
                                              Func<string, string> overwriteConfirmationMessageFunc = null,
                                              string overwriteConfirmationOKButtonText = null,
                                              string overwriteConfirmationCancelButtonText = null)
        {
            var param = new DialogParameters
            {
                { DialogParameterNames.Title, title }
            };
            if (!string.IsNullOrEmpty(fileNameText))
                param.Add(DialogParameterNames.FileNameText, fileNameText);
            if (!string.IsNullOrEmpty(fileTypeText))
                param.Add(DialogParameterNames.FileTypeText, fileTypeText);
            if (!string.IsNullOrEmpty(saveButtonText))
                param.Add(DialogParameterNames.SaveButtonText, saveButtonText);
            if (textResource != null)
                param.Add(DialogParameterNames.TextResource, textResource);
            if (filters != null)
                param.Add(DialogParameterNames.Filters, filters);
            if (rootFolders != null)
                param.Add(DialogParameterNames.RootFolders, rootFolders);
            if (!string.IsNullOrEmpty(overwriteConfirmationTitle))
                param.Add(DialogParameterNames.OverwriteConfirmationTitle, overwriteConfirmationTitle);
            if (overwriteConfirmationMessageFunc != null)
                param.Add(DialogParameterNames.OverwriteConfirmationMessageFunc, overwriteConfirmationMessageFunc);
            if (!string.IsNullOrEmpty(overwriteConfirmationOKButtonText))
                param.Add(DialogParameterNames.OverwriteConfirmationOKButtonText, overwriteConfirmationOKButtonText);
            if (!string.IsNullOrEmpty(overwriteConfirmationCancelButtonText))
                param.Add(DialogParameterNames.OverwriteConfirmationCancelButtonText, overwriteConfirmationCancelButtonText);
            dialogService.ShowDialog(DialogNames.FileSaveDialog, param, callBack);
        }
    }
}
