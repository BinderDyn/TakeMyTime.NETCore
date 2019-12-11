using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace TakeMyTime.WPF.Utility
{
    public class BitmapConverter
    {
        public static Bitmap ConvertFromBytes(byte[] imageData)
        {
            Bitmap bmp;
            using (var stream = new MemoryStream(imageData))
            {
                bmp = new Bitmap(stream);
            }
            return bmp;
        }
    }
}
