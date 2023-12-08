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


    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        //CmdHandOverAutority();
    }

    [Command]
    internal void CmdHandOverAutority()
    {
        netIdentity.AssignClientAuthority(connectionToClient);
        Debug.Log("Lobby Manager�� ���� quthority���� ���� �Ϸ�");

        _gameRoomCreateButton.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        Debug.Log("_gameRoomCreateButton ���� quthority���� ���� �Ϸ�");
    }


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

    [Command(requiresAuthority = false)]
    public void Btn_CreateGameRoom()
    {
        Debug.Log("Btn_CreateGameRoom");

        //return;
        RpcTest();
    }

    [ClientRpc]
    public void RpcTest()
    {
        Debug.Log("rpc test ����");
        string name = string.IsNullOrWhiteSpace(_gameRoomNameInputField.text) ? DEFAULT_NAME : _gameRoomNameInputField.text;

        Debug.Log("rpc test1");
        GameObject newRoom = Instantiate(_gameRoomPrefab);

        Debug.Log("rpc test2");
        newRoom.GetComponent<GameRoomButton_KYS>()
            .Init(name, DbAccessManager_KYS.Instance.UserData.Level.ToString());

        Debug.Log("rpc test3");
        newRoom.transform.SetParent(_gameRoomContainer);

        Debug.Log("rpc test4");
        _gameRoomPrefabs.Add(newRoom);
    }
}
