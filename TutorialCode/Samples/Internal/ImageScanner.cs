using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace TutorialCode.Internal
{
    class ImageScanner
    {
        public static Mat ScanImageAndReduceC(Mat I, byte[] table)
        {
            int channels = I.Channels();

            int nRows = I.Rows;
            int nCols = I.Cols * channels;

            if (I.IsContinuous())
            {
                nCols *= nRows;
                nRows = 1;
            }

            int i, j;

            unsafe
            {
                byte* p;
                for (i = 0; i < nRows; ++i)
                {
                    p = (byte*)I.Ptr(i).ToPointer();
                    for (j = 0; j < nCols; ++j)
                    {
                        p[j] = table[p[j]];
                    }
                }
            }

            return I;
        }

        /// <summary>
        /// This uses ForEach function, as there is no equivalent of
        /// "iterator" in OpenCVSharp.
        /// 
        /// <para>
        /// The function ForEachAsByte takes a function operation(pv,pp) as an argument.
        /// It then calls the operation() on each element of the matrix, passing in the 
        /// values of pv and pp. 
        /// </para>
        /// <para>
        /// pv - Pointer to the value of the element
        /// pp - Array of indices: pp[0] => Row-index of the element, pp[1] => Col-index of the
        ///      element, and pp[2] => Z-index of the element.
        /// </para>
        /// </summary>
        /// <param name="I"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static Mat ScanImageAndReduceForEach(Mat I, byte[] table)
        { 
            int channels = I.Channels();

            switch (channels) 
            {
                case 1: 
                    unsafe
                    {
                        I.ForEachAsByte((pv, pp) => *pv = table[*pv]);
                    }
                        
                    break;

                case 3:
                    unsafe
                    {
                        I.ForEachAsVec3b((pv, pp) => 
                        {
                            pv->Item0 = table[pv->Item0];
                            pv->Item1 = table[pv->Item1];
                            pv->Item2 = table[pv->Item2];
                        });
                    }

                    break;
            }

            return I;
        }
        //! [scan-iterator]

        //! [scan-random] 
        public static Mat ScanImageAndReduceRandomAccess(Mat I, byte[] table)
        {
            int channels = I.Channels();

            switch (channels)
            {
                case 1:
                        for (int i = 0; i < I.Rows; ++i)
                            for (int j = 0; j < I.Cols; ++j)
                                I.Set(i, j, table[I.At<byte>(i, j)]);
                        break;
                case 3:
                        for (int i = 0; i < I.Rows; ++i)
                            for (int j = 0; j < I.Cols; ++j)
                            {
                                Vec3b v = I.Get<Vec3b>(i, j);
                                v.Item0 = table[v.Item0];
                                v.Item1 = table[v.Item1];
                                v.Item2 = table[v.Item2];

                                I.Set<Vec3b>(i,j, v);
                            }

                        break;
            }

            return I;
        }
    }
}
