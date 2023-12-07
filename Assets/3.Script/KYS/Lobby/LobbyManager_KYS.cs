using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager_KYS : MonoBehaviour
{
    [SerializeField] private TMP_InputField _gameRoomNameInputField;

    [SerializeField] private Transform _gameRoomContainer;

    [SerializeField] private GameObject _gameRoomPrefab;

    private List<GameObject> _gameRoomPrefabs;

    private const string DEFAULT_NAME = "같이 게임해요~";


    private void Awake()
    {
        _gameRoomPrefabs = new List<GameObject>();
    }


    // [Command]
    public void Btn_CreateGameRoom()
    {
        string name = string.IsNullOrWhiteSpace(_gameRoomNameInputField.text) ? DEFAULT_NAME : _gameRoomNameInputField.text;

        GameObject newRoom = Instantiate(_gameRoomPrefab);

        newRoom.GetComponent<GameRoomButton_KYS>()
            .Init(name, DbAccessManager_KYS.Instance.UserData.Level.ToString());

        newRoom.transform.SetParent(_gameRoomContainer);

        _gameRoomPrefabs.Add(newRoom);
    }
}
