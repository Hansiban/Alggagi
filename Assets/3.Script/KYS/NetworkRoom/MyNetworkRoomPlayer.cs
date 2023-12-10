using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ����
public class MyNetworkRoomPlayer : NetworkRoomPlayer
{
    // �ʿ������� �� �ʿ�������
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
        Debug.Log(SceneManager.GetActiveScene().name + " ���� ���� Ȱ��ȭ �����Դϴ�.");

        Debug.Log(roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData.ToString());

        var playerProfiles = GameObject.FindObjectsOfType<PlayerProfile>();

        Debug.Log(playerProfiles.Length + " ���� PlayerProfiles���� �����Ǿ����ϴ�.");

        // rpc�� ���� �� ���� ����
        // �� ��ȭ�� Client �� �ʿ����� �ݿ������� �𸣰���
        if (!playerProfiles[0].IsInitialized)
            playerProfiles[0].Init(roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData);
        else
            playerProfiles[1].Init(roomPlayer.GetComponent<MyNetworkRoomPlayer>().LocalUserData);

    }

}
