using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum GateStatus
{
    close = 0,
    opening,
    opened
}
public class Gate : NetworkBehaviour
{
    [SyncVar]
    public GateStatus status;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private Decoder decoder;
    public override void OnStartServer()
    {
        base.OnStartServer();
        decoder.actionComplete = CmdOpenGate;
    }
    private void Update()
    {
        if (status == GateStatus.opening)
        {
            if (leftDoor.localPosition.x > -1.5f && rightDoor.localPosition.x < 1.5f)
            {
                leftDoor.localPosition -= Time.deltaTime * Vector3.right;
                rightDoor.localPosition += Time.deltaTime * Vector3.right;
            }
            else
            {
                status = GateStatus.opened;
            }
        }
    }

    public void OpenGate()
    {
        if (status == GateStatus.close)
        {
            status = GateStatus.opening;
        }
    }

    [Command(requiresAuthority=false)]
    public void CmdOpenGate(){
        ClientRpcOpenGate();
        OpenGate();
    }
    [ClientRpc]
    public void ClientRpcOpenGate(){
        OpenGate();
    }
}
