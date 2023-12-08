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

    // Prefab 생성 시 꼭 불러주어야 함
    internal void Init(UserDataModel_KYS hostData, string name)
    {
        IsOpen = true;

        _hostData = hostData;
        _name.text = name;
        _level.text = hostData.Lvl.ToString();
    }

    // _enterButton (이미 프리팹 본인이 버튼임)
    public void Btn_EnterGameRoom()
    {
        IsOpen = false;
    }

    // public void Btn_EditRoomName(){};

    public void Exit(bool hasHostExited) // 호스트가 떠난 거면 해당 방의 Level 정보를 남아있는 플레이어 정보로 변경
    {

    }
}
