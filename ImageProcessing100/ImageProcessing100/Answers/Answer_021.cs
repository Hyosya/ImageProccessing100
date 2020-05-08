using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_021
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_dark.jpg");

            var output = HistogramNormalization(img, byte.MinValue, byte.MaxValue);
            var hist = Util.MakeHistogram(output);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.ImShow("Histogram", hist);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat HistogramNormalization(Mat img, byte targetMin, byte targetMax)
        {
            var outMat = Mat.Zeros(img.Rows, img.Cols, MatType.CV_8UC3).ToMat();
            var sourceMin = targetMax;
            var sourceMax = targetMin;


            void minMax(byte t)
            {
                sourceMax = Math.Max(t, sourceMax);
                sourceMin = Math.Min(t, sourceMin);
            }
            var imgIndexer = img.GetGenericIndexer<Vec3b>();
            for (int y = 0; y < img.Rows; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    var pixel = imgIndexer[y, x];
                    minMax(pixel.Item0);
                    minMax(pixel.Item1);
                    minMax(pixel.Item2);
                }

            byte transformation(byte chValue)
            {
                if (chValue < targetMin) return targetMin;
                if (chValue <= targetMax) return (byte)((targetMax - targetMin) / (sourceMax - sourceMin) * (chValue - sourceMin) + targetMin);
                return targetMax;
            }
            var outIndexer = outMat.GetGenericIndexer<Vec3b>();
            for (int y = 0; y < img.Rows; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    var pixel = imgIndexer[y, x];
                    var newPixel = new Vec3b()
                    {
                        Item0 = transformation(pixel.Item0),
                        Item1 = transformation(pixel.Item1),
                        Item2 = transformation(pixel.Item2),
                    };
                    outIndexer[y, x] = newPixel;
                }

            return outMat;
        }
    }
}
