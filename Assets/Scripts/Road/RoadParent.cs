using UnityEngine;

namespace Road
{
    public class RoadParent : MonoBehaviour
    {
        public float speed;

        public void Update()
        {
            transform.position += Vector3.back * (speed * Time.deltaTime);
        }
    }
}