using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

// 여기가 아니라 MyNetworkRoomManager가 처리.. 아니다. 얘는 networkbrhavioye없어서커맨드안됨 
public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private GameObject _profilePrefab;
    [SerializeField] private GameObject _readyButtonPrefab;

    public RockManager_YG rockmanager;

    public void DiscardInfo()
    {
        CmdDestroyProfile(GameManager.Instance.LocalUserData.Nick);

        CmdRemoveUserData(GameManager.Instance.LocalUserData.Id);
    }

    [Command]
    private void CmdDestroyProfile(string nick)
    {
        var pf = FindObjectsOfType<PlayerProfile>().Where(x => x.Nick == nick).FirstOrDefault();
        NetworkServer.Destroy(pf.gameObject);
    }

    [Command]
    private void CmdRemoveUserData(string id)
    {
        MyNetworkRoomManager.singleton.RemoveUserData(id);
    }


    // Waiting Room에서의 프로필 데이터 연동도 해당 방법을 사용하면 좋았겠으나
    // 현재로서는 GamePlayer Scene의 프로필 데이터 연동에만 사용된다
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // null 뜸 OnClientEnterRoom에서도.
        //Debug.Log("connectionToClient.connectionId : " + connectionToClient.connectionId);
        Debug.Log("GameManager.Instance.LocalUserData : " + GameManager.Instance.LocalUserData.ToString());

        //CmdAddDataMyNetworkRoomManagerOnServerside(connectionToClient.connectionId, GameManager.Instance.LocalUserData);
        CmdAddDataMyNetworkRoomManagerOnServerside(GameManager.Instance.LocalUserData.Id, GameManager.Instance.LocalUserData.Pwd, GameManager.Instance.LocalUserData.Nick,
            GameManager.Instance.LocalUserData.Lvl, GameManager.Instance.LocalUserData.Exp, GameManager.Instance.LocalUserData.Win, GameManager.Instance.LocalUserData.Lose, GameManager.Instance.LocalUserData.Draw);
    }

    private bool isReady = false;

    public void Ready()
    {
        if (!isLocalPlayer) return;

        isReady = !isReady;

        CmdChangeReadyState(isReady);
    }

    public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
    {
        base.ReadyStateChanged(oldReadyState, newReadyState);

        Debug.Log("STATE CHANGED TO : ready ==" + newReadyState);
    }

    public override void OnClientEnterRoom()
    {
        base.OnClientEnterRoom();

        return;

        //Debug.Log("connectionToClient.connectionId : " + connectionToClient.connectionId);
        Debug.Log("GameManager.Instance.LocalUserData : " + GameManager.Instance.LocalUserData.ToString());

        //CmdAddDataMyNetworkRoomManagerOnServerside(connectionToClient.connectionId, GameManager.Instance.LocalUserData);
        //CmdAddDataMyNetworkRoomManagerOnServerside(GameManager.Instance.LocalUserData.Id, GameManager.Instance.LocalUserData);
    }

    [Command]
    private void CmdAddDataMyNetworkRoomManagerOnServerside(string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw) //  UserDataModel_KYS data 넘기면 안 나옴
    {
        Debug.Log($"AddDataMyNetworkRoomManagerOnServerside {id} {pwd} {nick} {lvl} {exp} {win} {lose} {draw}");

        MyNetworkRoomManager.singleton.AddData(id, id, pwd, nick, lvl, exp, win, lose, draw);
        //MyNetworkRoomManager.singleton.AddData(id, data);
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            //GameObject readyButton = Instantiate(_readyButtonPrefab);
            //readyButton.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform);
            //readyButton.GetComponent<Button>().onClick.AddListener(delegate { Ready(); });

            CmdSpawnProfile(GameManager.Instance.LocalUserData.Nick, GameManager.Instance.LocalUserData.Lvl);
        }
    }

    private static string s_hostNick;
    private static int s_hostLvl;
    private static Vector3 s_hostPos;

    [Command]
    private void CmdSpawnProfile(string nick, int lvl)
    {
        bool anotherPlayerExists = FindObjectsOfType<PlayerProfile>().Length > 0;

        Vector3 spawnPosition = anotherPlayerExists ? new Vector3(150, -350, 0) : new Vector3(-950, -350, 0);

        GameObject profile = Instantiate(_profilePrefab, Vector3.zero, Quaternion.identity);
        profile.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform); // Main Canvas
        profile.transform.localPosition = spawnPosition;
        profile.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        profile.GetComponent<PlayerProfile>().Init(nick, lvl);

        Debug.Log(profile.GetComponent<PlayerProfile>().IsInitialized);

        NetworkServer.Spawn(profile);

        RpcSpawnProfile(profile, spawnPosition, nick, lvl);

        if (anotherPlayerExists)
        {
            TargetSpawnHostProfile(s_hostPos, s_hostNick, s_hostLvl);
        }
        else
        {
            s_hostNick = nick;
            s_hostLvl = lvl;
            s_hostPos = profile.transform.localPosition;
        }
    }

    [ClientRpc]
    private void RpcSpawnProfile(GameObject profile, Vector3 position, string nick, int lvl)
    {
        profile.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform);
        profile.transform.localPosition = position;
        profile.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        profile.GetComponent<PlayerProfile>().Init(nick, lvl);
    }

    [TargetRpc]
    private void TargetSpawnHostProfile(Vector3 position, string nick, int lvl)
    {
        GameObject profile = Instantiate(_profilePrefab, Vector3.zero, Quaternion.identity);
        profile.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform);
        profile.transform.localPosition = position;
        profile.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        profile.GetComponent<PlayerProfile>().Init(nick, lvl);
    }
    public void  Get_rockmanager(RockManager_YG manager)
    {
       rockmanager = manager;
    }
}
