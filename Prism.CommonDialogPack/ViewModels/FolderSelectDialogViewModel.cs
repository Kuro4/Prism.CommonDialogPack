using Prism.Commands;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Extensions;
using Prism.Events;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class FolderSelectDialogViewModel : ExplorerDialogViewModelBase
    {
        private DelegateCommand selectCommand;
        /// <summary>
        /// Select command.
        /// </summary>
        public DelegateCommand SelectCommand => this.selectCommand ??= new DelegateCommand(this.Select);

        private DelegateCommand cancelCommand;
        /// <summary>
        /// Cancel command.
        /// </summary>
        public DelegateCommand CancelCommand => this.cancelCommand ??= new DelegateCommand(this.Cancel);

        private string folderNamePrefixText ="フォルダー：";
        /// <summary>
        /// Folder name prefix text.
        /// </summary>
        public string FolderNamePrefixText
        {
            get { return this.folderNamePrefixText; }
            set { SetProperty(ref this.folderNamePrefixText, value); }
        }

        private string selectButtonText = "選択";
        /// <summary>
        /// Select button text.
        /// </summary>
        public string SelectButtonText
        {
            get { return this.selectButtonText; }
            set { SetProperty(ref this.selectButtonText, value); }
        }

        private string cancelButtonText = "キャンセル";
        /// <summary>
        /// Cancel button text
        /// </summary>
        public string CancelButtonText
        {
            get { return this.cancelButtonText; }
            set { SetProperty(ref this.cancelButtonText, value); }
        }

        private ExplorerBaseRegionContext regionContext = ExplorerBaseRegionContext.CreateForSingleFolderSelect(); 
        /// <summary>
        /// Region context for ExplorerBase.
        /// </summary>
        public ExplorerBaseRegionContext RegionContext
        {
            get { return this.regionContext; }
            set { SetProperty(ref this.regionContext, value); }
        }

        private string selectedFolderName;
        /// <summary>
        /// Selected folder name.
        /// </summary>
        public string SelectedFolderName
        {
            get { return this.selectedFolderName; }
            set { SetProperty(ref this.selectedFolderName, value); }
        }

        /// <summary>
        /// Current folder path.
        /// </summary>
        /// <remarks>
        /// Received in <see cref="MoveCurrentFolderEvent"/>.
        /// </remarks>
        private string CurrentFolderPath { get; set; }
        /// <summary>
        /// Event aggregator.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize a new instance of the <see cref="FolderSelectDialogViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator"></param>
        public FolderSelectDialogViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }
        /// <summary>
        /// Called when the dialog is opened.
        /// </summary>
        /// <param name="parameters">The parameters passed to the dialog.</param>
        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            // Subscribe event
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(this.OnFileSelection);
            this.eventAggregator.GetEvent<MoveCurrentFolderEvent>().Subscribe(this.OnMoveCurrentFolder);
            // Configure parameters
            if (parameters.TryGetValue(DialogParameterNames.FolderNamePrefixText, out string folderNamePrefixText))
            {
                this.FolderNamePrefixText = folderNamePrefixText;
            }
            if (parameters.TryGetValue(DialogParameterNames.SelectButtonText, out string selectButtonText))
            {
                this.SelectButtonText = selectButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
            {
                this.CancelButtonText = cancelButtonText;
            }
            var regionContext = ExplorerBaseRegionContext.CreateForSingleFolderSelect();
            if (parameters.TryGetValue(DialogParameterNames.TextResource, out ExplorerBaseTextResource textResource))
            {
                regionContext.TextResource = textResource;
            }
            if (parameters.TryGetValue(DialogParameterNames.CanMultiSelect, out bool canMultiSelect))
            {
                regionContext.CanMultiSelect = canMultiSelect;
            }
            if (parameters.TryGetValue(DialogParameterNames.RootFolders, out IEnumerable<string> rootFolders))
            {
                regionContext.RootFolders = rootFolders;
            }
            this.RegionContext = regionContext;
        }
        /// <summary>
        /// Called when the dialog is closed.
        /// </summary>
        public override void OnDialogClosed()
        {
            base.OnDialogClosed();
            // Unsubscribe event
            this.eventAggregator.GetEvent<FileSelectionEvent>().Unsubscribe(this.OnFileSelection);
            this.eventAggregator.GetEvent<MoveCurrentFolderEvent>().Unsubscribe(this.OnMoveCurrentFolder);
        }
        /// <summary>
        /// Called when the file selected.
        /// </summary>
        public void OnFileSelection(FileSelectionEventValue value)
        {
            if (value.Paths.Count() <= 1)
            {
                this.SelectedFolderName = Path.GetFileName(value.Paths.First());
                return;
            }
            this.SelectedFolderName = string.Join(' ', value.Paths.Select(p => $"\"{Path.GetFileName(p)}\""));
        }
        /// <summary>
        /// Called when the move current folder.
        /// </summary>
        /// <param name="value"></param>
        public void OnMoveCurrentFolder(MoveCurrentFolderEventValue value)
        {
            this.CurrentFolderPath = value.Path;
            this.SelectedFolderName = string.Empty;
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.OK"/>.
        /// </summary>
        /// <remarks>
        /// Dialog parameters is selected folder path.
        /// </remarks>
        private void Select()
        {
            IEnumerable<string> res;
            if (string.IsNullOrEmpty(this.SelectedFolderName))
            {
                res = new string[] { this.CurrentFolderPath };
            }
            else if (!this.SelectedFolderName.Contains('\"'))
            {
                res = new string[] { Path.Combine(this.CurrentFolderPath, this.SelectedFolderName) };
            }
            else
            {
                res = this.SelectedFolderName.Unwind('\"').Select(x => Path.Combine(this.CurrentFolderPath, x));
            }
            var param = new DialogParameters
            {
                { DialogResultParameterNames.SelectedPaths, res }
            };
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK, param));
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.Cancel"/>.
        /// </summary>
        private void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
    }
}
