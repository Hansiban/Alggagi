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

        MyNetworkRoomManager.singleton.StartClient();

        Debug.Log("networkAddress before" + MyNetworkRoomManager.singleton.networkAddress);
        Debug.Log("transport before" + MyNetworkRoomManager.singleton.transport);
        MyNetworkRoomManager.singleton.networkAddress = "172.30.1.32";
        //MyNetworkRoomManager.singleton.transport = new KcpTransport() { port = 7777 };

        // 172.30.1.32에 연결해서 Client로 들어가기
    }
}
