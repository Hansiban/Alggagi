using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("test player starts");
        MyNetworkRoomManager.singleton.StopClient();
        MyNetworkRoomManager.singleton.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
