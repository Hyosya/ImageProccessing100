using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_8
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var output = UsingSubMat(img);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat UsingSubMat(Mat img)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC3).ToMat();
            const int range = 8;
            var maxBGR = (byte.MinValue, byte.MinValue, byte.MinValue);
            var points = Enumerable.Range(0, img.Rows / range).Select(i => i * range);
            var mainIndexes = points
                .SelectMany(_ => points, (x, y) => (x, y));

            var subIndexes = Enumerable.Range(0, range)
                .SelectMany(_ => Enumerable.Range(0, range), (x, y) => (x, y))
                .ToArray();

            foreach (var (x, y) in mainIndexes)
            {
                maxBGR = (0, 0, 0);
                var outSubIndexer = img.SubMat(y, y + range, x, x + range).GetGenericIndexer<Vec3b>();
                foreach (var (xi, yi) in subIndexes)
                {
                    var pixel = outSubIndexer[xi, yi];
                    maxBGR.Item1 = Math.Max(maxBGR.Item1, pixel.Item0);
                    maxBGR.Item2 = Math.Max(maxBGR.Item2, pixel.Item1);
                    maxBGR.Item3 = Math.Max(maxBGR.Item3, pixel.Item2);
                }
                outSubIndexer = outMat.SubMat(y, y + range, x, x + range).GetGenericIndexer<Vec3b>();
                foreach (var (xi, yi) in subIndexes)
                {
                    var pixel = new Vec3b()
                    {
                        Item0 = maxBGR.Item1,
                        Item1 = maxBGR.Item2,
                        Item2 = maxBGR.Item3,
                    };
                    outSubIndexer[xi, yi] = pixel;
                }
            }
            return outMat;
        }
    }
}
