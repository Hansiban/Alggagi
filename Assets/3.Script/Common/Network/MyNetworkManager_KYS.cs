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

    //    Debug.Log(conn.address + " �����Ͽ����ϴ�.");
    //}

    //public override void OnServerDisconnect(NetworkConnectionToClient conn)
    //{
    //    base.OnServerDisconnect(conn);

    //    list.Remove(conn);

    //    Debug.Log(conn.address + " ���� ������ϴ�.");
    //}

    //// Client �ʿ��� �Ҹ��� �޼ҵ��į
    //public override void OnClientDisconnect()
    //{
    //    base.OnClientDisconnect();
    //}
    #endregion

    Dictionary<string, NetworkRoomManager> _networkRoomManagers; // key = host id

    // call this from 

}
