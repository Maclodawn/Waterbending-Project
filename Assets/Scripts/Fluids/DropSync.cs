using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[NetworkSettings(channel = 1, sendInterval = 0.0001f)]
public class DropSync : NetworkBehaviour
{

    Vector3 syncPosition;

    float syncVol;
    DropVolume myDropVolume;

    [SerializeField]
    float lerpRate;

    bool setDone = false;

    void Start()
    {
        myDropVolume = GetComponent<DropVolume>();
    }

    void FixedUpdate()
    {
        TransmitPosition();
        TransmitVolume();

        if (NetworkClient.active)
        {
            if (syncVol > 0)
                myDropVolume.setVolume(syncVol);
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

    [ClientRpc]
    private void RpcProvideVolumeToClient(float _volume)
    {
        syncVol = _volume;
    }

    [ServerCallback]
    private void TransmitVolume()
    {
        if (NetworkServer.active)
            RpcProvideVolumeToClient(myDropVolume.m_volume);
    }
}
