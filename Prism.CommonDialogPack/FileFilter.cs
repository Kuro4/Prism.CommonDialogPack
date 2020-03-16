using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.CommonDialogPack
{
    public class FileFilter
    {
        /// <summary>
        /// テキスト
        /// </summary>
        public string Text { get; set; }

        private List<string> extensions { get; } = new List<string>();
        /// <summary>
        /// 拡張子リスト
        /// </summary>
        public IEnumerable<string> Extensions => this.extensions;

        public FileFilter(string text)
        {
            this.Text = text;
        }

        public FileFilter(string text, IEnumerable<string> extensions) : this(text)
        {
            this.extensions = new List<string>(extensions);
        }

        public FileFilter(string text, params string[] extensions) : this(text, (IEnumerable<string>)extensions)
        {
        }

        public void Add(string extension)
        {
            this.extensions.Add(extension);
        }

        public void AddRange(IEnumerable<string> extensions)
        {
            this.extensions.AddRange(extensions);
        }

        public void Remove(string extension)
        {
            this.extensions.Remove(extension);
        }

        public void Clear()
        {
            this.extensions.Clear();
        }
    }
}
