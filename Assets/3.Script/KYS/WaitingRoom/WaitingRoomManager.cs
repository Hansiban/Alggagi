using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomManager : MonoBehaviour
{
    private MyNetworkRoomManager _myNetworkRoomManager;

    private void Awake()
    {
        _myNetworkRoomManager = GetComponent<MyNetworkRoomManager>();
    }


}
