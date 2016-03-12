using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncPosition : NetworkBehaviour
{

    [SyncVar]
    private Vector3 syncPos; //Transmits value to all clients when changes
    [SerializeField]
    Transform myTransform;
    [SerializeField]
    float lerpRate = 15f;

    public void FixedUpdate()
    {
        TransmitPosition();
        lerpPosition();
    }

    private void lerpPosition()
    {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }

    //Called by client, executed on server
    [Command]
    private void CmdProvidePositionToServer(Vector3 Pos)
    {
        syncPos = Pos;
    }

    //Only executed by clients
    [ClientCallback]
    private void TransmitPosition()
    {
        if (isLocalPlayer)
            CmdProvidePositionToServer(myTransform.position);
    }
}
