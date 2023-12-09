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

    private void Awake()
    {
        // 로그인 server에 클라이언트로서 연결
        //안 돌아감 다 isOwned가 폴스임
        //if (!isOwned)
        //{
        //    Debug.Log("나는 지금 생성된 Main Menu를 가지고 있지 않습니다 " + connectionToClient);
        //    Debug.Log("isOwned"+isOwned);
        //    Debug.Log("isLocal"+isLocalPlayer);
        //    this.gameObject.SetActive(false);
        //}
        //else
        //{
        //    Debug.Log("내 겁니다");
        //}

        if (isClient)
        {
            Debug.Log("나는 클라이언트라 CmdCheckIfMine을 부를 것입니다");
            CmdCheckIfMine(connectionToClient, this.netIdentity);
        }
        else if (isServer)
        {
            Debug.Log("나는 서버입니다");
        }
        else if(isLocalPlayer)
        {
            Debug.Log("나는 로컬 플레이어입니다");
        }
        else
        {
            Debug.Log("나는 뭘까요?");
        }

        Debug.Log("asdasdasd " + (GetComponent<NetworkIdentity>().connectionToClient == connectionToClient));
        Debug.Log("!!!     isOwned" + isOwned);

        ShowLoginPanel();
    }

    private void Start()
    {
        Debug.Log("???     isOwned" + isOwned);
        if(!isOwned)
        {
            Debug.Log("내 거 아닙니다. disable 할게요");
            // destroy도 생각해보자
            gameObject.SetActive(false);
        }
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

    [TargetRpc]
    private void TargetTurnOffIfNotMine()
    {

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

    public IEnumerator LoadScene1SecLaterTest(string sceneName)
    {
        float timePassed = 0;
        Debug.Log("LoadScene1SecLaterTest 1");
        while (true)
        {
            timePassed += Time.deltaTime;
            yield return new WaitForSeconds(2);

            Debug.Log(timePassed + "passed");

            if (timePassed > 3)
            {
                Debug.Log("LoadScene1SecLaterTest 2");
                SceneManager.LoadScene(sceneName);
                yield break;
            }
        }

    }

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

    public void ShowLoginPanel()
    {
        _logInSignUpPanel.SetActive(true);
        _userProfilePanel.SetActive(false);
        _logoutButton.SetActive(false);
    }

    private bool _canStart = false;

    internal void EnableStartButton() => _canStart = true;
    internal void DisableStartButton() => _canStart = false;

    //[TargetRpc]
    //public void TargetShowProfile()
    //{
    //    _logInSignUpPanel.SetActive(false);
    //    _logoutButton.SetActive(true);

    //    _userProfilePanel.GetComponent<PlayerProfile>().Init(GameManager.Instance.LocalUserData);

    //    _userProfilePanel.SetActive(true);
    //}

    public void ShowProfile()
    {
        _logInSignUpPanel.SetActive(false);
        _logoutButton.SetActive(true);

        _userProfilePanel.GetComponent<PlayerProfile>().Init(GameManager.Instance.LocalUserData);

        _userProfilePanel.SetActive(true);
    }
}
