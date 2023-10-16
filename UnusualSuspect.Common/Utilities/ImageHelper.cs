using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;

namespace UnusualSuspect.Common.Utilities;

public static class ImageHelper
{
    public enum Dimensions
    {
        Width,
        Height
    }
    public enum AnchorPosition
    {
        Top,
        Center,
        Bottom,
        Left,
        Right
    }

    public static Image ScaleByPercent(Image imgPhoto, int Percent)
    {
        float nPercent = ((float)Percent / 100);

        int sourceWidth = imgPhoto.Width;
        int sourceHeight = imgPhoto.Height;
        int sourceX = 0;
        int sourceY = 0;

        int destX = 0;
        int destY = 0;
        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

        grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

        grPhoto.Dispose();
        return bmPhoto;
    }


    public static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
    {
        int sourceWidth = imgPhoto.Width;
        int sourceHeight = imgPhoto.Height;
        int sourceX = 0;
        int sourceY = 0;
        int destX = 0;
        int destY = 0;
        float nPercent = 0;

        switch (Dimension)
        {
            case Dimensions.Width:
                nPercent = ((float)Size / (float)sourceWidth);
                break;
            default:
                nPercent = ((float)Size / (float)sourceHeight);
                break;
        }

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

        grPhoto.DrawImage(imgPhoto,
        new Rectangle(destX, destY, destWidth, destHeight),
        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
        GraphicsUnit.Pixel);

        grPhoto.Dispose();
        return bmPhoto;
    }

    public static string GetContentType(string documentType)
    {
        documentType = documentType.Replace(".", "");
        documentType = documentType.ToLower();
        switch (documentType)
        {
            case "jpg": return "image/jpeg";
            case "jpeg": return "image/jpeg";
            case "gif": return "image/gif";
            case "png": return "image/png";
            default: return "";
        }
    }

    public static Stream GettImageFromAddress(string url)
    {
        System.Drawing.Image img = System.Drawing.Image.FromFile(url);
        return WriteImage(img, "", ImageFormat.Jpeg);
    }


    public static Stream WriteThumbnailImage(byte[] obj, string defaultImageUrl, string size)
    {
        if (obj == null || ((byte[])obj).Length == 0)
        {
            if (string.IsNullOrEmpty(defaultImageUrl))
            {
                //Response.Write("تصويري ارسال نشده است.");
                return null;
            }
            else
                return ImageHelper.GettImageFromAddress(defaultImageUrl);
        }
        else
            return WriteImage((byte[])obj, size/*, defaultImageUrl*/);
    }


    public static Stream WriteImage(byte[] imageBytes, string size/*, string defaultImageUrl*/)
    {
        System.Drawing.Image img = null;
        MemoryStream ms = new MemoryStream(imageBytes);
        ms.Position = 0;
        img = System.Drawing.Image.FromStream(ms);//در نمايش تصاوير از نوع Image در db خطا مي دهد.
        return WriteImage(img, size, ImageFormat.Jpeg);
    }


    public static Stream WriteImage(System.Drawing.Image img, string size, ImageFormat imageType)
    {


        Stream output = new MemoryStream();

        if (string.IsNullOrEmpty(size))
            img.Save(output, imageType);
        else
        {
            int width;
            int height;
            if (img.Height > img.Width)
            {
                height = size.ToInt();
                width = img.Width * size.ToInt() / img.Height;
            }
            else //if (img.height <= img.width)
            {
                width = size.ToInt();
                height = img.Height * size.ToInt() / img.Width;
            }
            if (imageType == ImageFormat.Png || imageType == ImageFormat.Gif)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                    img = img.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
                    img.Save(stream, imageType);
                    stream.WriteTo(output);
                }
            }
            else
            {
                ImageHelper.ConstrainProportions(img, height, Dimensions.Height).Save(output, imageType);
            }

        }
        return output;
    }


    public static bool ThumbnailCallback()
    {
        return false;
    }

    public static Image FixedSize(Image imgPhoto, int Width, int Height)
    {
        int sourceWidth = imgPhoto.Width;
        int sourceHeight = imgPhoto.Height;
        int sourceX = 0;
        int sourceY = 0;
        int destX = 0;
        int destY = 0;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)Width / (float)sourceWidth);
        nPercentH = ((float)Height / (float)sourceHeight);

        //if we have to pad the height pad both the top and the bottom
        //with the difference between the scaled height and the desired height
        if (nPercentH < nPercentW)
        {
            nPercent = nPercentH;
            destX = (int)((Width - (sourceWidth * nPercent)) / 2);
        }
        else
        {
            nPercent = nPercentW;
            destY = (int)((Height - (sourceHeight * nPercent)) / 2);
        }

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.Clear(Color.Red);
        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

        grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

        grPhoto.Dispose();
        return bmPhoto;
    }
    public static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
    {
        int sourceWidth = imgPhoto.Width;
        int sourceHeight = imgPhoto.Height;
        int sourceX = 0;
        int sourceY = 0;
        int destX = 0;
        int destY = 0;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)Width / (float)sourceWidth);
        nPercentH = ((float)Height / (float)sourceHeight);

        if (nPercentH < nPercentW)
        {
            nPercent = nPercentW;
            switch (Anchor)
            {
                case AnchorPosition.Top:
                    destY = 0;
                    break;
                case AnchorPosition.Bottom:
                    destY = (int)(Height - (sourceHeight * nPercent));
                    break;
                default:
                    destY = (int)((Height - (sourceHeight * nPercent)) / 2);
                    break;
            }
        }
        else
        {
            nPercent = nPercentH;
            switch (Anchor)
            {
                case AnchorPosition.Left:
                    destX = 0;
                    break;
                case AnchorPosition.Right:
                    destX = (int)(Width - (sourceWidth * nPercent));
                    break;
                default:
                    destX = (int)((Width - (sourceWidth * nPercent)) / 2);
                    break;
            }
        }

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

        grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

        grPhoto.Dispose();
        return bmPhoto;
    }
}