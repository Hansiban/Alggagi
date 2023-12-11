using Mirror;
using System.Collections;
using System.Collections.Generic;
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
}
