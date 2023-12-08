using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogChat_j : MonoBehaviour
{
    [SerializeField]private InputField input;
    Text logtxt;

    [SerializeField] private float upSpd;
    int count;
    Vector2 pos;
    Transform trans;
    WaitForSeconds wait;

    private void Start()
    {
        logtxt = GetComponent<Text>();
        wait = new WaitForSeconds(0.01f);
        trans = transform;
        pos = trans.position;
    }
    IEnumerator LogTextUp()
    {
        count = 0;
        trans.Translate(Vector2.down * upSpd * 10);
        while (count <10)
        {
            trans.Translate(Vector2.down * upSpd);
            yield return wait;
            count++;
        }
    }
    public void EnterTxt()
    {
        logtxt.text += string.Format("\n{0}", input.text);
        StartCoroutine(LogTextUp());
    }

}
