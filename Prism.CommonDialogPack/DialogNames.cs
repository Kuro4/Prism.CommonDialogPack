using Prism.CommonDialogPack.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack
{
    public static class DialogNames
    {
        public static readonly string Notification = nameof(NotificationDialog);
        public static readonly string Confirmation = nameof(ConfirmationDialog);
        public static readonly string FolderSelectDialog = nameof(Views.FolderSelectDialog);
        public static readonly string FileSelectDialog = nameof(Views.FileSelectDialog);
        public static readonly string FileSaveDialog = nameof(Views.FileSaveDialog);
    }
}
