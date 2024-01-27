using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Road;

public class Motorcycle : MonoSingleton<Motorcycle>
{
    public GameObject RoadMaker;
    private RoadParent roadParent;
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

    public float maxSpeed = 200;
    public float acceleration = 10;
    public float deceleration = 20;
    public float horizontalMoveSpeed = 10f;

    //public float rotationSpeed = 45f;
    public float maxRotationAngle = 30f; // 最大旋转角度
    public float maxBalanceSpeed = 2f; // 恢复平衡的速度
    public float balanceValueSpeed = 40f; // 平衡值的速度

    public float MaxBalanceValue = 100f; // 平衡值的最大值

    //public float balanceValueRegSpeed = 5f;

    private float currentRotationZ = 0f; // 当前的Z轴旋转角度

    private bool onSpringBack = false;
    [SerializeField]
    private float springBackTime = 0.4f;
    [SerializeField]
    private float springBackDistance = 1;
    [SerializeField]
    private float springBackBalance = 200;
    private float currentSpringBackTime = 0.0f;
    private float springBackDirection = 1;

    private void Start()
    {
        roadParent = RoadMaker.GetComponentInChildren<RoadParent>();
    }

    private void Update()
    {
        if(GameManager.Instance.GameState == GameManager.GAMESTATE.Start) {
            if (!onSpringBack)
            {
                if (Input.GetKey(KeyCode.A) && balanceValue < MaxBalanceValue)
                {
                    balanceValue -= balanceValueSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.D) && balanceValue > -MaxBalanceValue)
                {
                    balanceValue += balanceValueSpeed * Time.deltaTime;
                }
                else
                {
                    // 如果没有按键或已达到最大旋转角度，快速恢复平衡
                    balanceValue = Mathf.Lerp(balanceValue, 0, Mathf.Min(maxSpeed,roadParent.speed)* maxBalanceSpeed / maxSpeed * Time.deltaTime);
                }

                //if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                //{
                //    if (balanceValue > 0.05)
                //    {
                //        balanceValue -= balanceValueRegSpeed * Time.deltaTime;
                //    }
                //    else if (balanceValue < -0.05)
                //    {
                //        balanceValue += balanceValueRegSpeed * Time.deltaTime;
                //    }
                //    else
                //    {
                //        balanceValue = 0f;
                //    }
                //}

                if (Input.GetKey(KeyCode.W)){
                    if (roadParent.speed < maxSpeed)
                    {
                        roadParent.speed += acceleration * Time.deltaTime;
                    }
                }

                if (Input.GetKey(KeyCode.S))
                {
                    roadParent.speed -= deceleration * Time.deltaTime;
                }
                // 更新平衡值
                updateRotationValue();
                // 应用旋转
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentRotationZ);

                // 根据旋转角度调整X轴上的移动速度
                float moveHorizontal = currentRotationZ / maxRotationAngle;
                transform.position += new Vector3(-moveHorizontal * horizontalMoveSpeed * Time.deltaTime, 0, 0);


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
                balanceValue += springBackDirection * (springBackBalance/springBackTime) * Time.deltaTime;
                Mathf.Clamp(balanceValue, -100, 100);
                updateRotationValue();
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
        //BalanceValue -= (currentRotationZ / maxRotationAngle) * balanceValueSpeed;

        //if (BalanceValue > MaxBalanceValue)
        //{
        //    BalanceValue = MaxBalanceValue;
        //}
        //else if (BalanceValue < -MaxBalanceValue)
        //{
        //    BalanceValue = -MaxBalanceValue;
        //}
        currentRotationZ = -balanceValue / MaxBalanceValue * maxRotationAngle;
    }
}
