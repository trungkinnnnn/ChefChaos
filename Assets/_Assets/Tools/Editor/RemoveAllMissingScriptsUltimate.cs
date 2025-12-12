using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class RemoveAllMissingScriptsUltimate : Editor
{
    [MenuItem("Tools/Cleanup/💀 Remove ALL Missing Scripts (Full Project Scan)")]
    static void CleanAll()
    {
        int totalRemoved = 0;
        int totalObjects = 0;

        // Quét toàn bộ prefab trong Assets/
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(prefab);
            if (removed > 0)
            {
                PrefabUtility.SavePrefabAsset(prefab);
                totalRemoved += removed;
                totalObjects++;
                Debug.Log($"🧹 Cleaned {removed} missing scripts in prefab: {path}");
            }

            // Kiểm tra object con trong prefab
            foreach (Transform child in prefab.GetComponentsInChildren<Transform>(true))
            {
                removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
                if (removed > 0)
                {
                    PrefabUtility.SavePrefabAsset(prefab);
                    totalRemoved += removed;
                }
            }
        }

        // Quét luôn scene hiện tại (nếu có)
        foreach (GameObject go in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go)) && go.hideFlags == 0)
            {
                int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                if (removed > 0) totalRemoved += removed;
            }
        }

        Debug.Log($"✅ DONE! Cleaned {totalRemoved} missing script components from {totalObjects} prefabs and open scenes.");
    }
}
