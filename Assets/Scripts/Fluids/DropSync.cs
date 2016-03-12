using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DropSync : NetworkBehaviour
{

    Vector3 syncPosition;

    float syncVol;
    DropVolume myDropVolume;

//     [SerializeField]
//     float lerpRate = 15f;

    bool setDone = false;

    void Start()
    {
        myDropVolume = GetComponent<DropVolume>();
    }

    void FixedUpdate()
    {
        TransmitPosition();
        TransmitVolume();

        if (isClient)
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
        transform.position = syncPosition;
        //m_myTransform.position = Vector3.Lerp(m_myTransform.position, m_syncPosition, Time.deltaTime * lerpRate);
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
        if (isServer)
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
        if (isServer)
            RpcProvideVolumeToClient(myDropVolume.m_volume);
    }
}
