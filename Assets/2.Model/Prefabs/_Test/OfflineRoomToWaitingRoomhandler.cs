using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineRoomToWaitingRoomhandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("OfflineRoomToWaitingRoomhandler Start 1");

        StartCoroutine(LoadScene1SecLaterTest("WaitingRoom_KYS"));
    }

    public IEnumerator LoadScene1SecLaterTest(string sceneName)
    {
        float timePassed = 0;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);

        yield break;

        Debug.Log("LoadScene1SecLaterTest 1");
        while (true)
        {
            timePassed += Time.deltaTime;

            Debug.Log(timePassed + "passed");

            if (timePassed > 3)
            {
                Debug.Log("LoadScene1SecLaterTest 2");
                SceneManager.LoadScene(sceneName);
                yield break;
            }
        }

    }
}
