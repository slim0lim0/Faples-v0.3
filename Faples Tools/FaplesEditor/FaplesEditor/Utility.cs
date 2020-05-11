using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using reNX;

namespace FaplesEditor
{
    enum eResolution
    {
        e480x272,
        e640x360,
        e848x480,
        e854x480,
        e960x540,
        e960x544,
        e1024x576,
        e1024x600,
        e1136x600,
        e1280x720,
        e1334x750,
        e1366x768,
        e1600x900,
        e1776x1000,
        e1920x1080,
        e2048x1152,
        e2560x1440,
        e3200x1800,
        e3840x2160,
    }
    enum eAssetType
    {
        eDefault,
        eCharacter,
        eNormalize
    }

    class Utility
    {
        public const string RESOURCE_FOLDER = "Resources";
        public const string RESOURCE_PATH = RESOURCE_FOLDER + "\\";
        public const string OBJECTS_PATH = RESOURCE_PATH + "Objects";
        public const string TILES_PATH = RESOURCE_PATH + "Tiles";
        public const string SCENERY_PATH = RESOURCE_PATH + "Scenery";
        public const string MAPS_PATH = RESOURCE_PATH + "Maps";
        public const string MUSIC_PATH = RESOURCE_PATH + "Music";
        public const string GLOBAL_PATH = RESOURCE_PATH + "Global";
        public const string CONTROL_PATH = RESOURCE_PATH + "Controls";
        public const string CHARACTER_PATH = RESOURCE_PATH + "Characters";
        public const string UI_PATH = RESOURCE_PATH + "UI";
        public const string FONT_PATH = RESOURCE_PATH + "Fonts";
        public const string EDITOR_FILES_PATH = "Editor Files";
        public const string EXPORT_FILES_PATH = "Export Files";
        public const string PNG_EXT = ".png";

        public const int GRAPHICS_POINT_RECT = 5;
        public const int GRAPHICS_POINT_LINE = 2;

        public static eResolution GameResolution { get; set; } = eResolution.e3840x2160;

        public static double ResolutionPercentageX { get; set; } = 1.0;
        public static double ResolutionPercentageY { get; set; } = 1.0;

        public static int GetGameResolutionX(int iValue)
        {
            return (int)(iValue * ResolutionPercentageX);
        }

        public static int GetGameResolutionY(int iValue)
        {
            return (int)(iValue * ResolutionPercentageY);
        }

        public static void SetGameResolution(string sResolution)
        {
            switch (sResolution)
            {
                case "480x272":
                    GameResolution = eResolution.e480x272;
                    break;
                case "640x360":
                    GameResolution = eResolution.e640x360;
                    break;
                case "848x480":
                    GameResolution = eResolution.e848x480;
                    break;
                case "e854x480":
                    GameResolution = eResolution.e854x480;
                    break;
                case "960x540":
                    GameResolution = eResolution.e960x540;
                    break;
                case "960x544":
                    GameResolution = eResolution.e960x544;
                    break;
                case "1024x576":
                    GameResolution = eResolution.e1024x576;
                    break;
                case "1024x600":
                    GameResolution = eResolution.e1024x600;
                    break;
                case "1136x600":
                    GameResolution = eResolution.e1136x600;
                    break;
                case "1280x720":
                    GameResolution = eResolution.e1280x720;
                    break;
                case "1334x750":
                    GameResolution = eResolution.e1334x750;
                    break;
                case "1366x768":
                    GameResolution = eResolution.e1366x768;
                    break;
                case "1600x900":
                    GameResolution = eResolution.e1600x900;
                    break;
                case "1776x1000":
                    GameResolution = eResolution.e1776x1000;
                    break;
                case "1920x1080":
                    GameResolution = eResolution.e1920x1080;
                    break;
                case "2048x1152":
                    GameResolution = eResolution.e2048x1152;
                    break;
                case "2560x1440":
                    GameResolution = eResolution.e2560x1440;
                    break;
                case "3200x1800":
                    GameResolution = eResolution.e3200x1800;
                    break;
                case "3840x2160":
                    GameResolution = eResolution.e3840x2160;
                    break;
            }

            ResolutionPercentageX = SetResolutionX(GameResolution);
            ResolutionPercentageY = SetResolutionY(GameResolution);
        }

        public static double SetResolutionX(eResolution eRes)
        {
            switch (eRes)
            {
                case eResolution.e480x272:
                    return 480.0 / 3840.0;
                case eResolution.e640x360:
                    return 640.0 / 3840.0;
                case eResolution.e848x480:
                    return 848.0 / 3840.0;
                case eResolution.e854x480:
                    return 854.0 / 3840.0;
                case eResolution.e960x540:
                    return 960.0 / 3840.0;
                case eResolution.e960x544:
                    return 960.0 / 3840.0;
                case eResolution.e1024x576:
                    return 1024.0 / 3840.0;
                case eResolution.e1024x600:
                    return 1024.0 / 3840.0;
                case eResolution.e1136x600:
                    return 1136.0 / 3840.0;
                case eResolution.e1280x720:
                    return 1280.0 / 3840.0;
                case eResolution.e1334x750:
                    return 1334.0 / 3840.0;
                case eResolution.e1366x768:
                    return 1366.0 / 3840.0;
                case eResolution.e1600x900:
                    return 1600.0 / 3840.0;
                case eResolution.e1776x1000:
                    return 1776.0 / 3840.0;
                case eResolution.e1920x1080:
                    return 1920.0 / 3840.0;
                case eResolution.e2048x1152:
                    return 2048.0 / 3840;
                case eResolution.e2560x1440:
                    return 2560.0 / 3840.0;
                case eResolution.e3200x1800:
                    return 3200.0 / 3840.0;
                case eResolution.e3840x2160:
                    return 3840.0 / 3840.0;
                default:
                    return 1;
            }
        }

        public static double SetResolutionY(eResolution eRes)
        {
            switch (eRes)
            {
                case eResolution.e480x272:
                    return 272.0 / 2160.0;
                case eResolution.e640x360:
                    return 360.0 / 2160.0;
                case eResolution.e848x480:
                    return 480.0 / 2160.0;
                case eResolution.e854x480:
                    return 480.0 / 2160.0;
                case eResolution.e960x540:
                    return 540.0 / 2160.0;
                case eResolution.e960x544:
                    return 544.0 / 2160.0;
                case eResolution.e1024x576:
                    return 576.0 / 2160.0;
                case eResolution.e1024x600:
                    return 600.0 / 2160.0;
                case eResolution.e1136x600:
                    return 600.0 / 2160.0;
                case eResolution.e1280x720:
                    return 720.0 / 2160.0;
                case eResolution.e1334x750:
                    return 750.0 / 2160.0;
                case eResolution.e1366x768:
                    return 768.0 / 2160.0;
                case eResolution.e1600x900:
                    return 900.0 / 2160.0;
                case eResolution.e1776x1000:
                    return 1000.0 / 2160.0;
                case eResolution.e1920x1080:
                    return 1080.0 / 2160.0;
                case eResolution.e2048x1152:
                    return 1152.0 / 2160.0;
                case eResolution.e2560x1440:
                    return 1440.0 / 2160.0;
                case eResolution.e3200x1800:
                    return 1800.0 / 2160.0;
                case eResolution.e3840x2160:
                    return 2160.0 / 2160.0;
                default:
                    return 1;
            }
        }

        static public Bitmap GetSprite(Bitmap source, Rectangle section)
        {
            // Clone a portion of the Bitmap object.
            Rectangle cloneRect = new Rectangle(section.X, section.Y, section.Width, section.Height);
            PixelFormat format = source.PixelFormat;

            if (cloneRect.X + cloneRect.Width > source.Width)
                cloneRect.Width = 1;

            if (cloneRect.Y + cloneRect.Height > source.Height)
                cloneRect.Height = 1;

            Bitmap cloneBitmap = source.Clone(cloneRect, format);

            return cloneBitmap;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap UploadByGameResolution(Image image, double percent)
        {
            int iWidth = (int)(image.Width * percent);
            int iHeight = (int)(image.Height * percent);

            return ResizeImage(image, iWidth, iHeight);
        }

        public static bool XmlNodeExists(XmlNode oNode, string sName)
        {
            foreach(XmlNode oChild in oNode.ChildNodes)
            {
                if (oChild.Attributes["Name"].Value.Equals(sName))
                    return false;
            }

            return true;
        }

        // Font 
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbfont,
            IntPtr pdv, [In] ref uint pcFonts);

        private static PrivateFontCollection _privateFontCollection = new PrivateFontCollection();

        public static List<string> GetFontCollection()
        {
            List<string> colFonts = new List<string>();
           
            foreach(var font in _privateFontCollection.Families)
            {
                colFonts.Add(font.Name);
            }

            return colFonts;
        }

        public static void LoadAllFonts()
        {
            string[] fonts = Directory.GetFiles(FONT_PATH, "*.ttf");
            foreach (string font in fonts)
            {
                AddFont(font);
            }
        }

        public static FontFamily GetFontFamilyByName(string name)
        {
            if(name != "")
                return _privateFontCollection.Families.FirstOrDefault(x => x.Name == name);

            return _privateFontCollection.Families.FirstOrDefault(x => x.Name == "Lato");
        }

        public static void AddFont(string fullFileName)
        {
            AddFont(File.ReadAllBytes(fullFileName));
        }

        public static void AddFont(byte[] fontBytes)
        {
            int dataLength = fontBytes.Length;
            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);

            Marshal.Copy(fontBytes, 0, ptrData, dataLength);

            uint cFonts = 0;

            AddFontMemResourceEx(ptrData, (uint) dataLength, IntPtr.Zero, ref cFonts);

            try
            {
                _privateFontCollection.AddMemoryFont(ptrData, dataLength);
            }
            finally
            {
                Marshal.FreeCoTaskMem(ptrData);
            }
        }

        public static Bitmap RotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image
            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                //rotate
                g.RotateTransform(angle);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);

                g.DrawImage(b, 0, 0);
            }
            return returnBitmap;
        }

        public static Bitmap RotateBitmap(Bitmap bitmap, float angle)
        {
            int w, h, x, y;
            var dW = (double)bitmap.Width;
            var dH = (double)bitmap.Height;

            double degrees = Math.Abs(angle - 90);
            if (degrees <= 90)
            {
                double radians = 0.0174532925 * degrees;
                double dSin = Math.Sin(radians);
                double dCos = Math.Cos(radians);
                w = (int)(dH * dSin + dW * dCos);
                h = (int)(dW * dSin + dH * dCos);
            }
            else if(degrees > 180)
            {
                degrees -= 180;
                double radians = 0.0174532925 * degrees;
                double dSin = Math.Sin(radians);
                double dCos = Math.Cos(radians);
                w = (int)(dH * dSin + dW * dCos);
                h = (int)(dW * dSin + dH * dCos);
            }
            else 
            {
                degrees -= 90;
                double radians = 0.0174532925 * degrees;
                double dSin = Math.Sin(radians);
                double dCos = Math.Cos(radians);
                w = (int)(dW * dSin + dH * dCos);
                h = (int)(dH * dSin + dW * dCos);
            }

            x = (w - bitmap.Width) / 2;
            y = (h - bitmap.Height) / 2;

            var rotateAtX = bitmap.Width / 2f;
            var rotateAtY = bitmap.Height / 2f;

            var bmpRet = new Bitmap(Math.Abs(w), Math.Abs(h));
           
            bmpRet.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using (var graphics = Graphics.FromImage(bmpRet))
            {
                graphics.Clear(Color.White);
                graphics.TranslateTransform(rotateAtX + x, rotateAtY + y);
                graphics.RotateTransform(angle - 90);
                graphics.TranslateTransform(-rotateAtX - x, -rotateAtY - y);
                // bitmap.MakeTransparent(Color.White);
                graphics.DrawImage(bitmap, new PointF(0 + x, 0 + y));
            }
            return bmpRet;
        }

        public static fpxCharacterFeature RotateFeature(fpxCharacterFeature oFeature, Bitmap bitmap, float angle, string sType)
        {
            int w = 0;
            int h = 0;
            int x = 0;
            int y = 0;
            var dW = (double)bitmap.Width;
            var dH = (double)bitmap.Height;

            double degrees = Math.Abs(angle - 90);

            if(sType == "Limb")
            {
                if (degrees <= 90)
                {
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else if (degrees > 180)
                {
                    degrees -= 180;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else
                {
                    degrees -= 90;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dW * dSin + dH * dCos);
                    h = (int)(dH * dSin + dW * dCos);
                }

                x = (w - bitmap.Width) / 2;
                y = (h - bitmap.Height) / 2;

                var rotateAtX = bitmap.Width / 2f;
                var rotateAtY = bitmap.Height / 2f;

                var bmpRet = new Bitmap(Math.Abs(w), Math.Abs(h));

                bmpRet.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

                using (var graphics = Graphics.FromImage(bmpRet))
                {
                    graphics.TranslateTransform(rotateAtX + x, rotateAtY + y);
                    graphics.RotateTransform(angle - 90);
                    graphics.TranslateTransform(-rotateAtX - x, -rotateAtY - y);
                    graphics.DrawImage(bitmap, new PointF(0 + x, 0 + y));
                }

                if (oFeature.SpriteBitmap != null)
                {
                    oFeature.SpriteBitmap.Dispose();
                    oFeature.SpriteBitmap = null;
                }

                oFeature.SpriteBitmap = bmpRet;
            }
            else if(sType == "Head")
            {
                if (degrees <= 90)
                {
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else if (degrees > 180)
                {
                    degrees -= 180;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else
                {
                    degrees -= 90;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dW * dSin + dH * dCos);
                    h = (int)(dH * dSin + dW * dCos);
                }

                x = (w - bitmap.Width) / 2;
                y = (h - bitmap.Height) / 2;

                var rotateAtX = bitmap.Width / 2f;
                var rotateAtY = bitmap.Height / 2f;

                var bmpRet = new Bitmap(bitmap.Width, bitmap.Height);

                bmpRet.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

                using (var graphics = Graphics.FromImage(bmpRet))
                {
                    graphics.TranslateTransform(rotateAtX, rotateAtY);
                    graphics.RotateTransform(angle - 90);
                    graphics.TranslateTransform(-rotateAtX, -rotateAtY);
                    graphics.DrawImage(bitmap, new PointF(0, 0));
                }

                if (oFeature.SpriteBitmap != null)
                {
                    oFeature.SpriteBitmap.Dispose();
                    oFeature.SpriteBitmap = null;
                }

                oFeature.SpriteBitmap = bmpRet;
            }        

            return oFeature;
        }

        public static fpxSkeletonPart RotatePart( fpxSkeletonPart oPart, Bitmap bitmap, float angle)
        {
            int w = 0;
            int h = 0;
            int x = 0;
            int y = 0;
            var dW = (double)bitmap.Width;
            var dH = (double)bitmap.Height;

            double degrees = Math.Abs(angle - 90);

            if(oPart.Type == "Limb")
            {
                if (degrees <= 90)
                {
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else if (degrees > 180)
                {
                    degrees -= 180;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else
                {
                    degrees -= 90;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dW * dSin + dH * dCos);
                    h = (int)(dH * dSin + dW * dCos);
                }

                x = (w - bitmap.Width) / 2;
                y = (h - bitmap.Height) / 2;

                var rotateAtX = bitmap.Width / 2f;
                var rotateAtY = bitmap.Height / 2f;

                var bmpRet = new Bitmap(Math.Abs(w), Math.Abs(h));

                bmpRet.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

                using (var graphics = Graphics.FromImage(bmpRet))
                {
                    graphics.TranslateTransform(rotateAtX + x, rotateAtY + y);
                    graphics.RotateTransform(angle - 90);
                    graphics.TranslateTransform(-rotateAtX - x, -rotateAtY - y);
                    graphics.DrawImage(bitmap, new PointF(0 + x, 0 + y));
                }

                if (oPart.SpriteBitmap != null)
                {
                    oPart.SpriteBitmap.Dispose();
                    oPart.SpriteBitmap = null;
                }

                oPart.SpriteBitmap = bmpRet;
            }
            else if(oPart.Type == "Head")
            {
                if (degrees <= 90)
                {
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else if (degrees > 180)
                {
                    degrees -= 180;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dH * dSin + dW * dCos);
                    h = (int)(dW * dSin + dH * dCos);
                }
                else
                {
                    degrees -= 90;
                    double radians = 0.0174532925 * degrees;
                    double dSin = Math.Sin(radians);
                    double dCos = Math.Cos(radians);
                    w = (int)(dW * dSin + dH * dCos);
                    h = (int)(dH * dSin + dW * dCos);
                }

                x = (w - bitmap.Width) / 2;
                y = (h - bitmap.Height) / 2;

                var rotateAtX = bitmap.Width / 2f;
                var rotateAtY = bitmap.Height / 2f;

                var bmpRet = new Bitmap(bitmap.Width, bitmap.Height);

                bmpRet.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

                using (var graphics = Graphics.FromImage(bmpRet))
                {
                    graphics.TranslateTransform(rotateAtX, rotateAtY);
                    graphics.RotateTransform(angle - 90);
                    graphics.TranslateTransform(-rotateAtX, -rotateAtY);
                    graphics.DrawImage(bitmap, new PointF(0, 0));
                }

                if (oPart.SpriteBitmap != null)
                {
                    oPart.SpriteBitmap.Dispose();
                    oPart.SpriteBitmap = null;
                }

                oPart.SpriteBitmap = bmpRet;
            }

            return oPart;
        }
    }
}
