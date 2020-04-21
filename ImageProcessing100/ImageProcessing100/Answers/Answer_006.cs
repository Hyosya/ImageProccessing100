using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_006
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            unsafe
            {
                img.ForEachAsVec3b(DecreaseColor);
            }

            //Cv2.ImWrite("out.jpg", img);
            Cv2.ImShow("sample", img);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static unsafe void DecreaseColor(Vec3b* value, int* position)
        {
            static byte decrease(in byte v)
            {
                if (v < 64)
                {
                    return 32;
                }
                else if (v < 128)
                {
                    return 96;
                }
                else if (v < 192)
                {
                    return 160;
                }
                else
                {
                    return 224;
                }
            }
            value->Item0 = decrease(value->Item0);
            value->Item1 = decrease(value->Item1);
            value->Item2 = decrease(value->Item2);
        }
    }
}
