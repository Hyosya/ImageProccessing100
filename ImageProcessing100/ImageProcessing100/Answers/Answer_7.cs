using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_7
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var output = UsingSubMat(img);

            Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat UsingSubMat(Mat img)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC3).ToMat();
            const int range = 8;
            const int pixelCount = range * range;
            var sumBGR = (0d, 0d, 0d);
            var meanBGR = (0d, 0d, 0d);
            var points = Enumerable.Range(0, img.Rows / range).Select(i => i * range);
            var mainIndexes = points
                .SelectMany(_ => points, (x, y) => (x, y))
                .ToArray();
            var subIndexes = Enumerable.Range(0, range)
                .SelectMany(_ => Enumerable.Range(0, range), (x, y) => (x, y))
                .ToArray();

            foreach (var (x, y) in mainIndexes)
            {
                sumBGR = (0d, 0d, 0d);
                var outSubIndexer = img.SubMat(y, y + range, x, x + range).GetGenericIndexer<Vec3b>();
                foreach (var (xi, yi) in subIndexes)
                {
                    sumBGR.Item1 += outSubIndexer[xi, yi].Item0;
                    sumBGR.Item2 += outSubIndexer[xi, yi].Item1;
                    sumBGR.Item3 += outSubIndexer[xi, yi].Item2;
                }
                meanBGR = (sumBGR.Item1 / pixelCount, sumBGR.Item2 / pixelCount, sumBGR.Item3 / pixelCount);
                outSubIndexer = outMat.SubMat(y, y + range, x, x + range).GetGenericIndexer<Vec3b>();
                foreach (var (xi, yi) in subIndexes)
                {
                    var pixel = outSubIndexer[xi, yi];
                    pixel.Item0 = (byte)meanBGR.Item1;
                    pixel.Item1 += (byte)meanBGR.Item2;
                    pixel.Item2 += (byte)meanBGR.Item3;
                    outSubIndexer[xi, yi] = pixel;
                }
            }
            return outMat;
        }
    }
}
