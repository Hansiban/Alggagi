using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : NetworkBehaviour // temp
{
    [SerializeField] private GameObject _logInSignUpPanel;
    [SerializeField] private GameObject _userProfilePanel;
    [SerializeField] private GameObject _logoutButton;

    [SerializeField] private GameObject _offlinetoRoomHandler;

    private bool _canStart = false;

    private void Awake()
    {
        if (isClient)
        {
            Debug.Log("나는 클라이언트라 CmdCheckIfMine을 부를 것입니다");
            CmdCheckIfMine(connectionToClient, this.netIdentity);
        }
        //else if (isServer)
        //{
        //    Debug.Log("나는 서버입니다");
        //}
        //else if(isLocalPlayer)
        //{
        //    Debug.Log("나는 로컬 플레이어입니다");
        //}
        //else
        //{
        //    Debug.Log("나는 뭘까요?");
        //}

        // Debug.Log("!!!     isOwned" + isOwned); - 무조건 false

    }

    private void Start()
    {
        if(!isOwned)
        {
            Debug.Log("내 거 아닙니다. disable 할게요");
            // destroy도 생각해보자
            gameObject.SetActive(false);
        }

        ShowLoginPanel();
    }

    [Command]
    private void CmdCheckIfMine(NetworkConnectionToClient conn, NetworkIdentity identity)
    {
        var dd = NetworkServer.spawned[(uint)conn.connectionId];

        var asset = identity.assetId;

        Debug.Log("이거 주인 " + asset + " : " + connectionToClient.connectionId);
        Debug.Log(dd == null);

        Debug.Log("isLocal" + isLocalPlayer);
        this.gameObject.SetActive(false);
    }

    // 게임 시작 버튼
    public void Btn_StartGame(string sceneName)
    {
        if (_canStart)  // && room is not full
        {
            Debug.Log("start!");
            

            // works
            CmdDisconnectOnEnteringWaitingRoom(connectionToClient);
            //Debug.Log("Manual Disconnection ready");
            //connectionToClient.Disconnect();
            //Debug.Log("Manual Disconnection done");

            Instantiate(_offlinetoRoomHandler).GetComponent<OfflineRoomToWaitingRoomhandler>();
            // null ref
            //GetComponent<NetworkLoginManager_KYS>().gameObject.SetActive(false);

            //StartCoroutine(LoadScene1SecLaterTest(sceneName));

        }
        else
        {
            Debug.Log("nononononon");
        }
    }

    public void ShowLoginPanel()
    {
        _logInSignUpPanel.SetActive(true);
        _userProfilePanel.SetActive(false);
        _logoutButton.SetActive(false);
    }


    public void ShowProfile()
    {
        _logInSignUpPanel.SetActive(false);
        _logoutButton.SetActive(true);

        _userProfilePanel.GetComponent<PlayerProfile>().Init(GameManager.Instance.LocalUserData.Nick, GameManager.Instance.LocalUserData.Lvl);

        _userProfilePanel.SetActive(true);
    }


    internal void EnableStartButton() => _canStart = true;
    internal void DisableStartButton() => _canStart = false;

    #region 이하 발악의 흔적. 안 씀
    //[Command]
    //private void CmdDisconnectOnEnteringWaitingRoom()
    //{
    //    connectionToClient.Disconnect();
    //}

    [Command]
    private void CmdDisconnectOnEnteringWaitingRoom(NetworkConnectionToClient identity)
    {
        identity.Disconnect();
    }

    [TargetRpc]
    private void TargetDisconnectClient()
    {
        Debug.Log("DISCONNECTED");
        connectionToClient.Disconnect();
    }
    #endregion
}
