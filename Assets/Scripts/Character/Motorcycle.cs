using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motorcycle : MonoSingleton<Motorcycle>
{
    private float balanceValue = 0f;
    public float BalanceValue
    {
        set
        {
            balanceValue = value;
        }
        get
        {
            return balanceValue;
        }
    }

    public float moveSpeed = 5f;

    public float rotationSpeed = 45f;
    public float maxRotationAngle = 30f; // 最大旋转角度
    public float balanceSpeed = 2f; // 恢复平衡的速度

    private float currentRotationZ = 0f; // 当前的Z轴旋转角度
    
    private void Update()
    {

        if (Input.GetKey(KeyCode.A) && currentRotationZ < maxRotationAngle)
        {
            currentRotationZ += rotationSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) && currentRotationZ > -maxRotationAngle)
        {
            currentRotationZ -= rotationSpeed * Time.deltaTime;
        }
        else
        {
            // 如果没有按键或已达到最大旋转角度，快速恢复平衡
            currentRotationZ = Mathf.Lerp(currentRotationZ, 0, balanceSpeed * Time.deltaTime);
        }

        // 应用旋转
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotationZ);

        // 根据旋转角度调整X轴上的移动速度
        float moveHorizontal = currentRotationZ / maxRotationAngle;
        transform.position += new Vector3(-moveHorizontal * moveSpeed * Time.deltaTime, 0, 0);
    }
}
