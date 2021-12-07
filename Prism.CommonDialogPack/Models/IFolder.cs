using System.Collections.ObjectModel;

namespace Prism.CommonDialogPack.Models
{
    /// <summary>
    /// Folder interface for TreeView binding.
    /// </summary>
    public interface IFolder
    {
        /// <summary>
        /// Folder path.
        /// </summary>
        string Path { get; }
        /// <summary>
        /// Folder name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Has folder error.
        /// </summary>
        bool HasError { get; }
        /// <summary>
        /// Is folder expanded.
        /// </summary>
        bool IsExpanded { get; set; }
        /// <summary>
        /// Has folder expanded.
        /// </summary>
        bool HasExpanded { get; }
        /// <summary>
        /// Is folder selected.
        /// </summary>
        bool IsSelected { get; set; }
        /// <summary>
        /// Sub folders.
        /// </summary>
        ReadOnlyObservableCollection<IFolder> Children { get; }

        /// <summary>
        /// Expand folder.
        /// </summary>
        void Expand();
        /// <summary>
        /// Collapse folder.
        /// </summary>
        void Collapse();
        /// <summary>
        /// Reload folder.
        /// </summary>
        void Reload();
    }
}
