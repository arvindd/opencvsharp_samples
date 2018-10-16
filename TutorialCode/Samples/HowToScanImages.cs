using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenCvSharp;
using TutorialCode.Internal;

namespace TutorialCode
{
    /// <summary>
    /// How to scan images - we have three ways 
    /// <list type="number">
    /// <item>
    /// <description>Via direct access using native C operator[] (efficient way)</description>
    /// </item>
    /// <item>
    /// <description>Via iterator (safe way)</description>
    /// </item>
    /// <item>
    /// <description>On-the-fly address calculation with reference returning</description>
    /// </item>
    /// </list>
    /// <para>
    /// https://docs.opencv.org/master/db/da5/tutorial_how_to_scan_images.html
    /// </para>
    /// </summary>
    class HowToScanImages : ISample
    {
        static void Help()
        {
            Console.Write(
                @" --------------------------------------------------------------------------
                   This program shows how to scan image objects in OpenCV (cv::Mat). As use case
                   we take an input image and divide the native color palette (255) with the 
                   input. Shows C operator[] method, iterators and at function for on-the-fly item address calculation.
                   Usage:
                   ./ScanImages <imageNameToUse> <divideWith> [G]
                   if you add a G parameter the image is processed in gray scale
                   --------------------------------------------------------------------------
                 "
            );
        }

        public void Run(string[] args, string tutRoot)
        {
            Help();

            // Default arguments
            // Image digits.png is of dimension 2000x1000
            string imageName = $"{tutRoot}data\\digits.png";
            string argDivideWith = "10";
            string imageReadType = "";

            if (args.Length == 2) 
            {
                imageName = args[0];
                argDivideWith = args[1];
                imageReadType = "G";
            }
            else if (args.Length == 3)
            {
                imageName = args[0];
                argDivideWith = args[1];
                imageReadType = args[2];
            }

            Mat I, J;

            try
            {
                if (imageReadType == "G")
                    I = new Mat(imageName, ImreadModes.GrayScale);
                else
                    I = new Mat(imageName, ImreadModes.Color);
            } 
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The image {imageName} could not be found.");
                Cv2.WaitKey();
                return;
            }

            if (I.Empty())
            {
                Console.WriteLine($"The image {imageName} could not be loaded.");
                Cv2.WaitKey();
                return;
            }

            // [divideWith] Create a look-up table based on the divideWith value
            int divideWith = int.Parse(argDivideWith);
            if (divideWith == 0)
            {
                Console.WriteLine("Invalid number entered for dividing.");
                Cv2.WaitKey();
                return;
            }

            byte[] table = new byte[256];
            for (int i = 0; i < 256; ++i)
                table[i] = (byte)(divideWith * (i / divideWith));
            // [divideWith] ================================================

            const int times = 100;
            double t = Cv2.GetTickCount();

            for (int i = 0; i < times; ++i)
            {
                Mat clone_i = I.Clone();
                J = ImageScanner.ScanImageAndReduceC(clone_i, table);
            }

            t = 1000 * (Cv2.GetTickCount() - t) / Cv2.GetTickFrequency();
            t /= times;
            Console.WriteLine($"\nTime of reducing with the C " +
                $"operator [] (averaged for {times} runs): {t} milliseconds.");

            t = Cv2.GetTickCount();

            for (int i = 0; i < times; ++i)
            {
                Mat clone_i = I.Clone();
                J = ImageScanner.ScanImageAndReduceForEach(clone_i, table);
            }

            t = 1000 * (Cv2.GetTickCount() - t) / Cv2.GetTickFrequency();
            t /= times;

            Console.WriteLine($"Time of reducing with foreach " +
                $"(averaged for {times} runs): {t} milliseconds.");

            t = Cv2.GetTickCount();

            for (int i = 0; i < times; ++i)
            {
                Mat clone_i = I.Clone();
                ImageScanner.ScanImageAndReduceRandomAccess(clone_i, table);
            }

            t = 1000 * (Cv2.GetTickCount() - t) / Cv2.GetTickFrequency();
            t /= times;

            Console.WriteLine($"Time of reducing with the on-the-fly address generation - " +
                $"at function (averaged for {times} runs): {t} milliseconds.");

            t = Cv2.GetTickCount();

            for (int i = 0; i < times; ++i)
                //! [table-use]
                J = I.LUT(table);
            //! [table-use]

            t = 1000 * (Cv2.GetTickCount() - t) / Cv2.GetTickFrequency();
            t /= times;

            Console.WriteLine($"Time of reducing with the LUT function " +
                $"(averaged for {times} runs): {t} milliseconds.");

            Cv2.WaitKey();
        }
    }
}
