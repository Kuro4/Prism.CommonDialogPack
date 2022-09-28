using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Prism.CommonDialogPack.ViewModels
{
    public class ColorPickerDialogViewModel : DialogViewModelBase
    {
        //public Color Red { get; } = Color.Red;
        public string Red { get; } = "Red";
        public System.Windows.Media.Brush RedBrush { get; } = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
        public List<Color> Colors { get; } = new List<Color> { Color.Red, Color.Green, Color.Blue, Color.White, Color.Black, Color.Orange, Color.SkyBlue, Color.Magenta, Color.Gray, Color.PaleGreen };

        public ColorPickerDialogViewModel()
        {
        }
    }
}
