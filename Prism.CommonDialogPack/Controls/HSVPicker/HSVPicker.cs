using Prism.CommonDialogPack.Models;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Prism.CommonDialogPack.Controls
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Prism.CommonDialogPack.Controls.HSVPicker"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Prism.CommonDialogPack.Controls.HSVPicker;assembly=Prism.CommonDialogPack.Controls.HSVPicker"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:HSVPicker/>
    ///
    /// </summary>
    public class HSVPicker : Control
    {
        [Description("BrightnessSliderWidth"), Category("レイアウト")]
        public double BrightnessSliderWidth
        {
            get { return (double)GetValue(BrightnessSliderWidthProperty); }
            set { SetValue(BrightnessSliderWidthProperty, value); }
        }
        // Using a DependencyProperty as the backing store for BrightnessSliderWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrightnessSliderWidthProperty =
            DependencyProperty.Register("BrightnessSliderWidth", typeof(double), typeof(HSVPicker), new PropertyMetadata(15d));

        [Description("HSV"), Category("共通")]
        public HSV HSV
        {
            get { return (HSV)GetValue(HSVProperty); }
            set { SetValue(HSVProperty, value); }
        }
        // Using a DependencyProperty as the backing store for HSV.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HSVProperty =
            DependencyProperty.Register("HSV", typeof(HSV), typeof(HSVPicker), new PropertyMetadata(new HSV(0, 0, 1), HSVValueChanged));

        [Description("SelectedHSV"), Category("共通")]
        public HSV SelectedHSV
        {
            get { return (HSV)GetValue(SelectedHSVProperty); }
            set { SetValue(SelectedHSVProperty, value); }
        }
        // Using a DependencyProperty as the backing store for SelectedHSV.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedHSVProperty =
            DependencyProperty.Register("SelectedHSV", typeof(HSV), typeof(HSVPicker), new PropertyMetadata(new HSV(0, 0, 1)));

        [Description("DefaultHSV"), Category("共通")]
        public HSV DefaultHSV
        {
            get { return (HSV)GetValue(DefaultHSVProperty); }
            set { SetValue(DefaultHSVProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DefaultHSV.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultHSVProperty =
            DependencyProperty.Register("DefaultHSV", typeof(HSV), typeof(HSVPicker), new PropertyMetadata(new HSV(0, 0, 1)));

        HSPicker hsPicker;
        BrightnessSlider brightnessSlider;

        #region DependencyProperty CallBacks
        private static void HSVValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HSVPicker self = (HSVPicker)d;
            HSV value = (HSV)e.NewValue;
            self.hsPicker?.SetHS(value.H, value.S);
            self.brightnessSlider?.SetBrightness(value.V);
        }
        #endregion

        static HSVPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HSVPicker), new FrameworkPropertyMetadata(typeof(HSVPicker)));
        }

        public HSVPicker()
        {
            this.Loaded += HSVPicker_Loaded;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.hsPicker != null)
            {
                this.hsPicker.HSChangedEvent -= OnHSChanged;
            }
            if (this.brightnessSlider != null)
            {
                this.brightnessSlider.BrightnessChangedEvent -= OnBrightnessChanged;
            }

            this.hsPicker = this.GetTemplateChild("PART_HSPicker") as HSPicker;
            this.brightnessSlider = this.GetTemplateChild("PART_BrightnessSlider") as BrightnessSlider;
            
            if (this.hsPicker != null)
            {
                this.hsPicker.HSChangedEvent += OnHSChanged;
            }
            if (this.brightnessSlider != null)
            {
                this.brightnessSlider.BrightnessChangedEvent += OnBrightnessChanged;
            }
        }

        private void HSVPicker_Loaded(object sender, RoutedEventArgs e)
        {
            this.hsPicker.DefaultHue = this.DefaultHSV.H;
            this.hsPicker.DefaultSaturation = this.DefaultHSV.S;
            this.hsPicker.Hue = this.DefaultHSV.H;
            this.hsPicker.Saturation = this.DefaultHSV.S;
            this.brightnessSlider.DefaultBrightness = this.DefaultHSV.V;
        }
        /// <summary>
        /// It is called when "HSChanged" from HSPicker.
        /// <br/>Update <see cref="SelectedHSV"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHSChanged(object sender, HSChangedEventArgs e)
        {
            this.SelectedHSV = new HSV(e.Hue, e.Saturation, this.brightnessSlider.Brightness);
        }
        /// <summary>
        /// It is called when "BrightnessChanged" from BrightnessSlider.
        /// <br/>Update <see cref="SelectedHSV"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBrightnessChanged(object sender, BrightnessChangedEventArgs e)
        {
            this.SelectedHSV = new HSV(this.hsPicker.Hue, this.hsPicker.Saturation, e.Brightness);
        }
    }
}
