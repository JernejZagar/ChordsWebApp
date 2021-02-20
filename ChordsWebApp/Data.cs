using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChordsWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChordsWebApp
{
    public class ChordsWebAppContext : DbContext
    {
        public ChordsWebAppContext(DbContextOptions<ChordsWebAppContext> options) : base(options)
        {

        }

        public DbSet<Chords> Chords { get; set; }

        public DbSet<Photo> Photo { get; set; }


        public static class AuxiliaryFunctions
        {
            public static string[] ValidImageTypes = new string[] { "image/gif", "image/jpeg", "image/pjpeg", "image/png", "image/bmp" };

            /// <summary>
            /// Given an image data it creates a thumbnail data.
            /// </summary>
            public static byte[] CreateThumbnail(byte[] image, int thumbWidth)
            {
                MemoryStream msImage = new MemoryStream(image);
                System.Drawing.Image fullsizeImage = System.Drawing.Image.FromStream(msImage);

                int thumbHeight = (int)(fullsizeImage.Height * thumbWidth / (double)fullsizeImage.Width);

                var thumbnailBitmap = new Bitmap(thumbWidth, thumbHeight);
                Graphics thumbnailGraph = Graphics.FromImage(thumbnailBitmap);
                thumbnailGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbnailGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbnailGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var imageRectangle = new Rectangle(0, 0, thumbWidth, thumbHeight);
                thumbnailGraph.DrawImage(fullsizeImage, imageRectangle);

                // IF there will be again rotation problems, use this
                //newImage.ExifRotate();

                MemoryStream msThumb = new MemoryStream();
                thumbnailBitmap.Save(msThumb, fullsizeImage.RawFormat);

                fullsizeImage.Dispose();
                thumbnailGraph.Dispose();

                return msThumb.ToArray();
            }
        }
    }
}
