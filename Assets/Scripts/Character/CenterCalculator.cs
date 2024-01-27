using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCalculator : MonoBehaviour
{
    public Transform armL;
    public Transform armR;
    public Transform armAnchorL;
    public Transform armAnchorR;
    public UpperMan playerL;
    public UpperMan playerR;
    public Transform armTgtL;
    public Transform armTgtR;
    public Transform armIkL;
    public Transform armIkR;

    private bool connected;

    private void Awake()
    {
        playerL.OnChangeConnectState += OnChangeConnectState;
        playerR.OnChangeConnectState += OnChangeConnectState;
    }
    private void Update()
    {
        if (connected)
        {
            armAnchorL.position = armAnchorR.position = (armL.position + armR.position) * 0.5f;
            armIkL.position = armIkR.position = (armTgtL.position + armTgtR.position) * 0.5f;
        }
    }
    private void OnChangeConnectState(bool connect)
    {
        connected = connect;
        playerL.SyncConnectedState(connect);
        playerR.SyncConnectedState(connect);
    }
}
