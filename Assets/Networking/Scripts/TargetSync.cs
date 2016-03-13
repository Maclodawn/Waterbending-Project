using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TargetSync : NetworkBehaviour
{

    private Vector3 syncPos; //Transmits value to all clients when changes
    Transform myTransform;
    public bool isReady { get; private set; }
    bool firstTransmissionDone = false;

    [Command]
    public void CmdSetReady()
    {
        isReady = true;
    }

    void Awake()
    {
        isReady = false;
    }

    void Start()
    {
        myTransform = transform;
    }

    public void FixedUpdate()
    {
        TransmitTransform();

        if (NetworkServer.active && firstTransmissionDone)
        {
            lerpPosition();
        }
    }

    [Server]
    private void lerpPosition()
    {
        myTransform.position = syncPos;
    }

    //Called by client, executed on server
    [Command]
    private void CmdProvidePositionToServer(Vector3 Pos)
    {
        if (isReady)
            firstTransmissionDone = true;
        syncPos = Pos;
    }

    //Only executed by clients
    [ClientCallback]
    private void TransmitTransform()
    {
        if (hasAuthority)
            CmdProvidePositionToServer(myTransform.position);
    }
}
