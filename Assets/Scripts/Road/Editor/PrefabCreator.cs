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

                // 添加一个 Cube 到 0, 0 位置

                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(parent.transform);
                cube.transform.localPosition = Vector3.zero;
                cube.transform.localRotation = Quaternion.identity;
                cube.transform.localScale = Vector3.one;
                cube.name = "Cube";
                Object.DestroyImmediate(cube.GetComponent<BoxCollider>());


                var instance = PrefabUtility.InstantiatePrefab(go, parent.transform) as GameObject;
                instance.transform.position = Vector3.zero;
                instance.transform.rotation = Quaternion.identity;
                instance.transform.localScale = Vector3.one;
                PrefabUtility.SaveAsPrefabAsset(parent, $"Assets/Prefabs/Road/Buildings/{go.name}.prefab");
                Object.DestroyImmediate(parent);
            }
        }

        [MenuItem("Tools/AddBoxCollider")]
        public static void Create()
        {
            // 获取目录下的所有预制体

            var prefabs = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/SimpleTown/Prefabs/Vehicles" });

            // 生成一个空物体作为父物体

            foreach (var prefab in prefabs)
            {
                var path = AssetDatabase.GUIDToAssetPath(prefab);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                go.AddComponent<BoxCollider>();
            }
        }
    }
}