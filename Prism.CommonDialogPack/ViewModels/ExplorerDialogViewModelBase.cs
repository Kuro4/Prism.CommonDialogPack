using Prism.CommonDialogPack.Resources;
using Prism.Services.Dialogs;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ExplorerDialogViewModelBase : DialogViewModelBase
    {
        private ImageSource backwardIcon = new BitmapImage(IconUri.BackWard);
        /// <summary>
        /// Backward button icon.
        /// </summary>
        public ImageSource BackWardIcon
        {
            get { return this.backwardIcon; }
            set { SetProperty(ref this.backwardIcon, value); }
        }

        private ImageSource forwardIcon = new BitmapImage(IconUri.Forward);
        /// <summary>
        /// Forward button icon.
        /// </summary>
        public ImageSource ForwardIcon
        {
            get { return this.forwardIcon; }
            set { SetProperty(ref forwardIcon, value); }
        }

        private ImageSource upIcon = new BitmapImage(IconUri.Up);
        /// <summary>
        /// Up button icon.
        /// </summary>
        public ImageSource UpIcon
        {
            get { return upIcon; }
            set { SetProperty(ref upIcon, value); }
        }

        private ImageSource reloadIcon = new BitmapImage(IconUri.Reload);
        /// <summary>
        /// Reload button icon.
        /// </summary>
        public ImageSource ReloadIcon
        {
            get { return reloadIcon; }
            set { SetProperty(ref reloadIcon, value); }
        }

        private ImageSource folderClosedIcon = new BitmapImage(IconUri.FolderClosed);
        /// <summary>
        /// Folder closed icon.
        /// </summary>
        public ImageSource FolderClosedIcon
        {
            get { return folderClosedIcon; }
            set { SetProperty(ref folderClosedIcon, value); }
        }

        private ImageSource folderOpendIcon = new BitmapImage(IconUri.FolderOpened);
        /// <summary>
        /// Folder opened icon.
        /// </summary>
        public ImageSource FolderOpenedIcon
        {
            get { return folderOpendIcon; }
            set { SetProperty(ref folderOpendIcon, value); }
        }

        private ImageSource fileIcon = new BitmapImage(IconUri.File);
        /// <summary>
        /// File icon.
        /// </summary>
        public ImageSource FileIcon
        {
            get { return fileIcon; }
            set { SetProperty(ref fileIcon, value); }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ExplorerDialogViewModelBase"/> class.
        /// </summary>
        public ExplorerDialogViewModelBase()
        {
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.Width = 800;
            this.Height = 450;
        }
        /// <summary>
        /// Called when the dialog is opened.
        /// </summary>
        /// <param name="parameters">The parameters passed to the dialog.</param>
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            // Configure parameters
            if (!parameters.TryGetValue(DialogParameterNames.ExplorerIcons, out ExplorerIcons explorerIcons))
            {
                return;
            }
            if (explorerIcons.BackWardIcon != null)
            {
                this.BackWardIcon = explorerIcons.BackWardIcon;
            }
            if (explorerIcons.ForwardIcon != null)
            {
                this.ForwardIcon = explorerIcons.ForwardIcon;
            }
            if (explorerIcons.UpIcon != null)
            {
                this.UpIcon = explorerIcons.UpIcon;
            }
            if (explorerIcons.ReloadIcon != null)
            {
                this.ReloadIcon = explorerIcons.ReloadIcon;
            }
            if (explorerIcons.FolderClosedIcon != null)
            {
                this.FolderClosedIcon = explorerIcons.FolderClosedIcon;
            }
            if (explorerIcons.FolderOpenedIcon != null)
            {
                this.FolderOpenedIcon = explorerIcons.FolderOpenedIcon;
            }
            if (explorerIcons.FileIcon != null)
            {
                this.FileIcon = explorerIcons.FileIcon;
            }
        }
    }
}
