using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace ImageProcessing100.Answers
{
    public static class Answer_001
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");
            var newImg = Util.SwapRchAndBch(img);
            //Cv2.ImWrite("out.jpg", img);
            Cv2.ImShow("sample", newImg);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
