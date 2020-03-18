using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack.Events
{
    public class MoveDisplayFolderEventValue
    {
        public string Path { get; set; }

        public MoveDisplayFolderEventValue()
        {
        }

        public MoveDisplayFolderEventValue(string path)
        {
            this.Path = path;
        }
    }
}
