using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLoginManager_KYS : NetworkManager
{
    public static new NetworkLoginManager_KYS singleton { get; private set; }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }


    //public override void Awake()
    //{
    //    base.Awake();

    //    if (isNetworkActive)
    //        StartClient();
    //    else
    //        StartServer();
    //}

    // log in 

}
