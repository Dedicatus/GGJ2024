using UnityEditor;
using UnityEngine;

namespace Road.Editor
{
    public static class PrefabCreator
    {
        [MenuItem("Tools/CreateBuildings")]
        public static void Build()
        {
            // 获取目录下的所有预制体

            var prefabs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/SimpleTown/Prefabs/Buildings" });

            // 生成一个空物体作为父物体

            foreach (var prefab in prefabs)
            {
                var path = AssetDatabase.GUIDToAssetPath(prefab);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                var parent = new GameObject(go.name);
                parent.transform.position = Vector3.zero;
                parent.transform.rotation = Quaternion.identity;
                parent.transform.localScale = Vector3.one;
                parent.AddComponent<BoundMesh>();
                parent.AddComponent<RoadCell>();
                var instance = PrefabUtility.InstantiatePrefab(go, parent.transform) as GameObject;
                instance.transform.position = Vector3.zero;
                instance.transform.rotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one;
                PrefabUtility.SaveAsPrefabAsset(parent, $"Assets/Prefabs/Road/Buildings/{go.name}.prefab");
                Object.DestroyImmediate(parent);
            }

            // 然后同名嵌套多一层
        }
    }
}