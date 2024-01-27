using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Road
{
    public class RoadCell : SerializedMonoBehaviour
    {
        public float length;

        public int index;

        public Transform center;
        
        public float farDistance;
        
        [Button]
        public void GetLength()
        {
            // 通过获取子节点在世界内的最小z值和最大z值，计算出长度

            var minZ = float.MaxValue;
            var maxZ = float.MinValue;
            foreach (var child in GetComponentsInChildren<Transform>())
            {
                if (child == transform)
                {
                    continue;
                }

                var z = child.position.z;
                if (z < minZ)
                {
                    minZ = z;
                }

                if (z > maxZ)
                {
                    maxZ = z;
                }
            }
            length = maxZ - minZ;
        }


        public void Update()
        {
            // var distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            var thisToParent = center.transform.InverseTransformPoint(transform.position);
            var distance = thisToParent.z;
            if (distance < farDistance)
            {
                 Destroy(gameObject);
            }
        }
    }
}