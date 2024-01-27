using Sirenix.OdinInspector;
using UnityEngine;

namespace Road
{
    public class RoadCell : SerializedMonoBehaviour
    {
        public float length;

        public int index;

        public Transform center;

        public float farDistance = -150f;

        [Button]
        public void GetLengthX()
        {
            // 通过获取子节点在世界内的最小z值和最大z值，计算出长度

            var minZ = float.MaxValue;
            var maxZ = float.MinValue;
            foreach (var child in GetComponentsInChildren<MeshFilter>())
            {
                var bounds = child.sharedMesh.bounds;
                bounds.center = child.transform.localPosition;
                var max = child.sharedMesh.bounds.max.x;
                var min = child.sharedMesh.bounds.min.x;
                if (min < minZ)
                {
                    minZ = min;
                }

                if (max > maxZ)
                {
                    maxZ = max;
                }
            }

            length = maxZ - minZ;
        }


        [Button]
        public void GetLengthZ()
        {
            // 通过获取子节点在世界内的最小z值和最大z值，计算出长度

            var minZ = float.MaxValue;
            var maxZ = float.MinValue;
            foreach (var child in GetComponentsInChildren<MeshFilter>())
            {
                var max = child.sharedMesh.bounds.max.z;
                var min = child.sharedMesh.bounds.min.z;
                if (min < minZ)
                {
                    minZ = min;
                }

                if (max > maxZ)
                {
                    maxZ = max;
                }
            }

            length = maxZ - minZ;
        }


        public void Update()
        {
            var thisToParent = center.transform.InverseTransformPoint(transform.position);
            var distance = thisToParent.z;
            if (distance < farDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}