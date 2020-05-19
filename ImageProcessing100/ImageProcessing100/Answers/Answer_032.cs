using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageProcessing100.Answers
{
    public static class Answer_032
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var gray = Util.GBRToGray(img);
            var fourier = DFT(gray);
            var arr = GetPowerSpectrum(fourier);
            var spectal = Util.MakeSpectrum(arr);
            var outMat = Mat.Zeros(img.Height, img.Width, MatType.CV_8UC1).ToMat();
            IDFT(outMat, fourier);
            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("answer1", outMat);
            Cv2.ImShow("spectrum", spectal);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        /// <summary>
        /// Discrete Fourier transformation
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static Complex[,] DFT(Mat img)
        {
            var fourier = new Complex[img.Height, img.Width];
            var imgIndexer = img.GetGenericIndexer<byte>();
            for (int l = 0; l < img.Height; l++)
                for (int k = 0; k < img.Width; k++)
                {
                    var val = new Complex(0d, 0d);
                    for (int y = 0; y < img.Height; y++)
                        for (int x = 0; x < img.Width; x++)
                        {
                            var I = (double)imgIndexer[y, x];
                            var theta = -2 * Math.PI * ((double)k * x / img.Width + (double)l * y / img.Height);
                            var temp = new Complex(Math.Cos(theta), Math.Sin(theta)) * I;
                            val += temp;
                        }
                    val /= Math.Sqrt(img.Height * img.Width);
                    fourier[l, k] = val;
                }
            return fourier;
        }

        /// <summary>
        /// Inverse Discrete Fourier transformation
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="fourier_s"></param>
        /// <returns></returns>
        private static void IDFT(Mat outMat, Complex[,] fourier)
        {
            var outIndexer = outMat.GetGenericIndexer<byte>();
            for (int y = 0; y < outMat.Height; y++)
                for (int x = 0; x < outMat.Width; x++)
                {
                    var val = new Complex(0d, 0d);
                    for (int l = 0; l < outMat.Height; l++)
                    {
                        for (int k = 0; k < outMat.Width; k++)
                        {
                            var G = fourier[l, k];
                            var theta = 2 * Math.PI * (k * (double)x / outMat.Width + (double)l * y / outMat.Height);
                            var temp = new Complex(Math.Cos(theta), Math.Sin(theta)) * G;
                            val += temp;
                        }
                    }
                    var g = Complex.Abs(val) / Math.Sqrt(outMat.Height * outMat.Width);
                    outIndexer[y, x] = (byte)g;
                }
        }

        private static double[] GetPowerSpectrum(Complex[,] complices)
        {
            var yLength = complices.GetLength(0);
            var xLength = complices.GetLength(1);
            var array = new double[xLength * yLength];
            var i = 0;
            for (int y = 0; y < yLength; y++)
                for (int x = 0; x < xLength; x++)
                {
                    array[i++] = Complex.Abs(complices[y, x]);
                }

            var divisor = array.Max() * 255;
            foreach (ref var item in array.AsSpan())
            {
                item /= divisor;
            }
            return array;
        }
    }
}
