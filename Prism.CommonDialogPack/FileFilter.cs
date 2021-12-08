using System.Collections.Generic;
using System.Linq;

namespace Prism.CommonDialogPack
{
    public class FileFilter
    {
        /// <summary>
        /// Default all files filter text.
        /// </summary>
        public static string DefaultAllFilesFilterText { get; set; } = "すべてのファイル (*.*)";

        /// <summary>
        /// Filter text.
        /// </summary>
        public string Text { get; set; }

        private readonly List<string> extensions = new List<string>();
        /// <summary>
        /// Extensions.
        /// </summary>
        public IEnumerable<string> Extensions => this.extensions.AsEnumerable();

        /// <summary>
        /// Initialize a new instance of the <see cref="FileFilter"/> class.
        /// </summary>
        /// <param name="text">Filter text.</param>
        public FileFilter(string text)
        {
            this.Text = text;
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="FileFilter"/> class.
        /// </summary>
        /// <param name="text">Filter text.</param>
        /// <param name="extensions">Extensions.</param>
        public FileFilter(string text, IEnumerable<string> extensions) : this(text)
        {
            this.extensions = new List<string>(extensions);
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="FileFilter"/> class.
        /// </summary>
        /// <param name="text">Filter text.</param>
        /// <param name="extensions">Extensions.</param>
        public FileFilter(string text, params string[] extensions) : this(text, (IEnumerable<string>)extensions)
        {
        }
        /// <summary>
        /// Add extension.
        /// </summary>
        /// <param name="extension">Extension.</param>
        public void Add(string extension)
        {
            this.extensions.Add(extension);
        }
        /// <summary>
        /// Add extensions.
        /// </summary>
        /// <param name="extensions">Extensions.</param>
        public void AddRange(IEnumerable<string> extensions)
        {
            this.extensions.AddRange(extensions);
        }
        /// <summary>
        /// Remove extension.
        /// </summary>
        /// <param name="extension">Extension</param>
        /// <returns>Succcesfuly removed or not found.</returns>
        public bool Remove(string extension)
        {
            return this.extensions.Remove(extension);
        }
        /// <summary>
        /// Cler extensions.
        /// </summary>
        public void Clear()
        {
            this.extensions.Clear();
        }
        /// <summary>
        /// Create default file filter.
        /// </summary>
        /// <returns></returns>
        public static FileFilter CreateDefault()
        {
            return new FileFilter(DefaultAllFilesFilterText);
        }
    }
}
