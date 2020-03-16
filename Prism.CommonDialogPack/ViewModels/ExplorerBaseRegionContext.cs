using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ExplorerBaseRegionContext
    {
        /// <summary>
        /// 表示するテキスト
        /// </summary>
        public ExplorerBaseTextResource TextResource { get; set; } = new ExplorerBaseTextResource();
        /// <summary>
        /// 表示対象
        /// </summary>
        public TargetFileType DisplayTarget { get; set; } = TargetFileType.FileAndFolder;
        /// <summary>
        /// 選択対象
        /// </summary>
        public TargetFileType SelectionTarget { get; set; } = TargetFileType.FileOnly;
        /// <summary>
        /// 複数選択可能かどうか
        /// </summary>
        public bool CanMultiSelect { get; set; } = false;
        /// <summary>
        /// 検索するファイルの拡張子
        /// </summary>
        public IEnumerable<string> FileExtensions { get; set; }
        /// <summary>
        /// ルートに設定するフォルダのパス
        /// </summary>
        public IEnumerable<string> RootFolders { get; set; }

        public static ExplorerBaseRegionContext CreateForSingleFolderSelect()
        {
            return new ExplorerBaseRegionContext()
            {
                DisplayTarget = TargetFileType.FolderOnly,
                SelectionTarget = TargetFileType.FolderOnly,
            };
        }

        public static ExplorerBaseRegionContext CreateForMultiFolderSelect()
        {
            return new ExplorerBaseRegionContext()
            {
                DisplayTarget = TargetFileType.FolderOnly,
                SelectionTarget = TargetFileType.FolderOnly,
                CanMultiSelect = true,
            };
        }

        public static ExplorerBaseRegionContext CreateForSingleFileSelect()
        {
            return new ExplorerBaseRegionContext()
            {
                DisplayTarget = TargetFileType.FileAndFolder,
                SelectionTarget = TargetFileType.FileOnly,
            };
        }

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
