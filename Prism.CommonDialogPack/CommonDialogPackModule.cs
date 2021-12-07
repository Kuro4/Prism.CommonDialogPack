using Prism.CommonDialogPack.ViewModels;
using Prism.CommonDialogPack.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack
{
    public class CommonDialogPackModule : IModule
    {
        private IRegionManager regionManager;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            this.regionManager = containerProvider.Resolve<IRegionManager>();
            this.regionManager.RegisterViewWithRegion(nameof(ExplorerBase), typeof(ExplorerBase));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<NotificationDialog, NotificationDialogViewModel>();
            containerRegistry.RegisterDialog<ConfirmationDialog, ConfirmationDialogViewModel>();
            containerRegistry.RegisterDialog<FolderSelectDialog, FolderSelectDialogViewModel>();
            containerRegistry.RegisterDialog<FileSelectDialog, FileSelectDialogViewModel>();
            containerRegistry.RegisterDialog<FileSaveDialog, FileSaveDialogViewModel>();
        }
    }
}
