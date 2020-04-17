using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace ImageProcessing100.Answers
{
    public static class Answer_7
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var sw = new Stopwatch();
            sw.Start();

            var output = UsingSubMat(img);

            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat UsingSubMat(Mat img)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC3).ToMat();
            const int range = 8;
            const int pixelCount = range * range;
            var points = Enumerable.Range(0, img.Rows / range).Select(i => i * range);
            var mainIndexes = points
                .SelectMany(_ => points, (x, y) => (x, y));

            var subIndexes = Enumerable.Range(0, range)
                .SelectMany(_ => Enumerable.Range(0, range), (x, y) => (x, y))
                .ToArray();

            foreach (var (x, y) in mainIndexes)
            {
                var sumBGR = Vector3.Zero;
                var outSubIndexer = img.SubMat(y, y + range, x, x + range).GetGenericIndexer<Vec3b>();
                foreach (var (xi, yi) in subIndexes)
                {
                    var pixel = outSubIndexer[xi, yi];
                    var v = new Vector3(pixel.Item0, pixel.Item1, pixel.Item2);
                    sumBGR += v;
                }
                var mean = sumBGR / pixelCount;
                outSubIndexer = outMat.SubMat(y, y + range, x, x + range).GetGenericIndexer<Vec3b>();
                foreach (var (xi, yi) in subIndexes)
                {
                    var pixel = new Vec3b
                    {
                        Item0 = (byte)mean.X,
                        Item1 = (byte)mean.Y,
                        Item2 = (byte)mean.Z
                    };
                    outSubIndexer[xi, yi] = pixel;
                }
            }
            return outMat;
        }
    }
}
