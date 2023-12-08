using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
            Debug.Log("SERVER ACCESSED TEST PLAYER AWAKE");
        // NetworkManager.singleton.StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
