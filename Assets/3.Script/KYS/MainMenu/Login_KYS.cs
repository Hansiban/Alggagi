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
        ValidateLogin();
    }

    private void ValidateLogin()
    {
        string cmdTxt = $"SELECT * FROM user WHERE id = \"{_idInputField.text}\" && password = \"{_pwdInputField.text}\"";

        var res = DbAccessManager_KYS.Instance.Select(cmdTxt);

        if (!string.IsNullOrEmpty(res))
            Debug.Log("Login Succeeded with " + res);
        else
            StartCoroutine(nameof(NotifyInvalidation));
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
            Debug.Log("_invalidationTextTimer:  "+ _invalidationTextTimer);
            yield return null;
        }

        _invalidationTextTimer = 0;
        _invalidationText.SetActive(false);
    }
}
