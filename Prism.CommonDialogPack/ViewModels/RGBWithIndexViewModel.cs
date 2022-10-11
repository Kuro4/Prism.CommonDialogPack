using Prism.CommonDialogPack.Models;
using Prism.Mvvm;

namespace Prism.CommonDialogPack.ViewModels
{
    public class RGBWithIndexViewModel : BindableBase
    {
        private RGB rgb;
        /// <summary>
        /// RGB.
        /// </summary>
        public RGB RGB
        {
            get { return rgb; }
            set { SetProperty(ref rgb, value); }
        }

        private int index;
        /// <summary>
        /// Index of RGB.
        /// </summary>
        public int Index
        {
            get { return index; }
            set { SetProperty(ref index, value); }
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="RGBWithIndexViewModel"/> class.
        /// </summary>
        public RGBWithIndexViewModel()
        {

        }
        /// <summary>
        /// Initialize a new instance of the <see cref="RGBWithIndexViewModel"/> class.
        /// </summary>
        /// <param name="rgb">RGB</param>
        /// <param name="index">Index of RGB</param>
        public RGBWithIndexViewModel(RGB rgb, int index)
        {
            this.RGB = rgb;
            this.Index = index;
        }
    }
}
