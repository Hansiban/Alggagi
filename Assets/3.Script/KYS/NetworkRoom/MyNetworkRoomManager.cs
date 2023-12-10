using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetworkRoomManager : NetworkRoomManager
{
    public static new MyNetworkRoomManager singleton { get; private set; }

    private int _turnNum;
    public int TurnNum
    {
        get => _turnNum;
        private set
        {
            _turnNum = value;
            OnTurnNumChanged.Invoke(_turnNum);
        }
    }
    public Action<int> OnTurnNumChanged;

    // called by rock manager only
    public void ChangeTurnNumber(int newTurnNum)
    {
        Debug.Log("ChangeTurnNumber : " + newTurnNum);
        TurnNum = newTurnNum;
    }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        singleton = this;

        OnTurnNumChanged = (_turnNum) => { Debug.Log($"<b>! ! ! {_turnNum} ! ! !</b>"); };
    }

    private static int _playerNum = 0;
    internal void RegisterPlayer()
    {
        Debug.Log("RegisterPlayer " + _playerNum);
        _playerNum++;

        if (_playerNum >= 2)
        {
            Debug.Log("_playerNum >= 2");
            TurnNum = 1;
            OnTurnNumChanged.Invoke(_turnNum);
        }
    }

    /// <summary>
    /// Called on the server when a scene is completed loaded, when the scene load was initiated by the server with ServerChangeScene().
    /// </summary>
    /// <param name="sceneName">The name of the new scene.</param>

    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        // spawn the initial batch of Rewards
        //if (sceneName == GameplayScene)
        //{
        //    Debug.Log("OnRoomServerSceneChanged");
        //    TurnNum = 1;
        //}
    }

    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
        showStartButton = true;
#endif
    }

    public override void OnGUI()
    {
        base.OnGUI();

        if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
        {
            // set to false to hide it in the game scene
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }

    public int GetPlayerIndex(NetworkConnection conn)
    {
        // 플레이어 컨트롤러 가져오기
        NetworkRoomPlayer roomPlayer = conn.identity.GetComponent<NetworkRoomPlayer>();
        
        // 플레이어 컨트롤러의 인덱스 반환
        return roomPlayer.index;
    }
}
