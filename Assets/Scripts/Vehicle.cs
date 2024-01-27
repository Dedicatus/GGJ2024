using Sirenix.OdinInspector;
using UnityEngine;

public class Vehicle : SerializedMonoBehaviour
{
    public float speed = 1;

    public bool forward;

    // private void Update()
    // {
    //     transform.Translate(forward ? Vector3.forward : Vector3.back * (speed * Time.deltaTime));
    // }
}