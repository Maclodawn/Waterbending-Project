using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MyNetworkManager : NetworkManager
{

    public override void OnServerConnect(NetworkConnection conn)
    {
        //conn.SetChannelOption(0, ChannelOption.MaxPendingBuffers, 511);
    }
}
