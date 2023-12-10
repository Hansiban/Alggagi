using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestPlayer : NetworkBehaviour
{
    private MyNetworkRoomPlayer _myNetworkRoomPlayer;
    private RockManager_YG _myRockManager;


    void Start()
    {
        Debug.Log("TEST PLAYER CS STARTED!!!!");

        var gg = FindObjectsOfType<MyNetworkRoomPlayer>();
        Debug.Log("MyNetworkRoomPlayer are this many : " + gg.Length);
        Debug.Log("gg.Any(x => x.isOwned) : " + gg.Any(x => x.isOwned));
        _myNetworkRoomPlayer = gg.Where(x => x.isOwned).FirstOrDefault();


        Debug.Log("TurnManagers are this many : " + FindObjectsOfType<TurnManager_YG>().Length);
        Debug.Log("TurnManager_YG.Any(x => x.isOwned) : " + FindObjectsOfType<TurnManager_YG>().Any(x => x.isOwned));


        var rr = FindObjectsOfType<RockManager_YG>();
        Debug.Log("RockManager are this many : " + rr.Length);
        Debug.Log("RockManager.Any(x => x.isOwned) : " + rr.Any(x => x.isOwned));
        _myRockManager = rr.Where(x => x.isOwned).FirstOrDefault();

        Debug.Log("MyNetworkRoomManager are this many : " + FindObjectsOfType<MyNetworkRoomManager>().Length);

        MyNetworkRoomManager.singleton.OnTurnNumChanged += RpcCheckIfMyTurn;

        Debug.LogWarning((_myRockManager == null) + " _myRockManager == null");
        Debug.LogWarning((_myNetworkRoomPlayer == null) + " _myNetworkRoomPlayer == null");

        if (isOwned)
            RegisterPlayer();
    }

    // Command 안 하면 각 클라이언트에게 있는 MyNetworkRoomManager를 각기 호출하기에 _playerNum이 1을 넘지 못한다. 진짜 싱글톤이 아니다
    [Command(requiresAuthority =false)]
    private void RegisterPlayer()
    {
        MyNetworkRoomManager.singleton.RegisterPlayer();
    }

    [ClientRpc]
    private void RpcCheckIfMyTurn(int newTurnNum)
    {
        Debug.Log((_myRockManager == null) + " _myRockManager == null\n" + _myNetworkRoomPlayer == null + " _myNetworkRoomPlayer == null");

        _myRockManager.is_myturn = (_myNetworkRoomPlayer.index == newTurnNum);

        Debug.Log(newTurnNum + " CAME TO TEST PLAYER + " + _myRockManager.is_myturn);
    }
}
