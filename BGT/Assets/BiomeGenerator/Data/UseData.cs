using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;

public class UseData
{
    private static string id = ((Environment.MachineName + DateTime.Now).GetHashCode()).ToString();
    private static Stopwatch clock = new Stopwatch();
    private static Stopwatch gaClock = new Stopwatch();
    private static int createMapCounter = 0;
    private static Dictionary<string,int> fitenessFunctions = new Dictionary<string, int>();
    private static long sesionTime = 0;
    private static long suggestionsUsed = 0;
    private static bool isDummy = true;
    public static bool IsDummy => isDummy;

    public static void AddGa(string ganame)
    {
        if(fitenessFunctions.ContainsKey(ganame))
        {
            fitenessFunctions[ganame]++;
        }
        else
        {
            fitenessFunctions.Add(ganame, 1);
        }
    }

    public static void RegisterMapCreation()
    {
        createMapCounter++;
    }

    public static string GetCode()
    {
        return id;
    }

    public static void RegisterSuggestionUse()
    {
        suggestionsUsed++;
    }

    public static void StartClock()
    {
        clock.Restart();
    }

    public static void StopClock()
    {
        clock.Stop();
        sesionTime = clock.ElapsedMilliseconds / 1000;

    }

    public static void IsRunning(bool b)
    {
        if(b)
        {
            gaClock.Start();
        }
        else
        {
            gaClock.Stop();
        }
    }

    public static void Save()
    {
        var path = Application.dataPath + "/BiomeGenerator/Data/test.txt";
        if (!File.Exists(path))
        {
            var f = File.CreateText(path);
            f.Close();
        }

        var text = new List<string>();

        text.Add("User ID; " + id + "\n");
        text.Add("Dummy User; " + IsDummy);
        //UnityEngine.Debug.Log(sesionTime);
        var seconds = sesionTime % 60;
        sesionTime /= 60;
        var minutes = sesionTime % 60;
        sesionTime /= 60;
        text.Add("Suggestions Used; " + suggestionsUsed + "\n");
        text.Add("Generated Maps: " + createMapCounter + "\n");
        text.Add("Fitness Funcions Used \n");
        foreach (KeyValuePair<string,int> p in fitenessFunctions)
        {
            text.Add(p.Key + ": " + p.Value + "\n");
        }
        text.Add("User Sesion Time; " + sesionTime + ":" + minutes + ":" + seconds + "\n");

        File.AppendAllLines(path, text);

        var reducedPath = Application.dataPath + "/BiomeGenerator/Data/Parse.txt";
        if (!File.Exists(path))
        {
            var f = File.CreateText(reducedPath);
            f.Close();
        }

        var t = new List<string>();

        t.Add("" + id + ";");
        t.Add("" + IsDummy + ";");
        //UnityEngine.Debug.Log(sesionTime);
        t.Add("" + suggestionsUsed + ";");
        t.Add("" + createMapCounter + ";");
        t.Add("" + sesionTime + ":" + minutes + ":" + seconds + "\n\n");
        t.Add("-\n\n");
        foreach (KeyValuePair<string, int> p in fitenessFunctions)
        {
            t.Add(p.Key + ": " + p.Value + "\n");
        }

        File.AppendAllLines(reducedPath, t);

    }
}
