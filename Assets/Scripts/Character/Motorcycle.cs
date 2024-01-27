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

    public float boardValue = 10f;
    public float moveSpeed = 5f;

    public float rotationSpeed = 45f;
    public float maxRotationAngle = 30f; // 最大旋转角度
    public float balanceSpeed = 2f; // 恢复平衡的速度
    public float balanceValueSpeed = 2f; // 平衡值的速度

    public float balanceValueMax = 100f; // 平衡值的最大值

    public float balanceValueRegSpeed = 100f;

    private float currentRotationZ = 0f; // 当前的Z轴旋转角度

    private bool onSpringBack = false;
    [SerializeField]
    private float springBackTime = 0.4f;
    [SerializeField]
    private float springBackDistance = 1;
    [SerializeField]
    private float springBackRotation = 60;
    private float currentSpringBackTime = 0.0f;
    private float springBackDirection = 1;

    private void Update()
    {
        if(GameManager.Instance.GameState == GameManager.GAMESTATE.Start) {
            if (!onSpringBack)
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

                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    if (balanceValue > 0.05)
                    {
                        balanceValue -= balanceValueRegSpeed * Time.deltaTime;
                    }
                    else if (balanceValue < -0.05)
                    {
                        balanceValue += balanceValueRegSpeed * Time.deltaTime;
                    }
                    else
                    {
                        balanceValue = 0f;
                    }
                }

                // 应用旋转
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotationZ);

                // 根据旋转角度调整X轴上的移动速度
                float moveHorizontal = currentRotationZ / maxRotationAngle;
                transform.position += new Vector3(-moveHorizontal * moveSpeed * Time.deltaTime, 0, 0);

                // 更新平衡值
                updateBalanceValue();

                if (Mathf.Abs(transform.position.x) > boardValue)
                {
                    onSpringBack = true;
                    currentSpringBackTime = 0.0f;
                    springBackDirection = transform.position.x > 0 ? -1 : 1;
                }
            }
            else
            {
                transform.position += new Vector3(springBackDirection * (springBackDistance/springBackTime) * Time.deltaTime, 0, 0);
                currentSpringBackTime += Time.deltaTime;
                currentRotationZ += -springBackDirection * (springBackRotation/springBackTime) * Time.deltaTime;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotationZ);
                updateBalanceValue();

                if (currentSpringBackTime >= springBackTime)
                {
                    onSpringBack = false;
                }
            }
        }
    }

    private void updateBalanceValue()
    {
        BalanceValue -= (currentRotationZ / maxRotationAngle) * balanceValueSpeed;

        if (BalanceValue > balanceValueMax)
        {
            BalanceValue = balanceValueMax;
        }
        else if (BalanceValue < -balanceValueMax)
        {
            BalanceValue = -balanceValueMax;
        }
    }
}
