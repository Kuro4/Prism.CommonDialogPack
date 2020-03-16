using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Prism.CommonDialogPack.Models
{
    public class CommonFileSystemInfo : BindableBase, ICommonFileSystemInfo
    {
        private string path;
        public string Path
        {
            get { return this.path; }
            private set { SetProperty(ref this.path, value); }
        }

        private string name;
        public string Name
        {
            get { return this.name; }
            private set { SetProperty(ref this.name, value); }
        }

        private DateTime lastWriteTime;
        public DateTime LastWriteTime
        {
            get { return lastWriteTime; }
            private set { SetProperty(ref this.lastWriteTime, value); }
        }

        private string extension = string.Empty;
        public string Extension
        {
            get { return extension; }
            private set { SetProperty(ref this.extension, value); }
        }

        private long? length = null;
        public long? Length
        {
            get { return length; }
            private set { SetProperty(ref this.length, value); }
        }

        private bool hasError = true;
        public bool HasError
        {
            get { return this.hasError; }
            private set { SetProperty(ref this.hasError, value); }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { SetProperty(ref this.isSelected, value); }
        }

        public FileType FileType { get; } = FileType.Unknown;

        protected FileSystemInfo Info { get; }

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

        public CommonFileSystemInfo(FileSystemInfo fileSystemInfo) : this(fileSystemInfo.FullName)
        {
        }

        public override string ToString()
        {
            return this.Path;
        }
    }
}
