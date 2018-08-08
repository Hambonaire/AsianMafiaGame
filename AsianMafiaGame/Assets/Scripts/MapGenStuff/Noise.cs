using UnityEngine;
using System.Collections;

// Set class static since this is not going to be instantiated more than once
public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        // Psuedo random number generator. Store the map gen in this seed
        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            // If number is too high, returns same map
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        // Set minimum scale val to a small positive number to avoid division by 0 error
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfwidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfwidth) / scale * frequency + octaveOffsets[i].x; // higher frequency means further apart sample points
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y; // height values will changes more rapidly

                    // Mathf.PerlinNoise gives us value between 0 and 1
                    // Multiply by 2 and subtract 1 so that it returns values in between -1 and 1
                    // If perline values could be negative then our noiseHeight could decrease
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance; // Amplitude decreases for each octave since persistance is > 1
                    frequency *= lacunarity; // Frequency increases each octave since lacunarity is < 1
                }

                // Update max/min noise height
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        // Before returning noiseMap we need to normalize values to 0 and 1
        // to do this need to keep track of lowest and highest values in noiseMap
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //Mathf.InverseLerp returns a value between 0 and 1
                //so if noiseMap value is equal to min noise height, it will return 0. if it was equal to max
                //noise height, it would return 1, if it was halfway, it would return .5.
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}

        
