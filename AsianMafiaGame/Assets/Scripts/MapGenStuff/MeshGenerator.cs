using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;  // -2f to 2f to make mesh not mirrored
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        // Mesh works in right triangles. To generate mesh we iterate through the noise map 
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Then we add in the vertices. In this case we create the triangles in clockwise pattern
                meshData.vertices[vertexIndex] = new Vector3(topLeftX - x, heightMap[x, y] * heightMultiplier, topLeftZ - y);    //topLeftX + x. Changed to make mesh not mirrored
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                // Since there are no triangles on the right and bottom side
                // Ignore right and bottom sides of the vertices
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    // Addint three points to mesh triangle
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = c;   // (a, b, c) to (c, b, a). Backwards to make mesh not mirrored
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = a;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();//for lighting purposes
        return mesh;
    }
}
