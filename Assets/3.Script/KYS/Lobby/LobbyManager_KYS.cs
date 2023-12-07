using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager_KYS : NetworkBehaviour
{
    [SerializeField] private TMP_InputField _gameRoomNameInputField;

    [SerializeField] private Button _gameRoomCreateButton;

    [SerializeField]  private Transform _gameRoomContainer;

    [SerializeField] private GameObject _gameRoomPrefab;

    private List<GameObject> _gameRoomPrefabs;

    private const string DEFAULT_NAME = "같이 게임해요~";


    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        CmdHandOverAutority();
    }

    [Command]
    internal void CmdHandOverAutority()
    {
        netIdentity.AssignClientAuthority(connectionToClient);
        Debug.Log("Lobby Manager에 대한 quthorityㄷ고 세팅 완료");

        _gameRoomCreateButton.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        Debug.Log("_gameRoomCreateButton 대한 quthorityㄷ고 세팅 완료");
    }


    private void Awake()
    {
        if (isServer) return;

        //_gameRoomPrefabs = new List<GameObject>();

        //_gameRoomNameInputField = GetComponent<TMP_InputField>();
        //// test
        //_gameRoomContainer = GameObject.FindGameObjectWithTag("Container").transform;

        //Debug.Log("Lobby Manager 초기화 중");

        //_gameRoomCreateButton = _gameRoomNameInputField.transform.GetComponentInParent<Button>();

        //Debug.Log("Lobby Manager 버튼까지 초기화 완료 : " + _gameRoomCreateButton != null);

        //netIdentity.AssignClientAuthority(connectionToClient);
        //Debug.Log("Lobby Manager에 대한 quthorityㄷ고 세팅 완료");

        //_gameRoomCreateButton.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

        //Debug.Log("_gameRoomCreateButton 대한 quthorityㄷ고 세팅 완료");
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
