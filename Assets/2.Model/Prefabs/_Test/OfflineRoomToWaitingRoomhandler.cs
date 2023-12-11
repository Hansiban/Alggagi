using kcp2k;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineRoomToWaitingRoomhandler : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadScene1SecLaterTest("WaitingRoom_KYS"));
    }

    public IEnumerator LoadScene1SecLaterTest(string sceneName)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}
