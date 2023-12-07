using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager_KYS : MonoBehaviour
{
    [SerializeField] private TMP_InputField _gameRoomNameInputField;

    private List<GameRoom_KYS> _gameRooms;

    private void Awake()
    {
        _gameRooms = new List<GameRoom_KYS>();
    }


    public void Btn_CreateGameRoom()
    {
        // _gameRoomNameInputField.text;
        // _gameRooms.Add();
    }

    private void AddGameRoom()
    {

    }
}
