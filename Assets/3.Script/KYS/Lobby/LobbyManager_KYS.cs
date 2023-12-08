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

        //CmdHandOverAutority();
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

        _gameRoomPrefabs = new List<GameObject>();

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

    public void Btn_CreateGameRoom()
    {
        string roomName = string.IsNullOrWhiteSpace(_gameRoomNameInputField.text) ? DEFAULT_NAME : _gameRoomNameInputField.text;

        CmdCreateGameRoom(DbAccessManager_KYS.Instance.UserData, roomName);
    }

    [Command(requiresAuthority = false)]
    public void CmdCreateGameRoom(UserDataModel_KYS hostData, string roomName)
    {
        RpcTest(hostData, roomName);
    }

    [ClientRpc]
    public void RpcTest(UserDataModel_KYS hostData, string roomName)
    {
        GameObject newRoom = Instantiate(_gameRoomPrefab);

        newRoom.GetComponent<GameRoomButton_KYS>()
            .Init(hostData, roomName);

        newRoom.transform.SetParent(_gameRoomContainer);

        Debug.Log("rpc test4");
        _gameRoomPrefabs.Add(newRoom);
    }
}
