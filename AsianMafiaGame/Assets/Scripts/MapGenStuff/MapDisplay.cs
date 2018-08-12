using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    
    public void DrawTexture(Texture2D texture)
    {
        // Set the texture to the texture we just generated
        textureRender.sharedMaterial.mainTexture = texture;
        // Set the size of the plane equal to the size of the map
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh(); //.sharedmesh bc maybe generating mesh from outside of gamemode
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
