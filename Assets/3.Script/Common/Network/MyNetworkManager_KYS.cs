using Mirror;
using System;
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

    public new static MyNetworkManager_KYS singleton { get; internal set; }

    public GameObject _networkRoomManagerPrefab;

    [SerializeField]
    private Dictionary<string, MyNetworkRoomManager> _myNetworkRoomManagers; // key = host id

    public void AddMyNetworkRoomManager(UserDataModel_KYS hostData)
    {
        GameObject roomManagerObject = new GameObject(hostData.Id + "_RoomManager");
        MyNetworkRoomManager myRoomManager = roomManagerObject.AddComponent<MyNetworkRoomManager>();


        string tempSurfix = DateTime.Now.ToString();
        if (_myNetworkRoomManagers.ContainsKey(hostData.Id))
        {
            _myNetworkRoomManagers.Add(hostData.Id + tempSurfix, myRoomManager);
        }
        else
        {
            _myNetworkRoomManagers.Add(hostData.Id + tempSurfix, myRoomManager);
        }

        Debug.Log(hostData.Id + "  Added MNRM");
    }

    public MyNetworkRoomManager GetMyNetworkRoomManager(string key)
    {
        if(_myNetworkRoomManagers.ContainsKey(key))
            return _myNetworkRoomManagers[key];

        return null;
    }



    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);

        //if(newSceneName.Equals())
    }
}
