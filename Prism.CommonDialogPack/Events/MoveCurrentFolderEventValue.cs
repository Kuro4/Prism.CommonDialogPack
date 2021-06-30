using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack.Events
{
    public class MoveCurrentFolderEventValue
    {
        public string Path { get; set; }

        public MoveCurrentFolderEventValue()
        {
        }

        public MoveCurrentFolderEventValue(string path)
        {
            this.Path = path;
        }
    }
}
