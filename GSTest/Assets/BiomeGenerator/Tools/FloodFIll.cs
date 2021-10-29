using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FloodFIll
{
    public static int NodeCount(Tuple<int,int> start, float[] mapData, int mapWidth, bool[]Closed, float lowerBound, float upperBound, 
        int xBorderL, int xBroderR, int yBorderD, int yBorderU)
    {
        Queue<Tuple<int, int>> indexes = new Queue<Tuple<int, int>>();
        indexes.Enqueue(start);
        int val = 0;

        while (indexes.Count > 0)
        {
            Tuple<int, int> node = indexes.Dequeue();
            if (Closed[node.Item2 * mapWidth + node.Item1])
            {
                continue;
            }
            Closed[node.Item2 * mapWidth + node.Item1] = true;
            if (mapData[node.Item2 * mapWidth + node.Item1] < lowerBound || mapData[node.Item2 * mapWidth + node.Item1] > upperBound)
            {
                continue;
            }

            if (node.Item1 + 1 < xBroderR)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1 + 1, node.Item2));
            }
            if (node.Item1 - 1 >= xBorderL)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1 - 1, node.Item2));
            }
            if (node.Item2 + 1 < yBorderU)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1, node.Item2 + 1));
            }
            if (node.Item2 - 1 >= yBorderD)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1, node.Item2 - 1));
            }
            val++;
        }
        return val;
    }

    public static int NodeCount(Tuple<int, int> start, float[] mapData, int mapWidth, bool[] Closed, float lowerBound, float upperBound)
    {
        return NodeCount(start, mapData, mapWidth, Closed, lowerBound, upperBound, 0, mapWidth, 0, mapWidth);
    }

    public static float ValCount(Tuple<int, int> start, float[] mapData, int mapWidth, bool[] Closed, float lowerBound, float upperBound,
        int xBorderL, int xBroderR, int yBorderD, int yBorderU)
    {
        Queue<Tuple<int, int>> indexes = new Queue<Tuple<int, int>>();
        indexes.Enqueue(start);
        float val = 0;

        while (indexes.Count > 0)
        {
            Tuple<int, int> node = indexes.Dequeue();
            if (Closed[node.Item2 * mapWidth + node.Item1])
            {
                continue;
            }
            Closed[node.Item2 * mapWidth + node.Item1] = true;
            if (mapData[node.Item2 * mapWidth + node.Item1] < lowerBound || mapData[node.Item2 * mapWidth + node.Item1] > upperBound)
            {
                continue;
            }

            if (node.Item1 + 1 < xBroderR)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1 + 1, node.Item2));
            }
            if (node.Item1 - 1 >= xBorderL)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1 - 1, node.Item2));
            }
            if (node.Item2 + 1 < yBorderU)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1, node.Item2 + 1));
            }
            if (node.Item2 - 1 >= yBorderD)
            {
                indexes.Enqueue(new Tuple<int, int>(node.Item1, node.Item2 - 1));
            }
            val+= mapData[node.Item2 * mapWidth + node.Item1];
        }
        return val;
    }

    public static float ValCount(Tuple<int, int> start, float[] mapData, int mapWidth, bool[] Closed, float lowerBound, float upperBound)
    {
        return ValCount(start, mapData, mapWidth, Closed, lowerBound, upperBound, 0, mapWidth, 0, mapWidth);
    }
}
