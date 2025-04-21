using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace ElectronicShop.Helpers
{
    public static class ImageHelper
    {
        public static void ResizeAndSaveImage(HttpPostedFileBase file, string path, int width, int height)
        {
            using (var img = System.Drawing.Image.FromStream(file.InputStream))
            {
                var resized = new Bitmap(width, height);
                using (var g = Graphics.FromImage(resized))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawImage(img, 0, 0, width, height);
                }

                resized.Save(path, img.RawFormat);
            }
        }
    }

}