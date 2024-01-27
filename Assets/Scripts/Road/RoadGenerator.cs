using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Road
{
    public class RoadGenerator : MonoBehaviour
    {
        /// <summary>
        /// 已经生成出来的叠加距离
        /// </summary>
        public float generateDistance;


        public int curIndex;


        /// <summary>
        /// 玩家点到这个距离时，生成下一段道路
        /// </summary>
        public float maximumDistance;

        public float debugOffset;

        public RoadParent parent;

        public RoadCell[] cellList;

        [Button]
        public void Generate()
        {
            // 获取当前位置在 parent 坐标系中的位置

            var thisToParent = parent.transform.InverseTransformPoint(transform.position);
            while (debugOffset < maximumDistance)
            {
                debugOffset = Mathf.Abs(thisToParent.z - generateDistance);
                var cell = Instantiate(cellList[Random.Range(0, cellList.Length)], parent.transform);
                cell.transform.localPosition = Vector3.forward * generateDistance;
                generateDistance += cell.length;
                // cell.GetComponent<RoadCell>().index = curIndex;
                cell.index = curIndex;
                cell.center = transform;
                cell.name = curIndex.ToString();
                curIndex++;
            }
        }


        private void Update()
        {
            var thisToParent = parent.transform.InverseTransformPoint(transform.position);
            debugOffset = Mathf.Abs(thisToParent.z - generateDistance);
            Generate();
        }
    }
}