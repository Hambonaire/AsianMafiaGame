using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator {

    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);

        texture.filterMode = FilterMode.Point; //instead of default bilinear mode. makes map crisp
        texture.wrapMode = TextureWrapMode.Clamp; // Map outside of our range doesn't show up in the map
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        // Pass in the widdth and height of the noise map
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // Get all of the colors of the map and assign them to the map
        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Find the index of the section of the map then color it accordingly
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }

        return TextureFromColorMap(colorMap, width, height);
    }

}
