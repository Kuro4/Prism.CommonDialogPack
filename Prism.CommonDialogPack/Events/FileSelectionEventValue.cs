using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack.Events
{
    public class FileSelectionEventValue
    {
        public IEnumerable<string> Paths { get; set; }
        public TargetFileType TargetFileType { get; set; }

        public FileSelectionEventValue()
        {
        }

        public FileSelectionEventValue(IEnumerable<string> paths, TargetFileType targetFileType)
        {
            this.Paths = paths;
            this.TargetFileType = targetFileType;
        }
    }
}
