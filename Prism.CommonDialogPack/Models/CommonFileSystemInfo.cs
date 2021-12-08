using Prism.Mvvm;
using System;
using System.IO;

namespace Prism.CommonDialogPack.Models
{
    public class CommonFileSystemInfo : BindableBase, ICommonFileSystemInfo
    {
        private string path;
        /// <summary>
        /// File path.
        /// </summary>
        public string Path
        {
            get { return this.path; }
            private set { SetProperty(ref this.path, value); }
        }

        private string name;
        /// <summary>
        /// FileName
        /// </summary>
        public string Name
        {
            get { return this.name; }
            private set { SetProperty(ref this.name, value); }
        }

        private DateTime lastWriteTime;
        /// <summary>
        /// File last write time.
        /// </summary>
        public DateTime LastWriteTime
        {
            get { return lastWriteTime; }
            private set { SetProperty(ref this.lastWriteTime, value); }
        }

        private string extension = string.Empty;
        /// <summary>
        /// File extension.
        /// </summary>
        public string Extension
        {
            get { return extension; }
            private set { SetProperty(ref this.extension, value); }
        }

        private long? length = null;
        /// <summary>
        /// File size.
        /// </summary>
        public long? Length
        {
            get { return length; }
            private set { SetProperty(ref this.length, value); }
        }

        private bool hasError = true;
        /// <summary>
        /// Has error.
        /// </summary>
        public bool HasError
        {
            get { return this.hasError; }
            private set { SetProperty(ref this.hasError, value); }
        }

        private bool isSelected = false;
        /// <summary>
        /// Is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { SetProperty(ref this.isSelected, value); }
        }

        /// <summary>
        /// File type.
        /// </summary>
        public FileType FileType { get; } = FileType.Unknown;
        /// <summary>
        /// File info.
        /// </summary>
        protected FileSystemInfo Info { get; }

        /// <summary>
        /// Initialize a new instance of the <see cref="CommonFileSystemInfo"/> class width the specified path.
        /// </summary>
        /// <param name="path">File path.</param>
        public CommonFileSystemInfo(string path)
        {
            this.Path = path;
            if (Directory.Exists(path))
            {
                var dir = new DirectoryInfo(path);
                this.Name = dir.Name;
                this.LastWriteTime = dir.LastWriteTime;
                this.FileType = FileType.Folder;
                this.Info = dir;
            }
            else if (File.Exists(path))
            {
                var file = new FileInfo(path);
                this.Name = file.Name;
                this.LastWriteTime = file.LastWriteTime;
                this.Extension = file.Extension;
                this.Length = file.Length;
                this.FileType = FileType.File;
                this.Info = file;
            }
            else
            {
                return;
            }
            this.HasError = false;
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="CommonFileSystemInfo"/> class width the specified <see cref="FileSystemInfo"/>.
        /// </summary>
        /// <param name="fileSystemInfo">File system info.</param>
        public CommonFileSystemInfo(FileSystemInfo fileSystemInfo) : this(fileSystemInfo.FullName)
        {
        }
        /// <summary>
        /// Return the path to this file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Path;
        }
    }
}
