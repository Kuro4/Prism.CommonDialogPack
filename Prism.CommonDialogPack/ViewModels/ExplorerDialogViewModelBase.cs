using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ExplorerDialogViewModelBase : DialogViewModelBase
    {
        private ImageSource backwardIcon = new BitmapImage(new Uri("/Prism.CommonDialogPack;component/Resources/Previous_grey_16x.png", UriKind.Relative));
        public ImageSource BackWardIcon
        {
            get { return this.backwardIcon; }
            set { SetProperty(ref this.backwardIcon, value); }
        }

        private ImageSource forwardIcon = new BitmapImage(new Uri("/Prism.CommonDialogPack;component/Resources/Next_grey_16x.png", UriKind.Relative));
        public ImageSource ForwardIcon
        {
            get { return this.forwardIcon; }
            set { SetProperty(ref forwardIcon, value); }
        }

        private ImageSource upIcon = new BitmapImage(new Uri("/Prism.CommonDialogPack;component/Resources/Upload_gray_16x.png", UriKind.Relative));
        public ImageSource UpIcon
        {
            get { return upIcon; }
            set { SetProperty(ref upIcon, value); }
        }

        private ImageSource reloadIcon = new BitmapImage(new Uri("/Prism.CommonDialogPack;component/Resources/Refresh_grey_16x.png", UriKind.Relative));
        public ImageSource ReloadIcon
        {
            get { return reloadIcon; }
            set { SetProperty(ref reloadIcon, value); }
        }

        private ImageSource folderClosedIcon = new BitmapImage(new Uri("/Prism.CommonDialogPack;component/Resources/FolderClosed_16x.png", UriKind.Relative));
        public ImageSource FolderClosedIcon
        {
            get { return folderClosedIcon; }
            set { SetProperty(ref folderClosedIcon, value); }
        }

        private ImageSource folderOpendIcon = new BitmapImage(new Uri("/Prism.CommonDialogPack;component/Resources/FolderOpened_16x.png", UriKind.Relative));
        public ImageSource FolderOpenedIcon
        {
            get { return folderOpendIcon; }
            set { SetProperty(ref folderOpendIcon, value); }
        }

        private ImageSource fileIcon = new BitmapImage(new Uri("/Prism.CommonDialogPack;component/Resources/Document_16x.png", UriKind.Relative));
        public ImageSource FileIcon
        {
            get { return fileIcon; }
            set { SetProperty(ref fileIcon, value); }
        }

        public ExplorerDialogViewModelBase()
        {
            this.WindowStyle = DialogStyles.ExplorerDialogStyle;
            this.Width = 800;
            this.Height = 450;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            if (!parameters.TryGetValue(DialogParameterNames.ExplorerIcons, out ExplorerIcons explorerIcons)) return;
            if (explorerIcons.BackWardIcon != null)
                this.BackWardIcon = explorerIcons.BackWardIcon;
            if (explorerIcons.ForwardIcon != null)
                this.ForwardIcon = explorerIcons.ForwardIcon;
            if (explorerIcons.UpIcon != null)
                this.UpIcon = explorerIcons.UpIcon;
            if (explorerIcons.ReloadIcon != null)
                this.ReloadIcon = explorerIcons.ReloadIcon;
            if (explorerIcons.FolderClosedIcon != null)
                this.FolderClosedIcon = explorerIcons.FolderClosedIcon;
            if (explorerIcons.FolderOpenedIcon != null)
                this.FolderOpenedIcon = explorerIcons.FolderOpenedIcon;
            if (explorerIcons.FileIcon != null)
                this.FileIcon = explorerIcons.FileIcon;
        }
    }
}
