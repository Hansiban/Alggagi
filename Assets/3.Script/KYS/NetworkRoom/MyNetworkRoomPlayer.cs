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
        UserData = GameManager.Instance.LocalUserData;
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"{oldIndex}->{newIndex}인덱스 바꿈!");
        base.IndexChanged(oldIndex, newIndex);
    }

    public bool Check_myturn()
    {
        Debug.Log("Check_myturn");
        if (TurnManager_YG.instance.player_turn == index)
        {
            return true;
        }
        return false;
    }
}
