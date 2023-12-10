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
        Debug.Log($"id input {_idInputField.text}\npwd input {_pwdInputField.text}");

        CmdValidateLogin(_idInputField.text, _pwdInputField.text);
    }

    [Command(requiresAuthority =false)]
    private void CmdValidateLogin(string id, string pwd)
    {
        string cmdTxt = $"SELECT * FROM user WHERE id = \"{id}\" && pwd = \"{pwd}\"";
        UserDataModel_KYS userData = DbAccessManager_KYS.Instance.Select<UserDataModel_KYS>(cmdTxt);

        if (userData != default(UserDataModel_KYS))
        {
            UserDataModel_KYS copy = UserDataModel_KYS.GetCopy(userData);

            Debug.Log("SERVER : Logged In User Data :\n" + copy.ToString());
            TargetSaveLocalUserData(userData.Id, userData.Pwd, userData.Nick, userData.Lvl, userData.Exp, userData.Win, userData.Lose, userData.Draw);
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
    private void TargetSaveLocalUserData(string id, string pwd, string nick, int lvl, int exp, int win, int lose, int draw)
    {
        UserDataModel_KYS copy = UserDataModel_KYS.GetCopy(id, pwd, nick, lvl, exp, win, lose, draw);

        Debug.Log($"TargetSaveLocalUserData");

        // 로그인 한 유저가 본인 데이터 저장
        // 일단은 id와 레벨만
        GameManager.Instance.InsertLocalUserData(copy);

        Debug.Log($"<color = red>CLIENT : Logged In User Data : {copy.ToString()}");

        // 프로필 불러오기
        MainMenu_KYS mainMenu = gameObject.GetComponent<MainMenu_KYS>();
        mainMenu.ShowProfile();

        // 로그인 버튼 활성화
        mainMenu.EnableStartButton();
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
