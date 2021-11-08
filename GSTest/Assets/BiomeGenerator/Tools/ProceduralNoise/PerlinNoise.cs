using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ProceduralGeneration/PerlinNoise")]
public class PerlinNoise : ScriptableObject
{
    public float frequency;
    public float amplitude;
    public int octaves;
    //Renderer rend;

    //Texture2D testTexture;

    private void Start()
    {
        //rend = GetComponent<Renderer>();
        //Debug.Log(rend.material.mainTexture.width);
        //testTexture = new Texture2D(rend.material.mainTexture.width, rend.material.mainTexture.width);
        //rend.material.mainTexture = testTexture;
        //GenerateNoiseMap(testTexture.width, testTexture.height);
    }

    public float[] GenerateNoiseMap(int size, int scale)
    {
        var data = new float[size*size*scale*scale];
        int N = size * scale;

        /// track min and max noise value. Used to normalize the result to the 0 to 1.0 range.
        var min = float.MaxValue;
        var max = float.MinValue;

        /// rebuild the permutation table to get a different noise pattern. 
        /// Leave this out if you want to play with changing the number of octaves while 
        /// maintaining the same overall pattern.
        Noise2D.Reseed();

        var f = frequency;
        var a = amplitude;

        for (var octave = 0; octave < octaves; octave++)
        {
            /// parallel loop - easy and fast.
            Parallel.For(0
                , N * N
                , (offset) =>
                {
                    var i = offset % N;
                    var j = offset / N;
                    var noise = Noise2D.Noise(i * f* 1f / N, j * f * 1f / N);
                    noise = data[j * N + i] += noise * a;

                    min = Mathf.Min(min, noise);
                    max = Mathf.Max(max, noise);

                }
            );

            f *= 2;
            a /= 2;
        }

        Parallel.For(0, N * N, (i) => data[i] = (data[i] - min) / (max - min));

        return data;

        /*if (noiseTexture != null && (noiseTexture.width != width || noiseTexture.height != height))
        {
            //noiseTexture.Dispose();
            noiseTexture = null;
        }
        if (noiseTexture == null)
        {
            noiseTexture = new Texture2D(width, height);
        }

        var colors = data.Select(
            (f) =>
            {
                var norm = (f - min) / (max - min);
                return new Color(norm, norm, norm, 1);
            }
        ).ToArray();

        noiseTexture.SetPixels(colors);*/
    }
}


