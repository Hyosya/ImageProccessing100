using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using OpenCvSharp;

namespace ImageProcessing100.Answers
{
    public static class Answer_3
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");
            var gray = Util.GBRToGray(img);

            unsafe
            {
                gray.ForEachAsByte((value, position) =>
                {
                    *value = *value < 128 ? byte.MinValue : byte.MaxValue;
                });
            }

            //Cv2.ImWrite("out.jpg", img);
            Cv2.ImShow("sample", gray);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
