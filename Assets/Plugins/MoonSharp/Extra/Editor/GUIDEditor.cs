using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
[CustomEditor(typeof(UnityLua.GUID))]
public class GUIDEditor : Editor
{
    private SerializedProperty type;
    UnityLua.GUID guidObject;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        guidObject = (UnityLua.GUID)target;
        GUILayout.TextField($"GUID: {guidObject.guid}");
        if (GUILayout.Button("Update")) {
            update_guid();
        }
    }
    private void update_guid() {
        try {
            string asset_path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(guidObject);
            guidObject.guid = AssetDatabase.AssetPathToGUID(asset_path);
            PrefabUtility.ApplyPrefabInstance(guidObject.gameObject, InteractionMode.AutomatedAction);
        } catch (System.Exception) {
            if (string.IsNullOrEmpty(guidObject.guid)) guidObject.guid = Guid.NewGuid().ToString();
        }
    }
}
