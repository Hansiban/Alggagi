using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 대기실
public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    // 필요할지도 안 필요할지도
    [SerializeField] private GameObject _profilePrefab;

    public UserDataModel_KYS LocalUserData { get; private set; }

    private void Awake()
    {
        LocalUserData = GameManager.Instance.LocalUserData;
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        base.IndexChanged(oldIndex, newIndex);
    }

    internal void FillInMyInfo()
    {
        //CmdFillInPlayerProfiles(gameObject);
    }

    public override void OnStartClient()
    {
        Debug.Log("OnStartLocalPlayer");
        base.OnStartLocalPlayer();

        Debug.Log("is LOCAL PLAYER " + isLocalPlayer);
        CmdSpawnProfile("asdf", 2);
        //if (isLocalPlayer)
            //CmdSpawnProfile(connectionToClient, LocalUserData.Nick, LocalUserData.Lvl);
            //CmdFillInPlayerProfile(LocalUserData.Nick, LocalUserData.Lvl);
    }

    // 만약 이미 씬에 PlayerProfile이 없으면 a 포지션에, 있으면 b 포지션에 스폰하고 각자 identity로 아소리티 줌
    [Command]
    private void CmdSpawnProfile(string nick, int lvl)
    //private void CmdSpawnProfile(NetworkConnectionToClient conn, string nick, int lvl)
    {
        //RpcSpawnProfile(nick, lvl);
        //return;
        Debug.Log("CmdSpawnProfile");

        bool anotherPlayerExists = FindObjectsOfType<PlayerProfile>().Length <= 0;

        Vector3 spawnPosition = anotherPlayerExists ? new Vector3(134, -329, 0) : new Vector3(-961, -329, 0);

        GameObject profile = Instantiate(_profilePrefab, spawnPosition, Quaternion.identity);

        profile.GetComponent<PlayerProfile>().Init(nick, lvl);

        NetworkServer.Spawn(profile);

        if (anotherPlayerExists)
            FindObjectsOfType<PlayerProfile>()[0].Init(nick, lvl);

        //NetworkServer.Spawn(profile, conn);
    }

    [ClientRpc]
    private void RpcSpawnProfile(string nick, int lvl)
    {
        // without active server error 생김
        //bool anotherPlayerExists = FindObjectsOfType<PlayerProfile>().Length <= 0;

        //Vector3 spawnPosition = anotherPlayerExists ? new Vector3(134, -329, 0) : new Vector3(-961, -329, 0);

        //GameObject profile = Instantiate(_profilePrefab, spawnPosition, Quaternion.identity);

        //profile.GetComponent<PlayerProfile>().Init(nick, lvl);

        //NetworkServer.Spawn(profile);

        //if (anotherPlayerExists)
        //    FindObjectsOfType<PlayerProfile>()[0].Init(nick, lvl);
    }

    [Command]
    private void CmdFillInPlayerProfile(string nick, int level)
    {

        RpcFillPlayerProfile(0, nick, level);

        return;

        Debug.Log(SceneManager.GetActiveScene().name + " 씬이 현재 활성화 상태입니다.");

        PlayerProfile[] playerProfiles = GameObject.FindObjectsOfType<PlayerProfile>();

        Debug.Log(playerProfiles.Length + " 개의 PlayerProfiles들이 감지되었습니다.");

        if (!playerProfiles[0].IsInitialized)
        {
            var a = GameObject.FindObjectsOfType<PlayerProfile>()[0];
            a.Init(nick, level);

            //a.GetComponent<NetworkIdentity>().AssignClientAuthority(identity);

            RpcFillPlayerProfile(0, nick, level);
            //RpcFillPlayerProfile(0, roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Nick, roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Lvl);
            //RpcFillPlayerProfile(playerProfiles[0].gameObject, roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Nick , roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Lvl);
            //RpcFillPlayerProfile(playerProfiles[0], roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData);
        }
        else
        {
            var a = GameObject.FindObjectsOfType<PlayerProfile>()[1];
            a.Init(nick, level);

            //a.GetComponent<NetworkIdentity>().AssignClientAuthority(identity);

            RpcFillPlayerProfile(0, nick, level);
            //RpcFillPlayerProfile(1, roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Nick, roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Lvl);
            //RpcFillPlayerProfile(playerProfiles[1].gameObject, roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Nick, roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.Lvl);
            //RpcFillPlayerProfile(playerProfiles[1], roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData);
        }

    }

    // network identiry를 가진 놈이라 동기화 가능한 것인가
    [ClientRpc]
    //private void RpcFillPlayerProfile(PlayerProfile profile, UserDataModel_KYS userData)
    //private void RpcFillPlayerProfile(GameObject profile, string nick, int level)
    //private void RpcFillPlayerProfile(int profileIndex, string nick, int level)
    private void RpcFillPlayerProfile(int profileIndex, string nick, int level)
    {
        // 될 것인가 이게. 내 물체가 아닌데도.
        GameObject.FindObjectsOfType<PlayerProfile>()[profileIndex].Init(nick, level) ;
    }
}
