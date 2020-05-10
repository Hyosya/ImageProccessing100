using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace ImageProcessing100.Answers
{
    public static class Answer_023
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_dark.jpg");

            var output = HistogramEqualization(img, 128, 52);
            var hist = Util.MakeHistogram(output);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.ImShow("Histogram", hist);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat HistogramEqualization(Mat img, byte targetMean, byte targetStdDev)
        {
            var zmax = 255d;
            double S = img.Total() * img.Channels();

            img.GetArray(out Vec3b[] vec3bArray);
            var byteArray = vec3bArray
                .SelectMany(i => i.ToChannelEnumerable());

            var histogram = new int[256];
            foreach (var v in byteArray)
                histogram[v]++;

            var manipulated = new byte[256];
            for (int i = 0; i < histogram.Length; i++)
            {
                var histSum = histogram[0..(i + 1)].Sum();
                var result = zmax / S * histSum;
                manipulated[i] = (byte)result;
            }

            var outMat = Mat.Zeros(img.Rows, img.Cols, MatType.CV_8UC3).ToMat();
            var outIndexer = outMat.GetGenericIndexer<Vec3b>();
            var imgIndexer = img.GetGenericIndexer<Vec3b>();
            for (int y = 0; y < img.Rows; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    var pixel = imgIndexer[y, x];

                    var newPixel = new Vec3b()
                    {
                        Item0 = manipulated[pixel.Item0],
                        Item1 = manipulated[pixel.Item1],
                        Item2 = manipulated[pixel.Item2],
                    };
                    outIndexer[y, x] = newPixel;
                }
            return outMat;
        }

        private static IEnumerable<byte> ToChannelEnumerable(this Vec3b item)
        {
            yield return item.Item0;
            yield return item.Item1;
            yield return item.Item2;
        }
    }
}
