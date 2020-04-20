using OpenCvSharp;
using System;

namespace ImageProcessing100.Answers
{
    public static class Answer_10
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_noise.jpg");

            var output = MedianFiliter(img, 3);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat MedianFiliter(Mat img, int kernelSize)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC3).ToMat();
            var pad = kernelSize / 2;
            var arraySize = kernelSize * kernelSize;
            Span<byte> bArray = stackalloc byte[arraySize];
            Span<byte> gArray = stackalloc byte[arraySize];
            Span<byte> rArray = stackalloc byte[arraySize];
            for (int y = 0; y < img.Height; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    for (int dy = -pad; dy < pad + 1; dy++)
                        for (int dx = -pad; dx < pad + 1; dx++)
                        {
                            var i = dx + pad + ((dy + pad) * kernelSize);
                            if ((x + dx < 0) || (y + dy < 0))
                            {
                                bArray[i] = byte.MinValue;
                                gArray[i] = byte.MinValue;
                                rArray[i] = byte.MinValue;
                                continue;
                            }
                            var pixel = img.GetGenericIndexer<Vec3b>()[y + dy, x + dx];
                            bArray[i] = pixel.Item0;
                            gArray[i] = pixel.Item1;
                            rArray[i] = pixel.Item2;
                        }
                    SpanQuickSort(bArray);
                    SpanQuickSort(gArray);
                    SpanQuickSort(rArray);
                    outMat.GetGenericIndexer<Vec3b>()[y, x] =
                        new Vec3b(bArray[arraySize / 2], gArray[arraySize / 2], rArray[arraySize / 2]);
                }
            return outMat;
        }

        private static void SpanQuickSort(Span<byte> byteSpan)
        {
            if (byteSpan.Length < 2) return;
            var pivot = byteSpan[byteSpan.Length / 2];
            var x = 0;
            var y = byteSpan.Length - 1;
            while (true)
            {
                while (byteSpan[x] < pivot) x++;
                while (byteSpan[y] > pivot) y--;
                if (x >= y) break;
                var temp = byteSpan[x];
                byteSpan[x] = byteSpan[y];
                byteSpan[y] = temp;
                x++; y--;
            }
            SpanQuickSort(byteSpan.Slice(0, x));
            SpanQuickSort(byteSpan[(y + 1)..]);
        }
    }
}
