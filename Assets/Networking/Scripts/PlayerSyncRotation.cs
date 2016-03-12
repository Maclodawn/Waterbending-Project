using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerSyncRotation : NetworkBehaviour
{

    [SyncVar]
    private Quaternion syncRotation; //Transmits value to all clients when changes
    [SerializeField]
    Transform myTransform;
    [SerializeField]
    float lerpRate = 15f;

    public void FixedUpdate()
    {
        TransmitRotation();
        lerpRotation();
    }

    private void lerpRotation()
    {
        if (!isLocalPlayer)
        {
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, syncRotation, Time.deltaTime * lerpRate);
        }
    }

    //Called by client, executed on server
    [Command]
    private void CmdProvideRotationToServer(Quaternion rotation)
    {
        syncRotation = rotation;
    }

    //Only executed by clients
    [ClientCallback]
    private void TransmitRotation()
    {
        if (isLocalPlayer)
            CmdProvideRotationToServer(myTransform.rotation);
    }
}
