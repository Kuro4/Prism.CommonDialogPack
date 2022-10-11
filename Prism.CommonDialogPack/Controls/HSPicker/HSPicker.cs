using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prism.CommonDialogPack.Controls
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Prism.CommonDialogPack.Controls"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Prism.CommonDialogPack.Controls;assembly=Prism.CommonDialogPack.Controls"
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
    ///     <MyNamespace:HSPicker/>
    ///
    /// </summary>
    public class HSPicker : Control
    {
        [Description("PointerX")]
        public double PointerX
        {
            get { return (double)GetValue(PointerXProperty); }
            set { SetValue(PointerXProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PointerX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerXProperty =
            DependencyProperty.Register("PointerX", typeof(double), typeof(HSPicker), new PropertyMetadata(0d));

        [Description("PointerY")]
        public double PointerY
        {
            get { return (double)GetValue(PointerYProperty); }
            set { SetValue(PointerYProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PointerY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerYProperty =
            DependencyProperty.Register("PointerY", typeof(double), typeof(HSPicker), new PropertyMetadata(0d));

        [Description("Hue(色相) 0～359"), Category("共通")]
        public float Hue
        {
            get { return (float)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Hue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(float), typeof(HSPicker), new PropertyMetadata(0f, HueValueChanged, CoerceHueValue));

        [Description("Saturation(彩度) 0～1"), Category("共通")]
        public float Saturation
        {
            get { return (float)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Saturation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(float), typeof(HSPicker), new PropertyMetadata(0f, SaturationValueChanged, CoerceSaturationValue));

        [Description("DefaultHue"), Category("共通")]
        public float DefaultHue
        {
            get { return (float)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("DefaultHue", typeof(float), typeof(HSPicker), new PropertyMetadata(0f));

        [Description("DefaultSaturation"), Category("共通")]
        public float DefaultSaturation
        {
            get { return (float)GetValue(DefaultSaturationProperty); }
            set { SetValue(DefaultSaturationProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DefaultSaturation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultSaturationProperty =
            DependencyProperty.Register("DefaultSaturation", typeof(float), typeof(HSPicker), new PropertyMetadata(0f));

        public event EventHandler<HSChangedEventArgs> HSChangedEvent;

        private Grid baseGrid;

        #region DependencyProperty CallBacks
        private static void HueValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HSPicker self = (HSPicker)d;
        }
        private static object CoerceHueValue(DependencyObject d, object baseValue)
        {
            float value = (float)baseValue;
            if (value < 0f)
            {
                return -1 * (value % 360f);
            }
            if (360f <= value)
            {
                return value % 360f;
            }
            return value;
        }
        private static void SaturationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HSPicker self = (HSPicker)d;
        }
        private static object CoerceSaturationValue(DependencyObject d, object baseValue)
        {
            float value = (float)baseValue;
            if (value < 0f)
            {
                return 0f;
            }
            if (1f < value)
            {
                return 1f;
            }
            return value;
        }
        #endregion

        static HSPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HSPicker), new FrameworkPropertyMetadata(typeof(HSPicker)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.baseGrid != null)
            {
                this.baseGrid.Loaded -= OnLoaded;
                this.baseGrid.SizeChanged -= OnSizeChanged;
                this.baseGrid.MouseDown -= this.OnMouseDown;
                this.baseGrid.MouseUp -= this.OnMouseUp;
                this.baseGrid.MouseMove -= this.OnMouseMove;
                this.baseGrid.KeyDown -= this.OnKeyDown;
            }
            
            this.baseGrid = this.GetTemplateChild("PART_BaseGrid") as Grid;

            if (this.baseGrid != null)
            {
                this.baseGrid.Loaded += OnLoaded;
                this.baseGrid.SizeChanged += OnSizeChanged;
                this.baseGrid.MouseDown += this.OnMouseDown;
                this.baseGrid.MouseUp += this.OnMouseUp;
                this.baseGrid.MouseMove += this.OnMouseMove;
                this.baseGrid.KeyDown += OnKeyDown;                
            }

            
        }
        #region Event Handle
        /// <summary>
        /// It is called when "Loaded".
        /// <br/>Move pointer by <see cref="DefaultHue"/> and <see cref="DefaultSaturation"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.MovePointerByHS(this.DefaultHue, this.DefaultSaturation);
        }
        /// <summary>
        /// It is called when "SizeChanged".
        /// <br/>Move the Pointer by the percentage of the changed size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!this.IsLoaded)
            {
                return;
            }
            var widthRatio = e.NewSize.Width / e.PreviousSize.Width;
            var heightRatio = e.NewSize.Height / e.PreviousSize.Height;
            this.MovePointerAndHS(this.PointerX * widthRatio, this.PointerY * heightRatio);
        }
        /// <summary>
        /// It is called when "MouseDown".
        /// <br/>Capture the mouse and move the Pointer to set the Hue and Saturation, then focus on the baseGrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).CaptureMouse();
            var position = e.GetPosition(this);
            this.MovePointerAndHS(position);
            this.baseGrid?.Focus();
        }
        /// <summary>
        /// It is called when "MouseUp".
        /// <br/>Release the captured mouse.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).ReleaseMouseCapture();
        }
        /// <summary>
        /// It is called when "MouseMove".
        /// <br/>If the mouse is captured, move the Pointer and set the Hue and Saturation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.baseGrid.IsMouseCaptured)
            {
                var position = e.GetPosition(this);
                this.MovePointerAndHS(position);
            }
        }
        /// <summary>
        /// It is called whent "KeyDown".
        /// <br/>Pressing the arrow keys moves the Pointer up, down, left, or right to set Hue and Saturation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            double x = this.PointerX;
            double y = this.PointerY;
            if (e.Key == Key.Up)
            {
                y -= this.GetYUnit();
            }
            else if (e.Key == Key.Left)
            {
                x -= this.GetXUnit();
            }
            else if (e.Key == Key.Right)
            {
                x += this.GetXUnit();
            }
            else if (e.Key == Key.Down)
            {
                y += this.GetYUnit();
            }
            else
            {
                return;
            }
            x = x < 0 ? this.baseGrid.ActualWidth : this.baseGrid.ActualWidth < x ? 0 : x;
            y = y < 0 ? this.baseGrid.ActualHeight : this.baseGrid.ActualHeight < y ? 0 : y;
            MovePointerAndHS(x, y);
            e.Handled = true;
        }
        #endregion
        /// <summary>
        /// Move the Pointer to set the Hue and Satiuration.
        /// <br/>At this time, <see cref="HSChangedEvent"/> is fired.
        /// </summary>
        /// <param name="point"></param>
        private void MovePointerAndHS(Point point)
        {
            MovePointerAndHS(point.X, point.Y);
        }
        /// <summary>
        /// Move the Pointer to set the Hue and Satiuration.
        /// <br/>At this time, <see cref="HSChangedEvent"/> is fired.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MovePointerAndHS(double x, double y)
        {
            this.MovePointer(x, y);
            this.Hue = GetCurrentHue();
            this.Saturation = GetCurrentSaturation();
            this.HSChangedEvent?.Invoke(this, new HSChangedEventArgs(this.Hue, this.Saturation));
        }
        /// <summary>
        /// Move the Pointer.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MovePointer(double x, double y)
        {
            this.PointerX = x < 0 ? 0 : this.baseGrid.ActualWidth < x ? this.baseGrid.ActualWidth : x;
            this.PointerY = y < 0 ? 0 : this.baseGrid.ActualHeight < y ? this.baseGrid.ActualHeight : y;
        }
        /// <summary>
        /// Set Hue and Saturation and move the Pointer.
        /// <br/> Execution of this method does not fire <see cref="HSChangedEvent"/>.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        private void MovePointerByHS(float hue, float saturation)
        {
            var x = hue * this.GetXUnit();
            var y = this.baseGrid.ActualHeight - ((saturation * 100) * this.GetYUnit());
            this.MovePointer(x, y);
        }
        /// <summary>
        /// Get Hue at the current Pointer.
        /// </summary>
        /// <returns></returns>
        private float GetCurrentHue()
        {
            return this.GetHue(this.PointerX);
        }
        /// <summary>
        /// Get Saturaion at the current Pointer.
        /// </summary>
        /// <returns></returns>
        private float GetCurrentSaturation()
        {
            return this.GetSaturation(this.PointerY);
        }
        /// <summary>
        /// Get Hue by x value relative to the width of the <see cref="baseGrid"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private float GetHue(double x)
        {
            return (float)(x * (359d / this.baseGrid.ActualWidth));
        }
        /// <summary>
        /// Get Saturation by y value relative to the height of the <see cref="baseGrid"/>.
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private float GetSaturation(double y)
        {
            return 1f - (float)(y * (1d / this.baseGrid.ActualHeight));
        }
        /// <summary>
        /// Get X for a Hue unit.
        /// </summary>
        /// <returns></returns>
        private double GetXUnit()
        {
            return this.baseGrid.ActualWidth / 360d;
        }
        /// <summary>
        /// Get Y for a Saturation unit.
        /// </summary>
        /// <returns></returns>
        private double GetYUnit()
        {
            return this.baseGrid.ActualHeight / 101d;
        }

        /// <summary>
        /// Set Hue and Saturation.
        /// <br/> Execution of this method does not fire <see cref="HSChangedEvent"/>.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        public void SetHS(float hue, float saturation)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.MovePointerByHS(hue, saturation);
        }
    }
}
