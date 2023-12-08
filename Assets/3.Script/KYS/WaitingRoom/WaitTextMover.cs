using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitTextMover : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _waitText;

    private const string WAIT_TEXT_SURFIX = "´ë±â Áß";

    private int _maxDotCnt = 5;

    private void Awake()
    {
        StartCoroutine(nameof(AddDotsRoutine));
    }

    private IEnumerator AddDotsRoutine()
    {
        int _curDotCount = 0;
        string dotsTxt = string.Empty;

        while (true)
        {
            dotsTxt += ".";

            if (_curDotCount >= _maxDotCnt)
            {
                _curDotCount = 0;
                dotsTxt = string.Empty;
            }

            _waitText.text = WAIT_TEXT_SURFIX + dotsTxt;

            _curDotCount++;
            yield return new WaitForSeconds(1);
        }
    }
}
