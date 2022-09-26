using Prism.Commands;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Models;
using Prism.CommonDialogPack.Resources;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ExplorerBaseViewModel : BindableBase
    {
        private DelegateCommand goBackwardCommand;
        /// <summary>
        /// Go backward command.
        /// </summary>
        public DelegateCommand GoBackwardCommand => this.goBackwardCommand ??= new DelegateCommand(this.GoBackward).ObservesCanExecute(() => this.History.CanUndo);

        private DelegateCommand goForwardCommand;
        /// <summary>
        /// Go forward command.
        /// </summary>
        public DelegateCommand GoForwardCommand => this.goForwardCommand ??= new DelegateCommand(this.GoForward).ObservesCanExecute(() => this.History.CanRedo);

        private DelegateCommand goUpperFolderCommand;
        /// <summary>
        /// Go upper folder command.
        /// </summary>
        public DelegateCommand GoUpperFolderCommand => this.goUpperFolderCommand ??= new DelegateCommand(this.GoUpperFolder).ObservesCanExecute(() => this.CanGoUpperFolder);

        private DelegateCommand reloadCommand;
        /// <summary>
        /// Reload command.
        /// </summary>
        public DelegateCommand ReloadCommand => this.reloadCommand ??= new DelegateCommand(this.Reload);

        private DelegateCommand selectedItemChangedCommand;
        /// <summary>
        /// Selected item changed command.
        /// </summary>
        public DelegateCommand SelectedItemChangedCommand => this.selectedItemChangedCommand ??= new DelegateCommand(this.OnSelection);

        private DelegateCommand<object> enterCommande;
        /// <summary>
        /// Enter command.
        /// </summary>
        public DelegateCommand<object> EnterCommand => this.enterCommande ??= new DelegateCommand<object>(this.OnEnter);

        private string nameColumnText;
        /// <summary>
        /// Name column text.
        /// </summary>
        public string NameColumnText
        {
            get { return this.nameColumnText; }
            set { SetProperty(ref this.nameColumnText, value); }
        }

        private string dateModifiedColumnText;
        /// <summary>
        /// Date modified column text.
        /// </summary>
        public string DateModifiedColumnText
        {
            get { return this.dateModifiedColumnText; }
            set { SetProperty(ref this.dateModifiedColumnText, value); }
        }

        private string typeColumnText;
        /// <summary>
        /// Type column text.
        /// </summary>
        public string TypeColumnText
        {
            get { return this.typeColumnText; }
            set { SetProperty(ref this.typeColumnText, value); }
        }

        private string sizeColumnText;
        /// <summary>
        /// Size column text.
        /// </summary>
        public string SizeColumnText
        {
            get { return this.sizeColumnText; }
            set { SetProperty(ref this.sizeColumnText, value); }
        }

        private string currentFolderPath;
        /// <summary>
        /// Current folder path.
        /// <para>If a file path is entered, a <see cref="FileEnterEvent"/> will be published.</para>
        /// <para>If a folder path is entered, a <see cref="MoveCurrentFolderEvent"/> will be published.</para>
        /// </summary>
        public string CurrentFolderPath
        {
            get { return currentFolderPath; }
            set 
            {
                if (File.Exists(value)) 
                {
                    this.eventAggregator.GetEvent<FileEnterEvent>().Publish(new FileEnterEventValue(value));
                    return;
                }
                if (!Directory.Exists(value))
                {
                    return;
                }
                if (!string.IsNullOrEmpty(this.currentFolderPath) && this.currentFolderPath.Equals(value))
                {
                    return;
                }
                SetProperty(ref this.currentFolderPath, value);
                this.SelectFolder(value);
                this.CanGoUpperFolder = !this.RootFolders.Any(f => f.Path.Equals(value));
                this.eventAggregator.GetEvent<MoveCurrentFolderEvent>().Publish(new MoveCurrentFolderEventValue(value));
            }
        }

        private IFolder selectedFolder;
        /// <summary>
        /// Selected folder in TreeView.
        /// </summary>
        public IFolder SelectedFolder
        {
            get { return selectedFolder; }
            set 
            {
                SetProperty(ref this.selectedFolder, value);
                this.CurrentFolderPath = value.Path;
                this.UpdateCurrentFileSystems();
                if (this.CanEntry)
                {
                    this.History.Entry(value);
                }
            }
        }

        private bool canGoUpperFolder = false;
        /// <summary>
        /// Can go upper folder.
        /// </summary>
        public bool CanGoUpperFolder
        {
            get { return this.canGoUpperFolder; }
            set { SetProperty(ref this.canGoUpperFolder, value); }
        }

        private bool canMultiSelect = false;
        /// <summary>
        /// Can multi select.
        /// <para>Default: false</para>
        /// </summary>
        public bool CanMultiSelect
        {
            get { return this.canMultiSelect; }
            set { SetProperty(ref this.canMultiSelect, value); }
        }

        /// <summary>
        /// File type to be display.
        /// </summary>
        public TargetFileType DisplayTarget { get; set; } = TargetFileType.FileAndFolder;
        /// <summary>
        /// File type to be selected.
        /// </summary>
        public TargetFileType SelectionTarget { get; set; } = TargetFileType.FileOnly;

        private IEnumerable<string> fileExtensions;
        /// <summary>
        /// File extensions to be search target.
        /// </summary>
        public IEnumerable<string> FileExtensions 
        {
            get { return this.fileExtensions; }
            set 
            {
                this.fileExtensions = value;
                this.UpdateCurrentFileSystems();
            } 
        }

        /// <summary>
        /// Folder move history.
        /// </summary>
        public History<IFolder> History { get; } = new History<IFolder>(10);

        private readonly ObservableCollection<IFolder> rootFolders = new ObservableCollection<IFolder>();
        /// <summary>
        /// Root folders.
        /// </summary>
        public ReadOnlyObservableCollection<IFolder> RootFolders { get; }

        private readonly ObservableCollection<CommonFileSystemInfo> currentFileSystems = new ObservableCollection<CommonFileSystemInfo>();
        /// <summary>
        /// Current <see cref="CommonFileSystemInfo"/> collection.
        /// </summary>
        public ReadOnlyObservableCollection<CommonFileSystemInfo> CurrentFileSystems { get; }

        /// <summary>
        /// Can add to history.
        /// </summary>
        private bool CanEntry { get; set; } = true;
        /// <summary>
        /// Event aggregator.
        /// </summary>
        private readonly IEventAggregator eventAggregator;

        /// <summary>
        /// Initialize a new instance of the <see cref="ExplorerBaseViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator"></param>
        public ExplorerBaseViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.RootFolders = new ReadOnlyObservableCollection<IFolder>(this.rootFolders);
            this.CurrentFileSystems = new ReadOnlyObservableCollection<CommonFileSystemInfo>(this.currentFileSystems);
            this.InitializeRootFolders();
            // Set current directory from Settings.
            string path = Settings.Default.CurrentDirectoryPath;
            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
            {
                this.CurrentFolderPath = path;
                _ = System.Threading.Tasks.Task.Run(async () =>
                {
                    // Wait for SelectedFolder to be set on View side.
                    while (this.SelectedFolder is null || this.SelectedFolder.Path != path)
                    {
                        await System.Threading.Tasks.Task.Delay(100);
                    }
                    this.History.Clear();
                    this.History.Entry(this.SelectedFolder);
                });
            }
        }
        /// <summary>
        /// Initialize root folders with available drives.
        /// </summary>
        public void InitializeRootFolders()
        {
            try
            {
                var drives = DriveInfo.GetDrives().Where(d => d.IsReady).Select(d => new Folder(d.Name));
                if (!drives.Any())
                {
                    return;
                }
                this.InitializeRootFolders(drives);
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is IOException)
            {
                Debug.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Initialize root folders with <paramref name="rootFolders"/>.
        /// </summary>
        /// <param name="rootFolders"></param>
        public void InitializeRootFolders(IEnumerable<IFolder> rootFolders)
        {
            this.rootFolders.Clear();
            this.rootFolders.AddRange(rootFolders);            
            this.rootFolders.First().IsSelected = true;
        }
        /// <summary>
        /// Initialize root folders with <paramref name="rootFolder"/>.
        /// </summary>
        /// <param name="rootFolder"></param>
        public void InitializeRootFolders(IFolder rootFolder)
        {
            this.rootFolders.Clear();
            this.rootFolders.Add(rootFolder);
            this.rootFolders.First().IsSelected = true;
        }
        /// <summary>
        /// Update <see cref="CurrentFileSystems"/>.
        /// </summary>
        private void UpdateCurrentFileSystems()
        {
            if (this.SelectedFolder == null)
            {
                return;
            }
            this.currentFileSystems.Clear();
            try
            {
                IEnumerable<string> paths;
                switch (this.DisplayTarget)
                {
                    case TargetFileType.FileAndFolder:
                        paths = Directory.EnumerateDirectories(this.SelectedFolder.Path);
                        var files = Directory.EnumerateFiles(this.SelectedFolder.Path);
                        if (this.FileExtensions != null && this.FileExtensions.Any())
                        {
                            // Filtering files
                            files = files.Where(x => this.FileExtensions.Contains(Path.GetExtension(x)));
                        }
                        paths = paths.Concat(files);
                        break;
                    case TargetFileType.FileOnly:
                        paths = Directory.EnumerateFiles(this.SelectedFolder.Path);
                        if (this.FileExtensions != null && this.FileExtensions.Any())
                        {
                            // Filtering files
                            paths = paths.Where(x => this.FileExtensions.Contains(Path.GetExtension(x)));
                        }
                        break;
                    case TargetFileType.FolderOnly:
                        paths = Directory.EnumerateDirectories(this.SelectedFolder.Path);
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
                }
                this.currentFileSystems.AddRange(paths.Select(p => new CommonFileSystemInfo(p)));
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException)
            {
                Debug.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Select folder.
        /// </summary>
        /// <param name="path">target folder path</param>
        private void SelectFolder(string path)
        {
            var roots = this.RootFolders.Where(f => Path.GetPathRoot(f.Path).Equals(Path.GetPathRoot(path)));
            if (!roots.Any())
            {
                return;
            }
            // Set current directory path to Settings.
            if (Settings.Default.EnableDirectoryMemoization)
            {
                DialogSettings.SetCurrentDirectoryPath(path);
            }
            var root = roots.First();
            if (root.Path.Equals(path))
            {
                root.IsSelected = true;
                return;
            }
            // Expand the folders from root to path.
            var div = path.Split(Path.DirectorySeparatorChar);
            var target = root;
            var tmpPath = div[0];
            for (int i = 1; i < div.Length; i++)
            {
                tmpPath = Path.Combine(tmpPath, div[i]);
                target.IsExpanded = true;
                target = target.Children.First(f => f.Path.Equals(tmpPath));
            }
            target.IsSelected = true;
        }
        /// <summary>
        /// Go backward.
        /// </summary>
        private void GoBackward()
        {
            this.CanEntry = false;
            this.History.Undo();
            this.History.Current.IsSelected = true;
            this.CanEntry = true;
            // Set current directory path to Settings.
            if (Settings.Default.EnableDirectoryMemoization)
            {
                DialogSettings.SetCurrentDirectoryPath(this.History.Current.Path);
            }
        }
        /// <summary>
        /// Go forward.
        /// </summary>
        private void GoForward()
        {
            this.CanEntry = false;
            this.History.Redo();
            this.History.Current.IsSelected = true;
            this.CanEntry = true;
            // Set current directory path to Settings.
            if (Settings.Default.EnableDirectoryMemoization)
            {
                DialogSettings.SetCurrentDirectoryPath(this.History.Current.Path);
            }
        }
        /// <summary>
        /// Go upper folder.
        /// </summary>
        private void GoUpperFolder()
        {
            this.CurrentFolderPath = Directory.GetParent(this.CurrentFolderPath).FullName;
        }
        /// <summary>
        /// Reload current folder.
        /// </summary>
        private void Reload()
        {
            this.selectedFolder?.Reload();
            this.UpdateCurrentFileSystems();
        }
        /// <summary>
        /// Runs when the SelectedItemChanged event occurs in the DataGrid displaying a file.
        /// </summary>
        private void OnSelection()
        {
            var selectedFiles = this.CurrentFileSystems.Where(f => f.IsSelected);
            if (this.SelectionTarget != TargetFileType.FileAndFolder)
            {
                var fileType = this.SelectionTarget == TargetFileType.FileOnly ? FileType.File : FileType.Folder;
                selectedFiles = selectedFiles.Where(f => f.FileType == fileType);
            }
            if (!selectedFiles.Any())
            {
                return;
            }
            var value = new FileSelectionEventValue()
            {
                Paths = selectedFiles.Select(f => f.Path),
                TargetFileType = this.SelectionTarget,
            };
            this.eventAggregator.GetEvent<FileSelectionEvent>().Publish(value);
        }
        /// <summary>
        /// Runs when the Keydown event occurs in the DataGrid displaying a file.
        /// </summary>
        /// <param name="item"></param>
        public void OnEnter(object item)
        {
            if (!(item is CommonFileSystemInfo file))
            {
                return;
            }
            if (file.FileType == FileType.Unknown)
            {
                return;
            }
            this.CurrentFolderPath = file.Path;
        }
    }
}
