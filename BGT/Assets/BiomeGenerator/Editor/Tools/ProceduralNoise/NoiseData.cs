using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[CreateAssetMenu(menuName = "PerlinData")]
public class NoiseData : ScriptableObject
{
    public static List<float[]> PerlinData;
    public static int PerlinSize;
    public int dataSize;
    [SerializeField]
    Data data;
    public PerlinNoise perlin;
    //public string Running = "!running";

    [System.Serializable]
    internal class Data
    {

        public List<float[]> perlinData;
        public int perlinSize;
    }

    private void Awake()
    {
        Load();
    }

    private void OnEnable()
    {
        //Load();
        //GeneratePerlin();
        //PerlinData = perlinData;
        //PerlinSize = perlinSize;
    }

    public void AddPerlinData(float[] p)
    {
        data.perlinData.Add(p);
    }

    public static float[] GetPerlinNoise()
    {
        try
        {
            return PerlinData[(int)(new System.Random().NextDouble() * PerlinData.Count)];
        }
        catch
        {
            LoadData();
            return PerlinData[(int)(new System.Random().NextDouble() * PerlinData.Count)];
        }
    }

    public void GeneratePerlin()
    {
        data.perlinData = new List<float[]>();
        for (int i = 0; i < dataSize; i++)
        {
            AddPerlinData(perlin.GenerateNoiseMap(data.perlinSize, 1));
        }
        PerlinData = data.perlinData;
        PerlinSize = data.perlinSize;
    }

    public void Save()
    {
        var path = Application.dataPath + "/BiomeGenerator/Data/Noises.txt";
        FileStream file;
        if (!File.Exists(path))
        {
            var f = File.CreateText(path);
            f.Close();
        }

        file = File.OpenWrite(path);

        BinaryFormatter formater = new BinaryFormatter();
        formater.Serialize(file, data);

        file.Close();
        
    }

    public static void LoadData()
    {
        var path = Application.dataPath + "/BiomeGenerator/Data/Noises.txt";
        FileStream file;
        if (!File.Exists(path))
        {
            Debug.LogError("File Does Not Existe");
        }

        file = File.OpenRead(path);

        BinaryFormatter formater = new BinaryFormatter();
        var data = formater.Deserialize(file) as Data;
        PerlinData = data.perlinData;
        PerlinSize = data.perlinSize;

        file.Close();
    }

    public void Load()
    {
        var path = Application.dataPath + "/BiomeGenerator/Data/Noises.txt";
        FileStream file;
        if (!File.Exists(path))
        {
            Debug.LogError("File Does Not Existe");
        }

        file = File.OpenRead(path);

        BinaryFormatter formater = new BinaryFormatter();
        data = formater.Deserialize(file) as Data;
        PerlinData = data.perlinData;
        PerlinSize = data.perlinSize;

        file.Close();
    }
}



[CustomEditor(typeof(NoiseData))]
public class PerlinDataEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate New Data"))
        {
            //(target as NoiseData).Running = "Running!!!";
            (target as NoiseData).GeneratePerlin();
            //(target as NoiseData).Save();
        }
        if (NoiseData.PerlinData != null)
        {
            GUILayout.Label("" + NoiseData.PerlinData.Count);
        }
        else
        {
            GUILayout.Label("" + 0);
        }
        if (GUILayout.Button("Save Data"))
        {
            //(target as NoiseData).Running = "Running!!!";
            (target as NoiseData).Save();
            //(target as NoiseData).Save();
        }
    }
}
