using Prism.Commands;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Extensions;
using Prism.Events;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class FileSelectDialogViewModel : ExplorerDialogViewModelBase
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

        private string fileNamePrefixText = "ファイル：";
        /// <summary>
        /// File name prefix text.
        /// </summary>
        public string FileNamePrefixText
        {
            get { return this.fileNamePrefixText; }
            set { SetProperty(ref this.fileNamePrefixText, value); }
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

        private string selectedFileName;
        /// <summary>
        /// Selected file name.
        /// </summary>
        public string SelectedFileName
        {
            get { return this.selectedFileName; }
            set { SetProperty(ref this.selectedFileName, value); }
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
                if (this.selectButtonText.Equals(value))
                {
                    return;
                }
                SetProperty(ref this.selectedFilter, value);
                var context = new ExplorerBaseRegionContext(this.RegionContext)
                {
                    FileExtensions = value.Extensions
                };
                this.RegionContext = context;
                this.SelectedFileName = string.Empty;
            }
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
        /// Initialize a new instance of the <see cref="FileSelectDialogViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator"></param>
        public FileSelectDialogViewModel(IEventAggregator eventAggregator)
        {
            this.Filters = new ReadOnlyObservableCollection<FileFilter>(this.filters);
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
            this.eventAggregator.GetEvent<FileEnterEvent>().Subscribe(this.OnFileEnter);
            // Configure parameters
            if (parameters.TryGetValue(DialogParameterNames.FileNamePrefixText, out string fileNamePrefixText))
            {
                this.FileNamePrefixText = fileNamePrefixText;
            }
            if (parameters.TryGetValue(DialogParameterNames.SelectButtonText, out string selectButtonText))
            {
                this.SelectButtonText = selectButtonText;
            }
            if (parameters.TryGetValue(DialogParameterNames.CancelButtonText, out string cancelButtonText))
            {
                this.CancelButtonText = cancelButtonText;
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
            var regionContext = ExplorerBaseRegionContext.CreateForSingleFileSelect();
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
            this.eventAggregator.GetEvent<MoveCurrentFolderEvent>().Unsubscribe(this.OnMoveCurrentFolder);
            this.eventAggregator.GetEvent<FileEnterEvent>().Unsubscribe(this.OnFileEnter);
        }
        /// <summary>
        /// Called when the file selected.
        /// </summary>
        public void OnFileSelection(FileSelectionEventValue value)
        {
            if (value.Paths.Count() <= 1)
            {
                this.SelectedFileName = Path.GetFileName(value.Paths.First());
                return;
            }
            this.SelectedFileName = string.Join(' ', value.Paths.Select(p => $"\"{Path.GetFileName(p)}\""));
        }
        /// <summary>
        /// Called when the move current folder.
        /// </summary>
        /// <param name="value"></param>
        public void OnMoveCurrentFolder(MoveCurrentFolderEventValue value) 
        {
            this.CurrentFolderPath = value.Path;
            this.SelectedFileName = string.Empty;
        }
        /// <summary>
        /// Called when the file entered.
        /// </summary>
        /// <param name="value"></param>
        public void OnFileEnter(FileEnterEventValue value)
        {
            this.Select();
        }
        /// <summary>
        /// Close the dialog with the result as <see cref="ButtonResult.OK"/>.
        /// </summary>
        /// <remarks>
        /// Dialog parameters is selected file path.
        /// </remarks>
        private void Select()
        {
            IEnumerable<string> res;
            if (string.IsNullOrEmpty(this.SelectedFileName))
            {
                res = Enumerable.Empty<string>();
            }
            else if (!this.SelectedFileName.Contains('\"'))
            {
                res = new string[] { Path.Combine(this.CurrentFolderPath, this.SelectedFileName) };
            }
            else
            {
                res = this.SelectedFileName.Unwind('\"').Select(x => Path.Combine(this.CurrentFolderPath, x));
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
