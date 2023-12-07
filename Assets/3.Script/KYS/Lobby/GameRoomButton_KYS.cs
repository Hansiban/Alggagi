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
    internal void Init(string name, string level)
    {
        IsOpen = true;

        _name.text = name;
        _level.text = level;
    }

    // []
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