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
        private DelegateCommand selectCommand;
        public DelegateCommand SelectCommand => this.selectCommand ?? (this.selectCommand = new DelegateCommand(this.Select));

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

        private string selectedFileName;
        public string SelectedFileName
        {
            get { return this.selectedFileName; }
            set { SetProperty(ref this.selectedFileName, value); }
        }

        private ObservableCollection<FileFilter> filters = new ObservableCollection<FileFilter>();
        public ReadOnlyObservableCollection<FileFilter> Filters { get; }

        private FileFilter selectedFilter;
        public FileFilter SelectedFilter
        {
            get { return this.selectedFilter; }
            set
            {
                if (this.selectButtonText.Equals(value)) return;
                SetProperty(ref this.selectedFilter, value);
                var context = new ExplorerBaseRegionContext(this.RegionContext)
                {
                    FileExtensions = value.Extensions
                };
                this.RegionContext = context;
                this.SelectedFileName = string.Empty;
            }
        }

        private string DisplayFolderPath { get; set; }
        private readonly IEventAggregator eventAggregator;

        public FileSaveDialogViewModel(IEventAggregator eventAggregator)
        {
            this.Filters = new ReadOnlyObservableCollection<FileFilter>(this.filters);

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(x =>
            {
                if (x.Paths.Count() <= 1)
                {
                    this.SelectedFileName = Path.GetFileName(x.Paths.First());
                    return;
                }
                this.SelectedFileName = string.Join(' ', x.Paths.Select(p => $"\"{Path.GetFileName(p)}\""));
            }, ThreadOption.UIThread);
            this.eventAggregator.GetEvent<MoveDisplayFolderEvent>().Subscribe(x => this.DisplayFolderPath = x.Path);
            this.eventAggregator.GetEvent<FileEnterEvent>().Subscribe(x => this.Select());
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            if (parameters.TryGetValue(DialogParameterNames.FileNameText, out string fileNameText))
                this.FileNameText = fileNameText;
            if (parameters.TryGetValue(DialogParameterNames.FileTypeText, out string fileTypeText))
                this.FileTypeText = fileTypeText;
            if (parameters.TryGetValue(DialogParameterNames.SelectButtonText, out string selectButtonText))
                this.SelectButtonText = selectButtonText;
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
                this.CancelButtonText = cancelButtonText;
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
            var regionContext = ExplorerBaseRegionContext.CreateForSingleFileSelect();
            if (parameters.TryGetValue(DialogParameterNames.TextResource, out ExplorerBaseTextResource textResource))
                regionContext.TextResource = textResource;
            if (parameters.TryGetValue(DialogParameterNames.RootFolders, out IEnumerable<string> rootFolders))
                regionContext.RootFolders = rootFolders;
            this.RegionContext = regionContext;
        }

        private void Select()
        {
            string res = Path.HasExtension(this.SelectedFileName) ? this.SelectedFileName : Path.ChangeExtension(this.SelectedFileName, "");
            var param = new DialogParameters
            {
                { DialogParameterNames.SaveFileName, Path.Combine(this.DisplayFolderPath, res) }
            };
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK, param));
        }

        private void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
    }
}
