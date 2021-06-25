using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGen))]
public class MapGenInspector:Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        MapGen mapGen = (MapGen)target;
        if (GUILayout.Button("Generate Map"))
        {
            mapGen.GenerateMap();
        }
    }
}
