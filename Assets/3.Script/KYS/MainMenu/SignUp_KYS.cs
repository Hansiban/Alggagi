using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

class SignUp_KYS : NetworkBehaviour
{
    [SerializeField] private GameObject _signUpModal;

    [SerializeField] private GameObject _nickNameInvalidationText;
    [SerializeField] private GameObject _idInvalidationText;

    [SerializeField] private TMP_InputField _nicknameInputField;
    [SerializeField] private TMP_InputField _idInputField;
    [SerializeField] private TMP_InputField _pwdInputField;

    private void Awake()
    {
        _signUpModal.SetActive(false);

        _nickNameInvalidationText.SetActive(false);
        _idInvalidationText.SetActive(false);
    }

    // 회원가입 버튼
    public void Btn_OpenSignUpModal()
    {
        // 모든 인풋필드를 빈 값으로 초기화
        _nicknameInputField.text = string.Empty;
        _idInputField.text = string.Empty;
        _pwdInputField.text = string.Empty;

        _signUpModal.SetActive(true);
    }

    // 회원가입 창 닫기 버튼
    public void Btn_CloseSignUpModal() => CloseSignUpModal();
    private void CloseSignUpModal() => _signUpModal.SetActive(false);

    private bool _isIdVerified = false;
    protected bool IsIdVerified
    {
        get => _isIdVerified;
        private set
        {
            _isIdVerified = value;
        }
    }
    private bool _isNicknameVerified = false;
    protected bool IsNicknameVerified
    {
        get => _isNicknameVerified;
        private set
        {
            _isNicknameVerified = value;

            // id 쿼리 값과 nickname 쿼리 값이 순차적으로 들어올 거라는 것을 기반으로 한 코드.
            // 아닐 경우 바꿔야 함
            if(_isNicknameVerified && IsIdVerified)
            {
                Debug.Log("SignUpVerified");

                SignUp(_idInputField.text, _pwdInputField.text, _nicknameInputField.text);
                CloseSignUpModal();
            }
        }
    }

    private void SignUp(string id, string pwd, string nickName)
    {
        string cmdTxt = $"INSERT INTO user VALUES (\"{id}\", \"{pwd}\", \"{nickName}\"," +
                                                    $"1, 1, 1, 1, 1);";

        var res = DbAccessManager_KYS.Instance.Insert(cmdTxt);
    }

    // 회원가입 창의 제출 버튼
    public void Btn_Submit()
    {
        IsIdVerified = false;
        IsNicknameVerified = false;

        CmdCheckIfIdValid(_idInputField.text);
        CmdCheckIfNicknameValid(_nicknameInputField.text);
    }



    // 아이디 중복 검사
    [Command]
    private void CmdCheckIfIdValid(string id)
    {
        string cmdTxt = $"SELECT * FROM user WHERE id = \"{id}\"";

        // if does not exist, valid
        bool isValid = !DbAccessManager_KYS.Instance.Select(cmdTxt);

        TargetCheckIfIdValid(isValid);
    }

    [TargetRpc]
    private void TargetCheckIfIdValid(bool isValid)
    {
        Debug.LogWarning("Id " + isValid);

        _idInvalidationText.SetActive(!isValid);
        IsIdVerified = true;
    }

    // 닉네임 중복 검사
    [Command]
    private void CmdCheckIfNicknameValid(string nickName)
    {
        string cmdTxt = $"SELECT * FROM user WHERE nick = \"{nickName}\"";

        // if does not exist, valid
        bool isValid = !DbAccessManager_KYS.Instance.Select(cmdTxt);

        TargetCheckIfNicknameValid(isValid);
    }

    [TargetRpc]
    private void TargetCheckIfNicknameValid(bool isValid)
    {
        Debug.LogWarning("Nick " + isValid);

        _nickNameInvalidationText.SetActive(!isValid);
        IsNicknameVerified = true;
    }
}
