using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DropSync : NetworkBehaviour
{

    Vector3 syncPosition;

    [SerializeField]
    float lerpRate = 1.0f;

    bool setDone = false;

    void FixedUpdate()
    {
        TransmitPosition();

        if (NetworkClient.active)
        {
            if (setDone)
                lerpPosition();
        }
    }

    [Client]
    private void lerpPosition()
    {
        transform.position = Vector3.Lerp(transform.position, syncPosition, lerpRate);
    }

    [ClientRpc]
    private void RpcProvidePositionToClient(Vector3 _position)
    {
        syncPosition = _position;
        setDone = true;
    }

    [ServerCallback]
    private void TransmitPosition()
    {
        if (NetworkServer.active)
            RpcProvidePositionToClient(transform.position);
    }
}
