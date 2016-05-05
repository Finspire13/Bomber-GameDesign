using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapDataHelper))]
public class MapUnityEditorHelper: Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MapDataHelper helper = target as MapDataHelper;
        if (GUILayout.Button("Create Map Model"))
        {
            helper.createMapModel();
        }
        if (GUILayout.Button("Delete Map Model"))
        {
            helper.deleteMapModel();
        }
    }
}
