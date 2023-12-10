using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 대기실에서 생성
public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    [SerializeField] private GameObject _profilePrefab;

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
            GameObject hostProfileGO = profile;
            
            TargetSpawnHostProfile(hostProfileGO, new Vector3(-950, -350, 0), FindObjectsOfType<PlayerProfile>()[0].Nick, FindObjectsOfType<PlayerProfile>()[0].Lvl);
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
    private void TargetSpawnHostProfile(GameObject profile, Vector3 position, string nick, int lvl)
    {
        Debug.Log("TARGET!!" + nick + lvl);
        profile.transform.SetParent(GameObject.FindGameObjectWithTag("Test").transform);
        profile.transform.localPosition = position;
        //profile.GetComponent<PlayerProfile>().Init(nick, lvl);
    }
}
