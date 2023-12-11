using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//게임씬 들어오면 프로필 로딩하기
public class MyNetworkRoomManager : NetworkRoomManager
{
    //private Dictionary<int, UserDataModel_KYS> _userDatum = new Dictionary<int, UserDataModel_KYS>();
    private Dictionary<string, UserDataModel_KYS> _userDatum = new Dictionary<string, UserDataModel_KYS>();
    public Dictionary<string, UserDataModel_KYS> UserDatum
    {
        get => _userDatum;
        private set => _userDatum = value;
    }

    public void AddData(string userId, string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw)
    {
        Debug.Log("userData.Count111  " + UserDatum.Count + "\nid : " + userId);

        UserDataModel_KYS copy = UserDataModel_KYS.GetCopy(id, pwd, nick, lvl, exp, win, lose, draw);
        Debug.LogWarning(copy.ToString());

        UserDatum.Add(userId, copy);
    }

    public void AddData(string userId, UserDataModel_KYS data)
    {
        Debug.Log("userData.Count222  " + UserDatum.Count + "\nid : " + userId);

        UserDataModel_KYS copy = UserDataModel_KYS.GetCopy(data.Id, data.Pwd, data.Nick, data.Lvl, data.Exp, data.Win, data.Lose, data.Draw);
        Debug.LogWarning(copy.ToString());

        UserDatum.Add(userId, copy);
    }

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

    public int GetPlayerIndex(NetworkConnection conn)
    {
        // 플레이어 컨트롤러 가져오기
        NetworkRoomPlayer roomPlayer = conn.identity.GetComponent<NetworkRoomPlayer>();

        // 플레이어 컨트롤러의 인덱스 반환
        return roomPlayer.index;
    }

    bool showStartButton = true;


    //public override void OnGUI()
    //{
    //    base.OnGUI();

    //    if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
    //    {
    //        // set to false to hide it in the game scene
    //        showStartButton = false;

    //        ServerChangeScene(GameplayScene);
    //    }
    //}

    public override void OnRoomServerPlayersReady()
    {
        ServerChangeScene(GameplayScene);
    }

    //public override void ServerChangeScene(string newSceneName)
    //{
    //    // NetworkRoomManager 쪽 코드

    //    if (newSceneName == RoomScene)
    //    {
    //        foreach (NetworkRoomPlayer roomPlayer in roomSlots)
    //        {
    //            if (roomPlayer == null)
    //                continue;

    //            // find the game-player object for this connection, and destroy it
    //            NetworkIdentity identity = roomPlayer.GetComponent<NetworkIdentity>();

    //            if (NetworkServer.active)
    //            {
    //                // re-add the room object
    //                roomPlayer.GetComponent<NetworkRoomPlayer>().readyToBegin = false;
    //                NetworkServer.ReplacePlayerForConnection(identity.connectionToClient, roomPlayer.gameObject);
    //            }
    //        }

    //        allPlayersReady = false;
    //    }

        
    //    // 이하 NetworkManager 쪽 코드


    //    NetworkServer.SetAllClientsNotReady();
    //    networkSceneName = newSceneName;

    //    // 어차피 이걸 오버라이딩 하는 곳이 없어 부르는 것이 의미 없으나 일단 넣음
    //    OnServerChangeScene(newSceneName);

    //    NetworkServer.isLoadingScene = true;

    //    if (newSceneName != GameplayScene)
    //        loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName);
    //    else
    //    {
    //        loadingSceneAsync = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
    //    }

    //    if (NetworkServer.active)
    //    {
    //        // notify all clients about the new scene
    //        NetworkServer.SendToAll(new SceneMessage
    //        {
    //            sceneName = newSceneName
    //        });
    //    }

    //    startPositionIndex = 0;
    //    startPositions.Clear();
    //}
}
