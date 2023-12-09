using Mirror;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class Login_KYS : NetworkBehaviour
{
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _pwdInputField;

    [SerializeField] private GameObject _invalidationText;

    private float _invalidationTextTimer = 0;


    private void Awake()
    {
        _invalidationText.SetActive(false);
    }

    // 로그인 버튼
    public void Btn_Login()
    {
        Debug.Log("Clicked Login Button");

        if (isLocalPlayer)
        {
            Debug.Log("isLocalPlayer");
        }
        else
        {
            Debug.Log("NOT LOCAL PLAYER");
        }

        Debug.Log($"id input {_idInputField.text}\npwd input {_pwdInputField.text}");

        CmdValidateLogin(_idInputField.text, _pwdInputField.text);
    }

    [Command(requiresAuthority =false)]
    private void CmdValidateLogin(string id, string pwd)
    {
        Debug.Log($"CmdValidateLogin");

        string cmdTxt = $"SELECT * FROM user WHERE id = \"{id}\" && pwd = \"{pwd}\"";

        UserDataModel_KYS userData = DbAccessManager_KYS.Instance.Select<UserDataModel_KYS>(cmdTxt);

        if (userData != default(UserDataModel_KYS))
        {
            Debug.Log("SERVER : Logged In User Data :\n" + userData.ToString());
            TargetSaveLocalUserData(userData.Nick, userData.Lvl);
        }
        else
            TargetNotifyLoginFailed();

    }

    // 서버에 로그인 요청한 유저에게만 들어오는 메소드
    [TargetRpc]
    private void TargetNotifyLoginFailed()
    {
        StartCoroutine(nameof(NotifyInvalidation));
    }

    //private void TargetSaveLocalUserData(UserDataModel_KYS userData)
    [TargetRpc]
    private void TargetSaveLocalUserData(string nick, int level)
    {
        Debug.Log($"TargetSaveLocalUserData");

        // 로그인 한 유저가 본인 데이터 저장
        // 일단은 id와 레벨만
        GameManager.Instance.InsertLocalUserData(nick, level);

        // 로그인 버튼 활성화
        Debug.Log($"<color = red>CLIENT : Logged In User Data :\nnick : {nick}\tlevel:{level} </color>");
        Debug.Log($"InsertLocalUserData :\nid : {GameManager.Instance.LocalUserData.Nick}\tlevel:{GameManager.Instance.LocalUserData.Lvl}");

        // 프로필 불러오기
        MainMenu_KYS mainMenu = gameObject.GetComponent<MainMenu_KYS>();
        mainMenu.ShowProfile();
    }


    private IEnumerator NotifyInvalidation()
    {
        // NotifyInvalidation already running
        if (_invalidationText.activeSelf)
        {
            _invalidationTextTimer = 0;
            // may add fade effect
            yield break;
        }

        _invalidationText.SetActive(true);

        while (_invalidationTextTimer < 3)
        {
            _invalidationTextTimer += Time.deltaTime;
            yield return null;
        }

        _invalidationTextTimer = 0;
        _invalidationText.SetActive(false);
    }
}
