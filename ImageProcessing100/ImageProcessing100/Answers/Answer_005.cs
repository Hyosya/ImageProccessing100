using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_005
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            unsafe
            {
                img.ForEachAsVec3b(ReverseHue);
            }

            //img = CPPIMPL(img);

            //Cv2.ImWrite("out.jpg", img);
            Cv2.ImShow("sample", img);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat CPPIMPL(Mat img)
        {
            var hsv = BGRToHSV(img);
            Inverse_Hue(hsv);
            var inversed = HSVToBGR(hsv);
            return inversed;
        }

        private static Mat BGRToHSV(Mat img)
        {
            int width = img.Cols;
            int height = img.Rows;

            float r, g, b;
            float h = 0f, s, v;
            float _max, _min;

            var hsv = Mat.Zeros(height, width, MatType.CV_32FC3).ToMat();

            // each y, x
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // BGR -> HSV
                    r = (float)img.At<Vec3b>(y, x)[2] / 255;
                    g = (float)img.At<Vec3b>(y, x)[1] / 255;
                    b = (float)img.At<Vec3b>(y, x)[0] / 255;

                    _max = Math.Max(r, Math.Max(g, b));
                    _min = Math.Min(r, Math.Min(g, b));

                    // get Hue
                    if (_max == _min)
                    {
                        h = 0;
                    }
                    else if (_min == b)
                    {
                        h = 60 * (g - r) / (_max - _min) + 60;
                    }
                    else if (_min == r)
                    {
                        h = 60 * (b - g) / (_max - _min) + 180;
                    }
                    else if (_min == g)
                    {
                        h = 60 * (r - b) / (_max - _min) + 300;
                    }

                    // get Saturation
                    s = _max - _min;

                    // get Value
                    v = _max;

                    var pixel = hsv.At<Vec3f>(y, x);
                    pixel.Item0 = h;
                    pixel.Item1 = s;
                    pixel.Item2 = v;
                    hsv.Set(y, x, pixel);
                }
            }
            return hsv;
        }

        private static void Inverse_Hue(Mat hsv)
        {
            int height = hsv.Rows;
            int width = hsv.Cols;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = hsv.At<Vec3f>(y, x);
                    pixel.Item0 = (pixel.Item0 + 180) % 360;
                    hsv.Set(y, x, pixel);
                }
            }
        }

        private static Mat HSVToBGR(Mat hsv)
        {
            // get height and width
            int width = hsv.Cols;
            int height = hsv.Rows;

            float h, s, v;
            double c, _h, _x;
            double r, g, b;

            // prepare output
            var ou = Mat.Zeros(height, width, MatType.CV_8UC3).ToMat();

            // each y, x
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {

                    h = hsv.At<Vec3f>(y, x)[0];
                    s = hsv.At<Vec3f>(y, x)[1];
                    v = hsv.At<Vec3f>(y, x)[2];

                    c = s;
                    _h = h / 60;
                    _x = c * (1 - Math.Abs((_h % 2) - 1));

                    r = g = b = v - c;

                    if (_h < 1)
                    {
                        r += c;
                        g += _x;
                    }
                    else if (_h < 2)
                    {
                        r += _x;
                        g += c;
                    }
                    else if (_h < 3)
                    {
                        g += c;
                        b += _x;
                    }
                    else if (_h < 4)
                    {
                        g += _x;
                        b += c;
                    }
                    else if (_h < 5)
                    {
                        r += _x;
                        b += c;
                    }
                    else if (_h < 6)
                    {
                        r += c;
                        b += _x;
                    }
                    var pixel = ou.At<Vec3b>(y, x);
                    pixel.Item0 = (byte)(b * 255);
                    pixel.Item1 = (byte)(g * 255);
                    pixel.Item2 = (byte)(r * 255);
                    ou.Set(y, x, pixel);
                }
            }
            return ou;
        }

        private static unsafe void ReverseHue(Vec3b* value, int* position)
        {
            var bgr = (value->Item0 / 255f, value->Item1 / 255f, value->Item2 / 255f);

            var max = Math.Max(bgr.Item1, Math.Max(bgr.Item2, bgr.Item3));
            var min = Math.Min(bgr.Item1, Math.Min(bgr.Item2, bgr.Item3));

            var hsv = (double.NaN, double.NaN, double.NaN);
            hsv.Item2 = max - (double)min;
            hsv.Item3 = max;
            if (max == min)
            {
                hsv.Item1 = 0f;
            }
            else if (min == bgr.Item1)
            {
                hsv.Item1 = 60d * (bgr.Item2 - bgr.Item3) / hsv.Item2 + 60d;
            }
            else if (min == bgr.Item3)
            {
                hsv.Item1 = 60 * (bgr.Item1 - bgr.Item2) / hsv.Item2 + 180;
            }
            else if (min == bgr.Item2)
            {
                hsv.Item1 = 60 * (bgr.Item3 - bgr.Item1) / hsv.Item2 + 300;
            }

            hsv.Item1 = (hsv.Item1 + 180) % 360;

            var Hd = hsv.Item1 / 60d;

            var C = hsv.Item2;
            var X = C * (1 - Math.Abs((Hd % 2) - 1));

            var rgb = (hsv.Item3 - C, hsv.Item3 - C, hsv.Item3 - C);
            if (Hd < 1)
            {
                rgb.Item1 += C;
                rgb.Item2 += X;
            }
            else if (Hd < 2)
            {
                rgb.Item1 += X;
                rgb.Item2 += C;
            }
            else if (Hd < 3)
            {
                rgb.Item2 += C;
                rgb.Item3 += X;
            }
            else if (Hd < 4)
            {
                rgb.Item2 += X;
                rgb.Item3 += C;
            }
            else if (Hd < 5)
            {
                rgb.Item1 += X;
                rgb.Item3 += C;
            }
            else if (Hd < 6)
            {
                rgb.Item1 += C;
                rgb.Item3 += X;
            }

            value->Item0 = (byte)(rgb.Item3 * 255);
            value->Item1 = (byte)(rgb.Item2 * 255);
            value->Item2 = (byte)(rgb.Item1 * 255);
        }
    }
}
