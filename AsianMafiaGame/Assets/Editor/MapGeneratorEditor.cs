using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI()
    {
        // The object this custom editor is generating
        MapGenerator mapGen = (MapGenerator)target;

        // If any value was changed in the editor and autoUpdate is true then generate map
        if (DrawDefaultInspector() && mapGen.autoUpdate)
        {
             mapGen.GenerateMap();
        }
        // Generate map when Generate button is pressed
        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
