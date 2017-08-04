using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CapturerDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CapturerFactory _capturer = new CapturerFactory();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            if(!_capturer.Start())
                return;
            _capturer.ImageCaptured += bitmap =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Image.Source = BitmapToImageSource(bitmap);
                    bitmap.Dispose();
                }));
            };
        }

        public ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            var hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            var bitmap = _capturer.GetSingleImage();
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "jpg|*.jpg*",
                DefaultExt = ".jpg"
            };
            if (!saveFileDialog.ShowDialog() ?? false)
                return;
            var fileName = saveFileDialog.FileName;
            bitmap.Save(fileName, ImageFormat.Jpeg);
        }
    }
}