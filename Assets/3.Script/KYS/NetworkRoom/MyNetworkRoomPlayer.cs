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
        CmdFillInPlayerProfiles(gameObject);
    }

    [Command]
    private void CmdFillInPlayerProfiles(GameObject roomPlayer)
    {
        Debug.Log(SceneManager.GetActiveScene().name + " 씬이 현재 활성화 상태입니다.");

        Debug.Log(roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.ToString());

        var playerProfiles = GameObject.FindObjectsOfType<PlayerProfile>();

        Debug.Log(playerProfiles.Length + " 개의 PlayerProfiles들이 감지되었습니다.");

        // rpc로 빼야 할 수도 있음
        // 이 변화가 Client 들 쪽에서도 반영될지는 모르겠음
        if (!playerProfiles[0].IsInitialized)
            playerProfiles[0].Init(roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData);
        else
            playerProfiles[1].Init(roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData);

    }

}
