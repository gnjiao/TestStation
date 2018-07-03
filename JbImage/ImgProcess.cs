using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Utils;

namespace JbImage
{
    public class ImgProcess
    {
        static Logger _logger = new Logger("Image.Preprocess");

        public static int[][] pixelsValue;
        public static string FormatBmp(string path)
        {
            string file = Utils.String.FilePostfix(path, "-24b").Replace("jpg", "bmp");
            string fileplus = Utils.String.FilePostfix(file, "-bin");

            _logger.Info("Start FormatBmp");

            _logger.Info("Convert to BMP");
            using (Bitmap source = new Bitmap(path))
            {
                using (Bitmap bmp = new Bitmap(source.Width, source.Height, PixelFormat.Format24bppRgb))
                {
                    Graphics.FromImage(bmp).DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(file, ImageFormat.Bmp);
                }
            }

            Bitmap i = ImgProcess.Binarize(file);
            i.Save(fileplus);
            _logger.Info("End FormatBmp");

            return fileplus;
        }
        public static Bitmap Binarize(string path)
        {
            _logger.Info("load file " + path);
            Bitmap bmpobj = (Bitmap)Bitmap.FromFile(path);
            _logger.Info("gray " + path);
            ToGrey(bmpobj);
            _logger.Info("threshholding " + path);
            Thresholding(bmpobj);

            return bmpobj;
        }
        static void ToGrey(Bitmap img1)
        {
            pixelsValue = new int[img1.Width][];
            for (int i = 0; i < img1.Width; i++)
            {
                pixelsValue[i] = new int[img1.Height];

                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    int grey = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    pixelsValue[i][j] = grey;
                    Color newColor = Color.FromArgb(grey, grey, grey);
                    img1.SetPixel(i, j, newColor);
                }
            }
        }
        static void Thresholding(Bitmap img1)
        {
            int[] histogram = new int[256];
            int minGrayValue = 255, maxGrayValue = 0;
            //求取直方图
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    histogram[pixelColor.R]++;
                    if (pixelColor.R > maxGrayValue) maxGrayValue = pixelColor.R;
                    if (pixelColor.R < minGrayValue) minGrayValue = pixelColor.R;
                }
            }
            //迭代计算阀值
            int threshold = -1;
            int newThreshold = (minGrayValue + maxGrayValue) / 2;
            for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
            {
                threshold = newThreshold;
                int lP1 = 0;
                int lP2 = 0;
                int lS1 = 0;
                int lS2 = 0;
                //求两个区域的灰度的平均值
                for (int i = minGrayValue; i < threshold; i++)
                {
                    lP1 += histogram[i] * i;
                    lS1 += histogram[i];
                }
                int mean1GrayValue = (lP1 / lS1);
                for (int i = threshold + 1; i < maxGrayValue; i++)
                {
                    lP2 += histogram[i] * i;
                    lS2 += histogram[i];
                }
                int mean2GrayValue = (lP2 / lS2);
                newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
            }
            //计算二值化
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    if (pixelColor.R > threshold) img1.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else img1.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
        }
        public static byte[][] ToArray(Bitmap img)
        {
            Rectangle lockRect = new Rectangle(0, 0, img.Width, img.Height);
            BitmapData imgData = img.LockBits(lockRect, ImageLockMode.ReadOnly, img.PixelFormat);

            byte[,] rband = new byte[img.Height, img.Width];   // 彩色图片的R、G、B三层分别构造一个二维数组
            byte[,] gband = new byte[img.Height, img.Width];
            byte[,] bband = new byte[img.Height, img.Width];
            int rowOffset = imgData.Stride - img.Width * 3;

            // 这里不用img.GetPixel方法，而采用效率更高的指针来获取图像像素点的值
            unsafe
            {
                byte* imgPtr = (byte*)imgData.Scan0.ToPointer();

                for (int i = 0; i < img.Height; ++i)
                {
                    for (int j = 0; j < img.Width; ++j)
                    {
                        rband[i, j] = imgPtr[2];   // 每个像素的指针是按BGR的顺序存储的
                        gband[i, j] = imgPtr[1];
                        bband[i, j] = imgPtr[0];

                        imgPtr += 3;   // 偏移一个像素
                    }
                    imgPtr += rowOffset;   // 偏移到下一行
                }
            }

            img.UnlockBits(imgData);

            byte[][] result = new byte[img.Height][];
            for (int i = 0; i < img.Height; i++)
            {
                result[i] = new byte[img.Width];

                for (int j = 0; j < img.Width; j++)
                {
                    result[i][j] = (byte)((rband[i, j] == 255) ? 1 : 0);
                }
            }
            return result;
        }
        public static void Count(List<Round> rounds)
        {
            _logger.Info("Start Count");

            int sumWeight = 0;
            int sumLenX = 0;
            int sumLenY = 0;
            foreach (var round in rounds)
            {
                int count = 0;

                for (int x = round.MaxLenLine.Start; x <= round.MaxLenLine.End; x++)
                {
                    for (int y = round.StartY; y <= round.EndY; y++)
                    {
                        count += pixelsValue[x][y];
                    }
                }

                round.Weight = count;
                sumWeight += count;
                sumLenX += round.MaxLenLine.Length;
                sumLenY += round.EndY - round.StartY;
            }

            double averageWeight = (double)sumWeight / rounds.Count;
            double averageLenX = (double)sumLenX / rounds.Count;
            double averageLenY = (double)sumLenY / rounds.Count;
            foreach (var round in rounds)
            {
                round.WeightDiff = (double)(round.Weight - averageWeight) / averageWeight;
                round.LenXDiff = (double)(round.MaxLenLine.Length - averageLenX) / averageLenX;
                round.LenYDiff = (double)(round.EndY - round.StartY) / averageLenY;
            }

            _logger.Info("End Count");
        }
    }
}
