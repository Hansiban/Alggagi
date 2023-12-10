using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager_YG : NetworkBehaviour
{
    public static TurnManager_YG instance = null;
    public List<RockManager_YG> all_rockmanager = new List<RockManager_YG>();
    public bool haveallrockmanagers = false;
    [SyncVar(hook = nameof(Turn_setting))]
    public int player_turn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    [Server]
    public void Ser_change_turn()
    {
        if (all_rockmanager.Count < 2)
        {
            return;
        }

        Debug.Log("CmdChange_turn");
        player_turn = (player_turn == 2) ? 1 : 2;
        Debug.Log("player_turn:" + player_turn);
    }

    public void Change_turn()
    {
        Debug.Log("Change_turn");
        player_turn = (player_turn == 2) ? 1 : 2;
        Debug.Log("player_turn:" + player_turn);
    }

    private void Turn_setting(int old_, int new_)
    {
        player_turn = new_;
        Debug.Log("all_rockmanager.Count:" + all_rockmanager.Count);
        foreach (var player in all_rockmanager)
        {
            player.Set_myturn();
            Debug.Log("Turn_setting");
        }
    }

    IEnumerator co_setting()
    {
        Debug.Log("co_setting()");
        while (!haveallrockmanagers)
        {
            if (haveallrockmanagers)
            {
                if (player_turn == MyNetworkRoomManager.singleton.roomSlots[0].index)
                {
                    all_rockmanager[0].is_myturn = true;
                    all_rockmanager[1].is_myturn = false;
                }
                else
                {
                    all_rockmanager[0].is_myturn = false;
                    all_rockmanager[1].is_myturn = true;
                }
                yield break;
            }
            yield return null;
        }

    }

    [Command]
    public void Gameover()
    {
        Debug.Log("Gameover");
    }
    
    [Command]
    public void Add_list()
    {
        Debug.Log("Add_list");
        //all_rockmanager = FindObjectsOfType<RockManager_YG>();
        haveallrockmanagers = true;
    }

    [ClientRpc]
    public void RpcAdd_list(RockManager_YG[] managers)
    {
        Debug.Log("RpcAdd_list");
        //all_rockmanager = managers;
        haveallrockmanagers = true;
    }

    //public override void OnStartServer()
    //{
    //    Ser_change_turn();
    //}


}
