using Prism.Commands;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Extensions;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class FileSaveDialogViewModel : ExplorerDialogViewModelBase
    {
        private DelegateCommand saveCommand;
        /// <summary>
        /// Save command.
        /// </summary>
        public DelegateCommand SaveCommand => this.saveCommand ??= new DelegateCommand(this.Save).ObservesCanExecute(() => this.CanSave);

        private DelegateCommand cancelCommand;
        /// <summary>
        /// Cancel command.
        /// </summary>
        public DelegateCommand CancelCommand => this.cancelCommand ??= new DelegateCommand(this.Cancel);

        private string fileNamePrefixText = "ファイル：";
        /// <summary>
        /// File name prefix text.
        /// </summary>
        public string FileNamePrefixText
        {
            get { return this.fileNamePrefixText; }
            set { SetProperty(ref this.fileNamePrefixText, value); }
        }

        private string fileTypePrefixText = "ファイルの種類：";
        /// <summary>
        /// File type prefix text.
        /// </summary>
        public string FileTypePrefixText
        {
            get { return this.fileTypePrefixText; }
            set { SetProperty(ref this.fileTypePrefixText, value); }
        }

        private string saveButtonText = "保存";
        /// <summary>
        /// Save button text.
        /// </summary>
        public string SaveButtonText
        {
            get { return this.saveButtonText; }
            set { SetProperty(ref this.saveButtonText, value); }
        }

        private string cancelButtonText = "キャンセル";
        /// <summary>
        /// Cancel button text.
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

        private string saveFileName = string.Empty;
        /// <summary>
        /// Save file name.
        /// </summary>
        public string SaveFileName
        {
            get { return this.saveFileName; }
            set 
            {
                SetProperty(ref this.saveFileName, value);
                CanSave = !string.IsNullOrEmpty(value);
            }
        }

        private readonly ObservableCollection<FileFilter> filters = new ObservableCollection<FileFilter>();
        /// <summary>
        /// File filters.
        /// </summary>
        public ReadOnlyObservableCollection<FileFilter> Filters { get; }

        private FileFilter selectedFilter;
        /// <summary>
        /// Selected file filter.
        /// </summary>
        public FileFilter SelectedFilter
        {
            get { return this.selectedFilter; }
            set
            {
                if (this.saveButtonText.Equals(value)) return;
                SetProperty(ref this.selectedFilter, value);
                var context = new ExplorerBaseRegionContext(this.RegionContext)
                {
                    FileExtensions = value.Extensions
                };
                this.RegionContext = context;
                if (value.Extensions != null && value.Extensions.Any())
                {
                    this.SaveFileName = Path.ChangeExtension(this.SaveFileName, value.Extensions.First());
                }
            }
        }

        private bool canSave = false;
        /// <summary>
        /// Can save.
        /// </summary>
        public bool CanSave
        {
            get { return this.canSave; }
            private set { SetProperty(ref this.canSave, value); }
        }

        /// <summary>
        /// Current folder path.
        /// </summary>
        /// <remarks>
        /// Received in <see cref="MoveCurrentFolderEvent"/>.
        /// </remarks>
        private string CurrentFolderPath { get; set; }
        /// <summary>
        /// Title of the overwrite confirmation dialog.
        /// </summary>
        private string OverwriteConfirmationTitle { get; set; } = "名前を付けて保存の確認";
        /// <summary>
        /// Message function of the overwrite confirmation dialog.
        /// </summary>
        private Func<string, string> OverwriteConfirmationMessageFunc { get; set; } = x => $"{Path.GetFileName(x)}は既に存在します。{Environment.NewLine}上書きしますか?";
        /// <summary>
        /// OK button text of the overwrite confirmation dialog.
        /// </summary>
        private string OverwriteConfirmationOKButtonText { get; set; } = "はい";
        /// <summary>
        /// Cancel button text of the overwrite confirmation dialog.
        /// </summary>
        private string OverwriteConfirmationCancelButtonText { get; set; } = "いいえ";
        /// <summary>
        /// Event aggregator.
        /// </summary>
        private readonly IEventAggregator eventAggregator;
        /// <summary>
        /// Dialog service.
        /// </summary>
        private readonly IDialogService dialogService;

        /// <summary>
        /// Initialize a new instance of the <see cref="FileSaveDialogViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="dialogService"></param>
        public FileSaveDialogViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            this.Filters = new ReadOnlyObservableCollection<FileFilter>(this.filters);
            this.eventAggregator = eventAggregator;
            this.dialogService = dialogService;
            this.Title = "名前を付けて保存";
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
            this.eventAggregator.GetEvent<MoveCurrentFolderEvent>().Subscribe(this.OnMoveDisplayFolder);
            this.eventAggregator.GetEvent<FileEnterEvent>().Subscribe(this.OnFileEnter);
            // Configure parameters
            if (parameters.TryGetValue(DialogParameterNames.FileNamePrefixText, out string fileNamePrefixText))
            {
                this.FileNamePrefixText = fileNamePrefixText;
            }
            if (parameters.TryGetValue(DialogParameterNames.FileTypePrefixText, out string fileTypePrefixText))
            {
                this.FileTypePrefixText = fileTypePrefixText;
            }
            if (parameters.TryGetValue(DialogParameterNames.SaveButtonText, out string saveButtonText))
            {
                this.SaveButtonText = saveButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
            {
                this.CancelButtonText = cancelButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.DefaultSaveFileName, out string defaultSaveFileName))
            {
                this.SaveFileName = defaultSaveFileName;
            }
            this.filters.Clear();
            if (parameters.TryGetValue(DialogParameterNames.Filters, out IEnumerable<FileFilter> filters))
            {
                this.filters.AddRange(filters);
            }
            else
            {
                var filter = parameters.TryGetValue(DialogParameterNames.DefaultAllFilesFilterText, out string filterText) ? new FileFilter(filterText) : FileFilter.CreateDefault();
                this.filters.Add(filter);
            }
            this.selectedFilter = this.Filters.First();
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationTitle, out string overwriteConfirmationTitle))
            {
                this.OverwriteConfirmationTitle = overwriteConfirmationTitle;
            }
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationMessageFunc, out Func<string, string> overwriteConfirmationMessageFunc))
            {
                this.OverwriteConfirmationMessageFunc = overwriteConfirmationMessageFunc;
            }
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationOKButtonText, out string overwriteConfirmationOKButtonText))
            {
                this.OverwriteConfirmationOKButtonText = overwriteConfirmationOKButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationCancelButtonText, out string overwriteConfirmationCancelButtonText))
            {
                this.OverwriteConfirmationCancelButtonText = overwriteConfirmationCancelButtonText;
            }
            var regionContext = ExplorerBaseRegionContext.CreateForSingleFileSelect();
            if (parameters.TryGetValue(DialogParameterNames.TextResource, out ExplorerBaseTextResource textResource))
            {
                regionContext.TextResource = textResource;
            }
            if (parameters.TryGetValue(DialogParameterNames.RootFolders, out IEnumerable<string> rootFolders))
            {
                regionContext.RootFolders = rootFolders;
            }
            regionContext.FileExtensions = this.Filters.First().Extensions;
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
            this.eventAggregator.GetEvent<MoveCurrentFolderEvent>().Unsubscribe(this.OnMoveDisplayFolder);
            this.eventAggregator.GetEvent<FileEnterEvent>().Unsubscribe(this.OnFileEnter);
        }
        /// <summary>
        /// Called when the file selected.
        /// </summary>
        public void OnFileSelection(FileSelectionEventValue value)
        {
            this.SaveFileName = Path.GetFileName(value.Paths.First());
        }
        /// <summary>
        /// Called when the move current folder.
        /// </summary>
        /// <param name="value"></param>
        public void OnMoveDisplayFolder(MoveCurrentFolderEventValue value)
        {
            this.CurrentFolderPath = value.Path;
        }
        /// <summary>
        /// Called when the file entered.
        /// </summary>
        /// <param name="value"></param>
        public void OnFileEnter(FileEnterEventValue value)
        {
            this.Save();
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.OK"/>.
        /// </summary>
        /// <remarks>
        /// If the selected file exists, show the overwrite confirmation dialog.
        /// Dialog parameters is selected file path.
        /// </remarks>
        private void Save()
        {
            string res = Path.Combine(this.CurrentFolderPath, this.SaveFileName);
            if (this.SelectedFilter.Extensions != null && this.SelectedFilter.Extensions.Any() && !Path.HasExtension(res))
            {
                res = Path.ChangeExtension(res, this.SelectedFilter.Extensions.First());
            }
            if (File.Exists(res))
            {
                bool confirmed = false;
                this.dialogService.ShowConfirmation(
                    this.OverwriteConfirmationMessageFunc?.Invoke(res),
                    this.OverwriteConfirmationTitle,
                    res => confirmed = res.Result == ButtonResult.OK,
                    this.OverwriteConfirmationOKButtonText,
                    this.OverwriteConfirmationCancelButtonText);
                if (!confirmed)
                {
                    return;
                }
            }
            var param = new DialogParameters
            {
                { DialogResultParameterNames.SaveFilePath, res }
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
