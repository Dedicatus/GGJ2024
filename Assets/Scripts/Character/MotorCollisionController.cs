using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorCollisionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var mi = Motorcycle.Instance;
        if (other.CompareTag("Obstacle"))
        {
            Vector3 closestPoint = other.ClosestPoint(transform.position);
            Vector3 directionToClosestPoint = closestPoint - transform.position;

            // Normalize the direction
            directionToClosestPoint.Normalize();

            float angleToFront = Vector3.Angle(transform.forward, directionToClosestPoint);
            float angleToRight = Vector3.Angle(transform.right, directionToClosestPoint);

            // Determine the direction based on the angle
            if (angleToFront <= 30)
            {
                mi.roadParent.speed = Mathf.Max(mi.minSpeed, mi.roadParent.speed * 0.5f);
            }
            else
            {
                //Debug.Log(other.gameObject.name);
                //Debug.Log("Side collide");
                mi.startSpringBack();
            }
        }
    }
}
