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
    //Client only
    private bool setDone = false;

    void Start()
    {
        //FIXME enable
        myWaterReserve = GetComponent<WaterReserve>();
    }

    void Update()
    {
        TransmitVol();

        if (isClient && setDone)
        {
            myWaterReserve.setVolume(syncVol);
            myWaterReserve.isReady = true;
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
        if (isServer && initDone)
            RpcProvideVolToClient(myWaterReserve.m_volume);
    }
}
