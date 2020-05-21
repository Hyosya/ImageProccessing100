using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_034
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var gray = Util.GBRToGray(img);
            var fourier = DFT(gray);
            var outMat = Mat.Zeros(img.Height, img.Width, MatType.CV_8UC1).ToMat();

            HighPassFilter(fourier, 0.1d);
            IDFT(outMat, fourier);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("answer1", outMat);
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


        private static void HighPassFilter(Complex[,] fourier_s, double pass_r)
        {
            var height = fourier_s.GetLength(0) - 1;
            var width = fourier_s.GetLength(1) - 1;
            var r = height / 2;
            var filter_d = (int)(r * pass_r);
            for (int j = 0; j < height / 2; j++)
                for (int i = 0; i < width / 2; i++)
                {
                    if (Math.Sqrt(i * i + j * j) > filter_d) continue;
                    fourier_s[j, i] = 0;
                    fourier_s[j, width - i] = 0;
                    fourier_s[height - i, i] = 0;
                    fourier_s[height - i, width - i] = 0;
                }
        }
    }
}
