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
    [HideInInspector]
    public float currentBalenceVal;
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

    private bool connected;
    private Vector2 innerAxis;
    private Vector2 outterAxis;
    [HideInInspector]
    public UnityAction<bool> OnChangeConnectState;

    private void Start()
    {
        connected = true;
        OnChangeConnectState?.Invoke(connected);
    }

    private void Update()
    {
        Input();
        UpdateState();
        if (!connected)
        {
            TryConnect();
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
        if (connected)
        {
            var t = Mathf.Lerp(0f, 1f, outterAxis.y * 0.5f + 0.5f);
            if (outterAxis.magnitude < 0.1f)
            {
                t = 0f;
            }
            currentBalenceVal = outterAxis.y > 0f ? Mathf.Lerp(0f, 1f, outterAxis.x) : Mathf.Lerp(0.5f, 1f, outterAxis.x);

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
        else if (innerAxis.x < -0.3f)
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
        if(connected == state)
        {
            return;
        }
        if (state)
        {
            var pos = innerArmAnchor.localPosition;
            pos.z = disconnectArmAnchorX;
            innerArmAnchor.localPosition = pos;
            innerArmIkTarget.transform.position = pos;
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
            var body = gamepad.leftShoulder.ReadValue();
            body = Mathf.Max(body, gamepad.leftTrigger.ReadValue());
            body = Mathf.Max(body, gamepad.leftStickButton.ReadValue());
            if (body > 0.1f)
            {
                print("Body" + inputId + ": " + body);
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
            var body = gamepad.rightShoulder.ReadValue();
            body = Mathf.Max(body, gamepad.rightTrigger.ReadValue());
            body = Mathf.Max(body, gamepad.rightStickButton.ReadValue());
            if (body > 0.1f)
            {
                print("Body" + inputId + ": " + body);
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
