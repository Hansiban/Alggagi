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

    private const string DEFAULT_NAME = "���� �����ؿ�~";


    private void Awake()
    {
        if (isServer) return;

        _gameRoomPrefabs = new List<GameObject>();

        //_gameRoomPrefabs = new List<GameObject>();

        //_gameRoomNameInputField = GetComponent<TMP_InputField>();
        //// test
        //_gameRoomContainer = GameObject.FindGameObjectWithTag("Container").transform;

        //Debug.Log("Lobby Manager �ʱ�ȭ ��");

        //_gameRoomCreateButton = _gameRoomNameInputField.transform.GetComponentInParent<Button>();

        //Debug.Log("Lobby Manager ��ư���� �ʱ�ȭ �Ϸ� : " + _gameRoomCreateButton != null);

        //netIdentity.AssignClientAuthority(connectionToClient);
        //Debug.Log("Lobby Manager�� ���� quthority���� ���� �Ϸ�");

        //_gameRoomCreateButton.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);

        //Debug.Log("_gameRoomCreateButton ���� quthority���� ���� �Ϸ�");
    }

    public void Btn_CreateGameRoom()
    {
        string roomName = string.IsNullOrWhiteSpace(_gameRoomNameInputField.text) ? DEFAULT_NAME : _gameRoomNameInputField.text;

        CmdCreateGameRoom(GameManager.Instance.LocalUserData, roomName);

        //// add another MNRM
        //MyNetworkManager_KYS.singleton.AddMyNetworkRoomManager(DbAccessManager_KYS.Instance.UserData);

        //// navigate to the room/online scene of the made MNRM 
        //MyNetworkRoomManager mnrm = MyNetworkManager_KYS.singleton.GetMyNetworkRoomManager(DbAccessManager_KYS.Instance.UserData.Id);

        // mnrm.NavigateToScene();

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
