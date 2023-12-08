using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 대기실
public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    // 필요할지도 안 필요할지도
    [SerializeField] private GameObject _profilePrefab;

    public UserDataModel_KYS UserData { get; private set; }

    private void Awake()
    {
        Debug.Log("MyNetworkRoomPlayer AWAKE");

        UserData = DbAccessManager_KYS.Instance.UserData;
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        base.IndexChanged(oldIndex, newIndex);

        Debug.Log($"INDEX CHANGED from {oldIndex} to {newIndex}");
    }
}
