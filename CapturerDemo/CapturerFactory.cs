using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapturerDemo
{
    public class CapturerFactory
    {
        public Action<Bitmap> ImageCaptured;
        private bool _running;
        public void Start()
        {
            Task.Run(() =>
            {
                _running = true;
                while (_running)
                {
                    var image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    var imgGraphics = Graphics.FromImage(image);
                    imgGraphics.CopyFromScreen(0, 0, 0, 0,
                        new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
                    ImageCaptured.Invoke(image);
                    
                }
               
            });

        }

        public void Stop()
        {
            _running = false;
        }

        public Bitmap GetSingleImage()
        {
            var image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            var imgGraphics = Graphics.FromImage(image);
            imgGraphics.CopyFromScreen(0, 0, 0, 0,
                new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            return image;
        }
    }
}
