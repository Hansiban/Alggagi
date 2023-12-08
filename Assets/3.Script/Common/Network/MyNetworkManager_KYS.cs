using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkManager_KYS : NetworkManager
{
    #region
    //public static new MyNetworkManager_KYS singleton { get; private set; }

    //public List<NetworkConnectionToClient> list;

    //public override void Awake()
    //{
    //    base.Awake();
    //    //singleton = this;

    //    list = new List<NetworkConnectionToClient>();
    //}

    //public override void OnServerConnect(NetworkConnectionToClient conn)
    //{
    //    base.OnServerConnect(conn);

    //    list.Add(conn);

    //    Debug.Log(conn.address + " 접속하였습니다.");
    //}

    //public override void OnServerDisconnect(NetworkConnectionToClient conn)
    //{
    //    base.OnServerDisconnect(conn);

    //    list.Remove(conn);

    //    Debug.Log(conn.address + " 접속 끊겼습니다.");
    //}

    //// Client 쪽에서 불리는 메소드라캄
    //public override void OnClientDisconnect()
    //{
    //    base.OnClientDisconnect();
    //}
    #endregion

    Dictionary<string, NetworkRoomManager> _networkRoomManagers; // key = host id

    // call this from 

}
