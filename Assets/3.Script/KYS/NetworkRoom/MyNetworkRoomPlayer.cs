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
        UserData = DbAccessManager_KYS.Instance.UserData;
    }

    public override void IndexChanged(int oldIndex, int newIndex)
    {
        base.IndexChanged(oldIndex, newIndex);
    }
}
