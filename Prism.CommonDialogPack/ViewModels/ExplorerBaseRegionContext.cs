using System.Collections.Generic;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ExplorerBaseRegionContext
    {
        /// <summary>
        /// Text resource for <see cref="ExplorerBaseViewModel"/>
        /// </summary>
        public ExplorerBaseTextResource TextResource { get; set; } = new ExplorerBaseTextResource();
        /// <summary>
        /// File type to be display.
        /// </summary>
        public TargetFileType DisplayTarget { get; set; } = TargetFileType.FileAndFolder;
        /// <summary>
        /// File type to be selected.
        /// </summary>
        public TargetFileType SelectionTarget { get; set; } = TargetFileType.FileOnly;
        /// <summary>
        /// Can multi select.
        /// </summary>
        public bool CanMultiSelect { get; set; } = false;
        /// <summary>
        /// File extensions to be search target.
        /// </summary>
        public IEnumerable<string> FileExtensions { get; set; }
        /// <summary>
        /// Root folders.
        /// </summary>
        public IEnumerable<string> RootFolders { get; set; }

        /// <summary>
        /// Initialize a new instance of the <see cref="ExplorerBaseRegionContext"/> class.
        /// </summary>
        public ExplorerBaseRegionContext()
        {
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="ExplorerBaseRegionContext"/> class with the same as <paramref name="source"/>.
        /// </summary>
        /// <param name="source"></param>
        public ExplorerBaseRegionContext(ExplorerBaseRegionContext source)
        {
            this.TextResource = source.TextResource;
            this.DisplayTarget = source.DisplayTarget;
            this.SelectionTarget = source.SelectionTarget;
            this.CanMultiSelect = source.CanMultiSelect;
            this.FileExtensions = source.FileExtensions;
            this.RootFolders = source.RootFolders;
        }
        /// <summary>
        /// Create <see cref="ExplorerBaseRegionContext"/> for single folder select.
        /// </summary>
        /// <returns></returns>
        public static ExplorerBaseRegionContext CreateForSingleFolderSelect()
        {
            return new ExplorerBaseRegionContext()
            {
                DisplayTarget = TargetFileType.FolderOnly,
                SelectionTarget = TargetFileType.FolderOnly,
            };
        }
        /// <summary>
        /// Create <see cref="ExplorerBaseRegionContext"/> for multi folder select.
        /// </summary>
        /// <returns></returns>
        public static ExplorerBaseRegionContext CreateForMultiFolderSelect()
        {
            return new ExplorerBaseRegionContext()
            {
                DisplayTarget = TargetFileType.FolderOnly,
                SelectionTarget = TargetFileType.FolderOnly,
                CanMultiSelect = true,
            };
        }
        /// <summary>
        /// Create <see cref="ExplorerBaseRegionContext"/> for single file select.
        /// </summary>
        /// <returns></returns>
        public static ExplorerBaseRegionContext CreateForSingleFileSelect()
        {
            return new ExplorerBaseRegionContext()
            {
                DisplayTarget = TargetFileType.FileAndFolder,
                SelectionTarget = TargetFileType.FileOnly,
            };
        }
        /// <summary>
        /// Create <see cref="ExplorerBaseRegionContext"/> for multi file select.
        /// </summary>
        /// <returns></returns>
        public static ExplorerBaseRegionContext CreateForMultiFileSelect()
        {
            return new ExplorerBaseRegionContext()
            {
                DisplayTarget = TargetFileType.FileAndFolder,
                SelectionTarget = TargetFileType.FileOnly,
                CanMultiSelect = true,
            };
        }
    }
}
