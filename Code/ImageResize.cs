using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace IJPReporting.Code
{
    public class ImageResize
    {
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            newImage.SetResolution(72, 72);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }


        public static Image SetResolution(Image image, int res)
        {
            var newImage = new Bitmap(image);
            newImage.SetResolution(res, res);

            //     Graphics.FromImage(newImage);
            return newImage;
        }

        //garde le ratio du X pour adapter le y
        public static Image MinScaleImage(Image image, int minWidth, int MinHeight)
        {
            var ratioX = (double)minWidth / image.Width;

            var newWidth = (int)(image.Width * ratioX);
            var newHeight = (int)(image.Height * ratioX);
            if (newHeight < MinHeight)
            {
                var ratioY = (double)MinHeight / image.Height;

                newWidth = (int)(image.Width * ratioY);
                newHeight = (int)(image.Height * ratioY);
            }
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;

        }

        public static HtmlImage ScaleImage(HtmlImage image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            // var newImage = new Bitmap(newWidth, newHeight);
            HtmlImage newImg = new HtmlImage();
            newImg.Src = image.Src;
            newImg.Width = newWidth;
            newImg.Height = newHeight;

            return newImg;
        }

        public static byte[] ScaleImage(byte[] image, int maxWidth, int maxHeight)
        {
            byte[] ReturnedThumbnail;
            using (MemoryStream StartMemoryStream = new MemoryStream(),
                               NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(image, 0, image.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                var ratioX = (double)0;
                if ((double)maxWidth > startBitmap.Width)
                {
                    ratioX = 1;
                }
                else
                {
                    ratioX = (double)maxWidth / startBitmap.Width;
                }
                var ratioY = (double)0;
                if ((double)maxHeight > startBitmap.Height)
                {
                    ratioY = 1;
                }
                else
                {
                    ratioY = (double)maxHeight / startBitmap.Height;
                }
                var ratio = Math.Min(ratioX, ratioY);
                var newWidth = (int)(startBitmap.Width * ratio);
                var newHeight = (int)(startBitmap.Height * ratio);

                Bitmap newBitmap = new Bitmap(newWidth, newHeight);
                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            return ReturnedThumbnail;
        }

        // (RESIZE an image in a byte[] variable.)  
        public static byte[] MaxWidth(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                else
                {
                    newWidth = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                }

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }

        public static byte[] MaxHeight(byte[] PassedImage, int LargestSide)
        {
            byte[] ReturnedThumbnail;

            using (MemoryStream StartMemoryStream = new MemoryStream(),
                                NewMemoryStream = new MemoryStream())
            {
                // write the string to the stream  
                StartMemoryStream.Write(PassedImage, 0, PassedImage.Length);

                // create the start Bitmap from the MemoryStream that contains the image  
                Bitmap startBitmap = new Bitmap(StartMemoryStream);

                // set thumbnail height and width proportional to the original image.  
                int newHeight;
                int newWidth;
                double HW_ratio;
                //    if (startBitmap.Height > startBitmap.Width)
                {
                    newHeight = LargestSide;
                    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Height);
                    newWidth = (int)(HW_ratio * (double)startBitmap.Width);
                }
                //else
                //{
                //    newWidth = LargestSide;
                //    HW_ratio = (double)((double)LargestSide / (double)startBitmap.Width);
                //    newHeight = (int)(HW_ratio * (double)startBitmap.Height);
                //}

                // create a new Bitmap with dimensions for the thumbnail.  
                Bitmap newBitmap = new Bitmap(newWidth, newHeight);

                // Copy the image from the START Bitmap into the NEW Bitmap.  
                // This will create a thumnail size of the same image.  
                newBitmap = ResizeImage(startBitmap, newWidth, newHeight);

                // Save this image to the specified stream in the specified format.  
                newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Fill the byte[] for the thumbnail from the new MemoryStream.  
                ReturnedThumbnail = NewMemoryStream.ToArray();
            }

            // return the resized image as a string of bytes.  
            return ReturnedThumbnail;
        }


        // Resize a Bitmap  
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.DrawImage(image, new Rectangle(0, 0, width, height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }
            return resizedImage;
        }

        //take a part of photo
        public static Bitmap PartPhoto(Bitmap image, int width, int height)
        {
            Rectangle cloneRect = new Rectangle(0, 0, width, height);
            System.Drawing.Imaging.PixelFormat format = image.PixelFormat;
            Bitmap cloneBitmap = image.Clone(cloneRect, format);
            return cloneBitmap;
        }

        public static System.Drawing.Image LoadImageNoLock(string path)
        {
            var ms = new MemoryStream(File.ReadAllBytes(path)); // Don't use using!!
            return System.Drawing.Image.FromStream(ms);
        }


        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

    }
}