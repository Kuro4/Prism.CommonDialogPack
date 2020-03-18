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
        public DelegateCommand SaveCommand => this.saveCommand ?? (this.saveCommand = new DelegateCommand(this.Save, () => this.CanSave)).ObservesCanExecute(() => this.CanSave);

        private DelegateCommand cancelCommand;
        public DelegateCommand CancelCommand => this.cancelCommand ?? (this.cancelCommand = new DelegateCommand(this.Cancel));

        private string fileNameText = "ファイル：";
        public string FileNameText
        {
            get { return this.fileNameText; }
            set { SetProperty(ref this.fileNameText, value); }
        }

        private string fileTypeText = "ファイルの種類：";
        public string FileTypeText
        {
            get { return this.fileTypeText; }
            set { SetProperty(ref this.fileTypeText, value); }
        }

        private string saveButtonText = "保存";
        public string SaveButtonText
        {
            get { return this.saveButtonText; }
            set { SetProperty(ref this.saveButtonText, value); }
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

        private string saveFileName = string.Empty;
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
        public ReadOnlyObservableCollection<FileFilter> Filters { get; }

        private FileFilter selectedFilter;
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
        public bool CanSave
        {
            get { return this.canSave; }
            private set { SetProperty(ref this.canSave, value); }
        }

        private string DisplayFolderPath { get; set; }
        private string OverwriteConfirmationTitle { get; set; }
        private Func<string, string> OverwriteConfirmationMessageFunc { get; set; } = x => $"{Path.GetFileName(x)}は既に存在します。{Environment.NewLine}上書きしますか?";
        private string OverwriteConfirmationOKButtonText { get; set; } = "はい";
        private string OverwriteConfirmationCancelButtonText { get; set; } = "いいえ";
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogService dialogService;

        public FileSaveDialogViewModel(IEventAggregator eventAggregator, IDialogService dialogService)
        {
            this.Filters = new ReadOnlyObservableCollection<FileFilter>(this.filters);

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(x => this.SaveFileName = Path.GetFileName(x.Paths.First()), ThreadOption.UIThread);
            this.eventAggregator.GetEvent<MoveDisplayFolderEvent>().Subscribe(x => this.DisplayFolderPath = x.Path);
            this.eventAggregator.GetEvent<FileEnterEvent>().Subscribe(x => this.Save());
            
            this.dialogService = dialogService;
            this.Title = "名前を付けて保存";
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            if (parameters.TryGetValue(DialogParameterNames.FileNameText, out string fileNameText))
                this.FileNameText = fileNameText;
            if (parameters.TryGetValue(DialogParameterNames.FileTypeText, out string fileTypeText))
                this.FileTypeText = fileTypeText;
            if (parameters.TryGetValue(DialogParameterNames.SaveButtonText, out string saveButtonText))
                this.SaveButtonText = saveButtonText;
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
                this.CancelButtonText = cancelButtonText;
            if (parameters.TryGetValue(DialogParameterNames.InitialSaveFileName, out string initialSaveFileName))
                this.SaveFileName = initialSaveFileName;
            if (parameters.TryGetValue(DialogParameterNames.Filters, out IEnumerable<FileFilter> filters))
            {
                this.filters.Clear();
                this.filters.AddRange(filters);
            }
            else
            {
                this.filters.Clear();
                string defaultAllFilesFilterText = "すべてのファイル (*.*)";
                if (parameters.TryGetValue(DialogParameterNames.DefaultAllFilesFilterText, out string temp))
                    defaultAllFilesFilterText = temp;
                this.filters.Add(new FileFilter(defaultAllFilesFilterText));
            }
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationTitle, out string overwriteConfirmationTitle))
                this.OverwriteConfirmationTitle = overwriteConfirmationTitle;
            else
                this.OverwriteConfirmationTitle = $"{this.Title}の確認";
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationMessageFunc, out Func<string, string> overwriteConfirmationMessageFunc))
                this.OverwriteConfirmationMessageFunc = overwriteConfirmationMessageFunc;
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationOKButtonText, out string overwriteConfirmationOKButtonText))
                this.OverwriteConfirmationOKButtonText = overwriteConfirmationOKButtonText;
            if (parameters.TryGetValue(DialogParameterNames.OverwriteConfirmationCancelButtonText, out string overwriteConfirmationCancelButtonText))
                this.OverwriteConfirmationCancelButtonText = overwriteConfirmationCancelButtonText;
            var regionContext = ExplorerBaseRegionContext.CreateForSingleFileSelect();
            if (parameters.TryGetValue(DialogParameterNames.TextResource, out ExplorerBaseTextResource textResource))
                regionContext.TextResource = textResource;
            if (parameters.TryGetValue(DialogParameterNames.RootFolders, out IEnumerable<string> rootFolders))
                regionContext.RootFolders = rootFolders;
            this.RegionContext = regionContext;
        }

        private void Save()
        {
            string res = Path.Combine(this.DisplayFolderPath, this.SaveFileName);
            if (this.SelectedFilter.Extensions != null && this.SelectedFilter.Extensions.Any() && !Path.HasExtension(res))
                res = Path.ChangeExtension(res, this.SelectedFilter.Extensions.First());
            if (File.Exists(res))
            {
                bool confirmed = false;
                this.dialogService.ShowConfirmation(
                    this.OverwriteConfirmationMessageFunc?.Invoke(res),
                    this.OverwriteConfirmationTitle,
                    res => confirmed = res.Result == ButtonResult.OK,
                    this.OverwriteConfirmationOKButtonText,
                    this.OverwriteConfirmationCancelButtonText);
                if (!confirmed) return;
            }
            var param = new DialogParameters
            {
                { DialogResultParameterNames.SaveFilePath, res }
            };
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK, param));
        }

        private void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
    }
}
