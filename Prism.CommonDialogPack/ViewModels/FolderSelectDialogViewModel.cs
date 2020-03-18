using Prism.Commands;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Extensions;
using Prism.CommonDialogPack.Models;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class FolderSelectDialogViewModel : ExplorerDialogViewModelBase
    {
        private DelegateCommand selectCommand;
        public DelegateCommand SelectCommand => this.selectCommand ?? (this.selectCommand = new DelegateCommand(this.Select));

        private DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand => this.cancelCommand ?? (this.cancelCommand = new DelegateCommand(this.Cancel));

        private string folderNameText ="フォルダー：";
        public string FolderNameText
        {
            get { return this.folderNameText; }
            set { SetProperty(ref this.folderNameText, value); }
        }

        private string selectButtonText = "選択";
        public string SelectButtonText
        {
            get { return this.selectButtonText; }
            set { SetProperty(ref this.selectButtonText, value); }
        }

        private string cancelButtonText = "キャンセル";
        public string CancelButtonText
        {
            get { return this.cancelButtonText; }
            set { SetProperty(ref this.cancelButtonText, value); }
        }

        private ExplorerBaseRegionContext regionContext = ExplorerBaseRegionContext.CreateForSingleFolderSelect(); 
        public ExplorerBaseRegionContext RegionContext
        {
            get { return this.regionContext; }
            set { SetProperty(ref this.regionContext, value); }
        }

        private string selectedFolderName;
        public string SelectedFolderName
        {
            get { return this.selectedFolderName; }
            set { SetProperty(ref this.selectedFolderName, value); }
        }

        private string DisplayFolderPath { get; set; }
        private readonly IEventAggregator eventAggregator;

        public FolderSelectDialogViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(this.OnFileSelection);
            this.eventAggregator.GetEvent<MoveDisplayFolderEvent>().Subscribe(this.OnMoveDisplayFolder);
            if (parameters.TryGetValue(DialogParameterNames.FolderNameText, out string folderNameText))
                this.FolderNameText = folderNameText;
            if (parameters.TryGetValue(DialogParameterNames.SelectButtonText, out string selectButtonText))
                this.SelectButtonText = selectButtonText;
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
                this.CancelButtonText = cancelButtonText;
            var regionContext = ExplorerBaseRegionContext.CreateForSingleFolderSelect();
            if (parameters.TryGetValue(DialogParameterNames.TextResource, out ExplorerBaseTextResource textResource))
                regionContext.TextResource = textResource;
            if (parameters.TryGetValue(DialogParameterNames.CanMultiSelect, out bool canMultiSelect))
                regionContext.CanMultiSelect = canMultiSelect;
            if (parameters.TryGetValue(DialogParameterNames.RootFolders, out IEnumerable<string> rootFolders))
                regionContext.RootFolders = rootFolders;
            this.RegionContext = regionContext;
        }

        public override void OnDialogClosed()
        {
            base.OnDialogClosed();
            this.eventAggregator.GetEvent<FileSelectionEvent>().Unsubscribe(this.OnFileSelection);
            this.eventAggregator.GetEvent<MoveDisplayFolderEvent>().Unsubscribe(this.OnMoveDisplayFolder);
        }

        public void OnFileSelection(FileSelectionEventValue value)
        {
            if (value.Paths.Count() <= 1)
            {
                this.SelectedFolderName = Path.GetFileName(value.Paths.First());
                return;
            }
            this.SelectedFolderName = string.Join(' ', value.Paths.Select(p => $"\"{Path.GetFileName(p)}\""));
        }

        public void OnMoveDisplayFolder(MoveDisplayFolderEventValue value)
        {
            this.DisplayFolderPath = value.Path;
            this.SelectedFolderName = string.Empty;
        }

        private void Select()
        {
            IEnumerable<string> res;
            if (string.IsNullOrEmpty(this.SelectedFolderName))
                res = new string[] { this.DisplayFolderPath };
            else if (!this.SelectedFolderName.Contains('\"'))
                res = new string[] { Path.Combine(this.DisplayFolderPath, this.SelectedFolderName) };
            else
                res = this.SelectedFolderName.Unwind('\"').Select(x => Path.Combine(this.DisplayFolderPath, x));
            var param = new DialogParameters
            {
                { DialogResultParameterNames.SelectedPaths, res }
            };
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK, param));
        }

        private void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
    }
}
