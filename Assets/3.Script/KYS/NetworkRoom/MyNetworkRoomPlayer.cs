using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����
public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    // �ʿ������� �� �ʿ�������
    [SerializeField] private GameObject _profilePrefab;

    public UserDataModel_KYS UserData { get; private set; }

    private void Awake()
    {
        UserData = GameManager.Instance.LocalUserData;
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        Debug.Log($"{oldIndex}->{newIndex}�ε��� �ٲ�!");
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
