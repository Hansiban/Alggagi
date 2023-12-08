using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRoomButton_KYS : MonoBehaviour
{
    [SerializeField] private Button _enterButton;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;

    [SerializeField] private GameObject _waitingNotifier;
    [SerializeField] private GameObject _gameRunningNotifier;

    private UserDataModel_KYS _hostData;

    private bool _isOpen;
    public bool IsOpen
    {
        get => _isOpen;
        private set
        {
            _isOpen = value;

            _waitingNotifier.SetActive(_isOpen);
            _gameRunningNotifier.SetActive(!_isOpen);
        }
    }

    // Prefab ���� �� �� �ҷ��־�� ��
    internal void Init(UserDataModel_KYS hostData, string name)
    {
        IsOpen = true;

        _hostData = hostData;
        _name.text = name;
        _level.text = hostData.Lvl.ToString();
    }

    // _enterButton (�̹� ������ ������ ��ư��)
    public void Btn_EnterGameRoom()
    {
        IsOpen = false;
    }

    // public void Btn_EditRoomName(){};

    public void Exit(bool hasHostExited) // ȣ��Ʈ�� ���� �Ÿ� �ش� ���� Level ������ �����ִ� �÷��̾� ������ ����
    {

    }
}
