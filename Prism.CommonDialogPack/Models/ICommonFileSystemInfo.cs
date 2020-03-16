using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack.Models
{
    public interface ICommonFileSystemInfo
    {
        string Path { get; }
        string Name { get; }
        string Extension { get; }
        long? Length { get; }
        bool HasError { get; }
        bool IsSelected { get; set; }
        FileType FileType { get; }
    }
}
