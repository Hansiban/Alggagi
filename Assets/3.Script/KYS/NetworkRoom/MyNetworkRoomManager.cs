using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyNetworkRoomManager : NetworkRoomManager
{
    public static new MyNetworkRoomManager singleton { get; private set; }

    /// <summary>
    /// Runs on both Server and Client
    /// Networking is NOT initialized when this fires
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }

    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();

        Debug.Log(SceneManager.GetActiveScene().name + "로 " + gameObject.name + "의 씬이 바뀌었습니다.");

        if (SceneManager.GetActiveScene().name == "WaitingRoom_KYS") // to be modified
        {
            Debug.Log(gameObject.name);

            gameObject.GetComponent<MyNetworkRoomPlayer>().FillInMyInfo();
            //CmdFillInPlayerProfiles(this.gameObject);
        }
    }

    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        Debug.Log(sceneName + "Loaded when OnRoomServerSceneChanged");

        // spawn the initial batch of Rewards
        if (sceneName == GameplayScene)
        {
            // profile 로딩
        }
    }

    /// <summary>
    /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
    /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
    /// into the GamePlayer object as it is about to enter the Online scene.
    /// </summary>
    /// <param name="roomPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        base.OnRoomServerSceneLoadedForPlayer(conn, roomPlayer, gamePlayer);

        Debug.LogError("게임 씬입니다 OnRoomServerSceneLoadedForPlayer");
        //CmdFillInPlayerProfiles(roomPlayer);

        return true;
    }

    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();
    }

    public override void OnRoomStopServer()
    {
        base.OnRoomStopServer();
    }

    /*
        This code below is to demonstrate how to do a Start button that only appears for the Host player
        showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
        all players are ready, but if a player cancels their ready state there's no callback to set it back to false
        Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
        Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
        is set as DontDestroyOnLoad = true.
    */

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }

    private PlayerProfile _hostProfile = new PlayerProfile();

    private UserDataModel_KYS _hostData;
    public UserDataModel_KYS HostData
    {
        get => _hostData;

        private set
        {
            _hostData = value;

            if (_hostData == null)
            {
                _hostProfile = new PlayerProfile();
            }
            else
            {
                _hostProfile.Init(_hostData);

                Debug.Log($"HOST ENTERED : {_hostData.Id}");
            }
        } 
    }

    private PlayerProfile _guestProfile = new PlayerProfile();

    private UserDataModel_KYS _guestData;
    public UserDataModel_KYS GuestData
    {
        get => _guestData;

        private set
        {
            _guestData = value;

            if (_guestData == null)
            {
                _guestProfile = new PlayerProfile();
            }
            else
            {
                _guestProfile.Init(_guestData);

                Debug.Log($"GUEST ENTERED : {_guestData.Id}");
            }
        }
    }


    // Network Behaviour 가진 애가 불러줘야 함
    /// <summary>
    /// This is called on the client when the client is finished loading a new networked scene.
    /// </summary>
    public override void OnRoomClientSceneChanged()
    {
        //CmdInsertClientInfo();
    }

    //[Command]
    public void CmdInsertClientInfo()
    {

        RpcInsertClientInfo();
    }

    //[ClientRpc]
    public void RpcInsertClientInfo()
    {
        if(HostData == null)
            HostData = GameManager.Instance.LocalUserData;
        else
            GuestData = GameManager.Instance.LocalUserData;
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
