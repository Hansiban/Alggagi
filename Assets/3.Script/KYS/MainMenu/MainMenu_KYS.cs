using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu_KYS : NetworkBehaviour // temp
{
    [SerializeField] private GameObject _logInSignUpPanel;
    [SerializeField] private GameObject _userProfilePanel;
    [SerializeField] private GameObject _logoutButton;

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
        // check if already full
        SceneManager.LoadScene(sceneName);
    }

    public void ShowLoginPanel()
    {
        _logInSignUpPanel.SetActive(true);
        _userProfilePanel.SetActive(false);
        _logoutButton.SetActive(false);
    }

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
