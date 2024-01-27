using Sirenix.OdinInspector;
using UnityEngine;

public class BoundMesh : SerializedMonoBehaviour
{
    public Bounds bounds;

    public Vector3 forward = Vector3.right;
    
    [Button]
    public void GetBound()
    {
        var filters = GetComponentsInChildren<MeshFilter>();

        // 获取所有子物体的 mesh 的 bounds，然后合并
        for (var i = 0; i < filters.Length; i++)
        {
            var filter = filters[i];
            
            var childBounds = filter.sharedMesh.bounds;
            childBounds.center = filter.transform.position;
            if (i == 0)
            {
                bounds = childBounds;
            }
            else
            {
                bounds.Encapsulate(childBounds);
            }
        }
    }

    [Button]
    public void RemapPos()
    {
        
        // 依据 bounds 和 forward 重新计算子物体的位置

        var children = GetComponentsInChildren<MeshFilter>();
        
        foreach (var child in children)
        {
            if (child.name == "Cube")
            {
                continue;
            }
            child.transform.localPosition = bounds.center - forward * bounds.extents.x;
            child.transform.Rotate(Vector3.up, -90);
        }
    }
    
    // [Button]
    // public void DemapChildPos()
    // {
    //     var children = GetComponentsInChildren<Transform>();
    //     foreach (var child in children)
    //     {
    //         if (child == transform)
    //         {
    //             continue;
    //         }
    //
    //         var localPos = child.localPosition;
    //         var x = Mathf.Lerp(bounds.min.x, bounds.max.x, localPos.x);
    //         var y = Mathf.Lerp(bounds.min.y, bounds.max.y, localPos.y);
    //         var z = Mathf.Lerp(bounds.min.z, bounds.max.z, localPos.z);
    //         child.localPosition = new Vector3(x, y, z);
    //     }
    // }

    public void OnDrawGizmos()
    {
        // Gizmos.matrix = transform.worldToLocalMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}