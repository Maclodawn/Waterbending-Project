using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WaterReserveSync : NetworkBehaviour
{
    //Manually synchronized
    private float syncVol;
    WaterReserve myWaterReserve;

    [System.NonSerialized][SyncVar]
    public bool initDone = false;
    private bool setDone = false;

    void Start()
    {
        myWaterReserve = GetComponent<WaterReserve>();
    }

    void Update()
    {
        TransmitVol();

        if (NetworkClient.active && setDone)
        {
            myWaterReserve.setVolume(syncVol);
        }
    }

    [ClientRpc]
    private void RpcProvideVolToClient(float vol)
    {
        syncVol = vol;
        setDone = true;
    }

    [ServerCallback]
    private void TransmitVol()
    {
        if (NetworkServer.active && initDone)
            RpcProvideVolToClient(myWaterReserve.m_volume);
    }
}
