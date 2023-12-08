using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class Login_KYS : MonoBehaviour
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
        UserDataModel_KYS userData = ValidateLogin();

        if (userData == null)
            return;

        // 유저 데이터 저장
        DbAccessManager_KYS.Instance.InsertUserData(userData);

        // 프로필 불러오기
        MainMenu_KYS mainMenu = gameObject.GetComponent<MainMenu_KYS>();
        mainMenu.ShowProfile();
    }

    // 로그인이 잘 된다면, 유저 데이터 전체를 string으로 리턴
    private UserDataModel_KYS ValidateLogin()
    {
        string cmdTxt = $"SELECT * FROM user WHERE id = \"{_idInputField.text}\" && pwd = \"{_pwdInputField.text}\"";

        UserDataModel_KYS userData = DbAccessManager_KYS.Instance.Select<UserDataModel_KYS>(cmdTxt);

        if (userData == default(UserDataModel_KYS))
            StartCoroutine(nameof(NotifyInvalidation));

        return userData;
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
