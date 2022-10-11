using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.CommonDialogPack.Extensions
{
    /// <summary>
    /// <see cref="IDialogService"/> extensions.
    /// </summary>
    public static class DialogServiceExtensions
    {
        /// <summary>
        /// Show notification dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="message">Notification message.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        /// <param name="buttonText">button text.</param>
        public static void ShowNotification(this IDialogService dialogService,
                                            string message,
                                            string title,
                                            Action<IDialogResult> callBack,
                                            bool isModal = true,
                                            string buttonText = null)
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Message, message },
                { DialogParameterNames.Title, title },
            };
            if (!string.IsNullOrWhiteSpace(buttonText))
                param.Add(DialogParameterNames.ButtonText, buttonText);
            if (isModal)
                dialogService.ShowDialog(DialogNames.Notification, param, callBack);
            else
                dialogService.Show(DialogNames.Notification, param, callBack);
        }
        /// <summary>
        /// Show confirmation dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="message">Confirmation message.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        /// <param name="okButtonText">OK button text.</param>
        /// <param name="cancelButtonText">Cancel button text.</param>
        public static void ShowConfirmation(this IDialogService dialogService,
                                            string message,
                                            string title,
                                            Action<IDialogResult> callBack,
                                            bool isModal = true,
                                            string okButtonText = null,
                                            string cancelButtonText = null)
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
            if (isModal)
                dialogService.ShowDialog(DialogNames.Confirmation, param, callBack);
            else
                dialogService.Show(DialogNames.Confirmation, param, callBack);
        }
        /// <summary>
        /// Show folder select dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="title">Dialog title.</param>
        /// <param name="canMultiSelect">If true; can multi select.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        /// <param name="folderNamePrefixText">Folder name prefix text.</param>
        /// <param name="selectButtonText">Select button text</param>
        /// <param name="cancelButtonText">Cancel button text</param>
        /// <param name="textResource">Text resource for explorer.</param>
        /// <param name="rootFolders">Root folders path.</param>
        public static void ShowFolderSelectDialog(this IDialogService dialogService,
                                                  string title,
                                                  bool canMultiSelect,
                                                  Action<IDialogResult> callBack,
                                                  bool isModal = true,
                                                  string folderNamePrefixText = null,
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
            if (!string.IsNullOrEmpty(folderNamePrefixText))
                param.Add(DialogParameterNames.FolderNamePrefixText, folderNamePrefixText);
            if (!string.IsNullOrEmpty(selectButtonText))
                param.Add(DialogParameterNames.SelectButtonText, selectButtonText);
            if (!string.IsNullOrEmpty(cancelButtonText))
                param.Add(DialogParameterNames.CancelButtonText, cancelButtonText);
            if (textResource != null)
                param.Add(DialogParameterNames.TextResource, textResource);
            if (rootFolders != null)
                param.Add(DialogParameterNames.RootFolders, rootFolders);
            if (isModal)
                dialogService.ShowDialog(DialogNames.FolderSelectDialog, param, callBack);
            else
                dialogService.Show(DialogNames.FolderSelectDialog, param, callBack);
        }
        /// <summary>
        /// Show file select dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="title">Dialog title.</param>
        /// <param name="canMultiSelect">If true; can multi select.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        /// <param name="fileNamePrefixText">File name prefix text.</param>
        /// <param name="selectButtonText">Select button text.</param>
        /// <param name="cancelButtonText">Cancel button text.</param>
        /// <param name="textResource">Text resource for explorer.</param>
        /// <param name="filters">File filters.</param>
        /// <param name="rootFolders">Root folders path.</param>
        public static void ShowFileSelectDialog(this IDialogService dialogService,
                                                string title,
                                                bool canMultiSelect,
                                                Action<IDialogResult> callBack,
                                                bool isModal = true,
                                                string fileNamePrefixText = null,
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
            if (!string.IsNullOrEmpty(fileNamePrefixText))
                param.Add(DialogParameterNames.FileNamePrefixText, fileNamePrefixText);
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
            if (isModal)
                dialogService.ShowDialog(DialogNames.FileSelectDialog, param, callBack);
            else
                dialogService.Show(DialogNames.FileSelectDialog, param, callBack);
        }
        /// <summary>
        /// Show file save dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="title">Dialog title.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        /// <param name="fileNamePrefixText">File name prefix text.</param>
        /// <param name="fileTypePrefixText">File type prefix text.</param>
        /// <param name="saveButtonText">Save button text.</param>
        /// <param name="cancelButtonText">Cancel button text.</param>
        /// <param name="textResource">Text resource for explorer.</param>
        /// <param name="filters">File filters.</param>
        /// <param name="rootFolders">Root folders path.</param>
        /// <param name="overwriteConfirmationTitle">Dialog title when overwriting files.</param>
        /// <param name="overwriteConfirmationMessageFunc">Message function when overwriting files.</param>
        /// <param name="overwriteConfirmationOKButtonText">Dialog OK button text when overwriting files.</param>
        /// <param name="overwriteConfirmationCancelButtonText">Dialog Cancel button text when overwriting files.</param>
        public static void ShowFileSaveDialog(this IDialogService dialogService,
                                              string title,
                                              Action<IDialogResult> callBack,
                                              bool isModal = true,
                                              string fileNamePrefixText = null,
                                              string fileTypePrefixText = null,
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
            if (!string.IsNullOrEmpty(fileNamePrefixText))
                param.Add(DialogParameterNames.FileNamePrefixText, fileNamePrefixText);
            if (!string.IsNullOrEmpty(fileTypePrefixText))
                param.Add(DialogParameterNames.FileTypePrefixText, fileTypePrefixText);
            if (!string.IsNullOrEmpty(saveButtonText))
                param.Add(DialogParameterNames.SaveButtonText, saveButtonText);
            if (!string.IsNullOrEmpty(cancelButtonText))
                param.Add(DialogParameterNames.CancelButtonText, cancelButtonText);
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
            if (isModal)
                dialogService.ShowDialog(DialogNames.FileSaveDialog, param, callBack);
            else
                dialogService.Show(DialogNames.FileSaveDialog, param, callBack);
        }
        /// <summary>
        /// Show color picker dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowColorPickerDialog(this IDialogService dialogService, Action<ColorPickerDialogResult> callBack, string title = "", bool isModal = true)
        {
            var param = new DialogParameters();
            if (!string.IsNullOrEmpty(title))
            {
                param.Add(DialogParameterNames.Title, title);
            }
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.ColorPickerDialog, param, res =>
                {
                    var colorPickerDialogResult = new ColorPickerDialogResult(res);
                    callBack?.Invoke(colorPickerDialogResult);
                });
            }
            else
            {
                dialogService.Show(DialogNames.ColorPickerDialog, param, res =>
                {
                    var colorPickerDialogResult = new ColorPickerDialogResult(res);
                    callBack?.Invoke(colorPickerDialogResult);
                });
            }
        }
    }
}
