using System;
using System.Collections.Generic;
using System.Text;

namespace Prism.CommonDialogPack.Events
{
    public class FileEnterEventValue
    {
        public string Path { get; set; }

        public FileEnterEventValue()
        {
        }

        public FileEnterEventValue(string path)
        {
            this.Path = path;
        }
    }
}
