using Prism.Commands;
using Prism.CommonDialogPack.Events;
using Prism.CommonDialogPack.Models;
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
        #region Command
        private DelegateCommand goBackwardCommand;
        /// <summary>
        /// 進むコマンド
        /// </summary>
        public DelegateCommand GoBackwardCommand => this.goBackwardCommand ?? (this.goBackwardCommand = new DelegateCommand(this.GoBackward).ObservesCanExecute(() => this.History.CanGoBackward));

        private DelegateCommand goForwardCommand;
        /// <summary>
        /// 戻るコマンド
        /// </summary>
        public DelegateCommand GoForwardCommand => this.goForwardCommand ?? (this.goForwardCommand = new DelegateCommand(this.GoForward).ObservesCanExecute(() => this.History.CanGoForward));

        private DelegateCommand goUpperFolderCommand;
        /// <summary>
        /// 上の階層に戻るコマンド
        /// </summary>
        public DelegateCommand GoUpperFolderCommand => this.goUpperFolderCommand ?? (this.goUpperFolderCommand = new DelegateCommand(this.GoUpperFolder).ObservesCanExecute(() => this.CanGoUpperFolder));

        private DelegateCommand reloadCommand;
        /// <summary>
        /// リロードコマンド
        /// </summary>
        public DelegateCommand ReloadCommand => this.reloadCommand ?? (this.reloadCommand = new DelegateCommand(this.Reload));

        private DelegateCommand selectedItemChangedCommand;
        /// <summary>
        /// 選択アイテム変更時のコマンド
        /// </summary>
        public DelegateCommand SelectedItemChangedCommand => this.selectedItemChangedCommand ?? (this.selectedItemChangedCommand = new DelegateCommand(this.OnSelection));
        #endregion
        #region View Text
        private string nameColumnText;
        public string NameColumnText
        {
            get { return this.nameColumnText; }
            set { SetProperty(ref this.nameColumnText, value); }
        }

        private string dateModifiedColumnText;
        public string DateModifiedColumnText
        {
            get { return this.dateModifiedColumnText; }
            set { SetProperty(ref this.dateModifiedColumnText, value); }
        }

        private string typeColumnText;
        public string TypeColumnText
        {
            get { return this.typeColumnText; }
            set { SetProperty(ref this.typeColumnText, value); }
        }

        private string sizeColumnText;
        public string SizeColumnText
        {
            get { return this.sizeColumnText; }
            set { SetProperty(ref this.sizeColumnText, value); }
        }
        #endregion
        private string displayFolderPath;
        /// <summary>
        /// 表示中のフォルダのパス
        /// </summary>
        public string DisplayFolderPath
        {
            get { return displayFolderPath; }
            set 
            {
                if (File.Exists(value)) 
                {
                    this.eventAggregator.GetEvent<FileEnterEvent>().Publish(new FileEnterEventValue(value));
                    return;
                }
                if (!Directory.Exists(value)) return;
                if (!string.IsNullOrEmpty(this.displayFolderPath) && this.displayFolderPath.Equals(value)) return;
                SetProperty(ref this.displayFolderPath, value);
                this.SelectFolder(value);
                this.CanGoUpperFolder = !this.RootFolders.Any(f => f.Path.Equals(value));
                this.eventAggregator.GetEvent<MoveDisplayFolderEvent>().Publish(new MoveDisplayFolderEventValue(value));
            }
        }

        private IFolder selectedFolder;
        /// <summary>
        /// TreeView で選択しているフォルダ
        /// </summary>
        public IFolder SelectedFolder
        {
            get { return selectedFolder; }
            set 
            {
                SetProperty(ref this.selectedFolder, value);
                this.DisplayFolderPath = value.Path;
                this.UpdateDisplayFiles();
                if (this.CanEntry) this.History.Entry(value);
            }
        }

        private bool canGoUpperFolder = false;
        /// <summary>
        /// 上の階層へ移動できるかどうか
        /// </summary>
        public bool CanGoUpperFolder
        {
            get { return this.canGoUpperFolder; }
            set { SetProperty(ref this.canGoUpperFolder, value); }
        }

        private bool canMultiSelect = false;
        /// <summary>
        /// 複数選択可能かどうか
        /// </summary>
        public bool CanMultiSelect
        {
            get { return this.canMultiSelect; }
            set { SetProperty(ref this.canMultiSelect, value); }
        }

        /// <summary>
        /// 表示対象
        /// </summary>
        public TargetFileType DisplayTarget { get; set; } = TargetFileType.FileAndFolder;
        /// <summary>
        /// 選択対象
        /// </summary>
        public TargetFileType SelectionTarget { get; set; } = TargetFileType.FileOnly;

        private IEnumerable<string> fileExtensions;
        /// <summary>
        /// 検索するファイルの拡張子
        /// </summary>
        public IEnumerable<string> FileExtensions 
        {
            get { return this.fileExtensions; }
            set 
            {
                this.fileExtensions = value;
                this.UpdateDisplayFiles();
            } 
        }
        /// <summary>
        /// フォルダーの移動履歴
        /// </summary>
        public History<IFolder> History { get; } = new History<IFolder>(10);

        /// <summary>
        /// ルートフォルダー
        /// </summary>
        public ObservableCollection<IFolder> RootFolders { get; } = new ObservableCollection<IFolder>();

        private readonly ObservableCollection<CommonFileSystemInfo> displayFiles = new ObservableCollection<CommonFileSystemInfo>();
        /// <summary>
        /// 表示中のファイル
        /// </summary>
        public ReadOnlyObservableCollection<CommonFileSystemInfo> DisplayFiles { get; }

        /// <summary>
        /// 履歴の登録が可能かどうか
        /// </summary>
        private bool CanEntry { get; set; } = true;

        private readonly IEventAggregator eventAggregator;

        public ExplorerBaseViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.DisplayFiles = new ReadOnlyObservableCollection<CommonFileSystemInfo>(this.displayFiles);
            try
            {
                this.RootFolders.AddRange(DriveInfo.GetDrives().Where(d => d.IsReady).Select(d => new Folder(d.Name)));
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is IOException)
            {
                Debug.WriteLine(e.Message);
            }
            if (!this.RootFolders.Any()) return;
            var defaultFolder = this.RootFolders.First();
            defaultFolder.IsSelected = true;
        }

        private void UpdateDisplayFiles()
        {
            if (this.SelectedFolder == null) return;
            this.displayFiles.Clear();
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
                            files = files.Where(x => this.FileExtensions.Contains(Path.GetExtension(x)));
                        }
                        paths = paths.Concat(files);
                        break;
                    case TargetFileType.FileOnly:
                        paths = Directory.EnumerateFiles(this.SelectedFolder.Path);
                        if (this.FileExtensions != null && this.FileExtensions.Any())
                        {
                            paths = paths.Where(x => this.FileExtensions.Contains(Path.GetExtension(x)));
                        }
                        break;
                    case TargetFileType.FolderOnly:
                        paths = Directory.EnumerateDirectories(this.SelectedFolder.Path);
                        break;
                    default:
                        throw new InvalidEnumArgumentException();
                }
                foreach (var path in paths)
                {
                    this.displayFiles.Add(new CommonFileSystemInfo(path));
                }
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void SelectFolder(string value)
        {
            var roots = this.RootFolders.Where(f => Path.GetPathRoot(f.Path).Equals(Path.GetPathRoot(value)));
            if (!roots.Any()) return;
            var root = roots.First();
            if (root.Path.Equals(value))
            {
                root.IsSelected = true;
                return;
            }
            var div = value.Split(Path.DirectorySeparatorChar);
            var target = root;
            var path = div[0];
            for (int i = 1; i < div.Length; i++)
            {
                path = Path.Combine(path, div[i]);
                target.IsExpanded = true;
                target = target.Children.First(f => f.Path.Equals(path));
            }
            target.IsSelected = true;
        }

        private void GoBackward()
        {
            this.CanEntry = false;
            this.History.GoBackward();
            this.History.Current.IsSelected = true;
            this.CanEntry = true;
        }

        private void GoForward()
        {
            this.CanEntry = false;
            this.History.GoForward();
            this.History.Current.IsSelected = true;
            this.CanEntry = true;
        }

        private void GoUpperFolder()
        {
            this.DisplayFolderPath = Directory.GetParent(this.DisplayFolderPath).FullName;
        }

        private void Reload()
        {
            this.selectedFolder?.Reload();
            this.UpdateDisplayFiles();
        }

        private void OnSelection()
        {
            var selectedFiles = this.DisplayFiles.Where(f => f.IsSelected);
            if (this.SelectionTarget != TargetFileType.FileAndFolder)
            {
                var fileType = this.SelectionTarget == TargetFileType.FileOnly ? FileType.File : FileType.Folder;
                selectedFiles = selectedFiles.Where(f => f.FileType == fileType);
            }
            if (!selectedFiles.Any()) return;
            var value = new FileSelectionEventValue()
            {
                Paths = selectedFiles.Select(f => f.Path),
                TargetFileType = this.SelectionTarget,
            };
            this.eventAggregator.GetEvent<FileSelectionEvent>().Publish(value);
        }
    }
}
