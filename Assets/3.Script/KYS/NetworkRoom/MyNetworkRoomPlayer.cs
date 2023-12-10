using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private GameObject _profilePrefab;
    public RockManager_YG rockmanager;

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        base.IndexChanged(oldIndex, newIndex);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(isLocalPlayer)
            CmdSpawnProfile(GameManager.Instance.LocalUserData.Nick, GameManager.Instance.LocalUserData.Lvl);
    }

    private static string s_hostNick;
    private static int s_hostLvl;
    private static Vector3 s_hostPos;

    [Command]
    private void CmdSpawnProfile(string nick, int lvl)
    {
        bool anotherPlayerExists = FindObjectsOfType<PlayerProfile>().Length > 0;

        Vector3 spawnPosition = anotherPlayerExists ? new Vector3(150, -350, 0) : new Vector3(-950, -350, 0);

        GameObject profile = Instantiate(_profilePrefab);
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
        profile.GetComponent<PlayerProfile>().Init(nick, lvl);
    }
    [TargetRpc]
    private void TargetSpawnHostProfile(Vector3 position, string nick, int lvl)
    {
        GameObject profile = Instantiate(_profilePrefab);
        profile.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform);
        profile.transform.localPosition = position;
        profile.GetComponent<PlayerProfile>().Init(nick, lvl);
    }
}
