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

        private string fileText = "ファイル：";
        public string FileText
        {
            get { return this.fileText; }
            set { SetProperty(ref this.fileText, value); }
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

        private string selectedFilePath;
        public string SelectedFilePath
        {
            get { return this.selectedFilePath; }
            set { SetProperty(ref this.selectedFilePath, value); }
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
                this.SelectedFilePath = string.Empty;
            }
        }

        private string DisplayFolderPath { get; set; }
        private readonly IEventAggregator eventAggregator;

        public FileSelectDialogViewModel(IEventAggregator eventAggregator)
        {
            this.Filters = new ReadOnlyObservableCollection<FileFilter>(this.filters);

            this.eventAggregator = eventAggregator;
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(x =>
            {
                if (x.Paths.Count() <= 1)
                {
                    this.SelectedFilePath = Path.GetFileName(x.Paths.First());
                    return;
                }
                this.SelectedFilePath = string.Join(' ', x.Paths.Select(p => $"\"{Path.GetFileName(p)}\""));
            }, ThreadOption.UIThread);
            this.eventAggregator.GetEvent<MoveDisplayFolderEvent>().Subscribe(x => this.DisplayFolderPath = x.Path);
            this.eventAggregator.GetEvent<FileEnterEvent>().Subscribe(x => this.Select());            
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            if (parameters.TryGetValue(DialogParameterNames.FileText, out string fileText))
                this.FileText = fileText;
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

        private void Select()
        {
            IEnumerable<string> res;
            if (string.IsNullOrEmpty(this.SelectedFilePath))
                res = Enumerable.Empty<string>();
            else if (!this.SelectedFilePath.Contains('\"'))
                res = new string[] { Path.Combine(this.DisplayFolderPath, this.SelectedFilePath) };
            else
                res = this.SelectedFilePath.Unwind('\"').Select(x => Path.Combine(this.DisplayFolderPath, x));
            var param = new DialogParameters
            {
                { DialogParameterNames.SelectedPaths, res }
            };
            this.RaiseRequestClose(new DialogResult(ButtonResult.OK, param));
        }

        private void Cancel()
        {
            this.RaiseRequestClose(new DialogResult(ButtonResult.Cancel));
        }
    }
}
