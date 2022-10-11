using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.CommonDialogPack.Extensions
{
    /// <summary>
    /// <see cref="IDialogService"/> extensions.
    /// </summary>
    public static class DialogServiceExtensions
    {
        #region Notification Dialog.
        /// <summary>
        /// Show ConfirmationDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowNotificationDialog(this IDialogService dialogService, Action<IDialogResult> callBack, bool isModal = true)
        {
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.Notification, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.Notification, callBack);
            }
        }
        /// <summary>
        /// Show ConfirmationDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="param">Dialog parameters.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowNotificationDialog(this IDialogService dialogService, IDialogParameters param, Action<IDialogResult> callBack, bool isModal = true)
        {
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.Notification, param, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.Notification, param, callBack);
            }
        }
        /// <summary>
        /// Show notification dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="title">Dialog title.</param>
        /// <param name="message">Notification message.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowNotificationDialog(this IDialogService dialogService, string title, string message, Action<IDialogResult> callBack, bool isModal = true)
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Title, title },
                { DialogParameterNames.Message, message },
            };
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.Notification, param, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.Notification, param, callBack);
            }
        }
        #endregion
        #region Confirmation Dialog
        /// <summary>
        /// Show ConfirmationDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowConfirmationDialog(this IDialogService dialogService, Action<IDialogResult> callBack, bool isModal = true)
        {
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.Confirmation, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.Confirmation, callBack);
            }
        }
        /// <summary>
        /// Show ConfirmationDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="param">Dialog parameters.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowConfirmationDialog(this IDialogService dialogService, IDialogParameters param, Action<IDialogResult> callBack, bool isModal = true)
        {
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.Confirmation, param, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.Confirmation, param, callBack);
            }
        }
        /// <summary>
        /// Show confirmation dialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="title">Dialog title.</param>
        /// <param name="message">Confirmation message.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowConfirmationDialog(this IDialogService dialogService, string title, string message, Action<IDialogResult> callBack, bool isModal = true)
        {
            var param = new DialogParameters()
            {
                { DialogParameterNames.Title, title },
                { DialogParameterNames.Message, message },
            };
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.Confirmation, param, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.Confirmation, param, callBack);
            }
        }
        #endregion
        #region FolderSelect Dialog
        /// <summary>
        /// Show FolderSelectDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowFolderSelectDialog(this IDialogService dialogService, Action<PathSelectDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var fileSaveDialogResult = new PathSelectDialogResult(res);
                callBack?.Invoke(fileSaveDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.FolderSelectDialog, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.FolderSelectDialog, resultAction);
            }
        }
        /// <summary>
        /// Show FolderSelectDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="param">Dialog parameters.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowFolderSelectDialog(this IDialogService dialogService, IDialogParameters param, Action<PathSelectDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var fileSaveDialogResult = new PathSelectDialogResult(res);
                callBack?.Invoke(fileSaveDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.FolderSelectDialog, param, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.FolderSelectDialog, param, resultAction);
            }
        }
        #endregion
        #region FileSelect Dialog
        /// <summary>
        /// Show FileSelectDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal"></param>
        public static void ShowFileSelectDialog(this IDialogService dialogService, Action<PathSelectDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var fileSaveDialogResult = new PathSelectDialogResult(res);
                callBack?.Invoke(fileSaveDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.FileSelectDialog, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.FileSelectDialog, resultAction);
            }
        }
        /// <summary>
        /// Show FileSelectDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="param">Dialog parameters.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowFileSelectDialog(this IDialogService dialogService, IDialogParameters param, Action<PathSelectDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var fileSaveDialogResult = new PathSelectDialogResult(res);
                callBack?.Invoke(fileSaveDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.FileSelectDialog, param, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.FileSelectDialog, param, resultAction);
            }
        }
        #endregion
        #region FileSave Dialog
        /// <summary>
        /// Show FileSaveDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowFileSaveDialog(this IDialogService dialogService, Action<FileSaveDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var fileSaveDialogResult = new FileSaveDialogResult(res);
                callBack?.Invoke(fileSaveDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.FileSaveDialog, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.FileSaveDialog, resultAction);
            }
        }
        /// <summary>
        /// Show FileSaveDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="param">Dialog parameters.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowFileSaveDialog(this IDialogService dialogService, IDialogParameters param, Action<FileSaveDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var fileSaveDialogResult = new FileSaveDialogResult(res);
                callBack?.Invoke(fileSaveDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.FileSaveDialog, param, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.FileSaveDialog, param, resultAction);
            }
        }
        #endregion
        #region Progress Dialog
        /// <summary>
        /// Show ProgressDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModeal">If true; dialog is shown as a modal.</param>
        public static void ShowProgressDialog(this IDialogService dialogService, Action<IDialogResult> callBack, bool isModeal = true)
        {
            if (isModeal)
            {
                dialogService.ShowDialog(DialogNames.ProgressDialog, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.ProgressDialog, callBack);
            }
        }
        /// <summary>
        /// Show ProgressDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="param">Dialog parameters.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModeal">If true; dialog is shown as a modal.</param>
        public static void ShowProgressDialog(this IDialogService dialogService, IDialogParameters param, Action<IDialogResult> callBack, bool isModeal = true)
        {
            if (isModeal)
            {
                dialogService.ShowDialog(DialogNames.ProgressDialog, param, callBack);
            }
            else
            {
                dialogService.Show(DialogNames.ProgressDialog, param, callBack);
            }
        }
        #endregion
        #region Color Picker Dialog
        /// <summary>
        /// Show ColorPickerDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowColorPickerDialog(this IDialogService dialogService, Action<ColorPickerDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var colorPickerDialogResult = new ColorPickerDialogResult(res);
                callBack?.Invoke(colorPickerDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.ColorPickerDialog, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.ColorPickerDialog, resultAction);
            }
        }
        /// <summary>
        /// Show ColorPickerDialog.
        /// </summary>
        /// <param name="dialogService"></param>
        /// <param name="param">Dialog parameters.</param>
        /// <param name="callBack">Callback action.</param>
        /// <param name="isModal">If true; dialog is shown as a modal.</param>
        public static void ShowColorPickerDialog(this IDialogService dialogService, IDialogParameters param, Action<ColorPickerDialogResult> callBack, bool isModal = true)
        {
            var resultAction = new Action<IDialogResult>(res =>
            {
                var colorPickerDialogResult = new ColorPickerDialogResult(res);
                callBack?.Invoke(colorPickerDialogResult);
            });
            if (isModal)
            {
                dialogService.ShowDialog(DialogNames.ColorPickerDialog, param, resultAction);
            }
            else
            {
                dialogService.Show(DialogNames.ColorPickerDialog, param, resultAction);
            }
        }
        #endregion
    }
}
