using Microsoft.VisualBasic;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ImageProcessing100.Answers
{
    public static class Answer_022
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_dark.jpg");

            var output = HistogramManipulation(img, 128, 52);
            var hist = Util.MakeHistogram(output);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.ImShow("Histogram", hist);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat HistogramManipulation(Mat img, byte targetMean, byte targetStdDev)
        {
            img.GetArray(out Vec3b[] vec3bArray);
            var byteArray = vec3bArray.SelectMany(i => i.ToByteEnumerable()).ToArray();
            var sourceMean = byteArray.Sum(i => i) / (double)byteArray.Length;
            var sourceStdDev = Math.Sqrt(byteArray.Sum(i => Math.Pow(i - sourceMean, 2)) / byteArray.Length);

            byte manipulation(byte chValue)
            {
                return (byte)(targetStdDev / sourceStdDev * (chValue - sourceMean) + targetMean);
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
                        Item0 = manipulation(pixel.Item0),
                        Item1 = manipulation(pixel.Item1),
                        Item2 = manipulation(pixel.Item2),
                    };
                    outIndexer[y, x] = newPixel;
                }

            return outMat;
        }

        private static IEnumerable<byte> ToByteEnumerable(this Vec3b item)
        {
            yield return item.Item0;
            yield return item.Item1;
            yield return item.Item2;
        }
    }
}
