using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_9
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_noise.jpg");

            var output = GaussianFiliter(img, 3, 1.3);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat GaussianFiliter(Mat img, int kernelSize, double stdDev)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC3).ToMat();
            var kernel = new double[kernelSize, kernelSize];

            var pad = kernelSize / 2;
            var kernelSum = 0d;
            for (int y = 0; y < kernelSize; y++)
                for (int x = 0; x < kernelSize; x++)
                {
                    var _y = y - pad;
                    var _x = x - pad;
                    kernel[y, x] = 1 / (2 * Math.PI * stdDev * stdDev) * Math.Exp(-(_x * _x + _y * _y) / (2 * stdDev * stdDev));
                    kernelSum += kernel[y, x];
                }

            for (int y = 0; y < kernelSize; y++)
                for (int x = 0; x < kernelSize; x++)
                    kernel[y, x] /= kernelSum;

            var a = 0d;
            foreach (var item in kernel)
            {
                a += item;
            }


            double b, g, r;
            for (int y = 0; y < img.Height; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    b = g = r = 0d;
                    for (int dy = -pad; dy < pad + 1; dy++)
                        for (int dx = -pad; dx < pad + 1; dx++)
                        {
                            if ((x + dx < 0) || (y + dy < 0)) continue;

                            var pixel = img.GetGenericIndexer<Vec3b>()[y + dy, x + dx];
                            b += pixel.Item0 * kernel[dy + pad, dx + pad];
                            g += pixel.Item1 * kernel[dy + pad, dx + pad];
                            r += pixel.Item2 * kernel[dy + pad, dx + pad];

                        }
                    outMat.GetGenericIndexer<Vec3b>()[y, x] = new Vec3b((byte)b, (byte)g, (byte)r);
                }


            return outMat;
        }
    }
}
