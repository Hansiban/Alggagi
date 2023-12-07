using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager_KYS : NetworkBehaviour
{
    private TMP_InputField _gameRoomNameInputField;

    private Transform _gameRoomContainer;

    [SerializeField] private GameObject _gameRoomPrefab;

    private List<GameObject> _gameRoomPrefabs;

    private const string DEFAULT_NAME = "같이 게임해요~";


    private void Awake()
    {
        _gameRoomPrefabs = new List<GameObject>();

        _gameRoomNameInputField = GetComponent<TMP_InputField>();
        // test
        _gameRoomContainer = GameObject.FindGameObjectWithTag("Container").transform;
    }


    [Command]
    public void Btn_CreateGameRoom()
    {
        Debug.Log("Btn_CreateGameRoom");

        string name = string.IsNullOrWhiteSpace(_gameRoomNameInputField.text) ? DEFAULT_NAME : _gameRoomNameInputField.text;

        GameObject newRoom = Instantiate(_gameRoomPrefab);

        newRoom.GetComponent<GameRoomButton_KYS>()
            .Init(name, DbAccessManager_KYS.Instance.UserData.Level.ToString());

        newRoom.transform.SetParent(_gameRoomContainer);

        _gameRoomPrefabs.Add(newRoom);

    }

    [ClientRpc]
    public void Test()
    {
        Debug.Log("Test");
    }
}
