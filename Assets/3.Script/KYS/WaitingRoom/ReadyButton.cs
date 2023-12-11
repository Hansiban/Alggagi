using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour
{
    [SerializeField] private Sprite _readySprite;
    private const string READY_BUTTON_NAME = "준비!";

    [SerializeField] private Sprite _cancelReadySprite;
    private const string CANCEL_READY_BUTTON_NAME = "준비 취소";

    private void Awake()
    {
        gameObject.GetComponent<Button>().image.sprite = _readySprite;
        gameObject.GetComponent<Button>().image.type = Image.Type.Sliced;
    }

    public void Btn_Ready()
    {
        FindObjectsOfType<MyNetworkRoomPlayer>().Where(x => x.isOwned).FirstOrDefault()?.Ready();

        gameObject.GetComponent<Button>().image.sprite =
            gameObject.GetComponent<Button>().image.sprite == _readySprite ? _cancelReadySprite : _readySprite;
        gameObject.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text =
            gameObject.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text == READY_BUTTON_NAME ? CANCEL_READY_BUTTON_NAME : READY_BUTTON_NAME;
    }


}
