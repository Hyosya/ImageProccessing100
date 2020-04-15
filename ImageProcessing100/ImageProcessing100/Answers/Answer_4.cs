using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_4
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");
            var gray = Util.GBRToGray(img);

            int th = GetOHTSUThresshold(gray);

            unsafe
            {
                gray.ForEachAsByte((value, position) =>
                {
                    *value = *value < th ? byte.MinValue : byte.MaxValue;
                });
            }

            //Cv2.ImWrite("out.jpg", img);
            Cv2.ImShow("sample", gray);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static int GetOHTSUThresshold(Mat gray)
        {
            Span<int> histgram = stackalloc int[256];
            unsafe
            {
                var rawBytes = new ReadOnlySpan<byte>(gray.DataPointer, gray.Cols * gray.Rows);
                foreach (var element in rawBytes)
                    histgram[element]++;
            }

            var maxsb = 0d;
            var th = 0;
            double clsZeroCnt, clsZeroSum, clsOneCnt, clsOneSum;
            foreach (var t in Enumerable.Range(1, 255))
            {
                clsZeroCnt = 0d;
                clsZeroSum = 0d;
                clsOneCnt = 0d;
                clsOneSum = 0d;

                foreach (var clsZeroi in Enumerable.Range(1, t))
                {
                    clsZeroCnt += histgram[clsZeroi];
                    clsZeroSum += histgram[clsZeroi] * clsZeroi;
                }

                foreach (var clsOnei in Enumerable.Range(t, 256 - t))
                {
                    clsOneCnt += histgram[clsOnei];
                    clsOneSum += histgram[clsOnei] * clsOnei;
                }
                var clsZeroAvg = clsZeroCnt == 0 ? 0 : clsZeroSum / clsZeroCnt;
                var clsOneAvg = clsOneCnt == 0 ? 0 : clsOneSum / clsOneCnt;
                var sb = clsZeroCnt * clsOneCnt / Math.Pow(clsZeroCnt + clsOneCnt, 2d) * Math.Pow(clsZeroAvg - clsOneAvg, 2d);
                if (sb > maxsb)
                {
                    maxsb = sb;
                    th = t;
                }
            }

            return th;
        }
    }
}
