using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Prism.CommonDialogPack.Models
{
    public interface IFolder
    {
        string Path { get; }
        string Name { get; }
        bool HasError { get; }
        bool IsExpanded { get; set; }
        bool HasExpanded { get; }
        bool IsSelected { get; set; }
        ReadOnlyObservableCollection<IFolder> Children { get; }

        void Expand();
        void Collapse();
        void Reload();
    }
}
