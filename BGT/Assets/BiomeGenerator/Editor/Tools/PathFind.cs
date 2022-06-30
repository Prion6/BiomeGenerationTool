using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFind
{
    public static int[] Astar(float[] map, int width, int start, int end)
    {
        float[] seen = new float[map.Length];
        int[] path = new int[map.Length];
        int[] dirs = { 1, 1 - width, -width, -1 - width, -1, -1 + width, width, 1 + width };
        Queue<int> indexes = new Queue<int>();
        indexes.Enqueue(start);

        while(indexes.Count > 0)
        {
            indexes.OrderBy((index) => seen[index] + MathTools.Distance(index%width,index/width,end%width,end/width));
            int i = indexes.Dequeue();

            if(i == end)
            {
                return ReconstructPath(path, start, end);
            }

            for(int dir = 0; dir < dirs.Length; dir++)
            {
                int n = i + dirs[dir];
                if (n < 0 || n > map.Length - 1 || n == start) continue;
                float g = Mathf.Abs(seen[i]) + ((dir % 2 == 0) ? 1 : 1.41f);
                if(seen[n] != 0)
                {
                    if(Mathf.Abs(seen[n]) <= g)
                    {
                        continue;
                    }
                }
                seen[n] = g;
                if (!indexes.Contains(n))
                {
                    indexes.Enqueue(n);
                }
                path[n] = i;
            }
            seen[i] *= -1;
        }

        return null;
    }

    public static int[] ReconstructPath(int[] path, int start, int end)
    {
        List<int> rPath = new List<int>();
        int i = end;
        while(true)
        {
            rPath.Add(i);
            if (i == start)
                break;
            i = path[i];
        }
        rPath.Reverse();
        return rPath.ToArray();
    }
}
