using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

//게임씬 들어오면 프로필 로딩하기
public class MyNetworkRoomManager : NetworkRoomManager
{
    private Dictionary<string, UserDataModel_KYS> _userDatum = new Dictionary<string, UserDataModel_KYS>();
    public Dictionary<string, UserDataModel_KYS> UserDatum
    {
        get => _userDatum;
        private set => _userDatum = value;
    }

    public static new MyNetworkRoomManager singleton { get; private set; }

    public override void Awake()
    {
        base.Awake();
        singleton = this;
    }

    public void AddData(string userId, string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw)
    {
        Debug.Log("userData.Count111  " + UserDatum.Count + "\nid : " + userId);

        UserDataModel_KYS copy = UserDataModel_KYS.GetCopy(id, pwd, nick, lvl, exp, win, lose, draw);
        Debug.LogWarning(copy.ToString());

        UserDatum.Add(userId, copy);
    }

    public void RemoveUserData(string id)
    {
        UserDatum.Remove(id);
    }

    public int GetPlayerIndex(NetworkConnection conn)
    {
        // 플레이어 컨트롤러 가져오기
        NetworkRoomPlayer roomPlayer = conn.identity.GetComponent<NetworkRoomPlayer>();

        // 플레이어 컨트롤러의 인덱스 반환
        return roomPlayer.index;
    }

    public override void OnRoomServerPlayersReady()
    {
        ServerChangeScene(GameplayScene);
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        if(SceneManager.GetActiveScene().name == GameplayScene)
        {

            // 모두 room으로 돌아가기
        }
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log(UserDatum.Count + "개 남음");

        base.OnRoomServerDisconnect(conn);

        conn.Disconnect();

        StopClient();


        var cli = FindObjectsOfType<MyNetworkRoomPlayer>().Where(x => x.connectionToClient == conn).FirstOrDefault();
        if (cli == null)
            Debug.Log("cli == null");
        else
            cli.DiscardInfo();

        Debug.Log(UserDatum.Count + "개 남음");
        Debug.Log("1 CLIENT DISCONNECTED");

    }

    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();


    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        Debug.Log("OnApplicationQuit");

        if (NetworkClient.isConnected)
            StopClient();
        if (NetworkClient.active)
            StopServer();
    }
}
