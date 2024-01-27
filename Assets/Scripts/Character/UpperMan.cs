using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UpperMan : MonoBehaviour
{
    public int inputId;
    public GameObject outterLeg;
    public GameObject outterArm;
    public GameObject body;
    public GameObject innerArmIkTarget;
    private float currentArmBalanceVal;
    public float balanceChangeSpeed;
    public float collisionBalanceDamage;
    [Space(10)]
    public float outterAngleChangeSpd;
    public float outterArmMinAngle;
    public float outterArmMaxAngle;
    public float outterLegMinAngle;
    public float outterLegMaxAngle;
    [Space(10)]
    public Transform innerArmAnchor;
    public Transform innerArmTarget;
    public float innerArmMinMove;
    public float innerArmMaxMove;
    public float disconnectArmAnchorX;
    public float disconnectArmMove;
    [Space(10)]
    public Transform hand;
    public float handConnectRange;
    [Space(10)]
    public float crouchMove;

    private bool crouch;
    private Vector3 initBodyLocalPos;
    private bool connected;
    private Vector2 innerAxis;
    private Vector2 outterAxis;
    [HideInInspector]
    public UnityAction<bool> OnChangeConnectState;
    private float disconnectTimer;
    private float crouchTimer;

    private void Start()
    {
        connected = true;
        initBodyLocalPos = body.transform.localPosition;
        OnChangeConnectState?.Invoke(connected);
    }

    private void Update()
    {
        Input();
        UpdateState();
        if (!connected && disconnectTimer < 0f)
        {
            TryConnect();
        }
        disconnectTimer -= Time.deltaTime;
        crouchTimer -= Time.deltaTime;
        currentArmBalanceVal = Mathf.Max(0f, currentArmBalanceVal);
        if (inputId == 1)
        {
            Motorcycle.Instance.BalanceValue -= balanceChangeSpeed * currentArmBalanceVal * Time.deltaTime;
        }
        if (inputId == 2)
        {
            Motorcycle.Instance.BalanceValue += balanceChangeSpeed * currentArmBalanceVal * Time.deltaTime;
        }
    }

    private void UpdateState()
    {
        var yAngle = 0f;
        if (inputId == 1)
        {
            yAngle = 180f;
            outterAxis.x *= -1f;
            innerAxis.x *= -1f;
        }
        if (crouch)
        {
            body.transform.localPosition = initBodyLocalPos - Vector3.up * crouchMove;
        }
        else
        {
            body.transform.localPosition = initBodyLocalPos;
        }
        if (connected)
        {
            var t = Mathf.Lerp(0f, 1f, outterAxis.y * 0.5f + 0.5f);
            if (outterAxis.magnitude < 0.1f)
            {
                t = 0f;
            }
            currentArmBalanceVal = Mathf.Lerp(0f, 1f, outterAxis.x);

            var currentRot = outterArm.transform.rotation;
            var targetRot = Quaternion.Euler(0f, yAngle, Mathf.Lerp(outterArmMinAngle, outterArmMaxAngle, t));
            outterArm.transform.rotation = Quaternion.RotateTowards(currentRot, targetRot, outterAngleChangeSpd * Time.deltaTime);


            currentRot = outterLeg.transform.rotation;
            targetRot = Quaternion.Euler(0f, yAngle, Mathf.Lerp(outterLegMinAngle, outterLegMaxAngle, t));
            outterLeg.transform.rotation = Quaternion.RotateTowards(currentRot, targetRot, outterAngleChangeSpd * Time.deltaTime);

            t = -1f;
            if (innerAxis.magnitude > 0.9f && innerAxis.x < 0f)
            {
                t = Mathf.Lerp(0f, 1f, innerAxis.y * 0.5f + 0.5f);
            }
            if (t != -1f)
            {
                innerArmTarget.position = innerArmAnchor.position + Vector3.up * Mathf.Lerp(innerArmMinMove, innerArmMaxMove, t);
            }

        }
        else if (innerAxis.magnitude > 0.3f)
        {
            if (inputId == 1)
            {
                var invertX = (Vector3)innerAxis;
                invertX.x *= -1;
                innerArmIkTarget.transform.position = innerArmAnchor.position + invertX * disconnectArmMove;
            }
            else
            {
                innerArmIkTarget.transform.position = innerArmAnchor.position + (Vector3)innerAxis * disconnectArmMove;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AirObstacle"))
        {
            BreakConnect();
            if (inputId == 1)
            {
                Motorcycle.Instance.BalanceValue -= collisionBalanceDamage;
            }
            if (inputId == 2)
            {
                Motorcycle.Instance.BalanceValue += collisionBalanceDamage;
            }
        }
    }
    private void TryConnect()
    {
        var cols = Physics.OverlapSphere(hand.position, handConnectRange);
        foreach (var col in cols)
        {
            if (col.transform != hand && col.CompareTag("Hand"))
            {
                connected = true;
                OnChangeConnectState?.Invoke(connected);
            }
        }
    }

    public void SyncConnectedState(bool state)
    {
        if (connected == state)
        {
            return;
        }
        if (!state)
        {
            var pos = innerArmAnchor.localPosition;
            pos.z = disconnectArmAnchorX;
            innerArmAnchor.localPosition = pos;
            innerArmIkTarget.transform.position = pos;
            disconnectTimer = 0.5f;
            connected = false;
        }
        else
        {
            connected = true;
        }
    }
    public void BreakConnect()
    {
        var pos = innerArmAnchor.localPosition;
        pos.z = disconnectArmAnchorX;
        innerArmAnchor.localPosition = pos;
        innerArmIkTarget.transform.position = pos;
        connected = false;
        disconnectTimer = 0.5f;
        OnChangeConnectState?.Invoke(connected);
    }

    private void Input()
    {
        Gamepad gamepad = null;
        try
        {
            gamepad = Gamepad.all[inputId - 1];
        }
        catch (System.Exception)
        {
            return;
        }
        if (gamepad == null)
        {
            return;
        }
        if (inputId == 1)
        {
            outterAxis = gamepad.leftStick.ReadValue();
            innerAxis = gamepad.rightStick.ReadValue();
            var crouch = gamepad.leftShoulder.ReadValue();
            crouch = Mathf.Max(crouch, gamepad.leftTrigger.ReadValue());
            crouch = Mathf.Max(crouch, gamepad.leftStickButton.ReadValue());
            if (crouch > 0.1f && crouchTimer < 0f)
            {
                this.crouch = !this.crouch;
                crouchTimer = 0.3f;
            }
            var hand = gamepad.rightShoulder.ReadValue();
            hand = Mathf.Max(hand, gamepad.rightTrigger.ReadValue());
            hand = Mathf.Max(hand, gamepad.rightStickButton.ReadValue());
            if (hand > 0.1f)
            {
                BreakConnect();
            }
        }
        if (inputId == 2)
        {
            outterAxis = gamepad.rightStick.ReadValue();
            innerAxis = gamepad.leftStick.ReadValue();
            var crouch = gamepad.rightShoulder.ReadValue();
            crouch = Mathf.Max(crouch, gamepad.rightTrigger.ReadValue());
            crouch = Mathf.Max(crouch, gamepad.rightStickButton.ReadValue());
            if (crouch > 0.1f && crouchTimer < 0f)
            {
                this.crouch = !this.crouch;
                crouchTimer = 0.3f;
            }
            var hand = gamepad.leftShoulder.ReadValue();
            hand = Mathf.Max(hand, gamepad.leftTrigger.ReadValue());
            hand = Mathf.Max(hand, gamepad.leftStickButton.ReadValue());
            if (hand > 0.1f)
            {
                BreakConnect();
            }
        }
    }
}
