using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Prism.CommonDialogPack.Models
{
    public class Folder : BindableBase, IFolder
    {
        private string path;
        /// <summary>
        /// Folder path.
        /// </summary>
        public string Path
        {
            get { return this.path; }
            private set { SetProperty(ref this.path, value); }
        }

        private string name;
        /// <summary>
        /// Folder name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            private set { SetProperty(ref this.name, value); }
        }

        private bool hasError = true;
        /// <summary>
        /// Has folder error.
        /// </summary>
        public bool HasError
        {
            get { return this.hasError; }
            private set { SetProperty(ref this.hasError, value); }
        }

        private bool isExpanded = false;
        /// <summary>
        /// Is folder expanded.
        /// </summary>
        public bool IsExpanded
        {
            get { return this.isExpanded; }
            set 
            {
                if (value) Expand();
                else Collapse();
            }
        }

        private bool hasExpanded = false;
        /// <summary>
        /// Has folder expanded even once.
        /// </summary>
        public bool HasExpanded
        {
            get { return this.hasExpanded; }
            private set { SetProperty(ref this.hasExpanded, value); }
        }

        private bool isSelected = false;
        /// <summary>
        /// Is folder selected.
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { SetProperty(ref this.isSelected, value); }
        }

        protected readonly ObservableCollection<IFolder> children = new ObservableCollection<IFolder>();
        /// <summary>
        /// Children folders.
        /// </summary>
        public ReadOnlyObservableCollection<IFolder> Children { get; }

        /// <summary>
        /// Directory.
        /// </summary>
        protected DirectoryInfo Directory { get; }

        /// <summary>
        /// Initialize a new instance of the <see cref="Folder"/> class width the specified path.
        /// </summary>
        /// <param name="path"></param>
        public Folder(string path)
        {
            this.Children = new ReadOnlyObservableCollection<IFolder>(this.children);
            this.Path = path;
            if (!System.IO.Directory.Exists(path)) return;
            this.Directory = new DirectoryInfo(path);
            this.Initialize();
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="Folder"/> class width the specified <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="dir"></param>
        public Folder(DirectoryInfo dir)
        {
            this.Children = new ReadOnlyObservableCollection<IFolder>(this.children);
            this.Path = this.Directory.FullName;
            this.Directory = dir;
            if (!dir.Exists) return;
            this.Initialize();
        }
        /// <summary>
        /// Initialize folder.
        /// </summary>
        protected virtual void Initialize()
        {
            this.Name = this.Directory.Name;
            this.HasError = false;
            try
            {
                if (this.Directory.EnumerateDirectories().Any())
                {
                    this.children.Add(CreateDammy());
                }
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException)
            {
                Debug.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Expand folder.
        /// </summary>
        public virtual void Expand()
        {
            if (this.IsExpanded) return;
            this.isExpanded = true;
            RaisePropertyChanged(nameof(this.IsExpanded));
            if (this.HasExpanded) return;
            this.HasExpanded = true;
            Reload();
        }
        /// <summary>
        /// Collapse folder.
        /// </summary>
        public virtual void Collapse()
        {
            if (!this.IsExpanded) return;
            this.isExpanded = false;
            RaisePropertyChanged(nameof(this.IsExpanded));
        }
        /// <summary>
        /// Reload folder.
        /// </summary>
        public virtual void Reload()
        {
            this.children.Clear();
            try
            {
                foreach (var subDir in this.Directory.EnumerateDirectories())
                {
                    this.children.Add(new Folder(subDir.FullName));
                }
            }
            catch (Exception e) when (e is UnauthorizedAccessException || e is PathTooLongException)
            {
                Debug.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Return the path to this folder.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Path;
        }
        /// <summary>
        /// Create and return a dummy folder.
        /// </summary>
        /// <returns></returns>
        public static Folder CreateDammy()
        {
            return new Folder(string.Empty);
        }
    }
}
