using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_019
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_noise.jpg");

            var gray = Util.GBRToGray(img);
            var output = LoGFiliter(gray, 5, 1.3);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat LoGFiliter(Mat img, int kernelSize, double stdDev)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC1).ToMat();
            var kernel = new double[kernelSize, kernelSize];

            var pad = kernelSize / 2;
            var kernelSum = 0d;
            for (int y = 0; y < kernelSize; y++)
                for (int x = 0; x < kernelSize; x++)
                {
                    var _y = y - pad;
                    var _x = x - pad;
                    kernel[y, x] = (_x * _x + _y * _y - 2 * stdDev * stdDev) / (2 * Math.PI * Math.Pow(stdDev, 6)) * Math.Exp(-(_x * _x + _y * _y) / (2 * stdDev * stdDev));
                    kernelSum += kernel[y, x];
                }


            for (int y = 0; y < kernelSize; y++)
                for (int x = 0; x < kernelSize; x++)
                    kernel[y, x] /= kernelSum;


            var imgIndexer = img.GetGenericIndexer<byte>();
            var outIndexer = outMat.GetGenericIndexer<byte>();
            for (int y = 0; y < img.Height; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    var v = 0d;
                    for (int dy = -pad; dy < pad + 1; dy++)
                        for (int dx = -pad; dx < pad + 1; dx++)
                        {
                            if ((x + dx < 0) || (y + dy < 0) || (x + dx >= img.Width) || (y + dy >= img.Height)) continue;

                            var pixel = imgIndexer[y + dy, x + dx];
                            v += pixel * kernel[dy + pad, dx + pad];
                        }
                    outIndexer[y, x] = (byte)Math.Max(Math.Min(v, 255d), 0d);
                }


            return outMat;
        }
    }
}
