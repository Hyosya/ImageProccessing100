using OpenCvSharp;
using OpenCvSharp.Extensions;
using ScottPlot;
using System;
using System.Diagnostics;

namespace ImageProcessing100.Answers
{
    public static class Util
    {
        public static Mat SwapRchAndBch(Mat mat)
        {
            if (mat.Channels() != 3) throw new ArgumentException();
            var newMat = Mat.Zeros(mat.Rows, mat.Cols, MatType.CV_8UC3).ToMat();
            unsafe
            {
                mat.ForEachAsVec3b((value, position) =>
                {
                    var newPixel = (Vec3b*)newMat.Ptr(position[0], position[1]);
                    newPixel->Item0 = value->Item2;
                    newPixel->Item1 = value->Item1;
                    newPixel->Item2 = value->Item0;
                });
            }
            return newMat;
        }

        public static Mat GBRToGray(Mat mat)
        {
            if (mat.Channels() != 3) throw new ArgumentException();
            var newMat = Mat.Zeros(mat.Rows, mat.Cols, MatType.CV_8UC1).ToMat();
            var newIndexer = newMat.GetGenericIndexer<byte>();
            unsafe
            {
                mat.ForEachAsVec3b((value, position) =>
                {
                    newIndexer[position[0], position[1]]
                    = (byte)(0.2126f * value->Item2 + 0.7152f * value->Item1 + 0.0722f * value->Item0);
                });
            }
            return newMat;
        }

        public static T MeasureTicks<T>(Func<T> func)
        {
            var sw = new Stopwatch();
            sw.Start();
            var retValue = func();
            sw.Stop();
            Console.WriteLine(sw.ElapsedTicks);
            return retValue;
        }

        public static Mat MakeHistogram(Mat mat)
        {
            double[] array;
            unsafe
            {
                var span = new ReadOnlySpan<byte>(mat.Data.ToPointer(), (int)mat.Total() * mat.Channels());
                array = new double[span.Length];
                for (int i = 0; i < span.Length; i++)
                {
                    array[i] = span[i];
                }
            }

            var plt = new Plot(640, 480);
            var hist = new ScottPlot.Statistics.Histogram(array, min: 0, max: 255, binSize: 1);
            plt.PlotBar(hist.bins, hist.counts, barWidth: hist.binSize , outlineWidth: 0);
            plt.Axis(null, null, 0, null);
            plt.Grid(lineStyle: LineStyle.Dot);
            var bitmap = plt.GetBitmap();
            return bitmap.ToMat();
        }
    }
}
