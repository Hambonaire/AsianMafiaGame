using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        // X and Z value of the point where we first create the triangles
        float topLeftX = (width - 1) / 2f;
        float topLeftZ = (height - 1) / 2f;
        // If level of detail is 0 then set it to 1. 
        int meshSismplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSismplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        // Iterate through the mesh
        for (int y = 0; y < height; y += meshSismplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSismplificationIncrement)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX - x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                // Add the triangles only if the starting point is not on the right or bottom edge (because there's none to be made at those areas)
                if (x < width -1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }
}

/**
 *  Mesh is compsed of small iscosceles right triangles
 *  #vertex = width * height
 *  #triangles = (width - 1)(height - 1)
 */
public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    public int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    // Create a triangle with its 3 points
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = c;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = a;
        triangleIndex += 3;
    }

    // Create mesh
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
