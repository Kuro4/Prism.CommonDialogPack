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
    public class FileSelectDialogViewModel : ExplorerDialogViewModelBase
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

        private readonly ObservableCollection<FileFilter> filters = new ObservableCollection<FileFilter>();
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

        public FileSelectDialogViewModel(IEventAggregator eventAggregator)
        {
            this.Filters = new ReadOnlyObservableCollection<FileFilter>(this.filters);

            this.eventAggregator = eventAggregator;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(this.OnFileSelection);
            this.eventAggregator.GetEvent<MoveDisplayFolderEvent>().Subscribe(this.OnMoveDisplayFolder);
            this.eventAggregator.GetEvent<FileEnterEvent>().Subscribe(this.OnFileEnter);
            if (parameters.TryGetValue(DialogParameterNames.FileNameText, out string fileNameText))
                this.FileNameText = fileNameText;
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
            this.eventAggregator.GetEvent<FileEnterEvent>().Unsubscribe(this.OnFileEnter);
        }

        public void OnFileSelection(FileSelectionEventValue value)
        {
            if (value.Paths.Count() <= 1)
            {
                this.SelectedFileName = Path.GetFileName(value.Paths.First());
                return;
            }
            this.SelectedFileName = string.Join(' ', value.Paths.Select(p => $"\"{Path.GetFileName(p)}\""));
        }

        public void OnMoveDisplayFolder(MoveDisplayFolderEventValue value) 
        {
            this.DisplayFolderPath = value.Path;
            this.SelectedFileName = string.Empty;
        }

        public void OnFileEnter(FileEnterEventValue value)
        {
            this.Select();
        }

        private void Select()
        {
            IEnumerable<string> res;
            if (string.IsNullOrEmpty(this.SelectedFileName))
                res = Enumerable.Empty<string>();
            else if (!this.SelectedFileName.Contains('\"'))
                res = new string[] { Path.Combine(this.DisplayFolderPath, this.SelectedFileName) };
            else
                res = this.SelectedFileName.Unwind('\"').Select(x => Path.Combine(this.DisplayFolderPath, x));
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
