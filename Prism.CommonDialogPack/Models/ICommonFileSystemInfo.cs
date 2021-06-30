namespace Prism.CommonDialogPack.Models
{
    /// <summary>
    /// CommonFileSystem interface.
    /// </summary>
    public interface ICommonFileSystemInfo
    {
        /// <summary>
        /// File path.
        /// </summary>
        string Path { get; }
        /// <summary>
        /// File name.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// File extension.
        /// </summary>
        string Extension { get; }
        /// <summary>
        /// File size.
        /// </summary>
        long? Length { get; }
        /// <summary>
        /// Has file error.
        /// </summary>
        bool HasError { get; }
        /// <summary>
        /// Is selected file.
        /// </summary>
        bool IsSelected { get; set; }
        /// <summary>
        /// File type.
        /// </summary>
        FileType FileType { get; }
    }
}
