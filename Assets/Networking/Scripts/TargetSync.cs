using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class TargetSync : NetworkBehaviour
{

    [SyncVar]
    private Vector3 syncPos; //Transmits value to all clients when changes
    [SyncVar]
    private Quaternion syncRotation; //Transmits value to all clients when changes
    Transform myTransform;
//     [SerializeField]
//     float lerpRate = 15f;
    bool isReady = false;

    [Command]
    public void CmdSetReady()
    {
        isReady = true;
    }

    void Start()
    {
        myTransform = transform;
    }

    public void FixedUpdate()
    {
        TransmitTransform();

        if (isServer && isReady)
        {
            lerpPosition();
            lerpRotation();
        }
    }

    [Server]
    private void lerpPosition()
    {
        myTransform.position = syncPos;
        //myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    [Server]
    private void lerpRotation()
    {
        myTransform.rotation = syncRotation;
        //myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRotation, Time.deltaTime * lerpRate);
    }

    //Called by client, executed on server
    [Command]
    private void CmdProvideTransformToServer(Vector3 Pos, Quaternion rotation)
    {
        syncPos = Pos;
        syncRotation = rotation;
    }

    //Only executed by clients
    [ClientCallback]
    private void TransmitTransform()
    {
        if (hasAuthority)
            CmdProvideTransformToServer(myTransform.position, myTransform.rotation);
    }
}
