using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace UnityLua {
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
    public class GUID : MonoBehaviour {
        public string guid;
#if UNITY_EDITOR
        public bool update = false;
        private void Update() {
            if (!update) {
                return;
            }
            try {
                string asset_path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                guid = AssetDatabase.AssetPathToGUID(asset_path);
                update = false;
                PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.AutomatedAction);
            } catch (System.Exception) {
                if (string.IsNullOrEmpty(guid)) guid = Guid.NewGuid().ToString();
                update = false;
            }
        }
#endif
    }
}