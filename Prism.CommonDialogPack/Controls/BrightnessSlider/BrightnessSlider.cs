using Prism.CommonDialogPack.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Prism.CommonDialogPack.Controls
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Prism.CommonDialogPack.Controls.BrightnessSlider"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Prism.CommonDialogPack.Controls.BrightnessSlider;assembly=Prism.CommonDialogPack.Controls.BrightnessSlider"
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
    ///     <MyNamespace:BrightnessSlider/>
    ///
    /// </summary>
    public class BrightnessSlider : Control
    {
        [Description("PointerX")]
        public double PointerX
        {
            get { return (double)GetValue(PointerXProperty); }
            set { SetValue(PointerXProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PointerX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerXProperty =
            DependencyProperty.Register("PointerX", typeof(double), typeof(BrightnessSlider), new PropertyMetadata(0d));

        [Description("PointerY")]
        public double PointerY
        {
            get { return (double)GetValue(PointerYProperty); }
            set { SetValue(PointerYProperty, value); }
        }
        // Using a DependencyProperty as the backing store for PointerY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerYProperty =
            DependencyProperty.Register("PointerY", typeof(double), typeof(BrightnessSlider), new PropertyMetadata(0d));

        [Description("Hue(色相) 0～359"), Category("共通")]
        public float Hue
        {
            get { return (float)GetValue(HueProperty); }
            set { SetValue(HueProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Hue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HueProperty =
            DependencyProperty.Register("Hue", typeof(float), typeof(BrightnessSlider), new PropertyMetadata(0f, HueValueChanged, CoerceHueValue));

        [Description("Saturation(彩度) 0～1"), Category("共通")]
        public float Saturation
        {
            get { return (float)GetValue(SaturationProperty); }
            set { SetValue(SaturationProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Saturation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaturationProperty =
            DependencyProperty.Register("Saturation", typeof(float), typeof(BrightnessSlider), new PropertyMetadata(0f, SaturationValueChanged, CoerceSaturationValue));

        [Description("Brightness(明度) 0～1"), Category("共通")]
        public float Brightness
        {
            get { return (float)GetValue(BrightnessProperty); }
            set { SetValue(BrightnessProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Brightness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrightnessProperty =
            DependencyProperty.Register("Brightness", typeof(float), typeof(BrightnessSlider), new PropertyMetadata(1f));

        [Description("DefaultBrightness"), Category("共通")]
        public float DefaultBrightness
        {
            get { return (float)GetValue(DefaultBrightnessProperty); }
            set { SetValue(DefaultBrightnessProperty, value); }
        }
        // Using a DependencyProperty as the backing store for DefaultBrightness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DefaultBrightnessProperty =
            DependencyProperty.Register("DefaultBrightness", typeof(float), typeof(BrightnessSlider), new PropertyMetadata(1f));

        [Description("Orientation"), Category("レイアウト")]
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(BrightnessSlider), new PropertyMetadata(Orientation.Vertical, OrientationValueChanged));

        public event EventHandler<BrightnessChangedEventArgs> BrightnessChangedEvent;

        private Grid baseGrid;
        private Grid pointer;
        private LinearGradientBrush brightnessBrush;

        #region DependencyProperty CallBacks
        private static void HueValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BrightnessSlider self = (BrightnessSlider)d;
            self.UpdateBrushByHueAndSaturation((float)e.NewValue, self.Saturation);
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
            BrightnessSlider self = (BrightnessSlider)d;
            self.UpdateBrushByHueAndSaturation(self.Hue, (float)e.NewValue);
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
        private static void OrientationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BrightnessSlider self = (BrightnessSlider)d;
            if(self.brightnessBrush is null)
            {
                return;
            }
            self.UpdateBrushOrientaion();
        }
        #endregion

        static BrightnessSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BrightnessSlider), new FrameworkPropertyMetadata(typeof(BrightnessSlider)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.baseGrid != null)
            {
                this.baseGrid.Loaded -= this.OnLoaded;
                this.baseGrid.SizeChanged -= OnSizeChanged;
                this.baseGrid.MouseDown -= this.OnMouseDown;
                this.baseGrid.MouseUp -= this.OnMouseUp;
                this.baseGrid.MouseMove -= this.OnMouseMove;
                this.baseGrid.KeyDown -= this.OnKeyDown;
            }

            this.baseGrid = this.GetTemplateChild("PART_BaseGrid") as Grid;
            this.pointer = this.GetTemplateChild("PART_Pointer") as Grid;
            this.brightnessBrush = this.GetTemplateChild("PART_BrightnessBrush") as LinearGradientBrush;

            if (this.baseGrid != null)
            {
                this.baseGrid.Loaded += OnLoaded;
                this.baseGrid.SizeChanged += OnSizeChanged;
                this.baseGrid.MouseDown += this.OnMouseDown;
                this.baseGrid.MouseUp += this.OnMouseUp;
                this.baseGrid.MouseMove += this.OnMouseMove;
                this.baseGrid.KeyDown += OnKeyDown;
            }
            if (this.brightnessBrush != null)
            {
                this.UpdateBrushOrientaion();
            }

            this.UpdateBrushByHueAndSaturation(this.Hue, this.Saturation);
        }
        #region Event Handle
        /// <summary>
        /// It is called when "Loaded".
        /// <br/>Move pointer by <see cref="DefaultBrightness"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.UpdatePointer();
            this.MovePointerByBrightness(this.DefaultBrightness);
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
            this.UpdatePointer();
            if (this.Orientation == Orientation.Vertical)
            {
                this.MovePointerWithVertical(this.PointerY * (e.NewSize.Height / e.PreviousSize.Height));
            }
            else
            {
                this.MovePointerWithVertical(this.PointerX * (e.NewSize.Width / e.PreviousSize.Width));
            }
        }
        /// <summary>
        /// It is called when "MouseDown".
        /// <br/>Capture the mouse and move the Pointer to set the Brightness, then focus on the baseGrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((UIElement)sender).CaptureMouse();
            var position = e.GetPosition(this);
            this.MovePointerAndBrightness(position);
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
        /// <br/>If the mouse is captured, move the Pointer and set the Brightness.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.baseGrid.IsMouseCaptured)
            {
                var position = e.GetPosition(this);
                this.MovePointerAndBrightness(position);
            }
        }
        /// <summary>
        /// It is called whent "KeyDown".
        /// <br/>Pressing the arrow keys moves the Pointer up/down or left/right to set Brightness.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (this.Orientation == Orientation.Vertical)
            {
                double y = this.PointerY;
                if (e.Key == Key.Up)
                {
                    y -= this.GetBrightnessUnit();
                }
                else if (e.Key == Key.Down)
                {
                    y += this.GetBrightnessUnit();
                }
                else
                {
                    return;
                }
                this.MovePointerAndBrightnessWithVertical(y);
            }
            else
            {
                double x = this.PointerX;
                if (e.Key == Key.Left)
                {
                    x -= this.GetBrightnessUnit();
                }
                else if (e.Key == Key.Right)
                {
                    x += this.GetBrightnessUnit();
                }
                else
                {
                    return;
                }
                this.MovePointerAndBrightnessWithHorizontal(x);
            }   
        }
        #endregion
        /// <summary>
        /// Move the Pointer to set the Brightness.
        /// <br/>At this time, <see cref="BrightnessChangedEvent"/> is fired.
        /// </summary>
        /// <param name="point"></param>
        private void MovePointerAndBrightness(Point point)
        {
            this.MovePointerAndBrightness(point.X, point.Y);
        }
        /// <summary>
        /// Move the Pointer to set the Brightness.
        /// <br/>At this time, <see cref="BrightnessChangedEvent"/> is fired.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void MovePointerAndBrightness(double x, double y)
        {
            this.MovePointer(x, y);
            this.Brightness = this.GetCurrentBrightness();
            this.BrightnessChangedEvent?.Invoke(this, new BrightnessChangedEventArgs(this.Brightness));
        }
        /// <summary>
        /// Move the Pointer horizontally to set the Brightness.
        /// <br/>At this time, <see cref="BrightnessChangedEvent"/> is fired.
        /// </summary>
        /// <param name="x"></param>
        private void MovePointerAndBrightnessWithHorizontal(double x)
        {
            this.MovePointerWithHorizontal(x);
            this.Brightness = this.GetCurrentBrightness();
            this.BrightnessChangedEvent?.Invoke(this, new BrightnessChangedEventArgs(this.Brightness));
        }
        /// <summary>
        /// Move the Pointer vertically to set the Brightness.
        /// <br/>At this time, <see cref="BrightnessChangedEvent"/> is fired.
        /// </summary>
        /// <param name="y"></param>
        private void MovePointerAndBrightnessWithVertical(double y)
        {
            this.MovePointerWithVertical(y);
            this.Brightness = this.GetCurrentBrightness();
            this.BrightnessChangedEvent?.Invoke(this, new BrightnessChangedEventArgs(this.Brightness));
        }
        /// <summary>
        /// Move the Pointer.
        /// </summary>
        /// <param name="point"></param>
        private void MovePointer(double x, double y)
        {
            if (this.Orientation == Orientation.Vertical)
            {
                this.MovePointerWithVertical(y);
            }
            else
            {
                this.MovePointerWithHorizontal(x);
            }
        }
        /// <summary>
        /// Move the Pointer horizontally
        /// </summary>
        /// <param name="x"></param>
        private void MovePointerWithHorizontal(double x)
        {
            this.PointerX = x < 0 ? 0 : this.baseGrid.ActualWidth < x ? this.baseGrid.ActualWidth : x;
            this.PointerY = this.baseGrid.ActualHeight / 2;
        }
        /// <summary>
        /// Move the Pointer vertically.
        /// </summary>
        /// <param name="y"></param>
        private void MovePointerWithVertical(double y)
        {
            this.PointerX = this.baseGrid.ActualWidth / 2;
            this.PointerY = y < 0 ? 0 : this.baseGrid.ActualHeight < y ? this.baseGrid.ActualHeight : y;
        }
        /// <summary>
        /// Set Brightness and move the Pointer.
        /// <br/> Execution of this method does not fire <see cref="BrightnessChangedEvent"/>.
        /// </summary>
        /// <param name="brightness"></param>
        private void MovePointerByBrightness(float brightness)
        {
            double value = (brightness * 100) * this.GetBrightnessUnit();
            if (this.Orientation == Orientation.Vertical)
            {
                this.MovePointerWithVertical(this.baseGrid.ActualHeight - value);
            }
            else
            {
                this.MovePointerWithHorizontal(this.baseGrid.ActualWidth - value);
            }
        }
        /// <summary>
        /// Get Brightness at the current Pointer.
        /// </summary>
        /// <returns></returns>
        private float GetCurrentBrightness()
        {
            return this.GetBrightness(this.PointerX, this.PointerY);
        }
        /// <summary>
        /// Get Brightness by x/y value relative to the width/height of the <see cref="baseGrid"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private float GetBrightness(double x, double y)
        {
            if (this.Orientation == Orientation.Vertical)
            {
                return 1f - (float)(y * (1d / this.baseGrid.ActualHeight));
            }
            else
            {
                return 1f - (float)(x * (1d / this.baseGrid.ActualWidth));
            }
        }
        /// <summary>
        /// Get X/Y for a Brightness unit.
        /// </summary>
        /// <returns></returns>
        private double GetBrightnessUnit()
        {
            if (this.Orientation == Orientation.Vertical)
            {
                return this.baseGrid.ActualHeight / 101d;
            }
            else
            {
                return this.baseGrid.ActualWidth / 101d;
            }
        }
        /// <summary>
        /// Update Pointer.
        /// </summary>
        private void UpdatePointer()
        {
            double pointerSize = this.Orientation == Orientation.Vertical ? this.baseGrid.ActualWidth : this.baseGrid.ActualHeight;
            this.pointer.Width = pointerSize;
            this.pointer.Height = pointerSize;
            double offset = pointerSize / -2;
            this.pointer.RenderTransform = new TranslateTransform(offset, offset);
        }
        /// <summary>
        /// Update brush by Hue and Saturation.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        private void UpdateBrushByHueAndSaturation(float hue, float saturation)
        {
            HSV startHSV = new HSV(hue, saturation, 1f);
            HSV middleHSV = new HSV(hue, saturation, 0.5f);
            HSV endHSV = new HSV(hue, saturation, 0f);
            GradientStop start = new GradientStop(startHSV.ToMediaColor(), 0);
            GradientStop middle = new GradientStop(middleHSV.ToMediaColor(), 0.5);
            GradientStop end = new GradientStop(endHSV.ToMediaColor(), 1);
            this.brightnessBrush?.GradientStops.Clear();
            this.brightnessBrush?.GradientStops.Add(start);
            this.brightnessBrush?.GradientStops.Add(middle);
            this.brightnessBrush?.GradientStops.Add(end);
        }
        /// <summary>
        /// Update brush orientation.
        /// </summary>
        private void UpdateBrushOrientaion()
        {
            switch (this.Orientation)
            {
                case Orientation.Horizontal:
                    this.brightnessBrush.StartPoint = new Point(0, 0.5);
                    this.brightnessBrush.EndPoint = new Point(1, 0.5);
                    break;
                case Orientation.Vertical:
                    this.brightnessBrush.StartPoint = new Point(0.5, 0);
                    this.brightnessBrush.EndPoint = new Point(0.5, 1);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Set Brightness.
        /// <br/> Execution of this method does not fire <see cref="BrightnessChangedEvent"/>.
        /// </summary>
        /// <param name="brightness"></param>
        public void SetBrightness(float brightness)
        {
            this.Brightness = brightness;
            this.MovePointerByBrightness(brightness);
        }
    }
}
