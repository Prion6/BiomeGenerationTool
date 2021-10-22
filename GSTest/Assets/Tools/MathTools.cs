using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathTools
{
    public static float Q_sqrt(float number)
    {
        long i;
        float x2, y;

        x2 = number * 0.5f;
        y = number;
        unsafe
        {
            i = *(long*)&y;
            i = 0x1fbd1df5 + (i >> 1);
            y = *(float*)&i;
            y = y * 0.5f + (x2 / y);
        }
        return y;
    }

    public static float Q_Isqrt(float number)
    {
        long i;
        float x2, y;

        x2 = number * 0.5f;
        y = number;

        unsafe
        {
            i = *(long*)&y;
            i = 0x5f3759df + (i >> 1);
            y = *(float*)&i;
            y = y * (1.5f - x2*y*y);
        }
        return y;
    }

    public static float Distance(float x1, float y1, float x2, float y2)
    {
        float n = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        return Q_sqrt(n);
    }

    public static T[] Resize<T>(T[] pixels, int srcWidth, int srcHeight, int dstWidth, int dstheight)
    {
        T[] temp = new T[dstWidth * dstheight];
        // EDIT: added +1 to account for an early rounding problem
        int x_ratio = (int)((srcWidth << 16) / dstWidth) + 1;
        int y_ratio = (int)((srcHeight << 16) / dstheight) + 1;
        //int x_ratio = (int)((w1<<16)/w2) ;
        //int y_ratio = (int)((h1<<16)/h2) ;
        int x2, y2;
        for (int i = 0; i < dstheight; i++)
        {
            for (int j = 0; j < dstWidth; j++)
            {
                x2 = ((j * x_ratio) >> 16);
                y2 = ((i * y_ratio) >> 16);
                temp[(i * dstWidth) + j] = pixels[(y2 * srcWidth) + x2];
            }
        }
        return temp;
    }
}
