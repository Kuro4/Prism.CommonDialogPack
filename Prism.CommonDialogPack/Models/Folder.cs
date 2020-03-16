using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Prism.CommonDialogPack.Models
{
    public class Folder : BindableBase, IFolder
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

        private bool hasError = true;
        public bool HasError
        {
            get { return this.hasError; }
            private set { SetProperty(ref this.hasError, value); }
        }

        private bool isExpanded = false;
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
        public bool HasExpanded
        {
            get { return this.hasExpanded; }
            private set { SetProperty(ref this.hasExpanded, value); }
        }

        private bool isSelected = false;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { SetProperty(ref this.isSelected, value); }
        }

        protected readonly ObservableCollection<IFolder> children = new ObservableCollection<IFolder>();
        public ReadOnlyObservableCollection<IFolder> Children { get; }

        protected DirectoryInfo Directory { get; }

        public Folder(string path)
        {
            this.Children = new ReadOnlyObservableCollection<IFolder>(this.children);
            this.Path = path;
            if (!System.IO.Directory.Exists(path)) return;
            this.Directory = new DirectoryInfo(path);
            this.Initialize();
        }

        public Folder(DirectoryInfo dir)
        {
            this.Children = new ReadOnlyObservableCollection<IFolder>(this.children);
            this.Path = this.Directory.FullName;
            this.Directory = dir;
            if (!dir.Exists) return;
            this.Initialize();
        }

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

        public virtual void Expand()
        {
            if (this.IsExpanded) return;
            this.isExpanded = true;
            RaisePropertyChanged(nameof(this.IsExpanded));
            if (this.HasExpanded) return;
            this.HasExpanded = true;
            Reload();
        }

        public virtual void Collapse()
        {
            if (!this.IsExpanded) return;
            this.isExpanded = false;
            RaisePropertyChanged(nameof(this.IsExpanded));
        }

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

        public override string ToString()
        {
            return this.Path;
        }

        public static Folder CreateDammy()
        {
            return new Folder(string.Empty);
        }
    }
}
