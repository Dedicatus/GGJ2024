using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Road;
using Sirenix.OdinInspector;

public class Motorcycle : MonoSingleton<Motorcycle>
{
    public GameObject RoadMaker;
    [HideInInspector]
    public RoadParent roadParent;
    private float balanceValue = 0f;
    public float BalanceValue
    {
        set
        {
            balanceValue = value;
            updateRotationValue();
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotationZ);
            if (Mathf.Abs(balanceValue) > 100)
            {
                GameManager.Instance.EndGame(false);
                Crush();
            }
        }
        get
        {
            return balanceValue;
        }
    }
    private float upperManBalanceValue = 0f;
    public float UpperManBalanceValue
    {
        set
        {
            upperManBalanceValue = value;
            updateBalanceValue();

        }
        get
        {
            return upperManBalanceValue;
        }
    }
    private float motorcycleBalanceValue = 0f;
    public float MotorcycleBalanceValue
    {
        set
        {
            motorcycleBalanceValue = value;
            updateBalanceValue();
        }
        get
        {
            return motorcycleBalanceValue;
        }
    }

    [ReadOnly]
    public float passedDistance = 0f;
    public float boardValue = 10f;

    public float maxSpeed = 200;
    public float minSpeed = 1;
    public float acceleration = 10;
    public float deceleration = 20;
    public float horizontalMoveSpeed = 10f;

    //public float rotationSpeed = 45f;
    public float maxRotationAngle = 30f; // 最大旋转角度
    public float maxMotorBalanceRegenSpeed = 2f; // 恢复平衡的速度
    public float motorBalanceValueChangeSpeed = 40f; // 平衡值的速度

    public float MaxBalanceValue = 100f; // 平衡值的最大值

    private float currentRotationZ = 0f; // 当前的Z轴旋转角度

    private bool onSpringBack = false;
    [SerializeField]
    private float springBackTime = 0.4f;
    [SerializeField]
    private float springBackDistance = 1;
    [SerializeField]
    private float springBackBalance = 100;
    private float currentSpringBackTime = 0.0f;
    private float springBackDirection = 1;

    private void Start()
    {
        roadParent = RoadMaker.GetComponentInChildren<RoadParent>();
    }

    private void Update()
    {
        if (GameManager.Instance.GameState == GameManager.GAMESTATE.Start)
        {
            if (!onSpringBack)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    if (MotorcycleBalanceValue > -MaxBalanceValue * 0.5)
                    {
                        MotorcycleBalanceValue -= motorBalanceValueChangeSpeed * Time.deltaTime;
                    }
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    if (MotorcycleBalanceValue < MaxBalanceValue * 0.5)
                    {
                        MotorcycleBalanceValue += motorBalanceValueChangeSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    // 如果没有按键或已达到最大旋转角度，快速恢复平衡
                    MotorcycleBalanceValue = Mathf.Lerp(MotorcycleBalanceValue, 0, Mathf.Min(maxSpeed, roadParent.speed) * maxMotorBalanceRegenSpeed / maxSpeed * Time.deltaTime);
                }

                if (Input.GetKeyUp(KeyCode.W))
                {
                    if (roadParent.speed < maxSpeed)
                    {
                        roadParent.speed += acceleration * Time.deltaTime;
                    }
                }

                if (Input.GetKey(KeyCode.S))
                {
                    if (roadParent.speed > minSpeed)
                    {
                        roadParent.speed -= deceleration * Time.deltaTime;
                    }
                }

                passedDistance += roadParent.speed * Time.deltaTime;

                // 根据旋转角度调整X轴上的移动速度
                float moveHorizontal = currentRotationZ / maxRotationAngle;
                transform.position += new Vector3(-moveHorizontal * horizontalMoveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                transform.position += new Vector3(springBackDirection * (springBackDistance / springBackTime) * Time.deltaTime, 0, 0);
                currentSpringBackTime += Time.deltaTime;
                BalanceValue += springBackDirection * (springBackBalance / springBackTime) * Time.deltaTime;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotationZ);

                if (currentSpringBackTime >= springBackTime)
                {
                    onSpringBack = false;
                }
            }
        }
    }

    private void updateRotationValue()
    {

        currentRotationZ = -BalanceValue / MaxBalanceValue * maxRotationAngle;
        Mathf.Clamp(currentRotationZ, -maxRotationAngle, maxRotationAngle);
    }

    private void updateBalanceValue()
    {
        BalanceValue = UpperManBalanceValue + MotorcycleBalanceValue;
    }

    public void startSpringBack()
    {
        onSpringBack = true;
        currentSpringBackTime = 0.0f;
        springBackDirection = transform.position.x > 0 ? -1 : 1;
        springBackBalance = Mathf.Abs(BalanceValue) * 1.8f;
    }
    public void Crush()
    {
        transform.rotation = Quaternion.identity;
        roadParent.speed = 0f;
        GetComponent<Animator>().SetTrigger("Crush");
    }
}
